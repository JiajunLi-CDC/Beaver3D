using System;
using Beaver3D.LinearAlgebra;

namespace Beaver3D.Model
{
	// Token: 0x02000023 RID: 35
	public class DisplacementBound
	{
		// 节点
		public Node Node { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000F85C File Offset: 0x0000DA5C
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000F87A File Offset: 0x0000DA7A
		public double LBX
		{
			get
			{
				return this.LB[0];
			}
			set
			{
				this.LB[0] = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000F88C File Offset: 0x0000DA8C
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000F8AA File Offset: 0x0000DAAA
		public double LBY
		{
			get
			{
				return this.LB[1];
			}
			set
			{
				this.LB[1] = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000F8BC File Offset: 0x0000DABC
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000F8DA File Offset: 0x0000DADA
		public double LBZ
		{
			get
			{
				return this.LB[2];
			}
			set
			{
				this.LB[2] = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000F8EC File Offset: 0x0000DAEC
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000F90A File Offset: 0x0000DB0A
		public double LBRX
		{
			get
			{
				return this.LB[3];
			}
			set
			{
				this.LB[3] = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000F91C File Offset: 0x0000DB1C
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000F93A File Offset: 0x0000DB3A
		public double LBRY
		{
			get
			{
				return this.LB[4];
			}
			set
			{
				this.LB[4] = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000F94C File Offset: 0x0000DB4C
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000F96A File Offset: 0x0000DB6A
		public double LBRZ
		{
			get
			{
				return this.LB[5];
			}
			set
			{
				this.LB[5] = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000F97C File Offset: 0x0000DB7C
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000F99A File Offset: 0x0000DB9A
		public double UBX
		{
			get
			{
				return this.UB[0];
			}
			set
			{
				this.UB[0] = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000F9AC File Offset: 0x0000DBAC
		// (set) Token: 0x06000166 RID: 358 RVA: 0x0000F9CA File Offset: 0x0000DBCA
		public double UBY
		{
			get
			{
				return this.UB[1];
			}
			set
			{
				this.UB[1] = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000F9DC File Offset: 0x0000DBDC
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000F9FA File Offset: 0x0000DBFA
		public double UBZ
		{
			get
			{
				return this.UB[2];
			}
			set
			{
				this.UB[2] = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000FA0C File Offset: 0x0000DC0C
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000FA2A File Offset: 0x0000DC2A
		public double UBRX
		{
			get
			{
				return this.UB[3];
			}
			set
			{
				this.UB[3] = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000FA5A File Offset: 0x0000DC5A
		public double UBRY
		{
			get
			{
				return this.UB[4];
			}
			set
			{
				this.UB[4] = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000FA8A File Offset: 0x0000DC8A
		public double UBRZ
		{
			get
			{
				return this.UB[5];
			}
			set
			{
				this.UB[5] = value;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000FA9C File Offset: 0x0000DC9C
		public DisplacementBound(Node node, double lbx, double lby, double lbz, double lbrx, double lbry, double lbrz, double ubx, double uby, double ubz, double ubrx, double ubry, double ubrz)
		{
			this.Node = node;
			this.LBX = lbx;
			this.LBY = lby;
			this.LBZ = lbz;
			this.LBRX = lbrx;
			this.LBRY = lbry;
			this.LBRZ = lbrz;
			this.UBX = ubx;
			this.UBY = uby;
			this.UBZ = ubz;
			this.UBRX = ubrx;
			this.UBRY = ubry;
			this.UBRZ = ubrz;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000FB3C File Offset: 0x0000DD3C
		public DisplacementBound(Node node, double lbx, double lby, double lbz, double ubx, double uby, double ubz) : this(node, lbx, lby, lbz, -1E+100, -1E+100, -1E+100, ubx, uby, ubz, 1E+100, 1E+100, 1E+100)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000FB94 File Offset: 0x0000DD94
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
				DisplacementBound displacementBound = (DisplacementBound)obj;
				result = this.Node.Equals(displacementBound.Node);
			}
			return result;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
		public override int GetHashCode()
		{
			return this.Node.GetHashCode();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000FC18 File Offset: 0x0000DE18
		public DisplacementBound Clone()
		{
			return new DisplacementBound(this.Node, this.LBX, this.LBY, this.LBZ, this.LBRX, this.LBRY, this.LBRZ, this.UBX, this.UBY, this.UBZ, this.UBRX, this.UBRY, this.UBRZ);
		}

		// Token: 0x0400008E RID: 142
		public int LoadcaseNumber;

		// Token: 0x04000090 RID: 144
		internal Vector LB = new Vector(6);

		// Token: 0x04000091 RID: 145
		internal Vector UB = new Vector(6);
	}
}
