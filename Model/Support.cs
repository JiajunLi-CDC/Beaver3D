using System;
using System.Linq;

namespace Beaver3D.Model
{
	// Token: 0x0200002D RID: 45
	public class Support
	{
		//节点位置
		public Node Node { get; internal set; }

		// 是否固定
		public bool[] Fix { get; private set; }


		public bool FixTx
		{
			get
			{
				return this.Fix[0];
			}
			set
			{
				this.Fix[0] = value;
			}
		}

		public bool FixTy
		{
			get
			{
				return this.Fix[1];
			}
			set
			{
				this.Fix[1] = value;
			}
		}

		public bool FixTz
		{
			get
			{
				return this.Fix[2];
			}
			set
			{
				this.Fix[2] = value;
			}
		}

		public bool FixRx
		{
			get
			{
				return this.Fix[3];
			}
			set
			{
				this.Fix[3] = value;
			}
		}


		public bool FixRy
		{
			get
			{
				return this.Fix[4];
			}
			set
			{
				this.Fix[4] = value;
			}
		}

		public bool FixRz
		{
			get
			{
				return this.Fix[5];
			}
			set
			{
				this.Fix[5] = value;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000137D2 File Offset: 0x000119D2
		public Support(Node N) : this(N, false, false, false, false, false, false)
		{
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000137E4 File Offset: 0x000119E4
		public Support(Node N, bool FixTX, bool FixTY, bool FixTZ, bool FixRX = false, bool FixRY = false, bool FixRZ = false)
		{
			this.Fix = new bool[6];
			this.Fix[0] = FixTX;
			this.Fix[1] = FixTY;
			this.Fix[2] = FixTZ;
			this.Fix[3] = FixRX;
			this.Fix[4] = FixRY;
			this.Fix[5] = FixRZ;
			this.Node = N;
		}

		// 设置x移动
		public void ToggleTx(bool value)
		{
			this.FixTx = value;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00013853 File Offset: 0x00011A53
		public void ToggleTy(bool value)
		{
			this.FixTy = value;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001385E File Offset: 0x00011A5E
		public void ToggleTz(bool value)
		{
			this.FixTz = value;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00013869 File Offset: 0x00011A69
		public void ToggleRx(bool value)
		{
			this.FixRx = value;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00013874 File Offset: 0x00011A74
		public void ToggleRy(bool value)
		{
			this.FixRy = value;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001387F File Offset: 0x00011A7F
		public void ToggleRz(bool value)
		{
			this.FixRz = value;
		}

		// 固定所有
		public void FixAll()
		{
			this.Fix = Enumerable.Repeat<bool>(true, 6).ToArray<bool>();
		}

		// 解除所有固定
		public void FreeAll()
		{
			this.Fix = new bool[6];
		}

		// 固定移动
		public void FixTranslations()
		{
			this.Fix[0] = true;
			this.Fix[1] = true;
			this.Fix[2] = true;
		}

		// 固定旋转
		public void FixRotations()
		{
			this.Fix[3] = true;
			this.Fix[4] = true;
			this.Fix[5] = true;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000138EC File Offset: 0x00011AEC
		public void FreeTranslations()
		{
			this.Fix[0] = false;
			this.Fix[1] = false;
			this.Fix[2] = false;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001390A File Offset: 0x00011B0A
		public void FreeRotations()
		{
			this.Fix[3] = false;
			this.Fix[4] = false;
			this.Fix[5] = false;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00013928 File Offset: 0x00011B28
		public Support Clone()
		{
			return new Support(this.Node, this.FixTx, this.FixTy, this.FixTz, this.FixRx, this.FixRy, this.FixRz);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001396C File Offset: 0x00011B6C
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
				Support support = (Support)obj;
				result = this.Node.Equals(support.Node);
			}
			return result;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000139C8 File Offset: 0x00011BC8
		public override int GetHashCode()
		{
			return this.Node.GetHashCode();
		}
	}
}
