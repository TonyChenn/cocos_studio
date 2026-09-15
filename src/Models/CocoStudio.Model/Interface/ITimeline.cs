using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Interface
{
	public interface ITimeline
	{
		FrameCollection Frames { get; }
	}
}
