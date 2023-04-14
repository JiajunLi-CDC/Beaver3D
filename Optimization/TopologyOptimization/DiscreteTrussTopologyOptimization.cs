using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver3D.Optimization.SAND;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.TopologyOptimization
{
	// Token: 0x02000019 RID: 25
	public class DiscreteTrussTopologyOptimization
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006BC8 File Offset: 0x00004DC8
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00006BD0 File Offset: 0x00004DD0
		public Objective Objective { get; private set; } = Objective.MinStructureMass;

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006BD9 File Offset: 0x00004DD9
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00006BE1 File Offset: 0x00004DE1
		public double ObjectiveValue { get; private set; } = double.PositiveInfinity;

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00006BEA File Offset: 0x00004DEA
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00006BF2 File Offset: 0x00004DF2
		public OptimOptions Options { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00006BFB File Offset: 0x00004DFB
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00006C03 File Offset: 0x00004E03
		public bool TimeLimitReached { get; private set; } = false;

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006C0C File Offset: 0x00004E0C
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00006C14 File Offset: 0x00004E14
		public bool Interrupted { get; private set; } = false;

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00006C1D File Offset: 0x00004E1D
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00006C25 File Offset: 0x00004E25
		public string Message { get; private set; } = "";

		// Token: 0x060000EC RID: 236 RVA: 0x00006C30 File Offset: 0x00004E30
		public DiscreteTrussTopologyOptimization(Objective Objective, OptimOptions Options = null)
		{
			this.Objective = Objective;
			bool flag = Options == null;
			if (flag)
			{
				this.Options = new OptimOptions();
			}
			else
			{
				this.Options = Options;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006C9A File Offset: 0x00004E9A
		public void Solve(Structure Structure, Stock Stock)
		{
			this.Solve(Structure, new List<string>
			{
				"all"
			}, Stock);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006CB8 File Offset: 0x00004EB8
		public void Solve(Structure Structure, List<string> LoadCaseNames, Stock Stock)
		{
			List<LoadCase> loadCasesFromNames = Structure.GetLoadCasesFromNames(LoadCaseNames);
			bool flag = loadCasesFromNames == null || loadCasesFromNames.Count == 0;
			if (flag)
			{
				throw new ArgumentException("LoadCases with the provided names are not existing in the structure. Check the names or use 'all' to compute all LoadCases.");
			}
			MILPOptimizer milpoptimizer = this.Options.MILPOptimizer;
			MILPOptimizer milpoptimizer2 = milpoptimizer;
			if (milpoptimizer2 != MILPOptimizer.Gurobi)
			{
				this.SolveGurobiRS(Structure, loadCasesFromNames, Stock);
			}
			else
			{
				switch (this.Options.MILPFormulation)
				{
				case MILPFormulation.Bruetting:
					this.SolveGurobiBR(Structure, loadCasesFromNames, Stock);
					break;
				case MILPFormulation.RasmussenStolpe:
					this.SolveGurobiRS(Structure, loadCasesFromNames, Stock);
					break;
				case MILPFormulation.GhattasGrossmann:
					this.SolveGurobiGG(Structure, loadCasesFromNames, Stock);
					break;
				case MILPFormulation.NP:
					this.SolveGurobiNP(Structure, loadCasesFromNames, Stock);
					break;
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006D6C File Offset: 0x00004F6C
		public void SolveGurobiRS(Structure Structure, List<LoadCase> LoadCases, Stock Stock)
		{
			GRBEnv grbenv = new GRBEnv();
			GRBModel grbmodel = new GRBModel(grbenv);

			grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;
			foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)
			{
				grbmodel.Set(tuple.Item1, tuple.Item2);
			}

			GRBVar[] gurobiAssignmentVariables = SANDGurobiDiscreteRS.GetGurobiAssignmentVariables(grbmodel, Structure, Stock);
			Dictionary<LoadCase, GRBVar[]> gurobiMemberForceVariables = SANDGurobiDiscreteRS.GetGurobiMemberForceVariables(grbmodel, Structure, LoadCases, Stock, this.Options);
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			bool compatibility = this.Options.Compatibility;
			if (compatibility)
			{
				dictionary = SANDGurobiDiscreteRS.GetGurobiDisplacementVariables(grbmodel, Structure, LoadCases);
			}

			//设置目标
			SANDGurobiDiscreteRS.SetObjective(this.Objective, grbmodel, gurobiAssignmentVariables, Structure, Stock);
			//设置分配约束，对应论文1-2
			SANDGurobiDiscreteRS.AddAssignment(grbmodel, gurobiAssignmentVariables, Structure, Stock, this.Options);
			//设置力学约束，对应论文3-9
			foreach (LoadCase loadCase in LoadCases)
			{
				SANDGurobiDiscreteRS.AddEquilibrium(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock, this.Options);
				SANDGurobiDiscreteRS.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock, this.Options);
				bool compatibility2 = this.Options.Compatibility;
				if (compatibility2)
				{
					SANDGurobiDiscreteRS.AddCompatibility(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], dictionary[loadCase], Structure, loadCase, Stock, this.Options);
				}
			}
			//设置库存约束
			SANDGurobiReuse.AddGroup(grbmodel, gurobiAssignmentVariables, Structure, Stock);

			//运行信息UI
			FormStartPosition startPos;
			Point location;
			this.CloseOpenFormsAndGetPos(out startPos, out location);
			LightCallback lightCallback = null;
			LogCallback logCallback = null;
			bool logToConsole = this.Options.LogToConsole;             
			if (logToConsole)
			{
				logCallback = new LogCallback(startPos, location, this.Options.LogFormName);     //这里调用了输出面板？
				grbmodel.SetCallback(logCallback);
			}
			else
			{
				lightCallback = new LightCallback();
				grbmodel.SetCallback(lightCallback);
			}

			//开始计算
			try
			{
				grbmodel.Optimize();
				int status = grbmodel.Status;
				bool flag = status == 2 || status == 9 || status == 11;
				if (flag)
				{
					bool flag2 = status == 9;
					if (flag2)
					{
						this.TimeLimitReached = true;
					}
					bool flag3 = status == 11;
					if (flag3)
					{
						this.Interrupted = true;
					}
					try
					{
						this.ObjectiveValue = grbmodel.ObjVal;
						bool logToConsole2 = this.Options.LogToConsole;
						if (logToConsole2)
						{
							this.LowerBounds = logCallback.LowerBounds;
							this.UpperBounds = logCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						else
						{
							this.LowerBounds = lightCallback.LowerBounds;
							this.UpperBounds = lightCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						bool compatibility3 = this.Options.Compatibility;
						if (compatibility3)
						{
							List<double> list = new List<double>();
							foreach (GRBVar grbvar in dictionary[LoadCases[0]])
							{
								list.Add(grbvar.X);
							}
						}
						foreach (IMember member in Structure.Members)
						{
							Bar bar = (Bar)member;
							bar.Nx.Clear();
							Assignment assignment = new Assignment();
							bool flag4 = false;
							for (int j = 0; j < Stock.ElementGroups.Count; j++)
							{
								bool flag5 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + j].X >= 0.999;
								if (flag5)
								{
									flag4 = true;
									bar.CrossSection = Stock.ElementGroups[j].CrossSection;
									bar.Material = Stock.ElementGroups[j].Material;
									assignment.AddElementAssignment(Stock.ElementGroups[j], Stock.ElementGroups[j].Next);
									foreach (LoadCase loadCase2 in LoadCases)
									{
										bool compatibility4 = this.Options.Compatibility;
										if (compatibility4)
										{
											bar.AddNormalForce(loadCase2, new List<double>
											{
												gurobiMemberForceVariables[loadCase2][bar.Number * Stock.ElementGroups.Count + j].X
											});
										}
										else
										{
											bar.AddNormalForce(loadCase2, new List<double>
											{
												gurobiMemberForceVariables[loadCase2][bar.Number].X
											});
										}
									}
								}
							}
							bool flag6 = !flag4;
							if (flag6)
							{
								bar.CrossSection = new EmptySection();
								bar.Material = default(EmptyMaterial);
								foreach (LoadCase lc in LoadCases)
								{
									bar.AddNormalForce(lc, new List<double>
									{
										0.0
									});
								}
							}
							bar.SetAssignment(assignment);
						}
						Structure.SetResults(new Result(Structure, Stock, null));
					}
					catch (GRBException ex)
					{
						this.Message = ex.Message;
					}
				}
				else
				{
					bool flag7 = status == 3;
					if (flag7)
					{
						throw new SystemException("Gurobi problem is infeasible");
					}
				}
				grbmodel.Dispose();
				grbenv.Dispose();
			}
			catch (GRBException ex2)
			{
				throw new GurobiException(ex2.Message);
			}
			grbmodel.Dispose();
			grbenv.Dispose();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007428 File Offset: 0x00005628
		public void SolveGurobiGG(Structure Structure, List<LoadCase> LoadCases, Stock Stock)
		{
			GRBEnv grbenv = new GRBEnv();
			GRBModel grbmodel = new GRBModel(grbenv);
			grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;
			foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)
			{
				grbmodel.Set(tuple.Item1, tuple.Item2);
			}
			bool flag = !Structure.AllTopologyFixed();
			if (flag)
			{
				Stock.InsertElementGroup(0, ElementGroup.ZeroElement());
			}
			GRBVar[] gurobiAssignmentVariables = SANDGurobiDiscreteGG.GetGurobiAssignmentVariables(grbmodel, Structure, Stock);
			Dictionary<LoadCase, GRBVar[]> gurobiMemberForceVariables = SANDGurobiDiscreteGG.GetGurobiMemberForceVariables(grbmodel, Structure, LoadCases);
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			Dictionary<LoadCase, GRBVar[]> dictionary2 = new Dictionary<LoadCase, GRBVar[]>();
			Dictionary<LoadCase, GRBVar[]> dictionary3 = new Dictionary<LoadCase, GRBVar[]>();
			bool compatibility = this.Options.Compatibility;
			if (compatibility)
			{
				dictionary = SANDGurobiDiscreteGG.GetGurobiMemberStressVariables(grbmodel, Structure, LoadCases);
				dictionary2 = SANDGurobiDiscreteGG.GetGurobiDisplacementVariables(grbmodel, Structure, LoadCases);
				dictionary3 = SANDGurobiDiscreteGG.GetGurobiMemberElongationVariables(grbmodel, Structure, LoadCases, Stock);
			}
			SANDGurobiDiscreteGG.SetObjective(this.Objective, grbmodel, gurobiAssignmentVariables, Structure, Stock);
			SANDGurobiDiscreteGG.AddAssignment(grbmodel, gurobiAssignmentVariables, Structure, Stock, this.Options);
			foreach (LoadCase loadCase in LoadCases)
			{
				SANDGurobiDiscreteGG.AddEquilibrium(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock, this.Options);
				bool compatibility2 = this.Options.Compatibility;
				if (compatibility2)
				{
					SANDGurobiDiscreteGG.AddCompatibility(grbmodel, dictionary3[loadCase], dictionary2[loadCase], Structure, loadCase, Stock);
					SANDGurobiDiscreteGG.AddConstitutive(grbmodel, gurobiMemberForceVariables[loadCase], dictionary3[loadCase], Structure, loadCase, Stock);
					SANDGurobiDiscreteGG.AddHooke(grbmodel, dictionary[loadCase], dictionary3[loadCase], Structure, loadCase, Stock);
					SANDGurobiDiscreteGG.AddBigM(grbmodel, gurobiAssignmentVariables, dictionary3[loadCase], Structure, loadCase, Stock, this.Options);
				}
				else
				{
					SANDGurobiDiscreteGG.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock);
				}
			}
			SANDGurobiReuse.AddGroup(grbmodel, gurobiAssignmentVariables, Structure, Stock);
			FormStartPosition startPos;
			Point location;
			this.CloseOpenFormsAndGetPos(out startPos, out location);
			LightCallback lightCallback = null;
			LogCallback logCallback = null;
			bool logToConsole = this.Options.LogToConsole;
			if (logToConsole)
			{
				logCallback = new LogCallback(startPos, location, this.Options.LogFormName);
				grbmodel.SetCallback(logCallback);
			}
			else
			{
				lightCallback = new LightCallback();
				grbmodel.SetCallback(lightCallback);
			}

			try
			{
				grbmodel.Optimize();
				int status = grbmodel.Status;
				bool flag2 = status == 2 || status == 9 || status == 11;
				if (flag2)
				{
					bool flag3 = status == 9;
					if (flag3)
					{
						this.TimeLimitReached = true;
					}
					bool flag4 = status == 11;
					if (flag4)
					{
						this.Interrupted = true;
					}
					try
					{
						this.ObjectiveValue = grbmodel.ObjVal;
						bool logToConsole2 = this.Options.LogToConsole;
						if (logToConsole2)
						{
							this.LowerBounds = logCallback.LowerBounds;
							this.UpperBounds = logCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						else
						{
							this.LowerBounds = lightCallback.LowerBounds;
							this.UpperBounds = lightCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						foreach (IMember member in Structure.Members)
						{
							Bar bar = (Bar)member;
							Assignment assignment = new Assignment();
							bar.Nx.Clear();
							bool flag5 = false;
							for (int i = 0; i < Stock.ElementGroups.Count; i++)
							{
								bool flag6 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + i].X >= 0.999;
								if (flag6)
								{
									flag5 = true;
									bar.CrossSection = Stock.ElementGroups[i].CrossSection;
									bar.Material = Stock.ElementGroups[i].Material;
									assignment.AddElementAssignment(Stock.ElementGroups[i], Stock.ElementGroups[i].Next);
									foreach (LoadCase loadCase2 in LoadCases)
									{
										bar.AddNormalForce(loadCase2, new List<double>
										{
											gurobiMemberForceVariables[loadCase2][bar.Number].X
										});
									}
								}
							}
							bool flag7 = !flag5;
							if (flag7)
							{
								assignment.AddElementAssignment(Stock.ElementGroups[0], 0);
								bar.CrossSection = new EmptySection();
								foreach (LoadCase lc in LoadCases)
								{
									bar.AddNormalForce(lc, new List<double>
									{
										0.0
									});
								}
							}
							bar.SetAssignment(assignment);
						}
						Structure.SetResults(new Result(Structure, Stock, null));
					}
					catch (GRBException ex)
					{
						this.Message = ex.Message;
					}
				}
				else
				{
					bool flag8 = status == 3;
					if (flag8)
					{
						throw new SystemException("Gurobi problem is infeasible");
					}
				}
				grbmodel.Dispose();
				grbenv.Dispose();
			}
			catch (GRBException ex2)
			{
				throw new GurobiException(ex2.Message);
			}
			grbmodel.Dispose();
			grbenv.Dispose();
			bool flag9 = !Structure.AllTopologyFixed();
			if (flag9)
			{
				Stock.ElementGroups.RemoveAt(0);
			}
			Stock.ResetNext();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007AEC File Offset: 0x00005CEC
		public void SolveGurobiBR(Structure Structure, List<LoadCase> LoadCases, Stock Stock)
		{
			GRBEnv grbenv = new GRBEnv();
			GRBModel grbmodel = new GRBModel(grbenv);
			grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;
			foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)
			{
				grbmodel.Set(tuple.Item1, tuple.Item2);
			}
			bool flag = !Structure.AllTopologyFixed();
			if (flag)
			{
				Stock.InsertElementGroup(0, ElementGroup.ZeroElement());
			}
			GRBVar[] gurobiAssignmentVariables = SANDGurobiDiscreteBR.GetGurobiAssignmentVariables(grbmodel, Structure, Stock);
			Dictionary<LoadCase, GRBVar[]> gurobiMemberForceVariables = SANDGurobiDiscreteBR.GetGurobiMemberForceVariables(grbmodel, Structure, LoadCases);
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			Dictionary<LoadCase, GRBVar[]> dictionary2 = new Dictionary<LoadCase, GRBVar[]>();
			bool compatibility = this.Options.Compatibility;
			if (compatibility)
			{
				dictionary = SANDGurobiDiscreteBR.GetGurobiDisplacementVariables(grbmodel, Structure, LoadCases);
				dictionary2 = SANDGurobiDiscreteBR.GetGurobiMemberElongationVariables(grbmodel, Structure, LoadCases, Stock);
			}
			SANDGurobiDiscreteBR.SetObjective(this.Objective, grbmodel, gurobiAssignmentVariables, Structure, Stock);
			SANDGurobiDiscreteBR.AddAssignment(grbmodel, gurobiAssignmentVariables, Structure, Stock, this.Options);
			grbmodel.Update();
			foreach (LoadCase loadCase in LoadCases)
			{
				//平衡约束（4）
				SANDGurobiDiscreteBR.AddEquilibrium(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock, this.Options);
				grbmodel.Update();
				bool compatibility2 = this.Options.Compatibility;
				if (compatibility2)
				{
					//兼容性约束（5）
					SANDGurobiDiscreteBR.AddCompatibility(grbmodel, dictionary2[loadCase], dictionary[loadCase], Structure, loadCase, Stock);
					grbmodel.Update();
					//
					SANDGurobiDiscreteBR.AddConstitutive(grbmodel, gurobiMemberForceVariables[loadCase], dictionary2[loadCase], Structure, loadCase, Stock);
					grbmodel.Update();
					//
					SANDGurobiDiscreteBR.AddBigM(grbmodel, gurobiAssignmentVariables, dictionary2[loadCase], Structure, loadCase, Stock, this.Options);
					grbmodel.Update();
					//抗压能力约束（6）
					SANDGurobiDiscreteBR.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock);
					grbmodel.Update();
				}
				else
				{
					SANDGurobiDiscreteBR.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock);
				}
			}
			SANDGurobiReuse.AddGroup(grbmodel, gurobiAssignmentVariables, Structure, Stock);
			FormStartPosition startPos;
			Point location;
			this.CloseOpenFormsAndGetPos(out startPos, out location);
			LightCallback lightCallback = null;
			LogCallback logCallback = null;
			bool logToConsole = this.Options.LogToConsole;
			if (logToConsole)
			{
				logCallback = new LogCallback(startPos, location, this.Options.LogFormName);
				grbmodel.SetCallback(logCallback);
			}
			else
			{
				lightCallback = new LightCallback();
				grbmodel.SetCallback(lightCallback);
			}
			try
			{
				grbmodel.Optimize();
				int status = grbmodel.Status;
				bool flag2 = status == 2 || status == 9 || status == 11;
				if (flag2)
				{
					bool flag3 = status == 9;
					if (flag3)
					{
						this.TimeLimitReached = true;
					}
					bool flag4 = status == 11;
					if (flag4)
					{
						this.Interrupted = true;
					}
					try
					{
						this.ObjectiveValue = grbmodel.ObjVal;
						bool logToConsole2 = this.Options.LogToConsole;
						if (logToConsole2)
						{
							this.LowerBounds = logCallback.LowerBounds;
							this.UpperBounds = logCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						else
						{
							this.LowerBounds = lightCallback.LowerBounds;
							this.UpperBounds = lightCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						foreach (IMember member in Structure.Members)
						{
							Bar bar = (Bar)member;
							bar.Nx.Clear();
							Assignment assignment = new Assignment();
							bool flag5 = false;
							for (int i = 0; i < Stock.ElementGroups.Count; i++)
							{
								bool flag6 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + i].X >= 0.999;
								if (flag6)
								{
									flag5 = true;
									bar.CrossSection = Stock.ElementGroups[i].CrossSection;
									bar.Material = Stock.ElementGroups[i].Material;
									assignment.AddElementAssignment(Stock.ElementGroups[i], Stock.ElementGroups[i].Next);
									foreach (LoadCase loadCase2 in LoadCases)
									{
										bar.AddNormalForce(loadCase2, new List<double>
										{
											gurobiMemberForceVariables[loadCase2][bar.Number].X
										});
									}
								}
							}
							bool flag7 = !flag5;
							if (flag7)
							{
								assignment.AddElementAssignment(Stock.ElementGroups[0], 0);
								bar.CrossSection = new EmptySection();
								foreach (LoadCase lc in LoadCases)
								{
									bar.AddNormalForce(lc, new List<double>
									{
										0.0
									});
								}
							}
							bar.SetAssignment(assignment);
						}
						Structure.SetResults(new Result(Structure, Stock, null));
					}
					catch (GRBException ex)
					{
						this.Message = ex.Message;
					}
				}
				else
				{
					bool flag8 = status == 3;
					if (flag8)
					{
						throw new SystemException("Gurobi problem is infeasible");
					}
				}
				grbmodel.Dispose();
				grbenv.Dispose();
			}
			catch (GRBException ex2)
			{
				throw new GurobiException(ex2.Message);
			}
			grbmodel.Dispose();
			grbenv.Dispose();
			bool flag9 = !Structure.AllTopologyFixed();
			if (flag9)
			{
				Stock.ElementGroups.RemoveAt(0);
			}
			Stock.ResetNext();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000081C0 File Offset: 0x000063C0
		public void SolveGurobiNP(Structure Structure, List<LoadCase> LoadCases, Stock Stock)
		{
			GRBEnv grbenv = new GRBEnv();
			GRBModel grbmodel = new GRBModel(grbenv);
			grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;
			foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)
			{
				grbmodel.Set(tuple.Item1, tuple.Item2);
			}
			bool flag = !Structure.AllTopologyFixed();
			if (flag)
			{
				Stock.InsertElementGroup(0, ElementGroup.ZeroElement());
			}
			GRBVar[] gurobiAssignmentVariables = SANDGurobiDiscreteNP.GetGurobiAssignmentVariables(grbmodel, Structure, Stock);
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			Dictionary<LoadCase, GRBVar[]> dictionary2 = new Dictionary<LoadCase, GRBVar[]>();
			bool compatibility = this.Options.Compatibility;
			if (compatibility)
			{
				dictionary = SANDGurobiDiscreteNP.GetGurobiDisplacementVariables(grbmodel, Structure, LoadCases);
				dictionary2 = SANDGurobiDiscreteNP.GetGurobiMemberElongationVariables(grbmodel, Structure, LoadCases, Stock);
			}
			SANDGurobiDiscreteNP.SetObjective(this.Objective, grbmodel, gurobiAssignmentVariables, Structure, Stock);
			SANDGurobiDiscreteNP.AddAssignment(grbmodel, gurobiAssignmentVariables, Structure, Stock, this.Options);
			foreach (LoadCase loadCase in LoadCases)
			{
				SANDGurobiDiscreteNP.AddEquilibrium(grbmodel, gurobiAssignmentVariables, dictionary2[loadCase], Structure, loadCase, Stock, this.Options);
				bool compatibility2 = this.Options.Compatibility;
				if (compatibility2)
				{
					SANDGurobiDiscreteNP.AddCompatibility(grbmodel, dictionary2[loadCase], dictionary[loadCase], Structure, loadCase, Stock);
					SANDGurobiDiscreteNP.AddBigM(grbmodel, gurobiAssignmentVariables, dictionary2[loadCase], Structure, loadCase, Stock, this.Options);
				}
				else
				{
					SANDGurobiDiscreteNP.AddStress(grbmodel, gurobiAssignmentVariables, dictionary2[loadCase], Structure, loadCase, Stock);
				}
			}
			SANDGurobiReuse.AddGroup(grbmodel, gurobiAssignmentVariables, Structure, Stock);
			FormStartPosition startPos;
			Point location;
			this.CloseOpenFormsAndGetPos(out startPos, out location);
			LightCallback lightCallback = null;
			LogCallback logCallback = null;
			bool logToConsole = this.Options.LogToConsole;
			if (logToConsole)
			{
				logCallback = new LogCallback(startPos, location, this.Options.LogFormName);
				grbmodel.SetCallback(logCallback);
			}
			else
			{
				lightCallback = new LightCallback();
				grbmodel.SetCallback(lightCallback);
			}
			try
			{
				grbmodel.Optimize();
				int status = grbmodel.Status;
				bool flag2 = status == 2 || status == 9 || status == 11;
				if (flag2)
				{
					bool flag3 = status == 9;
					if (flag3)
					{
						this.TimeLimitReached = true;
					}
					bool flag4 = status == 11;
					if (flag4)
					{
						this.Interrupted = true;
					}
					try
					{
						this.ObjectiveValue = grbmodel.ObjVal;
						bool logToConsole2 = this.Options.LogToConsole;
						if (logToConsole2)
						{
							this.LowerBounds = logCallback.LowerBounds;
							this.UpperBounds = logCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						else
						{
							this.LowerBounds = lightCallback.LowerBounds;
							this.UpperBounds = lightCallback.UpperBounds;
							this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
							this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
						}
						foreach (IMember member in Structure.Members)
						{
							Bar bar = (Bar)member;
							bar.Nx.Clear();
							Assignment assignment = new Assignment();
							bool flag5 = false;
							for (int i = 0; i < Stock.ElementGroups.Count; i++)
							{
								bool flag6 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + i].X >= 0.999;
								if (flag6)
								{
									flag5 = true;
									bar.CrossSection = Stock.ElementGroups[i].CrossSection;
									bar.Material = Stock.ElementGroups[i].Material;
									assignment.AddElementAssignment(Stock.ElementGroups[i], Stock.ElementGroups[i].Next);
									foreach (LoadCase loadCase2 in LoadCases)
									{
										bar.AddNormalForce(loadCase2, new List<double>
										{
											dictionary2[loadCase2][bar.Number * Stock.ElementGroups.Count + i].X * bar.Material.E * Stock.ElementGroups[i].CrossSection.Area / bar.Length
										});
									}
								}
							}
							bool flag7 = !flag5;
							if (flag7)
							{
								assignment.AddElementAssignment(Stock.ElementGroups[0], 0);
								bar.CrossSection = new EmptySection();
								foreach (LoadCase lc in LoadCases)
								{
									bar.AddNormalForce(lc, new List<double>
									{
										0.0
									});
								}
							}
							bar.SetAssignment(assignment);
						}
						Structure.SetResults(new Result(Structure, Stock, null));
					}
					catch (GRBException ex)
					{
						this.Message = ex.Message;
					}
				}
				else
				{
					bool flag8 = status == 3;
					if (flag8)
					{
						throw new SystemException("Gurobi problem is infeasible");
					}
				}
				grbmodel.Dispose();
				grbenv.Dispose();
			}
			catch (GRBException ex2)
			{
				throw new GurobiException(ex2.Message);
			}
			grbmodel.Dispose();
			grbenv.Dispose();
			bool flag9 = !Structure.AllTopologyFixed();
			if (flag9)
			{
				Stock.RemoveElementGroup(0);
			}
			Stock.ResetNext();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00008868 File Offset: 0x00006A68
		private void CloseOpenFormsAndGetPos(out FormStartPosition Pos, out Point Location)
		{
			Pos = FormStartPosition.Manual;
			Location = default(Point);
			List<Form> list = new List<Form>();
			foreach (object obj in Application.OpenForms)
			{
				Form form = (Form)obj;
				bool flag = form.Name == this.Options.LogFormName;
				if (flag)
				{
					list.Add(form);
				}
			}
			foreach (Form form2 in list)
			{
				Pos = form2.StartPosition;
				Location = form2.Location;
				form2.Close();
				form2.Dispose();
			}
		}

		// Token: 0x0400007F RID: 127
		public List<Tuple<double, double>> LowerBounds;

		// Token: 0x04000080 RID: 128
		public List<Tuple<double, double>> UpperBounds;
	}
}
