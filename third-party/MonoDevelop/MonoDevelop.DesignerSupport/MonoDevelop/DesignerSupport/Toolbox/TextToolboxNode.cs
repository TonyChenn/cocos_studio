using System;
using System.ComponentModel;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	public class TextToolboxNode : ItemToolboxNode, ITextToolboxNode
	{
		private string text = string.Empty;

		private string domain = GettextCatalog.GetString("Text Snippets");

		[LocalizedDescription("The text that will be inserted into the document.")]
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		[Browsable(false)]
		public override string ItemDomain => domain;

		public TextToolboxNode(string text)
		{
			Text = text;
			ItemFilters.Add(new ToolboxItemFilterAttribute("text/plain", ToolboxItemFilterType.Allow));
		}

		public override bool Filter(string keyword)
		{
			if (!base.Filter(keyword))
			{
				if (Text != null)
				{
					return Text.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) >= 0;
				}
				return false;
			}
			return true;
		}

		public override bool Equals(object o)
		{
			if (o is TextToolboxNode textToolboxNode && text == textToolboxNode.text)
			{
				return base.Equals(o);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (text != null)
			{
				num ^= text.GetHashCode();
			}
			return num;
		}

		public bool IsCompatibleWith(Document document)
		{
			return true;
		}

		public string GetDragPreview(Document document)
		{
			return text;
		}

		public void InsertAtCaret(Document document)
		{
			document.Editor.InsertAtCaret(text);
		}
	}
}
