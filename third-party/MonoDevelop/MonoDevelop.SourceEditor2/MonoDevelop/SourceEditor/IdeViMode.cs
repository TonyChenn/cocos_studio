using System;
using System.Text.RegularExpressions;
using Gdk;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Vi;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.SourceEditor
{
	public class IdeViMode : ViEditMode
	{
		private new ExtensibleTextEditor editor;

		private TabAction tabAction;

		public IdeViMode(ExtensibleTextEditor editor)
		{
			this.editor = editor;
			tabAction = new TabAction(editor);
		}

		protected override Action<TextEditorData> GetInsertAction(Gdk.Key key, ModifierType modifier)
		{
			if (modifier == ModifierType.None)
			{
				switch (key)
				{
				case Gdk.Key.BackSpace:
					return EditActions.AdvancedBackspace;
				case Gdk.Key.Tab:
					return tabAction.Action;
				}
			}
			return base.GetInsertAction(key, modifier);
		}

		protected override string RunExCommand(string command)
		{
			if (':' != command[0] || 2 > command.Length)
			{
				return base.RunExCommand(command);
			}
			switch (command[1])
			{
			case 'w':
				if (2 < command.Length)
				{
					switch (command[2])
					{
					case 'q':
					{
						IWorkbenchWindow workbenchWindow = editor.View.WorkbenchWindow;
						workbenchWindow.Document.Save();
						Application.Invoke(delegate
						{
							workbenchWindow.CloseWindow(force: false);
						});
						return "Saved and closed file.";
					}
					case '!':
						break;
					default:
						return base.RunExCommand(command);
					}
					editor.View.Save();
				}
				else
				{
					editor.View.WorkbenchWindow.Document.Save();
				}
				return "Saved file.";
			case 'q':
			{
				bool force = false;
				if (2 < command.Length)
				{
					char c = command[2];
					if (c != '!')
					{
						return base.RunExCommand(command);
					}
					force = true;
				}
				if (!force && editor.View.IsDirty)
				{
					return "Document has not been saved!";
				}
				IWorkbenchWindow window = editor.View.WorkbenchWindow;
				Application.Invoke(delegate
				{
					window.CloseWindow(force);
				});
				if (!force)
				{
					return "Closed file.";
				}
				return "Closed file without saving.";
			}
			case 'm':
				if (Regex.IsMatch(command, "^:mak[e!]", RegexOptions.Compiled))
				{
					Project project = editor.View.Project;
					if (project != null)
					{
						IdeApp.ProjectOperations.Build(project);
						return $"Building project {project.Name}";
					}
					return "File is not part of a project";
				}
				break;
			case 'c':
				if (3 == command.Length)
				{
					switch (command[2])
					{
					case 'n':
						IdeApp.CommandService.DispatchCommand(ViewCommands.ShowNext);
						return string.Empty;
					case 'N':
					case 'p':
						IdeApp.CommandService.DispatchCommand(ViewCommands.ShowPrevious);
						return string.Empty;
					}
				}
				break;
			}
			return base.RunExCommand(command);
		}

		protected override void HandleKeypress(Gdk.Key key, uint unicodeKey, ModifierType modifier)
		{
			if ((ModifierType.ControlMask & modifier) != ModifierType.None)
			{
				if (key == Gdk.Key.bracketright)
				{
					IdeApp.CommandService.DispatchCommand("MonoDevelop.Refactoring.RefactoryCommands.GotoDeclaration", CommandSource.Keybinding);
					return;
				}
			}
			base.HandleKeypress(key, unicodeKey, modifier);
		}
	}
}
