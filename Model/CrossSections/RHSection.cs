using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beaver3D.Model.Materials;
using Beaver3D.Properties;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000038 RID: 56
	public class RHSection : ICrossSection
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00017751 File Offset: 0x00015951
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x00017759 File Offset: 0x00015959
		public string TypeName { get; private set; } = "RHS";

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00017762 File Offset: 0x00015962
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0001776A File Offset: 0x0001596A
		public string Name { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00017773 File Offset: 0x00015973
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0001777B File Offset: 0x0001597B
		public double Width { get; private set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00017784 File Offset: 0x00015984
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0001778C File Offset: 0x0001598C
		public double Height { get; private set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00017795 File Offset: 0x00015995
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0001779D File Offset: 0x0001599D
		public double Thickness { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000177A6 File Offset: 0x000159A6
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x000177AE File Offset: 0x000159AE
		public double cty { get; private set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x000177B7 File Offset: 0x000159B7
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x000177BF File Offset: 0x000159BF
		public double ctz { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x000177C8 File Offset: 0x000159C8
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x000177D0 File Offset: 0x000159D0
		public double Area { get; private set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x000177D9 File Offset: 0x000159D9
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x000177E1 File Offset: 0x000159E1
		public double Surface { get; private set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x000177EA File Offset: 0x000159EA
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x000177F2 File Offset: 0x000159F2
		public double cy { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x000177FB File Offset: 0x000159FB
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00017803 File Offset: 0x00015A03
		public double cz { get; private set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001780C File Offset: 0x00015A0C
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00017814 File Offset: 0x00015A14
		public double Iy { get; private set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0001781D File Offset: 0x00015A1D
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00017825 File Offset: 0x00015A25
		public double Iz { get; private set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001782E File Offset: 0x00015A2E
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00017836 File Offset: 0x00015A36
		public double iy { get; private set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0001783F File Offset: 0x00015A3F
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00017847 File Offset: 0x00015A47
		public double iz { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00017850 File Offset: 0x00015A50
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00017858 File Offset: 0x00015A58
		public double Wy { get; private set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00017861 File Offset: 0x00015A61
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00017869 File Offset: 0x00015A69
		public double Wz { get; private set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00017872 File Offset: 0x00015A72
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0001787A File Offset: 0x00015A7A
		public double Wypl { get; private set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00017883 File Offset: 0x00015A83
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0001788B File Offset: 0x00015A8B
		public double Wzpl { get; private set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00017894 File Offset: 0x00015A94
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001789C File Offset: 0x00015A9C
		public double It { get; private set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x000178A5 File Offset: 0x00015AA5
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x000178AD File Offset: 0x00015AAD
		public double Wt { get; private set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000178B6 File Offset: 0x00015AB6
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x000178BE File Offset: 0x00015ABE
		public double Avy { get; private set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000178C7 File Offset: 0x00015AC7
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x000178CF File Offset: 0x00015ACF
		public double Avz { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000178D8 File Offset: 0x00015AD8
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x000178E0 File Offset: 0x00015AE0
		public List<int> PossibleCompounds { get; private set; } = new List<int>
		{
			1
		};

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000178E9 File Offset: 0x00015AE9
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x000178F1 File Offset: 0x00015AF1
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000178FA File Offset: 0x00015AFA
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00017902 File Offset: 0x00015B02
		public double dy { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001790B File Offset: 0x00015B0B
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00017913 File Offset: 0x00015B13
		public double dz { get; private set; }

		// Token: 0x06000458 RID: 1112 RVA: 0x0001791C File Offset: 0x00015B1C
		public RHSection(double Height, double Width, double Thickness)
		{
			this.Height = Height;
			this.Width = Width;
			this.Thickness = Thickness;
			this.Name = string.Concat(new string[]
			{
				"RHS ",
				(1000.0 * Height).ToString(),
				"x",
				(1000.0 * Width).ToString(),
				"x",
				(1000.0 * Thickness).ToString()
			});
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000179E4 File Offset: 0x00015BE4
		public void SetSectionProperties()
		{
			bool flag = false;
			string[] array = Resources.RHSections.Split(new char[]
			{
				'\r',
				'\n'
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 1; i < array.Length; i++)
			{
				string text = array[i];
				string[] array2 = text.Split(new char[]
				{
					';'
				});
				bool flag2 = Convert.ToDouble(array2[1], new CultureInfo("en-US")) == this.Height && Convert.ToDouble(array2[2], new CultureInfo("en-US")) == this.Width && Convert.ToDouble(array2[3], new CultureInfo("en-US")) == this.Thickness;
				if (flag2)
				{
					this.Area = double.Parse(array2[4], new CultureInfo("en-US"));
					this.cty = double.Parse(array2[5], new CultureInfo("en-US"));
					this.ctz = double.Parse(array2[6], new CultureInfo("en-US"));
					this.Iy = double.Parse(array2[7], new CultureInfo("en-US"));
					this.Iz = double.Parse(array2[8], new CultureInfo("en-US"));
					this.iy = double.Parse(array2[9], new CultureInfo("en-US"));
					this.iz = double.Parse(array2[10], new CultureInfo("en-US"));
					this.Wy = double.Parse(array2[11], new CultureInfo("en-US"));
					this.Wz = double.Parse(array2[12], new CultureInfo("en-US"));
					this.Wypl = double.Parse(array2[13], new CultureInfo("en-US"));
					this.Wypl = double.Parse(array2[14], new CultureInfo("en-US"));
					this.It = double.Parse(array2[15], new CultureInfo("en-US"));
					this.Wt = double.Parse(array2[16], new CultureInfo("en-US"));
					this.Surface = double.Parse(array2[17], new CultureInfo("en-US"));
					this.cy = this.Width / 2.0;
					this.cz = this.Height / 2.0;
					this.Avy = 2.0 * this.Width * this.Thickness;
					this.Avz = 2.0 * this.Height * this.Thickness;
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				throw new Exception("No RHS-Section with the given dimension could be found in the standard table");
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00017C9C File Offset: 0x00015E9C
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

		// Token: 0x0600045B RID: 1115 RVA: 0x00017E3A File Offset: 0x0001603A
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00017E48 File Offset: 0x00016048
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00017E60 File Offset: 0x00016060
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

		// Token: 0x0600045E RID: 1118 RVA: 0x00017EE4 File Offset: 0x000160E4
		public List<double> GetBucklingResistance(IMaterial Material, BucklingType BucklingType, double BucklingLength)
		{
			List<double> result;
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
					-this.Area * Material.fc / Material.gamma_0,
					item,
					item2
				};
				break;
			}
			case BucklingType.Eurocode:
			{
				double num = 0.21;
				double num2 = 0.21;
				double num3 = Math.Sqrt(this.Iy / this.Area);
				double num4 = Math.Sqrt(this.Iz / this.Area);
				double num5 = 3.1415926535897931 * Math.Sqrt(Material.E / Material.fc);
				double num6 = BucklingLength / num3 / num5;
				double num7 = BucklingLength / num4 / num5;
				double num8 = 0.5 * (1.0 + num * (num6 - 0.2) + num6 * num6);
				double num9 = 0.5 * (1.0 + num2 * (num7 - 0.2) + num7 * num7);
				double num10 = Math.Min(1.0, 1.0 / (num8 + Math.Sqrt(num8 * num8 - num6 * num6)));
				double num11 = Math.Min(1.0, 1.0 / (num9 + Math.Sqrt(num9 * num9 - num7 * num7)));
				double item3 = -num10 * this.Area * Material.fc / Material.gamma_1;
				double item4 = -num11 * this.Area * Material.fc / Material.gamma_1;
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
			return result;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00018148 File Offset: 0x00016348
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

		// Token: 0x06000460 RID: 1120 RVA: 0x0001825C File Offset: 0x0001645C
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

		// Token: 0x06000461 RID: 1121 RVA: 0x000182D4 File Offset: 0x000164D4
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

		// Token: 0x06000462 RID: 1122 RVA: 0x00018514 File Offset: 0x00016714
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
