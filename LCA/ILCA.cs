using System;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.LCA
{
	// LCA
	public interface ILCA
	{
		// Token: 0x06000023 RID: 35
		double ReturnTotalImpact(Structure Structure, out double MaxMemberImpact);

		// Token: 0x06000024 RID: 36
		double ReturnElementMemberImpact(ElementGroup EG, bool AlreadyCounted, IMember1D Member);

		// Token: 0x06000025 RID: 37
		double ReturnStockElementImpact(ElementGroup EG);

		// Token: 0x06000026 RID: 38
		double ReturnMemberImpact(IMember1D Member);

		// Token: 0x06000027 RID: 39
		ILCA Clone();
	}
}
