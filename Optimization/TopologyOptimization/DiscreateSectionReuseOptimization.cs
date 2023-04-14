//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Gurobi;
//using Beaver3D.LCA;
//using Beaver3D.Model;
//using Beaver3D.Model.CrossSections;
//using Beaver3D.Optimization.SAND;
//using Beaver3D.Reuse;
//using System.Windows.Forms;
//using System.Drawing;

///*
// * 此方法用于计算多个现有可重复利用结构之间的杆件截面，目标是最大化杆件的重复利用率
// * 更新于2023.4.13
// * programmed by Jiajun Li
// */

//namespace Beaver3D.Optimization.TopologyOptimization
//{
//    class DiscreateSectionReuseOptimization
//    {
//        //目标
//        public Objective Objective { get; private set; } = Objective.MinStructureMass;   //默认目标最小结构质量

//        // 目标值
//        public double ObjectiveValue { get; private set; } = double.PositiveInfinity;

//        // 优化的各种选项
//        public OptimOptions Options { get; private set; }

//        // 是否到达优化最大时间
//        public bool TimeLimitReached { get; private set; } = false;

//        // 是否中断优化
//        public bool Interrupted { get; private set; } = false;

//        // 优化相关信息
//        public string Message { get; private set; } = "";

//        // 优化过程时间
//        public double Runtime { get; private set; } = 0.0;

//        // 实例化对象
//        public DiscreateSectionReuseOptimization(Objective Objective, OptimOptions Options = null)
//        {
//            this.Objective = Objective;
//            bool flag = Options == null;
//            if (flag)
//            {
//                this.Options = new OptimOptions();    //使用默认的options选项
//            }
//            else
//            {
//                this.Options = Options;     //使用传递进来的options选项
//            }
//        }

//        // 优化计算（这里算上了所有的荷载）
//        public void Solve(List<Structure> AllStructures, Stock Stock)
//        {
//            List<List<string>> LoadCaseNames = new List<List<string>>();
//            for (int i = 0; i < AllStructures.Count; i++)
//            {
//                List<string> each = new List<string> { "all" };
//                LoadCaseNames.Add(each);
//            }
//            this.Solve(AllStructures, LoadCaseNames, Stock, null);
//        }

//        // 优化计算
//        public void Solve(List<Structure> AllStructures, List<List<string>> LoadCaseNames, Stock Stock, ILCA LCA = null)
//        {
//            List<List<LoadCase>> ALL_loadCasesFromNames = new List<List<LoadCase>>();  //记录所有结构的荷载

//            for (int i = 0; i < AllStructures.Count; i++)
//            {
//                List<LoadCase> loadCasesFromNames = AllStructures[i].GetLoadCasesFromNames(LoadCaseNames[i]);  //根据输入的荷载名字从结构的荷载信息中获取

//                bool flag = loadCasesFromNames == null || loadCasesFromNames.Count == 0;
//                if (flag)
//                {
//                    throw new ArgumentException("Structure" + i + "'s" + " LoadCases with the provided names are not existing in the structure. Check the names or use 'all' to compute all LoadCases.");
//                }

//                ALL_loadCasesFromNames.Add(loadCasesFromNames);
//            }


//            MILPOptimizer milpoptimizer = this.Options.MILPOptimizer;  //获取选项中的优化器选项
//            MILPOptimizer milpoptimizer2 = milpoptimizer;

//            if (milpoptimizer2 != MILPOptimizer.Gurobi)   //这一步？？？
//            {
//                this.SolveGurobiBR(AllStructures, ALL_loadCasesFromNames, Stock, LCA);
//            }
//            else
//            {
//                this.SolveGurobiBR(AllStructures, ALL_loadCasesFromNames, Stock, LCA);
//            }
//        }

//        // 运用Bruetting的公式计算
//        public void SolveGurobiBR(List<Structure> All_Structures, List<List<LoadCase>> ALL_LoadCase, Stock Stock, ILCA LCA = null)
//        {
//            //创建Gurobi环境和模型
//            GRBEnv grbenv = new GRBEnv();
//            GRBModel grbmodel = new GRBModel(grbenv);
//            grbmodel.Parameters.TimeLimit = (double)this.Options.MaxTime;  //获取选项中的计算最大时间
//            foreach (Tuple<string, string> tuple in this.Options.GurobiParameters)   //设置gurobi每个变量的参数值，一般为默认
//            {
//                grbmodel.Set(tuple.Item1, tuple.Item2);
//            }

