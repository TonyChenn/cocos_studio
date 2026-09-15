using System;

namespace CocoStudio.UserStatistics
{
	public interface IUserStatistics
	{
		EditorInfo EditorInfo { get; set; }

		void UnHandledException(Exception ex, string feedbackInfo);

		void Exit();

		void ExitAll();
	}
}
