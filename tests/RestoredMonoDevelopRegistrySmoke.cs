using System;
using System.IO;
using System.Linq;
using Mono.Addins;

internal static class RestoredMonoDevelopRegistrySmoke
{
    private static int Main(string[] args)
    {
        try
        {
            // Use only the caller-created temporary cache; never initialize the editor's runtime or user registry.
            string cache = Path.GetFullPath(args[0]);
            Directory.CreateDirectory(cache);
            using (var registry = new AddinRegistry(Path.Combine(cache, "registry"), AppDomain.CurrentDomain.BaseDirectory,
                Path.Combine(cache, "addins"), Path.Combine(cache, "database")))
            {
                registry.Update(new ConsoleProgressStatus(1));
                var ids = registry.GetAddins().Select(a => "ADDIN " + a.Id)
                    .Concat(registry.GetAddinRoots().Select(a => "ROOT " + a.Id)).OrderBy(s => s).ToArray();
                foreach (string id in ids) Console.WriteLine(id);
                if (!ids.Any(id => id.Contains("MonoDevelop.Debugger,")) || !ids.Any(id => id.Contains("MonoDevelop.SourceEditor2,")))
                    throw new Exception("Restored MonoDevelop modules were not discovered.");
                Console.WriteLine("REGISTRY_COUNT " + ids.Length);
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
