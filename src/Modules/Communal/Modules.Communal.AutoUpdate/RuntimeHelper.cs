using System;
using System.Reflection;
using CocoStudio.Basic;
using Microsoft.Win32;
using MonoDevelop.Core;

namespace Modules.Communal.AutoUpdate
{
	internal class RuntimeHelper
	{
		public static Version RuntimeVersion
		{
			get
			{
				Version result;
				try
				{
					if (Platform.IsWindows)
					{
						using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
						{
							object value = registryKey.GetValue("Release");
							if (value != null)
							{
								return new Version("4.5.0.0");
							}
							return Environment.Version;
						}
					}
					Type type = Type.GetType("Mono.Runtime");
					MethodInfo method = type.GetMethod("GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic);
					string text = method.Invoke(null, null).ToString();
					string[] array = text.Split(new char[]
					{
						' '
					});
					result = new Version(array[0]);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("获取运行环境版本号时出错", exception);
					result = new Version("0.0.1");
				}
				return result;
			}
		}
	}
}
