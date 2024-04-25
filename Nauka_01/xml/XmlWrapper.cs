using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    public class XmlWrapper
    {
        string InterfaceInputName { get; set; } = "";
        string InterfaceInputDatatype { get; set; } = "";

        string InterfaceOutputName { get; set; } = "";
        string InterfaceOutputDatatype { get; set; } = "";

        string InterfaceStaticName { get; set; } = "";
        string InterfaceStaticDatatype { get; set; } = "";
        string InterfaceStaticStatrValue { get; set; } = "";
        
        string InterfaceTempName { get; set; } = "";
        string InterfaceTempDatatype { get; set; } = "";


        string StartValue { get; set; } = "";
        string Comment { get; set; } = "";
        Dictionary<string, string> Atributes { get; set; } = new Dictionary<string, string>() { 
            { "ExternalAccessible", "true" },
            { "ExternalVisible", "true" },
            {"ExternalWritable", "true" },
        };




    }
}
