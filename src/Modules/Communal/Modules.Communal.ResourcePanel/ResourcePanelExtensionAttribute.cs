using System;
using Mono.Addins;

namespace Modules.Communal.ResourcePanel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ResourcePanelExtensionAttribute : CustomExtensionAttribute
	{
		public Type ModelType { get; private set; }

		public ResourcePanelExtensionAttribute()
		{
		}

		public ResourcePanelExtensionAttribute(Type modelType)
		{
			this.ModelType = modelType;
		}
	}
}
