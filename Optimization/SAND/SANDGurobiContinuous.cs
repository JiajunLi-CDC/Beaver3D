using System;
using System.Collections.Generic;
using Gurobi;
using Beaver3D.Model;

namespace Beaver3D.Optimization.SAND
{
	// Token: 0x0200001F RID: 31
	public static class SANDGurobiContinuous
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000DE90 File Offset: 0x0000C090
		public static void SetObjective(Objective Objective, GRBModel model, GRBVar[] Areas, Dictionary<LoadCase, GRBVar[]> MemberForces, Structure Structure)
		{
			GRBLinExpr grblinExpr = new GRBLinExpr();
			if (Objective != Objective.MinStructureMass)
			{
				foreach (IMember member in Structure.Members)
				{
					Bar bar = (Bar)member;
					grblinExpr += Areas[bar.Number] * bar.Length * bar.Material.Density;
				}
				model.SetObjective(grblinExpr);
			}
			else
			{
				foreach (IMember member2 in Structure.Members)
				{
					Bar bar2 = (Bar)member2;
					grblinExpr += Areas[bar2.Number] * bar2.Length * bar2.Material.Density;
				}
				model.SetObjective(grblinExpr);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		public static void AddEquilibrium(GRBModel model, GRBVar[] Areas, GRBVar[] MemberForces, Structure Structure, LoadCase LC, OptimOptions Options)
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
							bool flag3 = i == 2 && Options.Selfweight;
							if (flag3)
							{
								grblinExpr += Areas[member1D.Number] * member1D.Length * member1D.Material.Density * LC.yg * 1E-05 * 0.5;
							}
							grblinExpr += node.ConnectedMembers[member1D] * member1D.Direction[i] * MemberForces[member1D.Number];
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

		// Token: 0x06000138 RID: 312 RVA: 0x0000E1F0 File Offset: 0x0000C3F0
		public static void AddStress(GRBModel model, GRBVar[] Areas, GRBVar[] MemberForces, Structure Structure, LoadCase LC, OptimOptions Options)
		{
			foreach (IMember member in Structure.Members)
			{
				Bar bar = (Bar)member;
				model.AddConstr(-bar.Material.fc / bar.Material.gamma_0 * Areas[bar.Number] <= MemberForces[bar.Number], "LC" + LC.Number.ToString() + "Compression" + bar.Number.ToString());
				model.AddConstr(bar.Material.ft / bar.Material.gamma_0 * Areas[bar.Number] >= MemberForces[bar.Number], "LC" + LC.Number.ToString() + "Tension" + bar.Number.ToString());
			}
		}
	}
}
