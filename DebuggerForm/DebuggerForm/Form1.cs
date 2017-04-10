using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebuggerForm
{
    public partial class Form1 : Form
    {
        private DebuggerService _debuggerService;
        public Form1()
        {
            InitializeComponent();
            _debuggerService = new DebuggerService();
        }

        private void attachToProcess_Click(object sender, EventArgs e)
        {
            _debuggerService.BreakpointsHit = GetBreakpointsHit();

            var process = System.Diagnostics.Process.GetProcessesByName("ConsoleApplication1")[0];
            _debuggerService.Attach(process);
        }

        private List<BreakpointBookmark> GetBreakpointsHit()
        {
            var breakpointsHit = new List<BreakpointBookmark>();

            var breakpoint1 = new BreakpointBookmark();
            breakpoint1.FileName = @"C:\Users\Przemysław\Desktop\ConsoleApplication1\ConsoleApplication1\Program.cs";
            breakpoint1.LineNumber = 15;
            breakpoint1.IsEnabled = true;
            breakpointsHit.Add(breakpoint1);

            return breakpointsHit;
        }
    }
}
