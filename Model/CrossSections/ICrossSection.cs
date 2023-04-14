using System;
using System.Collections.Generic;
using Beaver3D.Model.Materials;

namespace Beaver3D.Model.CrossSections
{
	// Token: 0x0200003E RID: 62
	public interface ICrossSection
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600057C RID: 1404
		string TypeName { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600057D RID: 1405
		string Name { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600057E RID: 1406
		double Area { get; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600057F RID: 1407
		double Iy { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000580 RID: 1408
		double Iz { get; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000581 RID: 1409
		double It { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000582 RID: 1410
		double Wy { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000583 RID: 1411
		double Wz { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000584 RID: 1412
		double Wt { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000585 RID: 1413
		double Avy { get; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000586 RID: 1414
		double Avz { get; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000587 RID: 1415
		List<int> PossibleCompounds { get; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000588 RID: 1416
		List<ValueTuple<double, double>> Polygon { get; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000589 RID: 1417
		double dy { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600058A RID: 1418
		double dz { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600058B RID: 1419
		double cy { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600058C RID: 1420
		double cz { get; }

		// Token: 0x0600058D RID: 1421
		void SetSectionProperties();

		// Token: 0x0600058E RID: 1422
		void SetPolygon();

		// Token: 0x0600058F RID: 1423
		void SetPossibleCompounds(List<int> Compounds);

		// Token: 0x06000590 RID: 1424
		string ToString();

		// Token: 0x06000591 RID: 1425
		double GetTensionResistance(IMaterial Material);

		// Token: 0x06000592 RID: 1426
		List<double> GetBucklingResistance(IMaterial Material, BucklingType BucklingType, double BucklingLength);

		// Token: 0x06000593 RID: 1427
		double GetUtilization(IMaterial Material, BucklingType BucklingType, double BucklingLength, bool Plastic, double Nx, double Vy, double Vz, double My, double Mz, double Mt);
	}
}
