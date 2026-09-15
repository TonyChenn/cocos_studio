using System;

namespace Modules.Communal.PropertyGrid
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class ValueRangeAttribute : Attribute
	{
		public int MaxValue { get; set; }

		public int MinValue { get; set; }

		public float Step { get; set; }

		public float PageStep { get; set; }

		public ValueRangeAttribute(int min = 0, int max = 2147483647, float step = 1f, float pageStep = 10f)
		{
			this.MaxValue = max;
			this.MinValue = min;
			this.Step = step;
			this.PageStep = pageStep;
		}
	}
}
