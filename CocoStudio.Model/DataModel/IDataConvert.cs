using System;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000004 RID: 4
	public interface IDataConvert
	{
		// Token: 0x0600000A RID: 10
		object CreateViewModel();

		// Token: 0x0600000B RID: 11
		void SetData(object viewObject);
	}
}
