using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	public class WorkspaceItemDescriptor : CustomDescriptor
	{
		private WorkspaceItem item;

		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Name")]
		[LocalizedDescription("Name of the item.")]
		public string Name
		{
			get
			{
				return item.Name;
			}
			set
			{
				IdeApp.ProjectOperations.RenameItem(item, value);
			}
		}

		[LocalizedDisplayName("File Path")]
		[LocalizedCategory("Misc")]
		[LocalizedDescription("File path of the item.")]
		public string FilePath => item.FileName;

		[LocalizedDescription("Root directory of source files and projects. File paths will be shown relative to this directory.")]
		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Root Directory")]
		public string RootDirectory
		{
			get
			{
				return item.BaseDirectory;
			}
			set
			{
				if (string.IsNullOrEmpty(value) || Directory.Exists(value))
				{
					item.BaseDirectory = value;
				}
			}
		}

		[LocalizedDisplayName("File Format")]
		[LocalizedDescription("File format of the project file.")]
		[LocalizedCategory("Misc")]
		public string FileFormat => item.FileFormat.Name;

		public WorkspaceItemDescriptor(WorkspaceItem item)
		{
			this.item = item;
		}
	}
}
