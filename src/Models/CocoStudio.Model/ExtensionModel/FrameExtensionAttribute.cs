using System;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class FrameExtensionAttribute : CustomExtensionAttribute
	{
		public Type DataType { get; private set; }

		public FrameExtensionAttribute()
		{
		}

		public FrameExtensionAttribute(Type dateType)
		{
			this.DataType = dateType;
		}
	}
}
