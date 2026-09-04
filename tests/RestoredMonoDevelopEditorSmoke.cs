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
    private sealed class TextReplaceProbe : MonoDevelop.Refactoring.TextReplaceChange
    {
        public Mono.TextEditor.TextEditorData Data;
        protected override Mono.TextEditor.TextEditorData TextEditorData { get { return Data; } }
    }
    private sealed class DesignerDescriptorProbe : MonoDevelop.DesignerSupport.CustomDescriptor
    {
        [System.ComponentModel.DefaultValue("initial")]
        public string Locked { get; set; }
        public string Editable { get; set; }
        protected override bool IsReadOnly(string propertyName) { return propertyName == "Locked"; }
    }

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

    private static void TestUpperEditor(string work, string profile)
    {
        Assembly upper = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoStudio.SourceEditor.dll"));
        Type bindingType = upper.GetType("CocoStudio.SourceEditor.SourceEditorDisplayBinding", true);
        object binding = Activator.CreateInstance(bindingType);
        MethodInfo canHandle = bindingType.GetMethod("CanHandle");
        string file = Path.Combine(work, "上层编辑器.LUA");
        Check((bool)canHandle.Invoke(binding, new object[] { (FilePath)file, "text/x-lua", null }), "Upper binding must accept uppercase Lua extension");
        Check(!(bool)canHandle.Invoke(binding, new object[] { (FilePath)(file + ".txt"), "text/plain", null }), "Upper binding must reject non-Lua extension");
        Check((bool)bindingType.GetProperty("CanUseAsDefault").GetValue(binding, null), "Upper binding default flag changed");
        string syntaxPath = bindingType.GetProperty("SyntaxModePath").GetValue(null, null).ToString();
        Check(Path.GetFullPath(syntaxPath).StartsWith(profile + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Upper syntax settings escaped test profile");
        string content = "-- 上层封装\r\nreturn '中文'\r\n";
        File.WriteAllText(file, content, new UTF8Encoding(false));
        using (var editor = (SourceEditorView)bindingType.GetMethod("CreateContent").Invoke(binding, new object[] { (FilePath)file, "text/x-lua", null }))
        {
            Check(editor.GetType().FullName == "CocoStudio.SourceEditor.TextEditorView", "Factory returned wrong view");
            editor.Load(file);
            Check(editor.Text == content, "Upper editor load differs");
            Check(editor.CanReuseView(file) && !editor.CanReuseView(file + ".other"), "Upper editor reuse rules changed");
            editor.TextEditor.TextArea.SizeAllocate(new Gdk.Rectangle(0, 0, 800, 600));
            editor.SetCaretTo(2, 3);
            Check(editor.TextEditor.Caret.Line == 2 && editor.TextEditor.Caret.Column == 3, "Upper editor caret differs");
            string prefix = "-- 修改\r\n";
            editor.InsertText(0, prefix);
            editor.Undo();
            Check(editor.Text == content, "Upper editor undo failed");
            editor.Redo();
            Check(editor.Text == prefix + content, "Upper editor redo failed");
            editor.Save(file);
            Check(File.ReadAllText(file, Encoding.UTF8) == prefix + content, "Upper editor save differs");
            editor.Load(file);
            Check(editor.Text == prefix + content, "Upper editor reload differs");
        }
        Console.WriteLine("PASS upper Lua binding/factory, load/save/reload, caret, undo/redo and view reuse");
    }

    private static void TestRefactoring(string work)
    {
        var data = new Mono.TextEditor.TextEditorData();
        try
        {
            data.Text = "abc中文def";
            data.Caret.Offset = 7;
            var change = new TextReplaceProbe { Data = data, Offset = 3, RemovedChars = 2, InsertedText = "X" };
            change.PerformChange(null, new MonoDevelop.Refactoring.RefactoringOptions());
            Check(data.Text == "abcXdef" && data.Caret.Offset == 6, "Refactoring replacement/caret adjustment changed");
            change.Offset = 0;
            change.RemovedChars = 0;
            change.InsertedText = "前";
            change.MoveCaretToReplace = true;
            change.PerformChange(null, new MonoDevelop.Refactoring.RefactoringOptions());
            Check(data.Text == "前abcXdef" && data.Caret.Offset == 1, "Refactoring explicit caret move changed");
            bool rejected = false;
            try { change.RemovedChars = -1; } catch (ArgumentOutOfRangeException) { rejected = true; }
            Check(rejected, "Negative removal was accepted");
            rejected = false;
            try { change.PerformChange(null, null); } catch (InvalidOperationException) { rejected = true; }
            Check(rejected, "Missing refactoring context was accepted");
        }
        finally { data.Dispose(); }
        string file = Path.Combine(work, "重构-BOM.cs");
        string content = "// 中文\r\nreturn 1;\r\n";
        File.WriteAllText(file, content, new UTF8Encoding(true));
        // Supply the loaded editor directly; closed-file discovery requires a full IDE workbench.
        using (var view = new ProbeEditorView())
        {
            view.Load(file);
            var edit = new TextReplaceProbe { Data = view.TextEditor.GetTextEditorData(), FileName = file, Offset = content.IndexOf("1"), RemovedChars = 1, InsertedText = "2" };
            edit.PerformChange(null, new MonoDevelop.Refactoring.RefactoringOptions());
            Check(view.Text == content.Replace("1", "2"), "Refactoring did not edit the loaded view");
            view.Undo();
            Check(view.Text == content, "Refactoring undo failed");
            view.Redo();
            Check(view.Text == content.Replace("1", "2"), "Refactoring redo failed");
            view.Save(file);
        }
        byte[] bytes = File.ReadAllBytes(file);
        Check(bytes[0] == 0xef && bytes[1] == 0xbb && bytes[2] == 0xbf && File.ReadAllText(file) == content.Replace("1", "2"), "Disk refactoring changed encoding or newlines");

        var group = new MonoDevelop.CodeIssues.IssueGroup(MonoDevelop.CodeIssues.NullGroupingProvider.Instance, "检查");
        var issue = new MonoDevelop.CodeIssues.IssueSummary { IssueDescription = "提示" };
        var tree = (MonoDevelop.CodeIssues.IIssueTreeNode)group;
        var leaf = (MonoDevelop.CodeIssues.IIssueTreeNode)issue;
        group.AddIssue(issue);
        Check(group.IssueCount == 1 && tree.Children.Count == 1 && tree.Visible && tree.Text == "检查 (1)", "Issue group population changed");
        int changes = 0;
        tree.TextChanged += delegate { changes++; };
        leaf.Visible = false;
        Check(!tree.Visible && tree.Text == "检查 (0)" && changes == 1, "Issue visibility propagation changed");
        leaf.Visible = true;
        Check(tree.Visible && changes == 2, "Issue visibility restoration changed");
        group.ClearStatistics();
        Check(group.IssueCount == 0 && tree.AllChildren.Count == 0 && !tree.Visible, "Issue group reset changed");
        Console.WriteLine("PASS Refactoring memory/loaded-editor replacements, undo/redo/save, caret, BOM/CRLF, validation and issue visibility");
    }

    private static void TestDesignerSupport()
    {
        var descriptor = new DesignerDescriptorProbe { Locked = "changed", Editable = "before" };
        var properties = descriptor.GetProperties();
        var locked = properties["Locked"];
        var editable = properties["Editable"];
        Check(locked != null && locked.IsReadOnly && !editable.IsReadOnly, "Designer property read-only metadata changed");
        Check((string)locked.GetValue(descriptor) == "changed" && locked.PropertyType == typeof(string), "Designer wrapped property value/type changed");
        Check(locked.CanResetValue(descriptor) && locked.ShouldSerializeValue(descriptor), "Designer property reset metadata changed");
        locked.ResetValue(descriptor);
        Check(descriptor.Locked == "initial" && !locked.ShouldSerializeValue(descriptor), "Designer property reset delegation failed");
        editable.SetValue(descriptor, "after");
        Check(descriptor.Editable == "after" && object.ReferenceEquals(descriptor.GetPropertyOwner(locked), descriptor), "Designer property owner/set changed");
        int changes = 0;
        EventHandler changed = delegate { changes++; };
        locked.AddValueChanged(descriptor, changed);
        // Read-only is UI metadata; the legacy wrapper still delegates explicit SetValue calls.
        locked.SetValue(descriptor, "event");
        locked.RemoveValueChanged(descriptor, changed);
        locked.SetValue(descriptor, "detached");
        Check(changes == 1, "Designer wrapped property event subscription changed");

        var text = new MonoDevelop.DesignerSupport.Toolbox.TextToolboxNode("return '中文'") { Name = "Snippet", Category = "Lua", Description = "Example" };
        var same = new MonoDevelop.DesignerSupport.Toolbox.TextToolboxNode("return '中文'") { Name = "Snippet", Category = "Lua", Description = "Example" };
        Check(text.Filter("sNIP") && text.Filter("EXAMPLE") && text.Filter("中文") && !text.Filter("missing"), "Designer toolbox filtering changed");
        Check(text.Equals(same) && text.GetHashCode() == same.GetHashCode(), "Designer toolbox equality changed");
        same.Text = "different";
        Check(!text.Equals(same) && text.GetDragPreview(null) == "return '中文'", "Designer snippet equality/preview changed");
        Check(text.ItemFilters.Count == 1 && text.ItemFilters[0].FilterString == "text/plain" && text.ItemFilters[0].FilterType == System.ComponentModel.ToolboxItemFilterType.Allow, "Designer snippet filter metadata changed");

        var reference = new MonoDevelop.DesignerSupport.Toolbox.TypeReference(typeof(string));
        Check(reference.Load() == typeof(string), "Designer type reference failed to load");
        var copy = new MonoDevelop.DesignerSupport.Toolbox.TypeReference(reference.TypeName, reference.AssemblyName, reference.AssemblyLocation);
        Check(reference.Equals(copy) && reference.GetHashCode() == copy.GetHashCode(), "Designer type reference equality changed");
        copy.TypeName = "System.Int32";
        Check(!reference.Equals(copy), "Designer type reference distinction changed");
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(reference);
        Check(converter.ConvertToInvariantString(reference) == "System.String", "Designer type reference converter changed");
        Check(reference.GetProjectReference() != null, "Designer project reference construction failed");
        Console.WriteLine("PASS DesignerSupport property descriptors, reset/events, toolbox filtering/preview and type references");
    }

    private static void TestLuaBinding()
    {
        Assembly lua = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoStudio.LuaBinding.dll"));
        Type languageType = lua.GetType("CocoStudio.LuaBinding.LuaLanguageBinding", true);
        var language = (MonoDevelop.Projects.ILanguageBinding)Activator.CreateInstance(languageType, true);
        Check(language.Language == "Lua" && language.SingleLineCommentTag == "--", "Lua language metadata changed");
        Check(language.BlockCommentStartTag == "--[[" && language.BlockCommentEndTag == "]]", "Lua block comment markers changed");
        Check(language.IsSourceCodeFile((FilePath)"中文.LUA") && !language.IsSourceCodeFile((FilePath)"a.lua.txt"), "Lua file recognition changed");
        Check(language.GetFileName((FilePath)"中文").ToString() == "中文.lua", "Lua file naming changed");

        Type providerType = lua.GetType("CocoStudio.LuaBinding.LuaParameterDataProvider", true);
        var provider = (MonoDevelop.Ide.CodeCompletion.ParameterDataProvider)Activator.CreateInstance(providerType, "tonumber", "value [, base]");
        Check(provider.Count == 2 && provider.GetParameterCount(0) == 1 && provider.GetParameterCount(1) == 2, "Optional Lua argument count changed");
        Check(provider.GetParameterName(1, 1) == "base" && !provider.AllowParameterList(2), "Lua parameter name/bounds changed");
        Check(provider.CreateTooltipInformation(1, 2, false).SignatureMarkup == "tonumber(value [, base])", "Lua tooltip signature changed");
        // The original loses the comma before nested optionals. Record parity as a known defect, not correct parameter behavior.
        var nested = (System.Collections.Generic.List<string>)providerType.GetMethod("Unpack").Invoke(null, new object[] { "list [, i [, j]]" });
        Check(string.Join("|", nested.ToArray()) == "list|list i|list i, j", "Known nested optional argument baseline changed");
        Console.WriteLine("KNOWN BASELINE DEFECT: nested optional arguments lose a comma: " + string.Join("|", nested.ToArray()));
        var independent = (System.Collections.Generic.List<string>)providerType.GetMethod("Unpack").Invoke(null, new object[] { "[a,] b [,c]" });
        Check(string.Join("|", independent.ToArray()) == "b|a, b|b, c|a, b, c", "Independent optional Lua arguments changed");

        Type compilerType = lua.GetType("CocoStudio.LuaBinding.LuaCompilerManager", true);
        MethodInfo parseError = compilerType.GetMethod("CreateErrorFromString", BindingFlags.NonPublic | BindingFlags.Static);
        string file = @"D:\临时目录\test.lua";
        var error = (MonoDevelop.Projects.BuildError)parseError.Invoke(null, new object[] { "luac: " + file + ":12: unexpected symbol near ')'" });
        Check(error != null && error.FileName == file && error.Line == 12 && !error.IsWarning && error.ErrorText == "unexpected symbol near ')'", "Lua compiler diagnostic changed");
        Check(parseError.Invoke(null, new object[] { "Lua 5.1.5" }) == null && parseError.Invoke(null, new object[] { "ordinary output" }) == null, "Lua compiler banner filtering changed");

        foreach (string resource in new string[] { "LuaSyntaxMode.xml", "GarrysModLuaSyntaxMode.xml" })
        {
            using (Stream stream = lua.GetManifestResourceStream(resource))
            {
                Check(stream != null, "Missing Lua syntax resource: " + resource);
                var mode = Mono.TextEditor.Highlighting.SyntaxMode.Read(stream);
                Check(mode != null && System.Linq.Enumerable.Any(mode.Keywords), "Lua syntax keywords failed to load: " + resource);
            }
        }
        object completion = Activator.CreateInstance(lua.GetType("CocoStudio.LuaBinding.LuaTextEditorCompletion", true));
        string[] globals = (string[])completion.GetType().GetField("Globals", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(completion);
        Check(globals.Length == 151 && Array.IndexOf(globals, "_G\tprint\t(...)") >= 0, "Lua built-in completion data changed");
        // This method is already a stub in the original library; preserve that baseline, not a claim about all completion paths.
        string[] locals = (string[])lua.GetType("CocoStudio.LuaBinding.LuaParser", true).GetMethod("GetLocals").Invoke(null, new object[] { "local x = 1", 11 });
        Check(locals.Length == 0, "LuaParser.GetLocals baseline changed");
        Console.WriteLine("PASS Lua language, optional parameters, tooltip, diagnostic parser, syntax resources and completion data");
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
            foreach (string name in new string[] { "Mono.Debugging", "MonoDevelop.Debugger", "MonoDevelop.SourceEditor2", "MonoDevelop.DesignerSupport", "MonoDevelop.Refactoring", "CocoStudio.WindowsPlatform", "CocoStudio.SourceEditor", "CocoStudio.LuaBinding" })
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
            // Match application startup: initialize the upper binding (and disable MonoDevelop autosave) before opening any views.
            TestUpperEditor(work, profile);
            TestLuaBinding();
            TestDesignerSupport();
            TestRefactoring(work);
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
