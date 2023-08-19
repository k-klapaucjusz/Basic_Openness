using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Basic_Openness
{
    public class ApiWrapper_test
    {
        public ApiWrapper_test() { }

        private IList<TiaPortalProcess> _tiaPortalProcessList;
        public IList<string> DoGetTiaPortalProcesses([CallerMemberName] string caller = "")
        {

            IList<string> processIds = null;
            //CurrentTiaPortalProcess = TiaPortal?.GetCurrentProcess();
            _tiaPortalProcessList = new List<TiaPortalProcess>();
            _tiaPortalProcessList = TiaPortal.GetProcesses();
            if (_tiaPortalProcessList.Count > 0)
            {
                processIds = new List<string>();
                foreach (var item in _tiaPortalProcessList)
                {
                    var mode = item.Mode == TiaPortalMode.WithoutUserInterface ? " Without UI" : " With UI";
                    processIds.Add($"ID {item.Id} \t {mode}");
                }
            }
            return processIds;


        }
        //public class EnumToBooleanConverter : IValueConverter
        //{
        //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        if (value == null || parameter == null)
        //            return false;

        //        if (value.ToString() == parameter.ToString())
        //            return true;

        //        return false;
        //    }

        //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        if (value == null || parameter == null)
        //            return null;

        //        if ((bool)value)
        //            return Enum.Parse(targetType, parameter.ToString());

        //        return null;
        //    }
        //}

    }
}
