using System;
using CocoStudio.Projects;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000013 RID: 19
	public interface IActivateControl
	{
		// Token: 0x060000AD RID: 173
		void Activated(CocosItem cocosItem);

		// Token: 0x060000AE RID: 174
		void Deactivated();
	}
}
