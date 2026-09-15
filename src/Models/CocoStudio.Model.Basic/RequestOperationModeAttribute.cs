using System;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class RequestOperationModeAttribute : Attribute
	{
		public OperationMask RequestMode { get; private set; }

		public RequestOperationModeAttribute(OperationMask requestMode)
		{
			this.RequestMode = requestMode;
		}
	}
}
