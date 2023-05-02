using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver3D.Optimization;

namespace Beaver3D.Model
{
	// Token: 0x02000025 RID: 37
	public class Bar : IMember1D, IMember
	{
		//杆件编号
		public int Number { get; set; } = -1;

		//杆件所属的结构编号
		public int GroupNumber { get; set; } = -1;

		//杆件材料
		public IMaterial Material { get; set; }

		//构件生产长度
		public double Production_length { get; set; }

		//杆件截面
		public ICrossSection CrossSection { get; set; }

		//杆件起点
		public Node From { get; set; }

		// 杆件终点
		public Node To { get; set; }

		// 几何属性记录
		public double CosX { get; set; }
		public double CosY { get; set; }
		public double CosZ { get; set; }

		// 杆件长度
		public double Length { get; set; }

		// 杆件方向
		public Vector Direction { get; set; } = new Vector(3);

		//杆件法向量，默认为z
		public Vector Normal { get; set; } = Vector.UnitZ();

		// 密集矩阵
		public MatrixDense T { get; set; } = new MatrixDense(3, 1.0);

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000100B2 File Offset: 0x0000E2B2
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000100BA File Offset: 0x0000E2BA
		public int MinCompound { get; set; } = 1;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000100C3 File Offset: 0x0000E2C3
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000100CB File Offset: 0x0000E2CB
		public int MaxCompound { get; set; } = 1;

		// 允许的截面
		public List<string> AllowedCrossSections { get; set; } = new List<string>();

		// 荷载字典
		public Dictionary<LoadCase, List<double>> Nx { get; set; } = new Dictionary<LoadCase, List<double>>();

		//允许的伸缩（误差）长度
		public ValueTuple<double, double> Buffer { get; set; } = new ValueTuple<double, double>(0.0, 0.0);

		// 屈曲类型
		public BucklingType BucklingType { get; set; } = BucklingType.Off;

