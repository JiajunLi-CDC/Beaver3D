using Beaver3D.Model;
using Beaver3D.Reuse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver3D.Optimization
{
    public class OptimizeResultJJ
    {
        public Dictionary<MemberProduceType, List<IMember>> memberProductionType { get; set; } = new Dictionary<MemberProduceType, List<IMember>>();  //记录每个长度，每个截面的生产杆件
        public Dictionary<MemberProduceType, List<IMember>> memberToUseType { get; set; } = new Dictionary<MemberProduceType, List<IMember>>();  //记录每个长度，每个截面的所用杆件（每个杆件的截面记录）

        //生产结果
        public List<string> ProductionResults { get; set; } = new List<string>();

        //库存使用结果
        public List<string> StockUseResults { get; set; } = new List<string>();

        //每个杆件的截面输出
        public List<double> EachMember_section { get; set; } = new List<double>();

        public OptimizeResultJJ()
        {
            memberProductionType = new Dictionary<MemberProduceType, List<IMember>>();
            memberToUseType = new Dictionary<MemberProduceType, List<IMember>>();
        }
    }

    public class MemberProduceType {
        public double Length { get; set; } = 0.0;
        public double crossSectionArea { get; set; } = 0.0;
        public string crossSectionTypeName { get; set; } = "";

        public List<int> memberProduceElement = new List<int>();    //所使用的截面索引

        public MemberProduceType(double Length,double crossSectionArea, string crossSectionTypeName)
        {
            this.Length = Length;
            this.crossSectionArea = crossSectionArea;
            this.crossSectionTypeName = crossSectionTypeName;
            this.memberProduceElement = new List<int>();
        }
    }

}
