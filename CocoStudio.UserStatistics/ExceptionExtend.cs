using System;
using System.Collections.Generic;
using System.Text;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000009 RID: 9
	public static class ExceptionExtend
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002300 File Offset: 0x00000500
		public static string GetAllStackTrace(this Exception ex)
		{
			Stack<Exception> stack = new Stack<Exception>();
			for (Exception ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
			{
				stack.Push(ex2);
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (stack.Count > 0)
			{
				Exception ex2 = stack.Pop();
				stringBuilder.Append(ex2.Message);
				stringBuilder.AppendLine();
				stringBuilder.Append(ex2.StackTrace);
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
