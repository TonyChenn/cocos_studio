using System;

namespace CocoStudio.Model
{
	public interface ILayoutSizeWithType : ILayoutSize
	{
		bool IsCustomSize { get; set; }

		ObjectSizeType SupportSizeType { get; }
	}
}
