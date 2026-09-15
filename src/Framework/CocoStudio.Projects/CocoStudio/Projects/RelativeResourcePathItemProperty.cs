using System;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public class RelativeResourcePathItemProperty : ItemPropertyAttribute
	{
		public RelativeResourcePathItemProperty()
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}

		public RelativeResourcePathItemProperty(string name) : base(name)
		{
			base.SerializationDataType = typeof(RelativePathDataType);
		}
	}
}
