using System;
using System.Collections;
using System.Linq;

namespace Beaver3D.LinearAlgebra
{
	// Token: 0x02000041 RID: 65
	public class Vector : IEnumerator, IEnumerable
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001E95B File Offset: 0x0001CB5B
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001E963 File Offset: 0x0001CB63
		public int Length { get; private set; } = 0;

		// Token: 0x17000207 RID: 519
		public double this[int i]
		{
			get
			{
				return this.Data[i];
			}
			set
			{
				this.Data[i] = value;
			}
		}

		// Token: 0x17000208 RID: 520
		public Vector this[int[] indices]
		{
			get
			{
				Vector vector = new Vector(indices.Length);
				for (int i = 0; i < indices.Length; i++)
				{
					vector[i] = this.Data[indices[i]];
				}
				return vector;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001E9D4 File Offset: 0x0001CBD4
		internal Vector()
		{
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001E9F8 File Offset: 0x0001CBF8
		public Vector(int n)
		{
			bool flag = n < 1;
			if (flag)
			{
				throw new ArgumentException("Vector Length must be >= 1");
			}
			this.Length = n;
			this.Data = new double[n];
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001EA50 File Offset: 0x0001CC50
		public Vector(int n, double x)
		{
			bool flag = n < 1;
			if (flag)
			{
				throw new ArgumentException("Vector Length must be >= 1");
			}
			this.Length = n;
			this.Data = Enumerable.Repeat<double>(x, n).ToArray<double>();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001EAAD File Offset: 0x0001CCAD
		public Vector(double[] data)
		{
			this.Length = data.Length;
			this.Data = data;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001EAE2 File Offset: 0x0001CCE2
		public Vector(Vector V) : this(V.Data)
		{
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001EAF4 File Offset: 0x0001CCF4
		public static Vector UnitX()
		{
			Vector vector = new Vector(3);
			vector[0] = 1.0;
			return vector;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001EB20 File Offset: 0x0001CD20
		public static Vector UnitY()
		{
			Vector vector = new Vector(3);
			vector[1] = 1.0;
			return vector;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001EB4C File Offset: 0x0001CD4C
		public static Vector UnitZ()
		{
			Vector vector = new Vector(3);
			vector[2] = 1.0;
			return vector;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001EB78 File Offset: 0x0001CD78
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001EB8C File Offset: 0x0001CD8C
		public bool MoveNext()
		{
			this.position++;
			return this.position < this.Data.Length;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001EBBC File Offset: 0x0001CDBC
		public void Reset()
		{
			this.position = 0;
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		public object Current
		{
			get
			{
				return this.Data[this.position];
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001EBEC File Offset: 0x0001CDEC
		internal void Add(double x)
		{
			double[] array = new double[this.Length + 1];
			for (int i = 0; i < this.Length; i++)
			{
				array[i] = this.Data[i];
			}
			array[this.Length] = x;
			this.Data = array;
			int length = this.Length;
			this.Length = length + 1;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001EC4C File Offset: 0x0001CE4C
		public void Scale(double s)
		{
			for (int i = 0; i < this.Length; i++)
			{
				int i2 = i;
				this[i2] *= s;
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001EC84 File Offset: 0x0001CE84
		public void AddScalar(double s)
		{
			for (int i = 0; i < this.Length; i++)
			{
				int i2 = i;
				this[i2] += s;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001ECBC File Offset: 0x0001CEBC
		public void AddVector(Vector B)
		{
			bool flag = this.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("Add To: Vectors not of same Length");
			}
			for (int i = 0; i < this.Length; i++)
			{
				int i2 = i;
				this[i2] += B[i];
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001ED1C File Offset: 0x0001CF1C
		public void PointwiseMultiply(Vector B)
		{
			bool flag = this.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("PointwiseMultiply by To: Vectors not of same Length");
			}
			for (int i = 0; i < this.Length; i++)
			{
				int i2 = i;
				this[i2] *= B[i];
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		public void Unitize()
		{
			double num = this.Norm();
			for (int i = 0; i < this.Length; i++)
			{
				this.Data[i] /= num;
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001EDB8 File Offset: 0x0001CFB8
		public Vector GetUnitizedVector()
		{
			Vector vector = new Vector(this);
			vector.Unitize();
			return vector;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001EDDC File Offset: 0x0001CFDC
		public double Norm()
		{
			double num = 0.0;
			for (int i = 0; i < this.Length; i++)
			{
				num += this.Data[i] * this.Data[i];
			}
			return Math.Sqrt(num);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001EE28 File Offset: 0x0001D028
		public static Vector CrossProduct(Vector A, Vector B)
		{
			bool flag = A.Length != 3 || B.Length != 3;
			if (flag)
			{
				throw new ArgumentException("Cross Product Vectors are not of size 3x1");
			}
			Vector vector = new Vector(3);
			vector[0] = A[1] * B[2] - A[2] * B[1];
			vector[1] = A[2] * B[0] - A[0] * B[2];
			vector[2] = A[0] * B[1] - A[1] * B[0];
			return vector;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001EEE0 File Offset: 0x0001D0E0
		public static double DotProduct(Vector A, Vector B)
		{
			bool flag = A.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("Dot Product: Vectors not of same length");
			}
			double num = 0.0;
			for (int i = 0; i < A.Length; i++)
			{
				num += A[i] * B[i];
			}
			return num;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001EF4C File Offset: 0x0001D14C
		public static double VectorAngle(Vector A, Vector B)
		{
			bool flag = A.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("Vector Angle: Vectors not of same length");
			}
			return Math.Acos(Vector.DotProduct(A, B) / (A.Norm() * B.Norm()));
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001EF9C File Offset: 0x0001D19C
		public double[] ToDouble()
		{
			return this.Data;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		public override string ToString()
		{
			bool flag = this.Data.Length <= 6;
			string result;
			if (flag)
			{
				string text = "(";
				for (int i = 0; i < this.Data.Length; i++)
				{
					text += Math.Round(this.Data[i], 3).ToString();
					bool flag2 = i < this.Data.Length - 1;
					if (flag2)
					{
						text += ",";
					}
				}
				text += ")";
				result = text;
			}
			else
			{
				result = "Beaver3D Vector of Length " + this.Data.Length.ToString();
			}
			return result;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001F068 File Offset: 0x0001D268
		public static Vector operator +(Vector A, double s)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] + s;
			}
			return vector;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001F0B0 File Offset: 0x0001D2B0
		public static Vector operator +(double s, Vector A)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] + s;
			}
			return vector;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001F0F8 File Offset: 0x0001D2F8
		public static Vector operator +(Vector A, Vector B)
		{
			bool flag = A.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("A + To: Vectors not of same Length");
			}
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] + B[i];
			}
			return vector;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001F168 File Offset: 0x0001D368
		public static Vector operator -(Vector A)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = -A[i];
			}
			return vector;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		public static Vector operator -(Vector A, double s)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] - s;
			}
			return vector;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
		public static Vector operator -(double s, Vector A)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = s - A[i];
			}
			return vector;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001F23C File Offset: 0x0001D43C
		public static Vector operator -(Vector A, Vector B)
		{
			bool flag = A.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("A + To: Vectors not of same Length");
			}
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] - B[i];
			}
			return vector;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		public static Vector operator *(Vector A, double s)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] * s;
			}
			return vector;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
		public static Vector operator *(double s, Vector A)
		{
			Vector vector = new Vector(A.Length);
			for (int i = 0; i < A.Length; i++)
			{
				vector[i] = A[i] * s;
			}
			return vector;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001F33C File Offset: 0x0001D53C
		public static double operator *(Vector A, Vector B)
		{
			bool flag = A.Length != B.Length;
			if (flag)
			{
				throw new ArgumentException("A * B: Vectors not of same Length");
			}
			double num = 0.0;
			for (int i = 0; i < A.Length; i++)
			{
				num += A[i] * B[i];
			}
			return num;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001F3A4 File Offset: 0x0001D5A4
		public Vector Clone()
		{
			Vector vector = new Vector(this.Length);
			for (int i = 0; i < this.Length; i++)
			{
				vector[i] = this[i];
			}
			return vector;
		}

		// Token: 0x040001FF RID: 511
		private double[] Data = new double[0];

		// Token: 0x04000200 RID: 512
		private int position = -1;
	}
}
