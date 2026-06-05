using System;
using System.Reflection;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000011 RID: 17
	public class DelegateReference : IDelegateReference
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002B10 File Offset: 0x00000D10
		public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
		{
			if (@delegate == null)
			{
				throw new ArgumentNullException("delegate");
			}
			if (keepReferenceAlive)
			{
				this._delegate = @delegate;
			}
			else
			{
				this._weakReference = new WeakReference(@delegate.Target);
				this._method = @delegate.Method;
				this._delegateType = @delegate.GetType();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002B7C File Offset: 0x00000D7C
		public Delegate Target
		{
			get
			{
				Delegate result;
				if (this._delegate != null)
				{
					result = this._delegate;
				}
				else
				{
					result = this.TryGetDelegate();
				}
				return result;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002BAC File Offset: 0x00000DAC
		private Delegate TryGetDelegate()
		{
			Delegate result;
			if (this._method.IsStatic)
			{
				result = Delegate.CreateDelegate(this._delegateType, null, this._method);
			}
			else
			{
				object target = this._weakReference.Target;
				if (target != null)
				{
					result = Delegate.CreateDelegate(this._delegateType, target, this._method);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x04000013 RID: 19
		private readonly Delegate _delegate;

		// Token: 0x04000014 RID: 20
		private readonly WeakReference _weakReference;

		// Token: 0x04000015 RID: 21
		private readonly MethodInfo _method;

		// Token: 0x04000016 RID: 22
		private readonly Type _delegateType;
	}
}
