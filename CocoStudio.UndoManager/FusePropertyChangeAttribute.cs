using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000013 RID: 19
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public abstract class FusePropertyChangeAttribute : Attribute
	{
		// Token: 0x06000089 RID: 137
		protected abstract bool FuseFunction(object originalValue, object firstChange, object seccondChange);

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003248 File Offset: 0x00001448
		public Func<object, object, object, bool> CanFuse
		{
			get
			{
				return new Func<object, object, object, bool>(this.FuseFunction);
			}
		}
	}
}
