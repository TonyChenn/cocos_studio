using System;
using System.Collections.Generic;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Refactoring;
using MonoDevelop.Refactoring.Rename;

namespace MonoDevelop.AnalysisCore.Fixes
{
	internal class RenameMemberHandler : IFixHandler
	{
		private class RenameFixAction : IAnalysisFixAction
		{
			public RenameRefactoring Refactoring;

			public RefactoringOptions Options;

			public RenameRefactoring.RenameProperties Properties;

			public bool Preview;

			public string Label { get; set; }

			public DocumentRegion DocumentRegion { get; set; }

			public string IdString { get; set; }

			public bool SupportsBatchFix => false;

			public void Fix()
			{
				if (string.IsNullOrEmpty(Properties.NewName))
				{
					Refactoring.Run(Options);
					return;
				}
				List<Change> changes = Refactoring.PerformChanges(Options, Properties);
				if (Preview)
				{
					MessageService.ShowCustomDialog(new RefactoringPreviewDialog(changes));
					return;
				}
				IProgressMonitor backgroundProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetBackgroundProgressMonitor("Rename", null);
				RefactoringService.AcceptChanges(backgroundProgressMonitor, changes);
			}

			public void BatchFix()
			{
				throw new InvalidOperationException("Batch fixing is not supported");
			}
		}

		public IEnumerable<IAnalysisFixAction> GetFixes(Document doc, object fix)
		{
			RenameMemberFix renameFix = (RenameMemberFix)fix;
			RenameRefactoring refactoring = new RenameRefactoring();
			RefactoringOptions options = new RefactoringOptions(doc)
			{
				SelectedItem = renameFix.Item
			};
			if (renameFix.Item == null)
			{
				options.SelectedItem = CurrentRefactoryOperationsHandler.GetItem(options.Document, out var _);
			}
			if (refactoring.IsValid(options))
			{
				RenameRefactoring.RenameProperties prop = new RenameRefactoring.RenameProperties
				{
					NewName = renameFix.NewName
				};
				if (string.IsNullOrEmpty(renameFix.NewName))
				{
					yield return new RenameFixAction
					{
						Label = GettextCatalog.GetString("Rename '{0}'...", renameFix.OldName),
						Refactoring = refactoring,
						Options = options,
						Properties = prop,
						Preview = false
					};
					yield break;
				}
				yield return new RenameFixAction
				{
					Label = GettextCatalog.GetString("Rename '{0}' to '{1}'", renameFix.OldName, renameFix.NewName),
					Refactoring = refactoring,
					Options = options,
					Properties = prop,
					Preview = false
				};
				yield return new RenameFixAction
				{
					Label = GettextCatalog.GetString("Rename '{0}' to '{1}' with preview", renameFix.OldName, renameFix.NewName),
					Refactoring = refactoring,
					Options = options,
					Properties = prop,
					Preview = true
				};
			}
		}
	}
}
