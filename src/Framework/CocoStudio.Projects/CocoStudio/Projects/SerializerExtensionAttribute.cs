using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SerializerExtensionAttribute : Attribute
	{
		[NodeAttribute]
		public bool IsDefault { get; private set; }

		public SerializerExtensionAttribute()
		{
		}

		public SerializerExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault)
		{
			this.IsDefault = isDefault;
		}
	}
}
