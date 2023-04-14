using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beaver3D.Model.Materials;
using Beaver3D.Properties;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x0200003A RID: 58
	public class IPESection : ICrossSection
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0001978D File Offset: 0x0001798D
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x00019795 File Offset: 0x00017995
		public string TypeName { get; private set; } = "IPE";

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001979E File Offset: 0x0001799E
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x000197A6 File Offset: 0x000179A6
		public string Name { get; private set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x000197AF File Offset: 0x000179AF
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x000197B7 File Offset: 0x000179B7
		public double Width { get; private set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x000197C0 File Offset: 0x000179C0
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x000197C8 File Offset: 0x000179C8
		public double Height { get; private set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x000197D1 File Offset: 0x000179D1
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x000197D9 File Offset: 0x000179D9
		public double WebThickness { get; private set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x000197E2 File Offset: 0x000179E2
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x000197EA File Offset: 0x000179EA
		public double FlangeThickness { get; private set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x000197F3 File Offset: 0x000179F3
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x000197FB File Offset: 0x000179FB
		public double CornerRadius { get; private set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00019804 File Offset: 0x00017A04
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0001980C File Offset: 0x00017A0C
		public double Area { get; private set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00019815 File Offset: 0x00017A15
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x0001981D File Offset: 0x00017A1D
		public double Surface { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00019826 File Offset: 0x00017A26
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x0001982E File Offset: 0x00017A2E
		public double cy { get; private set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00019837 File Offset: 0x00017A37
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x0001983F File Offset: 0x00017A3F
		public double cz { get; private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00019848 File Offset: 0x00017A48
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00019850 File Offset: 0x00017A50
		public double Iy { get; private set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00019859 File Offset: 0x00017A59
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00019861 File Offset: 0x00017A61
		public double Iz { get; private set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001986A File Offset: 0x00017A6A
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00019872 File Offset: 0x00017A72
		public double iy { get; private set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001987B File Offset: 0x00017A7B
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00019883 File Offset: 0x00017A83
		public double iz { get; private set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x0001988C File Offset: 0x00017A8C
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00019894 File Offset: 0x00017A94
		public double Wy { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001989D File Offset: 0x00017A9D
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x000198A5 File Offset: 0x00017AA5
		public double Wz { get; private set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000198AE File Offset: 0x00017AAE
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x000198B6 File Offset: 0x00017AB6
		public double Wypl { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x000198BF File Offset: 0x00017ABF
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x000198C7 File Offset: 0x00017AC7
		public double Wzpl { get; private set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000198D0 File Offset: 0x00017AD0
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x000198D8 File Offset: 0x00017AD8
		public double Avy { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000198E1 File Offset: 0x00017AE1
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x000198E9 File Offset: 0x00017AE9
		public double Avz { get; private set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x000198F2 File Offset: 0x00017AF2
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x000198FA File Offset: 0x00017AFA
		public double It { get; private set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00019903 File Offset: 0x00017B03
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x0001990B File Offset: 0x00017B0B
		public double Wt { get; private set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00019914 File Offset: 0x00017B14
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x0001991C File Offset: 0x00017B1C
		public List<int> PossibleCompounds { get; private set; } = new List<int>
		{
			1
		};

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00019925 File Offset: 0x00017B25
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0001992D File Offset: 0x00017B2D
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00019936 File Offset: 0x00017B36
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0001993E File Offset: 0x00017B3E
		public double dy { get; private set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00019947 File Offset: 0x00017B47
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x0001994F File Offset: 0x00017B4F
		public double dz { get; private set; }

		// Token: 0x060004D8 RID: 1240 RVA: 0x00019958 File Offset: 0x00017B58
		public IPESection(int Size = 100)
		{
			this.Name = "IPE " + Size.ToString();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000199B4 File Offset: 0x00017BB4
		public void SetSectionProperties()
		{
			bool flag = false;
			string[] array = Resources.IPESections.Split(new char[]
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
					this.Avy = this.Width * this.FlangeThickness * 2.0;
					this.cy = this.Width / 2.0;
					this.cz = this.Height / 2.0;
					flag = true;
					break;
				}
			}
			bool flag3 = !flag;
			if (flag3)
			{
				throw new Exception("No IPE-Section with the given dimension could be found in the standard table");
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00019C70 File Offset: 0x00017E70
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

		// Token: 0x060004DB RID: 1243 RVA: 0x00019FF4 File Offset: 0x000181F4
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001A000 File Offset: 0x00018200
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001A018 File Offset: 0x00018218
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

		// Token: 0x060004DE RID: 1246 RVA: 0x0001A09C File Offset: 0x0001829C
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
				double num2 = 0.34;
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0001A300 File Offset: 0x00018500
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001A414 File Offset: 0x00018614
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001A48C File Offset: 0x0001868C
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001A6CC File Offset: 0x000188CC
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
