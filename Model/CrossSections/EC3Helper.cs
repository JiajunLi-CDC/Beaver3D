using System;
using System.Collections.Generic;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000033 RID: 51
	public static class EC3Helper
	{
		// Token: 0x06000350 RID: 848 RVA: 0x00014028 File Offset: 0x00012228
		public static List<double> QK3_Util_NoStability(ICrossSection CS, Steel Material, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			bool flag = My == 0.0 || Mz == 0.0;
			List<double> result;
			if (flag)
			{
				result = EC3Helper.QK3_uniaxial(CS, Material, Nx, Vy, Vz, My, Mz, Mt);
			}
			else
			{
				result = EC3Helper.QK3_biaxial(CS, Material, Nx, Vy, Vz, My, Mz, Mt);
			}
			return result;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00014084 File Offset: 0x00012284
		public static List<double> QK3_uniaxial(ICrossSection CS, Steel Material, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			List<double> list = new List<double>();
			bool flag = Mz == 0.0;
			List<double> result;
			if (flag)
			{
				double num = CS.Avz * Material.ft / Math.Sqrt(3.0) / Material.gamma_0;
				bool flag2 = Vz <= num;
				double num2;
				if (flag2)
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Pow(2.0 * Vy / num - 1.0, 2.0);
				}
				list.Add(Nx / CS.Area + My / CS.Wy);
				list.Add(Nx / CS.Area - My / CS.Wy);
				for (int i = 0; i < list.Count; i++)
				{
					bool flag3 = list[i] >= 0.0;
					if (flag3)
					{
						double num3 = (1.0 - num2) * Material.ft;
						List<double> list2 = list;
						int index = i;
						list2[index] /= num3 / Material.gamma_0;
					}
					else
					{
						double num4 = (1.0 - num2) * Material.fc;
						List<double> list2 = list;
						int index = i;
						list2[index] /= -num4 / Material.gamma_0;
					}
				}
				result = list;
			}
			else
			{
				bool flag4 = My == 0.0;
				if (flag4)
				{
					double num5 = CS.Avy * Material.ft / Math.Sqrt(3.0) / Material.gamma_0;
					bool flag5 = Vy <= num5;
					double num2;
					if (flag5)
					{
						num2 = 0.0;
					}
					else
					{
						num2 = Math.Pow(2.0 * Vy / num5 - 1.0, 2.0);
					}
					list.Add(Nx / (CS.Area - CS.Avy * num2) + Mz / (CS.Wz * (1.0 - num2)));
					list.Add(Nx / (CS.Area - CS.Avy * num2) - Mz / (CS.Wz * (1.0 - num2)));
					for (int j = 0; j < list.Count; j++)
					{
						bool flag6 = list[j] >= 0.0;
						if (flag6)
						{
							double num6 = (1.0 - num2) * Material.ft;
							List<double> list2 = list;
							int index = j;
							list2[index] /= num6 / Material.gamma_0;
						}
						else
						{
							double num7 = (1.0 - num2) * Material.fc;
							List<double> list2 = list;
							int index = j;
							list2[index] /= -num7 / Material.gamma_0;
						}
					}
					result = list;
				}
				else
				{
					result = EC3Helper.QK3_biaxial(CS, Material, Nx, Vy, Vz, My, Mz, Mt);
				}
			}
			return result;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000143C4 File Offset: 0x000125C4
		public static List<double> QK3_biaxial(ICrossSection CS, Steel Material, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			List<double> list = new List<double>();
			double num = Nx / CS.Area;
			double num2 = My / CS.Wy;
			double num3 = Mz / CS.Wz;
			bool flag = Vy <= 0.5 * CS.Avy * Material.ft / Math.Sqrt(3.0) / Material.gamma_0 && Vz <= CS.Avz * Material.ft / Math.Sqrt(3.0) / Material.gamma_0;
			List<double> result;
			if (flag)
			{
				list.Add(num + num2 + num3);
				list.Add(num + num2 - num3);
				list.Add(num - num2 + num3);
				list.Add(num - num2 - num3);
				for (int i = 0; i < 4; i++)
				{
					bool flag2 = list[i] >= 0.0;
					if (flag2)
					{
						List<double> list2 = list;
						int index = i;
						list2[index] /= Material.ft / Material.gamma_0;
					}
					else
					{
						List<double> list2 = list;
						int index = i;
						list2[index] /= -Material.fc / Material.gamma_0;
					}
				}
				result = list;
			}
			else
			{
				list.Add(num + num2 + num3);
				list.Add(num + num2 - num3);
				list.Add(num - num2 + num3);
				list.Add(num - num2 - num3);
				for (int j = 0; j < 4; j++)
				{
					bool flag3 = list[j] >= 0.0;
					if (flag3)
					{
						List<double> list2 = list;
						int index = j;
						list2[index] /= Material.ft / Material.gamma_0;
					}
					else
					{
						List<double> list2 = list;
						int index = j;
						list2[index] /= -Material.fc / Material.gamma_0;
					}
					list[j] = list[j] * list[j] + 3.0 * (Vy / CS.Avy / (Material.ft / Material.gamma_0)) * (Vy / CS.Avy / (Material.ft / Material.gamma_0)) + 3.0 * (Vz / CS.Avz / (Material.ft / Material.gamma_0)) * (Vy / CS.Avz / (Material.ft / Material.gamma_0));
				}
				result = list;
			}
			return result;
		}
	}
}
