using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D7 RID: 471
	public class RemoteProjectBuilder : IDisposable
	{
		// Token: 0x060011F7 RID: 4599 RVA: 0x000493CC File Offset: 0x000475CC
		internal RemoteProjectBuilder(string file, RemoteBuildEngine engine)
		{
			this.file = file;
			this.engine = engine;
			this.builder = engine.LoadProject(file);
			this.referenceCache = new Dictionary<string, string[]>();
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x060011F8 RID: 4600 RVA: 0x000493FC File Offset: 0x000475FC
		// (remove) Token: 0x060011F9 RID: 4601 RVA: 0x00049434 File Offset: 0x00047634
		public event EventHandler Disconnected;

		// Token: 0x060011FA RID: 4602 RVA: 0x00049469 File Offset: 0x00047669
		private void CheckDisconnected()
		{
			if (this.engine.CheckDisconnected() && this.Disconnected != null)
			{
				this.Disconnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00049494 File Offset: 0x00047694
		public MSBuildResult Run(ProjectConfigurationInfo[] configurations, ILogWriter logWriter, MSBuildVerbosity verbosity, string[] runTargets, string[] evaluateItems, string[] evaluateProperties)
		{
			MSBuildResult result;
			try
			{
				result = this.builder.Run(configurations, logWriter, verbosity, runTargets, evaluateItems, evaluateProperties);
			}
			catch (Exception ex)
			{
				this.CheckDisconnected();
				LoggingService.LogError("RunTarget failed", ex);
				MSBuildTargetResult msbuildTargetResult = new MSBuildTargetResult(this.file, false, "", "", this.file, 1, 1, 1, 1, "Unknown MSBuild failure. Please try building the project again", "");
				MSBuildResult msbuildResult = new MSBuildResult(new MSBuildTargetResult[]
				{
					msbuildTargetResult
				});
				result = msbuildResult;
			}
			return result;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00049528 File Offset: 0x00047728
		public string[] ResolveAssemblyReferences(ProjectConfigurationInfo[] configurations)
		{
			string[] array = null;
			string key = configurations[0].Configuration + "|" + configurations[0].Platform;
			lock (this.referenceCache)
			{
				if (!this.referenceCache.TryGetValue(key, out array))
				{
					MSBuildResult msbuildResult;
					try
					{
						msbuildResult = this.builder.Run(configurations, null, MSBuildVerbosity.Normal, new string[]
						{
							"ResolveAssemblyReferences"
						}, new string[]
						{
							"ReferencePath"
						}, null);
					}
					catch (Exception ex)
					{
						this.CheckDisconnected();
						LoggingService.LogError("ResolveAssemblyReferences failed", ex);
						return new string[0];
					}
					List<MSBuildEvaluatedItem> list;
					if (msbuildResult.Items.TryGetValue("ReferencePath", out list) && list != null)
					{
						array = (from i in list
						select i.ItemSpec).ToArray<string>();
					}
					else
					{
						array = new string[0];
					}
					this.referenceCache[key] = array;
				}
			}
			return array;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00049654 File Offset: 0x00047854
		public void Refresh()
		{
			lock (this.referenceCache)
			{
				this.referenceCache.Clear();
			}
			try
			{
				this.builder.Refresh();
			}
			catch (Exception ex)
			{
				LoggingService.LogError("MSBuild refresh failed", ex);
				this.CheckDisconnected();
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000496C8 File Offset: 0x000478C8
		public void RefreshWithContent(string projectContent)
		{
			lock (this.referenceCache)
			{
				this.referenceCache.Clear();
			}
			try
			{
				this.builder.RefreshWithContent(projectContent);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("MSBuild refresh failed", ex);
				this.CheckDisconnected();
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0004973C File Offset: 0x0004793C
		public void Dispose()
		{
			if (!MSBuildProjectService.ShutDown && this.engine != null)
			{
				try
				{
					if (this.builder != null)
					{
						this.engine.UnloadProject(this.builder);
					}
					MSBuildProjectService.ReleaseProjectBuilder(this.engine);
				}
				catch
				{
				}
				GC.SuppressFinalize(this);
				this.engine = null;
				this.builder = null;
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000497A8 File Offset: 0x000479A8
		~RemoteProjectBuilder()
		{
			this.Dispose();
		}

		// Token: 0x04000529 RID: 1321
		private RemoteBuildEngine engine;

		// Token: 0x0400052A RID: 1322
		private IProjectBuilder builder;

		// Token: 0x0400052B RID: 1323
		private Dictionary<string, string[]> referenceCache;

		// Token: 0x0400052C RID: 1324
		private string file;
	}
}
