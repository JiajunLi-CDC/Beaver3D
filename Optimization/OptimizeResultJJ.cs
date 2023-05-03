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
        public OptimizeResultJJ()
        {
            memberProductionType=new Dictionary<MemberProduceType, List<IMember>>();
        }
    }

    public class MemberProduceType {
        public double Length { get; set; } = 0.0;
        public double crossSectionArea { get; set; } = 0.0;
        public string crossSectionTypeName { get; set; } = "";

        public List<int> memberProduceElement = new List<int>();

        public MemberProduceType(double Length,double crossSectionArea, string crossSectionTypeName)
        {
            this.Length = Length;
            this.crossSectionArea = crossSectionArea;
            this.crossSectionTypeName = crossSectionTypeName;
            this.memberProduceElement = new List<int>();
        }
    }

}
