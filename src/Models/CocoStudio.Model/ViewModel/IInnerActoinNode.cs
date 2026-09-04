using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000E0 RID: 224
	public interface IInnerActoinNode
	{
		// Token: 0x06000719 RID: 1817
		void ApplyActionValue(InnerActionValue actionValue);

		// Token: 0x0600071A RID: 1818
		void ApplyStep(InnerActionValue actionValue, int frameIndex);

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600071B RID: 1819
		// (set) Token: 0x0600071C RID: 1820
		InnerActionValue ActionValue { get; set; }
	}
}
