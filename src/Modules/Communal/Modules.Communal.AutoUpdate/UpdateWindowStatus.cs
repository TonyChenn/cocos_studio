using System;

namespace Modules.Communal.AutoUpdate
{
	internal enum UpdateWindowStatus
	{
		Init,
		Downloading,
		Finished,
		Failed,
		LowVersion
	}
}
