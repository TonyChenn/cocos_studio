using System;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007C RID: 124
	internal class UnknowResourceItem : ResourceItem
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000CEDE File Offset: 0x0000B0DE
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000CEEB File Offset: 0x0000B0EB
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000CEF3 File Offset: 0x0000B0F3
		public string Type { get; private set; }

		// Token: 0x060003AA RID: 938 RVA: 0x0000CEFC File Offset: 0x0000B0FC
		protected override DataError OnCheckDataError()
		{
			return new DataError("Unsupport resource type, the type is " + this.Type);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000CF13 File Offset: 0x0000B113
		public override ResourceData GetResourceData()
		{
			return null;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000CF16 File Offset: 0x0000B116
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000CF1E File Offset: 0x0000B11E
		internal DataItem DataItem { get; private set; }

		// Token: 0x060003AE RID: 942 RVA: 0x0000CF28 File Offset: 0x0000B128
		public UnknowResourceItem(FilePath baseDirectory, DataItem dataItem)
		{
			this.DataItem = dataItem;
			this.Type = dataItem.Name;
			if (dataItem.HasItemData)
			{
				DataValue dataValue = dataItem.ItemData["Name"] as DataValue;
				this.Name = dataValue.Value;
				this.fullPath = baseDirectory.Combine(new string[]
				{
					this.Name
				});
			}
		}

		// Token: 0x040000EE RID: 238
		private FilePath fullPath;
	}
}
