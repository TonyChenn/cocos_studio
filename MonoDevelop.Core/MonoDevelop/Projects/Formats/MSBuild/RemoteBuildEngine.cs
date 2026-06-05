using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001D6 RID: 470
	internal class RemoteBuildEngine : IBuildEngine, IDisposable
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x0004915A File Offset: 0x0004735A
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00049162 File Offset: 0x00047362
		public int ReferenceCount { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x0004916B File Offset: 0x0004736B
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00049173 File Offset: 0x00047373
		public DateTime ReleaseTime { get; set; }

		// Token: 0x060011EC RID: 4588 RVA: 0x0004917C File Offset: 0x0004737C
		public RemoteBuildEngine(Process proc, IBuildEngine engine)
		{
			this.proc = proc;
			this.engine = engine;
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x060011ED RID: 4589 RVA: 0x0004919C File Offset: 0x0004739C
		// (remove) Token: 0x060011EE RID: 4590 RVA: 0x000491D4 File Offset: 0x000473D4
		public event EventHandler Disconnected;

		// Token: 0x060011EF RID: 4591 RVA: 0x0004920C File Offset: 0x0004740C
		public IProjectBuilder LoadProject(string projectFile)
		{
			IProjectBuilder result;
			try
			{
				result = this.engine.LoadProject(projectFile);
			}
			catch (Exception)
			{
				this.CheckDisconnected();
				throw;
			}
			return result;
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00049244 File Offset: 0x00047444
		public void UnloadProject(IProjectBuilder pb)
		{
			try
			{
				this.engine.UnloadProject(pb);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Project unloading failed", ex);
				if (!this.CheckDisconnected())
				{
					throw;
				}
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00049288 File Offset: 0x00047488
		public void SetCulture(CultureInfo uiCulture)
		{
			try
			{
				this.engine.SetCulture(uiCulture);
			}
			catch (Exception)
			{
				this.CheckDisconnected();
				throw;
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x000492C0 File Offset: 0x000474C0
		public void SetGlobalProperties(IDictionary<string, string> properties)
		{
			try
			{
				this.engine.SetGlobalProperties(properties);
			}
			catch (Exception)
			{
				this.CheckDisconnected();
				throw;
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x000492F8 File Offset: 0x000474F8
		void IBuildEngine.Ping()
		{
			this.engine.Ping();
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00049308 File Offset: 0x00047508
		private bool CheckAlive()
		{
			if (!this.alive)
			{
				return false;
			}
			bool result;
			try
			{
				this.engine.Ping();
				result = true;
			}
			catch
			{
				this.alive = false;
				result = false;
			}
			return result;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004934C File Offset: 0x0004754C
		internal bool CheckDisconnected()
		{
			if (!this.CheckAlive())
			{
				if (this.Disconnected != null)
				{
					this.Disconnected(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00049374 File Offset: 0x00047574
		public void Dispose()
		{
			try
			{
				this.alive = false;
				if (this.proc != null)
				{
					try
					{
						this.proc.Kill();
						goto IL_2A;
					}
					catch
					{
						goto IL_2A;
					}
				}
				this.engine.Dispose();
				IL_2A:;
			}
			catch
			{
			}
		}

		// Token: 0x04000523 RID: 1315
		private IBuildEngine engine;

		// Token: 0x04000524 RID: 1316
		private Process proc;

		// Token: 0x04000525 RID: 1317
		private bool alive = true;
	}
}
