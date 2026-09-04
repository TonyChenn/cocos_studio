using System;
using CocoStudio.Core;
using Modules.UI.ComTool.Model;

namespace Modules.UI.ComTool
{
	// Token: 0x02000003 RID: 3
	public interface IComToolPad : IService
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2
		// (set) Token: 0x06000003 RID: 3
		IControlsViewFilter ControlsViewFilter { get; set; }
	}
}
