using System;
using System.Collections.Generic;
using System.Linq;
using Gurobi;
using Beaver3D.LCA;
using Beaver3D.Model;
using Beaver3D.Reuse;
namespace Beaver3D.Optimization.SAND
{

    /*
     * 用于计算多个可重复利用结构的杆件重复利用率，这里的主要方法是新建了有关于结构是否是每一个库存构件组中最大的使用数量的二元变量
     * Written by JiajunLi
     * 更新于2023.05.02
     * 
     * To do: 初次计算完成，但是暂时同一截面不同长度库存会有bug，这里要修改变量，要精简库存组为截面相同组。另外需要输出每个长度每个截面杆件的生产数量，可以根据变量结果直接计算
     */

    class SANDGurobiJJ
    {
        //添加关于每个结构所用的某一种类长度的杆件是否是每一个库存构件组中最大的使用数量的二元变量，最大的使用量则代表最后的生产量，其它的则可以重复使用
        public static GRBVar[,,] GetGurobiAssignmentIsLargeVariables(GRBModel Model, Structure Structure, Stock Stock, OptimOptions Options)
        {
            //return Model.AddVars(Stock.ElementGroups.Count * Structure.member_expectLenth.Count * Structure.merge_structure_num, 'B');

            GRBVar[,,] status = new GRBVar[Stock.ElementGroups.Count, Structure.member_prductionLenth.Count, Structure.merge_structure_num];

            for (int i = 0; i < Stock.ElementGroups.Count; i++)
            {
                for (int j = 0; j < Structure.member_prductionLenth.Count; j++)
                {
                    for (int k = 0; k < Structure.merge_structure_num; k++)
                    {
                        status[i, j, k] = Model.AddVar(0, 1, 0, GRB.BINARY, "o" + i + j + k);
                    }
                }
            }
            return status;
        }

        //添加约束：每个库存组只有一个结构使用数量最大(出现相同的情况则随机一个为最大)
        public static void OnlyOneIsLargestUse(GRBModel model, GRBVar[,,] IsLarge, Structure Structure, Stock Stock)
        {
            int num = Structure.merge_structure_num;   //结构数量

            for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每个库存组
            {
                for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度
                {
                    GRBLinExpr grblinExpr = new GRBLinExpr();
                    for (int k = 0; k < num; k++)    //每个组的该长度构件是否是每个结构最大使用数
                    {
                        grblinExpr.AddTerm(1.0, IsLarge[i, j, k]);
                    }
                    model.AddConstr(grblinExpr, '=', 1, "OnlyOneIsLargestUse_group" + i + j);
                }
            }
        }

