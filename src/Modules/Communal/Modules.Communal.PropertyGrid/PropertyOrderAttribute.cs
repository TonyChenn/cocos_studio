using System;

namespace Modules.Communal.PropertyGrid
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class PropertyOrderAttribute : Attribute
	{
		public int Order { get; set; }

		public PropertyOrderAttribute(int order)
		{
			this.Order = order;
		}
	}
}