		// 屈曲长度
		public double BucklingLength { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00010129 File Offset: 0x0000E329
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00010131 File Offset: 0x0000E331
		public double LBArea { get; set; } = 0.0;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0001013A File Offset: 0x0000E33A
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00010142 File Offset: 0x0000E342
		public double UBArea { get; set; } = double.PositiveInfinity;

		// 拓扑是否固定
		public bool TopologyFixed { get; set; } = false;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0001015C File Offset: 0x0000E35C
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00010164 File Offset: 0x0000E364
		public bool NormalUserDefined { get; set; } = false;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0001016D File Offset: 0x0000E36D
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00010175 File Offset: 0x0000E375
		public bool NormalOverwritten { get; set; } = false;

		// 设置分配
		public Assignment Assignment { get; set; } = new Assignment();

        public int structure_num { get; set; } = 0;

		// 实例化杆件
		public Bar(Node From, Node To)
		{
			this.From = From;
			this.To = To;
			this.SetGeometricProperties();  //设置几何属性
			this.SetMaterial(default(Steel));
			this.SetCrossSection(new CircularSection(0.05));
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000102A6 File Offset: 0x0000E4A6
		public void SetNumber(int MemberNumber)
		{
			this.Number = MemberNumber;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000102B1 File Offset: 0x0000E4B1
		public void SetGroupNumber(int GroupNumber)
		{
			this.GroupNumber = GroupNumber;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000102BC File Offset: 0x0000E4BC
		public void SetMaterial(IMaterial mat)
		{
			this.Material = mat;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000102C7 File Offset: 0x0000E4C7
		public void SetCrossSection(ICrossSection sec)
		{
			this.CrossSection = sec;
		}

		// 设置几何属性
		public void SetGeometricProperties()
		{
			//计算杆件的方向
			this.Direction[0] = this.To.X - this.From.X;
			this.Direction[1] = this.To.Y - this.From.Y;
			this.Direction[2] = this.To.Z - this.From.Z;
			//计算长度
			this.Length = Math.Sqrt(this.Direction[0] * this.Direction[0] + this.Direction[1] * this.Direction[1] + this.Direction[2] * this.Direction[2]);
			this.Direction.Unitize();

			this.CosX = this.Direction[0];
			this.CosY = this.Direction[1];
			this.CosZ = this.Direction[2];
			//设置密集矩阵
			this.SetT();
		}

		// 设置法向量
		public void SetNormal(Vector Normal)
		{
			bool flag = Normal.Length != 3;
			if (flag)
			{
				throw new ArgumentException("The defined Member Normal is not a 3x1 Vector");
			}
			bool flag2 = Normal.Norm() == 0.0;
			if (flag2)
			{
				throw new ArgumentException("The defined Member Normal has no direction [0,0,0]");
			}
			this.Normal = Normal.GetUnitizedVector();
			this.NormalUserDefined = true;
			this.SetT();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00010470 File Offset: 0x0000E670
		public void SetMinMaxCompoundSection(int MinCompound, int MaxCompound)
		{
			bool flag = MinCompound < 1 || MaxCompound < 1;
			if (flag)
			{
				throw new ArgumentException("Min and MaxCompound cannot be less than 1");
			}
			bool flag2 = MinCompound <= MaxCompound;
			if (flag2)
			{
				this.MinCompound = MinCompound;
				this.MaxCompound = MaxCompound;
			}
			else
			{
				this.MinCompound = MaxCompound;
				this.MaxCompound = MinCompound;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000104CA File Offset: 0x0000E6CA
		public void SetAllowedCrossSections(List<string> TypeList)
		{
			this.AllowedCrossSections = TypeList;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000104D5 File Offset: 0x0000E6D5
		public void AddAllowedCrossSection(string CSType)
		{
			this.AllowedCrossSections.Add(CSType);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000104E5 File Offset: 0x0000E6E5
		public void SetBufferLengths(double Buffer0, double Buffer1)
		{
			this.Buffer = new ValueTuple<double, double>(Buffer0, Buffer1);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000104F6 File Offset: 0x0000E6F6
		public void SetBuckling(BucklingType BucklingType, double BucklingLength)
		{
			this.BucklingType = BucklingType;
			this.BucklingLength = BucklingLength;
		}

		// 设置密集矩阵T值
		internal void SetT()
		{
			//如果法向量和杆件方向相同（夹角过于小，与pi取余后小于0.0001）
			bool flag = Vector.VectorAngle(this.Direction, this.Normal) % 3.1415926535897931 < 0.0001;
			//如果方向相同则重设法向量为xyz中一个
			if (flag)
			{
				bool normalUserDefined = this.NormalUserDefined;  //如果自定义了法向量
				if (normalUserDefined)
				{
					this.NormalOverwritten = true;  //覆盖了用户定义法向量
				}
				bool flag2 = Vector.VectorAngle(this.Normal, Vector.UnitZ()) % 3.1415926535897931 >= 0.0001;
				if (flag2)
				{
					this.Normal = Vector.UnitZ();
				}
				else
				{
					bool flag3 = Vector.VectorAngle(this.Normal, Vector.UnitX()) % 3.1415926535897931 >= 0.0001;
					if (flag3)
					{
						this.Normal = Vector.UnitX();
					}
					else
					{
						bool flag4 = Vector.VectorAngle(this.Normal, Vector.UnitY()) % 3.1415926535897931 >= 0.0001;
						if (flag4)
						{
							this.Normal = Vector.UnitY();
						}
					}
				}
			}
			Vector direction = this.Direction;
			Vector unitizedVector = (-Vector.CrossProduct(direction, this.Normal)).GetUnitizedVector();
			Vector unitizedVector2 = Vector.CrossProduct(direction, unitizedVector).GetUnitizedVector();
			this.T[0, 0] = direction[0];
			this.T[0, 1] = direction[1];
			this.T[0, 2] = direction[2];
			this.T[1, 0] = unitizedVector[0];
			this.T[1, 1] = unitizedVector[1];
			this.T[1, 2] = unitizedVector[2];
			this.T[2, 0] = unitizedVector2[0];
			this.T[2, 1] = unitizedVector2[1];
			this.T[2, 2] = unitizedVector2[2];
		}

		// 设置分配
		public void SetAssignment(Assignment Assignment)
		{
			for (int i = 0; i < Assignment.ElementGroups.Count; i++)
			{
				Assignment.ElementGroups[i].AddAssignedMember(this, Assignment.ElementIndices[i]);
			}
			this.Assignment = Assignment;
		}

		// 清除分配
		public void ClearAssignment()
		{
			this.Assignment = null;
		}

		// 设置拓扑是否固定
		public void FixTopology(bool TopologyFixed)
		{
			this.TopologyFixed = TopologyFixed;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0001075F File Offset: 0x0000E95F
		public void SetAreaBounds(double LBArea, double UBArea)
		{
			this.LBArea = LBArea;
			this.UBArea = UBArea;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00010772 File Offset: 0x0000E972
		public void ClearNormalForces()
		{
			this.Nx.Clear();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00010781 File Offset: 0x0000E981
		public void SetNormalForces(Dictionary<LoadCase, List<double>> NormalForces)
		{
			this.Nx = NormalForces;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0001078C File Offset: 0x0000E98C
		public void AddNormalForce(LoadCase LC, List<double> NormalForce)
		{
			bool flag = !this.Nx.ContainsKey(LC);
			if (flag)
			{
				this.Nx.Add(LC, NormalForce);
			}
			else
			{
				this.Nx.Remove(LC);
				this.Nx.Add(LC, NormalForce);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000107DC File Offset: 0x0000E9DC
		public List<double> GetMinMaxNormalForces()
		{
			double item = (from x in this.Nx.Values
			select x.Min()).Min();
			double item2 = (from x in this.Nx.Values
			select x.Max()).Max();
			return new List<double>
			{
				item,
				item2
			};
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00010870 File Offset: 0x0000EA70
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
				Bar bar = (Bar)obj;
				bool flag2 = bar.From == this.From && bar.To == this.To;
				result = flag2;
			}
			return result;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000108E0 File Offset: 0x0000EAE0
		public override int GetHashCode()
		{
			return this.Number.GetHashCode();
		}

		// 杆件的信息输出
		public override string ToString()
		{
			string str = "Bar";
			str = str + " Nr: " + this.Number.ToString();
			str = str + " Material: " + this.Material.ToString();
			str = str + " CS: " + this.CrossSection.ToString();
			str = str + " Length: " + this.Length.ToString();
			return str + " Normal: " + this.Normal.ToString();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00010992 File Offset: 0x0000EB92
		public static bool operator ==(Bar a, Bar b)
		{
			return a.Equals(b);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0001099B File Offset: 0x0000EB9B
		public static bool operator !=(Bar a, Bar b)
		{
			return !a.Equals(b);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000109A8 File Offset: 0x0000EBA8
		public IMember1D Clone()
		{
			Bar bar = new Bar(this.From, this.To);
			bar.Number = this.Number;
			bar.CrossSection = this.CrossSection;
			bar.Material = this.Material;
			bar.structure_num = this.structure_num;
			bar.CosX = this.CosX;
			bar.CosY = this.CosY;
			bar.CosZ = this.CosZ;
			bar.Length = this.Length;
			bar.Direction = new Vector((double[])this.Direction.ToDouble().Clone());
			bar.Normal = new Vector((double[])this.Normal.ToDouble().Clone());
			bar.T = new MatrixDense((double[,])this.T.ToDouble().Clone());
			bar.MinCompound = this.MinCompound;
			bar.MaxCompound = this.MaxCompound;
			bar.Nx = new Dictionary<LoadCase, List<double>>();
			foreach (LoadCase loadCase in this.Nx.Keys)
			{
				LoadCase key = loadCase.Clone();
				bar.Nx.Add(key, this.Nx[loadCase]);
			}
			bar.Buffer = new ValueTuple<double, double>(this.Buffer.Item1, this.Buffer.Item2);
			bar.BucklingType = this.BucklingType;
			bar.BucklingLength = this.BucklingLength;
			bar.LBArea = this.LBArea;
			bar.UBArea = this.UBArea;
			bar.TopologyFixed = this.TopologyFixed;
			bar.NormalUserDefined = this.NormalUserDefined;
			bar.NormalOverwritten = this.NormalOverwritten;
			bar.AllowedCrossSections = this.AllowedCrossSections;
			bar.GroupNumber = this.GroupNumber;
			bool flag = this.Assignment == null;
			if (flag)
			{
				bar.Assignment = null;
			}
			else
			{
				bar.Assignment = this.Assignment.Clone();
			}
			return bar;
		}


		public Bar CloneBar()
		{
			Bar bar = new Bar(this.From, this.To);
			bar.Number = this.Number;
			bar.CrossSection = this.CrossSection;
			bar.Material = this.Material;
			bar.structure_num = this.structure_num;
			bar.CosX = this.CosX;
			bar.CosY = this.CosY;
			bar.CosZ = this.CosZ;
			bar.Length = this.Length;
			bar.Direction = new Vector((double[])this.Direction.ToDouble().Clone());
			bar.Normal = new Vector((double[])this.Normal.ToDouble().Clone());
			bar.T = new MatrixDense((double[,])this.T.ToDouble().Clone());
			bar.MinCompound = this.MinCompound;
			bar.MaxCompound = this.MaxCompound;
			bar.Nx = new Dictionary<LoadCase, List<double>>();
			foreach (LoadCase loadCase in this.Nx.Keys)
			{
				LoadCase key = loadCase.Clone();
				bar.Nx.Add(key, this.Nx[loadCase]);
			}
			bar.Buffer = new ValueTuple<double, double>(this.Buffer.Item1, this.Buffer.Item2);
			bar.BucklingType = this.BucklingType;
			bar.BucklingLength = this.BucklingLength;
			bar.LBArea = this.LBArea;
			bar.UBArea = this.UBArea;
			bar.TopologyFixed = this.TopologyFixed;
			bar.NormalUserDefined = this.NormalUserDefined;
			bar.NormalOverwritten = this.NormalOverwritten;
			bar.AllowedCrossSections = this.AllowedCrossSections;
			bar.GroupNumber = this.GroupNumber;

			//bar.Assignment = null;
            bool flag = this.Assignment == null;
            if (flag)
            {
                bar.Assignment = null;
            }
            else
            {
                bar.Assignment = this.Assignment.Clone();
            }
            return bar;
        }
	}
}
