using System;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x0200004B RID: 75
	internal abstract class FiniteElement
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000628 RID: 1576
		// (set) Token: 0x06000629 RID: 1577
		internal abstract int[] Dofs { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600062A RID: 1578
		// (set) Token: 0x0600062B RID: 1579
		internal abstract int[] RedDofs { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600062C RID: 1580
		// (set) Token: 0x0600062D RID: 1581
		internal abstract MatrixDense K { get; set; }

		// Token: 0x0600062E RID: 1582
		internal abstract void SetDofs(IMember1D M);

		// Token: 0x0600062F RID: 1583
		internal abstract void SetRedDofs(IMember1D M);

		// Token: 0x06000630 RID: 1584
		internal abstract void SetGlobalElementStiffnessMatrix(IMember1D Member);

		// Token: 0x06000631 RID: 1585
		internal abstract Vector GetLocalDisplacements(Vector u, IMember1D Member);
	}
}
