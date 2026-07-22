using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000129 RID: 297
	public class ExtenderFactory
	{
		// Token: 0x06000B12 RID: 2834 RVA: 0x0002BA10 File Offset: 0x00029C10
		public static void Binding(BaseObject baseObject, params BaseExtender[] monitors)
		{
			CompositeExtender compositeExtender = new CompositeExtender(baseObject);
			foreach (BaseExtender monitor in monitors)
			{
				compositeExtender.Add(monitor);
			}
		}
	}
}
