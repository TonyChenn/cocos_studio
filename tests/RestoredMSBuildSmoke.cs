using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using System.Text;
using MonoDevelop.Projects.Formats.MSBuild;

public sealed class BridgeLog : MarshalByRefObject, ILogWriter
{
    public readonly StringBuilder Text = new StringBuilder();
    public void WriteLine(string text) { Text.AppendLine(text); }
}

internal static class RestoredMSBuildSmoke
{
    private static bool evaluateItems;
    private static void Check(bool value, string message) { if (!value) throw new Exception(message); }

    private static MSBuildResult Run(IProjectBuilder builder, ProjectConfigurationInfo[] config, BridgeLog log, string target)
    {
        return builder.Run(config, log, MSBuildVerbosity.Normal, target == null ? null : new[] { target },
            evaluateItems ? new[] { "Compile" } : null, new[] { "Label", "ExternalToken", "Configuration" });
    }

    private static int Main(string[] args)
    {
        AppDomain domain = null;
        try
        {
            Console.OutputEncoding = new UTF8Encoding(false);
            evaluateItems = args[1] == "new";
            string work = Path.GetFullPath(args[0]);
            Directory.CreateDirectory(work);
            string location = typeof(BuildEngine).Assembly.Location;
            Check(string.Equals(location, Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "MonoDevelop.Projects.Formats.MSBuild.dll"), StringComparison.OrdinalIgnoreCase), "Wrong bridge assembly loaded");
            Console.WriteLine("ASSEMBLY=" + location);
            string file = Path.Combine(work, "构建样本.proj");
            string xml = "<Project ToolsVersion=\"4.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">" +
                "<PropertyGroup><Label>磁盘内容</Label></PropertyGroup>" +
                "<ItemGroup><Compile Include=\"中文%3B样本.cs\"><Link>$(ExternalToken)/中文.cs</Link><Escaped>%253B</Escaped></Compile></ItemGroup>" +
                "<Target Name=\"Probe\"><Message Text=\"桥接日志\" Importance=\"High\"/>" +
                "<Warning Text=\"测试警告\" Code=\"SMOKE01\"/></Target>" +
                "<Target Name=\"Broken\"><Error Text=\"测试错误\" Code=\"SMOKE02\"/></Target></Project>";
            File.WriteAllText(file, xml, new UTF8Encoding(false));
            var config = new[] { new ProjectConfigurationInfo { ProjectFile = file,
                ProjectGuid = "{FB03C697-35E4-4428-A5B9-32F99806C3B7}", Configuration = "Debug", Platform = "x86" } };
            domain = AppDomain.CreateDomain("IsolatedBuildBridge", null, new AppDomainSetup {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
            var engine = (IBuildEngine)domain.CreateInstanceAndUnwrap(typeof(BuildEngine).Assembly.FullName, typeof(BuildEngine).FullName);
            Check(RemotingServices.IsTransparentProxy(engine), "Expected a cross-domain engine proxy");
            engine.SetCulture(CultureInfo.InvariantCulture);
            engine.SetGlobalProperties(new Dictionary<string, string> { { "ExternalToken", "外部参数" } });
            engine.Ping();
            var builder = engine.LoadProject(file);
            var log = new BridgeLog();
            var result = Run(builder, config, log, "Probe");
            Check(result.Properties["Label"] == "磁盘内容" && result.Properties["ExternalToken"] == "外部参数" &&
                result.Properties["Configuration"] == "Debug", "Evaluated properties differ");
            if (evaluateItems)
            {
                Check(result.Items["Compile"].Count == 1 && result.Items["Compile"][0].ItemSpec == "中文;样本.cs" &&
                    result.Items["Compile"][0].Metadata.Count == 2 &&
                    result.Items["Compile"][0].Metadata["Link"] == "外部参数/中文.cs" &&
                    result.Items["Compile"][0].Metadata["Escaped"] == "%3B", "Evaluated item/metadata differs");
                Console.WriteLine("PASS item evaluation and metadata escaping regression");
            }
            Check(result.Errors.Length == 1 && result.Errors[0].IsWarning && result.Errors[0].Code == "SMOKE01", "Warning result differs");
            Check(log.Text.ToString().Contains("桥接日志"), "Remote log callback missing");
            Console.WriteLine("PASS remote property evaluation, target execution and warning/log callbacks");
            if (!evaluateItems)
            {
                bool expectedFailure = false;
                try { builder.Run(config, null, MSBuildVerbosity.Quiet, null, new[] { "Compile" }, null); }
                catch (Exception error) { expectedFailure = error.InnerException is NullReferenceException; }
                Check(expectedFailure, "Expected the original Windows metadata reflection failure");
                Console.WriteLine("REPRODUCED legacy item metadata NullReferenceException (expected baseline defect)");
            }
            builder.RefreshWithContent(xml.Replace("磁盘内容", "未保存内容"));
            Check(Run(builder, config, log, null).Properties["Label"] == "未保存内容", "Unsaved project refresh failed");
            Check(File.ReadAllText(file, Encoding.UTF8) == xml, "Unsaved refresh changed disk file");
            builder.Refresh();
            Check(Run(builder, config, log, null).Properties["Label"] == "磁盘内容", "Disk refresh failed");
            result = Run(builder, config, log, "Broken");
            Check(result.Errors.Length == 1 && !result.Errors[0].IsWarning && result.Errors[0].Code == "SMOKE02", "Error result differs");
            builder.RefreshWithContent("<Project>");
            result = Run(builder, config, log, null);
            Check(result.Errors.Length == 1 && !result.Errors[0].IsWarning && result.Errors[0].LineNumber > 0, "Invalid XML diagnostic missing");
            engine.UnloadProject(builder);
            engine.Dispose();
            Console.WriteLine("PASS unsaved/disk refresh, error diagnostics, unload and remote result serialization");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        finally { if (domain != null) AppDomain.Unload(domain); }
    }
}
