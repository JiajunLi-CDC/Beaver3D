using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x0200003D RID: 61
	public class CircularHollowSection : ICrossSection
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001CF5D File Offset: 0x0001B15D
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x0001CF65 File Offset: 0x0001B165
		public string TypeName { get; private set; } = "CHS";

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001CF6E File Offset: 0x0001B16E
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x0001CF76 File Offset: 0x0001B176
		public string Name { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001CF7F File Offset: 0x0001B17F
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x0001CF87 File Offset: 0x0001B187
		public double Area { get; private set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001CF90 File Offset: 0x0001B190
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x0001CF98 File Offset: 0x0001B198
		public double Iy { get; private set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001CFA1 File Offset: 0x0001B1A1
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x0001CFA9 File Offset: 0x0001B1A9
		public double Iz { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0001CFB2 File Offset: 0x0001B1B2
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x0001CFBA File Offset: 0x0001B1BA
		public double It { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001CFC3 File Offset: 0x0001B1C3
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x0001CFCB File Offset: 0x0001B1CB
		public double Wy { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x0001CFDC File Offset: 0x0001B1DC
		public double Wz { get; private set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001CFE5 File Offset: 0x0001B1E5
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x0001CFED File Offset: 0x0001B1ED
		public double Wt { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001CFF6 File Offset: 0x0001B1F6
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x0001CFFE File Offset: 0x0001B1FE
		public double Avy { get; private set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001D007 File Offset: 0x0001B207
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0001D00F File Offset: 0x0001B20F
		public double Avz { get; private set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001D018 File Offset: 0x0001B218
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0001D020 File Offset: 0x0001B220
		public double dy { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001D029 File Offset: 0x0001B229
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0001D031 File Offset: 0x0001B231
		public double dz { get; private set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001D03A File Offset: 0x0001B23A
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x0001D042 File Offset: 0x0001B242
		public double cy { get; private set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001D04B File Offset: 0x0001B24B
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0001D053 File Offset: 0x0001B253
		public double cz { get; private set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001D05C File Offset: 0x0001B25C
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0001D064 File Offset: 0x0001B264
		public List<int> PossibleCompounds { get; private set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001D06D File Offset: 0x0001B26D
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0001D075 File Offset: 0x0001B275
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x06000571 RID: 1393 RVA: 0x0001D080 File Offset: 0x0001B280
		public CircularHollowSection(double Diameter = 0.05, double WallThickness = 0.001)
		{
			this.Name = "CHS" + Math.Round(1000.0 * Diameter, 2).ToString() + "x" + Math.Round(1000.0 * WallThickness, 2).ToString();
			this.Diameter = Diameter;
			this.Thickness = WallThickness;
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
			this.PossibleCompounds = new List<int>
			{
				1
			};
			this.Polygon = new List<ValueTuple<double, double>>();
			this.SetSectionProperties();
			this.SetPolygon();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		public void SetSectionProperties()
		{
			double num = this.Diameter / 2.0;
			double num2 = num - this.Thickness;
			this.Area = 3.1415926535897931 * (num * num - num2 * num2);
			this.Iy = (this.Iz = 3.1415926535897931 * (num * num * num * num - num2 * num2 * num2 * num2) / 4.0);
			this.It = 1.5707963267948966 / (num * num * num * num - num2 * num2 * num2 * num2);
			this.Wy = (this.Wz = this.Iy / num);
			this.Wt = this.It / num;
			this.Avz = (this.Avy = this.Area);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001D2C8 File Offset: 0x0001B4C8
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

		// Token: 0x06000574 RID: 1396 RVA: 0x0001D447 File Offset: 0x0001B647
		public void SetPossibleCompounds(List<int> Compounds)
		{
			this.PossibleCompounds = Compounds;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001D454 File Offset: 0x0001B654
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001D46C File Offset: 0x0001B66C
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

		// Token: 0x06000577 RID: 1399 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
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
			return result;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001D74C File Offset: 0x0001B94C
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

		// Token: 0x06000579 RID: 1401 RVA: 0x0001D860 File Offset: 0x0001BA60
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

		// Token: 0x0600057A RID: 1402 RVA: 0x0001D8D8 File Offset: 0x0001BAD8
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

		// Token: 0x0600057B RID: 1403 RVA: 0x0001DB18 File Offset: 0x0001BD18
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

		// Token: 0x040001E8 RID: 488
		public double Diameter;

		// Token: 0x040001E9 RID: 489
		public double Thickness;
	}
}
