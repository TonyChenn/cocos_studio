using System;
using CocoStudio.Core;

namespace Modules.Communal.Output
{
	// Token: 0x02000002 RID: 2
	public interface IOutputPad : IService
	{
		// Token: 0x06000001 RID: 1
		void Clear();

		// Token: 0x06000002 RID: 2
		void ScrollToEnd();
	}
}
