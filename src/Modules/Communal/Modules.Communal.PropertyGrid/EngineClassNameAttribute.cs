using System;

namespace Modules.Communal.PropertyGrid
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class EngineClassNameAttribute : Attribute
	{
		public string CocoType { get; set; }

		public EngineClassNameAttribute(string cocoType)
		{
			this.CocoType = cocoType;
		}
	}
}
