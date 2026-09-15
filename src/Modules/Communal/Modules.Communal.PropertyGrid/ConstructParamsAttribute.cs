using System;

namespace Modules.Communal.PropertyGrid
{
	public class ConstructParamsAttribute : Attribute
	{
		public object[] ConstructParams { get; private set; }

		public ConstructParamsAttribute(params object[] args)
		{
			this.ConstructParams = args;
		}
	}
}
