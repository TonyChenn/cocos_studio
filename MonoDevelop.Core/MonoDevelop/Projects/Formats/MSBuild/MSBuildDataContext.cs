using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BF RID: 447
	internal class MSBuildDataContext : DataContext
	{
		// Token: 0x0600113A RID: 4410 RVA: 0x000461DA File Offset: 0x000443DA
		protected override DataType CreateConfigurationDataType(Type type)
		{
			if (type == typeof(bool))
			{
				return new MSBuildBoolDataType();
			}
			if (type == typeof(bool?))
			{
				return new MSBuildNullableBoolDataType();
			}
			return base.CreateConfigurationDataType(type);
		}
	}
}
