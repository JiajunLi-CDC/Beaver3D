using System;

namespace Beaver3D.Model.Materials
{
	// Token: 0x02000032 RID: 50
	public struct Steel : IMaterial
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00013DA9 File Offset: 0x00011FA9
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00013DB1 File Offset: 0x00011FB1
		public MaterialType Type {  get; set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00013DBA File Offset: 0x00011FBA
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00013DC2 File Offset: 0x00011FC2
		public string Name {  get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00013DCB File Offset: 0x00011FCB
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00013DD3 File Offset: 0x00011FD3
		public double Density {  get; set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00013DDC File Offset: 0x00011FDC
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public double E {  get; set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00013DED File Offset: 0x00011FED
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00013DF5 File Offset: 0x00011FF5
		public double G {  get; set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00013DFE File Offset: 0x00011FFE
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00013E06 File Offset: 0x00012006
		public double PoissonRatio {  get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00013E0F File Offset: 0x0001200F
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00013E17 File Offset: 0x00012017
		public double ft {  get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00013E20 File Offset: 0x00012020
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00013E28 File Offset: 0x00012028
		public double fc {  get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00013E31 File Offset: 0x00012031
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00013E39 File Offset: 0x00012039
		public double gamma_0 {  get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00013E42 File Offset: 0x00012042
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00013E4A File Offset: 0x0001204A
		public double gamma_1 {  get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00013E53 File Offset: 0x00012053
		// (set) Token: 0x06000348 RID: 840 RVA: 0x00013E5B File Offset: 0x0001205B
		public double kmod {  get; set; }

		// Token: 0x06000349 RID: 841 RVA: 0x00013E64 File Offset: 0x00012064
		public Steel(double Density)
		{
			this = new Steel(Density, 0.0);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00013E78 File Offset: 0x00012078
		public Steel(double Density, double E)
		{
			this = new Steel(Density, E, 0.0);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00013E90 File Offset: 0x00012090
		public Steel(double Density, double E, double PoissonRatio)
		{
			this = new Steel(Density, E, PoissonRatio, 0.0, 235.0, 1.0, 1.1, 1.0);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00013ED5 File Offset: 0x000120D5
		public Steel(double Density, double E, double PoissonRatio, double ft, double fc)
		{
			this = new Steel(Density, E, PoissonRatio, ft, fc, 0.0);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00013EF0 File Offset: 0x000120F0
		public Steel(double Density, double E, double PoissonRatio, double ft, double fc, double gamma_0)
		{
			this = new Steel(Density, E, PoissonRatio, ft, fc, gamma_0, 0.0, 1.0);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00013F20 File Offset: 0x00012120
		public Steel(double Density = 7850.0, double E = 210000.0, double PoissonRatio = 0.3, double ft = 235.0, double fc = 235.0, double gamma_0 = 1.0, double gamma_1 = 1.1, double kmod = 1.0)
		{
			this.Type = MaterialType.Metal;
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
				"Steel - Density: ",
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

		// Token: 0x0600034F RID: 847 RVA: 0x00014008 File Offset: 0x00012208
		public override string ToString()
		{
			return this.Name.ToString();
		}
	}
}
