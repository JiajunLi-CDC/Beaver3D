using System;
using System.Collections.Generic;
using Beaver3D.LCA;

namespace Beaver3D.Optimization
{
	// Token: 0x0200000E RID: 14
	public class OptimOptions
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004656 File Offset: 0x00002856
		// (set) Token: 0x06000083 RID: 131 RVA: 0x0000465E File Offset: 0x0000285E
		public ILCA LCA { get; private set; } = new GHGFrontiers();

		// 最长计算时间
		public int MaxTime = int.MaxValue;

		// 是否弹出过程面板
		public bool LogToConsole = true;

		// 面板名称
		public string LogFormName = "MyLogFormName";

		// T是否考虑自重
		public bool Selfweight = true;

		// 是否考虑几何兼容性，默认为要考虑
		public bool Compatibility = true;

		// 最大质量？
		public double MaxMass = double.MaxValue;

		// Token: 0x04000041 RID: 65
		public bool SOS_Assignment = false;

		// Token: 0x04000042 RID: 66
		public bool SOS_Continuous = false;

		// Token: 0x04000043 RID: 67
		public LPOptimizer LPOptimizer = LPOptimizer.Gurobi;

		// MILP问题使用默认优化器gurobi
		public MILPOptimizer MILPOptimizer = MILPOptimizer.Gurobi;

		// Token: 0x04000045 RID: 69
		public NLPOptimizer NLPOptimizer = NLPOptimizer.IPOPT;

		// 默认MILP问题的计算公式为使用Bruetting的公式
		public MILPFormulation MILPFormulation = MILPFormulation.Bruetting;

		// gurobi参数
		public List<Tuple<string, string>> GurobiParameters = new List<Tuple<string, string>>();    

		// 是否切割
		public bool CuttingStock = false;

		// 是否拓扑固定
		public bool TopologyFixed = false;
	}
}
