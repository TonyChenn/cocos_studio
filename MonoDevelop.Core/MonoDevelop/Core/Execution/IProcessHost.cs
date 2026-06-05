using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200000B RID: 11
	public interface IProcessHost
	{
		// Token: 0x06000045 RID: 69
		void LoadAddins(string[] addinIds);

		// Token: 0x06000046 RID: 70
		IDisposable CreateInstance(Type type);

		// Token: 0x06000047 RID: 71
		IDisposable CreateInstance(string fullTypeName);

		// Token: 0x06000048 RID: 72
		IDisposable CreateInstance(string assemblyPath, string typeName);

		// Token: 0x06000049 RID: 73
		void DisposeObject(IDisposable obj);
	}
}
