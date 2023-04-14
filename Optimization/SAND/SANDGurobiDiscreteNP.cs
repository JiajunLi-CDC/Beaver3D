using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.SAND
{
	// Token: 0x0200001B RID: 27
	public static class SANDGurobiDiscreteNP
	{
		// Token: 0x06000109 RID: 265 RVA: 0x0000906C File Offset: 0x0000726C
		public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock)
		{
			return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000090A4 File Offset: 0x000072A4
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

		// Token: 0x0600010B RID: 267 RVA: 0x00009170 File Offset: 0x00007370
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

		// Token: 0x0600010C RID: 268 RVA: 0x000093A0 File Offset: 0x000075A0
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

		// Token: 0x0600010D RID: 269 RVA: 0x0000958C File Offset: 0x0000778C
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

		// Token: 0x0600010E RID: 270 RVA: 0x00009778 File Offset: 0x00007978
		public static void AddEquilibrium(GRBModel model, GRBVar[] T, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
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
							for (int j = 0; j < Stock.ElementGroups.Count; j++)
							{
								bool flag3 = Options.Selfweight && i == 2;
								if (flag3)
								{
									grblinExpr.AddTerm(Stock.ElementGroups[j].CrossSection.Area * member1D.Length * Stock.ElementGroups[j].Material.Density * LC.yg * 1E-05 * 0.5, T[member1D.Number * Stock.ElementGroups.Count + j]);
								}
								grblinExpr.AddTerm(node.ConnectedMembers[member1D] * member1D.Direction[i] * Stock.ElementGroups[j].CrossSection.Area * member1D.Material.E / member1D.Length, MemberElongations[member1D.Number * Stock.ElementGroups.Count + j]);
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

		// Token: 0x0600010F RID: 271 RVA: 0x00009A34 File Offset: 0x00007C34
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

		// Token: 0x06000110 RID: 272 RVA: 0x00009BC8 File Offset: 0x00007DC8
		public static void AddBigM(GRBModel model, GRBVar[] T, GRBVar[] MemberElongations, Structure Structure, LoadCase LC, Stock Stock, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					double num;
					double num2;
					SANDGurobiDiscreteNP.GetEminEmax(member1D, Stock.ElementGroups[j], LC, out num, out num2);
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

		// Token: 0x06000111 RID: 273 RVA: 0x00009DC0 File Offset: 0x00007FC0
		public static void AddStress(GRBModel model, GRBVar[] T, GRBVar[] MemberForces, Structure Structure, LoadCase LC, Stock Stock)
		{
			throw new NotImplementedException("Stress and buckling constraints not implemented here");
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009DD0 File Offset: 0x00007FD0
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
			if (flag3)
			{
				val = -M.Length * M.Material.fc / M.Material.E;
				val2 = M.Length * M.Material.ft / M.Material.E;
			}
			emin = Math.Max(num, val);
			emax = Math.Min(num2, val2);
		}
	}
}
