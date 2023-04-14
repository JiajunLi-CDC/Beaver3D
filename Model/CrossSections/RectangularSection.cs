using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000034 RID: 52
	public class RectangularSection : ICrossSection
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0001468A File Offset: 0x0001288A
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00014692 File Offset: 0x00012892
		public string TypeName { get; private set; } = "RS";

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0001469B File Offset: 0x0001289B
		// (set) Token: 0x06000356 RID: 854 RVA: 0x000146A3 File Offset: 0x000128A3
		public string Name { get; private set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000357 RID: 855 RVA: 0x000146AC File Offset: 0x000128AC
		// (set) Token: 0x06000358 RID: 856 RVA: 0x000146B4 File Offset: 0x000128B4
		public double Height { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000359 RID: 857 RVA: 0x000146BD File Offset: 0x000128BD
		// (set) Token: 0x0600035A RID: 858 RVA: 0x000146C5 File Offset: 0x000128C5
		public double Width { get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600035B RID: 859 RVA: 0x000146CE File Offset: 0x000128CE
		// (set) Token: 0x0600035C RID: 860 RVA: 0x000146D6 File Offset: 0x000128D6
		public double Area { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600035D RID: 861 RVA: 0x000146DF File Offset: 0x000128DF
		// (set) Token: 0x0600035E RID: 862 RVA: 0x000146E7 File Offset: 0x000128E7
		public double Iy { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000146F0 File Offset: 0x000128F0
		// (set) Token: 0x06000360 RID: 864 RVA: 0x000146F8 File Offset: 0x000128F8
		public double Iz { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00014701 File Offset: 0x00012901
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00014709 File Offset: 0x00012909
		public double Wy { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00014712 File Offset: 0x00012912
		// (set) Token: 0x06000364 RID: 868 RVA: 0x0001471A File Offset: 0x0001291A
		public double Wz { get; private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00014723 File Offset: 0x00012923
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0001472B File Offset: 0x0001292B
		public double Wypl { get; private set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00014734 File Offset: 0x00012934
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0001473C File Offset: 0x0001293C
		public double Wzpl { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00014745 File Offset: 0x00012945
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0001474D File Offset: 0x0001294D
		public double iy { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00014756 File Offset: 0x00012956
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0001475E File Offset: 0x0001295E
		public double iz { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00014767 File Offset: 0x00012967
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0001476F File Offset: 0x0001296F
		public double It { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00014778 File Offset: 0x00012978
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00014780 File Offset: 0x00012980
		public double Wt { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00014789 File Offset: 0x00012989
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00014791 File Offset: 0x00012991
		public double Avy { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0001479A File Offset: 0x0001299A
		// (set) Token: 0x06000374 RID: 884 RVA: 0x000147A2 File Offset: 0x000129A2
		public double Avz { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000375 RID: 885 RVA: 0x000147AB File Offset: 0x000129AB
		// (set) Token: 0x06000376 RID: 886 RVA: 0x000147B3 File Offset: 0x000129B3
		public double dy { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000377 RID: 887 RVA: 0x000147BC File Offset: 0x000129BC
		// (set) Token: 0x06000378 RID: 888 RVA: 0x000147C4 File Offset: 0x000129C4
		public double dz { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000147CD File Offset: 0x000129CD
		// (set) Token: 0x0600037A RID: 890 RVA: 0x000147D5 File Offset: 0x000129D5
		public double cy { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600037B RID: 891 RVA: 0x000147DE File Offset: 0x000129DE
		// (set) Token: 0x0600037C RID: 892 RVA: 0x000147E6 File Offset: 0x000129E6
		public double cz { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000147EF File Offset: 0x000129EF
		// (set) Token: 0x0600037E RID: 894 RVA: 0x000147F7 File Offset: 0x000129F7
		public List<int> PossibleCompounds { get; private set; } = new List<int>
		{
			1
		};

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00014800 File Offset: 0x00012A00
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00014808 File Offset: 0x00012A08
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x06000381 RID: 897 RVA: 0x00014814 File Offset: 0x00012A14
		public RectangularSection(double Height = 0.05, double Width = 0.03)
		{
			this.Name = "R " + Math.Round(1000.0 * Height, 2).ToString() + "x" + Math.Round(1000.0 * Width, 2).ToString();
			this.Height = Height;
			this.Width = Width;
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000148B0 File Offset: 0x00012AB0
		public void SetSectionProperties()
		{
			this.Area = this.Height * this.Width;
			this.Iy = this.Height * this.Height * this.Height * this.Width / 12.0;
			this.Iz = this.Height * this.Width * this.Width * this.Width / 12.0;
			this.iy = Math.Sqrt(this.Iy / this.Area);
			this.iz = Math.Sqrt(this.Iz / this.Area);
			this.Wy = this.Height * this.Height * this.Width / 6.0;
			this.Wz = this.Height * this.Width * this.Width / 6.0;
			this.Wypl = this.Height * this.Height * this.Width / 4.0;
			this.Wzpl = this.Height * this.Width * this.Width / 4.0;
			this.Avz = (this.Avy = this.Area);
			double num = (1.0 - 0.63 / (this.Height / this.Width) + 0.052 / Math.Pow(this.Height / this.Width, 5.0)) / 3.0;
			double num2 = 1.0 - 0.65 / (1.0 + Math.Pow(this.Height / this.Width, 3.0));
			this.It = num * this.Height * this.Width * this.Width * this.Width;
			this.Wt = num / num2 * this.Height * this.Width * this.Width;
			this.cy = this.Width / 2.0;
			this.cz = this.Height / 2.0;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00014B04 File Offset: 0x00012D04
		public void SetPolygon()
		{
			this.Polygon = new List<ValueTuple<double, double>>();
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, this.Height - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, this.Height - this.cz));
			this.dy = (from i in this.Polygon
			select i.Item1).Max() - (from i in this.Polygon
			select i.Item1).Min();
			this.dz = (from i in this.Polygon
			select i.Item2).Max() - (from i in this.Polygon
			select i.Item2).Min();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00014CA2 File Offset: 0x00012EA2
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00014CC8 File Offset: 0x00012EC8
		public double GetTensionResistance(IMaterial Material)
		{
			double result;
			switch (Material.Type)
			{
			case MaterialType.Empty:
				result = 0.0;
				break;
			case MaterialType.Metal:
				result = this.Area * Material.ft / Material.gamma_0;
				break;
			case MaterialType.Timber:
				result = Material.kmod * this.Area * Material.ft / Material.gamma_0;
				break;
			default:
				result = this.Area * Material.ft / Material.gamma_0;
				break;
			}
			return result;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00014D4C File Offset: 0x00012F4C
		public List<double> GetBucklingResistance(IMaterial Material, BucklingType BucklingType, double BucklingLength)
		{
			List<double> result;
			switch (Material.Type)
			{
			case MaterialType.Empty:
				result = new List<double>
				{
					0.0
				};
				break;
			case MaterialType.Metal:
				switch (BucklingType)
				{
				case BucklingType.Off:
					result = new List<double>
					{
						-this.Area * Material.fc / Material.gamma_0
					};
					break;
				case BucklingType.Euler:
				{
					double item = -9.869604401089358 * Material.E * this.Iy / BucklingLength / BucklingLength / Material.gamma_1;
					double item2 = -9.869604401089358 * Material.E * this.Iz / BucklingLength / BucklingLength / Material.gamma_1;
					result = new List<double>
					{
						item,
						item2
					};
					break;
				}
				case BucklingType.Eurocode:
				{
					double num = 0.21;
					double num2 = Math.Sqrt(this.Iy / this.Area);
					double num3 = Math.Sqrt(this.Iz / this.Area);
					double num4 = 3.1415926535897931 * Math.Sqrt(Material.E / Material.fc);
					double num5 = BucklingLength / num2 / num4;
					double num6 = BucklingLength / num3 / num4;
					double num7 = 0.5 * (1.0 + num * (num5 - 0.2) + num5 * num5);
					double num8 = 0.5 * (1.0 + num * (num6 - 0.2) + num6 * num6);
					double num9 = Math.Min(1.0, 1.0 / (num7 + Math.Sqrt(num7 * num7 - num5 * num5)));
					double num10 = Math.Min(1.0, 1.0 / (num8 + Math.Sqrt(num8 * num8 - num6 * num6)));
					double item3 = -num9 * this.Area * Material.fc / Material.gamma_1;
					double item4 = -num10 * this.Area * Material.fc / Material.gamma_1;
					result = new List<double>
					{
						item3,
						item4
					};
					break;
				}
				default:
					result = new List<double>
					{
						-this.Area * Material.fc / Material.gamma_0
					};
					break;
				}
				break;
			case MaterialType.Timber:
				switch (BucklingType)
				{
				case BucklingType.Off:
					result = new List<double>
					{
						-this.Area * Material.fc / Material.gamma_0
					};
					break;
				case BucklingType.Euler:
				{
					double item5 = -9.869604401089358 * Material.E * this.Iy / BucklingLength / BucklingLength / Material.gamma_1;
					double item6 = -9.869604401089358 * Material.E * this.Iz / BucklingLength / BucklingLength / Material.gamma_1;
					result = new List<double>
					{
						-this.Area * Material.fc / Material.gamma_0,
						item5,
						item6
					};
					break;
				}
				case BucklingType.Eurocode:
				{
					double num11 = 0.2;
					double num12 = Material.E * 2.0 / 3.0;
					double num13 = BucklingLength / this.iy;
					double num14 = BucklingLength / this.iz;
					double num15 = num13 / 3.1415926535897931 * Math.Sqrt(Material.fc / num12);
					double num16 = num14 / 3.1415926535897931 * Math.Sqrt(Material.fc / num12);
					double num17 = 0.5 * (1.0 + num11 * (num15 - 0.3) + num15 * num15);
					double num18 = 0.5 * (1.0 + num11 * (num16 - 0.3) + num16 * num16);
					double num19 = 1.0 / (num17 + Math.Sqrt(num17 * num17 - num15 * num15));
					double num20 = 1.0 / (num18 + Math.Sqrt(num18 * num18 - num16 * num16));
					double item7 = -num19 * Material.kmod * this.Area * Material.fc / Material.gamma_1;
					double item8 = -num20 * Material.kmod * this.Area * Material.fc / Material.gamma_1;
					result = new List<double>
					{
						item7,
						item8
					};
					break;
				}
				default:
					result = new List<double>
					{
						-this.Area * Material.fc / Material.gamma_0
					};
					break;
				}
				break;
			default:
				result = new List<double>
				{
					-this.Area * Material.fc / Material.gamma_0
				};
				break;
			}
			return result;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001524C File Offset: 0x0001344C
		public double GetUtilization(IMaterial Material, BucklingType BucklingType, double BucklingLength, bool Plastic, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			bool flag = Nx >= 0.0 && My == 0.0 && Mz == 0.0;
			double result;
			if (flag)
			{
				result = Nx / this.GetTensionResistance(Material);
			}
			else
			{
				if (Material is Steel)
				{
					Steel steel = (Steel)Material;
					if (Plastic)
					{
						throw new NotImplementedException("Eurocode plastic beam utilization not implemented yet");
					}
					switch (BucklingType)
					{
					case BucklingType.Off:
						return this.UtilizationElasticNoStability(steel, Nx, Vy, Vz, My, Mz, Mt);
					case BucklingType.Euler:
						return this.UtilizationElasticNoStability(steel, Nx, Vy, Vz, My, Mz, Mt);
					case BucklingType.Eurocode:
						throw new NotImplementedException("Eurocode steel bending and buckling not implemented");
					}
				}
				else if (Material is Timber)
				{
					Timber timber = (Timber)Material;
					throw new NotImplementedException("HEA beams in timber do not exist!");
				}
				result = 1000.0;
			}
			return result;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00015360 File Offset: 0x00013560
		private double UtilizationElasticNoStability(Steel Steel, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			bool flag = (My == 0.0 && Vz == 0.0) || (Mz == 0.0 && Vy == 0.0);
			double result;
			if (flag)
			{
				result = this.UtilizationElasticNoStabilityUniaxial(Steel, Nx, Vy, Vz, My, Mz, Mt);
			}
			else
			{
				result = this.UtilizationElasticNoStabilityBiaxial(Steel, Nx, Vy, Vz, My, Mz, Mt);
			}
			return result;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000153D8 File Offset: 0x000135D8
		private double UtilizationElasticNoStabilityUniaxial(Steel Steel, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			bool flag = Mz == 0.0 && Vy == 0.0;
			double result;
			if (flag)
			{
				double num = this.Avz * Steel.ft / Math.Sqrt(3.0) / Steel.gamma_0;
				bool flag2 = Math.Abs(Vz) <= 0.5 * num;
				double num2;
				if (flag2)
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Min(0.9999, Math.Pow(2.0 * Math.Abs(Vz) / num - 1.0, 2.0));
				}
				double num3 = Math.Max(Math.Abs(Nx / this.Area + My / this.Wy), Math.Abs(Nx / this.Area - My / this.Wy));
				num3 /= (1.0 - num2) * Steel.fc / Steel.gamma_0;
				result = num3;
			}
			else
			{
				bool flag3 = My == 0.0 && Vz == 0.0;
				if (flag3)
				{
					double num4 = this.Avy * Steel.ft / Math.Sqrt(3.0) / Steel.gamma_0;
					bool flag4 = Math.Abs(Vy) <= 0.5 * num4;
					double num2;
					if (flag4)
					{
						num2 = 0.0;
					}
					else
					{
						num2 = Math.Min(0.9999, Math.Pow(2.0 * Math.Abs(Vy) / num4 - 1.0, 2.0));
					}
					double num3 = Math.Max(Math.Abs(Nx / this.Area + My / this.Wy), Math.Abs(Nx / this.Area - My / this.Wy));
					num3 /= (1.0 - num2) * Steel.fc / Steel.gamma_0;
					result = num3;
				}
				else
				{
					result = this.UtilizationElasticNoStabilityBiaxial(Steel, Nx, Vy, Vz, My, Mz, Mt);
				}
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00015618 File Offset: 0x00013818
		private double UtilizationElasticNoStabilityBiaxial(Steel Steel, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			double[] array = new double[4];
			double num = Nx / this.Area;
			double num2 = My / this.Wy;
			double num3 = Mz / this.Wz;
			double num4 = Vy / this.Avy;
			double num5 = Vz / this.Avz;
			double num6 = this.Avy * Steel.fc / Math.Sqrt(3.0) / Steel.gamma_0;
			double num7 = this.Avz * Steel.fc / Math.Sqrt(3.0) / Steel.gamma_0;
			bool flag = Math.Abs(Vy) <= 0.5 * num6 && Math.Abs(Vz) <= num7;
			double result;
			if (flag)
			{
				array[0] = num + num2 + num3;
				array[1] = num + num2 - num3;
				array[2] = num - num2 + num3;
				array[3] = num - num2 - num3;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag2 = array[i] >= 0.0;
					if (flag2)
					{
						array[i] /= Steel.ft / Steel.gamma_0;
					}
					else
					{
						array[i] /= -Steel.fc / Steel.gamma_0;
					}
					array[i] = Math.Abs(array[i]);
				}
				result = array.Max();
			}
			else
			{
				array[0] = num + num2 + num3;
				array[1] = num + num2 - num3;
				array[2] = num - num2 + num3;
				array[3] = num - num2 - num3;
				for (int j = 0; j < array.Length; j++)
				{
					bool flag3 = array[j] >= 0.0;
					if (flag3)
					{
						array[j] /= Steel.ft / Steel.gamma_0;
					}
					else
					{
						array[j] /= -Steel.fc / Steel.gamma_0;
					}
					array[j] = array[j] * array[j] + 3.0 * (num4 / Steel.ft / Steel.gamma_0) * (num4 / Steel.ft / Steel.gamma_0) + 3.0 * (num5 / Steel.ft / Steel.gamma_0) * (num5 / Steel.ft / Steel.gamma_0);
					array[j] = Math.Abs(array[j]);
				}
				result = array.Max();
			}
			return result;
		}
	}
}
