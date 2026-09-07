using System;
using System.IO;
using System.Threading;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;

internal sealed class ProbeSession : DebuggerSession
{
    public void EmitExit() { OnTargetEvent(new TargetEventArgs(TargetEventType.TargetExited)); }
    // This fake session must never attach to or control a real process.
    protected override void OnRun(DebuggerStartInfo info) { throw new NotSupportedException(); }
    protected override void OnAttachToProcess(long id) { throw new NotSupportedException(); }
    protected override void OnDetach() { throw new NotSupportedException(); }
    protected override void OnSetActiveThread(long process, long thread) { throw new NotSupportedException(); }
    protected override void OnStop() { throw new NotSupportedException(); }
    protected override void OnExit() { throw new NotSupportedException(); }
    protected override void OnStepLine() { throw new NotSupportedException(); }
    protected override void OnNextLine() { throw new NotSupportedException(); }
    protected override void OnStepInstruction() { throw new NotSupportedException(); }
    protected override void OnNextInstruction() { throw new NotSupportedException(); }
    protected override void OnFinish() { throw new NotSupportedException(); }
    protected override void OnContinue() { throw new NotSupportedException(); }
    protected override BreakEventInfo OnInsertBreakEvent(BreakEvent point) { throw new NotSupportedException(); }
    protected override void OnRemoveBreakEvent(BreakEventInfo point) { throw new NotSupportedException(); }
    protected override void OnUpdateBreakEvent(BreakEventInfo point) { throw new NotSupportedException(); }
    protected override void OnEnableBreakEvent(BreakEventInfo point, bool enable) { throw new NotSupportedException(); }
    protected override ThreadInfo[] OnGetThreads(long process) { return new ThreadInfo[0]; }
    protected override ProcessInfo[] OnGetProcesses() { return new ProcessInfo[0]; }
    protected override Backtrace OnGetThreadBacktrace(long process, long thread) { throw new NotSupportedException(); }
}

internal static class SessionSmoke
{
    private static int Main()
    {
        try
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mono.Debugging.dll");
            if (!string.Equals(typeof(DebuggerSession).Assembly.Location, expected, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Incorrect library loaded");
            var session = new ProbeSession();
            int count = 0;
            EventHandler handler = delegate(object sender, EventArgs args)
            {
                if (sender != session) throw new Exception("Exit event sender");
                count++;
            };
            session.TargetExited += handler;
            session.EmitExit();
            if (count != 1 || !session.HasExited) throw new Exception("Exit event not delivered");
            session.TargetExited -= handler;
            session.EmitExit();
            if (count != 1) throw new Exception("Exit event handler not removed");
            Console.WriteLine("PASS original TargetExited subscribe, dispatch, unsubscribe (no real debug process)");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
