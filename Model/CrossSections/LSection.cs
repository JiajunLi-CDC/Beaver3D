using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Beaver3D.Model.Materials;
using Beaver3D.Properties;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x0200003B RID: 59
	public class LSection : ICrossSection
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001A955 File Offset: 0x00018B55
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001A95D File Offset: 0x00018B5D
		public string TypeName { get; private set; } = "LS";

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001A966 File Offset: 0x00018B66
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001A96E File Offset: 0x00018B6E
		public string Name { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0001A977 File Offset: 0x00018B77
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0001A97F File Offset: 0x00018B7F
		public double Width { get; private set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001A988 File Offset: 0x00018B88
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0001A990 File Offset: 0x00018B90
		public double Height { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0001A999 File Offset: 0x00018B99
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0001A9A1 File Offset: 0x00018BA1
		public double Thickness { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001A9AA File Offset: 0x00018BAA
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001A9B2 File Offset: 0x00018BB2
		public double Area { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0001A9BB File Offset: 0x00018BBB
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0001A9C3 File Offset: 0x00018BC3
		public double cy { get; private set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001A9CC File Offset: 0x00018BCC
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0001A9D4 File Offset: 0x00018BD4
		public double cz { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001A9DD File Offset: 0x00018BDD
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0001A9E5 File Offset: 0x00018BE5
		public double cu { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001A9EE File Offset: 0x00018BEE
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0001A9F6 File Offset: 0x00018BF6
		public double cv { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001A9FF File Offset: 0x00018BFF
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0001AA07 File Offset: 0x00018C07
		public double Iy { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001AA10 File Offset: 0x00018C10
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0001AA18 File Offset: 0x00018C18
		public double Iz { get; private set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001AA21 File Offset: 0x00018C21
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x0001AA29 File Offset: 0x00018C29
		public double Iu { get; private set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001AA32 File Offset: 0x00018C32
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x0001AA3A File Offset: 0x00018C3A
		public double Iv { get; private set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001AA43 File Offset: 0x00018C43
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001AA4B File Offset: 0x00018C4B
		public double Wy { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001AA54 File Offset: 0x00018C54
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0001AA5C File Offset: 0x00018C5C
		public double Wz { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001AA65 File Offset: 0x00018C65
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x0001AA6D File Offset: 0x00018C6D
		public double It { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001AA76 File Offset: 0x00018C76
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0001AA7E File Offset: 0x00018C7E
		public double Wt { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001AA87 File Offset: 0x00018C87
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0001AA8F File Offset: 0x00018C8F
		public double Avy { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001AA98 File Offset: 0x00018C98
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0001AAA0 File Offset: 0x00018CA0
		public double Avz { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001AAA9 File Offset: 0x00018CA9
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001AAB1 File Offset: 0x00018CB1
		public List<int> PossibleCompounds { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001AABA File Offset: 0x00018CBA
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001AAC2 File Offset: 0x00018CC2
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001AACB File Offset: 0x00018CCB
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001AAD3 File Offset: 0x00018CD3
		public double dy { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001AADC File Offset: 0x00018CDC
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001AAE4 File Offset: 0x00018CE4
		public double dz { get; private set; }

		// Token: 0x06000513 RID: 1299 RVA: 0x0001AAF0 File Offset: 0x00018CF0
		public LSection(double Height = 0.05, double Width = 0.05, double Thickness = 0.003)
		{
			this.Width = Width;
			this.Height = Height;
			this.Thickness = Thickness;
			this.Name = string.Concat(new string[]
			{
				"LS",
				(1000.0 * Height).ToString(),
				"x",
				(1000.0 * Width).ToString(),
				"x",
				(1000.0 * Thickness).ToString()
			});
			this.Area = 0.0;
			this.Iy = 0.0;
			this.Iz = 0.0;
			this.Iu = 0.0;
			this.Iv = 0.0;
			this.It = 0.0;
			this.Wy = 0.0;
			this.Wz = 0.0;
			this.Wt = 0.0;
			this.cy = 0.0;
			this.cz = 0.0;
			this.cu = 0.0;
			this.cv = 0.0;
			this.dy = 0.0;
			this.dz = 0.0;
			this.Avz = 0.0;
			this.Avy = 0.0;
			this.PossibleCompounds = new List<int>
			{
				1,
				2,
				4
			};
			this.Polygon = new List<ValueTuple<double, double>>();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001ACE4 File Offset: 0x00018EE4
		public void SetSectionProperties()
		{
			bool flag = false;
			string[] array = Resources.LSections.Split(new char[]
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
				bool flag2 = this.Height >= this.Width;
				if (flag2)
				{
					bool flag3 = Convert.ToDouble(array2[1], new CultureInfo("en-US")) == this.Height && Convert.ToDouble(array2[2], new CultureInfo("en-US")) == this.Width && Convert.ToDouble(array2[3], new CultureInfo("en-US")) == this.Thickness;
					if (flag3)
					{
						this.Area = double.Parse(array2[4], new CultureInfo("en-US"));
						this.cy = double.Parse(array2[5], new CultureInfo("en-US"));
						this.cz = double.Parse(array2[6], new CultureInfo("en-US"));
						this.cu = double.Parse(array2[7], new CultureInfo("en-US"));
						this.cv = double.Parse(array2[8], new CultureInfo("en-US"));
						this.Iy = double.Parse(array2[9], new CultureInfo("en-US"));
						this.Iz = double.Parse(array2[10], new CultureInfo("en-US"));
						this.Wy = double.Parse(array2[11], new CultureInfo("en-US"));
						this.Wz = double.Parse(array2[12], new CultureInfo("en-US"));
						this.Iu = double.Parse(array2[13], new CultureInfo("en-US"));
						this.Iv = double.Parse(array2[14], new CultureInfo("en-US"));
						this.Avy = this.Width * this.Thickness;
						this.Avz = this.Height * this.Thickness;
						flag = true;
						break;
					}
				}
				else
				{
					bool flag4 = Convert.ToDouble(array2[2]) == this.Width && Convert.ToDouble(array2[1]) == this.Height && Convert.ToDouble(array2[3]) == this.Thickness;
					if (flag4)
					{
						this.Area = double.Parse(array2[4], new CultureInfo("en-US"));
						this.cy = double.Parse(array2[6], new CultureInfo("en-US"));
						this.cz = double.Parse(array2[5], new CultureInfo("en-US"));
						this.cu = double.Parse(array2[8], new CultureInfo("en-US"));
						this.cv = double.Parse(array2[7], new CultureInfo("en-US"));
						this.Iy = double.Parse(array2[10], new CultureInfo("en-US"));
						this.Iz = double.Parse(array2[9], new CultureInfo("en-US"));
						this.Wy = double.Parse(array2[12], new CultureInfo("en-US"));
						this.Wz = double.Parse(array2[11], new CultureInfo("en-US"));
						this.Iu = double.Parse(array2[14], new CultureInfo("en-US"));
						this.Iv = double.Parse(array2[13], new CultureInfo("en-US"));
						flag = true;
						break;
					}
				}
			}
			bool flag5 = !flag;
			if (flag5)
			{
				throw new Exception("No L-Section with the given dimensions could be found in the standard table");
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001B098 File Offset: 0x00019298
		public void SetPolygon()
		{
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, 0.0 - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Width - this.cy, this.Thickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Thickness - this.cy, this.Thickness - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(this.Thickness - this.cy, this.Height - this.cz));
			this.Polygon.Add(new ValueTuple<double, double>(0.0 - this.cy, this.Height - this.cz));
			this.dy = (from i in this.Polygon
			select i.Item1).Max() - (from i in this.Polygon
			select i.Item1).Min();
			this.dz = (from i in this.Polygon
			select i.Item2).Max() - (from i in this.Polygon
			select i.Item2).Min();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001B280 File Offset: 0x00019480
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001B28C File Offset: 0x0001948C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001B2A4 File Offset: 0x000194A4
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

		// Token: 0x06000519 RID: 1305 RVA: 0x0001B328 File Offset: 0x00019528
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
				double num = 0.49;
				double num2 = 9.869604401089358 * Material.E * this.Iy / BucklingLength / BucklingLength;
				double num3 = 9.869604401089358 * Material.E * this.Iu / BucklingLength / BucklingLength;
				double num4 = 9.869604401089358 * Material.E * this.Iv / BucklingLength / BucklingLength;
				double num5 = 0.66666666666666663 * (this.Width - this.Thickness / 2.0) * this.Thickness * this.Thickness * this.Thickness;
				double num6 = this.cv - this.Thickness / Math.Sqrt(2.0);
				double num7 = num6 * num6 + (this.Iu + this.Iv) / this.Area;
				double num8 = Material.G * num5 / num7;
				double num9 = num7 - num6 * num6;
				double num10 = -num7 * (num8 + num3);
				double num11 = num7 * num3 * num8;
				double val = num4;
				double val2 = (-num10 + Math.Sqrt(num10 * num10 - 4.0 * num9 * num11)) / 2.0 / num9;
				double val3 = (-num10 - Math.Sqrt(num10 * num10 - 4.0 * num9 * num11)) / 2.0 / num9;
				double num12 = Math.Min(Math.Min(val, val2), val3);
				double num13 = num12 / this.Area;
				double num14 = Math.Sqrt(Material.fc / num13);
				double num15 = 0.5 * (1.0 + num * (num14 - 0.2) + num14 * num14);
				double num16 = Math.Min(1.0, 1.0 / (num15 + Math.Sqrt(num15 * num15 - num14 * num14)));
				double item3 = -num16 * Material.fc * this.Area / Material.gamma_1;
				double num17 = Math.Sqrt(this.Iy / this.Area);
				double num18 = Math.Sqrt(this.Iz / this.Area);
				double num19 = 3.1415926535897931 * Math.Sqrt(Material.E / Material.fc);
				double num20 = BucklingLength / num17 / num19;
				double num21 = BucklingLength / num18 / num19;
				double num22 = 0.5 * (1.0 + num * (num20 - 0.2) + num20 * num20);
				double num23 = 0.5 * (1.0 + num * (num21 - 0.2) + num21 * num21);
				double num24 = Math.Min(1.0, 1.0 / (num22 + Math.Sqrt(num22 * num22 - num20 * num20)));
				double num25 = Math.Min(1.0, 1.0 / (num23 + Math.Sqrt(num23 * num23 - num21 * num21)));
				double item4 = -num24 * this.Area * Material.fc / Material.gamma_1;
				double item5 = -num25 * this.Area * Material.fc / Material.gamma_1;
				result = new List<double>
				{
					item4,
					item5,
					item3
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

		// Token: 0x0600051A RID: 1306 RVA: 0x0001B788 File Offset: 0x00019988
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

		// Token: 0x0600051B RID: 1307 RVA: 0x0001B89C File Offset: 0x00019A9C
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

		// Token: 0x0600051C RID: 1308 RVA: 0x0001B914 File Offset: 0x00019B14
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

		// Token: 0x0600051D RID: 1309 RVA: 0x0001BB54 File Offset: 0x00019D54
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
