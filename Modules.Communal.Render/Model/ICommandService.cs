using System;
using CocoStudio.Model.Event;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000007 RID: 7
	public interface ICommandService
	{
		// Token: 0x06000053 RID: 83
		void DeleteObject();

		// Token: 0x06000054 RID: 84
		void CopyObject();

		// Token: 0x06000055 RID: 85
		void PasteObject(PasteObjectsChangeEventArgs args);

		// Token: 0x06000056 RID: 86
		void CutObject();

		// Token: 0x06000057 RID: 87
		void RotateObject(float rotation);
	}
}
