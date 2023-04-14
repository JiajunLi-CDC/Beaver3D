using System;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x02000048 RID: 72
	public class FiniteElementAnalysis
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
		public FiniteElementAnalysis() : this(new FEAOptions())
		{
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001FDB3 File Offset: 0x0001DFB3
		public FiniteElementAnalysis(FEAOptions Options)
		{
			this.Options = Options;
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0001FDD8 File Offset: 0x0001DFD8
		public FEAMethod Solution { get; private set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0001FDE1 File Offset: 0x0001DFE1
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0001FDE9 File Offset: 0x0001DFE9
		public FEAOptions Options { get; private set; } = new FEAOptions();

		// Token: 0x0600060A RID: 1546 RVA: 0x0001FDF4 File Offset: 0x0001DFF4
		public void Solve(Structure structure, LoadCase LC)
		{
			FEAnalysisMethod method = this.Options.Method;
			FEAnalysisMethod feanalysisMethod = method;
			if (feanalysisMethod != FEAnalysisMethod.LinearElastic)
			{
				this.Solution = new LinearElastic();
				Vector vector;
				this.Solution.Solve(structure, LC, out vector, this.Options);
			}
			else
			{
				this.Solution = new LinearElastic();
				Vector vector;
				this.Solution.Solve(structure, LC, out vector, this.Options);
			}
		}
	}
}
