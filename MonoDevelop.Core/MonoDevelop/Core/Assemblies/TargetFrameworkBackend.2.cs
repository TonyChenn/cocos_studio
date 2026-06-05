using System;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000BD RID: 189
	public abstract class TargetFrameworkBackend<T> : TargetFrameworkBackend where T : TargetRuntime
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x00018FFD File Offset: 0x000171FD
		public override bool SupportsRuntime(TargetRuntime runtime)
		{
			return runtime is T;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00019008 File Offset: 0x00017208
		protected internal override void Initialize(TargetRuntime runtime, TargetFramework framework)
		{
			base.Initialize(runtime, framework);
			this.targetRuntime = (T)((object)runtime);
		}

		// Token: 0x0400022A RID: 554
		protected T targetRuntime;
	}
}
