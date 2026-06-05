using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoDevelop.Projects
{
	// Token: 0x02000174 RID: 372
	public class BuildResult
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x00035BA0 File Offset: 0x00033DA0
		public BuildResult()
		{
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00035BBA File Offset: 0x00033DBA
		public BuildResult(string compilerOutput, int buildCount, int failedBuildCount)
		{
			this.CompilerOutput = compilerOutput;
			this.buildCount = buildCount;
			this.FailedBuildCount = failedBuildCount;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00035BEC File Offset: 0x00033DEC
		public BuildResult(CompilerResults compilerResults, string compilerOutput)
		{
			this.CompilerOutput = compilerOutput;
			if (compilerResults != null)
			{
				foreach (object obj in compilerResults.Errors)
				{
					CompilerError error = (CompilerError)obj;
					this.Append(new BuildError(error));
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00035C70 File Offset: 0x00033E70
		public ReadOnlyCollection<BuildError> Errors
		{
			get
			{
				return this.errors.AsReadOnly();
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00035C80 File Offset: 0x00033E80
		public void ClearErrors()
		{
			this.errors.Clear();
			this.warningCount = (this.errorCount = 0);
			this.buildCount = 1;
			this.FailedBuildCount = 0;
			this.CompilerOutput = "";
			this.sourceTarget = null;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00035CC8 File Offset: 0x00033EC8
		public void AddError(string file, int line, int col, string errorNum, string text)
		{
			this.Append(new BuildError(file, line, col, errorNum, text));
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00035CDD File Offset: 0x00033EDD
		public void AddError(string text)
		{
			this.Append(new BuildError(null, 0, 0, null, text));
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00035CF0 File Offset: 0x00033EF0
		public void AddError(string text, string file)
		{
			this.Append(new BuildError(file, 0, 0, null, text));
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00035D04 File Offset: 0x00033F04
		public void AddWarning(string file, int line, int col, string errorNum, string text)
		{
			this.Append(new BuildError(file, line, col, errorNum, text)
			{
				IsWarning = true
			});
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00035D2D File Offset: 0x00033F2D
		public void AddWarning(string text)
		{
			this.AddWarning(text, null);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00035D37 File Offset: 0x00033F37
		public void AddWarning(string text, string file)
		{
			this.AddWarning(file, 0, 0, null, text);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00035D44 File Offset: 0x00033F44
		public BuildResult Append(BuildResult res)
		{
			if (res == null)
			{
				return this;
			}
			this.errors.AddRange(res.Errors);
			this.warningCount += res.WarningCount;
			this.errorCount += res.ErrorCount;
			this.buildCount += res.BuildCount;
			this.FailedBuildCount += res.FailedBuildCount;
			if (!string.IsNullOrEmpty(res.CompilerOutput))
			{
				this.CompilerOutput = this.CompilerOutput + "\n" + res.CompilerOutput;
			}
			return this;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00035DE0 File Offset: 0x00033FE0
		public BuildResult Append(IEnumerable<BuildResult> results)
		{
			foreach (BuildResult res in results)
			{
				this.Append(res);
			}
			return this;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00035E2C File Offset: 0x0003402C
		public BuildResult Append(BuildError error)
		{
			if (error == null)
			{
				return this;
			}
			this.errors.Add(error);
			if (this.sourceTarget != null && error.SourceTarget == null)
			{
				error.SourceTarget = this.sourceTarget;
			}
			if (error.IsWarning)
			{
				this.warningCount++;
			}
			else
			{
				this.errorCount++;
				if (this.FailedBuildCount == 0)
				{
					this.FailedBuildCount = 1;
				}
			}
			return this;
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x00035E9C File Offset: 0x0003409C
		// (set) Token: 0x06000E9E RID: 3742 RVA: 0x00035EA4 File Offset: 0x000340A4
		public string CompilerOutput { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00035EAD File Offset: 0x000340AD
		public int WarningCount
		{
			get
			{
				return this.warningCount;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00035EB5 File Offset: 0x000340B5
		public int ErrorCount
		{
			get
			{
				return this.errorCount;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00035EBD File Offset: 0x000340BD
		// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x00035EC5 File Offset: 0x000340C5
		public int BuildCount
		{
			get
			{
				return this.buildCount;
			}
			set
			{
				this.buildCount = value;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00035ECE File Offset: 0x000340CE
		// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x00035ED6 File Offset: 0x000340D6
		public int FailedBuildCount { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x00035EDF File Offset: 0x000340DF
		public bool Failed
		{
			get
			{
				return this.FailedBuildCount > 0;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00035EEA File Offset: 0x000340EA
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x00035EF4 File Offset: 0x000340F4
		public IBuildTarget SourceTarget
		{
			get
			{
				return this.sourceTarget;
			}
			set
			{
				this.sourceTarget = value;
				if (this.sourceTarget != null)
				{
					foreach (BuildError buildError in this.Errors)
					{
						if (buildError.SourceTarget == null)
						{
							buildError.SourceTarget = this.sourceTarget;
						}
					}
				}
			}
		}

		// Token: 0x0400042C RID: 1068
		private int warningCount;

		// Token: 0x0400042D RID: 1069
		private int errorCount;

		// Token: 0x0400042E RID: 1070
		private int buildCount = 1;

		// Token: 0x0400042F RID: 1071
		private List<BuildError> errors = new List<BuildError>();

		// Token: 0x04000430 RID: 1072
		private IBuildTarget sourceTarget;
	}
}
