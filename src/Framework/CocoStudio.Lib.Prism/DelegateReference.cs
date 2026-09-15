using System;
using System.Reflection;

namespace CocoStudio.Lib.Prism
{
	public class DelegateReference : IDelegateReference
	{
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

		private readonly Delegate _delegate;

		private readonly WeakReference _weakReference;

		private readonly MethodInfo _method;

		private readonly Type _delegateType;
	}
}
