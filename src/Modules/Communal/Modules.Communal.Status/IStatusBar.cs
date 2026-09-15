using System;
using CocoStudio.Core;

namespace Modules.Communal.Status
{
	public interface IStatusBar : IService
	{
		void ShowWarningInfo(IStatusWarningInfo info);

		void HideWarningInfo();
	}
}
