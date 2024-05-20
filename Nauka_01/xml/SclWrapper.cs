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
        public SclWrapper(XNamespace ns) : this()
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

            public SclAssignment(Operand leftSide, Operand rightSide)
            {
                _leftSide = leftSide;
                _rightSide.Add(rightSide);
            }

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
            public LiteralConst()
            {
                Value = "0";
            }

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


            public virtual string MemoryArea { get; set; } = "LiteralConstant";

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
            // tu trzeba jakiś dobry konstruktor zrobić żeby był porządek 
            // - jeśli GlobalVar to LocalSection = null
            // - MemoryArea nie może być Literal. Domyślnie LocalVar
            // - MemoryArea powinna być nie do zmiany po deklaracji
            // - StartValue musi być dopasowane do DataType
            // - sprawdzać czy IsRetain jest zgodne z klasą Remanence
            // - sprawdzać Attributes, StartValue, IsRetain, IsSetpoint w zależności od LocalMemory
            // - sprawdzić jakie są zależności miedzy konstruktorem a definicja { } przy deklaracji
            public string DataType { get; set; }  //bool, int, real.....
            public string LocalSection { get; set; } // input, output, static....
            public string Name { get; set; }
            public override string MemoryArea { get; set; } // LocalVariable, GlobalVariable, LiteralConstant
            public MultiLanguageText Comment { get; set; } = null;
            public List<BooleanAttribute> Attributes { get; set; } = null;
            public string StartValue { get; set; } = null;
            public string IsRetain { get; set; } = null;
            public string IsSetPoint { get; set; } = null;
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
      

        public XElement SclGenerateAccess(Operand operand)
        {
            string[] localOperand =
            {
                LocalSection.Input, LocalSection.Output, LocalSection.InOut, LocalSection.Static, LocalSection.Temp, LocalSection.Constant
            };
            //XElement accessElement;

            bool isLocal = localOperand.Contains(operand.LocalSection);
            bool isGlobal = operand.MemoryArea == LocalSection.GlobalVariable;

            if (isLocal || isGlobal)
            {
                //accessElement = new XElement(SclNodes.Access, new XAttribute("Scope", OperandType.LocalVariable), new XAttribute("UId", _uid++.ToString())) ;
                string scope = null;
                if (isLocal || !isGlobal)
                    scope = LocalSection.LocalVariable;
                else if (!isLocal || isGlobal)
                    scope = LocalSection.GlobalVariable;
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
            // tutaj pewnie trzeba dodać sprawdzenie operand is LiteralConstant
            else if (operand.MemoryArea == LocalSection.LiteralConstant)
            {
                XElement accessElement = new XElement(SclNodes.Access,
                    new XAttribute(SclNodes.Scope, LocalSection.LiteralConstant),
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
        public List<XElement> SclGenerateAssignment(Operand leftSide, Operand rightSide)
        {
            return GeneateAssignment(leftSide, new List<ISclSyntax> { rightSide });
        }
        public List<XElement> SclGenerateAssignment(Operand leftSide, List<ISclSyntax> rightSide)
        {
            return GeneateAssignment(leftSide, rightSide );
        }


        private List<XElement> GeneateAssignment(Operand leftSide, List<ISclSyntax> rightSide)
        {
            List<XElement> assignmentElement = new List<XElement>();
            try
            {
                if (leftSide == null || rightSide == null)
                {
                    throw new ArgumentException("Operand for left and right side of assignment can't be null");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            // left side
            assignmentElement.Add(SclGenerateAccess(leftSide));
            // :=
            foreach (var element in SclGenerateAssignSign())
            {
                assignmentElement.Add(element);
            }
            // right side
            foreach (var element in rightSide)
            {
                if (element is Operand)
                {
                    assignmentElement.Add(SclGenerateAccess((Operand)element));
                }
                else if (element is SclToken)
                {
                    assignmentElement.Add(SclGenerateToken((SclToken)element));
                }
                else
                {
                    throw new ArgumentException("so far only Operand and Token");
                }
            }



            return assignmentElement;
        }
        private XElement[] SclGenerateAssignSign()
        {
            return new XElement[]
        {
            new XElement(SclNodes.Blank, new XAttribute(SclNodes.UId, _uid++)),
            new XElement(SclNodes.Token, new XAttribute(SclNodes.Text, Token.Assign), new XAttribute(SclNodes.UId, _uid++)),
            new XElement(SclNodes.Blank, new XAttribute(SclNodes.UId, _uid ++))
        };


        }
        private XElement SclGenerateToken(SclToken token)
        {
            return new XElement(SclNodes.Token, new XAttribute(SclNodes.Text, token.Value), new XAttribute(SclNodes.UId, _uid++));
        }
        private XElement SclGenerateNewLine()
        {
            return new XElement(SclNodes.NewLine, new XAttribute(SclNodes.UId, _uid++));
        }
        private XElement[] SclGenerateEndOfLine()
        {
            return new XElement[] {
                new XElement(SclNodes.Token, new XAttribute(SclNodes.Text, Token.EndOfExpr), new XAttribute(SclNodes.UId, _uid++)),
                SclGenerateNewLine()
                    };
        }
    }

}



