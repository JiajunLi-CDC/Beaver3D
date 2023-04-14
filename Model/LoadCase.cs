using System;
using System.Collections.Generic;

namespace Beaver3D.Model
{
	// Token: 0x02000022 RID: 34
	public class LoadCase
	{
		//编号
		public int Number { get; set; } = -1;

		// 名称
		public string Name { get; set; } = "";

		// 荷载
		public List<ILoad> Loads { get; private set; } = new List<ILoad>();

		// 位移界限
		public List<DisplacementBound> DisplacementBounds { get; private set; } = new List<DisplacementBound>();

		// 自重系数，默认为1
		public double yg { get; set; } = 1.0;

		//  实例化
		public LoadCase(string Name, double SelfWeightFactor = 1.0)
		{
			bool flag = Name == "";
			if (flag)
			{
				throw new ArgumentException("LoadCase name cannot be empty");
			}
			bool flag2 = Name == "all";
			if (flag2)
			{
				throw new ArgumentException("LoadCase name cannot be 'all'. It is reserved to request the computation of all load cases");
			}
			this.Name = Name;
			this.yg = SelfWeightFactor;
		}

		// 额外施加荷载
		public void AddLoad(ILoad l)
		{
			this.Loads.Add(l);
		}

		// 额外施加位移界限
		public void AddDisplacementBound(DisplacementBound db)
		{
			bool flag = this.DisplacementBounds.Contains(db);  //如果包含位移界限则替换
			if (flag)
			{
				this.DisplacementBounds.Remove(db);
				this.DisplacementBounds.Add(db);
			}
			else
			{
				this.DisplacementBounds.Add(db);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000F708 File Offset: 0x0000D908
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
				LoadCase loadCase = (LoadCase)obj;
				bool flag2 = loadCase.Name == this.Name;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000F760 File Offset: 0x0000D960
		public override string ToString()
		{
			return this.Name.ToString();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000F780 File Offset: 0x0000D980
		public LoadCase Clone()
		{
			LoadCase loadCase = new LoadCase(this.Name, this.yg);
			loadCase.Number = this.Number;
			loadCase.Loads = new List<ILoad>();
			for (int i = 0; i < this.Loads.Count; i++)
			{
				PointLoad pointLoad = this.Loads[i] as PointLoad;
				bool flag = pointLoad != null;
				if (flag)
				{
					loadCase.Loads.Add(pointLoad.Clone());
				}
			}
			for (int j = 0; j < this.DisplacementBounds.Count; j++)
			{
				loadCase.DisplacementBounds.Add(this.DisplacementBounds[j].Clone());
			}
			return loadCase;
		}
	}
}
