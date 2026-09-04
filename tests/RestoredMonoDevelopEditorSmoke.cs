using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.SourceEditor;

internal sealed class ProbeEditorView : SourceEditorView
{
    public int LoadCalls, SaveCalls, CaretCalls;
    protected override string ProcessLoadText(string text) { LoadCalls++; return base.ProcessLoadText(text); }
    protected override string ProcessSaveText(string text) { SaveCalls++; return base.ProcessSaveText(text); }
    protected override void PrepareToSetCaret(int line, int column) { CaretCalls++; base.PrepareToSetCaret(line, column); }
}

internal static class RestoredMonoDevelopEditorSmoke
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static string ReadStyle(System.Windows.Forms.WebBrowser browser)
    {
        object element = browser.Document.GetElementById("__cocostudio_xwt_style").DomElement;
        object sheet = element.GetType().InvokeMember("styleSheet", BindingFlags.GetProperty, null, element, null);
        return (string)sheet.GetType().InvokeMember("cssText", BindingFlags.GetProperty, null, sheet, null) ?? string.Empty;
    }

    private static void TestWebView()
    {
        Type type = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoStudio.WindowsPlatform.dll"))
            .GetType("CocoStudio.WindowsPlatform.CSWindowWebViewBackend", true);
        object backend = Activator.CreateInstance(type);
        var settings = (Xwt.Backends.IWebViewBackend)backend;
        Check(!settings.ContextMenuEnabled && !settings.ScrollBarsEnabled && settings.DrawsBackground, "Browser defaults changed");
        settings.ContextMenuEnabled = true;
        settings.ScrollBarsEnabled = true;
        settings.DrawsBackground = false;
        settings.CustomCss = "body { color: red; }";
        Check(settings.ContextMenuEnabled && settings.ScrollBarsEnabled && !settings.DrawsBackground, "Pre-realization settings lost");
        // Exercise the native DOM adapter without embedding a visible browser in GTK or visiting a network URL.
        using (var browser = new System.Windows.Forms.WebBrowser())
        {
            type.GetField("view", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(backend, browser);
            settings.ContextMenuEnabled = false;
            settings.ScrollBarsEnabled = false;
            Check(!browser.IsWebBrowserContextMenuEnabled && !browser.ScrollBarsEnabled, "Native browser settings not applied");
            settings.ContextMenuEnabled = true;
            settings.ScrollBarsEnabled = true;
            Check(browser.IsWebBrowserContextMenuEnabled && browser.ScrollBarsEnabled, "Native browser settings not restored");
            bool completed = false;
            browser.DocumentCompleted += delegate(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e) {
                type.GetMethod("view_DocumentCompleted", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(backend, new object[] { sender, e });
                completed = true;
            };
            IntPtr handle = browser.Handle;
            browser.DocumentText = "<html><head><title>local test</title></head><body>test</body></html>";
            DateTime deadline = DateTime.UtcNow.AddSeconds(10);
            while (!completed && DateTime.UtcNow < deadline) { System.Windows.Forms.Application.DoEvents(); Thread.Sleep(10); }
            Check(completed, "Local browser document did not finish loading");
            string css = ReadStyle(browser).ToLowerInvariant();
            Check(css.Contains("red") && css.Contains("transparent"), "Deferred CSS/background settings not applied");
            settings.DrawsBackground = true;
            settings.CustomCss = "body { color: blue; }";
            css = ReadStyle(browser).ToLowerInvariant();
            Check(css.Contains("blue") && !css.Contains("transparent") && !css.Contains("red"), "CSS/background settings not replaced");
            settings.CustomCss = null;
            Check(settings.CustomCss == string.Empty && ReadStyle(browser).Trim() == string.Empty, "CSS reset failed");
        }
        Console.WriteLine("PASS WebView settings and local HTML stylesheet");
    }

    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = new UTF8Encoding(false);
            foreach (string name in new string[] { "Mono.Debugging", "MonoDevelop.Debugger", "MonoDevelop.SourceEditor2", "CocoStudio.WindowsPlatform" })
            {
                string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".dll");
                Assembly loaded = Assembly.LoadFrom(expected);
                Check(string.Equals(Path.GetFullPath(loaded.Location), expected, StringComparison.OrdinalIgnoreCase), "Unexpected assembly location: " + loaded.Location);
                Console.WriteLine("ASSEMBLY=" + loaded.Location);
            }
            string work = Path.GetFullPath(args[0]);
            string profile = Path.GetFullPath(Environment.GetEnvironmentVariable("MONODEVELOP_PROFILE"));
            Check(profile.StartsWith(work + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Profile must be inside test directory");
            string config = UserProfile.Current.ConfigDir;
            Check(Path.GetFullPath(config).StartsWith(profile + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Profile isolation failed");
            Directory.CreateDirectory(config);
            // Seed an empty profile before PropertyService runs, so it cannot migrate real user preferences.
            File.WriteAllText(Path.Combine(config, "MonoDevelopProperties.xml"), "<MonoDevelopProperties version=\"2.0\" />");
            Console.WriteLine("PROFILE=" + config);
            Gtk.Application.Init();
            Console.WriteLine("GTK_INITIALIZED");
            SynchronizationContext.SetSynchronizationContext(new GtkSynchronizationContext());
            typeof(MonoDevelop.Core.Runtime).GetProperty("MainSynchronizationContext").GetSetMethod(true).Invoke(null, new object[] { SynchronizationContext.Current });
            foreach (string name in new string[] { "CocoStudio.Basic", "CocoStudio.Projects", "CocoStudio.Core" })
                Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".dll"));
            AddinManager.Initialize(Path.Combine(work, "registry"), AppDomain.CurrentDomain.BaseDirectory);
            AddinManager.Registry.Update(new ConsoleProgressStatus(1));
            Console.WriteLine("REGISTRY_INITIALIZED");
            Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoStudio.Basic.dll"))
                .GetType("CocoStudio.Basic.Log4Wrap", true).GetMethod("SetLocation", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { Path.Combine(work, "logs") });
            // Match CocoStudio's platform setup; MonoDevelop's standalone desktop initializer is a different path.
            Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoStudio.Core.dll"))
                .GetType("CocoStudio.Core.PlatformAdapter", true).GetMethod("Initialize").Invoke(null, null);
            Check(DesktopService.PlatformService != null, "CocoStudio platform setup failed");
            Check(MonoDevelop.Ide.Fonts.FontService.MonospaceFont != null, "Font service initialization failed");
            IdeApp.CommandService = new MonoDevelop.Components.Commands.CommandManager();
            Console.WriteLine("DESKTOP_INITIALIZED");
            var view = new ProbeEditorView();
            Console.WriteLine("EDITOR_CONSTRUCTED");
            string file = Path.Combine(work, "中文-load-save.lua");
            string content = "-- 测试\r\nlocal text = \"你好 & <世界>\"\r\nreturn text\r\n";
            File.WriteAllText(file, content, new UTF8Encoding(false));
            view.Load(file);
            Check(view.Text == content, "Loaded text differs");
            Check(view.LoadCalls == 1, "Load hook not called");
            view.TextEditor.TextArea.SizeAllocate(new Gdk.Rectangle(0, 0, 800, 600));
            view.SetCaretTo(2, 4);
            Check(view.CaretCalls == 1, "Caret hook not called");
            Check(view.TextEditor.Caret.Line == 2 && view.TextEditor.Caret.Column == 4, "Caret position differs");
            view.Save(file);
            Check(view.SaveCalls == 1, "Save hook not called");
            Check(File.ReadAllText(file, Encoding.UTF8) == content, "Saved text differs");
            Console.WriteLine("PASS editor load/save/caret hooks");
            string inserted = "-- 插入文本\r\n";
            view.InsertText(0, inserted);
            Check(view.Text == inserted + content, "Insert failed");
            view.Undo();
            Check(view.Text == content, "Undo failed");
            view.Redo();
            Check(view.Text == inserted + content, "Redo failed");
            view.DeleteText(0, inserted.Length);
            Check(view.Text == content, "Delete failed");
            view.Save(file);
            Check(File.ReadAllText(file, Encoding.UTF8) == content, "Edited save differs");
            Check(File.ReadAllBytes(file)[0] != 0xef, "Unexpected UTF-8 BOM added");
            Console.WriteLine("PASS insert/delete/undo/redo and UTF-8 without BOM");
            view.Dispose();
            Console.WriteLine("PASS editor disposal");
            using (var bomView = new ProbeEditorView())
            {
                string bomFile = Path.Combine(work, "中文-BOM.lua");
                string lfText = "-- UTF-8 BOM\nreturn '中文'\n";
                File.WriteAllText(bomFile, lfText, new UTF8Encoding(true));
                bomView.Load(bomFile);
                Check(bomView.Text == lfText, "LF/BOM load changed text");
                bomView.Save(bomFile);
                byte[] bytes = File.ReadAllBytes(bomFile);
                Check(bytes.Length >= 3 && bytes[0] == 0xef && bytes[1] == 0xbb && bytes[2] == 0xbf, "UTF-8 BOM lost");
                Check(File.ReadAllText(bomFile, Encoding.UTF8) == lfText, "LF/BOM save changed text");
            }
            Console.WriteLine("PASS UTF-8 BOM and LF preservation");
            TestWebView();
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
