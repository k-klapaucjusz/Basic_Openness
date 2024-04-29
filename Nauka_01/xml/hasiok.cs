using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nauka_01.xml
{
    internal class hasiok
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
    }
}