//            bool flag = true;
//            for (int i = 0; i < All_Structures.Count; i++)
//            {
//                flag = flag && !All_Structures[i].AllTopologyFixed();  //如果至少有一个结构拓扑不固定，即可以进行拓扑优化，那么在stock中加入空杆件进行优化
//            }

//            bool flag1 = !this.Options.TopologyFixed;

//            if (flag && flag1)
//            {
//                Stock.InsertElementGroup(0, ElementGroup.ZeroElement());
//            }

//            Stock stock = Stock;
//            bool cuttingStock = this.Options.CuttingStock;
//            if (cuttingStock)
//            {
//                Stock = Stock.ExtendStock();
//            }

//            //-------------------------------------------------------------------
//            //添加Gurobi变量
//            GRBVar[] All_gurobiAssignmentVariables = SANDGurobiReuse.GetGurobiAssignmentVariables(grbmodel, All_Structures, Stock, this.Options);  //设计所有的结构的分配变量

//            for (int i = 0; i < All_Structures.Count; i++)
//            {
//                Structure Structure = All_Structures[i];
//                List<LoadCase> LoadCases = ALL_LoadCase[i];   //获取每一项

//                GRBVar[] gurobiAssignmentVariables = new GRBVar[All_Structures[i].Members.OfType<IMember1D>().Count<IMember1D>()];  //从总的变量中取出需要的部分
//                for (int j = 0; j < gurobiAssignmentVariables.Count(); j++)   //计算在这之前的变量的索引
//                {
//                    int have = 0;
//                    if (i == 0)
//                    {
//                        have = 0;
//                    }
//                    else
//                    {
//                        for (int k = 0; k < i; k++)
//                        {
//                            have += All_Structures[k].Members.OfType<IMember1D>().Count<IMember1D>();
//                        }
//                    }
//                    gurobiAssignmentVariables[j] = All_gurobiAssignmentVariables[have + j];
//                }

//                Dictionary<LoadCase, GRBVar[]> gurobiMemberForceVariables = SANDGurobiReuse.GetGurobiMemberForceVariables(grbmodel, Structure, LoadCases);  //成员力变量
//                Dictionary<LoadCase, GRBVar[]> dictionary = new Dictionary<LoadCase, GRBVar[]>();
//                Dictionary<LoadCase, GRBVar[]> dictionary2 = new Dictionary<LoadCase, GRBVar[]>();



//                bool compatibility = this.Options.Compatibility;  //结构兼容性，默认为true
//                if (compatibility)
//                {
//                    dictionary = SANDGurobiReuse.GetGurobiDisplacementVariables(grbmodel, Structure, LoadCases);   //成员节点位移变量
//                    dictionary2 = SANDGurobiReuse.GetGurobiMemberElongationVariables(grbmodel, Structure, LoadCases, Stock);    //获取 Gurobi 成员伸长变量
//                }

//                //-------------------------------------------------------------------
//                //添加Gurobi约束
//                //设计分配约束，每一个杆件只分配了一个库存元素
//                SANDGurobiDiscreteBR.AddAssignment(grbmodel, gurobiAssignmentVariables, Structure, Stock, this.Options);
//                foreach (LoadCase loadCase in LoadCases)
//                {
//                    //平衡约束（4）
//                    SANDGurobiDiscreteBR.AddEquilibrium(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock, this.Options);
//                    grbmodel.Update();
//                    bool compatibility2 = this.Options.Compatibility;
//                    if (compatibility2)
//                    {
//                        //兼容性约束（5）
//                        SANDGurobiDiscreteBR.AddCompatibility(grbmodel, dictionary2[loadCase], dictionary[loadCase], Structure, loadCase, Stock);
//                        grbmodel.Update();
//                        //几何兼容性约束
//                        SANDGurobiDiscreteBR.AddConstitutive(grbmodel, gurobiMemberForceVariables[loadCase], dictionary2[loadCase], Structure, loadCase, Stock);
//                        grbmodel.Update();
//                        //“双线性”约束可以通过“big-M”技术和辅助连续变量的引入重新表述为线性约束。
//                        SANDGurobiDiscreteBR.AddBigM(grbmodel, gurobiAssignmentVariables, dictionary2[loadCase], Structure, loadCase, Stock, this.Options);
//                        grbmodel.Update();
//                        //抗压能力约束（6）
//                        SANDGurobiDiscreteBR.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock);
//                        grbmodel.Update();
//                    }
//                    else
//                    {
//                        SANDGurobiDiscreteBR.AddStress(grbmodel, gurobiAssignmentVariables, gurobiMemberForceVariables[loadCase], Structure, loadCase, Stock);
//                    }
//                }


