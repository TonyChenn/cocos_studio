using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B9 RID: 441
	internal class MSBuildSerializer : DataSerializer
	{
		// Token: 0x060010D4 RID: 4308 RVA: 0x00041C7B File Offset: 0x0003FE7B
		public MSBuildSerializer(string baseFile) : base(MSBuildProjectService.DataContext)
		{
			base.SerializationContext.BaseFile = baseFile;
			base.SerializationContext.DirectorySeparatorChar = '\\';
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00041CB8 File Offset: 0x0003FEB8
		protected internal override bool CanHandleProperty(ItemProperty prop, SerializationContext serCtx, object instance)
		{
			if (instance is Project && prop.Name == "Contents")
			{
				return false;
			}
			if (instance is DotNetProject && (prop.Name == "References" || prop.Name == "LanguageParameters"))
			{
				return false;
			}
			if (instance is SolutionEntityItem)
			{
				return prop.IsExtendedProperty(typeof(SolutionEntityItem)) || (prop.Name != "name" && prop.Name != "Configurations");
			}
			if (instance is SolutionFolder && prop.Name == "Files")
			{
				return false;
			}
			if (instance is ProjectFile)
			{
				return prop.IsExtendedProperty(typeof(ProjectFile));
			}
			if (instance is ProjectReference)
			{
				return prop.IsExtendedProperty(typeof(ProjectReference)) || prop.Name == "Package" || prop.Name == "Aliases";
			}
			return (!(instance is DotNetProjectConfiguration) || !(prop.Name == "CodeGeneration")) && (!(instance is ItemConfiguration) || !(prop.Name == "name"));
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00041E00 File Offset: 0x00040000
		protected internal override DataNode OnSerializeProperty(ItemProperty prop, SerializationContext serCtx, object instance, object value)
		{
			DataNode dataNode = base.OnSerializeProperty(prop, serCtx, instance, value);
			if (instance is SolutionEntityItem && dataNode != null)
			{
				if (prop.IsExternal)
				{
					this.ExternalItemProperties.ItemData.Add(dataNode);
				}
				else
				{
					this.InternalItemProperties.ItemData.Add(dataNode);
				}
			}
			return dataNode;
		}

		// Token: 0x040004E0 RID: 1248
		public DataItem InternalItemProperties = new DataItem();

		// Token: 0x040004E1 RID: 1249
		public DataItem ExternalItemProperties = new DataItem();
	}
}
