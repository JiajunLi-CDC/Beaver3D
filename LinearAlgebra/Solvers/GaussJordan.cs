using System;

namespace Beaver3D.LinearAlgebra.Solvers
{
	// Token: 0x02000043 RID: 67
	public static class GaussJordan
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x0001F54C File Offset: 0x0001D74C
		public static Vector Solve(MatrixDense A, Vector b)
		{
			MatrixDense matrixDense;
			return GaussJordan.Solve(A, b, out matrixDense);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001F568 File Offset: 0x0001D768
		internal static Vector Solve(MatrixDense A0, Vector b, out MatrixDense I)
		{
			MatrixDense matrixDense = A0.Clone();
			bool flag = matrixDense.NRows != matrixDense.NColumns;
			if (flag)
			{
				throw new ArgumentException("Matrix A is not square");
			}
			bool flag2 = b.Length != matrixDense.NColumns;
			if (flag2)
			{
				throw new ArgumentException("NColumns of A not equal Length of b");
			}
			for (int i = 0; i < matrixDense.NRows; i++)
			{
				for (int j = i + 1; j < matrixDense.NRows; j++)
				{
					double num = -matrixDense[j, i] / matrixDense[i, i];
					int num2 = j;
					b[num2] += num * b[i];
					for (int k = i; k < matrixDense.NRows; k++)
					{
						MatrixDense matrixDense2 = matrixDense;
						num2 = j;
						int num3 = k;
						matrixDense2[num2, num3] += num * matrixDense[i, k];
					}
				}
			}
			for (int l = 0; l < matrixDense.NRows; l++)
			{
				bool flag3 = matrixDense[l, l] != 1.0;
				if (flag3)
				{
					double num4 = matrixDense[l, l];
					int num3 = l;
					b[num3] /= num4;
					for (int m = l; m < matrixDense.NRows; m++)
					{
						MatrixDense matrixDense2 = matrixDense;
						num3 = l;
						int num2 = m;
						matrixDense2[num3, num2] /= num4;
					}
				}
			}
			for (int n = matrixDense.NRows - 1; n >= 0; n--)
			{
				for (int num5 = n - 1; num5 >= 0; num5--)
				{
					double num6 = -matrixDense[num5, n] / matrixDense[n, n];
					int num2 = num5;
					b[num2] += num6 * b[n];
					for (int num7 = matrixDense.NRows - 1; num7 >= n - 1; num7--)
					{
						MatrixDense matrixDense2 = matrixDense;
						num2 = num5;
						int num3 = num7;
						matrixDense2[num2, num3] += num6 * matrixDense[n, num7];
					}
				}
			}
			I = matrixDense;
			return b;
		}
	}
}
