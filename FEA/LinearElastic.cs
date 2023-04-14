using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;

namespace Beaver3D.FEA
{
	// Token: 0x02000049 RID: 73
	internal class LinearElastic : FEAMethod
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001FE5F File Offset: 0x0001E05F
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x0001FE67 File Offset: 0x0001E067
		public double solvertime { get; private set; } = 0.0;

		// Token: 0x0600060D RID: 1549 RVA: 0x0001FE70 File Offset: 0x0001E070
		internal LinearElastic()
		{
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001FE8C File Offset: 0x0001E08C
		internal override void Solve(Structure structure, LoadCase LC, out Vector u, FEAOptions Options)
		{
			MatrixDense kred = this.GetKred(structure);
			Vector b = this.Getfred(structure, LC);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			Vector ured = MatrixDense.Solve(kred, b, Options.Solver, Options.CGIterations, Options.CGTolerance);
			this.solvertime = (double)stopwatch.ElapsedMilliseconds;
			stopwatch.Stop();
			base.u = this.Extendu(structure, ured);
			u = base.u;
			LinearElastic.SetMemberForces(structure, LC, base.u);
			LinearElastic.SetStructureDisplacements(structure, LC, base.u);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001FF20 File Offset: 0x0001E120
		public override MatrixDense GetK(Structure Structure)
		{
			MatrixDense result = new MatrixDense(Structure.Nodes.Count * 6);
			foreach (IMember m in Structure.Members)
			{
				LinearElastic.AddElementStiffness(ref result, m);
			}
			return result;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001FF94 File Offset: 0x0001E194
		public MatrixDense GetKred(Structure Structure)
		{
			bool flag = !Structure.Members.OfType<Beam>().Any<Beam>();
			MatrixDense result;
			if (flag)
			{
				result = this.GetKredTruss(Structure);
			}
			else
			{
				result = this.GetKredFull(Structure);
			}
			return result;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001FFD0 File Offset: 0x0001E1D0
		private MatrixDense GetKredTruss(Structure Structure)
		{
			MatrixDense matrixDense = new MatrixDense(Structure.NFreeTranslations);
			foreach (IMember member in Structure.Members)
			{
				Bar bar = (Bar)member;
				Bar3D bar3D = new Bar3D(bar);
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						bool flag = !bar.From.Fix[i] && !bar.From.Fix[j];
						if (flag)
						{
							MatrixDense matrixDense2 = matrixDense;
							int num = bar.From.ReducedDofsTruss[i];
							int num2 = bar.From.ReducedDofsTruss[j];
							matrixDense2[num, num2] += bar3D.K[i, j];
						}
					}
				}
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						bool flag2 = !bar.To.Fix[k] && !bar.From.Fix[l];
						if (flag2)
						{
							MatrixDense matrixDense2 = matrixDense;
							int num2 = bar.To.ReducedDofsTruss[k];
							int num = bar.From.ReducedDofsTruss[l];
							matrixDense2[num2, num] += bar3D.K[6 + k, l];
						}
					}
				}
				for (int m = 0; m < 3; m++)
				{
					for (int n = 0; n < 3; n++)
					{
						bool flag3 = !bar.From.Fix[m] && !bar.To.Fix[n];
						if (flag3)
						{
							MatrixDense matrixDense2 = matrixDense;
							int num = bar.From.ReducedDofsTruss[m];
							int num2 = bar.To.ReducedDofsTruss[n];
							matrixDense2[num, num2] += bar3D.K[m, 6 + n];
						}
					}
				}
				for (int num3 = 0; num3 < 3; num3++)
				{
					for (int num4 = 0; num4 < 3; num4++)
					{
						bool flag4 = !bar.To.Fix[num3] && !bar.To.Fix[num4];
						if (flag4)
						{
							MatrixDense matrixDense2 = matrixDense;
							int num2 = bar.To.ReducedDofsTruss[num3];
							int num = bar.To.ReducedDofsTruss[num4];
							matrixDense2[num2, num] += bar3D.K[6 + num3, 6 + num4];
						}
					}
				}
			}
			return matrixDense;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002035C File Offset: 0x0001E55C
		private MatrixDense GetKredFull(Structure Structure)
		{
			MatrixDense matrixDense = new MatrixDense(Structure.NFreeTranslations);
			foreach (IMember member in Structure.Members)
			{
				Bar bar = (Bar)member;
				Bar3D bar3D = new Bar3D(bar);
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num = bar.From.ReducedDofs[i];
						int num2 = bar.From.ReducedDofs[j];
						matrixDense2[num, num2] += bar3D.K[i, j];
					}
				}
				for (int k = 0; k < 6; k++)
				{
					for (int l = 0; l < 6; l++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num2 = bar.To.ReducedDofs[k];
						int num = bar.From.ReducedDofs[l];
						matrixDense2[num2, num] += bar3D.K[6 + k, l];
					}
				}
				for (int m = 0; m < 6; m++)
				{
					for (int n = 0; n < 6; n++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num = bar.From.ReducedDofs[m];
						int num2 = bar.To.ReducedDofs[n];
						matrixDense2[num, num2] += bar3D.K[m, 6 + n];
					}
				}
				for (int num3 = 0; num3 < 6; num3++)
				{
					for (int num4 = 0; num4 < 6; num4++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num2 = bar.To.ReducedDofs[num3];
						int num = bar.To.ReducedDofs[num4];
						matrixDense2[num2, num] += bar3D.K[6 + num3, 6 + num4];
					}
				}
			}
			foreach (IMember member2 in Structure.Members)
			{
				Beam beam = (Beam)member2;
				Beam3D beam3D = new Beam3D(beam);
				for (int num5 = 0; num5 < 6; num5++)
				{
					for (int num6 = 0; num6 < 6; num6++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num = beam.From.ReducedDofs[num5];
						int num2 = beam.From.ReducedDofs[num6];
						matrixDense2[num, num2] += beam3D.K[num5, num6];
					}
				}
				for (int num7 = 0; num7 < 6; num7++)
				{
					for (int num8 = 0; num8 < 6; num8++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num2 = beam.To.ReducedDofs[num7];
						int num = beam.From.ReducedDofs[num8];
						matrixDense2[num2, num] += beam3D.K[6 + num7, num8];
					}
				}
				for (int num9 = 0; num9 < 6; num9++)
				{
					for (int num10 = 0; num10 < 6; num10++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num = beam.From.ReducedDofs[num9];
						int num2 = beam.To.ReducedDofs[num10];
						matrixDense2[num, num2] += beam3D.K[num9, 6 + num10];
					}
				}
				for (int num11 = 0; num11 < 6; num11++)
				{
					for (int num12 = 0; num12 < 6; num12++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int num2 = beam.To.ReducedDofs[num11];
						int num = beam.To.ReducedDofs[num12];
						matrixDense2[num2, num] += beam3D.K[6 + num11, 6 + num12];
					}
				}
			}
			return matrixDense;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00020878 File Offset: 0x0001EA78
		public override Vector Getf(Structure Structure, LoadCase LC)
		{
			Vector vector = new Vector(Structure.Nodes.Count * 6);
			foreach (ILoad load in LC.Loads)
			{
				PointLoad pointLoad = (PointLoad)load;
				bool flag = !Structure.FixedDofs[pointLoad.Node.Number * 6];
				if (flag)
				{
					vector[pointLoad.Node.Number * 6] = pointLoad.Fx;
				}
				bool flag2 = !Structure.FixedDofs[pointLoad.Node.Number * 6 + 1];
				if (flag2)
				{
					vector[pointLoad.Node.Number * 6 + 1] = pointLoad.Fy;
				}
				bool flag3 = !Structure.FixedDofs[pointLoad.Node.Number * 6 + 2];
				if (flag3)
				{
					vector[pointLoad.Node.Number * 6 + 2] = pointLoad.Fz;
				}
				bool flag4 = !Structure.FixedDofs[pointLoad.Node.Number * 6 + 3];
				if (flag4)
				{
					vector[pointLoad.Node.Number * 6 + 3] = pointLoad.Mx;
				}
				bool flag5 = !Structure.FixedDofs[pointLoad.Node.Number * 6 + 4];
				if (flag5)
				{
					vector[pointLoad.Node.Number * 6 + 4] = pointLoad.My;
				}
				bool flag6 = !Structure.FixedDofs[pointLoad.Node.Number * 6 + 5];
				if (flag6)
				{
					vector[pointLoad.Node.Number * 6 + 5] = pointLoad.Mz;
				}
			}
			return vector;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00020A88 File Offset: 0x0001EC88
		public Vector Getfred(Structure Structure, LoadCase LC)
		{
			bool flag = !Structure.Members.OfType<Beam>().Any<Beam>();
			Vector result;
			if (flag)
			{
				result = this.GetfredTruss(Structure, LC);
			}
			else
			{
				result = this.GetfredFull(Structure, LC);
			}
			return result;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00020AC8 File Offset: 0x0001ECC8
		private Vector GetfredTruss(Structure Structure, LoadCase LC)
		{
			Vector vector = new Vector(Structure.NFreeTranslations);
			foreach (ILoad load in LC.Loads)
			{
				PointLoad pointLoad = (PointLoad)load;
				bool flag = !pointLoad.Node.FixTx;
				if (flag)
				{
					vector[pointLoad.Node.ReducedDofsTruss[0]] = pointLoad.Fx;
				}
				bool flag2 = !pointLoad.Node.FixTy;
				if (flag2)
				{
					vector[pointLoad.Node.ReducedDofsTruss[1]] = pointLoad.Fy;
				}
				bool flag3 = !pointLoad.Node.FixTz;
				if (flag3)
				{
					vector[pointLoad.Node.ReducedDofsTruss[2]] = pointLoad.Fz;
				}
			}
			return vector;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		private Vector GetfredFull(Structure Structure, LoadCase LC)
		{
			Vector vector = new Vector(Structure.NFreeTranslations);
			foreach (ILoad load in LC.Loads)
			{
				PointLoad pointLoad = (PointLoad)load;
				bool flag = !pointLoad.Node.FixTx;
				if (flag)
				{
					vector[pointLoad.Node.ReducedDofs[0]] = pointLoad.Fx;
				}
				bool flag2 = !pointLoad.Node.FixTy;
				if (flag2)
				{
					vector[pointLoad.Node.ReducedDofs[1]] = pointLoad.Fy;
				}
				bool flag3 = !pointLoad.Node.FixTz;
				if (flag3)
				{
					vector[pointLoad.Node.ReducedDofs[2]] = pointLoad.Fz;
				}
				bool flag4 = !pointLoad.Node.FixRx;
				if (flag4)
				{
					vector[pointLoad.Node.ReducedDofs[4]] = pointLoad.Mx;
				}
				bool flag5 = !pointLoad.Node.FixRy;
				if (flag5)
				{
					vector[pointLoad.Node.ReducedDofs[5]] = pointLoad.My;
				}
				bool flag6 = !pointLoad.Node.FixRz;
				if (flag6)
				{
					vector[pointLoad.Node.ReducedDofs[6]] = pointLoad.Mz;
				}
			}
			return vector;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00020D98 File Offset: 0x0001EF98
		private Vector Extendu(Structure Structure, Vector ured)
		{
			Vector vector = new Vector(Structure.Nodes.Count * 6);
			foreach (Node node in Structure.Nodes)
			{
				bool flag = !Structure.Members.OfType<Beam>().Any<Beam>();
				if (flag)
				{
					for (int i = 0; i < 3; i++)
					{
						bool flag2 = !node.Fix[i];
						if (flag2)
						{
							vector[node.Number * 6 + i] = ured[node.ReducedDofsTruss[i]];
						}
					}
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						bool flag3 = !node.Fix[j];
						if (flag3)
						{
							vector[node.Number * 6 + j] = ured[node.ReducedDofs[j]];
						}
					}
				}
			}
			return vector;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00020EC4 File Offset: 0x0001F0C4
		public override Vector Getu(Structure Structure, LoadCase LC)
		{
			Vector result;
			this.Solve(Structure, LC, out result, new FEAOptions());
			return result;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00020EE8 File Offset: 0x0001F0E8
		private static void AddElementStiffness(ref MatrixDense K, IMember M)
		{
			IMember1D member1D = M as IMember1D;
			bool flag = member1D != null;
			if (flag)
			{
				bool flag2 = member1D is Bar;
				if (flag2)
				{
					FiniteElement finiteElement = new Bar3D(member1D);
					for (int i = 0; i < finiteElement.Dofs.Length; i++)
					{
						for (int j = 0; j < finiteElement.Dofs.Length; j++)
						{
							MatrixDense matrixDense = K;
							int num = finiteElement.Dofs[i];
							int num2 = finiteElement.Dofs[j];
							matrixDense[num, num2] += finiteElement.K[i, j];
						}
					}
				}
				else
				{
					bool flag3 = member1D is Beam;
					if (flag3)
					{
						FiniteElement finiteElement = new Beam3D(member1D);
						for (int k = 0; k < finiteElement.Dofs.Length; k++)
						{
							for (int l = 0; l < finiteElement.Dofs.Length; l++)
							{
								MatrixDense matrixDense = K;
								int num2 = finiteElement.Dofs[k];
								int num = finiteElement.Dofs[l];
								matrixDense[num2, num] += finiteElement.K[k, l];
							}
						}
					}
				}
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002103C File Offset: 0x0001F23C
		private static void SetMemberForces(Structure str, LoadCase LC, Vector u)
		{
			for (int i = 0; i < str.Members.Count; i++)
			{
				IMember1D member1D = str.Members[i] as IMember1D;
				bool flag = member1D != null;
				if (flag)
				{
					foreach (Node node in str.Nodes)
					{
						bool flag2 = node == member1D.From;
						if (flag2)
						{
							member1D.From = node;
						}
						bool flag3 = node == member1D.To;
						if (flag3)
						{
							member1D.To = node;
						}
					}
					Bar bar = member1D as Bar;
					bool flag4 = bar != null;
					if (flag4)
					{
						FiniteElement finiteElement = new Bar3D(member1D);
						Vector localDisplacements = finiteElement.GetLocalDisplacements(u, member1D);
						double num = localDisplacements[1] - localDisplacements[0];
						bar.AddNormalForce(LC, new List<double>
						{
							num * (member1D.Material.E * member1D.CrossSection.Area / member1D.Length)
						});
					}
					else
					{
						bool flag5 = member1D is Beam;
						if (flag5)
						{
							FiniteElement finiteElement = new Beam3D(member1D);
							Vector localDisplacements2 = finiteElement.GetLocalDisplacements(u, member1D);
							Vector vector = finiteElement.K * localDisplacements2;
						}
					}
				}
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000211AC File Offset: 0x0001F3AC
		private static void SetStructureDisplacements(Structure str, LoadCase LC, Vector u)
		{
			foreach (Node node in str.Nodes)
			{
				node.AddDisplacement(LC, new double[]
				{
					u[node.Number * 6],
					u[node.Number * 6 + 1],
					u[node.Number * 6 + 2],
					u[node.Number * 6 + 3],
					u[node.Number * 6 + 4],
					u[node.Number * 6 + 5]
				});
			}
		}
	}
}
