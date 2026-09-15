using System;

namespace CocoStudio.Model.ViewModel
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class IgnoreResizeAttribute : Attribute
	{
	}
}
