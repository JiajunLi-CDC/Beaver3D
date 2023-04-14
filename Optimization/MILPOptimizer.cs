using System;

namespace Beaver3D.Optimization
{
	// Token: 0x0200000C RID: 12
	public enum MILPOptimizer
	{
		// 优化器枚举类，目前只使用了gurobi
		Gurobi,
		// Token: 0x04000035 RID: 53
		CPLEX,
		// Token: 0x04000036 RID: 54
		GLPK,
		// Token: 0x04000037 RID: 55
		MOSEK
	}
}
