using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

internal static class DebuggingSmoke
{
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
    }

    private static object Get(object instance, string property)
    {
        return instance.GetType().GetProperty(property).GetValue(instance, null);
    }

    private static void Set(object instance, string property, object value)
    {
        instance.GetType().GetProperty(property).SetValue(instance, value, null);
    }

    private static XmlElement Save(object store)
    {
        MethodInfo method = store.GetType().GetMethod("Save");
        return (XmlElement)method.Invoke(store, method.GetParameters().Length == 0 ? null : new object[] { null });
    }

    private static void Load(object store, XmlElement xml)
    {
        MethodInfo method = store.GetType().GetMethod("Load");
        method.Invoke(store, method.GetParameters().Length == 1 ? new object[] { xml } : new object[] { xml, null });
    }

    private static void VerifyStore(object store, string filename)
    {
        Check((int)Get(store, "Count") == 2, "Breakpoint and catchpoint count");
        IList points = (IList)store.GetType().GetMethod("GetBreakpoints").Invoke(store, null);
        Check(points.Count == 1, "Breakpoint count");
        object point = points[0];
        Check((string)Get(point, "FileName") == filename, "Unicode path changed");
        Check((int)Get(point, "Line") == 12 && (int)Get(point, "Column") == 3, "Location changed");
        Check(!(bool)Get(point, "Enabled"), "Disabled breakpoint changed");
        Check((string)Get(point, "ConditionExpression") == "变量 < 2 && value == \"测试\"", "Condition changed");
        IList catches = (IList)store.GetType().GetMethod("GetCatchpoints").Invoke(store, null);
        Check(catches.Count == 1, "Catchpoint count");
        Check((string)Get(catches[0], "ExceptionName") == "System.InvalidOperationException", "Catchpoint changed");
    }

    private static Assembly LoadLocal(string name)
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".dll");
        Assembly assembly = Assembly.LoadFrom(path);
        Check(string.Equals(assembly.Location, path, StringComparison.OrdinalIgnoreCase), "Wrong loaded assembly: " + assembly.Location);
        Console.WriteLine("LOADED=" + assembly.FullName + " PATH=" + assembly.Location);
        return assembly;
    }

    private static int Main(string[] args)
    {
        try
        {
            // No IDE initialization, editor construction, user preferences or real debug sessions.
            Assembly debugging = LoadLocal("Mono.Debugging");
            Type storeType = debugging.GetType("Mono.Debugging.Client.BreakpointStore", true);
            int saveParameterCount = storeType.GetMethod("Save").GetParameters().Length;
            Check(saveParameterCount == (args[0] == "new" ? 1 : 0), "Incorrect old/new API loaded");
            object store = Activator.CreateInstance(storeType);
            string filename = Path.Combine(Path.GetFullPath(args[1]), "测试 & breakpoint.lua");
            string fixture = Path.Combine(args[1], args[0] + ".xml");
            if (args[0] == "old" || args[0] == "new")
            {
                object point = storeType.GetMethod("Add", new Type[] { typeof(string), typeof(int), typeof(int) }).Invoke(store, new object[] { filename, 12, 3 });
                Set(point, "Enabled", false);
                Set(point, "ConditionExpression", "变量 < 2 && value == \"测试\"");
                storeType.GetMethod("AddCatchpoint", new Type[] { typeof(string) }).Invoke(store, new object[] { "System.InvalidOperationException" });
                VerifyStore(store, filename);
                XmlElement saved = Save(store);
                File.WriteAllText(fixture, saved.OuterXml);
                object reloaded = Activator.CreateInstance(storeType);
                Load(reloaded, saved);
                VerifyStore(reloaded, filename);
                Console.WriteLine("PASS local persistence " + args[0]);
            }
            string crossFixture = Path.Combine(args[1], args[0] == "new" ? "old.xml" : "new.xml");
            if (File.Exists(crossFixture))
            {
                XmlDocument document = new XmlDocument();
                document.Load(crossFixture);
                Load(store, document.DocumentElement);
                VerifyStore(store, filename);
                Console.WriteLine("PASS cross-version persistence " + Path.GetFileName(crossFixture));
            }
            if (args[0] == "new")
            {
                Assembly debugger = LoadLocal("MonoDevelop.Debugger");
                Assembly editor = LoadLocal("MonoDevelop.SourceEditor2");
                Assembly upper = LoadLocal("CocoStudio.SourceEditor");
                Console.WriteLine("TYPES Debugger=" + debugger.GetTypes().Length + " SourceEditor2=" + editor.GetTypes().Length + " CocoStudio.SourceEditor=" + upper.GetTypes().Length);
                Type view = editor.GetType("MonoDevelop.SourceEditor.SourceEditorView", true);
                Type textView = upper.GetType("CocoStudio.SourceEditor.TextEditorView", true);
                Check(view.IsAssignableFrom(textView), "Upper editor inheritance");
                foreach (string name in new string[] { "ProcessSaveText", "ProcessLoadText", "PrepareToSetCaret" })
                {
                    MethodInfo method = view.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
                    Check(method != null && method.IsFamily && method.IsVirtual, "Missing protected virtual hook " + name);
                }
                Check(view.GetProperty("OwnerDocument", BindingFlags.NonPublic | BindingFlags.Instance) != null, "Missing OwnerDocument");
                Type session = debugging.GetType("Mono.Debugging.Client.DebuggerSession", true);
                Check(session.GetEvent("TargetExited").EventHandlerType.IsGenericType, "Expected updated TargetExited event");
                Console.WriteLine("PASS upper editor type loading and custom hooks");
            }
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            ReflectionTypeLoadException loader = error as ReflectionTypeLoadException;
            if (loader != null) foreach (Exception detail in loader.LoaderExceptions) Console.Error.WriteLine(detail);
            return 1;
        }
    }
}
