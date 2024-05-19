using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
            this.ns = null;
            _uid = 21;
        }
        public SclWrapper(XNamespace ns): this() 
        {
            this.ns = ns;
        }
        public SclWrapper(XNamespace ns, int uid) : this(ns)
        {
            this._uid = uid;
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
        public interface ISclSyntax
        {
            //string MemoryType { get; set; }
        }

        public class LiteralConst : ISclSyntax
        {
            private string _value;
            public string Value
            {
                get => _value;
                set
                {
                    if (IsValidNumber(value))
                    {
                        _value = value;
                    }
                    else
                    {
                        throw new ArgumentException("Value must be a valid number.");
                    }
                }
            }
            public LiteralConst() { }

            public LiteralConst(string value)
            {
                Value = value;
            }
            public LiteralConst(int value)
            {
                Value = value.ToString();
            }
            public LiteralConst(double value)
            {
                Value = value.ToString();
            }


            public string MemoryType { get; } = "LiteralConstant";

            private bool IsValidNumber(string value)
            {
                // Sprawdzenie czy string można skonwertować do liczby całkowitej
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                {
                    return true;
                }

                // Sprawdzenie czy string można skonwertować do liczby rzeczywistej
                return double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _);
            }



        }

        public class Operand : LiteralConst
        {
            // mniej więcej taka zawartość, punkt wyjścia
            public string Type { get; set; }
            public string Name { get; set; }
            public new string MemoryType { get; set; }
            public MultiLanguageText Comment { get; set; }
            public List<BooleanAttribute> Attributes { get; set; }
            public string StartValue { get; set; }
            public string IsRetain { get; set; }
            public string IsSetPoint { get; set; }
        }


        public class SclToken : ISclSyntax
        {
            private string _value;
            public string Value
            {
                get => _value;
                set
                {
                    if (IsValidToken(value))
                    {
                        _value = value;
                    }
                    else
                    {
                        throw new ArgumentException("Value must be a valid token.");
                    }
                }
            }
            public bool IsValidToken(string token)
            {
                // Get all fields from the Token class
                var fields = typeof(Token).GetFields(BindingFlags.Public | BindingFlags.Static);

                // Check if any field value equals the checkString
                return fields.Any(field => (string)field.GetValue(null) == token);
            }
        }
        //public class SclOperand : ISclSyntax
        //{
        //    public string Value { get; set; }
        //}



        public XElement SclGenerateAccess(Operand operand)
        {
            string[] localOperand =
            {
                OperandType.Input, OperandType.Output, OperandType.InOut, OperandType.Static, OperandType.Temp, OperandType.Constant
            };
            //XElement accessElement;

            bool isLocal = localOperand.Contains(operand.Type);
            bool isGlobal = operand.MemoryType == OperandType.GlobalVariable;

            if (isLocal || isGlobal)
            {
                //accessElement = new XElement(SclNodes.Access, new XAttribute("Scope", OperandType.LocalVariable), new XAttribute("UId", _uid++.ToString())) ;
                string scope = null;
                if (isLocal || !isGlobal)
                    scope = OperandType.LocalVariable;
                else if (!isLocal || isGlobal)
                    scope = OperandType.GlobalVariable;
                else Console.WriteLine("ERROR in SclGenerateAccess(): global / local missmatch !!!");


                string[] parts = operand.Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                XElement accessElement = new XElement("Access",
                    new XAttribute(SclNodes.Scope, scope),
                    new XAttribute(SclNodes.UId, _uid++)
                );

                XElement symbolElement = new XElement(SclNodes.Symbol,
                    new XAttribute(SclNodes.UId, _uid++)
                );

                SclGenerateComponents(symbolElement, parts, ref _uid);

                accessElement.Add(symbolElement);
                return accessElement;

            }
            else if (operand.MemoryType == OperandType.LiteralConstant)
            {
                XElement accessElement = new XElement(SclNodes.Access,
                    new XAttribute(SclNodes.Scope, OperandType.LiteralConstant),
                    new XAttribute(SclNodes.UId, _uid++));

                XElement constantElement = new XElement(SclNodes.Constant,
                    new XAttribute(SclNodes.UId, _uid++));

                XElement constantValueElement = new XElement(SclNodes.ConstantValue,
                    new XAttribute(SclNodes.UId, _uid++), operand.Value);

                constantElement.Add(constantValueElement);
                accessElement.Add(constantElement);
                return accessElement;
            }
            else
            {
                Console.WriteLine("ERROR in SclGenerateAccess(): no global or local or literal constant");
                return null;
            }

        }

        private void SclGenerateComponents(XElement symbolElement, string[] parts, ref int uid)
        {
            foreach (string part in parts)
            {
                symbolElement.Add(new XElement(SclNodes.Component,
                    new XAttribute(SclNodes.Name, part),
                    new XAttribute(SclNodes.UId, uid++)
                ));

                if (part != parts.Last())
                {
                    symbolElement.Add(new XElement(SclNodes.Token,
                        new XAttribute(SclNodes.Text, Token.Dot),
                        new XAttribute(SclNodes.UId, uid++)
                    ));
                }
            }
        }

    }
}
