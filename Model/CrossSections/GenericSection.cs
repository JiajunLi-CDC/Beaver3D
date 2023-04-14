using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000036 RID: 54
	public class GenericSection : ICrossSection
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00015B53 File Offset: 0x00013D53
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00015B5B File Offset: 0x00013D5B
		public string TypeName { get; private set; } = "GS";

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00015B64 File Offset: 0x00013D64
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00015B6C File Offset: 0x00013D6C
		public string Name { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00015B75 File Offset: 0x00013D75
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00015B7D File Offset: 0x00013D7D
		public double Area { get; private set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00015B86 File Offset: 0x00013D86
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00015B8E File Offset: 0x00013D8E
		public double Iy { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00015B97 File Offset: 0x00013D97
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00015B9F File Offset: 0x00013D9F
		public double Iz { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00015BA8 File Offset: 0x00013DA8
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00015BB0 File Offset: 0x00013DB0
		public double Wy { get; private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00015BB9 File Offset: 0x00013DB9
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00015BC1 File Offset: 0x00013DC1
		public double Wz { get; private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00015BCA File Offset: 0x00013DCA
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00015BD2 File Offset: 0x00013DD2
		public double It { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00015BDB File Offset: 0x00013DDB
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00015BE3 File Offset: 0x00013DE3
		public double Wt { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00015BEC File Offset: 0x00013DEC
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x00015BF4 File Offset: 0x00013DF4
		public double Avy { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00015BFD File Offset: 0x00013DFD
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00015C05 File Offset: 0x00013E05
		public double Avz { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00015C0E File Offset: 0x00013E0E
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00015C16 File Offset: 0x00013E16
		public double dy { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00015C1F File Offset: 0x00013E1F
		// (set) Token: 0x060003CE RID: 974 RVA: 0x00015C27 File Offset: 0x00013E27
		public double dz { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00015C30 File Offset: 0x00013E30
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x00015C38 File Offset: 0x00013E38
		public double cy { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00015C41 File Offset: 0x00013E41
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x00015C49 File Offset: 0x00013E49
		public double cz { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00015C52 File Offset: 0x00013E52
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00015C5A File Offset: 0x00013E5A
		public List<int> PossibleCompounds { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00015C63 File Offset: 0x00013E63
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00015C6B File Offset: 0x00013E6B
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x060003D7 RID: 983 RVA: 0x00015C74 File Offset: 0x00013E74
		public GenericSection(string Name, double Area, double Iy, double Iz, double Wy, double Wz, double Wt, double It, double Avy, double Avz)
		{
			this.Name = Name;
			this.Area = Area;
			this.Iy = Iy;
			this.Iz = Iz;
			this.Wy = Wy;
			this.Wz = Wz;
			this.Wt = Wt;
			this.It = It;
			this.Avy = Avy;
			this.Avz = Avz;
			CircularSection circularSection = new CircularSection(Math.Sqrt(4.0 * Area / 3.1415926535897931));
			this.Polygon = circularSection.Polygon;
			this.dy = (from i in this.Polygon
			select i.Item1).Max() - (from i in this.Polygon
			select i.Item1).Min();
			this.dz = (from i in this.Polygon
			select i.Item2).Max() - (from i in this.Polygon
			select i.Item2).Min();
			this.cy = this.dy / 2.0;
			this.cz = this.dz / 2.0;
			this.PossibleCompounds = new List<int>
			{
				1
			};
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015E28 File Offset: 0x00014028
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

		// Token: 0x060003D9 RID: 985 RVA: 0x00015EAC File Offset: 0x000140AC
		public List<double> GetBucklingResistance(IMaterial Material, BucklingType BucklingType, double BucklingLength)
		{
			return new List<double>
			{
				-Material.fc * this.Area / Material.gamma_0
			};
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015EDF File Offset: 0x000140DF
		public void SetPolygon()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00015EE7 File Offset: 0x000140E7
		public void SetPossibleCompounds(List<int> Compounds)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00015EEF File Offset: 0x000140EF
		public void SetSectionProperties()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00015EF8 File Offset: 0x000140F8
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

		// Token: 0x060003DE RID: 990 RVA: 0x0001600C File Offset: 0x0001420C
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

		// Token: 0x060003DF RID: 991 RVA: 0x00016084 File Offset: 0x00014284
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

		// Token: 0x060003E0 RID: 992 RVA: 0x000162C4 File Offset: 0x000144C4
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
