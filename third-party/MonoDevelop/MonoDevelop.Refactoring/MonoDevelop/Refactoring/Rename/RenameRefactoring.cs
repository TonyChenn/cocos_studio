using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.ProgressMonitoring;

namespace MonoDevelop.Refactoring.Rename
{
	public class RenameRefactoring : RefactoringOperation
	{
		public class RenameProperties
		{
			public string NewName { get; set; }

			public bool RenameFile { get; set; }

			public bool IncludeOverloads { get; set; }
		}

		public override string AccelKey => IdeApp.CommandService.GetCommandInfo(EditCommands.Rename).AccelKey?.Replace("dead_circumflex", "^");

		public RenameRefactoring()
		{
			base.Name = "Rename";
		}

		public override bool IsValid(RefactoringOptions options)
		{
			if (options.SelectedItem is IVariable || options.SelectedItem is IParameter)
			{
				return true;
			}
			if (options.SelectedItem is INamespace)
			{
				INamespace obj = (INamespace)options.SelectedItem;
				return obj.Types.Any((ITypeDefinition type) => !string.IsNullOrEmpty(type.Region.FileName));
			}
			if (options.SelectedItem is ITypeDefinition)
			{
				return !string.IsNullOrEmpty(((ITypeDefinition)options.SelectedItem).Region.FileName);
			}
			if (options.SelectedItem is IType && ((IType)options.SelectedItem).Kind == TypeKind.TypeParameter)
			{
				return !string.IsNullOrEmpty(((ITypeParameter)options.SelectedItem).Region.FileName);
			}
			if (options.SelectedItem is IMember member)
			{
				if (member.SymbolKind == SymbolKind.Operator)
				{
					return false;
				}
				ITypeDefinition declaringTypeDefinition = member.DeclaringTypeDefinition;
				return declaringTypeDefinition != null;
			}
			return false;
		}

		public static void Rename(IEntity entity, string newName)
		{
			if (newName == null)
			{
				RefactoringOptions refactoringOptions = new RefactoringOptions();
				refactoringOptions.SelectedItem = entity;
				RefactoringOptions options = refactoringOptions;
				new RenameRefactoring().Run(options);
				return;
			}
			using (NullProgressMonitor monitor = new NullProgressMonitor())
			{
				IEnumerable<MemberReference> enumerable = ReferenceFinder.FindReferences(entity, searchForAllOverloads: true, monitor);
				List<Change> list = new List<Change>();
				foreach (MemberReference item in enumerable)
				{
					TextReplaceChange textReplaceChange = new TextReplaceChange();
					textReplaceChange.FileName = item.FileName;
					textReplaceChange.Offset = item.Offset;
					textReplaceChange.RemovedChars = item.Length;
					textReplaceChange.InsertedText = newName;
					textReplaceChange.Description = string.Format(GettextCatalog.GetString("Replace '{0}' with '{1}'"), item.GetName(), newName);
					list.Add(textReplaceChange);
				}
				if (list.Count > 0)
				{
					RefactoringService.AcceptChanges(monitor, list);
				}
			}
		}

		public static void RenameVariable(IVariable variable, string newName)
		{
			using (NullProgressMonitor monitor = new NullProgressMonitor())
			{
				IEnumerable<MemberReference> enumerable = ReferenceFinder.FindReferences(variable, searchForAllOverloads: true, monitor);
				List<Change> list = new List<Change>();
				foreach (MemberReference item in enumerable)
				{
					TextReplaceChange textReplaceChange = new TextReplaceChange();
					textReplaceChange.FileName = item.FileName;
					textReplaceChange.Offset = item.Offset;
					textReplaceChange.RemovedChars = item.Length;
					textReplaceChange.InsertedText = newName;
					textReplaceChange.Description = string.Format(GettextCatalog.GetString("Replace '{0}' with '{1}'"), item.GetName(), newName);
					list.Add(textReplaceChange);
				}
				if (list.Count > 0)
				{
					RefactoringService.AcceptChanges(monitor, list);
				}
			}
		}

