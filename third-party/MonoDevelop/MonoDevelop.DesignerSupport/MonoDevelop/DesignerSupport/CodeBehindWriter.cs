using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Text;

namespace MonoDevelop.DesignerSupport
{
	public class CodeBehindWriter
	{
		private List<string> openFiles;

		private List<KeyValuePair<FilePath, string>> filesToWrite = new List<KeyValuePair<FilePath, string>>();

		private CodeDomProvider provider;

		private CodeGeneratorOptions options;

		private IProgressMonitor monitor;

		public CodeDomProvider Provider => provider;

		public CodeGeneratorOptions GeneratorOptions => options;

		public bool SupportsPartialTypes => provider.Supports(GeneratorSupport.PartialTypes);

		private List<string> OpenFiles
		{
			get
			{
				if (openFiles == null)
				{
					openFiles = new List<string>();
					if (!IdeApp.IsInitialized)
					{
						return openFiles;
					}
					DispatchService.GuiSyncDispatch(delegate
					{
						foreach (Document document in IdeApp.Workbench.Documents)
						{
							if (document.GetContent<IEditableTextBuffer>() != null)
							{
								openFiles.Add(document.FileName);
							}
						}
					});
				}
				return openFiles;
			}
		}

		public int WrittenCount { get; private set; }

		public CodeBehindWriter()
		{
		}

		private CodeBehindWriter(IProgressMonitor monitor, CodeDomProvider provider, CodeGeneratorOptions options)
		{
			this.provider = provider;
			this.options = options;
			this.monitor = monitor;
		}

		public static CodeBehindWriter CreateForProject(IProgressMonitor monitor, DotNetProject project)
		{
			TextStylePolicy textStylePolicy = project.Policies.Get<TextStylePolicy>();
			CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions();
			codeGeneratorOptions.IndentString = (textStylePolicy.TabsToSpaces ? new string(' ', textStylePolicy.TabWidth) : "\t");
			codeGeneratorOptions.BlankLinesBetweenMembers = true;
			CodeGeneratorOptions codeGeneratorOptions2 = codeGeneratorOptions;
			CodeDomProvider codeDomProvider = project.LanguageBinding.GetCodeDomProvider();
			return new CodeBehindWriter(monitor, codeDomProvider, codeGeneratorOptions2);
		}

		public void WriteFile(FilePath path, Action<TextWriter> write)
		{
			if (OpenFiles.Contains(path))
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					write(stringWriter);
					filesToWrite.Add(new KeyValuePair<FilePath, string>(path, stringWriter.ToString()));
					return;
				}
			}
			try
			{
				FilePath filePath = path.ParentDirectory.Combine(".#" + path.FileName);
				using (StreamWriter obj = new StreamWriter(filePath))
				{
					write(obj);
				}
				FileService.SystemRename(filePath, path);
				Application.Invoke(delegate
				{
					FileService.NotifyFileChanged(path);
				});
				WrittenCount++;
			}
			catch (IOException exception)
			{
				monitor.ReportError(GettextCatalog.GetString("Failed to write file '{0}'.", path), exception);
			}
			catch (Exception exception2)
			{
				monitor.ReportError(GettextCatalog.GetString("Failed to generate code for file '{0}'.", path), exception2);
			}
		}

		public void WriteFile(FilePath path, CodeCompileUnit ccu)
		{
			WriteFile(path, delegate(TextWriter tw)
			{
				provider.GenerateCodeFromCompileUnit(ccu, tw, options);
			});
		}

		public void WriteFile(FilePath path, string contents)
		{
			if (OpenFiles.Contains(path))
			{
				using (new StringWriter())
				{
					filesToWrite.Add(new KeyValuePair<FilePath, string>(path, contents));
					return;
				}
			}
			try
			{
				FilePath filePath = path.ParentDirectory.Combine(".#" + path.FileName);
				File.WriteAllText(filePath, contents);
				FileService.SystemRename(filePath, path);
				Application.Invoke(delegate
				{
					FileService.NotifyFileChanged(path);
				});
				WrittenCount++;
			}
			catch (IOException exception)
			{
				monitor.ReportError(GettextCatalog.GetString("Failed to write file '{0}'.", path), exception);
			}
			catch (Exception exception2)
			{
				monitor.ReportError(GettextCatalog.GetString("Failed to generate code for file '{0}'.", path), exception2);
			}
		}

		public void WriteOpenFiles()
		{
			if (filesToWrite == null)
			{
				return;
			}
			if (filesToWrite.Count == 0)
			{
				filesToWrite = null;
				return;
			}
			DispatchService.GuiSyncDispatch(delegate
			{
				foreach (KeyValuePair<FilePath, string> item in filesToWrite)
				{
					try
					{
						bool flag = false;
						foreach (Document document in IdeApp.Workbench.Documents)
						{
							if (document.FileName == item.Key)
							{
								IEditableTextFile content = document.GetContent<IEditableTextFile>();
								if (content != null)
								{
									document.Editor.Text = item.Value;
									document.IsDirty = true;
									document.Save();
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							TextFile textFile = TextFile.ReadFile(item.Key);
							textFile.Text = item.Value;
							textFile.Save();
						}
						WrittenCount++;
					}
					catch (IOException exception)
					{
						monitor.ReportError(GettextCatalog.GetString("Failed to write file '{0}'.", item.Key), exception);
					}
				}
			});
			filesToWrite = null;
		}
	}
}
