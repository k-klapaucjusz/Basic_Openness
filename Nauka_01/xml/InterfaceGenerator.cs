using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Basic_Openness
{
    

    public class InterfaceGenerator
    {

        private XNamespace ns;

        public InterfaceGenerator() {
            ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
        }

        public InterfaceGenerator(XNamespace ns)
        {
            this.ns = ns;
        }
        
        // Główna metoda generująca sekcję Interface
        public XElement GenerateInterfaceSection(InterfaceData data)
        {
            //XNamespace ns = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
            XElement interfaceSection = new XElement(ns + "Interface",
                new XElement(ns + "Sections", data.Sections.Select(section => GenerateSection(section, ns)))
            );
            return interfaceSection;
        }

        // Metoda generująca pojedynczą sekcję
        private XElement GenerateSection(SectionData section)
        {
            XElement sectionElement = new XElement(ns + "Section",
                new XAttribute("Name", section.Name),
                section.Members.Select(member => GenerateMember(member, ns))
            );
            return sectionElement;
        }

        // Metoda generująca elementy Member
        private XElement GenerateMember(MemberData member)
        {
            XElement memberElement = new XElement(ns + "Member",
                new XAttribute("Name", member.Name),
                new XAttribute("Datatype", member.DataType),
                member.Comment != null ? new XElement(ns + "Comment", GenerateMultiLanguageText(member.Comment, ns)) : null,
                member.Attributes != null ? new XElement(ns + "AttributeList", member.Attributes.Select(attr => GenerateBooleanAttribute(attr, ns))) : null,
                member.StartValue != null ? new XElement(ns + "StartValue", member.StartValue) : null
            );
            return memberElement;
        }

        // Metoda generująca komentarze wielojęzyczne
        private XElement GenerateMultiLanguageText(MultiLanguageText text)
        {
            return new XElement(ns + "MultiLanguageText",
                new XAttribute("Lang", text.Language),
                text.Value
            );
        }

        // Metoda generująca atrybuty Boolean
        private XElement GenerateBooleanAttribute(BooleanAttribute attribute)
        {
            return new XElement(ns + "BooleanAttribute",
                new XAttribute("Name", attribute.Name),
                new XAttribute("SystemDefined", attribute.SystemDefined),
                attribute.Value
            );
        }
        private XElement CreateTemp(string name, string datatype, string comment = null, string language = "en-US")
        {
            MemberData memberData = new MemberData();
            memberData.Name = name;
            memberData.DataType = datatype;
            memberData.Comment.Value = comment;
            memberData.Comment.Language = language;
            memberData.Attributes = null;
            memberData.StartValue = null;
        return GenerateMember(memberData);
        }
        private XElement CreateConstant(string name, string datatype, string startValue = null, string comment = null, string language = "en-US")
        {
            MemberData memberData = new MemberData();
            memberData.Name = name;
            memberData.DataType = datatype;
            memberData.Comment.Value = comment;
            memberData.Comment.Language = language;
            memberData.Attributes = null;
            memberData.StartValue = startValue;
            return GenerateMember(memberData);
        }

        private XElement CreateInput(); //dokończyć


    }

    // Klasy danych
    public class InterfaceData
    {
        public List<SectionData> Sections { get; set; }
    }

    public class SectionData
    {
        public string Name { get; set; }
        public List<MemberData> Members { get; set; }
    }

    public class MemberData
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public MultiLanguageText Comment { get; set; }
        public List<BooleanAttribute> Attributes { get; set; }
        public string StartValue { get; set; }
    }

    public class MultiLanguageText
    {
        public string Language { get; set; }
        public string Value { get; set; }
    }

    public class BooleanAttribute
    {
        public string Name { get; set; }
        public bool SystemDefined { get; set; }
        public string Value { get; set; }
    }

}