		public static void RenameTypeParameter(ITypeParameter typeParameter, string newName)
		{
			if (newName == null)
			{
				RefactoringOptions refactoringOptions = new RefactoringOptions();
				refactoringOptions.SelectedItem = typeParameter;
				RefactoringOptions options = refactoringOptions;
				new RenameRefactoring().Run(options);
				return;
			}
			using (NullProgressMonitor monitor = new NullProgressMonitor())
			{
				IEnumerable<MemberReference> enumerable = ReferenceFinder.FindReferences(typeParameter, searchForAllOverloads: true, monitor);
				List<Change> list = new List<Change>();
				foreach (MemberReference item in enumerable)
				{
					TextReplaceChange textReplaceChange = new TextReplaceChange();
					textReplaceChange.FileName = item.FileName;
					textReplaceChange.Offset = item.Offset;
					textReplaceChange.RemovedChars = item.Length;
					textReplaceChange.InsertedText = newName;
					textReplaceChange.Description = string.Format(GettextCatalog.GetString("Replace '{0}' with '{1}'"), item.GetName(), newName);
					list.Add(textReplaceChange);
				}
				if (list.Count > 0)
				{
					RefactoringService.AcceptChanges(monitor, list);
				}
			}
		}

		public static void RenameNamespace(INamespace ns, string newName)
		{
			using (NullProgressMonitor monitor = new NullProgressMonitor())
			{
				IEnumerable<MemberReference> enumerable = ReferenceFinder.FindReferences(ns, searchForAllOverloads: true, monitor);
				List<Change> list = new List<Change>();
				foreach (MemberReference item in enumerable)
				{
					TextReplaceChange textReplaceChange = new TextReplaceChange();
					textReplaceChange.FileName = item.FileName;
					textReplaceChange.Offset = item.Offset;
					textReplaceChange.RemovedChars = item.Length;
					textReplaceChange.InsertedText = newName;
					textReplaceChange.Description = string.Format(GettextCatalog.GetString("Replace '{0}' with '{1}'"), item.GetName(), newName);
					list.Add(textReplaceChange);
				}
				if (list.Count > 0)
				{
					RefactoringService.AcceptChanges(monitor, list);
				}
			}
		}

		public override string GetMenuDescription(RefactoringOptions options)
		{
			return IdeApp.CommandService.GetCommandInfo(EditCommands.Rename).Text;
		}

		public override void Run(RefactoringOptions options)
		{
			if (options.SelectedItem is IVariable)
			{
				if (options.SelectedItem is IField field && (field.Accessibility != Accessibility.Private || (field.DeclaringTypeDefinition != null && field.DeclaringTypeDefinition.Parts.Count > 1)))
				{
					MessageService.ShowCustomDialog(new RenameItemDialog(options, this));
					return;
				}
				if (options.SelectedItem is IParameter parameter && parameter.Owner != null && (parameter.Owner.Accessibility != Accessibility.Private || (parameter.Owner.DeclaringTypeDefinition != null && parameter.Owner.DeclaringTypeDefinition.Parts.Count > 1)))
				{
					MessageService.ShowCustomDialog(new RenameItemDialog(options, this));
					return;
				}
				IEnumerable<MemberReference> enumerable = ReferenceFinder.FindReferences(options.SelectedItem, searchForAllOverloads: true);
				if (enumerable == null)
				{
					return;
				}
				TextEditorData textEditorData = ((options.Document != null) ? options.GetTextEditorData() : IdeApp.Workbench.ActiveDocument.Editor);
				TextEditor editor = textEditorData.Parent;
				if (editor == null)
				{
					MessageService.ShowCustomDialog(new RenameItemDialog(options, this));
					return;
				}
				List<TextLink> list = new List<TextLink>();
				TextLink textLink = new TextLink("name");
				int num = int.MaxValue;
				foreach (MemberReference item in enumerable)
				{
					num = Math.Min(num, item.Offset);
				}
				foreach (MemberReference item2 in enumerable)
				{
					TextSegment textSegment = new TextSegment(item2.Offset - num, item2.Length);
					if (textSegment.Offset <= textEditorData.Caret.Offset - num && textEditorData.Caret.Offset - num <= textSegment.EndOffset)
					{
						textLink.Links.Insert(0, textSegment);
					}
					else
					{
						textLink.AddLink(textSegment);
					}
				}
				list.Add(textLink);
				if (editor.CurrentMode is TextLinkEditMode)
				{
					((TextLinkEditMode)editor.CurrentMode).ExitTextLinkMode();
				}
				TextLinkEditMode tle = new TextLinkEditMode(editor, num, list);
				tle.SetCaretPosition = false;
				tle.SelectPrimaryLink = true;
				if (!tle.ShouldStartTextLinkMode)
				{
					return;
				}
				tle.Cancel += delegate
				{
					if (tle.HasChangedText)
					{
						editor.Document.Undo();
					}
				};
				tle.OldMode = textEditorData.CurrentMode;
				tle.StartMode();
				textEditorData.CurrentMode = tle;
			}
			else
			{
				MessageService.ShowCustomDialog(new RenameItemDialog(options, this));
			}
		}

