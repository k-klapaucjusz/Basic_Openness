using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basic_Openness
{
    internal class XmlTest01
    {
        public XElement GenerateXml()
        {
            XElement section = new XElement("Section", new XAttribute("Name", "Static"),
                new XElement("Member", new XAttribute("Name", "rStatic1"), new XAttribute("Datatype", "Real"),
                    new XElement("AttributeList",
                        new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "true"),
                        new XElement("Comment", new XElement("MultiLanguageText", new XAttribute("Lang", "en-US"), "comment - static 1")),
                        new XElement("StartValue", "666.0")
                    )
                ),
                new XElement("Member", new XAttribute("Name", "rStatic2"), new XAttribute("Datatype", "Real"), new XAttribute("Remanence", "Retain"),
                    new XElement("AttributeList",
                        new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false")
                    ),
                    new XElement("StartValue", "1.1")
                ),
                new XElement("Member", new XAttribute("Name", "rStatic3"), new XAttribute("Datatype", "Real"), new XAttribute("Remanence", "SetInIDB"),
                    new XElement("AttributeList",
                        new XElement("BooleanAttribute", new XAttribute("Name", "ExternalVisible"), new XAttribute("SystemDefined", "true"), "false"),
                        new XElement("BooleanAttribute", new XAttribute("Name", "ExternalWritable"), new XAttribute("SystemDefined", "true"), "false")
                    )
                ),
                new XElement("Member", new XAttribute("Name", "mi_fb_2i2oScl"), new XAttribute("Datatype", "\"fb_2i2oScl\""),
                    new XElement("AttributeList",
                        new XElement("BooleanAttribute", new XAttribute("Name", "SetPoint"), new XAttribute("SystemDefined", "true"), "true")
                    )
                )
            );

            return section;
        }
        public XElement GenerateXml(Dictionary<string, Dictionary<string, string>> memberData)
        {
            XElement section = new XElement("Section", new XAttribute("Name", "Static"));

            foreach (var member in memberData)
            {
                XElement memberElement = new XElement("Member", new XAttribute("Name", member.Key));

                foreach (var attribute in member.Value)
                {
                    memberElement.Add(new XElement(attribute.Key, attribute.Value));
                }

                section.Add(memberElement);
            }

            return section;
        }

        Dictionary<string, Dictionary<string, string>> memberData = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "rStatic1", new Dictionary<string, string>()
                {
                    {"Datatype", "Real"},
                    {"StartValue", "666.0"},
                    {"SetPoint", "true"},
                    {"Comment", "comment - static 1"},
                }
            },
            {
                "rStatic2", new Dictionary<string, string>()
                {
                    {"Datatype", "Real"},
                    {"StartValue", "1.1"},
                    {"Remanence", "Retain"},
                    {"ExternalVisible", "false"},
                }
            },
            {
                "rStatic3", new Dictionary<string, string>()
                {
                    {"Datatype", "Real"},
                    {"Remanence", "SetInIDB"},
                    {"ExternalVisible", "false"},
                    {"ExternalWritable", "false"},
                }
            },
            {
                "mi_fb_2i2oScl", new Dictionary<string, string>()
                {
                    {"Datatype", "\"fb_2i2oScl\""},
                    {"SetPoint", "true"},
                }
            }
        };

    }
}
