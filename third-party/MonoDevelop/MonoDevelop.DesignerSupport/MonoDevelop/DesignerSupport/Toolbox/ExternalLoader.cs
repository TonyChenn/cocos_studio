using System;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[AddinDependency("MonoDevelop.DesignerSupport")]
	internal class ExternalLoader : RemoteProcessObject
	{
		public void Ping()
		{
		}

		public object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}
	}
}
