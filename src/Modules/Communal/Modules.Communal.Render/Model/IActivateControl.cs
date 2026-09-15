using System;
using CocoStudio.Projects;

namespace Modules.Communal.Render.Model
{
	public interface IActivateControl
	{
		void Activated(CocosItem cocosItem);

		void Deactivated();
	}
}
