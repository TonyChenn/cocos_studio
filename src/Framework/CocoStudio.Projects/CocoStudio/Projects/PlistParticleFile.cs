using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using Modules.Communal.Packer;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class PlistParticleFile : CompositeResourceFile
	{
		internal override string PreviewImagePath
		{
			get
			{
				return null;
			}
		}

		private PlistParticleFile()
		{
		}

		public PlistParticleFile(FilePath filePath) : base(filePath)
		{
		}

		public PlistParticleFile(ResourceData resourceData) : base(resourceData)
		{
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename)
		{
			string directoryName = Path.GetDirectoryName(newFilePath);
			if (this.imageFiles != null)
			{
				try
				{
					foreach (string text in this.imageFiles)
					{
						string fileName = Path.GetFileName(text);
						string newName = Path.Combine(directoryName, fileName);
						FileService.RenameFile(text, newName);
					}
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
			}
			base.OnSetLocation(newFilePath, isRename);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			string directoryName = Path.GetDirectoryName(newMovePath);
			if (this.imageFiles != null)
			{
				try
				{
					foreach (string text in this.imageFiles)
					{
						string fileName = Path.GetFileName(text);
						string dstFile = Path.Combine(directoryName, fileName);
						FileService.MoveFile(text, dstFile);
					}
				}
				catch (Exception ex)
				{
					LogConfig.Output.Error(ex.Message, ex);
				}
			}
			base.OnMove(newMovePath);
		}

		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			if (PlistParticleFile.pairResourceProcesser == null)
			{
				PlistParticleFile.pairResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
			}
			return PlistParticleFile.pairResourceProcesser;
		}

		protected override DataError OnCheckDataError()
		{
			if (!PlistParticleReader.IsValid(this.FullPath))
			{
				return new DataError("Image file not find.");
			}
			return null;
		}

		private static ICompositeResourceProcesser pairResourceProcesser;
	}
}
