using System;
using System.Collections.Generic;
using System.Linq;

namespace Beaver3D.Model
{
	// Token: 0x0200002A RID: 42
	public struct Node
	{
		//节点编号
		public int Number {  get; set; }
		//节点xyz坐标
		public double X {  get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		//xyz是否固定
		public bool[] Fix {  get; internal set; }

		//位置x固定
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

		//y固定
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

		//z固定
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

		// 旋转的x固定
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

		// 旋转的y固定
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

		// 旋转的z固定
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

		// 设置误差
		public double Tolerance {  get; set; }

		//减少自由度桁架
		public int[] ReducedDofsTruss {  get; internal set; }

		// 减少自由度
		public int[] ReducedDofs {  get; internal set; }

		// 节点的杆件连接信息
		public Dictionary<IMember, double> ConnectedMembers {  get; private set; }

		// 节点的荷载信息
		public Dictionary<LoadCase, PointLoad> PointLoads { get; private set; }

		// 节点的位移信息
		public Dictionary<LoadCase, double[]> Displacements {  get; private set; }

		// 节点的位移界限
		public Dictionary<LoadCase, DisplacementBound> DisplacementBounds {  get; private set; }

		// 实例化节点
		public Node(double X, double Y, double Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.Number = -1;
			this.Fix = new bool[6];
			this.Tolerance = 0.0001;

			this.ReducedDofsTruss = Enumerable.Repeat<int>(-1, 3).ToArray<int>();
			this.ReducedDofs = Enumerable.Repeat<int>(-1, 6).ToArray<int>();

			this.ConnectedMembers = new Dictionary<IMember, double>();
			this.PointLoads = new Dictionary<LoadCase, PointLoad>();
			this.Displacements = new Dictionary<LoadCase, double[]>();
			this.DisplacementBounds = new Dictionary<LoadCase, DisplacementBound>();
		}

		// 设置支座信息，这一步是在构造structure中完成的
		public void SetSupport(Support Support)
		{
			this.FixTx = Support.FixTx;
			this.FixTy = Support.FixTy;
			this.FixTz = Support.FixTz;
			this.FixRx = Support.FixRx;
			this.FixRy = Support.FixRy;
			this.FixRz = Support.FixRz;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00011E40 File Offset: 0x00010040
		public void SetNumber(int nb)
		{
			this.Number = nb;
		}

		// 施加荷载，这里是在构造structure的时候施加的
		internal void AddPointLoad(LoadCase LC, PointLoad PL)
		{
			bool flag = !this.PointLoads.ContainsKey(LC);   //是否不包含该荷载信息
			if (flag)
			{
				this.PointLoads.Add(LC, PL);
			}
			else
			{
				Dictionary<LoadCase, PointLoad> pointLoads = this.PointLoads;
				pointLoads[LC] += PL;    //添加已经包含lc名字的荷载列表中
			}
		}

		// 施加节点位移边界信息，也是在构造structure的时候施加的
		internal void AddDisplacementBound(LoadCase LC, DisplacementBound DB)
		{
			bool flag = !this.DisplacementBounds.ContainsKey(LC);
			if (flag)
			{
				this.DisplacementBounds.Add(LC, DB);
			}
			else
			{
				this.DisplacementBounds.Remove(LC);
				this.DisplacementBounds.Add(LC, DB);
			}
		}

		// 施加节点位移信息，FEA方法用
		internal void AddDisplacement(LoadCase LC, double[] displacements)
		{
			bool flag = !this.Displacements.ContainsKey(LC);
			if (flag)
			{
				this.Displacements.Add(LC, displacements);
			}
			else
			{
				this.Displacements.Remove(LC);
				this.Displacements.Add(LC, displacements);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00011F3E File Offset: 0x0001013E
		public void RemoveAllLoads()
		{
			this.PointLoads.Clear();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00011F4D File Offset: 0x0001014D
		public void RemoveAllDisplacementBounds()
		{
			this.DisplacementBounds.Clear();
		}

		// 解除所有支座约束
		public void FreeAllSupports()
		{
			this.FixTx = false;
			this.FixTy = false;
			this.FixTz = false;
			this.FixRx = false;
			this.FixRy = false;
			this.FixRz = false;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00011F8F File Offset: 0x0001018F
		public void ResetReducedDofs()
		{
			this.ReducedDofs = Enumerable.Repeat<int>(-1, 6).ToArray<int>();
			this.ReducedDofsTruss = Enumerable.Repeat<int>(-1, 3).ToArray<int>();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00011FB8 File Offset: 0x000101B8
		public List<PointLoad> GetPointLoadsFromLCNames(List<string> LoadCaseNames)
		{
			bool flag = LoadCaseNames.Count == 0;
			if (flag)
			{
				throw new ArgumentException("The list of LoadCase Names is empty");
			}
			bool flag2 = LoadCaseNames.Count == 1 && LoadCaseNames[0] == "all";
			List<PointLoad> result;
			if (flag2)
			{
				result = this.PointLoads.Values.ToList<PointLoad>();
			}
			else
			{
				List<PointLoad> list = new List<PointLoad>();
				foreach (string name in LoadCaseNames)
				{
					PointLoad item;
					bool flag3 = this.PointLoads.TryGetValue(new LoadCase(name, 1.0), out item);
					if (flag3)
					{
						list.Add(item);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0001208C File Offset: 0x0001028C
		public int GetNumber()
		{
			return this.Number;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000120A4 File Offset: 0x000102A4
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
				Node node = (Node)obj;
				double num = node.X - this.X;
				double num2 = node.Y - this.Y;
				double num3 = node.Z - this.Z;
				double num4 = Math.Sqrt(num * num + num2 * num2 + num3 * num3);
				bool flag2 = num4 < this.Tolerance;
				result = flag2;
			}
			return result;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00012148 File Offset: 0x00010348
		public override int GetHashCode()
		{
			return this.Number.GetHashCode();
		}

		// 添加相连杆件
		internal void AddConnectedMember(IMember Member, double Orientation)
		{
			bool flag = !this.ConnectedMembers.ContainsKey(Member);
			if (flag)
			{
				this.ConnectedMembers.Add(Member, Orientation);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00012197 File Offset: 0x00010397
		public static bool operator ==(Node A, Node B)
		{
			return A.Equals(B);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000121AC File Offset: 0x000103AC
		public static bool operator !=(Node A, Node B)
		{
			return !A.Equals(B);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000121C4 File Offset: 0x000103C4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Node ",
				this.Number.ToString(),
				" (",
				this.X.ToString(),
				",",
				this.Y.ToString(),
				",",
				this.Z.ToString(),
				") Fix [",
				Convert.ToInt16(this.FixTx).ToString(),
				",",
				Convert.ToInt16(this.FixTy).ToString(),
				",",
				Convert.ToInt16(this.FixTz).ToString(),
				",",
				Convert.ToInt16(this.FixRx).ToString(),
				",",
				Convert.ToInt16(this.FixRy).ToString(),
				",",
				Convert.ToInt16(this.FixRz).ToString(),
				"]"
			});
		}
	}
}
