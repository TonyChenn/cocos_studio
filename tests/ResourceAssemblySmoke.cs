using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

internal static class ResourceAssemblySmoke
{
    private static string Hash(Stream stream)
    {
        using (stream)
        using (SHA256 sha = SHA256.Create())
            return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", string.Empty);
    }

    private static int Main(string[] args)
    {
        try
        {
            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(args[0]));
            Type type = assembly.GetType(args[1], true);
            MethodInfo accessor = type.GetMethod("GetResourceStream", BindingFlags.Public | BindingFlags.Static);
            if (!type.IsAbstract || !type.IsSealed || accessor == null || accessor.ReturnType != typeof(Stream))
                throw new Exception("Resource accessor API changed.");
            ParameterInfo[] parameters = accessor.GetParameters();
            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(string))
                throw new Exception("Resource accessor parameters changed.");
            Console.WriteLine("IDENTITY=" + assembly.FullName);
            Console.WriteLine("FILE_VERSION=" + assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version);
            foreach (string name in assembly.GetManifestResourceNames().OrderBy(value => value, StringComparer.Ordinal))
            {
                Stream stream = (Stream)accessor.Invoke(null, new object[] { name });
                if (stream == null) throw new Exception("Accessor did not return " + name);
                Console.WriteLine("RESOURCE=" + name + "|" + Hash(stream));
            }
            if (accessor.Invoke(null, new object[] { "__missing_resource__" }) != null)
                throw new Exception("Missing resource must return null.");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }
}
