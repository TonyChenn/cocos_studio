using System;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class SolutionTemplateAttribute : Attribute
	{
		[NodeAttribute]
		public bool IsDefault { get; private set; }

		public SolutionTemplateAttribute()
		{
		}

		internal SolutionTemplateAttribute([NodeAttribute("IsDefault")] bool isDefault)
		{
			this.IsDefault = isDefault;
		}
	}
}
