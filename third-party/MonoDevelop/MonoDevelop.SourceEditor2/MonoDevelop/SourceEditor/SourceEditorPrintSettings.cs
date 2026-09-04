using MonoDevelop.Core;
using Pango;

namespace MonoDevelop.SourceEditor
{
	internal class SourceEditorPrintSettings
	{
		public bool UseHighlighting { get; private set; }

		public FontDescription Font { get; private set; }

		public int TabSize { get; private set; }

		public string ColorScheme { get; private set; }

		public string HeaderFormat { get; private set; }

		public string FooterFormat { get; private set; }

		public double HeaderSeparatorWeight { get; private set; }

		public double FooterSeparatorWeight { get; private set; }

		public double HeaderPadding { get; private set; }

		public double FooterPadding { get; private set; }

		public FontDescription HeaderFooterFont { get; private set; }

		public FontDescription LineNumberFont { get; private set; }

		public bool ShowLineNumbers { get; private set; }

		public bool WrapLines { get; private set; }

		public static SourceEditorPrintSettings Load()
		{
			return new SourceEditorPrintSettings();
		}

		public void Save()
		{
		}

		private SourceEditorPrintSettings()
		{
			Font = DefaultSourceEditorOptions.Instance.Font;
			TabSize = DefaultSourceEditorOptions.Instance.TabSize;
			HeaderFormat = "%F";
			FooterFormat = GettextCatalog.GetString("Page %N of %Q");
			ColorScheme = "default";
			HeaderSeparatorWeight = (FooterSeparatorWeight = 0.5);
			HeaderPadding = (FooterPadding = 6.0);
			UseHighlighting = true;
		}
	}
}
