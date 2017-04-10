using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebuggerForm
{
    public class BreakpointBookmark
    {
        private bool isHealthy;
        private bool isEnabled;
        public int LineNumber { get; set; }
        public string FileName { get; set; }

        public virtual bool IsHealthy
        {
            get
            {
                
                return isHealthy;
            }
            set
            {
                if (isHealthy != value)
                {
                    isHealthy = value;
                }
            }
        }

        [DefaultValue(true)]
        public virtual bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                }
            }
        }
    }
}