		public override List<Change> PerformChanges(RefactoringOptions options, object prop)
		{
			RenameProperties renameProperties = (RenameProperties)prop;
			List<Change> list = new List<Change>();
			IEnumerable<MemberReference> enumerable = null;
			using (MessageDialogProgressMonitor monitor = new MessageDialogProgressMonitor(showProgress: true, allowCancel: false, showDetails: false, hideWhenDone: true))
			{
				enumerable = ReferenceFinder.FindReferences(options.SelectedItem, renameProperties.IncludeOverloads, monitor);
				if (enumerable == null)
				{
					return list;
				}
				if (renameProperties.RenameFile && options.SelectedItem is IType)
				{
					ITypeDefinition definition = ((IType)options.SelectedItem).GetDefinition();
					int num = 1;
					HashSet<string> hashSet = new HashSet<string>();
					foreach (IUnresolvedTypeDefinition part in definition.Parts)
					{
						if (hashSet.Contains(part.Region.FileName))
						{
							continue;
						}
						hashSet.Add(part.Region.FileName);
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(part.Region.FileName);
						string newName = renameProperties.NewName;
						if (!string.IsNullOrEmpty(fileNameWithoutExtension) && !string.IsNullOrEmpty(newName) && !(fileNameWithoutExtension.ToUpper() == newName.ToUpper()) && !fileNameWithoutExtension.ToUpper().EndsWith("." + newName.ToUpper(), StringComparison.Ordinal))
						{
							int num2 = fileNameWithoutExtension.IndexOf(definition.Name, StringComparison.Ordinal);
							string fileName;
							if (num2 >= 0)
							{
								fileName = fileNameWithoutExtension.Substring(0, num2) + newName + fileNameWithoutExtension.Substring(num2 + definition.Name.Length);
							}
							else
							{
								fileName = ((num != 1) ? (newName + num) : newName);
								num++;
							}
							int i;
							for (i = 0; File.Exists(GetFullFileName(fileName, part.Region.FileName, i)); i++)
							{
							}
							list.Add(new RenameFileChange(part.Region.FileName, GetFullFileName(fileName, part.Region.FileName, i)));
						}
					}
				}
				foreach (MemberReference item in enumerable)
				{
					TextReplaceChange textReplaceChange = new TextReplaceChange();
					textReplaceChange.FileName = item.FileName;
					textReplaceChange.Offset = item.Offset;
					textReplaceChange.RemovedChars = item.Length;
					textReplaceChange.InsertedText = renameProperties.NewName;
					textReplaceChange.Description = string.Format(GettextCatalog.GetString("Replace '{0}' with '{1}'"), item.GetName(), renameProperties.NewName);
					list.Add(textReplaceChange);
				}
				return list;
			}
		}

		private static string GetFullFileName(string fileName, string oldFullFileName, int tryCount)
		{
			StringBuilder stringBuilder = new StringBuilder(fileName);
			if (tryCount > 0)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(tryCount.ToString());
			}
			if (Path.HasExtension(oldFullFileName))
			{
				stringBuilder.Append(Path.GetExtension(oldFullFileName));
			}
			return Path.Combine(Path.GetDirectoryName(oldFullFileName), stringBuilder.ToString());
		}
	}
}
