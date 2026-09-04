using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI.Design;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	public class ToolboxItemToolboxNode : TypeToolboxNode
	{
		[ItemProperty("itemcontents")]
		private string serializedToolboxItem;

		[ItemProperty("itemtype")]
		private TypeReference toolboxItemType;

		public override string ItemDomain => GettextCatalog.GetString("Web and Windows Forms Components");

		public ToolboxItemToolboxNode(ToolboxItem item)
			: base(item.TypeName, item.AssemblyName.FullName)
		{
			base.Name = item.DisplayName;
			if (item.Bitmap != null)
			{
				base.Icon = ImageToPixbuf(item.Bitmap);
			}
			foreach (ToolboxItemFilterAttribute item2 in item.Filter)
			{
				base.ItemFilters.Add(item2);
			}
			if (item.GetType() == typeof(ToolboxItem))
			{
				toolboxItemType = null;
				return;
			}
			if (item.GetType() == typeof(WebControlToolboxItem))
			{
				toolboxItemType = new TypeReference(typeof(WebControlToolboxItem));
				return;
			}
			serializedToolboxItem = SerializeToolboxItem(item);
			toolboxItemType = new TypeReference(item.GetType());
		}

		public ToolboxItemToolboxNode(ToolboxItem item, string assemblyLocation)
			: base(item.TypeName, item.AssemblyName.FullName)
		{
			base.Type.AssemblyLocation = assemblyLocation;
		}

		public ToolboxItemToolboxNode()
		{
		}

		public override bool Equals(object obj)
		{
			if (obj is ToolboxItemToolboxNode toolboxItemToolboxNode && ((toolboxItemType == null) ? (toolboxItemToolboxNode.toolboxItemType == null) : toolboxItemType.Equals(toolboxItemToolboxNode.toolboxItemType)))
			{
				return base.Equals((object)toolboxItemToolboxNode);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (toolboxItemType != null)
			{
				num ^= toolboxItemType.GetHashCode();
			}
			return num;
		}

		public ToolboxItem GetToolboxItem()
		{
			Type type = typeof(ToolboxItem);
			if (toolboxItemType != null && !string.IsNullOrEmpty(toolboxItemType.TypeName))
			{
				type = toolboxItemType.Load();
			}
			if (serializedToolboxItem != null && serializedToolboxItem.Length > 0)
			{
				return DeserializeToolboxItem(serializedToolboxItem);
			}
			Type type2 = base.Type.Load();
			return (ToolboxItem)Activator.CreateInstance(type, type2);
		}

		private ToolboxItem DeserializeToolboxItem(string serializedObject)
		{
			byte[] buffer = Convert.FromBase64String(serializedObject);
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			object obj = binaryFormatter.Deserialize(memoryStream);
			memoryStream.Close();
			if (!(obj is ToolboxItem))
			{
				throw new Exception("Could not deserialise ToolboxItem for " + base.Name);
			}
			return (ToolboxItem)obj;
		}

		private string SerializeToolboxItem(ToolboxItem toolboxItem)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, toolboxItem);
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			return Convert.ToBase64String(inArray);
		}
	}
}
