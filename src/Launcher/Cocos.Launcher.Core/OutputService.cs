using System;

namespace Cocos.Launcher.Core
{
	public class OutputService : IOutputService
	{
		public event Action<string> Output = delegate(string param0)
		{
		};

		public void Info(string info)
		{
			this.Output(info);
		}

		public static OutputService Instance
		{
			get
			{
				if (OutputService.instance == null)
				{
					OutputService.instance = new OutputService();
				}
				return OutputService.instance;
			}
		}

		private static OutputService instance;
	}
}
