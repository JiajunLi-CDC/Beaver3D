using System;
using Beaver3D.LinearAlgebra;

namespace Beaver3D.Model
{
	// Token: 0x02000024 RID: 36
	public class PointLoad : ILoad
	{
		//点荷载的位置
		public Node Node { get; set; }

		//x方向的荷载大小
		public double Fx
		{
			get
			{
				return this.FM[0];
			}
			set
			{
				this.FM[0] = value;
			}
		}

		// 
		public double Fy
		{
			get
			{
				return this.FM[1];
			}
			set
			{
				this.FM[1] = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000FCF0 File Offset: 0x0000DEF0
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000FD0E File Offset: 0x0000DF0E
		public double Fz
		{
			get
			{
				return this.FM[2];
			}
			set
			{
				this.FM[2] = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000FD20 File Offset: 0x0000DF20
		// (set) Token: 0x0600017D RID: 381 RVA: 0x0000FD3E File Offset: 0x0000DF3E
		public double Mx
		{
			get
			{
				return this.FM[3];
			}
			set
			{
				this.FM[3] = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000FD50 File Offset: 0x0000DF50
		// (set) Token: 0x0600017F RID: 383 RVA: 0x0000FD6E File Offset: 0x0000DF6E
		public double My
		{
			get
			{
				return this.FM[4];
			}
			set
			{
				this.FM[4] = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000FD80 File Offset: 0x0000DF80
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000FD9E File Offset: 0x0000DF9E
		public double Mz
		{
			get
			{
				return this.FM[5];
			}
			set
			{
				this.FM[5] = value;
			}
		}

		// 
		public PointLoad(Node node, double fx, double fy, double fz, double mx, double my, double mz)
		{
			this.Node = node;
			this.Fx = fx;
			this.Fy = fy;
			this.Fz = fz;
			this.Mx = mx;
			this.My = my;
			this.Mz = mz;
		}

		// 设置点荷载
		public PointLoad(Node node, double fx, double fy, double fz) : this(node, fx, fy, fz, 0.0, 0.0, 0.0)
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000FE38 File Offset: 0x0000E038
		public override bool Equals(object obj)
		{
			bool flag = obj == null || !base.GetType().Equals(obj.GetType());
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				PointLoad pointLoad = (PointLoad)obj;
				result = this.Node.Equals(pointLoad.Node);
			}
			return result;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000FE94 File Offset: 0x0000E094
		public override int GetHashCode()
		{
			return this.Node.GetHashCode();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		public void AddPointLoad(PointLoad PL)
		{
			bool flag = this.Node != PL.Node;
			if (flag)
			{
				throw new ArgumentException("The provided PointLoad cannot be added to this PointLoad because they are not acting on the same node.");
			}
			this.FM += PL.FM;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000FF04 File Offset: 0x0000E104
		public PointLoad Clone()
		{
			return new PointLoad(this.Node, this.Fx, this.Fy, this.Fz, this.Mx, this.My, this.Mz);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000FF48 File Offset: 0x0000E148
		public static PointLoad operator +(PointLoad A, PointLoad B)
		{
			bool flag = A.Node != B.Node;
			if (flag)
			{
				throw new ArgumentException("The PointLoads cannot be added because they are not acting on the same node.");
			}
			return new PointLoad(A.Node, A.Fx + B.Fx, A.Fy + B.Fy, A.Fz + B.Fz, A.Mx + B.Mx, A.My + B.My, A.Mz + B.Mz);
		}

		// Token: 0x04000092 RID: 146
		public int LoadcaseNumber;

		// 记录6个方向的荷载大小
		internal Vector FM = new Vector(6);
	}
}
