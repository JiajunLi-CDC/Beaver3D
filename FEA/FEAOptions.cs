using System;
using Beaver3D.LinearAlgebra;

namespace Beaver3D.FEA
{
	// Token: 0x02000045 RID: 69
	public class FEAOptions
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x0001F83A File Offset: 0x0001DA3A
		public FEAOptions() : this(FEAnalysisMethod.LinearElastic)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001F845 File Offset: 0x0001DA45
		public FEAOptions(FEAnalysisMethod Method) : this(Method, MatrixSolver.GaussJordan)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001F851 File Offset: 0x0001DA51
		public FEAOptions(MatrixSolver Solver) : this(FEAnalysisMethod.LinearElastic, Solver)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001F85D File Offset: 0x0001DA5D
		public FEAOptions(FEAnalysisMethod Method, MatrixSolver Solver) : this(Method, Solver, 0)
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001F86A File Offset: 0x0001DA6A
		public FEAOptions(FEAnalysisMethod Method, MatrixSolver Solver, int CGIterations = 60) : this(Method, Solver, CGIterations, 0.0)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001F880 File Offset: 0x0001DA80
		public FEAOptions(FEAnalysisMethod Method, MatrixSolver Solver, int CGIterations = 60, double CGTolerance = 0.001) : this(Method, Solver, CGIterations, CGTolerance, 0)
		{
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001F890 File Offset: 0x0001DA90
		public FEAOptions(FEAnalysisMethod Method, MatrixSolver Solver, int CGIterations = 60, double CGTolerance = 0.001, int NewtonRaphsonIterations = 20)
		{
			this.Method = Method;
			this.Solver = Solver;
			this.CGIterations = CGIterations;
			this.CGTolerance = CGTolerance;
			this.NewtonRaphsonIterations = NewtonRaphsonIterations;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0001F8FC File Offset: 0x0001DAFC
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0001F904 File Offset: 0x0001DB04
		public FEAnalysisMethod Method { get; private set; } = FEAnalysisMethod.LinearElastic;

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001F90D File Offset: 0x0001DB0D
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0001F915 File Offset: 0x0001DB15
		public MatrixSolver Solver { get; private set; } = MatrixSolver.GaussJordan;

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001F91E File Offset: 0x0001DB1E
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0001F926 File Offset: 0x0001DB26
		public int CGIterations { get; private set; } = 60;

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001F92F File Offset: 0x0001DB2F
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x0001F937 File Offset: 0x0001DB37
		public double CGTolerance { get; private set; } = 0.001;

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001F940 File Offset: 0x0001DB40
		// (set) Token: 0x060005F7 RID: 1527 RVA: 0x0001F948 File Offset: 0x0001DB48
		public int NewtonRaphsonIterations { get; private set; } = 20;
	}
}
