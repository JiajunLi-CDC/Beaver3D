using System;
using System.Collections.Generic;
using System.Linq;
using Beaver3D.LCA;
using Beaver3D.Optimization;

namespace Beaver3D.Model
{
    // Token: 0x0200002C RID: 44
    public class Structure
    {
        // 所有的杆件
        public List<IMember> Members { get; private set; } = new List<IMember>();
        public List<Bar> original_Members { get; private set; } = new List<Bar>();

        // 所有的节点
        public List<Node> Nodes { get; private set; } = new List<Node>();

        //多个结构的数量，默认为1
        public int merge_structure_num = 1;

        //每个杆件最终的生产长度（按每个聚类最小的长度生产）
        public List<double> member_expectLenth = new List<double>();

        // 荷载
        public List<LoadCase> LoadCases { get; private set; } = new List<LoadCase>();
        public List<LoadCase> original_LoadCases { get; private set; } = new List<LoadCase>();

        // 支座
        public List<Support> Supports { get; private set; } = new List<Support>();
        public List<Support> original_Supports { get; private set; } = new List<Support>();

        // 固定自由度
        internal bool[] FixedDofs { get; private set; } = new bool[0];

        // 
        public int NFixedTranslations { get; private set; }

        // 
        public int NFixedRotations { get; private set; }

        // 
        public int NFreeTranslations { get; private set; }

        // 
        public int NFreeRotations { get; private set; }

        // 杆件排序
        public SortStructureMembersBy SortBy { get; private set; } = SortStructureMembersBy.Off;

        //
        public List<int> SortMap { get; private set; } = new List<int>();

        // 
        public Result Results { get; private set; }

        // 
        public ILCA LCA { get; private set; } = new GHGFrontiers();

        // 杆件编组？
        public Dictionary<int, List<IMember>> MemberGroups { get; private set; } = new Dictionary<int, List<IMember>>();

        // 添加杆件，并且赋予杆件的编号
        public void AddMember(IMember M)
        {
            //Bar barcopy = (Bar)M;
            //original_Members.Add(barcopy.CloneBar());  //复制一份

            bool flag = !this.Members.Contains(M);    //是否不包含该杆件
            if (flag)
            {
                M.SetNumber(this.Members.Count);   //杆件的编号为当前杆件的序号
                this.Members.Add(M);  //添加进结构的杆件列表

                bool flag2 = this.MemberGroups.ContainsKey(M.GroupNumber) && this.MemberGroups[M.GroupNumber] != null;     //获取每根杆件的结构编号，这里之前默认为-1
                if (flag2)
                {
                    this.MemberGroups[M.GroupNumber].Add(M);
                }
                else
                {
                    this.MemberGroups.Add(M.GroupNumber, new List<IMember>());      //根据编号创建杆件群组，但这里默认都是-1，应该之后所有的杆件都在同一个组里？
                    this.MemberGroups[M.GroupNumber].Add(M);
                }

                Bar bar = M as Bar;

                if (bar == null)    //这一步是判断是否转换成功，bar==null则转换失败
                {
                    Beam beam = M as Beam;  //M转换为beam
                    if (beam != null)
                    {                   
                        this.AssembleIMember1D(beam);
                        foreach (LoadCase lc in beam.Nx.Keys)      //这里暂时还是null？？
                        {
                            this.AddLoadcase(lc);
                        }
                    }
                }
                else
                {                 
                    this.AssembleIMember1D(bar);
                    foreach (LoadCase lc2 in bar.Nx.Keys)
                    {
                        this.AddLoadcase(lc2);
                    }
                }


                this.SetResults(new Result(this, this.LCA));    //新建结构的结果数据，
                return;
            }

            throw new ArgumentException("This member already exists in the structure!");
        }

