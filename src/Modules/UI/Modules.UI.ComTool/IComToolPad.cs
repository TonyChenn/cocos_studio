using System;
using CocoStudio.Core;
using Modules.UI.ComTool.Model;

namespace Modules.UI.ComTool
{
	public interface IComToolPad : IService
	{
		IControlsViewFilter ControlsViewFilter { get; set; }
	}
}
