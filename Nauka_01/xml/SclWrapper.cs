using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    internal class SclWrapper
    {
        public class SclAssignment
        {

            public SclAssignment(Operand leftSide, List<ISclSyntax> rightSide) { }

        }
        public interface ISclSyntax { }

        public class LiteralCost : ISclSyntax
        {
            public string Value { get; set; }
            public const string DataType = "LiteralConstant";
        }

        public class Operand : ISclSyntax
        {
            // mniej więcej taka zawartość, punkt wyjścia
            public string MemberType { get; set; }
            public string Name { get; set; }
            public string DataType { get; set; }
            public MultiLanguageText Comment { get; set; }
            public List<BooleanAttribute> Attributes { get; set; }
            public string StartValue { get; set; }
            public string IsRetain { get; set; }
            public string IsSetPoint { get; set; }
        }


        public class SclToken : ISclSyntax
        {
            public string Value { get; set; }
        }
        //public class SclOperand : ISclSyntax
        //{
        //    public string Value { get; set; }
        //}

    }
}
