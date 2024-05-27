using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basic_Openness
{
    internal class SclWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _uid;
        private XNamespace _ns;

        public SclWrapper()
        {
            this._ns = null;
            _uid = 21;
        }
        public SclWrapper(XNamespace ns) : this()
        {
            this._ns = ns;
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

        public XNamespace Ns { get => _ns; }
        public int UId
        {
            get => _uid;
            set
            {
                if (_uid <= value)
                {
                    _uid = value;
                    NotifyPropertyChanged(nameof(UId));
                }
                else if (_uid == value)
                {
                    Console.WriteLine($"Warning: UId = _uid {value}");  //rewise idea with UId and checking Uid == _uid
                }
                else
                {
                    Console.WriteLine($"Error: Wrong UId {value}"); //exception temporary blocked to figure out what is the poblem
                    //throw new ArgumentException("Error: you tries set value then the current value to the UId. Naughty, naughty!");
                }
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
            // - jeśli GlobalVariable to IntefaceSectionn = null
            // - MemoryArea nie może być LiteralConstant. Domyślnie LocalVariable
            // - MemoryArea powinna być niezmienna po deklaracji
            // - StartValue musi być dopasowane do DataType
            // - sprawdzać czy IsRetain jest zgodne z klasą Remanence
            // - sprawdzać Attributes, StartValue, IsRetain, IsSetpoint w zależności od LocalMemory
            //      - Constatnt: Attributies, IsRetain, IsSetPoint = null
            //      - Temp: Attributies, StartValue, IsRetain, IsSetPoint = null
            //      - Input: IsSetPoint = null
            //      - Output: IsSetPoint = null
            //      - InOut: IsSetPoint = null
            //      - Static: all properties allowed
            // - Value = null (dziedziczone z LiteralConst)
            // - sprawdzić jakie są zależności miedzy konstruktorem a definicja { } przy deklaracji
            ////public string DataType { get; set; }  //bool, int, real.....
            ////public string IntefaceSectionn { get; set; } // input, output, static....
            ////public string Name { get; set; }
            ////public override string MemoryArea { get; set; } // LocalVariable, GlobalVariable, LiteralConstant
            ////public MultiLanguageText Comment { get; set; } = null;
            ////public List<BooleanAttribute> Attributes { get; set; } = null;
            ////public string StartValue { get; set; } = null;
            ////public string IsRetain { get; set; } = null;
            ////public string IsSetPoint { get; set; } = null;
            ///
            /// <summary>
            /// 1) assign MemoryArea - its the most important property!!!
            /// <summary"/>
            public Operand()    // default MemoryArea is LocalVariable
            {
                _memoryArea = MemoryAreas.LocalVariable;
            }
            public Operand(string memoryArea, string dataType, string name)
            {
                if (memoryArea == MemoryAreas.LiteralConstant)
                {
                    throw new ArgumentException("MemoryArea cannot be LiteralConstant for Operand.");
                }

                _memoryArea = memoryArea;
                if (memoryArea == MemoryAreas.GlobalVariable)
                {
                    //InterfaceSection = null; // this properties don't describe GlobalVariable
                    // version below is probably better because it skips setter of InterfaceSection
                    _interfaceSection = null; // this properties don't describe GlobalVariable
                }

                DataType = dataType;
                Name = name;
            }

            /// <summary>
            /// 2) assign InterfaceSection. It's second most important property (this property defines
            ///     properties like:StartValue, IsRetain, IsSetPoint)
            /// </summary>
            private string _interfaceSection;
            public string InterfaceSection
            {
                get => _interfaceSection;
                set
                {
                    if (_memoryArea == MemoryAreas.LocalVariable)
                    {
                        if (IsValidInterfaceSection(value))
                        {
                            _interfaceSection = value;
                            ValidateProperties();
                        }
                        else
                        {
                            throw new ArgumentException("Invalid InterfaceSection value.");
                        }
                    }
                    else if (_memoryArea == MemoryAreas.GlobalVariable)
                    {
                        _interfaceSection = null;
                        throw new InvalidOperationException("Setting InterfaceSection is allowed only for LocalVariable");
                    }
                    else
                    {
                        throw new InvalidOperationException("Setting InterfaceSection is allowed only for LocalVariable");
                    }
                }
            }
            /// <summary>
            /// 3) assign DataType. So far this property dosn't have any restrictions and dependencies with other properties
            /// </summary>
            private string _dataType;
            public string DataType
            {
                get => _dataType;
                set
                {
                    if (value == null || value == "")
                    {
                        throw new ArgumentException("DataType cannot be null or empty.");
                    }
                    _dataType = value;
                }
            }  // bool, int, real.....
            /// <summary>
            /// 4) Name is string without whitespaces. Whitespaces are converted to _
            /// </summary>

            private string _name;
            public string Name
            {
                get => _name;
                set
                {
                    if (value == null || value == "")
                    {
                        throw new ArgumentException("Name cannot be null or empty.");
                    }
                    _name = value.Replace(" ", "_");
                    // add more rules for Name later
                }
            }

            private readonly string _memoryArea;

            /// <summary>
            /// 5) MemoryArae cannot be changed after initialization
            /// </summary>
            public override string MemoryArea
            {
                get => _memoryArea;
                set => throw new InvalidOperationException("MemoryArea cannot be changed after initialization.");
            }

            public MultiLanguageText Comment { get; set; } = null;
            public List<BooleanAttribute> Attributes { get; set; } = null;
            //private string[] _startValueAllowed = { InterfaceSections.Input, InterfaceSections.Output, InterfaceSections.InOut, InterfaceSections.Static, InterfaceSections.Constant };
            /// <summary>
            /// 6) StartValue are only allowed for LocalVariable interfaceSection: Input, Output, InOut, Static, Constant
            ///      for basic DataTypes.BasicDataTypes!!!
            /// </summary>
            private string _startValue;
            public string StartValue
            {
                get => _startValue;
                set
                {
                    if (ValidateStartValue(value))
                    {
                        _startValue = value;
                    }

                }
            }
            public string IsRetain { get; set; } = null;
            public string IsSetPoint { get; set; } = null;

            public bool ValidateStartValue(string startValue)
            {
                string[] _startValueAllowed = { InterfaceSections.Input, InterfaceSections.Output, InterfaceSections.InOut
                        , InterfaceSections.Static, InterfaceSections.Constant };

                if (DataType == null)
                {

                    throw new ArgumentNullException("DataType cannot be null when assinging StartValue");

                }

                else if (_startValueAllowed.Contains(InterfaceSection) && DataTypes.BasicDataTypes.ContainsKey(DataType.ToUpper()))
                {
                    if (XmlGeneral.IsConvertibleToType(DataType, startValue))
                    {
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException($"StartValue: {startValue} is not convertible to DataType: {DataType}");
                    }
                }
                else
                {
                    throw new ArgumentException($"For this InterfaceSection: {InterfaceSection} " +
                        $"or DataType: {DataType} StartValue is not allowed");
                }
            }




            private void ValidateProperties()
            {
                if (_interfaceSection == InterfaceSections.Constant)
                {
                    if (Attributes != null) Attributes = null;
                    if (IsRetain != null) IsRetain = null;
                    if (IsSetPoint != null) IsSetPoint = null;
                }
                else if (_interfaceSection == InterfaceSections.Temp)
                {
                    if (Attributes != null) Attributes = null;
                    if (StartValue != null) StartValue = null;
                    if (IsRetain != null) IsRetain = null;
                    if (IsSetPoint != null) IsSetPoint = null;
                }
                else if (_interfaceSection == InterfaceSections.Input ||
                    _interfaceSection == InterfaceSections.Output || _interfaceSection == InterfaceSections.InOut)
                {
                    if (IsSetPoint != null) IsSetPoint = null;
                }
            }
            private bool IsValidInterfaceSection(string value)
            {
                var interfaceSections = typeof(InterfaceSections).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                    .Select(fi => (string)fi.GetRawConstantValue());

                return interfaceSections.Contains(value);
            }
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

        //klasa FBInterface
        //
        public class FBInterface
        {
            public List<Operand> Input { get; set; } = new List<Operand>();
            public List<Operand> Output { get; set; } = new List<Operand>();
            public List<Operand> InOut { get; set; } = new List<Operand>();

        }
        /////////////////////////////////////////////////////
        /// ODCZYT INTERFACE BLOKU FB
        // Metoda wywołująca blok FB w SCL musi wiedzieć jak wyglada interface bloku FB.
        // Żeby się tego dowiedzieć trzeba blok o takiej nazwie znaleźć w projekcie, następnie:
        //  - wyeksportować do XML
        //  - odcztać strukturę inteface z pliku XML do zmiennej klasy FBInterface 
        //    (oraz zapisać ten interfejs do słownika zmierające interfejsy bloków FB na których
        //    aplikacja aktualnie pracuje Dictionary currentFBInterfaces<string, FBInterface>)

        /// <summary>
        /// funkcja zwraca interface bloku FB. 
        /// Przeszukuje miejsca w których takie informację mogą być przechowywane
        ///   1) w słowniku currentFBInterfaces
        ///   2) w folderze na dysku szuka pliku xml z nazwą FB. Jeśli nie będzie podana ścieżka do 
        ///       pliku to otwiera się okna dialogowe wyboru folderu.
        ///   3) szuka w projekcie bloku FB i exportuje go do pliku xml. Z pliku xml odczytany zostaje
        ///      interfejs bloku FB.
        ///   4) jeśli zostanie odnaleziony interface bloku FB to medoda zwraca null
        /// 
        /// </summary>
        /// <param string name="fbName" Dcitionary FBInterface="currentFBInterface"
        /// folder="pathToExportedFBs"></param>
        /// <returns> interface lub null jeśli nie zostanie odnaleziony blok FB </returns>
        /// 

        public FBInterface GetFBInterface(string fbName, Dictionary<string, FBInterface> currentFBInterfaces, string folder = null)
        {
            FBInterface fbInterface = new FBInterface();
            if (currentFBInterfaces.ContainsKey(fbName))
            {
                return  currentFBInterfaces[fbName];
            }
            else
            {
                // sprawdzenie czy poszukiwany FB został już wcześniej wyeksportowany
                
                
                {
                    // otwarcie okna dialogowego do wyboru folderu
                    // folder = OpenFolderDialog();
                }
                // sprawdzenie czy w folderze jest plik xml z interfejsem bloku FB
                // jeśli nie ma to szukanie bloku FB w projekcie i eksportowanie go do pliku xml
                // odczytanie interfejsu bloku FB z pliku xml
            }
            return fbInterface;
        }

        public bool FindFile(string fbName, string folder)
        {
            // TODO
            return true;
        }
        
        public FBInterface GetFBInterfaceFromXML(string fbName)
        {
            // TODO
            return null;
        }
        public void ExportBlockToXML(string blockName, string blockType)
        {
            // TODO
        }




        public XElement SclGenerateAccess(Operand operand)
        {
            string[] localOperand =
            {
                InterfaceSections.Input, InterfaceSections.Output, InterfaceSections.InOut, InterfaceSections.Static, InterfaceSections.Temp, InterfaceSections.Constant
            };
            //XElement accessElement;

            bool isLocal = localOperand.Contains(operand.InterfaceSection);
            bool isGlobal = operand.MemoryArea == MemoryAreas.GlobalVariable;

            if (isLocal || isGlobal)
            {
                //accessElement = new XElement(SclNodes.Access, new XAttribute("Scope", OperandType.LocalVariable), new XAttribute("UId", ++_uid.ToString())) ;
                string scope = null;
                if (isLocal || !isGlobal)
                    scope = MemoryAreas.LocalVariable;
                else if (!isLocal || isGlobal)
                    scope = MemoryAreas.GlobalVariable;
                else Console.WriteLine("ERROR in SclGenerateAccess(): global / local missmatch !!!");


                string[] parts = operand.Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                XElement accessElement = new XElement(_ns + SclNodes.Access,
                    new XAttribute(SclNodes.Scope, scope),
                    new XAttribute(SclNodes.UId, ++_uid)
                );

                XElement symbolElement = new XElement(_ns + SclNodes.Symbol,
                    new XAttribute(SclNodes.UId, ++_uid)
                );

                SclGenerateComponents(symbolElement, parts, ref _uid);

                accessElement.Add(symbolElement);
                //UId = _uid;
                return accessElement;

            }
            // tutaj pewnie trzeba dodać sprawdzenie operand is LiteralConstant
            else if (operand.MemoryArea == MemoryAreas.LiteralConstant)
            {
                XElement accessElement = new XElement(_ns + SclNodes.Access,
                    new XAttribute(SclNodes.Scope, MemoryAreas.LiteralConstant),
                    new XAttribute(SclNodes.UId, ++_uid));

                XElement constantElement = new XElement(_ns + SclNodes.Constant,
                    new XAttribute(SclNodes.UId, ++_uid));

                XElement constantValueElement = new XElement(_ns + SclNodes.ConstantValue,
                    new XAttribute(SclNodes.UId, ++_uid), operand.Value);

                constantElement.Add(constantValueElement);
                accessElement.Add(constantElement);
                UId = _uid;
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
                symbolElement.Add(new XElement(_ns + SclNodes.Component,
                    new XAttribute(SclNodes.Name, part),
                    new XAttribute(SclNodes.UId, ++uid)
                ));

                if (part != parts.Last())
                {
                    symbolElement.Add(new XElement(_ns + SclNodes.Token,
                        new XAttribute(SclNodes.Text, Token.Dot),
                        new XAttribute(SclNodes.UId, ++uid)
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
            return GeneateAssignment(leftSide, rightSide);
        }


        /// <summary>
        /// Generates assignment elements based on the provided left and right operands.
        /// </summary>
        /// <param name="leftSide">The left operand of the assignment.</param>
        /// <param name="rightSide">The list of right operands for the assignment.</param>
        /// <returns>
        /// A list of XElement representing the assignment elements.
        /// </returns>
        private List<XElement> GeneateAssignment(Operand leftSide, List<ISclSyntax> rightSide)
        {
            List<XElement> assignmentElement = new List<XElement>();
            //// kosmiczna konstrukcja ale okazała się bezużyteczna ;))))
            //_uid = elementWithStructuredTextNode != null ? // 1) - check if argument != null
            //    // 1) argument not null
            //    (GetMaxUId(elementWithStructuredTextNode) >= _uid ? GetMaxUId(elementWithStructuredTextNode) : 0) 
            //    // 1) argument == null
            //    : ( (uid >= _uid) ? 
            //        (uid) 
            //            : (uid == 0 ? _uid : 0) ); // by default argument uid = 0 
            //if (_uid < 21) throw new ArgumentException("Error:UId < 21 ");

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
            foreach (var element in SclGenerateEndOfLine())
            {
                assignmentElement.Add(element);
            }
            UId = _uid;
            return assignmentElement;
        }

        private XElement[] SclGenerateAssignSign()
        {
            return new XElement[]
        {
            new XElement(_ns + SclNodes.Blank, new XAttribute(SclNodes.UId, ++_uid)),
            new XElement(_ns + SclNodes.Token, new XAttribute(SclNodes.Text, Token.Assign), new XAttribute(SclNodes.UId, ++_uid)),
            new XElement(_ns + SclNodes.Blank, new XAttribute(SclNodes.UId, ++_uid))
        };


        }
        private XElement SclGenerateToken(SclToken token)
        {
            return new XElement(_ns + SclNodes.Token, new XAttribute(SclNodes.Text, token.Value), new XAttribute(SclNodes.UId, ++_uid));
        }
        private XElement SclGenerateNewLine()
        {
            return new XElement(_ns + SclNodes.NewLine, new XAttribute(SclNodes.UId, ++_uid));
        }
        private XElement[] SclGenerateEndOfLine()
        {
            return new XElement[] {
                new XElement(_ns + SclNodes.Token, new XAttribute(SclNodes.Text, Token.EndOfExpr), new XAttribute(SclNodes.UId, ++_uid)),
                SclGenerateNewLine()
                    };
        }
        public static int GetMaxUId(XElement xmlElement)
        {
            // Select all attributes named "UId" and parse their values to integers
            var uids = xmlElement
                .DescendantsAndSelf()
                .Attributes("UId")
                .Select(attr => int.TryParse(attr.Value, out int value) ? value : (int?)null)
                .Where(value => value.HasValue)
                .Select(value => value.Value);
            Console.WriteLine(uids);
            // Return the maximum value or 0 if no valid UId attributes are found
            return uids.Any() ? uids.Max() : 0;
        }
        public void XmlSetUid(int uid)
        {
            //_uid = ((uid >= _uid) ?
            //       (uid)
            //           : (uid == 0 ? _uid : 0)); // by default argument uid = 0 
            //if (_uid < 21) throw new ArgumentException("Error:UId < 21 ");
            UId = uid;
        }


    }

}



