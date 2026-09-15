using System;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[Obsolete("This class is just to read old solution, do not use this class.")]
	[Extension(Type = typeof(IResource))]
	[DataItem(Name = "Material")]
	internal class MaterialFile : ResourceFile
	{
		private MaterialFile()
		{
		}

		public MaterialFile(FilePath fileName) : base(fileName)
		{
		}

		public MaterialFile(ResourceData resourceData) : base(resourceData)
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
