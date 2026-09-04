using System;
using System.Collections;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000083 RID: 131
	public abstract class SolutionItem : IExtendedDataItem, ILoadController, IPublish
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000DACB File Offset: 0x0000BCCB
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000418 RID: 1048
		// (set) Token: 0x06000419 RID: 1049
		public abstract string Name { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000DAE6 File Offset: 0x0000BCE6
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x0000DAEE File Offset: 0x0000BCEE
		public Solution ParentSolution { get; internal set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000DAF7 File Offset: 0x0000BCF7
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0000DAFF File Offset: 0x0000BCFF
		public SolutionFolder ParentFolder { get; internal set; }

		// Token: 0x0600041E RID: 1054 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			try
			{
				this.OnPublish(monitor, info);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Run publish failed.", exception);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000DB44 File Offset: 0x0000BD44
		protected virtual void OnPublish(IProgressMonitor monitor, PublishInfo info)
		{
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000DB46 File Offset: 0x0000BD46
		void ILoadController.BeginLoad()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000DB4D File Offset: 0x0000BD4D
		void ILoadController.EndLoad()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400011A RID: 282
		private Hashtable extendedProperties;
	}
}
