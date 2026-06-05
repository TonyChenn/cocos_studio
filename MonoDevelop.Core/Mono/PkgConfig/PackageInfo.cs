using System;
using System.Collections.Generic;

namespace Mono.PkgConfig
{
	// Token: 0x020000BA RID: 186
	internal class PackageInfo
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00018940 File Offset: 0x00016B40
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00018948 File Offset: 0x00016B48
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00018951 File Offset: 0x00016B51
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00018959 File Offset: 0x00016B59
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00018962 File Offset: 0x00016B62
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0001896A File Offset: 0x00016B6A
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00018973 File Offset: 0x00016B73
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x0001897B File Offset: 0x00016B7B
		public string Requires
		{
			get
			{
				return this.requires;
			}
			set
			{
				this.requires = value;
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00018984 File Offset: 0x00016B84
		public string GetData(string name)
		{
			if (this.customData == null)
			{
				return null;
			}
			string result;
			this.customData.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000189AB File Offset: 0x00016BAB
		public void SetData(string name, string value)
		{
			if (this.customData == null)
			{
				this.customData = new Dictionary<string, string>();
			}
			this.customData[name] = value;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000189CD File Offset: 0x00016BCD
		public void RemoveData(string name)
		{
			if (this.customData != null)
			{
				this.customData.Remove(name);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x000189E4 File Offset: 0x00016BE4
		internal Dictionary<string, string> CustomData
		{
			get
			{
				return this.customData;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x000189EC File Offset: 0x00016BEC
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x000189F4 File Offset: 0x00016BF4
		internal DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
			set
			{
				this.lastWriteTime = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x000189FD File Offset: 0x00016BFD
		internal bool HasCustomData
		{
			get
			{
				return this.customData != null && this.customData.Count > 0;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00018A17 File Offset: 0x00016C17
		protected internal virtual bool IsValidPackage
		{
			get
			{
				return this.HasCustomData;
			}
		}

		// Token: 0x04000220 RID: 544
		private Dictionary<string, string> customData;

		// Token: 0x04000221 RID: 545
		private DateTime lastWriteTime;

		// Token: 0x04000222 RID: 546
		private string name;

		// Token: 0x04000223 RID: 547
		private string version;

		// Token: 0x04000224 RID: 548
		private string description;

		// Token: 0x04000225 RID: 549
		private string requires;
	}
}
