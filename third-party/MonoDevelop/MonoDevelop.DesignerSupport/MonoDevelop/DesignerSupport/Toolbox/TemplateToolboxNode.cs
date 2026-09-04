using System;
using System.ComponentModel;
using MonoDevelop.Ide.CodeTemplates;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	public class TemplateToolboxNode : ItemToolboxNode, ITextToolboxNode
	{
		public override string Name
		{
			get
			{
				return Template.Shortcut;
			}
			set
			{
				Template.Shortcut = value;
			}
		}

		public override string Description
		{
			get
			{
				return Template.Description;
			}
			set
			{
				Template.Description = value;
			}
		}

		public string Code
		{
			get
			{
				return Template.Code;
			}
			set
			{
				Template.Code = value;
			}
		}

		public bool IsSurrondingTemplate
		{
			get
			{
				return Template.CodeTemplateType.HasFlag(CodeTemplateType.SurroundsWith);
			}
			set
			{
				if (value)
				{
					Template.CodeTemplateType |= CodeTemplateType.SurroundsWith;
				}
				else
				{
					Template.CodeTemplateType &= ~CodeTemplateType.SurroundsWith;
				}
			}
		}

		[Browsable(false)]
		public CodeTemplate Template { get; set; }

		public string GetDragPreview(Document document)
		{
			return Template.Shortcut;
		}

		public bool IsCompatibleWith(Document document)
		{
			return true;
		}

		public void InsertAtCaret(Document document)
		{
			Template.Insert(document);
		}

		public TemplateToolboxNode(CodeTemplate template)
		{
			Template = template;
			ItemFilters.Add(new ToolboxItemFilterAttribute("text/plain", ToolboxItemFilterType.Allow));
		}
	}
}
