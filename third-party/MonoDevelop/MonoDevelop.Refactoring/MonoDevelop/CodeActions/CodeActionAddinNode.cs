using Mono.Addins;

namespace MonoDevelop.CodeActions
{
	public class CodeActionAddinNode : TypeExtensionNode
	{
		[NodeAttribute("mimeType", Required = true, Description = "The mime type of this action.")]
		private string mimeType;

		[NodeAttribute("_title", Required = true, Localizable = true, Description = "The title of this action.")]
		private string title;

		[NodeAttribute("_description", Required = true, Localizable = true, Description = "The description of this action.")]
		private string description;

		private CodeActionProvider action;

		public string MimeType => mimeType;

		public string Title => title;

		public string Description => description;

		public CodeActionProvider Action
		{
			get
			{
				if (action == null)
				{
					action = (CodeActionProvider)CreateInstance();
					action.Title = title;
					action.Description = description;
					action.MimeType = MimeType;
				}
				return action;
			}
		}
	}
}
