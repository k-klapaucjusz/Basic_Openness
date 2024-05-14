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
    public static class MemberType
    {
        public const string Input = "Input";
        public const string Output = "Output";
        public const string InOut = "InOut";
        public const string Static = "Static";
        public const string Temp = "Temp";
        public const string Constans = "Constans";
    }

    public static class Remanence
    {
        public const string None = null;
        public const string Retain = "Retain";
        public const string SetInIDB = "SetInIDB";

    }
}
