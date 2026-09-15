using System;

namespace CocoStudio.Model
{
	public interface IPlayControl
	{
		bool HasData();

		bool IsPlaying { get; set; }
	}
}
