using System;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	internal sealed class FramePropertyAttribute : Attribute
	{
		public Type FrameType { get; private set; }

		public bool IsAutoCreate { get; private set; }

		public FramePropertyAttribute()
		{
		}

		public FramePropertyAttribute(bool isAutoCreate)
		{
			this.IsAutoCreate = isAutoCreate;
		}

		public FramePropertyAttribute(Type frameType) : this()
		{
			this.FrameType = frameType;
		}
	}
}
