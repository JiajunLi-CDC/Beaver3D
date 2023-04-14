using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.SAND
{
	// Token: 0x0200001D RID: 29
	public static class SANDGurobiDiscreteGG
	{
		// Token: 0x0600011F RID: 287 RVA: 0x0000B670 File Offset: 0x00009870
		public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock)
		{
			return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000B6A8 File Offset: 0x000098A8
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

		// Token: 0x06000121 RID: 289 RVA: 0x0000B75C File Offset: 0x0000995C
		public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberStressVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases)
		{
			Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
			foreach (LoadCase loadCase in LoadCases)
			{
				GRBVar[] array = new GRBVar[Structure.Members.OfType<IMember1D>().Count<IMember1D>()];
				foreach (IMember member in Structure.Members)
				{
					array[member.Number] = Model.AddVar(-member.Material.fc, member.Material.ft, 0.0, 'C', "StressLC" + loadCase.Name + "Member" + member.Number.ToString());
				}
				dictionary.Add(loadCase, array);
			}
			return dictionary;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000B874 File Offset: 0x00009A74
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

		// Token: 0x06000123 RID: 291 RVA: 0x0000B940 File Offset: 0x00009B40
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

		// Token: 0x06000124 RID: 292 RVA: 0x0000BB70 File Offset: 0x00009D70
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

		// Token: 0x06000125 RID: 293 RVA: 0x0000BD5C File Offset: 0x00009F5C
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
						T[member1D.Number * Stock.ElementGroups.Count + j].UB = 0.0;
					}
					else
					{
						grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + j]);
					}
				}
				model.AddConstr(grblinExpr, '=', 1.0, "AssignmentBar" + member1D.Number.ToString());
				bool sos_Assignment = Options.SOS_Assignment;
				if (sos_Assignment)
				{
					model.AddSOS(T.Skip(member1D.Number * Stock.ElementGroups.Count).Take(Stock.ElementGroups.Count).ToArray<GRBVar>(), (from i in Enumerable.Range(1, Stock.ElementGroups.Count)
					select (double)i).ToArray<double>(), 1);
				}
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000BF48 File Offset: 0x0000A148
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

		// Token: 0x06000127 RID: 295 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
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

		// Token: 0x06000128 RID: 296 RVA: 0x0000C358 File Offset: 0x0000A558
		public static void AddConstitutive(GRBModel model, GRBVar[] MemberForces, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				for (int i = 0; i < Stock.ElementGroups.Count; i++)
				{
					grblinExpr.AddTerm(member1D.Material.E * Stock.ElementGroups[i].CrossSection.Area / member1D.Length, MemberElongations[member1D.Number * Stock.ElementGroups.Count + i]);
				}
				model.AddConstr(MemberForces[member1D.Number], '=', grblinExpr, "ConstitutiveLC" + LC.Name + "Member" + member1D.Number.ToString());
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000C460 File Offset: 0x0000A660
		public static void AddHooke(GRBModel model, GRBVar[] MemberStresses, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				GRBLinExpr grblinExpr = new GRBLinExpr();
				for (int i = 0; i < Stock.ElementGroups.Count; i++)
				{
					bool flag = Stock.ElementGroups[i].CrossSection.Area > 0.0;
					if (flag)
					{
						grblinExpr.AddTerm(member1D.Material.E / member1D.Length, MemberElongations[member1D.Number * Stock.ElementGroups.Count + i]);
					}
				}
				model.AddConstr(MemberStresses[member1D.Number], '=', grblinExpr, "HookeLC" + LC.Name + "Member" + member1D.Number.ToString());
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000C578 File Offset: 0x0000A778
		public static void AddBigM(GRBModel model, GRBVar[] T, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					double num;
					double num2;
					SANDGurobiDiscreteGG.GetEminEmax(member1D, Stock.ElementGroups[j], out num, out num2);
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

		// Token: 0x0600012B RID: 299 RVA: 0x0000C770 File Offset: 0x0000A970
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

		// Token: 0x0600012C RID: 300 RVA: 0x0000C94C File Offset: 0x0000AB4C
		private static void GetEminEmax(IMember1D M, ElementGroup EG, out double emin, out double emax)
		{
			emin = -M.Length;
			emax = M.Length;
			bool flag = EG.CrossSection.Area > 0.0;
			if (flag)
			{
				emin = -M.Length * M.Material.fc / M.Material.E;
				emax = M.Length * M.Material.ft / M.Material.E;
			}
		}
	}
}
