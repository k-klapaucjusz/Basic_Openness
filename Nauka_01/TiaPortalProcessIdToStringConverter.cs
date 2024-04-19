using Siemens.Engineering;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Basic_Openness
{
    public class TiaPortalProcessIdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"CONVERT  {this.ToString()}. Target type: {targetType.ToString()}");
            if ((TiaPortalProcessId)value != null)
                return ((TiaPortalProcessId)value).ProcessId.ToString();
            else
                return "empty";
            
            //return (bool)value ? TiaPortalMode.WithUserInterface : TiaPortalMode.WithoutUserInterface; //to wygenerował sztuczniak, ładnie wygląda
            //ale powoduje błędy
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return new TiaPortalProcessId(-1, "");
            return Binding.DoNothing;
        }
    }
 
}
