using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.DataModel
{
	public class GameFileLoadResult
	{
		public AbstractNodeObject RootObject { get; set; }

		public TimelineAction TimelineAction { get; set; }

		public HashSet<string> Names { get; set; }

		public GameFileLoadResult()
		{
			this.Names = new HashSet<string>();
		}
	}
}
