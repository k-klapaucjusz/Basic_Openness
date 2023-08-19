using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Siemens.Engineering;

namespace Basic_Openness
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("parameter: " + parameter.ToString() + " value.ToString(): " + value.ToString());
            //Console.WriteLine(parameter);
            if (value == null)
                return false;

            if (value.ToString() == parameter.ToString())
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || parameter == null)
                return null;

            if ((bool)value)
            {
                string enumValueString = parameter.ToString();

                if (Enum.TryParse(enumValueString, out TiaPortalMode enumValue))
                    return enumValue;
            }

            return Binding.DoNothing; // tego nie rozumien, to jest od sztuczniaka.
        }
    }

}

