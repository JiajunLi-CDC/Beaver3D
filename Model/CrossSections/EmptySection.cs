using System;
using System.Collections.Generic;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x02000035 RID: 53
	public class EmptySection : ICrossSection
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000158A1 File Offset: 0x00013AA1
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000158A9 File Offset: 0x00013AA9
		public string Name { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000158B2 File Offset: 0x00013AB2
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000158BA File Offset: 0x00013ABA
		public double Area { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000158C3 File Offset: 0x00013AC3
		// (set) Token: 0x06000391 RID: 913 RVA: 0x000158CB File Offset: 0x00013ACB
		public double Iy { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000392 RID: 914 RVA: 0x000158D4 File Offset: 0x00013AD4
		// (set) Token: 0x06000393 RID: 915 RVA: 0x000158DC File Offset: 0x00013ADC
		public double Iz { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000394 RID: 916 RVA: 0x000158E5 File Offset: 0x00013AE5
		// (set) Token: 0x06000395 RID: 917 RVA: 0x000158ED File Offset: 0x00013AED
		public double It { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000158F6 File Offset: 0x00013AF6
		// (set) Token: 0x06000397 RID: 919 RVA: 0x000158FE File Offset: 0x00013AFE
		public double Wy { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00015907 File Offset: 0x00013B07
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0001590F File Offset: 0x00013B0F
		public double Wz { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00015918 File Offset: 0x00013B18
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00015920 File Offset: 0x00013B20
		public double Wt { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00015929 File Offset: 0x00013B29
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00015931 File Offset: 0x00013B31
		public double dy { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0001593A File Offset: 0x00013B3A
		// (set) Token: 0x0600039F RID: 927 RVA: 0x00015942 File Offset: 0x00013B42
		public double dz { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0001594B File Offset: 0x00013B4B
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x00015953 File Offset: 0x00013B53
		public double cy { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0001595C File Offset: 0x00013B5C
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00015964 File Offset: 0x00013B64
		public double cz { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001596D File Offset: 0x00013B6D
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00015975 File Offset: 0x00013B75
		public double Avy { get; private set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0001597E File Offset: 0x00013B7E
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00015986 File Offset: 0x00013B86
		public double Avz { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0001598F File Offset: 0x00013B8F
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00015997 File Offset: 0x00013B97
		public List<int> PossibleCompounds { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003AA RID: 938 RVA: 0x000159A0 File Offset: 0x00013BA0
		// (set) Token: 0x060003AB RID: 939 RVA: 0x000159A8 File Offset: 0x00013BA8
		public List<ValueTuple<double, double>> Polygon { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003AC RID: 940 RVA: 0x000159B1 File Offset: 0x00013BB1
		// (set) Token: 0x060003AD RID: 941 RVA: 0x000159B9 File Offset: 0x00013BB9
		public string TypeName { get; private set; }

		// Token: 0x060003AE RID: 942 RVA: 0x000159C4 File Offset: 0x00013BC4
		public EmptySection()
		{
			this.Name = "ES";
			this.Area = 0.0;
			this.Iy = 0.0;
			this.Iz = 0.0;
			this.It = 0.0;
			this.Wy = 0.0;
			this.Wz = 0.0;
			this.Wt = 0.0;
			this.Avy = 0.0;
			this.Avz = 0.0;
			this.dy = 0.0;
			this.dz = 0.0;
			this.cy = 0.0;
			this.cz = 0.0;
			this.PossibleCompounds = new List<int>
			{
				0
			};
			this.Polygon = new List<ValueTuple<double, double>>();
			this.TypeName = "ES";
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00015AE4 File Offset: 0x00013CE4
		public double GetTensionResistance(IMaterial Material)
		{
			return 0.0;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00015B00 File Offset: 0x00013D00
		public List<double> GetBucklingResistance(IMaterial Material, BucklingType BucklingType, double BucklingLength)
		{
			return new List<double>
			{
				0.0
			};
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00015B27 File Offset: 0x00013D27
		public void SetPolygon()
		{
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00015B2C File Offset: 0x00013D2C
		public void SetPossibleCompounds(List<int> Compounds)
		{
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00015B31 File Offset: 0x00013D31
		public void SetSectionProperties()
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015B38 File Offset: 0x00013D38
		public double GetUtilization(IMaterial Material, BucklingType BucklingType, double BucklingLength, bool Plastic, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			return 0.0;
		}
	}
}
