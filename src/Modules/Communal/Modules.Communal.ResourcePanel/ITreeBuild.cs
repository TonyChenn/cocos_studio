using System;
using System.Collections;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001A RID: 26
	public interface ITreeBuild
	{
		// Token: 0x060000B6 RID: 182
		void UpdateAll();

		// Token: 0x060000B7 RID: 183
		void Update();

		// Token: 0x060000B8 RID: 184
		void Update(object objecData);

		// Token: 0x060000B9 RID: 185
		void UpdateChildren();

		// Token: 0x060000BA RID: 186
		void Remove();

		// Token: 0x060000BB RID: 187
		void Remove(object dataObject);

		// Token: 0x060000BC RID: 188
		void AddChild(object dataObject);

		// Token: 0x060000BD RID: 189
		void AddChildren(IEnumerable dataObjects);

		// Token: 0x060000BE RID: 190
		void AddChild(object dataObject, bool moveToChild);

		// Token: 0x060000BF RID: 191
		void AddChild(object parent, object dataObject, bool moveToChild);
	}
}
