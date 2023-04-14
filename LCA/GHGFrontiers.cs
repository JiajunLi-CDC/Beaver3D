using System;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.LCA
{
	// Token: 0x02000003 RID: 3
	public class GHGFrontiers : ILCA
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000216A File Offset: 0x0000036A
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002172 File Offset: 0x00000372
		public double EI_Deconstruction { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000217B File Offset: 0x0000037B
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002183 File Offset: 0x00000383
		public double EI_Demolition { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000218C File Offset: 0x0000038C
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002194 File Offset: 0x00000394
		public double EI_NewSteelProduction { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000219D File Offset: 0x0000039D
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000021A5 File Offset: 0x000003A5
		public double EI_Assembly { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021AE File Offset: 0x000003AE
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000021B6 File Offset: 0x000003B6
		public double EI_Transport { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000021BF File Offset: 0x000003BF
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000021C7 File Offset: 0x000003C7
		public double EI_Transport_Stock { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000021D0 File Offset: 0x000003D0
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000021D8 File Offset: 0x000003D8
		public double EI_Transport_NewSteel { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000021E1 File Offset: 0x000003E1
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000021E9 File Offset: 0x000003E9
		public double EI_Transport_Structure { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000021F2 File Offset: 0x000003F2
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000021FA File Offset: 0x000003FA
		public double EI_Transport_Waste { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002203 File Offset: 0x00000403
		// (set) Token: 0x0600001A RID: 26 RVA: 0x0000220B File Offset: 0x0000040B
		public double EI_Transport_Recycling { get; private set; }

		// Token: 0x0600001B RID: 27 RVA: 0x00002214 File Offset: 0x00000414
		public GHGFrontiers() : this(150.0, 10.0, 10.0, 10.0, 10.0)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000224C File Offset: 0x0000044C
		public GHGFrontiers(double TransportDistance_Stock, double TransportDistance_NewSteel, double TransportDistance_Structure, double TransportDistance_Waste, double TransportDistance_Recycling)
		{
			this.EI_Deconstruction = 0.337;
			this.EI_Demolition = 0.05;
			this.EI_NewSteelProduction = 0.734;
			this.EI_Assembly = 0.11;
			this.EI_Transport = 0.00011;
			this.EI_Transport_Stock = 0.0165;
			this.EI_Transport_NewSteel = 0.0011;
			this.EI_Transport_Structure = 0.0011;
			this.EI_Transport_Waste = 0.0011;
			this.EI_Transport_Recycling = 0.0011;
			//base..ctor();
			this.EI_Transport_Stock = TransportDistance_Stock * this.EI_Transport;
			this.EI_Transport_NewSteel = TransportDistance_NewSteel * this.EI_Transport;
			this.EI_Transport_Structure = TransportDistance_Structure * this.EI_Transport;
			this.EI_Transport_Waste = TransportDistance_Waste * this.EI_Transport;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002334 File Offset: 0x00000534
		public GHGFrontiers(double EI_Deconstruction, double EI_Demolition, double EI_NewSteelProduction, double EI_Assembly, double EI_Transport, double TransportDistance_Stock, double TransportDistance_NewSteel, double TransportDistance_Structure, double TransportDistance_Waste, double TransportDistance_Recycling)
		{
			this.EI_Deconstruction = 0.337;
			this.EI_Demolition = 0.05;
			this.EI_NewSteelProduction = 0.734;
			this.EI_Assembly = 0.11;
			this.EI_Transport = 0.00011;
			this.EI_Transport_Stock = 0.0165;
			this.EI_Transport_NewSteel = 0.0011;
			this.EI_Transport_Structure = 0.0011;
			this.EI_Transport_Waste = 0.0011;
			this.EI_Transport_Recycling = 0.0011;
			//base..ctor();
			this.EI_NewSteelProduction = EI_NewSteelProduction;
			this.EI_Deconstruction = EI_Deconstruction;
			this.EI_Demolition = EI_Demolition;
			this.EI_Assembly = EI_Assembly;
			this.EI_Transport = EI_Transport;
			this.EI_Transport_Stock = TransportDistance_Stock * EI_Transport;
			this.EI_Transport_NewSteel = TransportDistance_NewSteel * EI_Transport;
			this.EI_Transport_Structure = TransportDistance_Structure * EI_Transport;
			this.EI_Transport_Waste = TransportDistance_Waste * EI_Transport;
			this.EI_Transport_Recycling = TransportDistance_Recycling * EI_Transport;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002448 File Offset: 0x00000648
		public double ReturnMemberImpact(IMember1D Member)
		{
			return Member.Length * Member.CrossSection.Area * Member.Material.Density * (this.EI_Demolition + this.EI_Transport_Recycling + this.EI_NewSteelProduction + this.EI_Assembly + this.EI_Transport_NewSteel + this.EI_Transport_Structure);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024A4 File Offset: 0x000006A4
		public double ReturnStockElementImpact(ElementGroup EG)
		{
			bool flag = EG.Type == ElementType.Reuse;
			double result;
			if (flag)
			{
				result = EG.Material.Density * EG.CrossSection.Area * (EG.Length * (this.EI_Deconstruction + this.EI_Transport_Stock + this.EI_Transport_Waste));
			}
			else
			{
				result = 0.0;
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002504 File Offset: 0x00000704
		public double ReturnElementMemberImpact(ElementGroup EG, bool AlreadyCounted, IMember1D Member)
		{
			bool flag = EG.Type == ElementType.Reuse;
			double result;
			if (flag)
			{
				if (AlreadyCounted)
				{
					result = EG.Material.Density * EG.CrossSection.Area * (Member.Length * (this.EI_Assembly + this.EI_Transport_Structure - this.EI_Transport_Waste));
				}
				else
				{
					result = EG.Material.Density * EG.CrossSection.Area * (EG.Length * (this.EI_Deconstruction + this.EI_Transport_Stock + this.EI_Transport_Waste) + Member.Length * (this.EI_Assembly + this.EI_Transport_Structure - this.EI_Transport_Waste));
				}
			}
			else
			{
				bool flag2 = EG.Type == ElementType.New;
				if (flag2)
				{
					result = EG.Material.Density * EG.CrossSection.Area * Member.Length * (this.EI_Demolition + this.EI_Transport_Recycling + this.EI_NewSteelProduction + this.EI_Assembly + this.EI_Transport_NewSteel + this.EI_Transport_Structure);
				}
				else
				{
					result = 0.0;
				}
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002618 File Offset: 0x00000818
		public double ReturnTotalImpact(Structure Structure, out double MaxMemberImpact)
		{
			double num = 0.0;
			MaxMemberImpact = 0.0;
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				double num2 = 0.0;
				for (int i = 0; i < member1D.Assignment.ElementGroups.Count; i++)
				{
					ElementGroup elementGroup = member1D.Assignment.ElementGroups[i];
					int num3 = member1D.Assignment.ElementIndices[i];
					num += this.ReturnElementMemberImpact(elementGroup, elementGroup.AlreadyCounted[num3], member1D);
					num2 += this.ReturnElementMemberImpact(elementGroup, elementGroup.AlreadyCounted[num3], member1D);
					elementGroup.AlreadyCounted[num3] = true;
				}
				MaxMemberImpact = Math.Max(MaxMemberImpact, num2);
			}
			foreach (IMember member2 in Structure.Members)
			{
				IMember1D member1D2 = (IMember1D)member2;
				foreach (ElementGroup elementGroup2 in member1D2.Assignment.ElementGroups)
				{
					elementGroup2.ResetAlreadyCounted();
				}
			}
			return num;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027C4 File Offset: 0x000009C4
		public ILCA Clone()
		{
			return new GHGFrontiers
			{
				EI_Deconstruction = this.EI_Deconstruction,
				EI_Demolition = this.EI_Demolition,
				EI_NewSteelProduction = this.EI_NewSteelProduction,
				EI_Assembly = this.EI_Assembly,
				EI_Transport_Stock = this.EI_Transport_Stock,
				EI_Transport_NewSteel = this.EI_Transport_NewSteel,
				EI_Transport_Structure = this.EI_Transport_Structure
			};
		}
	}
}
