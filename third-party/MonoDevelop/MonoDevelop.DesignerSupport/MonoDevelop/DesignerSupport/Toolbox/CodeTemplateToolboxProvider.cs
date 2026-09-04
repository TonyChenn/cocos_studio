using System;
using System.Collections.Generic;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeTemplates;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class CodeTemplateToolboxProvider : IToolboxDynamicProvider
	{
		private static string category = GettextCatalog.GetString("Text Snippets");

		public event EventHandler ItemsChanged
		{
			add
			{
				CodeTemplateService.TemplatesChanged += value;
			}
			remove
			{
				CodeTemplateService.TemplatesChanged -= value;
			}
		}

		public IEnumerable<ItemToolboxNode> GetDynamicItems(IToolboxConsumer consumer)
		{
			if (!(consumer is IExtensibleTextEditor editor))
			{
				yield break;
			}
			foreach (CodeTemplate ct in CodeTemplateService.GetCodeTemplatesForFile(editor.Name))
			{
				if (ct.CodeTemplateContext == CodeTemplateContext.Standard)
				{
					yield return new TemplateToolboxNode(ct)
					{
						Category = category,
						Icon = ImageService.GetIcon("md-template", IconSize.Menu)
					};
				}
			}
		}
	}
}
