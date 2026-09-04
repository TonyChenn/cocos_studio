using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class LoaderContext
	{
		private readonly Hashtable values = new Hashtable();

		private Dictionary<TargetRuntime, ExternalLoader> externalLoaders;

		private int counter;

		public object this[object key]
		{
			get
			{
				return values[key];
			}
			set
			{
				values[key] = value;
			}
		}

		public T CreateExternalLoader<T>(TargetRuntime runtime) where T : MarshalByRefObject
		{
			if (externalLoaders == null)
			{
				externalLoaders = new Dictionary<TargetRuntime, ExternalLoader>();
			}
			if (!externalLoaders.TryGetValue(runtime, out var value))
			{
				value = (ExternalLoader)Runtime.ProcessService.CreateExternalProcessObject(typeof(ExternalLoader), runtime);
				externalLoaders[runtime] = value;
				values[counter++] = value;
			}
			else
			{
				try
				{
					value.Ping();
				}
				catch
				{
					value.Dispose();
					externalLoaders.Remove(runtime);
					return CreateExternalLoader<T>(runtime);
				}
			}
			return (T)value.CreateInstance(typeof(T));
		}

		public IList<ItemToolboxNode> LoadItemsIsolated(TargetRuntime runtime, Type loaderType, string filename)
		{
			if (!typeof(IExternalToolboxLoader).IsAssignableFrom(loaderType))
			{
				throw new InvalidOperationException(string.Concat("Type '", loaderType, "' does not implement 'IExternalToolboxLoader'"));
			}
			try
			{
				ExternalItemLoader externalItemLoader = CreateExternalLoader<ExternalItemLoader>(runtime);
				string s = externalItemLoader.LoadItems(loaderType.Assembly.FullName, loaderType.FullName, filename);
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(Services.ProjectService.DataContext);
				return (ToolboxList)xmlDataSerializer.Deserialize(new StringReader(s), typeof(ToolboxList));
			}
			catch
			{
				return new List<ItemToolboxNode>();
			}
		}

		internal void Dispose()
		{
			foreach (object value in values.Values)
			{
				if (value is IDisposable)
				{
					try
					{
						((IDisposable)value).Dispose();
					}
					catch
					{
					}
				}
			}
		}
	}
}
