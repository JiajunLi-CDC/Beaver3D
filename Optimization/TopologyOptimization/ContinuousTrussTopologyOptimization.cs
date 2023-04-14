using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver3D.Optimization.SAND;

namespace Beaver3D.Optimization.TopologyOptimization
{
	// Token: 0x0200001A RID: 26
	public class ContinuousTrussTopologyOptimization
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00008958 File Offset: 0x00006B58
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00008960 File Offset: 0x00006B60
		public List<Assignment> Assignments { get; private set; } = new List<Assignment>();

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00008969 File Offset: 0x00006B69
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008971 File Offset: 0x00006B71
		public Objective Objective { get; private set; } = Objective.MinStructureMass;

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000897A File Offset: 0x00006B7A
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00008982 File Offset: 0x00006B82
		public double ObjectiveValue { get; private set; } = double.PositiveInfinity;

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000898B File Offset: 0x00006B8B
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00008993 File Offset: 0x00006B93
		public OptimOptions Options { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000899C File Offset: 0x00006B9C
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000089A4 File Offset: 0x00006BA4
		public Dictionary<LoadCase, double[]> MemberForces { get; private set; } = new Dictionary<LoadCase, double[]>();

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000089AD File Offset: 0x00006BAD
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000089B5 File Offset: 0x00006BB5
		public bool TimeLimitReached { get; private set; } = false;

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000089BE File Offset: 0x00006BBE
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000089C6 File Offset: 0x00006BC6
		public bool Interrupted { get; private set; } = false;

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000089CF File Offset: 0x00006BCF
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000089D7 File Offset: 0x00006BD7
		public string Message { get; private set; } = "";

		// Token: 0x06000104 RID: 260 RVA: 0x000089E0 File Offset: 0x00006BE0
		public ContinuousTrussTopologyOptimization(OptimOptions Options = null)
		{
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

		// Token: 0x06000105 RID: 261 RVA: 0x00008A58 File Offset: 0x00006C58
		public void Solve(Structure Structure)
		{
			this.Solve(Structure, new List<string>
			{
				"all"
			});
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008A74 File Offset: 0x00006C74
		public void Solve(Structure Structure, List<string> LoadCaseNames)
		{
			List<LoadCase> loadCasesFromNames = Structure.GetLoadCasesFromNames(LoadCaseNames);
			bool flag = loadCasesFromNames == null || loadCasesFromNames.Count == 0;
			if (flag)
			{
				throw new ArgumentException("LoadCases with the provided names are not existing in the structure. Check the names or use 'all' to compute all LoadCases.");
			}
			LPOptimizer lpoptimizer = this.Options.LPOptimizer;
			LPOptimizer lpoptimizer2 = lpoptimizer;
			if (lpoptimizer2 != LPOptimizer.Gurobi)
			{
				this.SolveGurobi(Structure, loadCasesFromNames);
			}
			else
			{
				this.SolveGurobi(Structure, loadCasesFromNames);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public void SolveGurobi(Structure Structure, List<LoadCase> LoadCases)
		{
			GRBEnv grbenv = new GRBEnv();
			GRBModel grbmodel = new GRBModel(grbenv);
			int num = Structure.Members.OfType<Bar>().Count<Bar>();
			GRBVar[] array = grbmodel.AddVars(num, 'C');
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			int num2 = 0;
			foreach (IMember member in Structure.Members)
			{
				Bar bar = (Bar)member;
				array[num2].LB = bar.LBArea;
				array[num2].UB = bar.UBArea;
				array[num2].VarName = "a" + num2.ToString();
				num2++;
			}
			foreach (LoadCase loadCase in LoadCases)
			{
				dictionary.Add(loadCase, grbmodel.AddVars(Enumerable.Repeat<double>(double.NegativeInfinity, num).ToArray<double>(), Enumerable.Repeat<double>(double.PositiveInfinity, num).ToArray<double>(), new double[num], Enumerable.Repeat<char>('C', num).ToArray<char>(), null));
				SANDGurobiContinuous.AddEquilibrium(grbmodel, array, dictionary[loadCase], Structure, loadCase, this.Options);
				SANDGurobiContinuous.AddStress(grbmodel, array, dictionary[loadCase], Structure, loadCase, this.Options);
			}
			SANDGurobiContinuous.SetObjective(this.Objective, grbmodel, array, dictionary, Structure);
			FormStartPosition startPos;
			Point location;
			this.CloseOpenFormsAndGetPos(out startPos, out location);
			bool logToConsole = this.Options.LogToConsole;
			if (logToConsole)
			{
				grbmodel.SetCallback(new LogCallback(startPos, location, this.Options.LogFormName));
			}
			else
			{
				grbmodel.SetCallback(new LightCallback());
			}
			try
			{
				grbmodel.Optimize();
			}
			catch
			{
			}
			int status = grbmodel.Status;
			bool flag = status == 9;
			if (flag)
			{
				this.TimeLimitReached = true;
			}
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
					num2 = 0;
					foreach (IMember member2 in Structure.Members)
					{
						Bar bar2 = (Bar)member2;
						double x = array[bar2.Number].X;
						bar2.CrossSection = new CircularSection(Math.Sqrt(4.0 * x / 3.1415926535897931));
						bar2.Nx.Clear();
						num2++;
					}
					foreach (LoadCase loadCase2 in LoadCases)
					{
						double[] array2 = new double[dictionary[loadCase2].Length];
						foreach (IMember member3 in Structure.Members)
						{
							Bar bar3 = (Bar)member3;
							array2[bar3.Number] = dictionary[loadCase2][bar3.Number].X;
							bar3.AddNormalForce(loadCase2, new List<double>
							{
								dictionary[loadCase2][bar3.Number].X
							});
						}
						this.MemberForces.Add(loadCase2, array2);
					}
					Structure.SetResults(new Result(Structure, null));
				}
				catch (GRBException ex)
				{
					this.Message = ex.Message;
				}
			}
			else
			{
				bool flag5 = status == 3;
				if (flag5)
				{
					throw new SystemException("Gurobi problem is infeasible");
				}
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00008F7C File Offset: 0x0000717C
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
	}
}
