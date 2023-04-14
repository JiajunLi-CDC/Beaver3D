using System;

namespace Beaver3D.Model.Materials
{
	// Token: 0x0200002F RID: 47
	public interface IMaterial
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000300 RID: 768
		// (set) Token: 0x06000301 RID: 769
		MaterialType Type { get; set; }

		// 材料密度
		double Density { get; set; }

		// 杨氏模量
		double E { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000306 RID: 774
		// (set) Token: 0x06000307 RID: 775
		double G { get; set; }

		// 泊松比
		double PoissonRatio { get; set; }

		// 抗拉强度
		double ft { get; set; }

		// 抗压强度
		double fc { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600030E RID: 782
		// (set) Token: 0x0600030F RID: 783
		double gamma_0 { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000310 RID: 784
		// (set) Token: 0x06000311 RID: 785
		double gamma_1 { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000312 RID: 786
		// (set) Token: 0x06000313 RID: 787
		double kmod { get; set; }

		// Token: 0x06000314 RID: 788
		string ToString();
	}
}
