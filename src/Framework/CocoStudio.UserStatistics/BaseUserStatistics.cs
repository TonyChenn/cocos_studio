using System;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000007 RID: 7
	public class BaseUserStatistics : IUserStatistics
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000021B8 File Offset: 0x000003B8
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000021D0 File Offset: 0x000003D0
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

		// Token: 0x0600000F RID: 15 RVA: 0x000021E1 File Offset: 0x000003E1
		protected BaseUserStatistics()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021FC File Offset: 0x000003FC
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

		// Token: 0x06000011 RID: 17 RVA: 0x0000222C File Offset: 0x0000042C
		protected virtual void OnExit()
		{
			this.m_time = (double)Environment.TickCount - this.m_time;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002242 File Offset: 0x00000442
		protected virtual void OnInit()
		{
			this.m_time = (double)Environment.TickCount;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002254 File Offset: 0x00000454
		protected bool IsStartFromLaunch()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			return commandLineArgs != null && commandLineArgs.Length > 0 && commandLineArgs[0].Contains("Start By Launch");
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002294 File Offset: 0x00000494
		public void Exit()
		{
			this.OnExit();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000229E File Offset: 0x0000049E
		public virtual void ExitAll()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022A1 File Offset: 0x000004A1
		public virtual void UnHandledException(Exception ex, string feedbackInfo)
		{
		}

		// Token: 0x04000007 RID: 7
		protected double m_time = 0.0;

		// Token: 0x04000008 RID: 8
		private EditorInfo editorInfo;
	}
}
