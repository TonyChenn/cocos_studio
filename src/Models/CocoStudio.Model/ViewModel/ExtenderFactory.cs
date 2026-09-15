using System;

namespace CocoStudio.Model.ViewModel
{
	public class ExtenderFactory
	{
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
