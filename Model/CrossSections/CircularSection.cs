using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x0200003C RID: 60
	public class CircularSection : ICrossSection
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001BDDD File Offset: 0x00019FDD
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x0001BDE5 File Offset: 0x00019FE5
		public string TypeName { get; private set; } = "CS";

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001BDEE File Offset: 0x00019FEE
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x0001BDF6 File Offset: 0x00019FF6
		public string Name { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001BDFF File Offset: 0x00019FFF
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x0001BE07 File Offset: 0x0001A007
		public double Area { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001BE10 File Offset: 0x0001A010
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x0001BE18 File Offset: 0x0001A018
		public double Iy { get; private set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001BE21 File Offset: 0x0001A021
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0001BE29 File Offset: 0x0001A029
		public double Iz { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001BE32 File Offset: 0x0001A032
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0001BE3A File Offset: 0x0001A03A
		public double iy { get; private set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001BE43 File Offset: 0x0001A043
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x0001BE4B File Offset: 0x0001A04B
		public double iz { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001BE54 File Offset: 0x0001A054
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x0001BE5C File Offset: 0x0001A05C
		public double It { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001BE65 File Offset: 0x0001A065
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x0001BE6D File Offset: 0x0001A06D
		public double Wy { get; private set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001BE76 File Offset: 0x0001A076
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0001BE7E File Offset: 0x0001A07E
		public double Wz { get; private set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001BE87 File Offset: 0x0001A087
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001BE8F File Offset: 0x0001A08F
		public double Wt { get; private set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0001BE98 File Offset: 0x0001A098
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0001BEA0 File Offset: 0x0001A0A0
		public double Avy { get; private set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001BEA9 File Offset: 0x0001A0A9
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0001BEB1 File Offset: 0x0001A0B1
		public double Avz { get; private set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001BEBA File Offset: 0x0001A0BA
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0001BEC2 File Offset: 0x0001A0C2
		public double dy { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001BECB File Offset: 0x0001A0CB
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x0001BED3 File Offset: 0x0001A0D3
		public double dz { get; private set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001BEDC File Offset: 0x0001A0DC
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0001BEE4 File Offset: 0x0001A0E4
		public double cy { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001BEED File Offset: 0x0001A0ED
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0001BEF5 File Offset: 0x0001A0F5
		public double cz { get; private set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001BEFE File Offset: 0x0001A0FE
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001BF06 File Offset: 0x0001A106
		public List<int> PossibleCompounds { get; private set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001BF0F File Offset: 0x0001A10F
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001BF17 File Offset: 0x0001A117
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x06000544 RID: 1348 RVA: 0x0001BF20 File Offset: 0x0001A120
		public CircularSection(double D = 0.05)
		{
			this.Name = "CS" + Math.Round(1000.0 * D, 2).ToString();
			this.Diameter = D;
			this.Area = 0.0;
			this.Iy = 0.0;
			this.Iz = 0.0;
			this.It = 0.0;
			this.Wy = 0.0;
			this.Wz = 0.0;
			this.Wt = 0.0;
			this.dy = 0.0;
			this.dz = 0.0;
			this.cy = 0.0;
			this.cz = 0.0;
			this.Avy = 0.0;
			this.Avz = 0.0;
			this.iy = 0.0;
			this.iz = 0.0;
			this.PossibleCompounds = new List<int>
			{
				1
			};
			this.Polygon = new List<ValueTuple<double, double>>();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001C090 File Offset: 0x0001A290
		public void SetSectionProperties()
		{
			this.Area = 3.1415926535897931 * this.Diameter * this.Diameter / 4.0;
			this.Iy = (this.Iz = 3.1415926535897931 * this.Diameter * this.Diameter * this.Diameter * this.Diameter / 4.0);
			this.iy = (this.iz = Math.Sqrt(this.Iy / this.Area));
			this.It = 3.1415926535897931 * this.Diameter * this.Diameter * this.Diameter * this.Diameter / 2.0;
			this.Wy = (this.Wz = 3.1415926535897931 * this.Diameter * this.Diameter * this.Diameter / 4.0);
			this.Wt = 3.1415926535897931 * this.Diameter * this.Diameter * this.Diameter / 2.0;
			this.Avy = (this.Avz = this.Area);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001C1E0 File Offset: 0x0001A3E0
		public void SetPolygon()
		{
			int num = 16;
			double num2 = this.Diameter / 2.0;
			for (int j = 0; j < num; j++)
			{
				this.Polygon.Add(new ValueTuple<double, double>(num2 * Math.Cos((double)j * 3.1415926535897931 * 2.0 / (double)num), num2 * Math.Sin((double)j * 3.1415926535897931 * 2.0 / (double)num)));
			}
			this.dy = (from i in this.Polygon
			select i.Item1).Max() - (from i in this.Polygon
			select i.Item1).Min();
			this.dz = (from i in this.Polygon
			select i.Item2).Max() - (from i in this.Polygon
			select i.Item2).Min();
			this.cy = this.Diameter / 2.0;
			this.cz = this.Diameter / 2.0;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001C35F File Offset: 0x0001A55F
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001C36C File Offset: 0x0001A56C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001C384 File Offset: 0x0001A584
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

		// Token: 0x0600054A RID: 1354 RVA: 0x0001C408 File Offset: 0x0001A608
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

		// Token: 0x0600054B RID: 1355 RVA: 0x0001C908 File Offset: 0x0001AB08
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

		// Token: 0x0600054C RID: 1356 RVA: 0x0001CA1C File Offset: 0x0001AC1C
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

		// Token: 0x0600054D RID: 1357 RVA: 0x0001CA94 File Offset: 0x0001AC94
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

		// Token: 0x0600054E RID: 1358 RVA: 0x0001CCD4 File Offset: 0x0001AED4
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

		// Token: 0x040001D4 RID: 468
		public double Diameter;
	}
}
