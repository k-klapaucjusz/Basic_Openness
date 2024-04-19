using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{
    class TiaPortalProcessId
    {
        public int ProcessId { get; set; }
        public string UiMode { get; set; }
        public TiaPortalProcessId(int Id, string Mode)
        {
            this.ProcessId = Id;
            this.UiMode = Mode;
        }
        public override string ToString()
        {
            return String.Format($"ID {ProcessId} \t {UiMode}");
        }
    }
}
