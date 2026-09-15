using System;

namespace CocoStudio.UndoManager
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class UndoPropertyAttribute : Attribute
	{
	}
}
