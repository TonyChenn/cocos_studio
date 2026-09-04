using System.Collections.Generic;
using System.Xml;
using Gdk;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	public class WatchPad : ObjectValuePad, IMementoCapable, ICustomXmlSerializer
	{
		private static readonly TargetEntry[] DropTargets = new TargetEntry[1]
		{
			new TargetEntry("text/plain;charset=utf-8", TargetFlags.App, 0u)
		};

		private List<string> storedVars;

		public ICustomXmlSerializer Memento
		{
			get
			{
				return this;
			}
			set
			{
				if (tree != null)
				{
					tree.ClearExpressions();
					if (storedVars != null)
					{
						tree.AddExpressions(storedVars);
					}
				}
			}
		}

		public WatchPad()
		{
			tree.EnableModelDragDest(DropTargets, DragAction.Copy);
			tree.DragDataReceived += HandleDragDataReceived;
			tree.AllowAdding = true;
		}

		private void HandleDragDataReceived(object o, DragDataReceivedArgs args)
		{
			string text = args.SelectionData.Text;
			args.RetVal = true;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split('\n');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					AddWatch(text2.Trim());
				}
			}
		}

		public void AddWatch(string expression)
		{
			tree.AddExpression(expression);
		}

		void ICustomXmlSerializer.WriteTo(XmlWriter writer)
		{
			if (tree == null)
			{
				return;
			}
			writer.WriteStartElement("Values");
			foreach (string expression in tree.Expressions)
			{
				writer.WriteElementString("Value", expression);
			}
			writer.WriteEndElement();
		}

		ICustomXmlSerializer ICustomXmlSerializer.ReadFrom(XmlReader reader)
		{
			storedVars = new List<string>();
			reader.MoveToContent();
			if (reader.IsEmptyElement)
			{
				reader.Read();
				return null;
			}
			reader.ReadStartElement();
			reader.MoveToContent();
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					storedVars.Add(reader.ReadElementString());
				}
				else
				{
					reader.Skip();
				}
			}
			reader.ReadEndElement();
			return null;
		}
	}
}
