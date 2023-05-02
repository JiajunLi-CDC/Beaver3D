using System;

namespace Beaver3D.Optimization
{
	// Token: 0x02000016 RID: 22
	public enum Objective
	{
		// 枚举各项优化目标

		// 0
		MinStructureMass,
		// 1
		MinStockMass,
		// 2
		MinWaste,
		// 3
		MinLCA,
		// 4......最大化多结构重复利用率
		MaxReuseRate,

	}
}
