using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x0200024B RID: 587
	internal class AssertLoggingTraceListener : TraceListener
	{
		// Token: 0x06001593 RID: 5523 RVA: 0x000578BA File Offset: 0x00055ABA
		public override void Write(string message)
		{
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000578BC File Offset: 0x00055ABC
		public override void WriteLine(string message)
		{
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x000578BE File Offset: 0x00055ABE
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x000578C0 File Offset: 0x00055AC0
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000578C4 File Offset: 0x00055AC4
		public override void Fail(string message, string detailMessage)
		{
			StackFrame[] frames = new StackTrace(1, true).GetFrames();
			int num = 0;
			while (num < frames.Length && AssertLoggingTraceListener.IsInfrastructureMethod(frames[num]))
			{
				num++;
			}
			if (num == frames.Length - 1)
			{
				num = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (AssertLoggingTraceListener.IsRealMessage(message))
			{
				if (!string.IsNullOrEmpty(detailMessage))
				{
					stringBuilder.AppendFormat("Failed assertion: {0} - {1}", message, detailMessage);
				}
				else
				{
					stringBuilder.AppendFormat("Failed assertion: {0}", message);
				}
			}
			else
			{
				stringBuilder.Append("Failed assertion at ");
				AssertLoggingTraceListener.FormatStackFrame(stringBuilder, frames[num]);
				num++;
			}
			stringBuilder.Append("\n");
			AssertLoggingTraceListener.FormatStackTrace(stringBuilder, frames, num);
			LoggingService.LogError(stringBuilder.ToString());
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0005796D File Offset: 0x00055B6D
		private static bool IsRealMessage(string message)
		{
			return !string.IsNullOrEmpty(message) && !message.StartsWith("   at System.Diagnostics.TraceImpl.Assert", StringComparison.Ordinal);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0005798C File Offset: 0x00055B8C
		private static bool IsInfrastructureMethod(StackFrame frame)
		{
			MethodBase method = frame.GetMethod();
			if (method == null)
			{
				return true;
			}
			string fullName = method.DeclaringType.Assembly.FullName;
			return fullName == AssertLoggingTraceListener.mscorlibName || fullName == AssertLoggingTraceListener.systemName;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000579D8 File Offset: 0x00055BD8
		private static void FormatStackTrace(StringBuilder sb, StackFrame[] frames, int startIndex = 0)
		{
			for (int i = startIndex; i < frames.Length; i++)
			{
				StackFrame frame = frames[i];
				if (i > startIndex)
				{
					sb.Append("\n");
				}
				sb.Append("   at ");
				AssertLoggingTraceListener.FormatStackFrame(sb, frame);
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00057A1C File Offset: 0x00055C1C
		private static void FormatStackFrame(StringBuilder sb, StackFrame frame)
		{
			MethodBase method = frame.GetMethod();
			if (method != null)
			{
				sb.AppendFormat("{0}.{1}", method.DeclaringType.FullName, method.Name);
				sb.Append("(");
				ParameterInfo[] parameters = method.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					if (i > 0)
					{
						sb.Append(", ");
					}
					Type type = parameters[i].ParameterType;
					bool isByRef = type.IsByRef;
					if (isByRef)
					{
						type = type.GetElementType();
					}
					if (type.IsClass && type.Namespace != string.Empty)
					{
						sb.Append(type.Namespace);
						sb.Append(".");
					}
					sb.Append(type.Name);
					if (isByRef)
					{
						sb.Append(" ByRef");
					}
					sb.AppendFormat(" {0}", parameters[i].Name);
				}
				sb.Append(")");
			}
			else
			{
				sb.Append("<unknown method>");
			}
			string fileName = frame.GetFileName();
			if (!string.IsNullOrEmpty(fileName) && fileName != "<filename unknown>")
			{
				sb.AppendFormat(" in {0}:line {1}", fileName, frame.GetFileLineNumber());
			}
		}

		// Token: 0x04000685 RID: 1669
		private static readonly string mscorlibName = typeof(int).Assembly.FullName;

		// Token: 0x04000686 RID: 1670
		private static readonly string systemName = typeof(TraceListener).Assembly.FullName;
	}
}
