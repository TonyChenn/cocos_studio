using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000099 RID: 153
	public interface IFreezable
	{
		/// <summary>
		/// Gets if this instance is frozen. Frozen instances are immutable and thus thread-safe.
		/// </summary>
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060004D3 RID: 1235
		bool IsFrozen { get; }

		/// <summary>
		/// Freezes this instance.
		/// </summary>
		// Token: 0x060004D4 RID: 1236
		void Freeze();
	}
}
