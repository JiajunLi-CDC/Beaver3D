using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;

namespace Beaver3D.Reuse
{
	// Token: 0x02000005 RID: 5
	public class ElementGroup
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002838 File Offset: 0x00000A38
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002840 File Offset: 0x00000A40
		public string Name { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002849 File Offset: 0x00000A49
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002851 File Offset: 0x00000A51
		public int Number { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000285A File Offset: 0x00000A5A
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002862 File Offset: 0x00000A62
		public ElementType Type { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000286B File Offset: 0x00000A6B
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002873 File Offset: 0x00000A73
		public bool CanBeCut { get; private set; } = true;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000287C File Offset: 0x00000A7C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002884 File Offset: 0x00000A84
		public ICrossSection CrossSection { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000288D File Offset: 0x00000A8D
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002895 File Offset: 0x00000A95
		public IMaterial Material { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000289E File Offset: 0x00000A9E
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000028A6 File Offset: 0x00000AA6
		public double Length { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000028AF File Offset: 0x00000AAF
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000028B7 File Offset: 0x00000AB7
		public int NumberOfElements { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000028C0 File Offset: 0x00000AC0
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000028C8 File Offset: 0x00000AC8
		public double[] RemainLengths { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000028D1 File Offset: 0x00000AD1
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000028D9 File Offset: 0x00000AD9
		public double[] RemainLengthsTemp { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000028E2 File Offset: 0x00000AE2
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000028EA File Offset: 0x00000AEA
		public bool[] AlreadyCounted { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000028F3 File Offset: 0x00000AF3
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000028FB File Offset: 0x00000AFB
		public double[] Stack { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002904 File Offset: 0x00000B04
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000290C File Offset: 0x00000B0C
		public List<IMember1D>[] AssignedMembers { get; private set; }

		//记录下一个杆件的序号，判断是否超出库存
		public int Next
		{
			get
			{
				bool flag = this.Type == ElementType.Reuse && this.next >= this.NumberOfElements;
				if (flag)
				{
					throw new OverflowException("next became larger than number of elements in the Element Group");
				}
				bool flag2 = this.Type == ElementType.Reuse;
				int result;
				if (flag2)
				{
					int num = this.next;
					this.next = num + 1;
					result = num;
				}
				else
				{
					int num = this.next;
					this.next = num + 1;
					result = num;
				}
				return result;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002994 File Offset: 0x00000B94
		public ElementGroup(IMaterial Material, ICrossSection CrossSection, string Name = null)
		{
			this.Type = ElementType.New;
			this.Material = Material;
			this.CrossSection = CrossSection;
			this.CanBeCut = this.CanBeCut;
			this.Length = double.PositiveInfinity;
			this.NumberOfElements = 1;
			this.Next = 0;
			bool flag = Name == null;
			if (flag)
			{
				this.Name = this.ToString();
			}
			else
			{
				this.Name = Name;
			}
			this.RemainLengths = Enumerable.Repeat<double>(this.Length, this.NumberOfElements).ToArray<double>();
			this.RemainLengthsTemp = Enumerable.Repeat<double>(this.Length, this.NumberOfElements).ToArray<double>();
			this.AlreadyCounted = new bool[this.NumberOfElements];
			this.Stack = new double[this.NumberOfElements];
			this.AssignedMembers = new List<IMember1D>[this.NumberOfElements];
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002A84 File Offset: 0x00000C84
		public ElementGroup(ElementType Type, IMaterial Material, ICrossSection CrossSection, double Length, int NumberOfElements, bool CanBeCut = true, string Name = null)
		{
			this.Name = Name;
			this.Type = Type;
			this.Material = Material;
			this.CrossSection = CrossSection;
			this.CanBeCut = CanBeCut;
			this.Next = 0;
			bool flag = Name == null;
			if (flag)
			{
				this.Name = this.ToString();
			}
			else
			{
				this.Name = Name;
			}
			bool flag2 = Type == ElementType.Reuse;
			if (flag2)
			{
				this.Length = Length;
				this.NumberOfElements = NumberOfElements;
			}
			else
			{
				bool flag3 = Type == ElementType.New || Type == ElementType.Zero;
				if (flag3)
				{
					this.Length = double.MaxValue;
					this.NumberOfElements = 1;
				}
			}
			this.RemainLengths = Enumerable.Repeat<double>(Length, NumberOfElements).ToArray<double>();
			this.RemainLengthsTemp = Enumerable.Repeat<double>(Length, NumberOfElements).ToArray<double>();
			this.AlreadyCounted = new bool[NumberOfElements];
			this.Stack = new double[NumberOfElements];
			this.AssignedMembers = new List<IMember1D>[NumberOfElements];
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B90 File Offset: 0x00000D90
		public static ElementGroup ZeroElement()
		{
			return new ElementGroup(ElementType.Zero, default(EmptyMaterial), new EmptySection(), double.MaxValue, 1, true, null);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BC7 File Offset: 0x00000DC7
		public void SetNumber(int Number)
		{
			this.Number = Number;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BD2 File Offset: 0x00000DD2
		public void ResetRemainLengths()
		{
			this.RemainLengths = Enumerable.Repeat<double>(this.Length, this.NumberOfElements).ToArray<double>();
			this.Next = 0;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002BFA File Offset: 0x00000DFA
		public void ResetRemainLengthsTemp()
		{
			this.RemainLengths.CopyTo(this.RemainLengthsTemp, 0);
			this.Next = 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C18 File Offset: 0x00000E18
		public void ResetAlreadyCounted()
		{
			this.AlreadyCounted = new bool[this.NumberOfElements];
			this.Next = 0;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C35 File Offset: 0x00000E35
		public void ResetNext()
		{
			this.Next = 0;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C40 File Offset: 0x00000E40
		public void AddAssignedMember(IMember1D M, int ElementIndex)
		{
			bool flag = this.Type != ElementType.Reuse;
			if (flag)
			{
				ElementIndex = 0;
			}
			bool flag2 = this.AssignedMembers[ElementIndex] == null;
			if (flag2)
			{
				this.AssignedMembers[ElementIndex] = new List<IMember1D>();
			}
			this.AssignedMembers[ElementIndex].Add(M);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C8E File Offset: 0x00000E8E
		internal void ResetAssignedMembers()
		{
			this.AssignedMembers = new List<IMember1D>[this.NumberOfElements];
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CA3 File Offset: 0x00000EA3
		internal void ResetStack()
		{
			this.Stack = new double[this.NumberOfElements];
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public override string ToString()
		{
			bool flag = this.Type == ElementType.Reuse;
			string result;
			if (flag)
			{
				result = string.Concat(new string[]
				{
					"StockElementGroup Mat: ",
					this.Material.ToString(),
					" CS: ",
					this.CrossSection.ToString(),
					" L: ",
					this.Length.ToString(),
					" N: ",
					this.NumberOfElements.ToString()
				});
			}
			else
			{
				bool flag2 = this.Type == ElementType.New;
				if (flag2)
				{
					result = "NewElement Mat: " + this.Material.ToString() + " CS: " + this.CrossSection.ToString();
				}
				else
				{
					bool flag3 = this.Type == ElementType.Zero;
					if (flag3)
					{
						result = "ZeroElement";
					}
					else
					{
						result = "No Element Type defined";
					}
				}
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D98 File Offset: 0x00000F98
		public ElementGroup Clone()
		{
			ElementGroup elementGroup = new ElementGroup(this.Material, this.CrossSection, null);
			elementGroup.Number = this.Number;
			elementGroup.Type = this.Type;
			elementGroup.CanBeCut = this.CanBeCut;
			elementGroup.CrossSection = this.CrossSection;
			elementGroup.Material = this.Material;
			elementGroup.Length = this.Length;
			elementGroup.NumberOfElements = this.NumberOfElements;
			elementGroup.RemainLengths = (double[])this.RemainLengths.Clone();
			elementGroup.RemainLengthsTemp = (double[])this.RemainLengthsTemp.Clone();
			elementGroup.AlreadyCounted = (bool[])this.AlreadyCounted.Clone();
			elementGroup.Stack = (double[])this.Stack.Clone();
			elementGroup.ResetNext();
			elementGroup.next = this.next;
			elementGroup.Next = this.next + 1;
			return elementGroup;
		}

		// Token: 0x0400001A RID: 26
		private int next;
	}
}
