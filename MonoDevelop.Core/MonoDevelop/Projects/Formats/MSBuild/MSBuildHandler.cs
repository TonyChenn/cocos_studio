using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001B3 RID: 435
	public class MSBuildHandler : ISolutionItemHandler, IDisposable
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0003C908 File Offset: 0x0003AB08
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0003C910 File Offset: 0x0003AB10
		internal List<string> UnresolvedProjectDependencies { get; set; }

		// Token: 0x06001055 RID: 4181 RVA: 0x0003C919 File Offset: 0x0003AB19
		protected internal MSBuildHandler()
		{
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0003C921 File Offset: 0x0003AB21
		public MSBuildHandler(string typeGuid, string itemId)
		{
			this.Initialize(typeGuid, itemId);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0003C931 File Offset: 0x0003AB31
		internal void Initialize(string typeGuid, string itemId)
		{
			this.typeGuid = typeGuid;
			this.id = itemId;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0003C941 File Offset: 0x0003AB41
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x0003C949 File Offset: 0x0003AB49
		internal bool SavingSolution { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0003C952 File Offset: 0x0003AB52
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x0003C95A File Offset: 0x0003AB5A
		protected internal SolutionItem Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0003C963 File Offset: 0x0003AB63
		public virtual bool SyncFileName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0003C966 File Offset: 0x0003AB66
		public string TypeGuid
		{
			get
			{
				return this.typeGuid;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0003C96E File Offset: 0x0003AB6E
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x0003C976 File Offset: 0x0003AB76
		internal string[] SlnProjectContent
		{
			get
			{
				return this.slnProjectContent;
			}
			set
			{
				this.slnProjectContent = value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0003C980 File Offset: 0x0003AB80
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x0003C9C3 File Offset: 0x0003ABC3
		public string ItemId
		{
			get
			{
				if (this.id == null)
				{
					this.id = string.Format("{{{0}}}", Guid.NewGuid().ToString().ToUpper());
				}
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x0003C9CC File Offset: 0x0003ABCC
		// (set) Token: 0x06001063 RID: 4195 RVA: 0x0003C9D4 File Offset: 0x0003ABD4
		internal MSBuildFileFormat SolutionFormat { get; private set; }

		// Token: 0x06001064 RID: 4196 RVA: 0x0003C9DD File Offset: 0x0003ABDD
		internal virtual void SetSolutionFormat(MSBuildFileFormat format, bool converting)
		{
			this.SolutionFormat = format;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0003C9E6 File Offset: 0x0003ABE6
		public virtual BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0003C9F0 File Offset: 0x0003ABF0
		public void Save(IProgressMonitor monitor)
		{
			if (this.HasSlnData && !this.SavingSolution && this.Item.ParentSolution != null)
			{
				monitor.BeginTask(null, 2);
				this.SaveItem(monitor);
				monitor.Step(1);
				Solution parentSolution = this.Item.ParentSolution;
				this.SolutionFormat.SlnFileFormat.WriteFile(parentSolution.FileName, parentSolution, this.SolutionFormat, false, monitor);
				parentSolution.NeedsReload = false;
				monitor.EndTask();
				return;
			}
			this.SaveItem(monitor);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0003CA75 File Offset: 0x0003AC75
		protected virtual void SaveItem(IProgressMonitor monitor)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0003CA7C File Offset: 0x0003AC7C
		public virtual void OnModified(string hint)
		{
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0003CA7E File Offset: 0x0003AC7E
		public virtual void Dispose()
		{
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0003CA80 File Offset: 0x0003AC80
		public virtual bool HasSlnData
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0003CA83 File Offset: 0x0003AC83
		public virtual DataItem WriteSlnData()
		{
			return this.customSlnData;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0003CA8B File Offset: 0x0003AC8B
		public virtual void ReadSlnData(DataItem item)
		{
			this.customSlnData = item;
		}

		/// <summary>
		/// Gets a service instance of a given type
		/// </summary>
		/// <returns>
		/// The service.
		/// </returns>
		/// <param name="t">
		/// Type of the service
		/// </param>
		/// <remarks>
		/// This method looks for an imlpementation of a service of the given type.
		/// </remarks>
		// Token: 0x0600106D RID: 4205 RVA: 0x0003CA94 File Offset: 0x0003AC94
		public virtual object GetService(Type t)
		{
			return null;
		}

		// Token: 0x040004AE RID: 1198
		private SolutionItem item;

		// Token: 0x040004AF RID: 1199
		private string typeGuid;

		// Token: 0x040004B0 RID: 1200
		private string id;

		// Token: 0x040004B1 RID: 1201
		private string[] slnProjectContent;

		// Token: 0x040004B2 RID: 1202
		private DataItem customSlnData;
	}
}
