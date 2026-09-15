using System;

namespace Modules.Communal.Render.Model
{
	public interface IOperateModule : IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		void Initialize(IGLView glView);
	}
}
