using Beaver3D.Model.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Beaver3D.Reuse
{
    // Token: 0x02000008 RID: 8
    public class Stock
    {
        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000051 RID: 81 RVA: 0x00002E97 File Offset: 0x00001097
        // (set) Token: 0x06000052 RID: 82 RVA: 0x00002E9F File Offset: 0x0000109F
        public List<ElementGroup> ElementGroups { get; private set; } = new List<ElementGroup>();

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000053 RID: 83 RVA: 0x00002EA8 File Offset: 0x000010A8
        // (set) Token: 0x06000054 RID: 84 RVA: 0x00002EB0 File Offset: 0x000010B0
        public SortStockElementsBy SortBy { get; private set; } = SortStockElementsBy.Off;

        public List<ICrossSection> crossSectionType { get; set; } = new List<ICrossSection>();   //不同的截面种类（这里是筛掉去掉了重复截面但不同长度的）

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000055 RID: 85 RVA: 0x00002EB9 File Offset: 0x000010B9
        // (set) Token: 0x06000056 RID: 86 RVA: 0x00002EC1 File Offset: 0x000010C1
        public List<int> SortMap { get; private set; } = new List<int>();

        // Token: 0x06000057 RID: 87 RVA: 0x00002ECA File Offset: 0x000010CA
        public Stock() : this(new List<ElementGroup>(), SortStockElementsBy.Off)
        {
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00002EDA File Offset: 0x000010DA
        public Stock(List<ElementGroup> ElementGroups) : this(ElementGroups, SortStockElementsBy.Off)
        {
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00002EE8 File Offset: 0x000010E8
        public Stock(List<ElementGroup> ElementGroups, SortStockElementsBy SortBy)
        {
            this.ElementGroups = ElementGroups;
            this.SortBy = SortBy;
            this.SortElementGroups(SortBy);
            this.SetElementGroupNumber();
        }

        // Token: 0x0600005A RID: 90 RVA: 0x00002F39 File Offset: 0x00001139
        public void AddElementGroup(ElementGroup ElementGroup)
        {
            this.ElementGroups.Add(ElementGroup);
            this.SortElementGroups(this.SortBy);
            this.SetElementGroupNumber();
        }

        public void GetDifferentCrossSectionGroup()
        {
            crossSectionType = new List<ICrossSection>();
            for (int i = 0; i < this.ElementGroups.Count; i++)
            {
                if (crossSectionType.Count > 0)
                {
                    bool alreadyContain = false;
                    
                    for (int j = 0; j < crossSectionType.Count; j++)
                    {
                        if ( crossSectionType[j].Name == this.ElementGroups[i].CrossSection.Name  &&  Math.Abs(crossSectionType[j].Area - this.ElementGroups[i].CrossSection.Area) < 0.001)
                        {
                            alreadyContain = true;
                            break;
                        }

                    }
                    if (!alreadyContain)
                    {
                        crossSectionType.Add(this.ElementGroups[i].CrossSection);
                    }
                }
                else
                {
                    crossSectionType.Add(this.ElementGroups[i].CrossSection);
                }       
            }

            this.crossSectionType = crossSectionType;
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00002F5D File Offset: 0x0000115D
        public void InsertElementGroup(int Index, ElementGroup ElementGroup)
        {
            this.ElementGroups.Insert(Index, ElementGroup);
            this.SortElementGroups(this.SortBy);
            this.SetElementGroupNumber();
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00002F82 File Offset: 0x00001182
        public void RemoveElementGroup(int Index)
        {
            this.ElementGroups.RemoveAt(Index);
            this.SortElementGroups(this.SortBy);
            this.SetElementGroupNumber();
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00002FA8 File Offset: 0x000011A8
        private void SetElementGroupNumber()
        {
            for (int i = 0; i < this.ElementGroups.Count; i++)
            {
                this.ElementGroups[i].SetNumber(i);
            }
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00002FE5 File Offset: 0x000011E5
        public void ClearElementGroup()
        {
            this.ElementGroups.Clear();
            this.SortBy = SortStockElementsBy.Off;
            this.SortMap.Clear();
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00003008 File Offset: 0x00001208
        public void SortElementGroups(SortStockElementsBy sort)
        {
            this.SortBy = sort;
            switch (this.SortBy)
            {
                case SortStockElementsBy.Type:
                    {
                        this.SortMap.Clear();
                        var source = (from pair in this.ElementGroups.Zip(Enumerable.Range(0, this.ElementGroups.Count), (ElementGroup x, int y) => new
                        {
                            x,
                            y
                        })
                                      orderby pair.x.Type
                                      select pair).ToList();
                        this.ElementGroups = (from pair in source
                                              select pair.x).ToList<ElementGroup>();
                        this.SortMap = (from pair in source
                                        select pair.y).ToList<int>();
                        break;
                    }
                case SortStockElementsBy.ForceThenLength:
                    {
                        this.SortMap.Clear();
                        var source2 = (from pair in this.ElementGroups.Zip(Enumerable.Range(0, this.ElementGroups.Count), (ElementGroup x, int y) => new
                        {
                            x,
                            y
                        })
                                       orderby pair.x.Type, Math.Abs(pair.x.CrossSection.Area), pair.x.Length
                                       select pair).ToList();
                        this.ElementGroups = (from pair in source2
                                              select pair.x).ToList<ElementGroup>();
                        this.SortMap = (from pair in source2
                                        select pair.y).ToList<int>();
                        break;
                    }
                case SortStockElementsBy.LengthThenForce:
                    {
                        this.SortMap.Clear();
                        var source3 = (from pair in this.ElementGroups.Zip(Enumerable.Range(0, this.ElementGroups.Count), (ElementGroup x, int y) => new
                        {
                            x,
                            y
                        })
                                       orderby pair.x.Type, pair.x.Length, Math.Abs(pair.x.CrossSection.Area)
                                       select pair).ToList();
                        this.ElementGroups = (from pair in source3
                                              select pair.x).ToList<ElementGroup>();
                        this.SortMap = (from pair in source3
                                        select pair.y).ToList<int>();
                        break;
                    }
            }
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00003364 File Offset: 0x00001564
        internal Stock ExtendStock()
        {
            List<ElementGroup> list = new List<ElementGroup>();
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                for (int i = 0; i < elementGroup.NumberOfElements; i++)
                {
                    list.Add(new ElementGroup(elementGroup.Type, elementGroup.Material, elementGroup.CrossSection, elementGroup.Length, 1, elementGroup.CanBeCut, null));
                }
            }
            return new Stock(list);
        }

        // Token: 0x06000061 RID: 97 RVA: 0x0000340C File Offset: 0x0000160C
        internal Stock ReduceStock(Stock OriginalStock)
        {
            throw new NotImplementedException();
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00003414 File Offset: 0x00001614
        public void ResetAssignedMembers()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetAssignedMembers();
            }
        }

        // Token: 0x06000063 RID: 99 RVA: 0x0000346C File Offset: 0x0000166C
        public void ResetRemainLenghts()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetRemainLengths();
            }
        }

        // Token: 0x06000064 RID: 100 RVA: 0x000034C4 File Offset: 0x000016C4
        public void ResetRemainLenghtsTemp()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetRemainLengthsTemp();
            }
        }

        // Token: 0x06000065 RID: 101 RVA: 0x0000351C File Offset: 0x0000171C
        public void ResetAlreadyCounted()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetAlreadyCounted();
            }
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00003574 File Offset: 0x00001774
        public void ResetNext()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetNext();
            }
        }

        // Token: 0x06000067 RID: 103 RVA: 0x000035CC File Offset: 0x000017CC
        public void ResetStacks()
        {
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                elementGroup.ResetStack();
            }
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00003624 File Offset: 0x00001824
        public Stock Clone()
        {
            Stock stock = new Stock();
            stock.ElementGroups = new List<ElementGroup>();
            foreach (ElementGroup elementGroup in this.ElementGroups)
            {
                stock.ElementGroups.Add(elementGroup.Clone());
            }
            stock.SortBy = this.SortBy;
            stock.crossSectionType = this.crossSectionType;
            stock.SortMap = new List<int>();
            foreach (int item in this.SortMap)
            {
                stock.SortMap.Add(item);
            }
            stock.ResetAlreadyCounted();
            stock.ResetAssignedMembers();
            stock.ResetRemainLenghts();
            stock.ResetRemainLenghtsTemp();
            stock.ResetStacks();
            stock.ResetNext();
            return stock;
        }
    }
}
