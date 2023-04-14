using System;
using System.Linq;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization
{
	// Token: 0x02000014 RID: 20
	public class Utilization
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00004DB8 File Offset: 0x00002FB8
		public Utilization(IMember1D Member, Assignment Assignment)
		{
			Tuple<ElementGroup, int>[] assignmentIndices = Assignment.GetAssignmentIndices();
			double num = 0.0;
			double num2 = 0.0;
			foreach (Tuple<ElementGroup, int> tuple in assignmentIndices)
			{
				num += tuple.Item1.CrossSection.GetTensionResistance(tuple.Item1.Material);
				num2 += tuple.Item1.CrossSection.GetBucklingResistance(tuple.Item1.Material, Member.BucklingType, Member.BucklingLength).Max();
			}
		}

		// Token: 0x04000058 RID: 88
		public double Utilization1;
	}
}
