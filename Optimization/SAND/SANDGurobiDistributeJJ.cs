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
     * 更新于2023.04.14
     * 
     * To do: 完善约束，修改struture类，使得其包含多个结构的杆件信息
     */

    class SANDGurobiDistributeJJ
    {
        //添加关于每个结构所用的杆件是否是每一个库存构件组中最大的使用数量的二元变量，最大的使用量则代表最后的生产量，其它的则可以重复使用
        public static GRBVar[] GetGurobiAssignmentVariables(GRBModel Model, Structure Structure, Stock Stock, OptimOptions Options)
        {
            return Model.AddVars(Structure.merge_structure_num * Stock.ElementGroups.Count, 'B');
        }

        //添加约束：每个库存组只有一个结构使用数量最大(出现相同的情况则随机一个为最大)
        public static void OnlyOneIsLargestUse(GRBModel model, GRBVar[] IsLarge, Structure Structure, Stock Stock)
        {
            int num = Structure.merge_structure_num;   //结构数量

            for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每个库存组
            {
                GRBLinExpr grblinExpr = new GRBLinExpr();
                for (int j = 0; j < num; j++)    //每个组每个结构是否是最大使用数
                {
                    grblinExpr.AddTerm(1, IsLarge[i * num + j]);
                }
                model.AddConstr(grblinExpr, '=', 1, "OnlyOneIsLargestUse_group" + i);
            }
        }

        //添加约束：某一个结构为某个库存组最大使用时，其使用的杆件数量大于另外的结构
        public static void LargerThanOther(GRBModel model, GRBVar[] T, GRBVar[] IsLarge, Structure Structure, Stock Stock)
        {
            int num = Structure.merge_structure_num;   //结构数量
            for (int i = 0; i < Stock.ElementGroups.Count; i++)   //每库存个组
            {
                //model.AddGenConstrIndicator();   //当某个结构是最大时
            }
        }
    }
}
