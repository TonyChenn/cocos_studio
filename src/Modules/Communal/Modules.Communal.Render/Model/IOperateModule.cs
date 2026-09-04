using System;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000018 RID: 24
	public interface IOperateModule : IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		// Token: 0x060000DD RID: 221
		void Initialize(IGLView glView);
	}
}
