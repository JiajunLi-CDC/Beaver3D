using System;
using System.Linq;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x0200004A RID: 74
	internal class Beam3D : FiniteElement
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0002128C File Offset: 0x0001F48C
		internal Beam3D(IMember1D M)
		{
			this.SetMemberStiffnessMatrices(M);
			this.SetDofs(M);
			this.SetRedDofs(M);
			this.SetGlobalElementStiffnessMatrix(M);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00021301 File Offset: 0x0001F501
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x00021309 File Offset: 0x0001F509
		internal override int[] Dofs { get; set; } = Enumerable.Repeat<int>(-1, 12).ToArray<int>();

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00021312 File Offset: 0x0001F512
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x0002131A File Offset: 0x0001F51A
		internal override int[] RedDofs { get; set; } = Enumerable.Repeat<int>(-1, 12).ToArray<int>();

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00021323 File Offset: 0x0001F523
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x0002132B File Offset: 0x0001F52B
		internal override MatrixDense K { get; set; }

		// Token: 0x06000623 RID: 1571 RVA: 0x00021334 File Offset: 0x0001F534
		internal override void SetDofs(IMember1D M)
		{
			for (int i = 0; i < 6; i++)
			{
				this.Dofs[i] = M.From.Number * 6 + i;
				this.Dofs[i + 6] = M.To.Number * 6 + i;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0002138C File Offset: 0x0001F58C
		internal override void SetRedDofs(IMember1D M)
		{
			for (int i = 0; i < 6; i++)
			{
				this.RedDofs[i] = M.From.ReducedDofs[i];
				this.RedDofs[6 + i] = M.To.ReducedDofs[i];
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000213DE File Offset: 0x0001F5DE
		internal override void SetGlobalElementStiffnessMatrix(IMember1D Member)
		{
			this.K = this.T.Transpose() * this.k0 * this.T;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0002140C File Offset: 0x0001F60C
		internal override Vector GetLocalDisplacements(Vector u, IMember1D Member)
		{
			Vector vector = new Vector(12);
			for (int i = 0; i < 6; i++)
			{
				vector[i] = u[Member.From.Number * 6 + i];
				vector[i + 6] = u[Member.To.Number * 6 + i];
			}
			return this.T * vector;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00021488 File Offset: 0x0001F688
		private void SetMemberStiffnessMatrices(IMember1D Member)
		{
			MatrixDense t = Member.T;
			this.T = new MatrixDense(12, 12);
			for (int i = 0; i < this.T.NRows / 3; i++)
			{
				for (int j = 0; j < t.NRows; j++)
				{
					this.T[i * 3 + j, i * 3] = t[j, 0];
					this.T[i * 3 + j, i * 3 + 1] = t[j, 1];
					this.T[i * 3 + j, i * 3 + 2] = t[j, 2];
				}
			}
			this.k0[0, 0] = Member.Material.E * Member.CrossSection.Area / Member.Length;
			this.k0[0, 6] = -Member.Material.E * Member.CrossSection.Area / Member.Length;
			this.k0[1, 1] = 12.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length * Member.Length);
			this.k0[1, 5] = 6.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length);
			this.k0[1, 7] = -12.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length * Member.Length);
			this.k0[1, 11] = 6.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length);
			this.k0[2, 2] = 12.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length * Member.Length);
			this.k0[2, 4] = -6.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length);
			this.k0[2, 8] = -12.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length * Member.Length);
			this.k0[2, 10] = -6.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length);
			this.k0[3, 3] = Member.Material.G * Member.CrossSection.It / Member.Length;
			this.k0[3, 9] = -Member.Material.G * Member.CrossSection.It / Member.Length;
			this.k0[4, 4] = 4.0 * Member.Material.E * Member.CrossSection.Iy / Member.Length;
			this.k0[4, 8] = 6.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length);
			this.k0[4, 10] = 2.0 * Member.Material.E * Member.CrossSection.Iy / Member.Length;
			this.k0[5, 5] = 4.0 * Member.Material.E * Member.CrossSection.Iz / Member.Length;
			this.k0[5, 7] = -6.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length);
			this.k0[5, 11] = 2.0 * Member.Material.E * Member.CrossSection.Iz / Member.Length;
			this.k0[6, 6] = Member.Material.E * Member.CrossSection.Area / Member.Length;
			this.k0[7, 7] = 12.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length * Member.Length);
			this.k0[7, 11] = -6.0 * Member.Material.E * Member.CrossSection.Iz / (Member.Length * Member.Length);
			this.k0[8, 8] = 12.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length * Member.Length);
			this.k0[8, 10] = 6.0 * Member.Material.E * Member.CrossSection.Iy / (Member.Length * Member.Length);
			this.k0[9, 9] = Member.Material.G * Member.CrossSection.It / Member.Length;
			this.k0[10, 10] = 4.0 * Member.Material.E * Member.CrossSection.Iy / Member.Length;
			this.k0[11, 11] = 4.0 * Member.Material.E * Member.CrossSection.Iz / Member.Length;
			for (int k = 0; k < this.k0.NRows; k++)
			{
				for (int l = k; l < this.k0.NRows; l++)
				{
					this.k0[l, k] = this.k0[k, l];
				}
			}
		}

		// Token: 0x04000213 RID: 531
		private MatrixDense k0 = new MatrixDense(12);

		// Token: 0x04000214 RID: 532
		private MatrixDense T = new MatrixDense(12);
	}
}
