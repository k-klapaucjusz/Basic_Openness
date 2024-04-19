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
    public class TiaPortalModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"CONVERT  {this.ToString()}. Target type: {targetType.ToString()}");
            if ((TiaPortalMode)value == TiaPortalMode.WithUserInterface)
                return true;
            else
                return false;
            
            //return (bool)value ? TiaPortalMode.WithUserInterface : TiaPortalMode.WithoutUserInterface; //to wygenerował sztuczniak, ładnie wygląda
            //ale powoduje błędy
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"CONVERT BACK  {this.ToString()}. Target type: {targetType.ToString()}");
            if ((bool)value == true)
                return TiaPortalMode.WithUserInterface;
            else
                return TiaPortalMode.WithoutUserInterface;
            //return (TiaPortalMode)value == TiaPortalMode.WithUserInterface; //to wygenerował sztuczniak, ładnie wygląda
            //ale powoduje błędy
        }
    }
 
}
