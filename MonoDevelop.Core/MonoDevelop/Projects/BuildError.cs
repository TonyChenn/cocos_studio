using System;
using System.CodeDom.Compiler;
using System.Text;

namespace MonoDevelop.Projects
{
	// Token: 0x0200026F RID: 623
	public class BuildError
	{
		// Token: 0x06001671 RID: 5745 RVA: 0x0005A69C File Offset: 0x0005889C
		public BuildError() : this(string.Empty, 0, 0, string.Empty, string.Empty)
		{
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0005A6B5 File Offset: 0x000588B5
		public BuildError(string fileName, int line, int column, string errorNumber, string errorText)
		{
			this.FileName = fileName;
			this.Line = line;
			this.Column = column;
			this.ErrorNumber = errorNumber;
			this.ErrorText = errorText;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0005A6E4 File Offset: 0x000588E4
		public BuildError(CompilerError error)
		{
			this.FileName = error.FileName;
			this.Line = error.Line;
			this.Column = error.Column;
			this.ErrorNumber = error.ErrorNumber;
			this.ErrorText = error.ErrorText;
			this.IsWarning = error.IsWarning;
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0005A740 File Offset: 0x00058940
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this.FileName))
			{
				stringBuilder.Append(this.FileName);
				if (this.Line > 1)
				{
					stringBuilder.Append('(').Append(this.Line);
					if (this.Column > 1)
					{
						stringBuilder.Append(',').Append(this.Column);
					}
					stringBuilder.Append(')');
				}
				stringBuilder.Append(" : ");
			}
			if (this.IsWarning)
			{
				stringBuilder.Append("warning");
			}
			else
			{
				stringBuilder.Append("error");
			}
			if (!string.IsNullOrEmpty(this.ErrorNumber))
			{
				stringBuilder.Append(' ').Append(this.ErrorNumber);
			}
			stringBuilder.Append(": ").Append(this.ErrorText);
			return stringBuilder.ToString();
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x0005A81C File Offset: 0x00058A1C
		// (set) Token: 0x06001676 RID: 5750 RVA: 0x0005A824 File Offset: 0x00058A24
		public string FileName { get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x0005A82D File Offset: 0x00058A2D
		// (set) Token: 0x06001678 RID: 5752 RVA: 0x0005A835 File Offset: 0x00058A35
		public string Subcategory { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x0005A83E File Offset: 0x00058A3E
		// (set) Token: 0x0600167A RID: 5754 RVA: 0x0005A846 File Offset: 0x00058A46
		public bool IsWarning { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x0005A84F File Offset: 0x00058A4F
		// (set) Token: 0x0600167C RID: 5756 RVA: 0x0005A857 File Offset: 0x00058A57
		public string ErrorNumber { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600167D RID: 5757 RVA: 0x0005A860 File Offset: 0x00058A60
		// (set) Token: 0x0600167E RID: 5758 RVA: 0x0005A868 File Offset: 0x00058A68
		public string ErrorText { get; set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x0005A871 File Offset: 0x00058A71
		// (set) Token: 0x06001680 RID: 5760 RVA: 0x0005A879 File Offset: 0x00058A79
		public int Line { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x0005A882 File Offset: 0x00058A82
		// (set) Token: 0x06001682 RID: 5762 RVA: 0x0005A88A File Offset: 0x00058A8A
		public int Column { get; set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x0005A893 File Offset: 0x00058A93
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x0005A89B File Offset: 0x00058A9B
		public int EndLine { get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0005A8A4 File Offset: 0x00058AA4
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x0005A8AC File Offset: 0x00058AAC
		public int EndColumn { get; set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x0005A8B5 File Offset: 0x00058AB5
		// (set) Token: 0x06001688 RID: 5768 RVA: 0x0005A8BD File Offset: 0x00058ABD
		public IBuildTarget SourceTarget { get; set; }

		// Token: 0x06001689 RID: 5769 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		public static BuildError FromMSBuildErrorFormat(string lineText)
		{
			MSBuildErrorParser.Result result = MSBuildErrorParser.TryParseLine(lineText);
			if (result == null)
			{
				return null;
			}
			return new BuildError
			{
				FileName = (result.Origin ?? ""),
				Subcategory = result.Subcategory,
				IsWarning = !result.IsError,
				ErrorNumber = result.Code,
				ErrorText = result.Message,
				Line = result.Line,
				EndLine = result.EndLine,
				Column = result.Column,
				EndColumn = result.EndColumn
			};
		}
	}
}
