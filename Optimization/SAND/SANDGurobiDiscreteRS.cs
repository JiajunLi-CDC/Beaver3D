using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.SAND
{
	// Token: 0x0200001E RID: 30
	public static class SANDGurobiDiscreteRS
	{
		// Token: 0x0600012D RID: 301 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock)
		{
			return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000CA00 File Offset: 0x0000AC00
		public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberForceVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases, Stock Stock, OptimOptions Options)
		{
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			int num = Structure.Members.OfType<Bar>().Count<Bar>();
			int count = Stock.ElementGroups.Count;
			foreach (LoadCase key in LoadCases)
			{
				bool compatibility = Options.Compatibility;
				if (compatibility)
				{
					dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num * count).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num * count).ToArray<double>(), new double[num * count], Enumerable.Repeat<char>('C', num * count).ToArray<char>(), null));
				}
				else
				{
					dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num).ToArray<double>(), new double[num], Enumerable.Repeat<char>('C', num).ToArray<char>(), null));
				}
			}
			return dictionary;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000CB30 File Offset: 0x0000AD30
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

		// Token: 0x06000130 RID: 304 RVA: 0x0000CD60 File Offset: 0x0000AF60
		//设置目标
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

		// Token: 0x06000131 RID: 305 RVA: 0x0000CF4C File Offset: 0x0000B14C
		//分配约束
		public static void AddAssignment(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				bool flag = Options.SOS_Assignment && !member1D.TopologyFixed;
				if (flag)
				{
					model.AddSOS(T.Skip(member1D.Number * Stock.ElementGroups.Count).Take(Stock.ElementGroups.Count).ToArray<GRBVar>(), (from i in Enumerable.Range(1, Stock.ElementGroups.Count)
					select (double)i).ToArray<double>(), 1);
				}
				else
				{
					GRBLinExpr grblinExpr = new GRBLinExpr();
					for (int j = 0; j < Stock.ElementGroups.Count; j++)
					{
						grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + j]);
					}
					bool topologyFixed = member1D.TopologyFixed;
					if (topologyFixed)
					{
						model.AddConstr(grblinExpr, '=', 1.0, "AssignmentBar" + member1D.Number.ToString());
					}
					else
					{
						model.AddConstr(grblinExpr, '<', 1.0, "AssignmentBar" + member1D.Number.ToString());
					}
				}
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000D104 File Offset: 0x0000B304
		//平衡性约束
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
							grblinExpr -= node.PointLoads[LC].FM[i];
						}
						foreach (IMember member in node.ConnectedMembers.Keys)
						{
							IMember1D member1D = (IMember1D)member;
							bool flag3 = Options.Compatibility || Options.Selfweight;
							if (flag3)
							{
								for (int j = 0; j < Stock.ElementGroups.Count; j++)
								{
									bool flag4 = i == 2 && Options.Selfweight;
									if (flag4)
									{
										grblinExpr += T[member1D.Number * Stock.ElementGroups.Count + j] * Stock.ElementGroups[j].CrossSection.Area * member1D.Length * Stock.ElementGroups[j].Material.Density * LC.yg * 1E-05 * 0.5;
									}
									bool compatibility = Options.Compatibility;
									if (compatibility)
									{
										grblinExpr += node.ConnectedMembers[member1D] * member1D.Direction[i] * MemberForces[member1D.Number * Stock.ElementGroups.Count + j];
									}
								}
							}
							bool flag5 = !Options.Compatibility;
							if (flag5)
							{
								grblinExpr += node.ConnectedMembers[member1D] * member1D.Direction[i] * MemberForces[member1D.Number];
							}
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

		// Token: 0x06000133 RID: 307 RVA: 0x0000D428 File Offset: 0x0000B628
		//荷载约束
		public static void AddStress(GRBModel model, GRBVar[] T, GRBVar[] MemberForces, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			bool compatibility = Options.Compatibility;
			if (compatibility)
			{
				foreach (IMember member in Structure.Members)
				{
					IMember1D member1D = (IMember1D)member;
					for (int k = 0; k < Stock.ElementGroups.Count; k++)
					{
						model.AddConstr(Stock.ElementGroups[k].CrossSection.GetBucklingResistance(member1D.Material, member1D.BucklingType, member1D.BucklingLength).Max() * T[member1D.Number * Stock.ElementGroups.Count + k], '<', MemberForces[member1D.Number * Stock.ElementGroups.Count + k], string.Concat(new string[]
						{
							"LC",
							LC.Number.ToString(),
							"Compression",
							member1D.Number.ToString(),
							"_",
							k.ToString()
						}));
						model.AddConstr(Stock.ElementGroups[k].CrossSection.GetTensionResistance(Stock.ElementGroups[k].Material) * T[member1D.Number * Stock.ElementGroups.Count + k], '>', MemberForces[member1D.Number * Stock.ElementGroups.Count + k], string.Concat(new string[]
						{
							"LC",
							LC.Number.ToString(),
							"Tension",
							member1D.Number.ToString(),
							"_",
							k.ToString()
						}));
					}
					bool sos_Continuous = Options.SOS_Continuous;
					if (sos_Continuous)
					{
						model.AddSOS(MemberForces.Skip(member1D.Number * Stock.ElementGroups.Count).Take(Stock.ElementGroups.Count).ToArray<GRBVar>(), (from i in Enumerable.Range(1, Stock.ElementGroups.Count)
						select (double)i).ToArray<double>(), 1);
					}
				}
			}
			else
			{
				foreach (IMember member2 in Structure.Members)
				{
					IMember1D member1D2 = (IMember1D)member2;
					GRBLinExpr grblinExpr = new GRBLinExpr();
					GRBLinExpr grblinExpr2 = new GRBLinExpr();
					for (int j = 0; j < Stock.ElementGroups.Count; j++)
					{
						grblinExpr += Stock.ElementGroups[j].CrossSection.GetBucklingResistance(member1D2.Material, member1D2.BucklingType, member1D2.BucklingLength).Max() * T[member1D2.Number * Stock.ElementGroups.Count + j];
						grblinExpr2 += Stock.ElementGroups[j].CrossSection.GetTensionResistance(Stock.ElementGroups[j].Material) * T[member1D2.Number * Stock.ElementGroups.Count + j];
					}
					model.AddConstr(grblinExpr, '<', MemberForces[member1D2.Number], "LC" + LC.Number.ToString() + "Compression" + member1D2.Number.ToString());
					model.AddConstr(grblinExpr2, '>', MemberForces[member1D2.Number], "LC" + LC.Number.ToString() + "Tension" + member1D2.Number.ToString());
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000D894 File Offset: 0x0000BA94
		//兼容性约束
		public static void AddCompatibility(GRBModel model, GRBVar[] T, GRBVar[] MemberForces, GRBVar[] Displacements, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				for (int i = 0; i < Stock.ElementGroups.Count; i++)
				{
					double num;
					double num2;
					SANDGurobiDiscreteRS.GetCminCmax(member1D, Stock.ElementGroups[i], LC, out num, out num2);
					GRBLinExpr grblinExpr = new GRBLinExpr();
					for (int j = 0; j < 3; j++)
					{
						bool flag = !member1D.From.Fix[j];
						if (flag)
						{
							grblinExpr.AddTerm(-member1D.Direction[j] * member1D.Material.E * Stock.ElementGroups[i].CrossSection.Area / member1D.Length, Displacements[member1D.From.ReducedDofsTruss[j]]);
						}
						bool flag2 = !member1D.To.Fix[j];
						if (flag2)
						{
							grblinExpr.AddTerm(member1D.Direction[j] * member1D.Material.E * Stock.ElementGroups[i].CrossSection.Area / member1D.Length, Displacements[member1D.To.ReducedDofsTruss[j]]);
						}
					}
					model.AddConstr(num - T[member1D.Number * Stock.ElementGroups.Count + i] * num, '<', grblinExpr - MemberForces[member1D.Number * Stock.ElementGroups.Count + i], null);
					model.AddConstr(num2 - T[member1D.Number * Stock.ElementGroups.Count + i] * num2, '>', grblinExpr - MemberForces[member1D.Number * Stock.ElementGroups.Count + i], null);
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000DAEC File Offset: 0x0000BCEC
		private static void GetCminCmax(IMember1D M, ElementGroup EG, LoadCase LC, out double cmin, out double cmax)
		{
			cmin = 0.0;
			cmax = 0.0;
			for (int i = 0; i < 3; i++)
			{
				bool flag = M.To.DisplacementBounds.ContainsKey(LC) && M.From.DisplacementBounds.ContainsKey(LC);
				if (flag)
				{
					cmin += M.To.DisplacementBounds[LC].LB[i] * Math.Max(M.To.ConnectedMembers[M] * M.Direction[i], 0.0) + M.From.DisplacementBounds[LC].UB[i] * Math.Min(M.From.ConnectedMembers[M] * M.Direction[i], 0.0);
					cmin += M.From.DisplacementBounds[LC].LB[i] * Math.Max(M.From.ConnectedMembers[M] * M.Direction[i], 0.0) + M.To.DisplacementBounds[LC].UB[i] * Math.Min(M.To.ConnectedMembers[M] * M.Direction[i], 0.0);
				}
				else
				{
					cmin = -1E+100;
				}
				bool flag2 = M.From.DisplacementBounds.ContainsKey(LC) && M.To.DisplacementBounds.ContainsKey(LC);
				if (flag2)
				{
					cmax += M.From.DisplacementBounds[LC].UB[i] * Math.Max(M.From.ConnectedMembers[M] * M.Direction[i], 0.0) + M.To.DisplacementBounds[LC].LB[i] * Math.Min(M.To.ConnectedMembers[M] * M.Direction[i], 0.0);
					cmax += M.To.DisplacementBounds[LC].UB[i] * Math.Max(M.To.ConnectedMembers[M] * M.Direction[i], 0.0) + M.From.DisplacementBounds[LC].LB[i] * Math.Min(M.From.ConnectedMembers[M] * M.Direction[i], 0.0);
				}
				else
				{
					cmax = 1E+100;
				}
			}
			cmin *= M.Material.E * EG.CrossSection.Area / M.Length;
			cmax *= M.Material.E * EG.CrossSection.Area / M.Length;
		}
	}
}
