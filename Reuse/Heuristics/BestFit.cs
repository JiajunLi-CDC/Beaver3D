using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Beaver3D.LCA;
using Beaver3D.Model;
using Beaver3D.Optimization;

namespace Beaver3D.Reuse.Heuristics
{
	// Token: 0x02000009 RID: 9
	public class BestFit
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003730 File Offset: 0x00001930
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003738 File Offset: 0x00001938
		public Objective Objective { get; private set; } = Objective.MinStructureMass;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003741 File Offset: 0x00001941
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003749 File Offset: 0x00001949
		public double Runtime { get; private set; } = 0.0;

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003752 File Offset: 0x00001952
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000375A File Offset: 0x0000195A
		public Result Result { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003763 File Offset: 0x00001963
		// (set) Token: 0x06000070 RID: 112 RVA: 0x0000376B File Offset: 0x0000196B
		public bool PromoteFirstPart { get; private set; } = false;

		// Token: 0x06000071 RID: 113 RVA: 0x00003774 File Offset: 0x00001974
		public BestFit(Objective Objective, ILCA LCA = null, bool PromoteFirstPart = false)
		{
			this.Objective = Objective;
			this.PromoteFirstPart = PromoteFirstPart;
			bool flag = LCA == null;
			if (flag)
			{
				this.LCA = new GHGFrontiers();
			}
			else
			{
				this.LCA = LCA;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000037D4 File Offset: 0x000019D4
		public void Solve(Structure Structure, Stock Stock, List<LoadCase> LoadCases, double allowedCapacity)
		{
			Stock.ResetRemainLenghts();
			Stock.ResetRemainLenghtsTemp();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				Assignment assignment = new Assignment();
				for (int i = member1D.MinCompound; i <= member1D.MaxCompound; i++)
				{
					Assignment bestAssignment = this.GetBestAssignment(i, member1D, Stock, LoadCases, allowedCapacity);
					bool flag = bestAssignment.Feasible && bestAssignment.ObjectiveValue < assignment.ObjectiveValue;
					if (flag)
					{
						assignment = bestAssignment;
					}
					foreach (ElementGroup elementGroup in bestAssignment.ElementGroups)
					{
						elementGroup.ResetRemainLengthsTemp();
					}
				}
				bool feasible = assignment.Feasible;
				if (feasible)
				{
					member1D.SetAssignment(assignment);
					member1D.SetCrossSection(assignment.ElementGroups[0].CrossSection);
					for (int j = 0; j < assignment.ElementGroups.Count; j++)
					{
						bool flag2 = assignment.ElementGroups[j].Type == ElementType.Reuse;
						if (flag2)
						{
							bool canBeCut = assignment.ElementGroups[j].CanBeCut;
							if (canBeCut)
							{
								assignment.ElementGroups[j].RemainLengths[assignment.ElementIndices[j]] -= member1D.Length - member1D.Buffer.Item1;
								assignment.ElementGroups[j].RemainLengthsTemp[assignment.ElementIndices[j]] = assignment.ElementGroups[j].RemainLengths[assignment.ElementIndices[j]];
							}
							else
							{
								assignment.ElementGroups[j].RemainLengths[assignment.ElementIndices[j]] = 0.0;
								assignment.ElementGroups[j].RemainLengthsTemp[assignment.ElementIndices[j]] = 0.0;
							}
						}
					}
				}
				else
				{
					bool flag3 = allowedCapacity < 1.0;
					if (!flag3)
					{
						Stock.ResetRemainLenghts();
						Stock.ResetRemainLenghtsTemp();
						Stock.ResetAlreadyCounted();
						Stock.ResetNext();
						this.Runtime = (double)(stopwatch.ElapsedMilliseconds / 1000L);
						stopwatch.Stop();
						throw new Exception("Infeasible problem");
					}
					Stock.ResetRemainLenghts();
					Stock.ResetRemainLenghtsTemp();
					Stock.ResetAlreadyCounted();
					Stock.ResetNext();
				}
			}
			this.Runtime = (double)(stopwatch.ElapsedMilliseconds / 1000L);
			stopwatch.Stop();
			Stock.ResetAlreadyCounted();
			Stock.ResetRemainLenghts();
			Structure.SetResults(new Result(Structure, Stock, this.LCA));
			Structure.SetLCA(this.LCA);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003B38 File Offset: 0x00001D38
		private Assignment GetBestAssignment(int NumberOfCompounds, IMember1D Member, Stock Stock, List<LoadCase> LoadCases, double allowedCapacity)
		{
			Assignment assignment = new Assignment();
			assignment.SetFeasible(true);
			double num = 0.0;
			for (int i = 0; i < NumberOfCompounds; i++)
			{
				int index = 0;
				int num2 = 0;
				bool flag = false;
				double num3 = double.PositiveInfinity;
				for (int j = 0; j < Stock.ElementGroups.Count; j++)
				{
					ElementGroup elementGroup = Stock.ElementGroups[j];
					bool flag2 = !elementGroup.CrossSection.PossibleCompounds.Contains(NumberOfCompounds);
					if (!flag2)
					{
						bool flag3 = Member.AllowedCrossSections.Any<string>() && !Member.AllowedCrossSections.Contains(elementGroup.CrossSection.TypeName);
						if (!flag3)
						{
							bool flag4 = Member.Nx.Count != 0 && !this.CheckForceCapacity(Member, elementGroup, NumberOfCompounds, LoadCases, allowedCapacity);
							if (!flag4)
							{
								for (int k = 0; k < elementGroup.NumberOfElements; k++)
								{
									double num4 = elementGroup.RemainLengthsTemp[k];
									bool flag5;
									if (!elementGroup.CanBeCut)
									{
										flag5 = (Member.Length - Member.Buffer.Item1 <= num4 && num4 <= Member.Length - Member.Buffer.Item2);
									}
									else
									{
										flag5 = (Member.Length - Member.Buffer.Item1 <= num4);
									}
									bool flag6 = elementGroup.Type == ElementType.New || flag5;
									if (flag6)
									{
										flag = true;
										double objectiveValue = this.GetObjectiveValue(this.Objective, Member, elementGroup, num4, num4 < elementGroup.Length);
										bool flag7 = objectiveValue < num3;
										if (flag7)
										{
											index = j;
											num2 = k;
											num3 = objectiveValue;
											bool flag8 = Math.Abs(num4 - elementGroup.Length) < 0.001;
											if (flag8)
											{
												break;
											}
										}
									}
									else
									{
										bool flag9 = !flag5 && Math.Abs(num4 - elementGroup.Length) < 0.001;
										if (flag9)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
				bool flag10 = flag;
				if (!flag10)
				{
					assignment.SetFeasible(false);
					break;
				}
				num += num3;
				assignment.AddElementAssignment(Stock.ElementGroups[index], num2);
				bool canBeCut = Stock.ElementGroups[index].CanBeCut;
				if (canBeCut)
				{
					Stock.ElementGroups[index].RemainLengthsTemp[num2] -= Member.Length - Member.Buffer.Item1;
				}
				else
				{
					Stock.ElementGroups[index].RemainLengthsTemp[num2] = 0.0;
				}
			}
			bool feasible = assignment.Feasible;
			if (feasible)
			{
				assignment.SetObjectiveValue(num);
			}
			return assignment;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E34 File Offset: 0x00002034
		private bool CheckForceCapacity(IMember1D Member, ElementGroup EG, int Divisor, List<LoadCase> LoadCases, double allowedCapacity)
		{
			Bar bar = Member as Bar;
			bool result;
			if (bar == null)
			{
				Beam beam = Member as Beam;
				if (beam == null)
				{
					result = this.CheckBarForceCapacity(Member, EG, Divisor, LoadCases, allowedCapacity);
				}
				else
				{
					result = this.CheckBeamForceCapacity(beam, EG, Divisor, LoadCases, allowedCapacity);
				}
			}
			else
			{
				result = this.CheckBarForceCapacity(bar, EG, Divisor, LoadCases, allowedCapacity);
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003E98 File Offset: 0x00002098
		private bool CheckBarForceCapacity(IMember1D Bar, ElementGroup EG, int Divisor, List<LoadCase> LoadCases, double allowedCapacity)
		{
			double num = Math.Max(0.0, (from x in LoadCases.Where(new Func<LoadCase, bool>(Bar.Nx.ContainsKey))
			select Bar.Nx[x].Max()).Max() / (double)Divisor);
			double num2 = Math.Min(0.0, (from x in LoadCases.Where(new Func<LoadCase, bool>(Bar.Nx.ContainsKey))
			select Bar.Nx[x].Min()).Min() / (double)Divisor);
			double num3 = EG.CrossSection.GetTensionResistance(EG.Material) * allowedCapacity;
			double num4 = EG.CrossSection.GetBucklingResistance(EG.Material, Bar.BucklingType, Bar.BucklingLength).Max() * allowedCapacity;
			return num4 <= num2 && num <= num3;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003FA8 File Offset: 0x000021A8
		private bool CheckBeamForceCapacity(Beam Beam, ElementGroup EG, int Divisor, List<LoadCase> LoadCases, double allowedCapacity)
		{
			double num = EG.CrossSection.GetTensionResistance(EG.Material) * allowedCapacity;
			double num2 = EG.CrossSection.GetBucklingResistance(EG.Material, Beam.BucklingType, Beam.BucklingLength).Max() * allowedCapacity;
			foreach (LoadCase key in LoadCases)
			{
				bool flag = Beam.Nx[key][0] < num2 || Beam.Nx[key][0] > num;
				if (flag)
				{
					return false;
				}
				for (int i = 0; i < Beam.Nx[key].Count; i++)
				{
					double nx = Beam.Nx[key][i] / (double)Divisor;
					double vy = Beam.Vy[key][i] / (double)Divisor;
					double vz = Beam.Vz[key][i] / (double)Divisor;
					double my = Beam.My[key][i] / (double)Divisor;
					double mz = Beam.Mz[key][i] / (double)Divisor;
					double mt = Beam.Mt[key][i] / (double)Divisor;
					bool flag2 = EG.CrossSection.GetUtilization(EG.Material, Beam.BucklingType, Beam.BucklingLength, false, nx, vy, vz, my, mz, mt) > 1.0;
					if (flag2)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004184 File Offset: 0x00002384
		private double GetObjectiveValue(Objective objective, IMember1D Member, ElementGroup EG, double L_remain, bool alreadyused)
		{
			double result;
			switch (objective)
			{
			case Objective.MinStructureMass:
			{
				bool flag = EG.Type == ElementType.Reuse;
				if (flag)
				{
					result = EG.Material.Density * EG.CrossSection.Area * Member.Length;
				}
				else
				{
					bool flag2 = EG.Type == ElementType.New;
					if (flag2)
					{
						result = EG.Material.Density * EG.CrossSection.Area * Member.Length;
					}
					else
					{
						result = 0.0;
					}
				}
				break;
			}
			case Objective.MinStockMass:
			{
				bool flag3 = EG.Type == ElementType.Reuse;
				if (flag3)
				{
					if (alreadyused)
					{
						result = 0.0;
					}
					else
					{
						result = EG.Material.Density * EG.Length * EG.CrossSection.Area;
					}
				}
				else
				{
					bool flag4 = EG.Type == ElementType.New;
					if (flag4)
					{
						result = EG.Material.Density * Member.Length * EG.CrossSection.Area;
					}
					else
					{
						result = 0.0;
					}
				}
				break;
			}
			case Objective.MinWaste:
			{
				bool flag5 = EG.Type == ElementType.Reuse;
				if (flag5)
				{
					bool flag6 = EG.CanBeCut && L_remain >= Member.Length;
					if (flag6)
					{
						if (alreadyused)
						{
							result = -EG.Material.Density * EG.CrossSection.Area * Member.Length;
						}
						else
						{
							result = EG.Material.Density * EG.CrossSection.Area * (L_remain - Member.Length);
						}
					}
					else
					{
						bool flag7 = EG.CanBeCut && L_remain < Member.Length;
						if (flag7)
						{
							result = 0.5 * EG.Material.Density * EG.CrossSection.Area * (-L_remain + Member.Length);
						}
						else
						{
							bool flag8 = !EG.CanBeCut && EG.Length < Member.Length;
							if (flag8)
							{
								result = 0.0;
							}
							else
							{
								result = 0.5 * EG.Material.Density * EG.CrossSection.Area * (EG.Length - Member.Length);
							}
						}
					}
				}
				else
				{
					bool flag9 = EG.Type == ElementType.New;
					if (flag9)
					{
						result = 0.0;
					}
					else
					{
						result = 0.0;
					}
				}
				break;
			}
			case Objective.MinLCA:
			{
				bool flag10 = L_remain < EG.Length;
				if (flag10)
				{
					result = this.LCA.ReturnElementMemberImpact(EG, true, Member);
				}
				else
				{
					bool promoteFirstPart = this.PromoteFirstPart;
					if (promoteFirstPart)
					{
						result = this.LCA.ReturnElementMemberImpact(EG, true, Member);
					}
					else
					{
						result = this.LCA.ReturnElementMemberImpact(EG, false, Member);
					}
				}
				break;
			}
			default:
			{
				bool flag11 = EG.Type == ElementType.Reuse;
				if (flag11)
				{
					result = EG.Material.Density * EG.CrossSection.Area * Member.Length;
				}
				else
				{
					bool flag12 = EG.Type == ElementType.New;
					if (flag12)
					{
						result = EG.Material.Density * EG.CrossSection.Area * Member.Length;
					}
					else
					{
						result = 0.0;
					}
				}
				break;
			}
			}
			return result;
		}

		// Token: 0x0400002B RID: 43
		private ILCA LCA;
	}
}
