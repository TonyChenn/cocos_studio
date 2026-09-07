using System.IO;
using System.Reflection;

namespace Cocos.Launcher
{
	public static class Resources
	{
		private static Assembly assembly = typeof(Resources).Assembly;

		public static Stream GetResourceStream(string resourceName)
		{
			return assembly.GetManifestResourceStream(resourceName);
		}
	}
}
