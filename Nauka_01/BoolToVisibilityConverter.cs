using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using Siemens.Engineering;

namespace Basic_Openness
{
    //[ValueConversion(typeof(TiaPortalMode), typeof(Visibility))]
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("CONVERTER - BoolToVisibilityConverter. Parameter: " + parameter?.ToString());

            if (parameter == null || (string)parameter != "invert")
            {
                if ((bool)value == true)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
            else
            {
                if((bool)value == false || value == null)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //if (value == null)
            //    return null;

            //if ((bool)value)
            //{
            //    string enumValueString = parameter.ToString();

            //    if (Enum.TryParse(enumValueString, out TiaPortalMode enumValue))
            //        return enumValue;
            //}

            return Binding.DoNothing; // tego nie rozumien, to jest od sztuczniaka.
        }
    }
}
