using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Debugger;
using Debugger.Interop.CorPublish;

namespace DebuggerForm
{
    public class DebuggerService
    {
        public static DebuggerService Instance { get; set; }
        public static NDebugger CurrentDebugger { get; private set; }
        public static Process CurrentProcess { get; private set; }
        public static Thread CurrentThread { get; set; }
        public static StackFrame CurrentStackFrame { get; set; }

        public static PdbSymbolSource PdbSymbolSource = new PdbSymbolSource();
        public List<BreakpointBookmark> BreakpointsHit { get; set; }

        bool attached;

        ICorPublish corPublish;

        /// <inheritdoc/>
        public bool BreakAtBeginning { get; set; }

        public bool ServiceInitialized
        {
            get { return CurrentDebugger != null; }
        }

        public bool IsDebugging
        {
            get
            {
                return ServiceInitialized && CurrentProcess != null;
            }
        }

        public bool IsAttached
        {
            get
            {
                return ServiceInitialized && attached;
            }
        }

        public bool IsProcessRunning
        {
            get
            {
                return IsDebugging && CurrentProcess.IsRunning;
            }
        }

        public event EventHandler DebugStarting;

        public event EventHandler DebugStopped;

        public event EventHandler Initialize;

        public DebuggerService()
        {
            Instance = this;
        }


        public static Thread EvalThread
        {
            get
            {
                if (CurrentProcess == null)
                    throw new GetValueException("Debugger is not running");
                if (CurrentProcess.IsRunning)
                    throw new GetValueException("Process is not paused");
                if (CurrentThread == null)
                    throw new GetValueException("No thread selected");

                return CurrentThread;
            }
        }

        public void Attach(System.Diagnostics.Process existingProcess)
        {
            if (existingProcess == null)
                return;

            if (IsDebugging)
            {
                Console.WriteLine("DEBUGGER ERROR: IS DEBUGGING");
                return;
            }
            if (!ServiceInitialized)
            {
                InitializeService();
            }

            string version = CurrentDebugger.GetProgramVersion(existingProcess.MainModule.FileName);
            if (version.StartsWith("v1.0"))
            {
                Console.WriteLine("DEBUGGER ERROR: .NET 1.0 NOT SUPPORTED");
            }
            else
            {
                OnDebugStarting(EventArgs.Empty);

                UpdateBreakpointLines();

                try
                {
                    CurrentProcess = CurrentDebugger.Attach(existingProcess);
                    DebuggerProcessStarted();
                    attached = true;
                    CurrentProcess.ModuleLoaded += ProcessModulesAdded;
                }
                catch (System.Exception e)
                {
                    // CORDBG_E_DEBUGGER_ALREADY_ATTACHED
                    if (e is COMException || e is UnauthorizedAccessException)
                    {
                        Console.WriteLine("COM EXCEPTION");
                        OnDebugStopped(EventArgs.Empty);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Break()
        {
            if (CurrentProcess != null && CurrentProcess.IsRunning)
            {
                CurrentProcess.Break();
            }
        }

        public void Continue()
        {
            if (CurrentProcess != null && CurrentProcess.IsPaused)
            {
                CurrentProcess.AsyncContinue();
            }
        }

        public void StepInto()
        {
            if (CurrentStackFrame != null)
            {
                CurrentStackFrame.AsyncStepInto();
            }
        }

        public void StepOver()
        {
            if (CurrentStackFrame != null)
            {
                CurrentStackFrame.AsyncStepOver();
            }
        }

        public void StepOut()
        {
            if (CurrentStackFrame != null)
            {
                CurrentStackFrame.AsyncStepOut();
            }
        }

        private void UpdateBreakpointLines()
        {
            Console.WriteLine("UpdateBreakpointLines");
        }

        private void DebuggerProcessStarted()
        {
            OnDebugStarted(EventArgs.Empty);

            //CurrentProcess.ModuleLoaded += (s, e) => UpdateBreakpointIcons();
            //CurrentProcess.ModuleLoaded += (s, e) => RefreshPads();
            //CurrentProcess.ModuleUnloaded += (s, e) => RefreshPads();
            CurrentProcess.LogMessage += LogMessage;
            CurrentProcess.Paused += DebuggedProcessDebuggingPaused;
            CurrentProcess.Resumed += DebuggedProcessDebuggingResumed;
            CurrentProcess.Exited += (s, e) => DebuggerProcessExited();
        }

        private void LogMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Log message");
        }

        private void DebuggedProcessDebuggingPaused(object sender, DebuggerPausedEventArgs e)
        {
            Console.WriteLine("DebuggingPaused");

            CurrentProcess = e.Process;
            if (e.Thread != null)
                CurrentThread = e.Thread;
            else if (CurrentThread != null && CurrentThread.HasExited)
                CurrentThread = null;
        }

        private void DebuggedProcessDebuggingResumed(object sender, DebuggerEventArgs e)
        {
            Console.WriteLine("DebuggingResumed");

            CurrentThread = null;
            CurrentStackFrame = null;
        }

        private void DebuggerProcessExited()
        {
            Console.WriteLine("DebuggingExited");
        }

        private void OnDebugStarted(EventArgs empty)
        {
            Console.WriteLine("DebugStarted");
        }

        private void ProcessModulesAdded(object sender, ModuleEventArgs e)
        {
            Console.WriteLine("ProcessModulesAdded");
        }

        private void OnDebugStopped(EventArgs e)
        {
            if (DebugStopped != null)
            {
                DebugStopped(this, e);
            }
        }

        private void OnDebugStarting(EventArgs e)
        {
            if (DebugStarting != null)
            {
                DebugStarting(null, e);
            }
        }

        private void InitializeService()
        {
            CurrentDebugger = new NDebugger();

            if (Initialize != null)
            {
                Initialize(this, null);
            }

            if (BreakpointsHit != null)
            {
                foreach (var item in BreakpointsHit)
                {
                    AddBreakpoint(item);
                }
            }
        }

        private void AddBreakpoint(BreakpointBookmark bookmark)
        {
            Breakpoint breakpoint = CurrentDebugger.AddBreakpoint(bookmark.FileName, bookmark.LineNumber, 0, bookmark.IsEnabled);
            bookmark.IsHealthy = (CurrentProcess == null) || breakpoint.IsSet;
            //bookmark.IsEnabledChanged += delegate { breakpoint.IsEnabled = bookmark.IsEnabled; };
        }
    }
}
