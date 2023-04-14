using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver3D.Optimization;

namespace Beaver3D.Model
{
	// Token: 0x02000026 RID: 38
	public class Beam : IMember1D, IMember
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		public Beam(Node a, Node b)
		{
			this.From = a;
			this.To = b;
			this.SetGeometricProperties();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00010CBF File Offset: 0x0000EEBF
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00010CC7 File Offset: 0x0000EEC7
		public int Number { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00010CD0 File Offset: 0x0000EED0
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00010CD8 File Offset: 0x0000EED8
		public int GroupNumber { get; set; } = -1;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00010CE1 File Offset: 0x0000EEE1
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00010CE9 File Offset: 0x0000EEE9
		public IMaterial Material { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00010CF2 File Offset: 0x0000EEF2
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00010CFA File Offset: 0x0000EEFA
		public ICrossSection CrossSection { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00010D03 File Offset: 0x0000EF03
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00010D0B File Offset: 0x0000EF0B
		public Node From { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00010D14 File Offset: 0x0000EF14
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00010D1C File Offset: 0x0000EF1C
		public Node To { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00010D25 File Offset: 0x0000EF25
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00010D2D File Offset: 0x0000EF2D
		public double Length { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00010D36 File Offset: 0x0000EF36
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00010D3E File Offset: 0x0000EF3E
		public double CosX { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00010D47 File Offset: 0x0000EF47
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00010D4F File Offset: 0x0000EF4F
		public double CosY { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00010D58 File Offset: 0x0000EF58
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00010D60 File Offset: 0x0000EF60
		public double CosZ { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00010D69 File Offset: 0x0000EF69
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00010D71 File Offset: 0x0000EF71
		public Vector Direction { get; set; } = new Vector(3);

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00010D7A File Offset: 0x0000EF7A
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00010D82 File Offset: 0x0000EF82
		public Vector Normal { get; set; } = Vector.UnitZ();

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00010D8B File Offset: 0x0000EF8B
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00010D93 File Offset: 0x0000EF93
		public MatrixDense T { get; set; } = new MatrixDense(3, 1.0);

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00010D9C File Offset: 0x0000EF9C
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		public Dictionary<LoadCase, List<double>> Nx { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00010DAD File Offset: 0x0000EFAD
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00010DB5 File Offset: 0x0000EFB5
		public Dictionary<LoadCase, List<double>> Vy { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00010DBE File Offset: 0x0000EFBE
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00010DC6 File Offset: 0x0000EFC6
		public Dictionary<LoadCase, List<double>> Vz { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00010DCF File Offset: 0x0000EFCF
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00010DD7 File Offset: 0x0000EFD7
		public Dictionary<LoadCase, List<double>> My { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00010DE0 File Offset: 0x0000EFE0
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		public Dictionary<LoadCase, List<double>> Mz { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00010DF9 File Offset: 0x0000EFF9
		public Dictionary<LoadCase, List<double>> Mt { get; set; } = new Dictionary<LoadCase, List<double>>();

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00010E02 File Offset: 0x0000F002
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00010E0A File Offset: 0x0000F00A
		public ValueTuple<double, double> Buffer { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00010E13 File Offset: 0x0000F013
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00010E1B File Offset: 0x0000F01B
		public BucklingType BucklingType { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00010E24 File Offset: 0x0000F024
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00010E2C File Offset: 0x0000F02C
		public double BucklingLength { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00010E35 File Offset: 0x0000F035
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00010E3D File Offset: 0x0000F03D
		public int MinCompound { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00010E46 File Offset: 0x0000F046
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00010E4E File Offset: 0x0000F04E
		public int MaxCompound { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00010E57 File Offset: 0x0000F057
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00010E5F File Offset: 0x0000F05F
		public List<string> AllowedCrossSections { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00010E68 File Offset: 0x0000F068
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00010E70 File Offset: 0x0000F070
		public double LBArea { get; set; } = 0.0;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00010E79 File Offset: 0x0000F079
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00010E81 File Offset: 0x0000F081
		public double UBArea { get; set; } = double.PositiveInfinity;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00010E8A File Offset: 0x0000F08A
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00010E92 File Offset: 0x0000F092
		public bool TopologyFixed { get; set; } = false;

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00010E9B File Offset: 0x0000F09B
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00010EA3 File Offset: 0x0000F0A3
		public bool NormalUserDefined { get; set; } = false;

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00010EAC File Offset: 0x0000F0AC
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00010EB4 File Offset: 0x0000F0B4
		public bool NormalOverwritten { get; set; } = false;

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00010EBD File Offset: 0x0000F0BD
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00010EC5 File Offset: 0x0000F0C5
		public Assignment Assignment { get; set; } = new Assignment();

		// Token: 0x06000217 RID: 535 RVA: 0x00010ECE File Offset: 0x0000F0CE
		public void SetNumber(int MemberNumber)
		{
			this.Number = MemberNumber;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00010ED9 File Offset: 0x0000F0D9
		public void SetGroupNumber(int GroupNumber)
		{
			this.GroupNumber = GroupNumber;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		public void SetMaterial(IMaterial Material)
		{
			this.Material = Material;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00010EEF File Offset: 0x0000F0EF
		public void SetCrossSection(ICrossSection CrossSection)
		{
			this.CrossSection = CrossSection;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00010EFC File Offset: 0x0000F0FC
		public void SetGeometricProperties()
		{
			this.Direction[0] = this.To.X - this.From.X;
			this.Direction[1] = this.To.Y - this.From.Y;
			this.Direction[2] = this.To.Z - this.From.Z;
			this.Length = Math.Sqrt(this.Direction[0] * this.Direction[0] + this.Direction[1] * this.Direction[1] + this.Direction[2] * this.Direction[2]);
			this.Direction.Unitize();
			this.CosX = this.Direction[0];
			this.CosY = this.Direction[1];
			this.CosZ = this.Direction[2];
			bool flag = Vector.VectorAngle(this.Direction, this.Normal) < 0.0001;
			if (flag)
			{
				this.Normal = new Vector(Vector.UnitX());
			}
			this.SetT();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00011060 File Offset: 0x0000F260
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
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000110C0 File Offset: 0x0000F2C0
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

		// Token: 0x0600021E RID: 542 RVA: 0x00011154 File Offset: 0x0000F354
		private void SetT()
		{
			bool flag = Vector.VectorAngle(this.Direction, this.Normal) % 3.1415926535897931 < 0.0001;
			if (flag)
			{
				bool normalUserDefined = this.NormalUserDefined;
				if (normalUserDefined)
				{
					this.NormalOverwritten = true;
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

		// Token: 0x0600021F RID: 543 RVA: 0x00011340 File Offset: 0x0000F540
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

		// Token: 0x06000220 RID: 544 RVA: 0x0001139A File Offset: 0x0000F59A
		public void SetAllowedCrossSections(List<string> TypeList)
		{
			this.AllowedCrossSections = TypeList;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000113A5 File Offset: 0x0000F5A5
		public void AddAllowedCrossSection(string CSType)
		{
			this.AllowedCrossSections.Add(CSType);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000113B5 File Offset: 0x0000F5B5
		public void SetBufferLengths(double Buffer0, double Buffer1)
		{
			this.Buffer = new ValueTuple<double, double>(Buffer0, Buffer1);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000113C6 File Offset: 0x0000F5C6
		public void SetBuckling(BucklingType BucklingType, double BucklingLength)
		{
			this.BucklingType = BucklingType;
			this.BucklingLength = BucklingLength;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000113DC File Offset: 0x0000F5DC
		public void SetAssignment(Assignment Assignment)
		{
			for (int i = 0; i < Assignment.ElementGroups.Count; i++)
			{
				Assignment.ElementGroups[i].AddAssignedMember(this, Assignment.ElementIndices[i]);
			}
			this.Assignment = Assignment;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0001142D File Offset: 0x0000F62D
		public void ClearAssignment()
		{
			this.Assignment = null;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00011438 File Offset: 0x0000F638
		public void FixTopology(bool TopologyFixed)
		{
			this.TopologyFixed = TopologyFixed;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00011443 File Offset: 0x0000F643
		public void SetAreaBounds(double LBArea, double UBArea)
		{
			this.LBArea = LBArea;
			this.UBArea = UBArea;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00011456 File Offset: 0x0000F656
		public void SetNormalForces(Dictionary<LoadCase, List<double>> NormalForces)
		{
			this.Nx = NormalForces;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00011464 File Offset: 0x0000F664
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

		// Token: 0x0600022A RID: 554 RVA: 0x000114B4 File Offset: 0x0000F6B4
		public void AddInternalForces(LoadCase LC, List<double> Nx, List<double> Vy, List<double> Vz, List<double> My, List<double> Mz, List<double> Mt)
		{
			bool flag = Nx.Count != Vy.Count || Nx.Count != Vy.Count || Nx.Count != Vz.Count || Nx.Count != My.Count || Nx.Count != Mz.Count || Nx.Count != Mt.Count;
			if (flag)
			{
				throw new ArgumentException("The provided lists with internal forces along the beam do not have the same length!");
			}
			bool flag2 = !this.Nx.ContainsKey(LC);
			if (flag2)
			{
				this.Nx.Add(LC, Nx);
				this.Vy.Add(LC, Vy);
				this.Vz.Add(LC, Vz);
				this.My.Add(LC, My);
				this.Mz.Add(LC, Mz);
				this.Mt.Add(LC, Mt);
			}
			else
			{
				this.Nx.Remove(LC);
				this.Vy.Remove(LC);
				this.Vz.Remove(LC);
				this.My.Remove(LC);
				this.Mz.Remove(LC);
				this.Mt.Remove(LC);
				this.Nx.Add(LC, Nx);
				this.Vy.Add(LC, Vy);
				this.Vz.Add(LC, Vz);
				this.My.Add(LC, My);
				this.Mz.Add(LC, Mz);
				this.Mt.Add(LC, Mt);
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0001164C File Offset: 0x0000F84C
		public void AddInternalForce(LoadCase LC, double Nx, double Vy, double Vz, double My, double Mz, double Mt)
		{
			bool flag = !this.Nx.ContainsKey(LC);
			if (flag)
			{
				this.Nx.Add(LC, new List<double>
				{
					Nx
				});
				this.Vy.Add(LC, new List<double>
				{
					Vy
				});
				this.Vz.Add(LC, new List<double>
				{
					Vz
				});
				this.My.Add(LC, new List<double>
				{
					My
				});
				this.Mz.Add(LC, new List<double>
				{
					Mz
				});
				this.Mt.Add(LC, new List<double>
				{
					Mt
				});
			}
			else
			{
				this.Nx[LC].Add(Nx);
				this.Vy[LC].Add(Vy);
				this.Vz[LC].Add(Vz);
				this.My[LC].Add(My);
				this.Mz[LC].Add(Mz);
				this.Mt[LC].Add(Mt);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0001178C File Offset: 0x0000F98C
		public void ClearInternalForces()
		{
			this.Nx.Clear();
			this.Vy.Clear();
			this.Vz.Clear();
			this.My.Clear();
			this.Mz.Clear();
			this.Mt.Clear();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000117E4 File Offset: 0x0000F9E4
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
				Beam beam = (Beam)obj;
				bool flag2 = beam.From == this.From && beam.To == this.To;
				result = flag2;
			}
			return result;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00011854 File Offset: 0x0000FA54
		public override int GetHashCode()
		{
			return this.Number.GetHashCode();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011874 File Offset: 0x0000FA74
		public static bool operator ==(Beam a, Beam b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001187D File Offset: 0x0000FA7D
		public static bool operator !=(Beam a, Beam b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001188C File Offset: 0x0000FA8C
		public IMember1D Clone()
		{
			Beam beam = new Beam(this.From, this.To);
			beam.Number = this.Number;
			beam.CrossSection = this.CrossSection;
			beam.Material = this.Material;
			beam.CosX = this.CosX;
			beam.CosY = this.CosY;
			beam.CosZ = this.CosZ;
			beam.Length = this.Length;
			beam.Direction = new Vector((double[])this.Direction.ToDouble().Clone());
			beam.Normal = new Vector((double[])this.Normal.ToDouble().Clone());
			beam.T = new MatrixDense((double[,])this.T.ToDouble().Clone());
			beam.MinCompound = this.MinCompound;
			beam.MaxCompound = this.MaxCompound;
			beam.Nx = new Dictionary<LoadCase, List<double>>();
			beam.Vy = new Dictionary<LoadCase, List<double>>();
			beam.Vz = new Dictionary<LoadCase, List<double>>();
			beam.My = new Dictionary<LoadCase, List<double>>();
			beam.Mz = new Dictionary<LoadCase, List<double>>();
			beam.Mt = new Dictionary<LoadCase, List<double>>();
			foreach (LoadCase loadCase in this.Nx.Keys)
			{
				LoadCase key = loadCase.Clone();
				beam.Nx.Add(key, this.Nx[loadCase]);
				beam.Vy.Add(key, this.Vy[loadCase]);
				beam.Vz.Add(key, this.Vz[loadCase]);
				beam.My.Add(key, this.My[loadCase]);
				beam.Mz.Add(key, this.Mz[loadCase]);
				beam.Mt.Add(key, this.Mt[loadCase]);
			}
			beam.Buffer = new ValueTuple<double, double>(this.Buffer.Item1, this.Buffer.Item2);
			beam.BucklingType = this.BucklingType;
			beam.BucklingLength = this.BucklingLength;
			beam.LBArea = this.LBArea;
			beam.UBArea = this.UBArea;
			beam.TopologyFixed = this.TopologyFixed;
			beam.NormalUserDefined = this.NormalUserDefined;
			beam.NormalOverwritten = this.NormalOverwritten;
			beam.AllowedCrossSections = this.AllowedCrossSections;
			beam.GroupNumber = this.GroupNumber;
			bool flag = this.Assignment == null;
			if (flag)
			{
				beam.Assignment = null;
			}
			else
			{
				beam.Assignment = this.Assignment.Clone();
			}
			return beam;
		}
	}
}
