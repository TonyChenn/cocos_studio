using System;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class ResourceIgnoreAttribute : Attribute
	{
	}
}
