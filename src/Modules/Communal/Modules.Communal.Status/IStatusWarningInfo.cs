using System;

namespace Modules.Communal.Status
{
	public interface IStatusWarningInfo
	{
		string Tooltip { get; }

		void OnClick();
	}
}
