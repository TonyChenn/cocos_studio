using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public class ResourcePathItemProperty : ItemPropertyAttribute
	{
		public ResourcePathItemProperty()
		{
			base.SerializationDataType = typeof(PathDataType);
		}

		public ResourcePathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(PathDataType);
		}
	}
}