//                //-------------------------------------------------------------------
//                //添加Gurobi约束
//                bool flag2 = !this.Options.CuttingStock;    //默认CuttingStock为false,表示是否允许库存杆件切割（一个长的库存杆件可以切割成多个短的构件）
//                if (flag2)
//                {
//                    SANDGurobiReuse.SetObjective(this.Objective, grbmodel, gurobiAssignmentVariables, Structure, Stock, LCA);
//                    SANDGurobiReuse.AddAvailability(grbmodel, gurobiAssignmentVariables, Structure, Stock);  //库存存量可用约束，对应论文约束（2）
//                    SANDGurobiReuse.AddLength(grbmodel, gurobiAssignmentVariables, Structure, Stock);  //长度约束，分配的库存应该大于结构中的杆件长度
//                }
//                else
//                {
//                    GRBVar[] gurobiCuttingStockVariables = SANDGurobiReuse.GetGurobiCuttingStockVariables(grbmodel, Stock);
//                    SANDGurobiReuse.AddLengthCuttingStock(grbmodel, gurobiAssignmentVariables, gurobiCuttingStockVariables, Structure, Stock);
//                    SANDGurobiReuse.SetObjectiveCuttingStock(this.Objective, grbmodel, gurobiAssignmentVariables, gurobiCuttingStockVariables, Structure, Stock, LCA);
//                }
//            }



//            //-------------------------------------------------------------------
//            //Gurobi计算界面UI
//            FormStartPosition startPos;
//            Point location;
//            this.CloseOpenFormsAndGetPos(out startPos, out location);
//            LightCallback lightCallback = null;
//            LogCallback logCallback = null;
//            bool logToConsole = this.Options.LogToConsole;
//            if (logToConsole)
//            {
//                logCallback = new LogCallback(startPos, location, this.Options.LogFormName);
//                grbmodel.SetCallback(logCallback);
//            }
//            else
//            {
//                lightCallback = new LightCallback();
//                grbmodel.SetCallback(lightCallback);
//            }

//            //--------------------------------------------------------------------
//            //Gurobi计算及输出
//            try
//            {
//                grbmodel.Optimize();
//                this.Runtime = grbmodel.Runtime;
//                int status = grbmodel.Status;  //模型状态
//                bool flag3 = status == 2 || status == 9 || status == 11;    //几个状态满足其中一个
//                if (flag3)
//                {
//                    bool flag4 = status == 9;
//                    if (flag4)
//                    {
//                        this.TimeLimitReached = true;
//                    }
//                    bool flag5 = status == 11;
//                    if (flag5)
//                    {
//                        this.Interrupted = true;
//                    }

