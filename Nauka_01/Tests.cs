using Siemens.Engineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    public static class Tests_chyba_hasiok
    {
        public static void DisplayCompositionInfos(IEngineeringObject obj)
        {
            IList<EngineeringCompositionInfo> compositionInfos = obj.GetCompositionInfos();
            foreach (EngineeringCompositionInfo compositionInfo in compositionInfos)
            {
                Console.WriteLine(compositionInfo.Name);
            }
        }

    }
}
