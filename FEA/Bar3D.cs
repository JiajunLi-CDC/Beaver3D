using System;
using System.Linq;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x02000047 RID: 71
	internal class Bar3D : FiniteElement
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0001F954 File Offset: 0x0001DB54
		internal Bar3D(IMember1D M)
		{
			this.SetMemberStiffnessMatrices(M);
			this.SetDofs(M);
			this.SetRedDofs(M);
			this.SetGlobalElementStiffnessMatrix(M);
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0001F9C9 File Offset: 0x0001DBC9
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0001F9D1 File Offset: 0x0001DBD1
		internal override int[] Dofs { get; set; } = Enumerable.Repeat<int>(-1, 12).ToArray<int>();

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001F9DA File Offset: 0x0001DBDA
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001F9E2 File Offset: 0x0001DBE2
		internal override int[] RedDofs { get; set; } = Enumerable.Repeat<int>(-1, 12).ToArray<int>();

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001F9EB File Offset: 0x0001DBEB
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0001F9F3 File Offset: 0x0001DBF3
		internal override MatrixDense K { get; set; }

		// Token: 0x060005FF RID: 1535 RVA: 0x0001F9FC File Offset: 0x0001DBFC
		internal override void SetDofs(IMember1D M)
		{
			for (int i = 0; i < 6; i++)
			{
				this.Dofs[i] = M.From.Number * 6 + i;
				this.Dofs[i + 6] = M.To.Number * 6 + i;
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001FA54 File Offset: 0x0001DC54
		internal override void SetRedDofs(IMember1D M)
		{
			for (int i = 0; i < 3; i++)
			{
				this.RedDofs[i] = M.From.ReducedDofs[i];
				this.RedDofs[6 + i] = M.To.ReducedDofs[i];
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001FAA6 File Offset: 0x0001DCA6
		internal override void SetGlobalElementStiffnessMatrix(IMember1D Member)
		{
			this.K = this.T.Transpose() * this.k0 * this.T;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
		internal override Vector GetLocalDisplacements(Vector u, IMember1D Member)
		{
			MatrixDense matrixDense = new MatrixDense(2, 6);
			matrixDense[0, 0] = Member.T[0, 0];
			matrixDense[0, 1] = Member.T[0, 1];
			matrixDense[0, 2] = Member.T[0, 2];
			matrixDense[1, 3] = Member.T[0, 0];
			matrixDense[1, 4] = Member.T[0, 1];
			matrixDense[1, 5] = Member.T[0, 2];
			Vector vector = new Vector(6);
			vector[0] = u[Member.From.Number * 6];
			vector[1] = u[Member.From.Number * 6 + 1];
			vector[2] = u[Member.From.Number * 6 + 2];
			vector[3] = u[Member.To.Number * 6];
			vector[4] = u[Member.To.Number * 6 + 1];
			vector[5] = u[Member.To.Number * 6 + 2];
			return matrixDense * vector;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001FC3C File Offset: 0x0001DE3C
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
			this.k0[6, 6] = Member.Material.E * Member.CrossSection.Area / Member.Length;
			this.k0[6, 0] = -Member.Material.E * Member.CrossSection.Area / Member.Length;
		}

		// Token: 0x0400020B RID: 523
		private readonly MatrixDense k0 = new MatrixDense(12);

		// Token: 0x0400020C RID: 524
		private MatrixDense T = new MatrixDense(12);
	}
}
