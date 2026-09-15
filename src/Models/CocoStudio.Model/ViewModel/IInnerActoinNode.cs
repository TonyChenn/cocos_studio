using System;

namespace CocoStudio.Model.ViewModel
{
	public interface IInnerActoinNode
	{
		void ApplyActionValue(InnerActionValue actionValue);

		void ApplyStep(InnerActionValue actionValue, int frameIndex);

		InnerActionValue ActionValue { get; set; }
	}
}
