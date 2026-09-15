using System;
using System.Text;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	public class FileOpenInfo
	{
		public int Line { get; set; }

		public int Column { get; set; }

		public ResourceFile Project { get; set; }

		public IViewContentExtend NewContent { get; set; }

		public FilePath FileName { get; set; }

		public bool BringToFront { get; set; }

		public IViewDisplayBuilder DisplayBuilder { get; set; }

		public Encoding Encoding { get; set; }

		public FileOpenInfo(FilePath file, ResourceFile project, bool bringToFront)
		{
			this.FileName = file;
			this.Project = project;
			this.BringToFront = bringToFront;
		}
	}
}
