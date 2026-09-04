using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components;
using MonoDevelop.Ide.Fonts;
using Pango;

namespace MonoDevelop.SourceEditor
{
	public class LanguageItemWindow : TooltipWindow
	{
		public bool IsEmpty { get; set; }

		public LanguageItemWindow(ExtensibleTextEditor ed, ModifierType modifierState, ResolveResult result, string errorInformations, IUnresolvedFile unit)
		{
			string text = null;
			if (result is UnknownIdentifierResolveResult)
			{
				text = $"error CS0103: The name `{((UnknownIdentifierResolveResult)result).Identifier}' does not exist in the current context";
			}
			else if (result is UnknownMemberResolveResult)
			{
				UnknownMemberResolveResult unknownMemberResolveResult = (UnknownMemberResolveResult)result;
				if (unknownMemberResolveResult.TargetType.Kind != TypeKind.Unknown)
				{
					text = $"error CS0117: `{unknownMemberResolveResult.TargetType.FullName}' does not contain a definition for `{unknownMemberResolveResult.MemberName}'";
				}
			}
			else if (result == null || ed.TextEditorResolverProvider == null)
			{
				text = errorInformations;
			}
			if (string.IsNullOrEmpty(text) || text == "?")
			{
				IsEmpty = true;
				return;
			}
			FixedWidthWrapLabel fixedWidthWrapLabel = new FixedWidthWrapLabel
			{
				Wrap = Pango.WrapMode.WordChar,
				Indent = -20,
				BreakOnCamelCasing = true,
				BreakOnPunctuation = true,
				Markup = text
			};
			base.BorderWidth = 3u;
			Add(fixedWidthWrapLabel);
			UpdateFont(fixedWidthWrapLabel);
			base.EnableTransparencyControl = true;
		}

		public int SetMaxWidth(int maxWidth)
		{
			if (!(base.Child is FixedWidthWrapLabel fixedWidthWrapLabel))
			{
				return base.Allocation.Width;
			}
			fixedWidthWrapLabel.MaxWidth = maxWidth;
			return fixedWidthWrapLabel.RealWidth;
		}

		protected override void OnStyleSet(Gtk.Style previous_style)
		{
			base.OnStyleSet(previous_style);
			UpdateFont(base.Child as FixedWidthWrapLabel);
		}

		private void UpdateFont(FixedWidthWrapLabel label)
		{
			if (label != null)
			{
				label.FontDescription = FontService.GetFontDescription("Pad");
			}
		}
	}
}