//                    try
//                    {
//                        this.ObjectiveValue = grbmodel.ObjVal;      //目标
//                        bool logToConsole2 = this.Options.LogToConsole;    //计算结果
//                        if (logToConsole2)
//                        {
//                            this.LowerBounds = logCallback.LowerBounds;
//                            this.UpperBounds = logCallback.UpperBounds;
//                            this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
//                            this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
//                        }
//                        else
//                        {
//                            this.LowerBounds = lightCallback.LowerBounds;
//                            this.UpperBounds = lightCallback.UpperBounds;
//                            this.LowerBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjBound));
//                            this.UpperBounds.Add(new Tuple<double, double>(grbmodel.Runtime, grbmodel.ObjVal));
//                        }
//                        foreach (IMember member in Structure.Members)
//                        {
//                            Bar bar = (Bar)member;
//                            bar.Nx.Clear();
//                            Assignment assignment = new Assignment();
//                            bool flag6 = false;
//                            bool flag7 = !this.Options.CuttingStock;
//                            if (flag7)
//                            {
//                                for (int i = 0; i < Stock.ElementGroups.Count; i++)
//                                {
//                                    bool flag8 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + i].X >= 0.999;
//                                    if (flag8)
//                                    {
//                                        flag6 = true;
//                                        bar.CrossSection = Stock.ElementGroups[i].CrossSection;
//                                        bar.Material = Stock.ElementGroups[i].Material;
//                                        bool flag9 = Stock.ElementGroups[i].Type == ElementType.Reuse;
//                                        if (flag9)
//                                        {
//                                            assignment.AddElementAssignment(Stock.ElementGroups[i], Stock.ElementGroups[i].Next);
//                                        }
//                                        else
//                                        {
//                                            assignment.AddElementAssignment(Stock.ElementGroups[i], 0);
//                                        }
//                                        foreach (LoadCase loadCase2 in LoadCases)
//                                        {
//                                            bar.AddNormalForce(loadCase2, new List<double>
//                                            {
//                                                gurobiMemberForceVariables[loadCase2][bar.Number].X
//                                            });
//                                        }
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                int num = 0;
//                                foreach (ElementGroup elementGroup in stock.ElementGroups)
//                                {
//                                    for (int j = 0; j < elementGroup.NumberOfElements; j++)
//                                    {
//                                        bool flag10 = gurobiAssignmentVariables[bar.Number * Stock.ElementGroups.Count + num + j].X >= 0.999;
//                                        if (flag10)
//                                        {
//                                            flag6 = true;
//                                            bar.CrossSection = elementGroup.CrossSection;
//                                            bar.Material = elementGroup.Material;
//                                            bool flag11 = elementGroup.Type == ElementType.Reuse;
//                                            if (flag11)
//                                            {
//                                                assignment.AddElementAssignment(elementGroup, j);
//                                            }
//                                            else
//                                            {
//                                                assignment.AddElementAssignment(elementGroup, 0);
//                                            }
//                                            foreach (LoadCase loadCase3 in LoadCases)
//                                            {
//                                                bar.AddNormalForce(loadCase3, new List<double>
//                                                {
//                                                    gurobiMemberForceVariables[loadCase3][bar.Number].X
//                                                });
//                                            }
//                                        }
//                                    }
//                                    num += elementGroup.NumberOfElements;
//                                }
//                            }
//                            bool flag12 = !flag6;
//                            if (flag12)
//                            {
//                                assignment.AddElementAssignment(stock.ElementGroups[0], 0);
//                                bar.CrossSection = new EmptySection();
//                                foreach (LoadCase lc in LoadCases)
//                                {
//                                    bar.AddNormalForce(lc, new List<double>
//                                    {
//                                        0.0
//                                    });
//                                }
//                            }
//                            bar.SetAssignment(assignment);
//                        }
//                        Stock.ResetRemainLenghts();
//                        Stock.ResetRemainLenghtsTemp();
//                        Stock.ResetAlreadyCounted();
//                        Stock.ResetNext();
//                        Structure.SetResults(new Result(Structure, stock, LCA));
//                        Structure.SetLCA(LCA);
//                    }
//                    catch (GRBException ex)
//                    {
//                        this.Message = ex.Message;
//                    }
//                }
//                else
//                {
//                    bool flag13 = status == 3;
//                    if (flag13)
//                    {
//                        throw new SystemException("Gurobi problem is infeasible");
//                    }
//                }
//                grbmodel.Dispose();
//                grbenv.Dispose();
//            }
//            catch (GRBException ex2)
//            {
//                throw new GurobiException(ex2.Message);
//            }
//            grbmodel.Dispose();
//            grbenv.Dispose();
//            bool flag14 = !Structure.AllTopologyFixed();
//            if (flag14)
//            {
//                Stock.RemoveElementGroup(0);
//                bool cuttingStock2 = this.Options.CuttingStock;
//                if (cuttingStock2)
//                {
//                    stock.RemoveElementGroup(0);
//                }
//            }
//            Stock.ResetNext();
//        }

//        // Token: 0x060000DF RID: 223 RVA: 0x00006AD8 File Offset: 0x00004CD8
//        private void CloseOpenFormsAndGetPos(out FormStartPosition Pos, out Point Location)
//        {
//            Pos = FormStartPosition.Manual;
//            Location = default(Point);
//            List<Form> list = new List<Form>();
//            foreach (object obj in Application.OpenForms)
//            {
//                Form form = (Form)obj;
//                bool flag = form.Name == this.Options.LogFormName;
//                if (flag)
//                {
//                    list.Add(form);
//                }
//            }
//            foreach (Form form2 in list)
//            {
//                Pos = form2.StartPosition;
//                Location = form2.Location;
//                form2.Close();
//                form2.Dispose();
//            }
//        }

//        // Token: 0x04000077 RID: 119
//        public List<Tuple<double, double>> LowerBounds;

//        // Token: 0x04000078 RID: 120
//        public List<Tuple<double, double>> UpperBounds;

//    }
//}
