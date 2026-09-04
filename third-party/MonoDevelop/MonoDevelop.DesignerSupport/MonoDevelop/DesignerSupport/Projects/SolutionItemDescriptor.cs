using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class SolutionItemDescriptor : CustomDescriptor
	{
		private SolutionItem entry;

		[LocalizedDescription("Name of the solution item.")]
		[LocalizedDisplayName("Name")]
		[LocalizedCategory("Misc")]
		public string Name
		{
			get
			{
				return entry.Name;
			}
			set
			{
				if (entry is IWorkspaceFileObject)
				{
					IdeApp.ProjectOperations.RenameItem((IWorkspaceFileObject)entry, value);
				}
				else
				{
					entry.Name = value;
				}
			}
		}

		[LocalizedDescription("File path of the solution item.")]
		[LocalizedDisplayName("File Path")]
		[LocalizedCategory("Misc")]
		public string FilePath
		{
			get
			{
				if (entry is SolutionEntityItem)
				{
					return ((SolutionEntityItem)entry).FileName;
				}
				return "";
			}
		}

		[LocalizedDisplayName("Root Directory")]
		[LocalizedDescription("Root directory of source files and projects. File paths will be shown relative to this directory.")]
		[LocalizedCategory("Misc")]
		public string RootDirectory
		{
			get
			{
				return entry.BaseDirectory;
			}
			set
			{
				if (string.IsNullOrEmpty(value) || Directory.Exists(value))
				{
					entry.BaseDirectory = value;
				}
			}
		}

		public SolutionItemDescriptor(SolutionItem entry)
		{
			this.entry = entry;
		}
	}
}
