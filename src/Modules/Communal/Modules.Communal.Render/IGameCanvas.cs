using System;
using CocoStudio.Core;

namespace Modules.Communal.Render
{
	public interface IGameCanvas : IService
	{
		bool IsRecordAnimation { get; set; }
	}
}