        // 设置杆件的连接属性，这里会对structure构造中添加nodes的数据
        internal void AssembleIMember1D(IMember1D Member)
        {
            bool flag = !this.Nodes.Contains(Member.From);  //如果节点中不包含杆件的出发点
            if (flag)
            {
                Node from = Member.From;
                from.Number = this.Nodes.Count;
                Member.From = from;
                Member.From.AddConnectedMember(Member, -1.0);
                this.Nodes.Add(Member.From);             //新建节点并且添加进结构的nodes中，并且设置该nodes的连接杆件，这里对出发点相接的杆件的double索引默认为-1
            }
            else
            {
                Member.From = this.Nodes.Find((Node N) => N.Equals(Member.From));    //lambda表达式，表示一个匿名函数，=>前面的是参数，后面的是函数体。这里是说从nodes列表中找到等于杆件from位置的
                Member.From.AddConnectedMember(Member, -1.0);
            }


            bool flag2 = !this.Nodes.Contains(Member.To);   //如果节点中不包含杆件的结束点
            if (flag2)
            {
                Node to = Member.To;
                to.Number = this.Nodes.Count;
                Member.To = to;
                Member.To.AddConnectedMember(Member, 1.0);     //新建节点并且添加进结构的nodes中，并且设置该nodes的连接杆件，这里对出发点相接的杆件的double索引默认为1
                this.Nodes.Add(Member.To);
            }
            else
            {
                Member.To = this.Nodes.Find((Node N) => N.Equals(Member.To));
                Member.To.AddConnectedMember(Member, 1.0);
            }
        }

        // Token: 0x060002BE RID: 702 RVA: 0x000127F8 File Offset: 0x000109F8
        private void SetDofs()
        {
            this.FixedDofs = new bool[this.Nodes.Count * 6];
            this.NFixedTranslations = 0;
            this.NFixedRotations = 0;
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                this.NFixedTranslations += (this.Nodes[i].FixTx ? 1 : 0) + (this.Nodes[i].FixTy ? 1 : 0) + (this.Nodes[i].FixTz ? 1 : 0);
                this.NFixedRotations += (this.Nodes[i].FixRx ? 1 : 0) + (this.Nodes[i].FixRy ? 1 : 0) + (this.Nodes[i].FixRz ? 1 : 0);
                this.FixedDofs[this.Nodes[i].Number * 6] = this.Nodes[i].FixTx;
                this.FixedDofs[this.Nodes[i].Number * 6 + 1] = this.Nodes[i].FixTy;
                this.FixedDofs[this.Nodes[i].Number * 6 + 2] = this.Nodes[i].FixTz;
                this.FixedDofs[this.Nodes[i].Number * 6 + 3] = this.Nodes[i].FixRx;
                this.FixedDofs[this.Nodes[i].Number * 6 + 4] = this.Nodes[i].FixRy;
                this.FixedDofs[this.Nodes[i].Number * 6 + 5] = this.Nodes[i].FixRz;
                this.Nodes[i].ResetReducedDofs();
                bool flag = !this.Nodes[i].FixTx;
                if (flag)
                {
                    this.Nodes[i].ReducedDofs[0] = num;
                    this.Nodes[i].ReducedDofsTruss[0] = num2;
                    num++;
                    num2++;
                }
                bool flag2 = !this.Nodes[i].FixTy;
                if (flag2)
                {
                    this.Nodes[i].ReducedDofs[1] = num;
                    this.Nodes[i].ReducedDofsTruss[1] = num2;
                    num++;
                    num2++;
                }
                bool flag3 = !this.Nodes[i].FixTz;
                if (flag3)
                {
                    this.Nodes[i].ReducedDofs[2] = num;
                    this.Nodes[i].ReducedDofsTruss[2] = num2;
                    num++;
                    num2++;
                }
                bool flag4 = !this.Nodes[i].FixRx;
                if (flag4)
                {
                    this.Nodes[i].ReducedDofs[3] = num;
                    num++;
                }
                bool flag5 = !this.Nodes[i].FixRy;
                if (flag5)
                {
                    this.Nodes[i].ReducedDofs[4] = num;
                    num++;
                }
                bool flag6 = !this.Nodes[i].FixRz;
                if (flag6)
                {
                    this.Nodes[i].ReducedDofs[5] = num;
                    num++;
                }
            }
            this.NFreeTranslations = this.Nodes.Count * 3 - this.NFixedTranslations;
            this.NFreeRotations = this.Nodes.Count * 3 - this.NFixedRotations;
        }

