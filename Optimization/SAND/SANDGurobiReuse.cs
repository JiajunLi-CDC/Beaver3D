using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.LCA;
using Beaver3D.Model;
using Beaver3D.Reuse;

namespace Beaver3D.Optimization.SAND
{
    // Token: 0x02000020 RID: 32
    public static class SANDGurobiReuse
    {
        //添加关于分配的变量，数量为stock中杆件的数量 * 结构中的杆件数量，变量类型为二元变量binary，表示是否在结构S的位置i放置库存stock杆件j
        public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock, OptimOptions Options)
        {
            return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
        }

        //添加关于分配的变量，定义截面用，数量为stock中杆件的数量 * 结构中的杆件数量，变量类型为二元变量binary，表示是否在结构S的位置i放置库存stock杆件j
        public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, List<Structure> All_Structures, Stock Stock, OptimOptions Options)
        {
            int num = 0;
            for (int i = 0; i < All_Structures.Count; i++)
            {
                num += All_Structures[i].Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count;
            }
            return Model.AddVars(num, 'B');
        }


        public static GRBVar[] GetGurobiCuttingStockVariables(GRBModel Model, Stock Stock)
        {
            return Model.AddVars(Stock.ElementGroups.Count, 'B');
        }

        //获取 Gurobi 杆件成员力变量p，,这里为连续变量，详见论文《Design of Truss Structures Through Reuse》
        public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberForceVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases)
        {
            Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
            int num = Structure.Members.OfType<IMember1D>().Count<IMember1D>();   //每个loadcase的变量数量都和杆件数量相同
            foreach (LoadCase key in LoadCases)
            {
                dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num).ToArray<double>(), new double[num], Enumerable.Repeat<char>('C', num).ToArray<char>(), null));
            }
            return dictionary;
        }


        //获取 Gurobi 成员伸长变量,这里为连续变量
        public static Dictionary<LoadCase, GRBVar[]> GetGurobiMemberElongationVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases, Stock Stock)
        {
            Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
            int num = Structure.Members.OfType<Bar>().Count<Bar>();
            int count = Stock.ElementGroups.Count;
            foreach (LoadCase key in LoadCases)
            {
                dictionary.Add(key, Model.AddVars(Enumerable.Repeat<double>(-1E+100, num * count).ToArray<double>(), Enumerable.Repeat<double>(1E+100, num * count).ToArray<double>(), new double[num * count], Enumerable.Repeat<char>('C', num * count).ToArray<char>(), null));
            }
            return dictionary;
        }

        // Token: 0x0600013D RID: 317 RVA: 0x0000E500 File Offset: 0x0000C700
        //获取节点位移变量
        public static Dictionary<LoadCase, GRBVar[]> GetGurobiDisplacementVariables(GRBModel Model, Structure Structure, List<LoadCase> LoadCases)
        {
            Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
            foreach (LoadCase loadCase in LoadCases)
            {
                GRBVar[] array = new GRBVar[Structure.NFreeTranslations];
                foreach (Node node in Structure.Nodes)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bool flag = node.Fix[i];
                        if (!flag)
                        {
                            bool flag2 = node.DisplacementBounds.ContainsKey(loadCase);
                            if (flag2)
                            {
                                array[node.ReducedDofsTruss[i]] = Model.AddVar(node.DisplacementBounds[loadCase].LB[i], node.DisplacementBounds[loadCase].UB[i], 0.0, 'C', string.Concat(new string[]
                                {
                                    "u_LC_",
                                    loadCase.Name,
                                    "Node",
                                    node.Number.ToString(),
                                    "d",
                                    i.ToString()
                                }));
                            }
                            else
                            {
                                array[node.ReducedDofsTruss[i]] = Model.AddVar(-1E+100, 1E+100, 0.0, 'C', string.Concat(new string[]
                                {
                                    "u_LC_",
                                    loadCase.Name,
                                    "Node",
                                    node.Number.ToString(),
                                    "d",
                                    i.ToString()
                                }));
                            }
                        }
                    }
                }
                dictionary.Add(loadCase, array);
            }
            return dictionary;
        }

        // Token: 0x0600013E RID: 318 RVA: 0x0000E730 File Offset: 0x0000C930
        //设置优化目标
        public static void SetObjective(Objective Objective, GRBModel model, GRBVar[] T, Structure Structure, Stock Stock, ILCA LCA = null)
        {
            GRBLinExpr grblinExpr = new GRBLinExpr();
            switch (Objective)
            {
                case Objective.MinStructureMass:     //最小化结构重量
                    foreach (IMember member in Structure.Members)
                    {
                        IMember1D member1D = (IMember1D)member;
                        for (int i = 0; i < Stock.ElementGroups.Count; i++)
                        {
                            //计算每根杆件的质量，长度*密度*截面面积*分配binary
                            grblinExpr += T[member1D.Number * Stock.ElementGroups.Count + i] * member1D.Length * Stock.ElementGroups[i].Material.Density * Stock.ElementGroups[i].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);  //默认是求最小化
                    break;
                case Objective.MinStockMass:  //最小化所用库存质量
                    for (int j = 0; j < Stock.ElementGroups.Count; j++)
                    {
                        foreach (IMember member2 in Structure.Members)
                        {
                            IMember1D member1D2 = (IMember1D)member2;
                            bool flag = Stock.ElementGroups[j].Type == ElementType.Reuse;
                            if (flag)
                            {
                                grblinExpr += T[member1D2.Number * Stock.ElementGroups.Count + j] * Stock.ElementGroups[j].Length * Stock.ElementGroups[j].Material.Density * Stock.ElementGroups[j].CrossSection.Area;
                            }
                            else
                            {
                                grblinExpr += T[member1D2.Number * Stock.ElementGroups.Count + j] * member1D2.Length * Stock.ElementGroups[j].Material.Density * Stock.ElementGroups[j].CrossSection.Area;
                            }
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                case Objective.MinWaste:  //最小浪费质量（杆件的切割浪费）
                    for (int k = 0; k < Stock.ElementGroups.Count; k++)
                    {
                        foreach (IMember member3 in Structure.Members)
                        {
                            IMember1D member1D3 = (IMember1D)member3;
                            grblinExpr += T[member1D3.Number * Stock.ElementGroups.Count + k] * (Stock.ElementGroups[k].Length - member1D3.Length) * Stock.ElementGroups[k].Material.Density * Stock.ElementGroups[k].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                case Objective.MinLCA:  //最小生命周期碳排放，进行生命周期评估 (LCA) 以量化钢结构的隐含能量和碳。
                    foreach (IMember member4 in Structure.Members)
                    {
                        IMember1D member1D4 = (IMember1D)member4;
                        for (int l = 0; l < Stock.ElementGroups.Count; l++)
                        {
                            grblinExpr += T[member1D4.Number * Stock.ElementGroups.Count + l] * LCA.ReturnElementMemberImpact(Stock.ElementGroups[l], false, member1D4);
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                default:          //其他结果均最小化结构重量
                    foreach (IMember member5 in Structure.Members)
                    {
                        Bar bar = (Bar)member5;
                        for (int m = 0; m < Stock.ElementGroups.Count; m++)
                        {
                            //计算每根杆件的质量，长度*密度*截面面积*分配binary
                            grblinExpr += T[bar.Number * Stock.ElementGroups.Count + m] * bar.Length * Stock.ElementGroups[m].Material.Density * Stock.ElementGroups[m].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
            }
        }

        // Token: 0x0600013F RID: 319 RVA: 0x0000EC98 File Offset: 0x0000CE98
        public static void SetObjectiveCuttingStock(Objective Objective, GRBModel model, GRBVar[] T, GRBVar[] Y, Structure Structure, Stock Stock, ILCA LCA = null)
        {
            GRBLinExpr grblinExpr = new GRBLinExpr();
            switch (Objective)
            {
                case Objective.MinStructureMass:
                    foreach (IMember member in Structure.Members)
                    {
                        IMember1D member1D = (IMember1D)member;
                        for (int i = 0; i < Stock.ElementGroups.Count; i++)
                        {
                            grblinExpr += T[member1D.Number * Stock.ElementGroups.Count + i] * member1D.Length * Stock.ElementGroups[i].Material.Density * Stock.ElementGroups[i].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                case Objective.MinStockMass:
                    for (int j = 0; j < Stock.ElementGroups.Count; j++)
                    {
                        bool flag = Stock.ElementGroups[j].Type == ElementType.Reuse;
                        if (flag)
                        {
                            grblinExpr += Y[j] * Stock.ElementGroups[j].Length * Stock.ElementGroups[j].Material.Density * Stock.ElementGroups[j].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                case Objective.MinWaste:
                    for (int k = 0; k < Stock.ElementGroups.Count; k++)
                    {
                        grblinExpr += Y[k] * Stock.ElementGroups[k].Length * Stock.ElementGroups[k].Material.Density * Stock.ElementGroups[k].CrossSection.Area;
                        foreach (IMember member2 in Structure.Members)
                        {
                            IMember1D member1D2 = (IMember1D)member2;
                            grblinExpr -= T[member1D2.Number * Stock.ElementGroups.Count + k] * member1D2.Length * Stock.ElementGroups[k].Material.Density * Stock.ElementGroups[k].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                case Objective.MinLCA:
                    for (int l = 0; l < Stock.ElementGroups.Count; l++)
                    {
                        grblinExpr += Y[l] * LCA.ReturnStockElementImpact(Stock.ElementGroups[l]);
                        foreach (IMember member3 in Structure.Members)
                        {
                            IMember1D member1D3 = (IMember1D)member3;
                            grblinExpr += T[member1D3.Number * Stock.ElementGroups.Count + l] * LCA.ReturnElementMemberImpact(Stock.ElementGroups[l], true, member1D3);
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
                default:
                    foreach (IMember member4 in Structure.Members)
                    {
                        Bar bar = (Bar)member4;
                        for (int m = 0; m < Stock.ElementGroups.Count; m++)
                        {
                            grblinExpr += T[bar.Number * Stock.ElementGroups.Count + m] * bar.Length * Stock.ElementGroups[m].Material.Density * Stock.ElementGroups[m].CrossSection.Area;
                        }
                    }
                    model.SetObjective(grblinExpr);
                    break;
            }
        }

        // Token: 0x06000140 RID: 320 RVA: 0x0000F16C File Offset: 0x0000D36C
        public static void AddGroup(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock)
        {
            foreach (KeyValuePair<int, List<IMember>> keyValuePair in Structure.MemberGroups)
            {
                bool flag = keyValuePair.Key == -1;
                if (!flag)
                {
                    int number = keyValuePair.Value[0].Number;
                    for (int i = 1; i < keyValuePair.Value.Count; i++)
                    {
                        int number2 = keyValuePair.Value[i].Number;
                        for (int j = 0; j < Stock.ElementGroups.Count; j++)
                        {
                            model.AddConstr(T[number * Stock.ElementGroups.Count + j], '=', T[number2 * Stock.ElementGroups.Count + j], null);
                        }
                    }
                }
            }
        }

        // Token: 0x06000141 RID: 321 RVA: 0x0000F280 File Offset: 0x0000D480
        //长度约束，所使用的库存元素长度分配应该大于等于结构中每根杆件所需的长度
        public static void AddLength(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock)
        {
            foreach (IMember member in Structure.Members)
            {
                IMember1D member1D = (IMember1D)member;
                GRBLinExpr grblinExpr = new GRBLinExpr();
                for (int i = 0; i < Stock.ElementGroups.Count; i++)
                {
                    bool flag = Stock.ElementGroups[i].Type == ElementType.Reuse;
                    if (flag)
                    {
                        grblinExpr.AddTerm(Stock.ElementGroups[i].Length - member1D.Length, T[member1D.Number * Stock.ElementGroups.Count + i]);
                    }
                }
                model.AddConstr(grblinExpr, '>', 0.0, "Length" + member1D.Number.ToString());
            }
        }

        // Token: 0x06000142 RID: 322 RVA: 0x0000F384 File Offset: 0x0000D584
        public static void AddLengthCuttingStock(GRBModel model, GRBVar[] T, GRBVar[] Y, Structure Structure, Stock Stock)
        {
            for (int i = 0; i < Stock.ElementGroups.Count; i++)
            {
                bool flag = Stock.ElementGroups[i].Type == ElementType.Reuse;
                if (flag)
                {
                    GRBLinExpr grblinExpr = new GRBLinExpr();
                    foreach (IMember member in Structure.Members)
                    {
                        IMember1D member1D = (IMember1D)member;
                        grblinExpr.AddTerm(member1D.Length, T[member1D.Number * Stock.ElementGroups.Count + i]);
                    }
                    model.AddConstr(grblinExpr, '<', Stock.ElementGroups[i].Length * Y[i], "Length_" + Stock.ElementGroups[i].Number.ToString());
                }
            }
        }

        // Token: 0x06000143 RID: 323 RVA: 0x0000F494 File Offset: 0x0000D694
        //存量可用约束：结构中杆件所匹配的一种库存杆件的数量总和应该小于等于库存中该杆件的存量
        public static void AddAvailability(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock)
        {
            for (int i = 0; i < Stock.ElementGroups.Count; i++)  //对于每个库存种类
            {
                bool flag = Stock.ElementGroups[i].Type == ElementType.Reuse;
                if (flag)
                {
                    GRBLinExpr grblinExpr = new GRBLinExpr();
                    foreach (IMember member in Structure.Members)
                    {
                        IMember1D member1D = (IMember1D)member;
                        grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i]);
                    }
                    model.AddConstr(grblinExpr, '<', (double)Stock.ElementGroups[i].NumberOfElements, "Avail_" + Stock.ElementGroups[i].Number.ToString());
                }
            }
        }
    }
}
