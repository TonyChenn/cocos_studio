using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class DataModelExtensionAttribute : CustomExtensionAttribute
	{
		public Type ModelType { get; private set; }

		public DataModelExtensionAttribute()
		{
		}

		public DataModelExtensionAttribute(Type modelType)
		{
			this.ModelType = modelType;
		}
	}
}
