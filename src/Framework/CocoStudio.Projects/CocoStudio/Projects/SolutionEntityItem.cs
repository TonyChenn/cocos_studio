using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public abstract class SolutionEntityItem : SolutionItem, IWorkspaceObject, IExtendedDataItem, IFoldeItem, IDisposable
	{
		public override string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public FilePath ItemDirectory { get; set; }

		public FilePath BaseDirectory { get; set; }

		public void Save(IProgressMonitor monitor)
		{
			try
			{
				this.OnSave(monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Save failed.", exception);
			}
		}

		protected virtual void OnSave(IProgressMonitor monitor)
		{
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
