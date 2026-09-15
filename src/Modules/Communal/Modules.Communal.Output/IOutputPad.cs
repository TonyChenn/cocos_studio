using System;
using CocoStudio.Core;

namespace Modules.Communal.Output
{
	public interface IOutputPad : IService
	{
		void Clear();

		void ScrollToEnd();
	}
}
