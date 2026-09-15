using System;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "Mesh")]
	public class MeshFile : CompositeResourceFile
	{
		private MeshFile()
		{
		}

		public MeshFile(FilePath fileName) : base(fileName)
		{
		}

		public MeshFile(ResourceData resourceData) : base(resourceData)
		{
		}

		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			return base.CreateResourceData(filePath);
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
		}
	}
}
