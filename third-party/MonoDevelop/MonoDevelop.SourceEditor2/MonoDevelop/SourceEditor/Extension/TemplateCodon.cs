using System;
using System.IO;
using Mono.Addins;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.SourceEditor.Extension
{
	[ExtensionNode(Description = "A template for color and syntax shemes.")]
	public class TemplateCodon : ExtensionNode, IStreamProvider
	{
		[NodeAttribute("resource", "Name of the resource where the template is stored.")]
		private string resource;

		[NodeAttribute("file", "Name of the file where the template is stored.")]
		private string file;

		public TemplateCodon()
		{
			resource = (file = null);
		}

		public Stream Open()
		{
			Stream stream;
			if (!string.IsNullOrEmpty(file))
			{
				stream = File.OpenRead(base.Addin.GetFilePath(file));
			}
			else
			{
				if (string.IsNullOrEmpty(resource))
				{
					throw new InvalidOperationException("Template file or resource not provided");
				}
				stream = base.Addin.GetResource(resource);
				if (stream == null)
				{
					throw new ApplicationException("Template " + resource + " not found");
				}
			}
			return stream;
		}
	}
}
