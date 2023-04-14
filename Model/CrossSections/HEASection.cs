using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beaver3D.Model.Materials;
using Beaver3D.Properties;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000037 RID: 55
	public class HEASection : ICrossSection
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001654D File Offset: 0x0001474D
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00016555 File Offset: 0x00014755
		public string TypeName { get; private set; } = "HEA";

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001655E File Offset: 0x0001475E
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x00016566 File Offset: 0x00014766
		public string Name { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001656F File Offset: 0x0001476F
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00016577 File Offset: 0x00014777
		public double Width { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00016580 File Offset: 0x00014780
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00016588 File Offset: 0x00014788
		public double Height { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00016591 File Offset: 0x00014791
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00016599 File Offset: 0x00014799
		public double WebThickness { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000165A2 File Offset: 0x000147A2
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x000165AA File Offset: 0x000147AA
		public double FlangeThickness { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000165B3 File Offset: 0x000147B3
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x000165BB File Offset: 0x000147BB
		public double CornerRadius { get; private set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000165C4 File Offset: 0x000147C4
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000165CC File Offset: 0x000147CC
		public double Area { get; private set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000165D5 File Offset: 0x000147D5
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000165DD File Offset: 0x000147DD
		public double Surface { get; private set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000165E6 File Offset: 0x000147E6
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x000165EE File Offset: 0x000147EE
		public double cy { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000165F7 File Offset: 0x000147F7
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x000165FF File Offset: 0x000147FF
		public double cz { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00016608 File Offset: 0x00014808
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00016610 File Offset: 0x00014810
		public double Iy { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00016619 File Offset: 0x00014819
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00016621 File Offset: 0x00014821
		public double Iz { get; private set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0001662A File Offset: 0x0001482A
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00016632 File Offset: 0x00014832
		public double iy { get; private set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001663B File Offset: 0x0001483B
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00016643 File Offset: 0x00014843
		public double iz { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001664C File Offset: 0x0001484C
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x00016654 File Offset: 0x00014854
		public double Wy { get; private set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0001665D File Offset: 0x0001485D
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x00016665 File Offset: 0x00014865
		public double Wz { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0001666E File Offset: 0x0001486E
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x00016676 File Offset: 0x00014876
		public double Wypl { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0001667F File Offset: 0x0001487F
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00016687 File Offset: 0x00014887
		public double Wzpl { get; private set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00016690 File Offset: 0x00014890
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00016698 File Offset: 0x00014898
		public double Avy { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x000166A1 File Offset: 0x000148A1
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x000166A9 File Offset: 0x000148A9
		public double Avz { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x000166B2 File Offset: 0x000148B2
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x000166BA File Offset: 0x000148BA
		public double It { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000166C3 File Offset: 0x000148C3
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000166CB File Offset: 0x000148CB
		public double Wt { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000166D4 File Offset: 0x000148D4
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000166DC File Offset: 0x000148DC
		public List<int> PossibleCompounds { get; private set; } = new List<int>
		{
			1
		};

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000166E5 File Offset: 0x000148E5
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x000166ED File Offset: 0x000148ED
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x000166F6 File Offset: 0x000148F6
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x000166FE File Offset: 0x000148FE
		public double dy { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00016707 File Offset: 0x00014907
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0001670F File Offset: 0x0001490F
		public double dz { get; private set; }

		// Token: 0x06000417 RID: 1047 RVA: 0x00016718 File Offset: 0x00014918
		public HEASection(int Size = 100)
		{
			this.Name = "HEA " + Size.ToString();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00016774 File Offset: 0x00014974
		public void SetSectionProperties()
		{
			bool flag = false;
			string[] array = Resources.HEASections.Split(new char[]
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
				bool flag2 = array2[0] == this.Name;
				if (flag2)
				{
					this.Height = double.Parse(array2[1], new CultureInfo("en-US"));
					this.Width = double.Parse(array2[2], new CultureInfo("en-US"));
					this.WebThickness = double.Parse(array2[3], new CultureInfo("en-US"));
					this.FlangeThickness = double.Parse(array2[4], new CultureInfo("en-US"));
					this.CornerRadius = double.Parse(array2[5], new CultureInfo("en-US"));
					this.Area = double.Parse(array2[6], new CultureInfo("en-US"));
					this.Surface = double.Parse(array2[7], new CultureInfo("en-US"));
					this.Iy = double.Parse(array2[8], new CultureInfo("en-US"));
					this.Wy = double.Parse(array2[9], new CultureInfo("en-US"));
					this.Wypl = double.Parse(array2[10], new CultureInfo("en-US"));
					this.iy = double.Parse(array2[11], new CultureInfo("en-US"));
					this.Avz = double.Parse(array2[12], new CultureInfo("en-US"));
					this.Iz = double.Parse(array2[13], new CultureInfo("en-US"));
					this.Wz = double.Parse(array2[14], new CultureInfo("en-US"));
					this.Wzpl = double.Parse(array2[15], new CultureInfo("en-US"));
					this.iz = double.Parse(array2[16], new CultureInfo("en-US"));
					this.It = double.Parse(array2[18], new CultureInfo("en-US"));
					this.Wt = double.Parse(array2[19], new CultureInfo("en-US"));
					this.Avy = 2.0 * this.Width * this.FlangeThickness;
					this.cy = this.Width / 2.0;
					this.cz = this.Height / 2.0;
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				throw new Exception("No HEA-Section with the given dimension could be found in the standard table");
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00016A30 File Offset: 0x00014C30
		public void SetPolygon()
		{
			this.Polygon = new List<ValueTuple<double, double>>();
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width / 2.0 + this.WebThickness / 2.0 - this.cy, this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width / 2.0 + this.WebThickness / 2.0 - this.cy, this.Height - this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, this.Height - this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, this.Height - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, this.Height - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, this.Height - this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width / 2.0 - this.WebThickness / 2.0 - this.cy, this.Height - this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width / 2.0 - this.WebThickness / 2.0 - this.cy, this.FlangeThickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, this.FlangeThickness - this.cz));
			this.dy = (from i in this.Polygon
			select i.Item1).Max() - (from i in this.Polygon
			select i.Item1).Min();
			this.dz = (from i in this.Polygon
			select i.Item2).Max() - (from i in this.Polygon
			select i.Item2).Min();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00016DB4 File Offset: 0x00014FB4
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00016DC0 File Offset: 0x00014FC0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00016DD8 File Offset: 0x00014FD8
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

		// Token: 0x0600041D RID: 1053 RVA: 0x00016E5C File Offset: 0x0001505C
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
				bool flag = this.Height / this.Width > 1.2;
				double num;
				double num2;
				if (flag)
				{
					num = 0.21;
					num2 = 0.34;
				}
				else
				{
					num = 0.34;
					num2 = 0.49;
				}
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

		// Token: 0x0600041E RID: 1054 RVA: 0x000170FC File Offset: 0x000152FC
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

		// Token: 0x0600041F RID: 1055 RVA: 0x00017210 File Offset: 0x00015410
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

		// Token: 0x06000420 RID: 1056 RVA: 0x00017288 File Offset: 0x00015488
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

		// Token: 0x06000421 RID: 1057 RVA: 0x000174C8 File Offset: 0x000156C8
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
