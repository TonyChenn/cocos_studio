using System;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	internal class UnknowResourceItem : ResourceItem
	{
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		public string Type { get; private set; }

		protected override DataError OnCheckDataError()
		{
			return new DataError("Unsupport resource type, the type is " + this.Type);
		}

		public override ResourceData GetResourceData()
		{
			return null;
		}

		internal DataItem DataItem { get; private set; }

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

		private FilePath fullPath;
	}
}
