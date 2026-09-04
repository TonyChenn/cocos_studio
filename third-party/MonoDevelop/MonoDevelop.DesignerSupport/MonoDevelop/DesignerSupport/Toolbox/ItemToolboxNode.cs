using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Xwt.Drawing;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	[DataItem(Name = "item", FallbackType = typeof(UnknownToolboxNode))]
	public abstract class ItemToolboxNode : ICustomDataItem, IComparable, IComparable<ItemToolboxNode>
	{
		[NonSerialized]
		private Xwt.Drawing.Image icon;

		[ItemProperty("name")]
		private string name = "";

		[ItemProperty("category")]
		private string category = "";

		[ItemProperty("description")]
		private string description = "";

		private List<ToolboxItemFilterAttribute> itemFilters = new List<ToolboxItemFilterAttribute>();

		[Browsable(false)]
		public virtual Xwt.Drawing.Image Icon
		{
			get
			{
				return icon;
			}
			set
			{
				icon = value;
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		[ReadOnly(true)]
		public virtual string Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}

		public int CategoryPriority { get; set; }

		public virtual string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		[Browsable(false)]
		public virtual IList<ToolboxItemFilterAttribute> ItemFilters => itemFilters;

		[Browsable(false)]
		public virtual string ItemDomain => GettextCatalog.GetString("Unknown");

		public ItemToolboxNode()
		{
		}

		public virtual bool Filter(string keyword)
		{
			if (Name == null || Name.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) < 0)
			{
				if (Description != null)
				{
					return Description.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0;
				}
				return false;
			}
			return true;
		}

		public override bool Equals(object o)
		{
			if (o is ItemToolboxNode itemToolboxNode && itemToolboxNode.Name == Name && itemToolboxNode.Category == Category)
			{
				return itemToolboxNode.Description == Description;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (Name != null)
			{
				num ^= Name.GetHashCode();
			}
			if (Category != null)
			{
				num ^= Category.GetHashCode();
			}
			if (Description != null)
			{
				num ^= Description.GetHashCode();
			}
			return num;
		}

		public int CompareTo(object other)
		{
			return CompareTo(other as ItemToolboxNode);
		}

		public virtual int CompareTo(ItemToolboxNode other)
		{
			if (other == null)
			{
				return -1;
			}
			if (Category == other.Category)
			{
				return Name.CompareTo(other.Name);
			}
			return Category.CompareTo(other.Category);
		}

		protected Xwt.Drawing.Image ImageToPixbuf(System.Drawing.Image image)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
				memoryStream.Position = 0L;
				return Xwt.Drawing.Image.FromStream(memoryStream);
			}
		}

		public DataCollection Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			dataCollection.Extract("filters");
			dataCollection.Extract("icon");
			DataItem dataItem = new DataItem();
			dataItem.Name = "filters";
			dataCollection.Add(dataItem);
			foreach (ToolboxItemFilterAttribute itemFilter in itemFilters)
			{
				DataItem dataItem2 = new DataItem();
				dataItem2.Name = "filter";
				dataItem2.ItemData.Add(new DataValue("string", itemFilter.FilterString));
				dataItem2.ItemData.Add(new DataValue("type", Enum.GetName(typeof(ToolboxItemFilterType), itemFilter.FilterType)));
				dataItem.ItemData.Add(dataItem2);
			}
			if (icon != null)
			{
				DataItem dataItem3 = new DataItem();
				dataItem3.Name = "icon";
				dataCollection.Add(dataItem3);
				MemoryStream memoryStream = new MemoryStream();
				icon.Save(memoryStream, ImageFileType.Png);
				string value = Convert.ToBase64String(memoryStream.ToArray());
				dataItem3.ItemData.Add(new DataValue("enc", value));
			}
			return dataCollection;
		}

		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			if (data.Extract("filters") is DataItem dataItem && dataItem.HasItemData)
			{
				foreach (DataItem itemDatum in dataItem.ItemData)
				{
					string value = ((DataValue)itemDatum.ItemData["string"]).Value;
					string value2 = ((DataValue)itemDatum.ItemData["type"]).Value;
					ToolboxItemFilterType filterType = (ToolboxItemFilterType)Enum.Parse(typeof(ToolboxItemFilterType), value2);
					itemFilters.Add(new ToolboxItemFilterAttribute(value, filterType));
				}
			}
			if (data.Extract("icon") is DataItem dataItem3)
			{
				DataValue dataValue = (DataValue)dataItem3["enc"];
				MemoryStream stream = new MemoryStream(Convert.FromBase64String(dataValue.Value));
				icon = Xwt.Drawing.Image.FromStream(stream);
			}
			handler.Deserialize(this, data);
		}
	}
}
