using Gdk;
using ICSharpCode.NRefactory.Completion;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.Refactoring
{
	internal class ImportSymbolCompletionData : CompletionData
	{
		private IType type;

		private Ambience ambience;

		private ParsedDocument unit;

		private Document doc;

		private ImportSymbolCache cache;

		private bool initialized;

		private bool generateUsing;

		private bool insertNamespace;

		private string displayText;

		private string displayDescription;

		public IType Type => type;

		public override IconId Icon => type.GetStockIcon();

		public override string DisplayText
		{
			get
			{
				if (displayText == null)
				{
					displayText = ambience.GetString(type, OutputFlags.IncludeGenerics);
				}
				return displayText;
			}
		}

		public override string Description
		{
			get
			{
				Initialize();
				if (generateUsing)
				{
					return string.Format(GettextCatalog.GetString("Add namespace import '{0}'"), type.Namespace);
				}
				return null;
			}
		}

		public override string CompletionText => type.Name;

		public ImportSymbolCompletionData(Document doc, ImportSymbolCache cache, IType type)
		{
			this.doc = doc;
			this.cache = cache;
			ambience = AmbienceService.GetAmbience(doc.Editor.MimeType);
			this.type = type;
			unit = doc.ParsedDocument;
			DisplayFlags |= DisplayFlags.IsImportCompletion;
		}

		private void Initialize()
		{
			if (!initialized)
			{
				initialized = true;
				if (!string.IsNullOrEmpty(type.Namespace))
				{
					GenerateNamespaceImport result = cache.GetResult(unit.ParsedFile, type, doc);
					generateUsing = result.GenerateUsing;
					insertNamespace = result.InsertNamespace;
				}
			}
		}

		public override void InsertCompletionText(CompletionListWindow window, ref KeyActions ka, Key closeChar, char keyChar, ModifierType modifier)
		{
			Initialize();
			using (doc.Editor.OpenUndoGroup())
			{
				string text = (insertNamespace ? (type.Namespace + "." + type.Name) : type.Name);
				if (text != CompletionData.GetCurrentWord(window))
				{
					if (window.WasShiftPressed && generateUsing)
					{
						text = type.Namespace + "." + text;
					}
					window.CompletionWidget.SetCompletionText(window.CodeCompletionContext, CompletionData.GetCurrentWord(window), text);
				}
				if (!window.WasShiftPressed && generateUsing)
				{
					CodeGenerator codeGenerator = CodeGenerator.CreateGenerator(doc);
					if (codeGenerator != null)
					{
						codeGenerator.AddGlobalNamespaceImport(doc, type.Namespace);
						doc.UpdateParseDocument();
					}
				}
			}
			ka |= KeyActions.Ignore;
		}

		private static string GetDefaultDisplaySelection(string description, bool isSelected)
		{
			if (!isSelected)
			{
				return "<span foreground=\"darkgray\">" + description + "</span>";
			}
			return description;
		}

		public override string GetDisplayDescription(bool isSelected)
		{
			if (displayDescription == null)
			{
				Initialize();
				if (generateUsing || insertNamespace)
				{
					displayDescription = string.Format(GettextCatalog.GetString("(from '{0}')"), type.Namespace);
				}
				else
				{
					displayDescription = "";
				}
			}
			return GetDefaultDisplaySelection(displayDescription, isSelected);
		}
	}
}
