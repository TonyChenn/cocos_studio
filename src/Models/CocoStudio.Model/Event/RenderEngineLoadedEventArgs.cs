using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	public class RenderEngineLoadedEventArgs
	{
		public GameWindow GameWindow { get; private set; }

		public RenderEngineLoadedEventArgs(GameWindow gameWindow)
		{
			this.GameWindow = gameWindow;
		}
	}
}
