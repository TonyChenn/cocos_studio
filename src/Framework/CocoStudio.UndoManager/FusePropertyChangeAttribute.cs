using System;

namespace CocoStudio.UndoManager
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public abstract class FusePropertyChangeAttribute : Attribute
	{
		protected abstract bool FuseFunction(object originalValue, object firstChange, object seccondChange);

		public Func<object, object, object, bool> CanFuse
		{
			get
			{
				return new Func<object, object, object, bool>(this.FuseFunction);
			}
		}
	}
}
