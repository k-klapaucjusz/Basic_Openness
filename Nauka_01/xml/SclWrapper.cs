using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basic_Openness
{
    internal class SclWrapper
    {
        private int _uid;
        public XNamespace ns;

        public SclWrapper()
        {
            ns = null;
        }
        public SclWrapper(XNamespace ns)
        {
            this.ns = ns;
        }

        public class SclAssignment
        {
            private Operand _leftSide;
            private List<ISclSyntax> _rightSide = new List<ISclSyntax>();

            public SclAssignment(Operand leftSide, List<ISclSyntax> rightSide)
            {
                _leftSide = leftSide;
                _rightSide = rightSide;
            }


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
        
        public XElement SclAccess(Operand operand)
        {
            string[] localOperand =
            {
                OperandType.Input, OperandType.Output, OperandType.InOut, OperandType.Static, OperandType.Temp, OperandType.Constant
            };
            XElement accessElement;

            if (localOperand.Contains(operand.MemberType)) {
                accessElement = new XElement(SclNodes.Access, new XAttribute("Scope", OperandType.LocalVariable), new XAttribute("UId", _uid++.ToString())) ;

                        }
            else if(operand.MemberType == OperandType.LiteralConstant)
            {
                // uzupełnić tutaj kod
            }


            return new XElement("hasiok");
        }

    }
}
