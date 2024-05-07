using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Basic_Openness
{
    public class XmlWrapper : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private string _generatedXmlAsString;
        public string GeneratedXmlAsString {
            get => _generatedXmlAsString;
            set
            {
                if (value != _generatedXmlAsString)
                {
                    _generatedXmlAsString = value;
                    NotifyPropertyChanged(nameof(GeneratedXmlAsString));
                }
            }
        }

        public XElement GeneratedXml { get; set; }
        public XNamespace NsInterface { get; set; } = "http://www.siemens.com/automation/Openness/SW/Interface/v5";
        public XDocument XmlFile { get; set; }
        public Microsoft.Win32.OpenFileDialog XmlOpenFileDialog { get; set; }

        public string InterfaceInputName { get; set; } = "";
        public string InterfaceInputDatatype { get; set; } = "";

        public string InterfaceOutputName { get; set; } = "";
        public string InterfaceOutputDatatype { get; set; } = "";

        public string InterfaceStaticName { get; set; } = "";
        public string InterfaceStaticDatatype { get; set; } = "";
        public string InterfaceStaticStatrValue { get; set; } = "";
        
        public string InterfaceTempName { get; set; } = "";
        public string InterfaceTempDatatype { get; set; } = "";


        public string StartValue { get; set; } = "";
        public string Comment { get; set; } = "";
        Dictionary<string, string> Atributes { get; set; } = new Dictionary<string, string>() { 
            { "ExternalAccessible", "true" },
            { "ExternalVisible", "true" },
            {"ExternalWritable", "true" },
        };


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
