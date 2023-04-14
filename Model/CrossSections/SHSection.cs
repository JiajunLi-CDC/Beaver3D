using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beaver3D.Model.Materials;
using Beaver3D.Properties;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000039 RID: 57
	public class SHSection : ICrossSection
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001879D File Offset: 0x0001699D
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x000187A5 File Offset: 0x000169A5
		public string TypeName { get; private set; } = "SHS";

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000187AE File Offset: 0x000169AE
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x000187B6 File Offset: 0x000169B6
		public string Name { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000187BF File Offset: 0x000169BF
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x000187C7 File Offset: 0x000169C7
		public double Width { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000187D0 File Offset: 0x000169D0
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x000187D8 File Offset: 0x000169D8
		public double Height { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000187E1 File Offset: 0x000169E1
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x000187E9 File Offset: 0x000169E9
		public double Thickness { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x000187F2 File Offset: 0x000169F2
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x000187FA File Offset: 0x000169FA
		public double ct { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00018803 File Offset: 0x00016A03
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x0001880B File Offset: 0x00016A0B
		public double Area { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00018814 File Offset: 0x00016A14
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x0001881C File Offset: 0x00016A1C
		public double Surface { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00018825 File Offset: 0x00016A25
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x0001882D File Offset: 0x00016A2D
		public double cy { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00018836 File Offset: 0x00016A36
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x0001883E File Offset: 0x00016A3E
		public double cz { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00018847 File Offset: 0x00016A47
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x0001884F File Offset: 0x00016A4F
		public double Iy { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00018858 File Offset: 0x00016A58
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x00018860 File Offset: 0x00016A60
		public double Iz { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00018869 File Offset: 0x00016A69
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x00018871 File Offset: 0x00016A71
		public double iy { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0001887A File Offset: 0x00016A7A
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x00018882 File Offset: 0x00016A82
		public double iz { get; private set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001888B File Offset: 0x00016A8B
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00018893 File Offset: 0x00016A93
		public double Wy { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001889C File Offset: 0x00016A9C
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x000188A4 File Offset: 0x00016AA4
		public double Wz { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x000188AD File Offset: 0x00016AAD
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x000188B5 File Offset: 0x00016AB5
		public double Wypl { get; private set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000188BE File Offset: 0x00016ABE
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x000188C6 File Offset: 0x00016AC6
		public double Wzpl { get; private set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x000188CF File Offset: 0x00016ACF
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x000188D7 File Offset: 0x00016AD7
		public double It { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000188E0 File Offset: 0x00016AE0
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x000188E8 File Offset: 0x00016AE8
		public double Wt { get; private set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000188F1 File Offset: 0x00016AF1
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000188F9 File Offset: 0x00016AF9
		public double Avy { get; private set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00018902 File Offset: 0x00016B02
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x0001890A File Offset: 0x00016B0A
		public double Avz { get; private set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00018913 File Offset: 0x00016B13
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0001891B File Offset: 0x00016B1B
		public List<int> PossibleCompounds { get; private set; } = new List<int>
		{
			1
		};

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00018924 File Offset: 0x00016B24
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0001892C File Offset: 0x00016B2C
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x00018935 File Offset: 0x00016B35
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0001893D File Offset: 0x00016B3D
		public double dy { get; private set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00018946 File Offset: 0x00016B46
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0001894E File Offset: 0x00016B4E
		public double dz { get; private set; }

		// Token: 0x06000497 RID: 1175 RVA: 0x00018958 File Offset: 0x00016B58
		public SHSection(double Width, double Thickness)
		{
			this.Height = Math.Round(Width, 6);
			this.Width = Math.Round(Width, 6);
			this.Thickness = Math.Round(Thickness, 6);
			this.Name = "SHS " + (1000.0 * Width).ToString() + "x" + (1000.0 * Thickness).ToString();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00018A00 File Offset: 0x00016C00
		public void SetSectionProperties()
		{
			bool flag = false;
			string[] array = Resources.SHSections.Split(new char[]
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
				bool flag2 = Convert.ToDouble(array2[1], new CultureInfo("en-US")) == this.Height && Convert.ToDouble(array2[3], new CultureInfo("en-US")) == this.Thickness;
				if (flag2)
				{
					this.Area = double.Parse(array2[4], new CultureInfo("en-US"));
					this.ct = double.Parse(array2[5], new CultureInfo("en-US"));
					this.Iy = double.Parse(array2[6], new CultureInfo("en-US"));
					this.iy = double.Parse(array2[7], new CultureInfo("en-US"));
					this.Wy = double.Parse(array2[8], new CultureInfo("en-US"));
					this.Wypl = double.Parse(array2[9], new CultureInfo("en-US"));
					this.Iz = this.Iy;
					this.Wz = this.Wy;
					this.Wzpl = this.Wypl;
					this.iz = this.iz;
					this.It = double.Parse(array2[10], new CultureInfo("en-US"));
					this.Wt = double.Parse(array2[11], new CultureInfo("en-US"));
					this.Surface = double.Parse(array2[12], new CultureInfo("en-US"));
					this.cy = this.Width / 2.0;
					this.cz = this.Height / 2.0;
					this.Avy = (this.Avz = 2.0 * this.Width * this.Thickness);
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				throw new Exception(string.Concat(new string[]
				{
					"No SHS-Section with width and height of ",
					(this.Width * 1000.0).ToString(),
					" mm and thickness ",
					(this.Thickness * 1000.0).ToString(),
					" mm could be found in the standard table"
				}));
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00018C8C File Offset: 0x00016E8C
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

		// Token: 0x0600049A RID: 1178 RVA: 0x00018E2A File Offset: 0x0001702A
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00018E38 File Offset: 0x00017038
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00018E50 File Offset: 0x00017050
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

		// Token: 0x0600049D RID: 1181 RVA: 0x00018ED4 File Offset: 0x000170D4
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

		// Token: 0x0600049E RID: 1182 RVA: 0x00019138 File Offset: 0x00017338
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

		// Token: 0x0600049F RID: 1183 RVA: 0x0001924C File Offset: 0x0001744C
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

		// Token: 0x060004A0 RID: 1184 RVA: 0x000192C4 File Offset: 0x000174C4
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

		// Token: 0x060004A1 RID: 1185 RVA: 0x00019504 File Offset: 0x00017704
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
