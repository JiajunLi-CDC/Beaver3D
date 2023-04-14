using System;
using Beaver3D.LinearAlgebra.Solvers;

namespace Beaver3D.LinearAlgebra
{
	// Token: 0x0200003F RID: 63
	public class MatrixDense
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001DDA1 File Offset: 0x0001BFA1
		public int NRows { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001DDA9 File Offset: 0x0001BFA9
		public int NColumns { get; }

		// Token: 0x17000205 RID: 517
		public double this[int i, int j]
		{
			get
			{
				return this.Data[i, j];
			}
			set
			{
				this.Data[i, j] = value;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001DDE4 File Offset: 0x0001BFE4
		public MatrixDense(int n)
		{
			bool flag = n < 1;
			if (flag)
			{
				throw new ArgumentException("Size of Matrix should be n >= 1");
			}
			this.NColumns = n;
			this.NRows = n;
			this.Data = new double[n, n];
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001DE2C File Offset: 0x0001C02C
		public MatrixDense(int n, double x)
		{
			bool flag = n < 1;
			if (flag)
			{
				throw new ArgumentException("Size of Matrix should be n >= 1");
			}
			this.NColumns = n;
			this.NRows = n;
			this.Data = new double[n, n];
			this.FillDiagonal(x);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001DE7C File Offset: 0x0001C07C
		public MatrixDense(int m, int n)
		{
			bool flag = m < 1;
			if (flag)
			{
				throw new ArgumentException("Number of Rows m should be >= 1");
			}
			bool flag2 = n < 1;
			if (flag2)
			{
				throw new ArgumentException("Number of Columns n should be >= 1");
			}
			this.NRows = m;
			this.NColumns = n;
			this.Data = new double[m, n];
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001DED4 File Offset: 0x0001C0D4
		public MatrixDense(int m, int n, double x)
		{
			bool flag = m < 1;
			if (flag)
			{
				throw new ArgumentException("Number of Rows m should be >= 1");
			}
			bool flag2 = n < 1;
			if (flag2)
			{
				throw new ArgumentException("Number of Columns n should be >= 1");
			}
			this.NRows = m;
			this.NColumns = n;
			this.Data = new double[m, n];
			this.Fill(x);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001DF34 File Offset: 0x0001C134
		public MatrixDense(double[,] data)
		{
			this.NRows = data.GetLength(0);
			this.NColumns = data.GetLength(1);
			this.Data = data;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001DF5F File Offset: 0x0001C15F
		public MatrixDense(MatrixDense A) : this(A.Data)
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001DF70 File Offset: 0x0001C170
		private void Fill(double x)
		{
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					this.Data[i, j] = x;
				}
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001DFB8 File Offset: 0x0001C1B8
		private void FillDiagonal(double x)
		{
			bool flag = this.NRows != this.NColumns;
			if (flag)
			{
				throw new ArgumentException("Cannot create diagonal entries as Matrix is not square");
			}
			for (int i = 0; i < this.NRows; i++)
			{
				this.Data[i, i] = x;
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001E00C File Offset: 0x0001C20C
		public MatrixDense Transpose()
		{
			MatrixDense matrixDense = new MatrixDense(this.NColumns, this.NRows);
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					matrixDense[j, i] = this[i, j];
				}
			}
			return matrixDense;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001E070 File Offset: 0x0001C270
		public MatrixDense PointwiseMultiply(MatrixDense B)
		{
			bool flag = this.NRows != B.NRows || this.NColumns != B.NColumns;
			if (flag)
			{
				throw new ArgumentException("PointsiweMultiply by To: Matrices not of same size");
			}
			MatrixDense matrixDense = new MatrixDense(this.NRows, this.NColumns);
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					matrixDense[i, j] = this[i, j] * B[i, j];
				}
			}
			return matrixDense;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001E114 File Offset: 0x0001C314
		public void Scale(double s)
		{
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					int i2 = i;
					int j2 = j;
					this[i2, j2] *= s;
				}
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001E168 File Offset: 0x0001C368
		public void AddScalar(double s)
		{
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					int i2 = i;
					int j2 = j;
					this[i2, j2] += s;
				}
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001E1BC File Offset: 0x0001C3BC
		public void AddMatrix(MatrixDense B)
		{
			bool flag = this.NRows != B.NRows || this.NColumns != B.NColumns;
			if (flag)
			{
				throw new ArgumentException("Add To: Matrices not of same size");
			}
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					int i2 = i;
					int j2 = j;
					this[i2, j2] += B[i, j];
				}
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001E24C File Offset: 0x0001C44C
		public static MatrixDense BlockDiagonal(MatrixDense A, int n)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows * n, A.NColumns * n);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < A.NRows; j++)
				{
					for (int k = 0; k < A.NColumns; k++)
					{
						matrixDense[i * A.NRows + j, i * A.NColumns + k] = A[j, k];
					}
				}
			}
			return matrixDense;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001E2DC File Offset: 0x0001C4DC
		public double[,] ToDouble()
		{
			return this.Data;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001E2F4 File Offset: 0x0001C4F4
		public MatrixDense Clone()
		{
			MatrixDense matrixDense = new MatrixDense(this.NRows, this.NColumns);
			for (int i = 0; i < this.NRows; i++)
			{
				for (int j = 0; j < this.NColumns; j++)
				{
					double value = this.Data[i, j];
					matrixDense[i, j] = value;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001E364 File Offset: 0x0001C564
		public static MatrixDense operator +(MatrixDense A, double s)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] + s;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001E3CC File Offset: 0x0001C5CC
		public static MatrixDense operator +(double s, MatrixDense A)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] + s;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001E434 File Offset: 0x0001C634
		public static MatrixDense operator +(MatrixDense A, MatrixDense B)
		{
			bool flag = A.NRows != B.NRows || A.NColumns != B.NColumns;
			if (flag)
			{
				throw new ArgumentException("A plus To: Matrices not of same size");
			}
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] + B[i, j];
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
		public static MatrixDense operator -(MatrixDense A)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = -A[i, j];
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001E540 File Offset: 0x0001C740
		public static MatrixDense operator -(MatrixDense A, MatrixDense B)
		{
			bool flag = A.NRows != B.NRows || A.NColumns != B.NColumns;
			if (flag)
			{
				throw new ArgumentException("A minus To: Matrices not of same size");
			}
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] - B[i, j];
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001E5E4 File Offset: 0x0001C7E4
		public static MatrixDense operator *(MatrixDense A, double s)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] * s;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001E64C File Offset: 0x0001C84C
		public static MatrixDense operator *(double s, MatrixDense A)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] * s;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
		public static MatrixDense operator *(MatrixDense A, MatrixDense B)
		{
			bool flag = A.NColumns != B.NRows;
			if (flag)
			{
				throw new ArgumentException("A times To: Number of Columns in not equal to Number of rows in To");
			}
			MatrixDense matrixDense = new MatrixDense(A.NRows, B.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < B.NColumns; j++)
				{
					for (int k = 0; k < B.NRows; k++)
					{
						MatrixDense matrixDense2 = matrixDense;
						int i2 = i;
						int j2 = j;
						matrixDense2[i2, j2] += A[i, k] * B[k, j];
					}
				}
			}
			return matrixDense;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001E77C File Offset: 0x0001C97C
		public static Vector operator *(MatrixDense A, Vector x)
		{
			bool flag = A.NColumns != x.Length;
			if (flag)
			{
				throw new ArgumentException("A times To: Number of Columns is not equal to Number of rows in To");
			}
			Vector vector = new Vector(A.NRows);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					Vector vector2 = vector;
					int i2 = i;
					vector2[i2] += A[i, j] * x[j];
				}
			}
			return vector;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001E818 File Offset: 0x0001CA18
		public static Vector operator *(Vector x, MatrixDense A)
		{
			bool flag = A.NRows != x.Length;
			if (flag)
			{
				throw new ArgumentException("A times To: Number of Columns in not equal to Number of rows in To");
			}
			Vector vector = new Vector(A.NColumns);
			for (int i = 0; i < A.NColumns; i++)
			{
				for (int j = 0; j < A.NRows; j++)
				{
					Vector vector2 = vector;
					int i2 = i;
					vector2[i2] += A[j, i] * x[j];
				}
			}
			return vector;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001E8B4 File Offset: 0x0001CAB4
		public static MatrixDense operator /(MatrixDense A, double s)
		{
			MatrixDense matrixDense = new MatrixDense(A.NRows, A.NColumns);
			for (int i = 0; i < A.NRows; i++)
			{
				for (int j = 0; j < A.NColumns; j++)
				{
					matrixDense[i, j] = A[i, j] / s;
				}
			}
			return matrixDense;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001E91C File Offset: 0x0001CB1C
		public static Vector Solve(MatrixDense A, Vector b, MatrixSolver Solver = MatrixSolver.GaussJordan, int CGIterations = 60, double CGtolerance = 0.001)
		{
			Vector result;
			if (Solver != MatrixSolver.GaussJordan)
			{
				if (Solver != MatrixSolver.ConjugateGradient)
				{
					result = GaussJordan.Solve(A, b);
				}
				else
				{
					result = ConjugateGradient.Solve(A, b, CGIterations, CGtolerance);
				}
			}
			else
			{
				result = GaussJordan.Solve(A, b);
			}
			return result;
		}

		// Token: 0x040001F9 RID: 505
		private readonly double[,] Data;
	}
}
