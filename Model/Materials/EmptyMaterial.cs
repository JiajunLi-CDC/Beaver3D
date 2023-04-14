using System;

namespace Beaver3D.Model.Materials
{
	// Token: 0x0200002E RID: 46
	public struct EmptyMaterial : IMaterial
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002EB RID: 747 RVA: 0x000139EE File Offset: 0x00011BEE
		// (set) Token: 0x060002EC RID: 748 RVA: 0x000139F6 File Offset: 0x00011BF6
		public MaterialType Type {  get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002ED RID: 749 RVA: 0x000139FF File Offset: 0x00011BFF
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00013A07 File Offset: 0x00011C07
		public double Density {  get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00013A10 File Offset: 0x00011C10
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x00013A18 File Offset: 0x00011C18
		public double E {  get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00013A21 File Offset: 0x00011C21
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x00013A29 File Offset: 0x00011C29
		public double G {  get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00013A32 File Offset: 0x00011C32
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x00013A3A File Offset: 0x00011C3A
		public double PoissonRatio {  get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00013A43 File Offset: 0x00011C43
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00013A4B File Offset: 0x00011C4B
		public double ft { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00013A54 File Offset: 0x00011C54
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x00013A5C File Offset: 0x00011C5C
		public double fc {  get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00013A65 File Offset: 0x00011C65
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00013A6D File Offset: 0x00011C6D
		public double gamma_0 { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00013A76 File Offset: 0x00011C76
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00013A7E File Offset: 0x00011C7E
		public double gamma_1 {  get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00013A87 File Offset: 0x00011C87
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00013A8F File Offset: 0x00011C8F
		public double kmod {  get; set; }

		// Token: 0x060002FF RID: 767 RVA: 0x00013A98 File Offset: 0x00011C98
		public EmptyMaterial(string name)
		{
			this.Type = MaterialType.Empty;
			this.Density = 0.0;
			this.E = 0.0;
			this.G = 0.0;
			this.PoissonRatio = 0.0;
			this.ft = 0.0;
			this.fc = 0.0;
			this.gamma_0 = 1.0;
			this.gamma_1 = 1.0;
			this.kmod = 1.0;
		}
	}
}
