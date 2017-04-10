using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebuggerForm
{
    public class BreakpointBookmarkEventArgs : EventArgs
    {
        BreakpointBookmark breakpointBookmark;

        public BreakpointBookmark BreakpointBookmark
        {
            get
            {
                return breakpointBookmark;
            }
        }

        public BreakpointBookmarkEventArgs(BreakpointBookmark breakpointBookmark)
        {
            this.breakpointBookmark = breakpointBookmark;
        }
    }
}
