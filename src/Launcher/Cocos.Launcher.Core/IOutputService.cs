using System;

namespace Cocos.Launcher.Core
{
	public interface IOutputService
	{
		event Action<string> Output;

		void Info(string info);
	}
}
