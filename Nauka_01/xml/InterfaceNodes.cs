using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    internal class InterfaceNodes
    {
        public const string Ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
        public const string AttributeList = "AttributeList";
        public const string Interface = "Interface";
        public const string Sections = "Sections";
        public const string Section = "Section";
        public const string Member = "Member";
        public const string Comment = "Comment";
        public const string MultiLanguageText = "MultiLanguageText";
        public const string BooleanAttribute = "BooleanAttribute";
        public const string StartValue = "StartValue";
        public const string MemoryLayout = "MemoryLayout";
        public const string MemoryReserve = "MemoryReserve";
        public const string Name = "Name";
        public const string Number = "Number";
        public const string ProgrammingLanguage = "ProgrammingLanguage";
        public const string SetENOAutomatically = "SetENOAutomatically";
    }
    internal static class SclNodes
    {
        public const string Access = @"Access";
        public const string Blank = @"Blank";
        public const string NewLine = @"NewLine";
        public const string Token = @"Token";
        public const string Call = @"CallInfo";
        public const string Instance = @"Instance";
        public const string Symbol = @"Symbol";
        public const string Component = @"Component";
        public const string Constant = @"Constant";
        public const string Parameter = @"Parameter";


    }
    public static class OperandType
    {
        public const string Input = "Input";
        public const string Output = "Output";
        public const string InOut = "InOut";
        public const string Static = "Static";
        public const string Temp = "Temp";
        public const string Constant = "Constant";
        public const string GlobalVariable = "GlobalVariable";
        public const string LiteralConstant = "LiteralConstant";
        public const string LocalVariable = "LocalVariable";
    }

    public static class Remanence
    {
        public const string None = null;
        public const string Retain = "Retain";
        public const string SetInIDB = "SetInIDB";

    }
    public static class Token
    {
        // Syntax elements
        public const string Assign = @":=";
        public const string Out = @"=&gt";
        public const string BracketL = @"(";
        public const string BracketR = @")";
        public const string EndOfExpr = @";";
        public const string Dot = @".";

        // Arythmetical Operators
        public const string Add = @"+";
        public const string Sub = @"-";
        public const string Mul = @"*";
        public const string Exp = @"**";
        public const string Div = @"/";
        public const string Mod = @"MOD";

        // Comparison Operators
        public const string Equal = @"=";
        public const string NotEqual = @"<>";
        public const string GreaterThan = @">";
        public const string LessThan = @"<";
        public const string GreaterThanOrEqual = @">=";
        public const string LessThanOrEqual = @"<=";

        // Logical Operators
        public const string And = @"AND";
        public const string Or = @"OR";
        public const string Xor = @"XOR";
        public const string Not = @"NOT";

        // Other Operators
        //public const string Concat = @"||";  // String concatenation in SQL
        //public const string In = @"IN";
        //public const string NotIn = @"NOT IN";
        //public const string Like = @"LIKE";
        //public const string NotLike = @"NOT LIKE";
        //public const string Is = @"IS";
        //public const string IsNot = @"IS NOT";
        //public const string Between = @"BETWEEN";
        //public const string NotBetween = @"NOT BETWEEN";
        //public const string Exists = @"EXISTS";
        //public const string NotExists = @"NOT EXISTS";

        //// Bitwise Operators
        //public const string BitwiseAnd = @"&";
        //public const string BitwiseOr = @"|";
        //public const string BitwiseXor = @"^";
        //public const string BitwiseNot = @"~";

    }
}
