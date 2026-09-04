using System;
using System.Text;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	[Serializable]
	public class MSBuildTargetResult
	{
		public string ProjectFile { get; set; }

		public bool IsWarning { get; set; }

		public string Subcategory { get; set; }

		public string Code { get; set; }

		public string File { get; set; }

		public int LineNumber { get; set; }

		public int ColumnNumber { get; set; }

		public int EndLineNumber { get; set; }

		public int EndColumnNumber { get; set; }

		public string Message { get; set; }

		public string HelpKeyword { get; set; }

		public MSBuildTargetResult(string projectFile, bool isWarning, string subcategory, string code, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, string helpKeyword)
		{
			ProjectFile = projectFile;
			IsWarning = isWarning;
			Subcategory = subcategory;
			Code = code;
			File = file;
			LineNumber = lineNumber;
			ColumnNumber = columnNumber;
			EndLineNumber = endLineNumber;
			EndColumnNumber = endColumnNumber;
			Message = message;
			HelpKeyword = helpKeyword;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(File))
			{
				stringBuilder.Append(File);
				if (LineNumber > 0)
				{
					stringBuilder.Append("(");
					stringBuilder.Append(LineNumber);
					if (ColumnNumber > 0)
					{
						stringBuilder.Append(",");
						stringBuilder.Append(ColumnNumber);
						if (EndColumnNumber > 0)
						{
							if (EndLineNumber > 0)
							{
								stringBuilder.Append(",");
								stringBuilder.Append(EndLineNumber);
								stringBuilder.Append(",");
								stringBuilder.Append(EndColumnNumber);
							}
							else
							{
								stringBuilder.Append("-");
								stringBuilder.Append(EndColumnNumber);
							}
						}
					}
					else if (EndLineNumber > 0)
					{
						stringBuilder.Append("-");
						stringBuilder.Append(EndLineNumber);
					}
					stringBuilder.Append(")");
				}
				stringBuilder.Append(": ");
			}
			if (!string.IsNullOrEmpty(Subcategory))
			{
				stringBuilder.Append(Subcategory);
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(IsWarning ? "warning" : "error");
			if (!string.IsNullOrEmpty(Code))
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(Code);
			}
			stringBuilder.Append(": ");
			stringBuilder.Append(Message);
			return stringBuilder.ToString();
		}
	}
}
