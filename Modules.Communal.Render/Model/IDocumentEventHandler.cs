using System;
using CocoStudio.Projects;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000014 RID: 20
	public interface IDocumentEventHandler
	{
		// Token: 0x060000AF RID: 175
		void OnDocumentChanged(CocosItem cocosItem);

		// Token: 0x060000B0 RID: 176
		void OnDocumentSaved(CocosItem cocosItem);

		// Token: 0x060000B1 RID: 177
		void OnDocumentBeforeSave(CocosItem cocosItem);

		// Token: 0x060000B2 RID: 178
		void OnDocumentClosed(CocosItem cocosItem);
	}
}
