using System;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007B RID: 123
	public class UnknowCodeFile : CodeFile
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000CE3E File Offset: 0x0000B03E
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000CE46 File Offset: 0x0000B046
		public string Type { get; private set; }

		// Token: 0x060003A2 RID: 930 RVA: 0x0000CE4F File Offset: 0x0000B04F
		protected override DataError OnCheckDataError()
		{
			return new DataError("Unsupport resource type, the type is " + this.Type);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000CE66 File Offset: 0x0000B066
		public override ResourceData GetResourceData()
		{
			return null;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000CE69 File Offset: 0x0000B069
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000CE71 File Offset: 0x0000B071
		internal DataItem DataItem { get; private set; }

		// Token: 0x060003A6 RID: 934 RVA: 0x0000CE7C File Offset: 0x0000B07C
		public UnknowCodeFile(FilePath baseDirectory, DataItem dataItem)
		{
			this.DataItem = dataItem;
			this.Type = dataItem.Name;
			if (dataItem.HasItemData)
			{
				DataValue dataValue = dataItem.ItemData["Name"] as DataValue;
				this.fileName = baseDirectory.Combine(new string[]
				{
					dataValue.Value
				});
			}
		}
	}
}
