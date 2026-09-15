using System;

namespace CocoStudio.UserStatistics
{
	public class BaseUserStatistics : IUserStatistics
	{
		public EditorInfo EditorInfo
		{
			get
			{
				return this.editorInfo;
			}
			set
			{
				this.editorInfo = value;
				this.Init();
			}
		}

		protected BaseUserStatistics()
		{
		}

		private void Init()
		{
			try
			{
				this.OnInit();
			}
			catch (Exception)
			{
			}
		}

		protected virtual void OnExit()
		{
			this.m_time = (double)Environment.TickCount - this.m_time;
		}

		protected virtual void OnInit()
		{
			this.m_time = (double)Environment.TickCount;
		}

		protected bool IsStartFromLaunch()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			return commandLineArgs != null && commandLineArgs.Length > 0 && commandLineArgs[0].Contains("Start By Launch");
		}

		public void Exit()
		{
			this.OnExit();
		}

		public virtual void ExitAll()
		{
		}

		public virtual void UnHandledException(Exception ex, string feedbackInfo)
		{
		}

		protected double m_time = 0.0;

		private EditorInfo editorInfo;
	}
}
