using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gtk;
using ICSharpCode.NRefactory.Refactoring;
using MonoDevelop.CodeActions;
using MonoDevelop.CodeIssues;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Refactoring;

namespace MonoDevelop.AnalysisCore
{
	internal class ExportRulesHandler : CommandHandler
	{
		protected override void Run()
		{
			string lang = "text/x-csharp";
			OpenFileDialog openFileDialog = new OpenFileDialog("Export Rules", FileChooserAction.Save);
			openFileDialog.InitialFileName = "rules.html";
			if (!openFileDialog.Run())
			{
				return;
			}
			Dictionary<BaseCodeIssueProvider, Severity> dictionary = new Dictionary<BaseCodeIssueProvider, Severity>();
			foreach (CodeIssueProvider inspector in RefactoringService.GetInspectors(lang))
			{
				dictionary[inspector] = inspector.GetSeverity();
				if (!inspector.HasSubIssues)
				{
					continue;
				}
				foreach (BaseCodeIssueProvider subIssue in inspector.SubIssues)
				{
					dictionary[subIssue] = subIssue.GetSeverity();
				}
			}
			IOrderedEnumerable<IGrouping<string, CodeIssueProvider>> orderedEnumerable = (from node in dictionary.Keys.OfType<CodeIssueProvider>()
				group node by node.Category).OrderBy((IGrouping<string, CodeIssueProvider> g) => g.Key, StringComparer.Ordinal);
			using (StreamWriter streamWriter = new StreamWriter(openFileDialog.SelectedFile))
			{
				streamWriter.WriteLine("<h1>Code Rules</h1>");
				foreach (IGrouping<string, CodeIssueProvider> item in orderedEnumerable)
				{
					streamWriter.WriteLine("<h2>" + item.Key + "</h2>");
					streamWriter.WriteLine("<table border='1'>");
					foreach (CodeIssueProvider item2 in item.OrderBy((CodeIssueProvider n) => n.Title, StringComparer.Ordinal))
					{
						string title = item2.Title;
						string text = ((item2.Description != title) ? item2.Description : "");
						streamWriter.WriteLine(string.Concat("<tr><td>", title, "</td><td>", text, "</td><td>", item2.GetSeverity(), "</td></tr>"));
						if (!item2.HasSubIssues)
						{
							continue;
						}
						foreach (BaseCodeIssueProvider subIssue2 in item2.SubIssues)
						{
							title = subIssue2.Title;
							text = ((subIssue2.Description != title) ? subIssue2.Description : "");
							streamWriter.WriteLine(string.Concat("<tr><td> - ", title, "</td><td>", text, "</td><td>", subIssue2.GetSeverity(), "</td></tr>"));
						}
					}
					streamWriter.WriteLine("</table>");
				}
				Dictionary<CodeActionProvider, bool> dictionary2 = new Dictionary<CodeActionProvider, bool>();
				string text2 = PropertyService.Get("ContextActions." + lang, "");
				foreach (CodeActionProvider item3 in RefactoringService.ContextAddinNodes.Where((CodeActionProvider n) => n.MimeType == lang))
				{
					dictionary2[item3] = text2.IndexOf(item3.IdString, StringComparison.Ordinal) < 0;
				}
				streamWriter.WriteLine("<h1>Code Actions</h1>");
				streamWriter.WriteLine("<table border='1'>");
				IOrderedEnumerable<CodeActionProvider> orderedEnumerable2 = dictionary2.Keys.OrderBy((CodeActionProvider n) => n.Title, StringComparer.Ordinal);
				foreach (CodeActionProvider item4 in orderedEnumerable2)
				{
					string text3 = ((item4.Title != item4.Description) ? item4.Description : "");
					streamWriter.WriteLine("<tr><td>" + item4.Title + "</td><td>" + text3 + "</td></tr>");
				}
				streamWriter.WriteLine("</table>");
			}
		}
	}
}
