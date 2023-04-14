using System;

namespace Beaver3D.Model.Materials
{
	// Token: 0x02000031 RID: 49
	public struct Timber : IMaterial
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00013B3E File Offset: 0x00011D3E
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00013B46 File Offset: 0x00011D46
		public MaterialType Type {  get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00013B4F File Offset: 0x00011D4F
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00013B57 File Offset: 0x00011D57
		public string Name {  get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00013B60 File Offset: 0x00011D60
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00013B68 File Offset: 0x00011D68
		public double Density {  get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00013B71 File Offset: 0x00011D71
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00013B79 File Offset: 0x00011D79
		public double E {  get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00013B82 File Offset: 0x00011D82
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00013B8A File Offset: 0x00011D8A
		public double G {  get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00013B93 File Offset: 0x00011D93
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00013B9B File Offset: 0x00011D9B
		public double PoissonRatio {  get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00013BA4 File Offset: 0x00011DA4
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00013BAC File Offset: 0x00011DAC
		public double fc {  get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00013BB5 File Offset: 0x00011DB5
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00013BBD File Offset: 0x00011DBD
		public double ft {  get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00013BC6 File Offset: 0x00011DC6
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00013BCE File Offset: 0x00011DCE
		public double gamma_0 {  get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00013BD7 File Offset: 0x00011DD7
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00013BDF File Offset: 0x00011DDF
		public double gamma_1 {  get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00013BE8 File Offset: 0x00011DE8
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00013BF0 File Offset: 0x00011DF0
		public double kmod {  get; set; }

		// Token: 0x0600032B RID: 811 RVA: 0x00013BF9 File Offset: 0x00011DF9
		public Timber(double Density)
		{
			this = new Timber(Density, 0.0);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00013C0D File Offset: 0x00011E0D
		public Timber(double Density, double E)
		{
			this = new Timber(Density, E, 0.0);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00013C22 File Offset: 0x00011E22
		public Timber(double Density, double E, double PoissonRatio)
		{
			this = new Timber(Density, E, PoissonRatio, 0.0, 0.0);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00013C41 File Offset: 0x00011E41
		public Timber(double Density, double E, double PoissonRatio, double ft, double fc)
		{
			this = new Timber(Density, E, PoissonRatio, ft, fc, 0.0);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00013C5B File Offset: 0x00011E5B
		public Timber(double Density, double E, double PoissonRatio, double ft, double fc, double gamma_0)
		{
			this = new Timber(Density, E, PoissonRatio, ft, fc, gamma_0, 0.0);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00013C78 File Offset: 0x00011E78
		public Timber(double Density, double E, double PoissonRatio, double ft, double fc, double gamma_0, double gamma_1)
		{
			this = new Timber(Density, E, PoissonRatio, ft, fc, gamma_0, gamma_1, 0.0);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00013CA4 File Offset: 0x00011EA4
		public Timber(double Density = 500.0, double E = 12000.0, double PoissonRatio = 0.25, double ft = 12.0, double fc = 8.0, double gamma_0 = 1.0, double gamma_1 = 1.1, double kmod = 0.8)
		{
			this.Type = MaterialType.Timber;
			this.Density = Density;
			this.E = E;
			this.PoissonRatio = PoissonRatio;
			this.G = this.E / (2.0 * (1.0 + PoissonRatio));
			this.ft = ft;
			this.fc = fc;
			this.gamma_0 = gamma_0;
			this.gamma_1 = gamma_1;
			this.kmod = kmod;
			this.Name = string.Concat(new string[]
			{
				"Timber - Density: ",
				Density.ToString(),
				"kg/m3 ft: ",
				this.ft.ToString(),
				"N/mm2 fc: ",
				fc.ToString(),
				"N/mm2 E: ",
				E.ToString(),
				"N/mm2"
			});
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00013D8C File Offset: 0x00011F8C
		public override string ToString()
		{
			return this.Name.ToString();
		}
	}
}
