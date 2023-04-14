using System;

namespace Beaver3D.Optimization
{
	// Token: 0x0200000F RID: 15
	public enum MILPFormulation
	{
		// 枚举了MILP算法的各种公式，目前默认使用Bruetting的公式，参看论文
		Bruetting,
		// Token: 0x0400004B RID: 75
		RasmussenStolpe,
		// Token: 0x0400004C RID: 76
		GhattasGrossmann,
		// Token: 0x0400004D RID: 77
		NP
	}
}
