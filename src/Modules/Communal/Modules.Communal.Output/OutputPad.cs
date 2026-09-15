using System;
using CocoStudio.Core;
using Modules.Communal.Output.View;

namespace Modules.Communal.Output
{
	public class OutputPad : DefaultPadContent
	{
		public OutputPad() : base(new OutputPadWidget())
		{
		}
	}
}
