using System;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x02000044 RID: 68
	public abstract class FEAMethod
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001F81F File Offset: 0x0001DA1F
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0001F827 File Offset: 0x0001DA27
		public Vector u { get; internal set; }

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001F830 File Offset: 0x0001DA30
		internal FEAMethod()
		{
		}

		// Token: 0x060005E3 RID: 1507
		internal abstract void Solve(Structure structure, LoadCase LC, out Vector u, FEAOptions Options);

		// Token: 0x060005E4 RID: 1508
		public abstract MatrixDense GetK(Structure Structure);

		// Token: 0x060005E5 RID: 1509
		public abstract Vector Getu(Structure Structure, LoadCase LC);

		// Token: 0x060005E6 RID: 1510
		public abstract Vector Getf(Structure Structure, LoadCase LC);
	}
}
