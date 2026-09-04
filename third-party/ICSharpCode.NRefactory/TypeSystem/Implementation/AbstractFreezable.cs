using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	public abstract class AbstractFreezable : IFreezable
	{
		/// <summary>
		/// Gets if this instance is frozen. Frozen instances are immutable and thus thread-safe.
		/// </summary>
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0000BFDC File Offset: 0x0000AFDC
		public bool IsFrozen
		{
			get
			{
				return this.isFrozen;
			}
		}

		/// <summary>
		/// Freezes this instance.
		/// </summary>
		// Token: 0x060004F5 RID: 1269 RVA: 0x0000BFE4 File Offset: 0x0000AFE4
		public void Freeze()
		{
			if (!this.isFrozen)
			{
				this.FreezeInternal();
				this.isFrozen = true;
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000BFFB File Offset: 0x0000AFFB
		protected virtual void FreezeInternal()
		{
		}

		// Token: 0x04000150 RID: 336
		private bool isFrozen;
	}
}
