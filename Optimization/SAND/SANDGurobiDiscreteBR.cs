using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.SAND
{
	// Token: 0x0200001C RID: 28
	public static class SANDGurobiDiscreteBR
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000A1C4 File Offset: 0x000083C4
		public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock)
		{
			return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A1FC File Offset: 0x000083FC
		public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberForceVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases)
		{
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			int num = Structure.Members.OfType<IMember1D>().Count<IMember1D>();
			foreach (LoadCase key in LoadCases)
			{
				dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num).ToArray<double>(), new double[num], Enumerable.Repeat<char>('C', num).ToArray<char>(), null));
			}
			return dictionary;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberElongationVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases, Stock Stock)
		{
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			int num = Structure.Members.OfType<Bar>().Count<Bar>();
			int count = Stock.ElementGroups.Count;
			foreach (LoadCase key in LoadCases)
			{
				dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num * count).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num * count).ToArray<double>(), new double[num * count], Enumerable.Repeat<char>('C', num * count).ToArray<char>(), null));
			}
			return dictionary;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000A37C File Offset: 0x0000857C
		public static Dictionary<LoadCase, GRBVar[]> GetGurobiDisplacementVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases)
		{
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			foreach (LoadCase loadCase in LoadCases)
			{
				GRBVar[] array = new GRBVar[Structure.NFreeTranslations];
				foreach (Node node in Structure.Nodes)
				{
					for (int i = 0; i < 3; i++)
					{
						bool flag = node.Fix[i];
						if (!flag)
						{
							bool flag2 = node.DisplacementBounds.ContainsKey(loadCase);
							if (flag2)
							{
								array[node.ReducedDofsTruss[i]] = Model.AddVar(node.DisplacementBounds[loadCase].LB[i], node.DisplacementBounds[loadCase].UB[i], 0.0, 'C', string.Concat(new string[]
								{
									"u_LC_",
									loadCase.Name,
									"Node",
									node.Number.ToString(),
									"d",
									i.ToString()
								}));
							}
							else
							{
								array[node.ReducedDofsTruss[i]] = Model.AddVar(-1E+100, 1E+100, 0.0, 'C', string.Concat(new string[]
								{
									"u_LC_",
									loadCase.Name,
									"Node",
									node.Number.ToString(),
									"d",
									i.ToString()
								}));
							}
						}
					}
				}
				dictionary.Add(loadCase, array);
			}
			return dictionary;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A5AC File Offset: 0x000087AC
		public static void SetObjective(Objective Objective, GRBModel model, GRBVar[] T, Structure Structure, Stock Stock)
		{
			GRBLinExpr grblinExpr = new GRBLinExpr();
			if (Objective != Objective.MinStructureMass)
			{
				foreach (IMember member in Structure.Members)
				{
					Bar bar = (Bar)member;
					for (int i = 0; i < Stock.ElementGroups.Count; i++)
					{
						grblinExpr += T[bar.Number * Stock.ElementGroups.Count + i] * bar.Length * Stock.ElementGroups[i].Material.Density * Stock.ElementGroups[i].CrossSection.Area;
					}
				}
				model.SetObjective(grblinExpr);
			}
			else
			{
				foreach (IMember member2 in Structure.Members)
				{
					IMember1D member1D = (IMember1D)member2;
					for (int j = 0; j < Stock.ElementGroups.Count; j++)
					{
						grblinExpr += T[member1D.Number * Stock.ElementGroups.Count + j] * member1D.Length * Stock.ElementGroups[j].Material.Density * Stock.ElementGroups[j].CrossSection.Area;
					}
				}
				model.SetObjective(grblinExpr);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000A798 File Offset: 0x00008998
		//设计分配约束，具体的分配矩阵见论文
		public static void AddAssignment(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					bool flag = member1D.TopologyFixed && Stock.ElementGroups[j].CrossSection.Area == 0.0;  
					if (flag)
					{
						grblinExpr.AddTerm(0.0, T[member1D.Number * Stock.ElementGroups.Count + j]);   
						T[member1D.Number * Stock.ElementGroups.Count + j].UB = 0.0;     //如果某处拓扑固定（一定有该杆件），那么该处不可能出现截面为0的杆件？
					}
					else
					{
						grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + j]);
					}
				}
				model.AddConstr(grblinExpr, '=', 1.0, "AssignmentBar" + member1D.Number.ToString());   //每一个杆件，最多只分配了一个库存元素，应该为<=？

				bool sos_Assignment = Options.SOS_Assignment;//这一步加入sos约束，默认false，type=1，指定列表中最多允许一个变量取非零值。详细见gurobi手册。感觉和前面添加的线性约束差是同样意思？？？？
				if (sos_Assignment)
				{
					model.AddSOS(T.Skip(member1D.Number * Stock.ElementGroups.Count).Take(Stock.ElementGroups.Count).ToArray<GRBVar>(), (from i in Enumerable.Range(1, Stock.ElementGroups.Count)
					select (double)i).ToArray<double>(), 1);
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000A984 File Offset: 0x00008B84
		//平衡约束
		public static void AddEquilibrium(GRBModel model, GRBVar[] T, GRBVar[] MemberForces, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			foreach (Node node in Structure.Nodes)
			{
				for (int i = 0; i < 3; i++)
				{
					bool flag = node.Fix[i];
					if (!flag)
					{
						GRBLinExpr grblinExpr = 0.0;
						bool flag2 = node.PointLoads.ContainsKey(LC);
						if (flag2)
						{
							grblinExpr.AddConstant(-node.PointLoads[LC].FM[i]);
						}
						foreach (IMember member in node.ConnectedMembers.Keys)
						{
							IMember1D member1D = (IMember1D)member;
							bool flag3 = Options.Selfweight && i == 2;
							if (flag3)
							{
								for (int j = 0; j < Stock.ElementGroups.Count; j++)
								{
									grblinExpr.AddTerm(Stock.ElementGroups[j].CrossSection.Area * member1D.Length * Stock.ElementGroups[j].Material.Density * LC.yg * 1E-05 * 0.5, T[member1D.Number * Stock.ElementGroups.Count + j]);
								}
							}
							grblinExpr.AddTerm(node.ConnectedMembers[member1D] * member1D.Direction[i], MemberForces[member1D.Number]);
						}
						model.AddConstr(grblinExpr, '=', 0.0, string.Concat(new string[]
						{
							"LC",
							LC.Number.ToString(),
							"EquilibriumNode",
							node.Number.ToString(),
							"dof",
							i.ToString()
						}));
					}
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000AC00 File Offset: 0x00008E00
		//几何兼容性约束
		public static void AddCompatibility(GRBModel model, GRBVar[] MemberElongations, GRBVar[] Displacements, Structure Structure, LoadCase LC, Stock Stock)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				GRBLinExpr grblinExpr2 = new GRBLinExpr();
				for (int i = 0; i < 3; i++)
				{
					bool flag = !member1D.From.Fix[i];
					if (flag)
					{
						grblinExpr.AddTerm(-member1D.Direction[i], Displacements[member1D.From.ReducedDofsTruss[i]]);
					}
					bool flag2 = !member1D.To.Fix[i];
					if (flag2)
					{
						grblinExpr.AddTerm(member1D.Direction[i], Displacements[member1D.To.ReducedDofsTruss[i]]);
					}
				}
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					grblinExpr2.AddTerm(1.0, MemberElongations[member1D.Number * Stock.ElementGroups.Count + j]);
				}
				model.AddConstr(grblinExpr, '=', grblinExpr2, "CompatibilityLC" + LC.Name + "Member" + member1D.Number.ToString());
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000AD94 File Offset: 0x00008F94
		//几何兼容性
		public static void AddConstitutive(GRBModel model, GRBVar[] MemberForces, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				for (int i = 0; i < Stock.ElementGroups.Count; i++)
				{
					grblinExpr.AddTerm(Stock.ElementGroups[i].Material.E * Stock.ElementGroups[i].CrossSection.Area / member1D.Length, MemberElongations[member1D.Number * Stock.ElementGroups.Count + i]);
				}
				model.AddConstr(MemberForces[member1D.Number], '=', grblinExpr, "ConstitutiveLC" + LC.Name + "Member" + member1D.Number.ToString());
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public static void AddBigM(GRBModel model, GRBVar[] T, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					double num;
					double num2;
					SANDGurobiDiscreteBR.GetEminEmax(member1D, Stock.ElementGroups[j], LC, out num, out num2);
					model.AddConstr(T[member1D.Number * Stock.ElementGroups.Count + j] * num, '<', MemberElongations[member1D.Number * Stock.ElementGroups.Count + j], "BigMemin" + LC.Name + "Member" + member1D.Number.ToString());
					model.AddConstr(T[member1D.Number * Stock.ElementGroups.Count + j] * num2, '>', MemberElongations[member1D.Number * Stock.ElementGroups.Count + j], "BigMemax" + LC.Name + "Member" + member1D.Number.ToString());
				}
				bool sos_Continuous = Options.SOS_Continuous;
				if (sos_Continuous)
				{
					model.AddSOS(MemberElongations.Skip(member1D.Number * Stock.ElementGroups.Count).Take(Stock.ElementGroups.Count).ToArray<GRBVar>(), (from i in Enumerable.Range(1, Stock.ElementGroups.Count)
					select (double)i).ToArray<double>(), 1);
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public static void AddStress(GRBModel model, GRBVar[] T, GRBVar[] MemberForces, Structure Structure, LoadCase LC, Stock Stock)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				GRBLinExpr grblinExpr2 = new GRBLinExpr();
				for (int i = 0; i < Stock.ElementGroups.Count; i++)
				{
					grblinExpr += Stock.ElementGroups[i].CrossSection.GetBucklingResistance(Stock.ElementGroups[i].Material, member1D.BucklingType, member1D.BucklingLength).Max() * T[member1D.Number * Stock.ElementGroups.Count + i];
					grblinExpr2 += Stock.ElementGroups[i].CrossSection.GetTensionResistance(Stock.ElementGroups[i].Material) * T[member1D.Number * Stock.ElementGroups.Count + i];
				}
				model.AddConstr(grblinExpr <= MemberForces[member1D.Number], "LC" + LC.Number.ToString() + "Compression" + member1D.Number.ToString());
				model.AddConstr(grblinExpr2 >= MemberForces[member1D.Number], "LC" + LC.Number.ToString() + "Tension" + member1D.Number.ToString());
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000B27C File Offset: 0x0000947C
		private static void GetEminEmax(IMember1D M, ElementGroup EG, LoadCase LC, out double emin, out double emax)
		{
			double num = 0.0;
			double num2 = 0.0;
			for (int i = 0; i < 3; i++)
			{
				bool flag = M.To.DisplacementBounds.ContainsKey(LC) && M.From.DisplacementBounds.ContainsKey(LC);
				if (flag)
				{
					num += M.To.DisplacementBounds[LC].LB[i] * Math.Max(M.To.ConnectedMembers[M] * M.Direction[i], 0.0) + M.From.DisplacementBounds[LC].UB[i] * Math.Min(M.From.ConnectedMembers[M] * M.Direction[i], 0.0);
					num += M.From.DisplacementBounds[LC].LB[i] * Math.Max(M.From.ConnectedMembers[M] * M.Direction[i], 0.0) + M.To.DisplacementBounds[LC].UB[i] * Math.Min(M.To.ConnectedMembers[M] * M.Direction[i], 0.0);
				}
				else
				{
					num = -1E+100;
				}
				bool flag2 = M.From.DisplacementBounds.ContainsKey(LC) && M.To.DisplacementBounds.ContainsKey(LC);
				if (flag2)
				{
					num2 += M.From.DisplacementBounds[LC].UB[i] * Math.Max(M.From.ConnectedMembers[M] * M.Direction[i], 0.0) + M.To.DisplacementBounds[LC].LB[i] * Math.Min(M.To.ConnectedMembers[M] * M.Direction[i], 0.0);
					num2 += M.To.DisplacementBounds[LC].UB[i] * Math.Max(M.To.ConnectedMembers[M] * M.Direction[i], 0.0) + M.From.DisplacementBounds[LC].LB[i] * Math.Min(M.From.ConnectedMembers[M] * M.Direction[i], 0.0);
				}
				else
				{
					num2 = 1E+100;
				}
			}
			double val = -M.Length;
			double val2 = M.Length;
			bool flag3 = EG.CrossSection.Area > 0.0;
			//if (flag3)
			//{
			//	val = -M.Length * M.Material.fc / M.Material.E;   //这里我有修改，应该是用库存原始的材料来计算？？
			//	val2 = M.Length * M.Material.ft / M.Material.E;
			//}
			if (flag3)
			{
				val = -M.Length * EG.Material.fc / EG.Material.E;
				val2 = M.Length * EG.Material.ft / EG.Material.E;
			}
			emin = Math.Max(num, val);
			emax = Math.Min(num2, val2);
		}
	}
}
