using System;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects.Text;

namespace CocoStudio.Core.View
{
	// Token: 0x02000057 RID: 87
	internal class LoadFileWrapper
	{
		// Token: 0x06000376 RID: 886 RVA: 0x0000FF4C File Offset: 0x0000E14C
		public LoadFileWrapper(IProgressMonitor monitor, MainWindow workbench, IViewDisplayBuilder binding, CocosItem project, FileOpenInfo fileInfo)
		{
			this.monitor = monitor;
			this.workbench = workbench;
			this.fileInfo = fileInfo;
			this.builder = binding;
			this.project = project;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000FF7C File Offset: 0x0000E17C
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

		// Token: 0x0400017B RID: 379
		private IViewDisplayBuilder builder;

		// Token: 0x0400017C RID: 380
		private CocosItem project;

		// Token: 0x0400017D RID: 381
		private FileOpenInfo fileInfo;

		// Token: 0x0400017E RID: 382
		private MainWindow workbench;

		// Token: 0x0400017F RID: 383
		private IProgressMonitor monitor;

		// Token: 0x04000180 RID: 384
		private IViewContentExtend newContent;
	}
}
