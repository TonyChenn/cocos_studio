using System;

namespace Modules.Communal.CocosAdapter
{
	[Flags]
	public enum EnumPlatform
	{
		Unknown = -1,
		None = 0,
		Android = 1,
		iOS = 2,
		Web = 4,
		Windows = 8,
		Mac = 16,
		Simulator = 32
	}
}
