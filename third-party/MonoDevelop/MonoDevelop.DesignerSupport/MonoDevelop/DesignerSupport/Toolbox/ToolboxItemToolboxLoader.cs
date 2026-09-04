using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public abstract class ToolboxItemToolboxLoader : IToolboxLoader, IExternalToolboxLoader
	{
		private bool initialized;

		public string[] FileTypes => new string[2] { "dll", "exe" };

		public IList<ItemToolboxNode> Load(LoaderContext ctx, string filename)
		{
			List<ItemToolboxNode> list = new List<ItemToolboxNode>();
			foreach (TargetRuntime supportedRuntime in GetSupportedRuntimes(filename))
			{
				list.AddRange(ctx.LoadItemsIsolated(supportedRuntime, GetType(), filename));
			}
			return list;
		}

		private IEnumerable<TargetRuntime> GetSupportedRuntimes(string filename)
		{
			bool found = false;
			foreach (TargetRuntime runtime in Runtime.SystemAssemblyService.GetTargetRuntimes())
			{
				SystemPackage p = runtime.AssemblyContext.GetPackageFromPath(filename);
				if (p != null)
				{
					found = true;
					yield return runtime;
				}
			}
			if (found)
			{
				yield break;
			}
			foreach (TargetRuntime targetRuntime in Runtime.SystemAssemblyService.GetTargetRuntimes())
			{
				yield return targetRuntime;
			}
		}

		IList<ItemToolboxNode> IExternalToolboxLoader.Load(string filename)
		{
			TargetRuntime currentRuntime = Runtime.SystemAssemblyService.CurrentRuntime;
			List<ItemToolboxNode> list = new List<ItemToolboxNode>();
			Assembly assembly;
			try
			{
				assembly = ((!(currentRuntime is MsNetTargetRuntime)) ? Assembly.LoadFile(filename) : Assembly.ReflectionOnlyLoadFrom(filename));
			}
			catch (Exception ex)
			{
				LoggingService.LogError("ToolboxItemToolboxLoader: Could not load assembly '" + filename + "'", ex);
				return list;
			}
			SystemPackage packageFromPath = currentRuntime.AssemblyContext.GetPackageFromPath(filename);
			if (!initialized)
			{
				Application.Init();
				initialized = true;
			}
			ClrVersion clrVersion = ClrVersion.Default;
			byte[] a = new byte[8] { 183, 122, 92, 86, 25, 52, 224, 137 };
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			foreach (AssemblyName assemblyName in referencedAssemblies)
			{
				if (assemblyName.Name == "mscorlib" && byteArraysEqual(a, assemblyName.GetPublicKeyToken()))
				{
					if (assemblyName.Version == new Version(4, 0, 0, 0))
					{
						clrVersion = ClrVersion.Net_4_0;
						break;
					}
					if (assemblyName.Version == new Version(2, 0, 0, 0))
					{
						clrVersion = ClrVersion.Net_2_0;
						break;
					}
					if (assemblyName.Version == new Version(1, 0, 5000, 0))
					{
						clrVersion = ClrVersion.Net_1_1;
						break;
					}
				}
			}
			if (clrVersion == ClrVersion.Default)
			{
				LoggingService.LogError("ToolboxItemToolboxLoader: assembly '{0}' references unknown runtime version.", filename);
				return list;
			}
			Type[] types = assembly.GetTypes();
			Type[] array = types;
			foreach (Type type in array)
			{
				if (type.IsAbstract || !type.IsPublic || !type.IsClass)
				{
					continue;
				}
				object[] customAttributes = type.GetCustomAttributes(typeof(ToolboxItemAttribute), inherit: true);
				if (customAttributes == null || customAttributes.Length == 0)
				{
					continue;
				}
				ToolboxItemAttribute toolboxItemAttribute = (ToolboxItemAttribute)customAttributes[0];
				if (toolboxItemAttribute.Equals(ToolboxItemAttribute.None) || toolboxItemAttribute.ToolboxItemType == null)
				{
					continue;
				}
				string attributeCategory = null;
				customAttributes = type.GetCustomAttributes(typeof(CategoryAttribute), inherit: true);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					attributeCategory = ((CategoryAttribute)customAttributes[0]).Category;
				}
				try
				{
					ItemToolboxNode node = GetNode(type, toolboxItemAttribute, attributeCategory, (packageFromPath != null) ? filename : null, clrVersion);
					if (node != null)
					{
						node.ItemFilters.Add(new ToolboxItemFilterAttribute("TargetRuntime." + currentRuntime.Id, ToolboxItemFilterType.Require));
						list.Add(node);
					}
				}
				catch (Exception ex2)
				{
					LoggingService.LogError("Unhandled error in toolbox node loader '" + GetType().FullName + "' with type '" + type.FullName + "' in assembly '" + assembly.FullName + "'", ex2);
				}
			}
			return list;
		}

		private static bool byteArraysEqual(byte[] a, byte[] b)
		{
			if (a == null)
			{
				return b == null;
			}
			if (b == null)
			{
				return a == null;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		public abstract ItemToolboxNode GetNode(Type type, ToolboxItemAttribute attribute, string attributeCategory, string assemblyPath, ClrVersion referencedRuntime);
	}
}
