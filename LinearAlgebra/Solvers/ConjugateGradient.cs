using System;

namespace Beaver3D.LinearAlgebra.Solvers
{
	// Token: 0x02000042 RID: 66
	public static class ConjugateGradient
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0001F3E8 File Offset: 0x0001D5E8
		public static Vector Solve(MatrixDense A, Vector b, int max_iter = 60, double epsilon = 0.001)
		{
			bool flag = A.NRows != A.NColumns;
			if (flag)
			{
				throw new ArgumentException("Matrix A is not square");
			}
			bool flag2 = b.Length != A.NColumns;
			if (flag2)
			{
				throw new ArgumentException("NColumns of A not equal Length of b");
			}
			Vector vector = new Vector(A.NColumns);
			Vector vector2 = b - A * vector;
			Vector vector3 = vector2.Clone();
			double num = vector2 * vector2;
			double num2 = num;
			int num3 = 0;
			while (num3 < max_iter && num > epsilon * epsilon * num2)
			{
				Vector vector4 = A * vector3;
				double s = num / (vector3 * vector4);
				vector += s * vector3;
				bool flag3 = num3 % 10 == 0;
				if (flag3)
				{
					vector2 = b - A * vector;
				}
				else
				{
					vector2 -= s * vector4;
				}
				double num4 = num;
				num = vector2 * vector2;
				double s2 = num / num4;
				vector3 = vector2 + s2 * vector3;
				num3++;
			}
			bool flag4 = num3 < max_iter;
			if (flag4)
			{
				Console.WriteLine("Conjugate Gradient Solver converged in " + num3.ToString() + " iterations");
			}
			else
			{
				Console.WriteLine("Conjugate Gradient Solver converged did not converge within the maximum number of iterations: " + max_iter.ToString());
			}
			return vector;
		}
	}
}