        // 施加荷载
        public void AddLoadcase(LoadCase LC)
        {
            this.original_LoadCases.Add(LC.Clone());   //复制一份

            bool flag = this.LoadCases.Contains(LC);   //判断是否已经包含荷载
            if (flag)
            {
                this.LoadCases[this.LoadCases.IndexOf(LC)].Loads.AddRange(LC.Loads);     //在之前荷载的名字序列后面继续添加
                this.LoadCases[this.LoadCases.IndexOf(LC)].DisplacementBounds.AddRange(LC.DisplacementBounds);
                this.AssemblePointLoads();
                this.AssembleDisplacementBounds();
            }
            else
            {
                LC.Number = this.LoadCases.Count;
                this.LoadCases.Add(LC);    //分配并添加荷载            
                this.AssemblePointLoads();
                this.AssembleDisplacementBounds();
            }
        }

        // 施加点荷载
        private void AssemblePointLoads()
        {
            foreach (Node node in this.Nodes)
            {
                node.RemoveAllLoads();
            }
            foreach (LoadCase loadCase in this.LoadCases)
            {
                using (List<ILoad>.Enumerator enumerator3 = loadCase.Loads.GetEnumerator())  //枚举的写法，这里枚举了loadcase中的每一个荷载
                {
                    while (enumerator3.MoveNext())  //相当于获取了loads中的所有元素？
                    {
                        PointLoad PL = (PointLoad)enumerator3.Current;
                        Node node2 = this.Nodes.Find((Node N) => N.Equals(PL.Node));    //获取点荷载所在的点对应的结构nodes列表中的node

                        PL.Node = node2;    //将列表中的node重新赋予点荷载
                        node2.AddPointLoad(loadCase, PL);
                    }
                }
            }
        }

