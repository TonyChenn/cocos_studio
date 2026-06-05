using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200015F RID: 351
	internal class FileStatusTracker<TEventArgs> where TEventArgs : EventArgs
	{
		// Token: 0x06000D46 RID: 3398 RVA: 0x0003081C File Offset: 0x0002EA1C
		public FileStatusTracker(IWorkspaceFileObject item, Action<TEventArgs> onReloadRequired, TEventArgs eventArgs)
		{
			this.item = item;
			this.eventArgs = eventArgs;
			this.lastSaveTime = new Dictionary<string, DateTime>();
			this.reloadCheckTime = new Dictionary<string, DateTime>();
			this.savingFlag = false;
			this.reloadRequired = null;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00030856 File Offset: 0x0002EA56
		public void BeginSave()
		{
			this.savingFlag = true;
			this.DisposeWatchers();
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00030865 File Offset: 0x0002EA65
		public void EndSave()
		{
			this.ResetLoadTimes();
			this.savingFlag = false;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00030874 File Offset: 0x0002EA74
		public void ResetLoadTimes()
		{
			this.lastSaveTime.Clear();
			this.reloadCheckTime.Clear();
			foreach (FilePath filePath in this.item.GetItemFiles(false))
			{
				this.lastSaveTime[filePath] = (this.reloadCheckTime[filePath] = this.GetLastWriteTime(filePath));
			}
			if (this.reloadRequired != null)
			{
				this.InternalNeedsReload();
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00030918 File Offset: 0x0002EB18
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x0003092C File Offset: 0x0002EB2C
		public bool NeedsReload
		{
			get
			{
				return !this.savingFlag && this.InternalNeedsReload();
			}
			set
			{
				if (value)
				{
					this.reloadCheckTime.Clear();
					using (List<FilePath>.Enumerator enumerator = this.item.GetItemFiles(false).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							FilePath filePath = enumerator.Current;
							this.reloadCheckTime[filePath] = DateTime.MinValue;
						}
						return;
					}
				}
				this.ResetLoadTimes();
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x000309A8 File Offset: 0x0002EBA8
		public bool ItemFilesChanged
		{
			get
			{
				if (this.savingFlag)
				{
					return false;
				}
				foreach (FilePath file in this.item.GetItemFiles(false))
				{
					if (this.GetLastSaveTime(file) != this.GetLastWriteTime(file))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00030A20 File Offset: 0x0002EC20
		private bool InternalNeedsReload()
		{
			foreach (FilePath file in this.item.GetItemFiles(false))
			{
				if (this.GetLastReloadCheckTime(file) != this.GetLastWriteTime(file))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00030A90 File Offset: 0x0002EC90
		private void DisposeWatchers()
		{
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00030A94 File Offset: 0x0002EC94
		private DateTime GetLastWriteTime(FilePath file)
		{
			try
			{
				if (!file.IsNullOrEmpty && File.Exists(file))
				{
					return File.GetLastWriteTime(file);
				}
			}
			catch
			{
			}
			return this.GetLastSaveTime(file);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00030AE4 File Offset: 0x0002ECE4
		private DateTime GetLastSaveTime(FilePath file)
		{
			DateTime result;
			if (this.lastSaveTime.TryGetValue(file, out result))
			{
				return result;
			}
			return DateTime.MinValue;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00030B10 File Offset: 0x0002ED10
		private DateTime GetLastReloadCheckTime(FilePath file)
		{
			DateTime result;
			if (this.reloadCheckTime.TryGetValue(file, out result))
			{
				return result;
			}
			return DateTime.MinValue;
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000D52 RID: 3410 RVA: 0x00030B39 File Offset: 0x0002ED39
		// (remove) Token: 0x06000D53 RID: 3411 RVA: 0x00030B67 File Offset: 0x0002ED67
		public event EventHandler<TEventArgs> ReloadRequired
		{
			add
			{
				this.reloadRequired = (EventHandler<TEventArgs>)Delegate.Combine(this.reloadRequired, value);
				if (this.InternalNeedsReload())
				{
					value(this, this.eventArgs);
				}
			}
			remove
			{
				this.reloadRequired = (EventHandler<TEventArgs>)Delegate.Remove(this.reloadRequired, value);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00030B80 File Offset: 0x0002ED80
		public void FireReloadRequired(TEventArgs args)
		{
			if (this.reloadRequired != null)
			{
				this.reloadRequired(this, args);
			}
		}

		// Token: 0x040003E4 RID: 996
		private Dictionary<string, DateTime> lastSaveTime;

		// Token: 0x040003E5 RID: 997
		private Dictionary<string, DateTime> reloadCheckTime;

		// Token: 0x040003E6 RID: 998
		private bool savingFlag;

		// Token: 0x040003E7 RID: 999
		private TEventArgs eventArgs;

		// Token: 0x040003E8 RID: 1000
		private EventHandler<TEventArgs> reloadRequired;

		// Token: 0x040003E9 RID: 1001
		private IWorkspaceFileObject item;
	}
}
