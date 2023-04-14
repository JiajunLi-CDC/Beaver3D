using System;
using System.Collections.Generic;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization
{
	// Token: 0x02000015 RID: 21
	public class Assignment
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004E56 File Offset: 0x00003056
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004E5E File Offset: 0x0000305E
		public bool Feasible { get; private set; } = false;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004E67 File Offset: 0x00003067
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004E6F File Offset: 0x0000306F
		public double ObjectiveValue { get; private set; } = double.PositiveInfinity;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004E78 File Offset: 0x00003078
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004E80 File Offset: 0x00003080
		public List<ElementGroup> ElementGroups { get; private set; } = new List<ElementGroup>();

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004E89 File Offset: 0x00003089
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004E91 File Offset: 0x00003091
		public List<int> ElementIndices { get; private set; } = new List<int>();

		// Token: 0x060000A1 RID: 161 RVA: 0x00004ED0 File Offset: 0x000030D0
		public void SetFeasible(bool Feasible)
		{
			this.Feasible = Feasible;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004EDB File Offset: 0x000030DB
		public void SetObjectiveValue(double Obj)
		{
			this.ObjectiveValue = Obj;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004EE6 File Offset: 0x000030E6
		public void AddElementAssignment(ElementGroup ElementGroup, int ElementIndex)
		{
			this.ElementGroups.Add(ElementGroup);
			this.ElementIndices.Add(ElementIndex);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004F03 File Offset: 0x00003103
		public void ClearElementAssignments()
		{
			this.ElementGroups.Clear();
			this.ElementIndices.Clear();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004F20 File Offset: 0x00003120
		public Tuple<ElementGroup, int>[] GetAssignmentIndices()
		{
			Tuple<ElementGroup, int>[] array = new Tuple<ElementGroup, int>[this.ElementGroups.Count];
			for (int i = 0; i < this.ElementGroups.Count; i++)
			{
				array[i] = new Tuple<ElementGroup, int>(this.ElementGroups[i], this.ElementIndices[i]);
			}
			return array;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004F80 File Offset: 0x00003180
		public Assignment Clone()
		{
			Assignment assignment = new Assignment();
			assignment.ElementGroups = new List<ElementGroup>();
			assignment.ElementIndices = new List<int>();
			for (int i = 0; i < this.ElementGroups.Count; i++)
			{
				ElementGroup item = this.ElementGroups[i].Clone();
				assignment.ElementGroups.Add(item);
			}
			for (int j = 0; j < this.ElementIndices.Count; j++)
			{
				assignment.ElementIndices.Add(this.ElementIndices[j]);
			}
			assignment.Feasible = this.Feasible;
			assignment.ObjectiveValue = this.ObjectiveValue;
			return assignment;
		}
	}
}
