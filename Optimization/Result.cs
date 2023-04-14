using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.LCA;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization
{
	// Token: 0x02000017 RID: 23
	public class Result
	{
		// 记录结果
		public double StockMass { get; private set; } = 0.0;

		// 库存元素的总质量
		public double StructureMass { get; private set; } = 0.0;

		// 结构元素的总质量
		public double ReuseMass { get; private set; } = 0.0;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00005076 File Offset: 0x00003276
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000507E File Offset: 0x0000327E
		public double NewMass { get; private set; } = 0.0;

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005087 File Offset: 0x00003287
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000508F File Offset: 0x0000328F
		public double Waste { get; private set; } = 0.0;

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005098 File Offset: 0x00003298
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000050A0 File Offset: 0x000032A0
		public double EnvironmentalImpact { get; private set; } = 0.0;

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000050A9 File Offset: 0x000032A9
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000050B1 File Offset: 0x000032B1
		public int ReusedMembers { get; private set; } = 0;

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000050BA File Offset: 0x000032BA
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000050C2 File Offset: 0x000032C2
		public int NewMembers { get; private set; } = 0;

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000050CB File Offset: 0x000032CB
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000050D3 File Offset: 0x000032D3
		public int TotalMembers { get; private set; } = 0;

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000050DC File Offset: 0x000032DC
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000050E4 File Offset: 0x000032E4
		public double ReuseRateMass { get; private set; } = 0.0;

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000050ED File Offset: 0x000032ED
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000050F5 File Offset: 0x000032F5
		public double ReuseRateMembers { get; private set; } = 0.0;

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000050FE File Offset: 0x000032FE
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00005106 File Offset: 0x00003306
		public double MaxMemberMass { get; private set; } = 0.0;

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000510F File Offset: 0x0000330F
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005117 File Offset: 0x00003317
		public double MaxMemberImpact { get; private set; } = 0.0;

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005120 File Offset: 0x00003320
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00005128 File Offset: 0x00003328
		public double[] Utilization { get; private set; }

		// Token: 0x060000C3 RID: 195 RVA: 0x00005134 File Offset: 0x00003334
		public Result()
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000051F4 File Offset: 0x000033F4
		public Result(Structure Structure, Stock Stock, ILCA LCA = null)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				double num = 0.0;
				Assignment assignment = member1D.Assignment;
				for (int i = 0; i < assignment.ElementGroups.Count; i++)
				{
					ElementGroup elementGroup = assignment.ElementGroups[i];
					int num2 = assignment.ElementIndices[i];
					int num3 = this.TotalMembers;
					this.TotalMembers = num3 + 1;
					bool flag = elementGroup.Type == ElementType.Reuse;
					if (flag)
					{
						num3 = this.ReusedMembers;
						this.ReusedMembers = num3 + 1;
						bool flag2 = !elementGroup.AlreadyCounted[num2];
						if (flag2)
						{
							this.StockMass += elementGroup.Material.Density * elementGroup.CrossSection.Area * elementGroup.Length;
							elementGroup.AlreadyCounted[num2] = true;
						}
						this.ReuseMass += elementGroup.Material.Density * elementGroup.CrossSection.Area * member1D.Length;
					}
					else
					{
						bool flag3 = elementGroup.Type == ElementType.New;
						if (flag3)
						{
							num3 = this.NewMembers;
							this.NewMembers = num3 + 1;
							this.NewMass += elementGroup.Material.Density * elementGroup.CrossSection.Area * member1D.Length;
						}
					}
					this.StructureMass += elementGroup.Material.Density * elementGroup.CrossSection.Area * member1D.Length;
					num += elementGroup.Material.Density * elementGroup.CrossSection.Area * member1D.Length;
				}
				this.MaxMemberMass = Math.Max(this.MaxMemberMass, num);
			}
			Stock.ResetAlreadyCounted();
			this.Waste = this.StockMass - this.ReuseMass;
			this.ReuseRateMass = this.ReuseMass / this.StructureMass;
			this.ReuseRateMembers = (double)this.ReusedMembers / (double)this.TotalMembers;
			double maxMemberImpact = 0.0;
			bool flag4 = LCA != null;
			if (flag4)
			{
				this.EnvironmentalImpact = LCA.ReturnTotalImpact(Structure, out maxMemberImpact);
			}
			this.MaxMemberImpact = maxMemberImpact;
			this.Utilization = Result.GetUtilization(Structure);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000555C File Offset: 0x0000375C
		public Result(Structure Structure, ILCA LCA = null)
		{
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				this.StructureMass += member1D.CrossSection.Area + member1D.Material.Density * member1D.Length;
				this.MaxMemberMass = Math.Max(this.MaxMemberMass, member1D.Material.Density * member1D.CrossSection.Area * member1D.Length);
				bool flag = LCA != null;
				if (flag)
				{
					double num = LCA.ReturnMemberImpact(member1D);
					this.EnvironmentalImpact += num;
					this.MaxMemberImpact = Math.Max(this.MaxMemberImpact, num);
				}
			}
			this.Utilization = Result.GetUtilization(Structure.Members.OfType<IMember1D>().ToList<IMember1D>(), null);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005718 File Offset: 0x00003918
		public static double[] GetUtilization(Structure Structure)
		{
			double[] array = new double[Structure.Members.OfType<IMember1D>().Count<IMember1D>()];
			int num = 0;
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				foreach (LoadCase lc in member1D.Nx.Keys)
				{
					array[num] = Math.Max(array[num], Result.GetMemberUtilization(member1D, lc));
				}
				num++;
			}
			return array;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000057EC File Offset: 0x000039EC
		public static double[] GetUtilization(List<IMember1D> Members, LoadCase LC = null)
		{
			double[] array = new double[Members.Count];
			int num = 0;
			foreach (IMember1D m in Members)
			{
				array[num] = Result.GetMemberUtilization(m, LC);
				num++;
			}
			return array;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000585C File Offset: 0x00003A5C
		public static double GetMemberUtilization(IMember1D M, LoadCase LC)
		{
			Bar bar = M as Bar;
			double result;
			if (bar == null)
			{
				Beam beam = M as Beam;
				if (beam == null)
				{
					result = Result.GetBarUtilization(M as Bar, LC);
				}
				else
				{
					result = Result.GetBeamUtilization(beam, LC);
				}
			}
			else
			{
				result = Result.GetBarUtilization(bar, LC);
			}
			return result;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000058B0 File Offset: 0x00003AB0
		public static double GetBarUtilization(Bar M, LoadCase LC)
		{
			double num = 0.0;
			double num2 = 0.0;
			bool flag = M.Nx.Count == 0;
			double result;
			if (flag)
			{
				result = 0.0;
			}
			else
			{
				bool flag2 = LC == null;
				if (flag2)
				{
					result = 0.0;
				}
				else
				{
					bool flag3 = M.Assignment != null && M.Assignment.ElementGroups.Count != 0;
					if (flag3)
					{
						foreach (ElementGroup elementGroup in M.Assignment.ElementGroups)
						{
							bool flag4 = elementGroup.Type == ElementType.Zero;
							if (flag4)
							{
								return 0.0;
							}
							num += elementGroup.CrossSection.GetTensionResistance(elementGroup.Material);
							num2 += elementGroup.CrossSection.GetBucklingResistance(elementGroup.Material, M.BucklingType, M.BucklingLength).Max();
						}
						double num3 = Math.Max(0.0, M.Nx[LC].Max());
						double num4 = Math.Min(0.0, M.Nx[LC].Min());
						double val = num3 / num;
						double val2 = num4 / num2;
						result = Math.Max(val, val2);
					}
					else
					{
						double num5 = Math.Max(0.0, M.Nx[LC].Max());
						double num6 = Math.Min(0.0, M.Nx[LC].Min());
						num = M.CrossSection.GetTensionResistance(M.Material);
						num2 = M.CrossSection.GetBucklingResistance(M.Material, M.BucklingType, M.BucklingLength).Max();
						double val3 = num5 / num;
						double val4 = num6 / num2;
						result = Math.Max(val3, val4);
					}
				}
			}
			return result;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005ACC File Offset: 0x00003CCC
		public static double GetBeamUtilization(Beam Beam, LoadCase LC)
		{
			bool flag = Beam.Assignment != null && Beam.Assignment.ElementGroups.Count != 0;
			double result;
			if (flag)
			{
				double num = (double)Beam.Assignment.ElementGroups.Count;
				double num2 = 0.0;
				foreach (ElementGroup elementGroup in Beam.Assignment.ElementGroups)
				{
					for (int i = 0; i < Beam.Nx[LC].Count; i++)
					{
						double nx = Beam.Nx[LC][i] / num;
						double vy = Beam.Vy[LC][i] / num;
						double vz = Beam.Vz[LC][i] / num;
						double my = Beam.My[LC][i] / num;
						double mz = Beam.Mz[LC][i] / num;
						double mt = Beam.Mt[LC][i] / num;
						num2 = Math.Max(num2, elementGroup.CrossSection.GetUtilization(elementGroup.Material, Beam.BucklingType, Beam.BucklingLength, false, nx, vy, vz, my, mz, mt));
					}
				}
				result = num2;
			}
			else
			{
				bool flag2 = LC == null;
				if (flag2)
				{
					result = 0.0;
				}
				else
				{
					double num3 = 0.0;
					for (int j = 0; j < Beam.Nx[LC].Count; j++)
					{
						double nx2 = Beam.Nx[LC][j];
						double vy2 = Beam.Vy[LC][j];
						double vz2 = Beam.Vz[LC][j];
						double my2 = Beam.My[LC][j];
						double mz2 = Beam.Mz[LC][j];
						double mt2 = Beam.Mt[LC][j];
						num3 = Math.Max(num3, Beam.CrossSection.GetUtilization(Beam.Material, Beam.BucklingType, Beam.BucklingLength, false, nx2, vy2, vz2, my2, mz2, mt2));
					}
					result = num3;
				}
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005D78 File Offset: 0x00003F78
		public override string ToString()
		{
			string str = "---- R E S U L T S ---- \n \n";
			str = str + "Stock Mass: \t \t \t \t \t \t \t \t \t" + Math.Round(this.StockMass).ToString() + "\n";
			str = str + "Structure Mass: \t \t \t \t " + Math.Round(this.StructureMass).ToString() + "\n";
			str = str + "Reuse Mass: \t \t \t \t \t \t \t \t " + Math.Round(this.ReuseMass).ToString() + "\n";
			str = str + "New Mass: \t \t \t \t \t \t \t \t \t \t " + Math.Round(this.NewMass).ToString() + "\n";
			str = str + "Waste: \t \t \t \t \t \t \t \t \t \t \t \t \t " + Math.Round(this.Waste).ToString() + "\n";
			str += "\n";
			str = str + "Reused Members: \t \t \t \t " + this.ReusedMembers.ToString() + "\n";
			str = str + "New Members: \t \t \t \t \t \t \t " + this.NewMembers.ToString() + "\n";
			str = str + "Total Members: \t \t \t \t \t " + this.TotalMembers.ToString() + "\n";
			str += "\n";
			str = str + "Reuse Rate Mass: \t \t \t " + this.ReuseRateMass.ToString() + "\n";
			str = str + "Reuse Rate Members: " + this.ReuseRateMembers.ToString() + "\n";
			str += "\n";
			return str + "Environmental Impact: " + Math.Round(this.EnvironmentalImpact).ToString() + "\n";
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005F28 File Offset: 0x00004128
		public Result Clone()
		{
			return new Result
			{
				StockMass = this.StockMass,
				StructureMass = this.StructureMass,
				ReuseMass = this.ReuseMass,
				NewMass = this.NewMass,
				Waste = this.Waste,
				EnvironmentalImpact = this.EnvironmentalImpact,
				ReusedMembers = this.ReusedMembers,
				NewMembers = this.NewMembers,
				TotalMembers = this.TotalMembers,
				ReuseRateMass = this.ReuseRateMass,
				ReuseRateMembers = this.ReuseRateMembers,
				MaxMemberMass = this.MaxMemberMass,
				MaxMemberImpact = this.MaxMemberImpact,
				Utilization = (double[])this.Utilization.Clone()
			};
		}
	}
}
