using System;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model
{
	// Token: 0x02000028 RID: 40
	public interface IMember
	{
		// 杆件编号
		int Number { get; set; }

		// 所属结构编号
		int GroupNumber { get; set; }

		// 材质
		IMaterial Material { get; set; }

		// 拓扑是否固定
		bool TopologyFixed { get; set; }

		// Token: 0x0600023A RID: 570
		void SetNumber(int MemberNumber);

		// Token: 0x0600023B RID: 571
		void SetMaterial(IMaterial m);
	}
}