        // 施加位移边界
        private void AssembleDisplacementBounds()
        {
            foreach (Node node in this.Nodes)
            {
                node.RemoveAllDisplacementBounds();
            }
            foreach (LoadCase loadCase in this.LoadCases)
            {
                using (List<DisplacementBound>.Enumerator enumerator3 = loadCase.DisplacementBounds.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        DisplacementBound DB = enumerator3.Current;
                        Node node2 = this.Nodes.Find((Node N) => N.Equals(DB.Node));

                        DB.Node = node2;        //将列表中的node重新赋予位移边界
                        node2.AddDisplacementBound(loadCase, DB);
                    }
                }
            }
        }

        // 施加支座约束
        public void AddSupport(Support S)
        {
            bool flag = this.Supports.Contains(S);
            if (flag)
            {
                this.Supports.Remove(S);
                this.Supports.Add(S);
            }
            else
            {
                this.Supports.Add(S);     //将所有的支座添加进structure
            }
            this.AssembleSupports();
        }

        // 施加支座约束
        private void AssembleSupports()
        {
            foreach (Node node in this.Nodes)
            {
                node.FreeAllSupports();
            }
            using (List<Support>.Enumerator enumerator2 = this.Supports.GetEnumerator())  //枚举structure中所有的支座list
            {
                while (enumerator2.MoveNext())
                {
                    Support S = enumerator2.Current;
                    Node node2 = this.Nodes.Find((Node N) => N.Equals(S.Node));

                    S.Node = node2;
                    node2.SetSupport(S);    //将node的support设置关联
                }
            }
            this.SetDofs();
        }

        // 设置结果
        public void SetResults(Result Results)
        {
            this.Results = Results;
        }

        // 设置LCA
        public void SetLCA(ILCA LCA)
        {
            this.LCA = LCA;
        }

        public void addMultyStructureInformation(List<Structure> multyStructures)   //根据单个结构保存的原始数据进行多个结构的初始化
        {
            List<IMember> list = new List<IMember>();
            List<LoadCase> list2 = new List<LoadCase>();
            List<Support> list3 = new List<Support>();

            //add member
            for (int i = 0; i < multyStructures.Count; i++)
            {
                for (int j = 0; j < multyStructures[i].original_Members.Count; j++)
                {
                    IMember M = multyStructures[i].original_Members[j];

                    list.Add(M);
                }
            }

            foreach (IMember m in list)
            {
                this.AddMember(m);
            }

            //add Loadcase
            for (int i = 0; i < multyStructures.Count; i++)
            {
                for (int j = 0; j < multyStructures[i].original_LoadCases.Count; j++)
                {
                    LoadCase LC = multyStructures[i].original_LoadCases[j];
                    list2.Add(LC);
                }
            }
            foreach (LoadCase lc in list2)
            {
                this.AddLoadcase(lc);
            }

            //add support
            for (int i = 0; i < multyStructures.Count; i++)
            {
                for (int j = 0; j < multyStructures[i].original_Supports.Count; j++)
                {
                    Support S = multyStructures[i].original_Supports[j];
                    list3.Add(S);
                }
            }
            foreach (Support s in list3)
            {
                this.AddSupport(s);
            }
        }

        public Structure()
        {

        }


        // 判断是否所有的杆件都拓扑固定
        public bool AllTopologyFixed()
        {
            foreach (IMember member in this.Members)
            {
                bool flag = !member.TopologyFixed;
                if (flag)
                {
                    return false;
                }
            }
            return true;
        }

        // 根据名称获取荷载信息
        public List<LoadCase> GetLoadCasesFromNames(List<string> LoadCaseNames)
        {
            bool flag = LoadCaseNames.Count == 0;
            if (flag)    //当前没有荷载信息
            {
                throw new ArgumentException("The list of LoadCase Names is empty");
            }
            bool flag2 = LoadCaseNames[0] == "all";  //获取所有的荷载

            List<LoadCase> result;
            if (flag2)
            {
                result = this.LoadCases;
            }
            else
            {
                List<LoadCase> list = this.LoadCases.FindAll((LoadCase x) => LoadCaseNames.Contains(x.Name));   //找出输入荷载名称相同的结构荷载并且加入列表
                bool flag3 = list.Count == 0;  //如果找不到任何一个以x命名的荷载信息
                if (flag3)
                {
                    throw new ArgumentException("No loadcases with the provided names could be found in the structure!");
                }
                result = list;
            }
            return result;
        }

        // Token: 0x060002C8 RID: 712 RVA: 0x0001318C File Offset: 0x0001138C
        public void SortMembers1D(SortStructureMembersBy SortBy)
        {
            switch (SortBy)
            {
                case SortStructureMembersBy.ForceThenLength:
                    {
                        var source = (from pair in this.Members.OfType<IMember1D>().Zip(Enumerable.Range(0, this.Members.OfType<IMember1D>().Count<IMember1D>()), (IMember1D x, int y) => new
                        {
                            x,
                            y
                        })
                                      orderby Math.Abs((from x in pair.x.Nx.Values
                                                        select x.Min()).Min()) descending, Math.Abs((from x in pair.x.Nx.Values
                                                                                                     select x.Min()).Max()) descending, pair.x.Length descending
                                      select pair).ToList();
                        this.Members.RemoveAll((IMember m) => m is IMember1D);
                        this.Members.AddRange((from pair in source
                                               select pair.x).ToList<IMember1D>());
                        this.SortMap = (from pair in source
                                        select pair.y).ToList<int>();
                        break;
                    }
                case SortStructureMembersBy.LengthThenForce:
                    {
                        var source2 = (from pair in this.Members.OfType<IMember1D>().Zip(Enumerable.Range(0, this.Members.OfType<IMember1D>().Count<IMember1D>()), (IMember1D x, int y) => new
                        {
                            x,
                            y
                        })
                                       orderby pair.x.Length descending, Math.Abs((from x in pair.x.Nx.Values
                                                                                   select x.Min()).Min()) descending, Math.Abs((from x in pair.x.Nx.Values
                                                                                                                                select x.Max()).Max()) descending
                                       select pair).ToList();
                        this.Members.RemoveAll((IMember m) => m is IMember1D);
                        this.Members.AddRange((from pair in source2
                                               select pair.x).ToList<IMember1D>());
                        this.SortMap = (from pair in source2
                                        select pair.y).ToList<int>();
                        break;
                    }
            }
        }

        // Token: 0x060002C9 RID: 713 RVA: 0x00013458 File Offset: 0x00011658
        public Structure Clone()
        {
            Structure structure = new Structure();
            structure.Members = new List<IMember>();
            structure.Nodes = new List<Node>();
            structure.LoadCases = new List<LoadCase>();
            structure.Supports = new List<Support>();
            structure.SortMap = new List<int>();
            foreach (IMember member in this.Members)
            {
                IMember1D member1D = (IMember1D)member;
                structure.Members.Add(member1D.Clone());
            }         
            foreach (Node item in this.Nodes)
            {
                structure.Nodes.Add(item);
            }
            foreach (LoadCase loadCase in this.LoadCases)
            {
                structure.LoadCases.Add(loadCase.Clone());
            }
            foreach (Support support in this.Supports)
            {
                structure.Supports.Add(support.Clone());
            }
            structure.FixedDofs = (bool[])this.FixedDofs.Clone();
            structure.NFixedTranslations = this.NFixedTranslations;
            structure.NFixedRotations = this.NFixedRotations;
            structure.NFreeTranslations = this.NFreeTranslations;
            structure.NFreeRotations = this.NFreeRotations;
            structure.SortBy = this.SortBy;
            structure.merge_structure_num = this.merge_structure_num;
            foreach (int item2 in this.SortMap)
            {
                structure.SortMap.Add(item2);
            }
            structure.Results = this.Results.Clone();
            structure.LCA = this.LCA.Clone();
            return structure;
        }

        public Structure Clone2()   //多结构用
        {
            Structure structure = new Structure();
            structure.Members = new List<IMember>();
            structure.Nodes = new List<Node>();
            structure.LoadCases = new List<LoadCase>();
            structure.Supports = new List<Support>();
            structure.SortMap = new List<int>();     
            foreach (IMember member in this.Members)
            {
                IMember1D member1D = (IMember1D)member;
                structure.Members.Add(member1D.Clone());
            }
            foreach (Bar bar in this.original_Members)
            {
                structure.original_Members.Add(bar.CloneBar());
            }
            foreach (Node item in this.Nodes)
            {
                structure.Nodes.Add(item);
            }
            foreach (LoadCase loadCase in this.LoadCases)
            {
                structure.LoadCases.Add(loadCase.Clone());
            }
            foreach (LoadCase loadCase in this.original_LoadCases)
            {
                structure.original_LoadCases.Add(loadCase.Clone());
            }
            foreach (Support support in this.Supports)
            {
                structure.Supports.Add(support.Clone());
            }
            foreach (Support support in this.original_Supports)
            {
                structure.original_Supports.Add(support.Clone());
            }
            structure.FixedDofs = (bool[])this.FixedDofs.Clone();
            structure.NFixedTranslations = this.NFixedTranslations;
            structure.NFixedRotations = this.NFixedRotations;
            structure.NFreeTranslations = this.NFreeTranslations;
            structure.NFreeRotations = this.NFreeRotations;
            structure.merge_structure_num = this.merge_structure_num;
            structure.SortBy = this.SortBy;
            foreach (int item2 in this.SortMap)
            {
                structure.SortMap.Add(item2);
            }
            //structure.Results = this.Results.Clone();
            structure.LCA = this.LCA.Clone();
            return structure;
        }
    }
}
