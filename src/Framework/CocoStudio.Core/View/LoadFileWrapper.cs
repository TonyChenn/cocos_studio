using System;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects.Text;

namespace CocoStudio.Core.View
{
	internal class LoadFileWrapper
	{
		public LoadFileWrapper(IProgressMonitor monitor, MainWindow workbench, IViewDisplayBuilder binding, CocosItem project, FileOpenInfo fileInfo)
		{
			this.monitor = monitor;
			this.workbench = workbench;
			this.fileInfo = fileInfo;
			this.builder = binding;
			this.project = project;
		}

		public void Invoke(string fileName)
		{
			try
			{
				if (this.builder.CanHandle(fileName, null, this.project))
				{
					this.newContent = this.builder.CreateContent(fileName, null, this.project);
					if (this.newContent == null)
					{
						this.monitor.ReportError(string.Format("The file '{0}' could not be opened.", fileName), null);
					}
				}
				if (this.project != null)
				{
					this.newContent.Project = CocosProject.Instance;
					this.newContent.File = this.project;
				}
				IEncodedTextContent encodedTextContent = (IEncodedTextContent)this.newContent.GetContent(typeof(IEncodedTextContent));
				try
				{
					if (this.fileInfo.Encoding != null && encodedTextContent != null)
					{
						encodedTextContent.Load(fileName, this.fileInfo.Encoding);
					}
					else
					{
						this.newContent.Load(fileName);
					}
				}
				catch (InvalidEncodingException ex)
				{
					this.monitor.ReportError(string.Format("The file '{0}' could not opened. {1}", fileName, ex.Message), null);
					return;
				}
				catch (OverflowException)
				{
					this.monitor.ReportError(string.Format("The file '{0}' could not opened. File too large.", fileName), null);
					return;
				}
			}
			catch (Exception exception)
			{
				this.monitor.ReportError("", exception);
			}
			if (this.newContent.WorkbenchWindow != null)
			{
				this.newContent.WorkbenchWindow.SelectWindow();
				this.fileInfo.NewContent = this.newContent;
			}
			else
			{
				this.workbench.ShowView(this.newContent, this.fileInfo.BringToFront);
				this.newContent.WorkbenchWindow.DocumentType = this.builder.Name;
				this.fileInfo.NewContent = this.newContent;
			}
		}

		private IViewDisplayBuilder builder;

		private CocosItem project;

		private FileOpenInfo fileInfo;

		private MainWindow workbench;

		private IProgressMonitor monitor;

		private IViewContentExtend newContent;
	}
}