        //添加约束：某一个结构为某个库存组最大使用时，其使用的杆件数量大于另外的结构
        public static void LargerThanOther(GRBModel model, GRBVar[] T, GRBVar[,,] IsLarge, Structure Structure, Stock Stock)
        {
            int num = Structure.merge_structure_num;   //结构数量

            for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每库存个组
            {

                for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度
                {
                    for (int k = 0; k < num; k++)    //每个结构
                    {
                        for (int m = 0; m < num; m++)    //每个结构和其他结构比较
                        {
                            if (k != m)  //两个结构不相同
                            {
                                GRBLinExpr grblinExpr = new GRBLinExpr();

                                foreach (IMember member in Structure.Members)  //对于每个杆件
                                {
                                    IMember1D member1D = (IMember1D)member;

                                    if (member1D.Production_length == Structure.member_prductionLenth[j])  //如果杆件长度等于目标长度，结构等于目标结构
                                    {
                                        if (member1D.structure_num == k)
                                        {
                                            grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i]);  //每个库存组的相同长度，相同结构的杆件数量
                                        }
                                        else if (member1D.structure_num == m)
                                        {
                                            grblinExpr.AddTerm(-1.0, T[member1D.Number * Stock.ElementGroups.Count + i]); //减去其他结构的数量
                                        }
                                    }
                                }

                                model.AddGenConstrIndicator(IsLarge[i, j, k], 1, grblinExpr, '>', 0, "OneIsLargerThanOther" + i + j + k);   //当某个结构是最大时
                            }
                        }
                    }

                }
            }
        }

        //添加目标：最小化最终生产杆件的数量
        public static void SetObjective(Objective Objective, GRBModel model, GRBVar[] T, GRBVar[,,] IsLarge, Structure Structure, Stock Stock, ILCA LCA = null)
        {
            GRBLinExpr grblinExpr = new GRBLinExpr();
            GRBQuadExpr grbquadxpr = new GRBQuadExpr();
            switch (Objective)
            {
                case Objective.MinStructureMass:     //最小化所有结构的总重量
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

                case Objective.MaxReuseRate:     //最大化重复利用率,即最小化最终杆件的数量               
                    for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每库存个组
                    {
                        for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度
                        {
                            for (int k = 0; k < Structure.merge_structure_num; k++)    //每个结构
                            {
                                foreach (IMember member in Structure.Members)  //对于每个杆件
                                {
                                    IMember1D member1D = (IMember1D)member;

                                    if (member1D.Production_length == Structure.member_prductionLenth[j])  //如果杆件长度等于目标长度，结构等于目标结构
                                    {
                                        if (member1D.structure_num == k)
                                        {
                                            grbquadxpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i], IsLarge[i, j, k]);  //每个库存组的相同长度，相同结构的杆件数量
                                        }
                                    }
                                }
                            }
                        }
                    }
                    model.SetObjective(grbquadxpr, GRB.MINIMIZE);  //默认是求最小化
                    break;

                case Objective.MinLCA:     //最小化碳排放生命周期（多目标优化）                                 

                    double RatioWeight = 1.0;
                    double MassWeight = 0.5;

                    for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每库存个组
                    {
                        for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度
                        {
                            for (int k = 0; k < Structure.merge_structure_num; k++)    //每个结构
                            {
                                foreach (IMember member in Structure.Members)  //对于每个杆件
                                {
                                    IMember1D member1D = (IMember1D)member;

                                    if (member1D.Production_length == Structure.member_prductionLenth[j])  //如果杆件长度等于目标长度，结构等于目标结构
                                    {
                                        if (member1D.structure_num == k)
                                        {
                                            grbquadxpr.AddTerm(RatioWeight, T[member1D.Number * Stock.ElementGroups.Count + i], IsLarge[i, j, k]);  //每个库存组的相同长度，相同结构的杆件数量
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (IMember member in Structure.Members)
                    {
                        IMember1D member1D = (IMember1D)member;
                        for (int i = 0; i < Stock.ElementGroups.Count; i++)
                        {
                            //计算每根杆件的质量，长度*密度*截面面积*分配binary
                            grbquadxpr.AddTerm(MassWeight * member1D.Length * Stock.ElementGroups[i].Material.Density * Stock.ElementGroups[i].CrossSection.Area, T[member1D.Number * Stock.ElementGroups.Count + i]);
                        }
                    }

                    model.SetObjective(grbquadxpr, GRB.MINIMIZE);  //默认是求最小化
                    break;

                default:          //其他结果均最大化重复利用率

                    for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每库存个组
                    {
                        for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度
                        {
                            for (int k = 0; k < Structure.merge_structure_num; k++)    //每个结构
                            {
                                foreach (IMember member in Structure.Members)  //对于每个杆件
                                {
                                    IMember1D member1D = (IMember1D)member;

                                    if (member1D.Production_length == Structure.member_prductionLenth[j])  //如果杆件长度等于目标长度，结构等于目标结构
                                    {
                                        if (member1D.structure_num == k)
                                        {
                                            grbquadxpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i], IsLarge[i, j, k]);  //每个库存组的相同长度，相同结构的杆件数量
                                        }
                                    }
                                }

                            }
                        }
                    }
                    model.SetObjective(grbquadxpr, GRB.MINIMIZE);  //默认是求最小化
                    break;
            }
        }

        //存量可用约束：相同截面不同长度杆件的单个结构最大用量，其总和用量应该小于等于库存中该截面杆件的存量（//不可切割）
        public static void AddAvailabilityNotCut(GRBModel model, GRBVar[] T, GRBVar[,,] IsLarge, Structure Structure, Stock Stock, List<double> crossSectionArea, List<string> crossSectionType)
        {

            for (int m = 0; m < crossSectionType.Count; m++)  //判断库存元素的类型与截面是否相同
            {
                GRBQuadExpr grbquadxpr = new GRBQuadExpr();
                for (int i = 0; i < Stock.ElementGroups.Count; i++)  //对于每个库存种类
                {
                    if (Stock.ElementGroups[i].CrossSection.TypeName == crossSectionType[m] && Stock.ElementGroups[i].CrossSection.Area == crossSectionArea[m])  //如果相同
                    {
                        for (int j = 0; j < Structure.member_prductionLenth.Count; j++)    //对于每个生产的长度 
                        {
                            bool flag = Stock.ElementGroups[i].Type == ElementType.Reuse;
                            if (flag)
                            {
                                foreach (IMember member in Structure.Members)
                                {
                                    for (int k = 0; k < Structure.merge_structure_num; k++)    //每个结构
                                    {
                                        if (member.structure_num == k)
                                        {
                                            IMember1D member1D = (IMember1D)member;
                                            grbquadxpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i], IsLarge[i, j, k]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //model.AddQConstr(grbquadxpr, '<', (double)Stock.ElementGroups[i].NumberOfElements, "MultyStructureAvail_" + Stock.ElementGroups[i].Number.ToString());
            }
        }

        ////存量可用约束：相同截面不同长度杆件的单个结构最大用量，其总和用量应该小于等于库存中该截面杆件的存量（//可切割）
        //public static void AddAvailabilityCut(GRBModel model, GRBVar[] T, Structure Structure, Stock Stock)
        //{
        //    for (int i = 0; i < Stock.ElementGroups.Count; i++)  //对于每个库存种类
        //    {
        //        bool flag = Stock.ElementGroups[i].Type == ElementType.Reuse;
        //        if (flag)
        //        {
        //            GRBLinExpr grblinExpr = new GRBLinExpr();
        //            foreach (IMember member in Structure.Members)
        //            {
        //                IMember1D member1D = (IMember1D)member;
        //                grblinExpr.AddTerm(1.0, T[member1D.Number * Stock.ElementGroups.Count + i]);
        //            }
        //            model.AddConstr(grblinExpr, '<', (double)Stock.ElementGroups[i].NumberOfElements, "Avail_" + Stock.ElementGroups[i].Number.ToString());
        //        }
        //    }
        //}


        ////添加辅助变量：每一个结构所用杆件是否是该截面库存（长度一定）的该长度杆件的最大使用个数，表示为二元变量
        //public static GRBVar[] GetGurobiAssignmentVariablesUnderSectiosn(GRBModel Model, Structure Structure, Stock Stock, OptimOptions Options)
        //{
        //    return Model.AddVars(Structure.Members.OfType<IMember1D>().Count<IMember1D>() * Stock.ElementGroups.Count, 'B');
        //}

        ////添加约束：每个库存组只有一个结构使用数量最大(出现相同的情况则随机一个为最大)
        //public static void OnlyOneIsLargestUse(GRBModel model, GRBVar[] IsLarge, Structure Structure, Stock Stock)
        //{
        //    for (int j = 0; j < Stock.ElementGroups.Count; j++)     //对于每个库存组
        //    {
        //        foreach (IMember member in Structure.Members)  //对于每个杆件
        //        {
        //            IMember1D member1D = (IMember1D)member;

        //            GRBLinExpr grblinExpr = new GRBLinExpr();

        //            for (int i = 0; i < Structure.member_expectLenth.Count; i++)   //对于每种预制构件聚类的生产长度
        //            {
        //                if (member1D.Length == Structure.member_expectLenth[i])   //如果杆件长度等于预制长度
        //                {
        //                    grblinExpr.AddTerm(1, IsLarge[member1D.Number * Stock.ElementGroups.Count + j]);
        //                }
        //            }
        //            model.AddConstr(grblinExpr, '=', 1, "OnlyOneIsLargestUse_group" + i + j);    //添加约束

        //        }
        //    }

        //}

    }



}
