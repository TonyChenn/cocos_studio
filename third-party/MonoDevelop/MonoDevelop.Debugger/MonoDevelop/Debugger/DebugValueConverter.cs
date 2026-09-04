using System;
using Mono.Debugging.Client;

namespace MonoDevelop.Debugger
{
	public abstract class DebugValueConverter<T>
	{
		public abstract bool CanGetValue(ObjectValue val);

		public abstract T GetValue(ObjectValue val);

		public virtual bool CanSetValue(ObjectValue val)
		{
			return false;
		}

		public virtual void SetValue(T value, ObjectValue val)
		{
			throw new NotImplementedException();
		}
	}
}
