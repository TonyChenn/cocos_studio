using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using Modules.Communal.Packer;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000056 RID: 86
	public class PlistParticleFile : CompositeResourceFile
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00009269 File Offset: 0x00007469
		internal override string PreviewImagePath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000926C File Offset: 0x0000746C
		private PlistParticleFile()
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009274 File Offset: 0x00007474
		public PlistParticleFile(FilePath filePath) : base(filePath)
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000927D File Offset: 0x0000747D
		public PlistParticleFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009288 File Offset: 0x00007488
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

		// Token: 0x06000261 RID: 609 RVA: 0x0000932C File Offset: 0x0000752C
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

		// Token: 0x06000262 RID: 610 RVA: 0x000093D0 File Offset: 0x000075D0
		protected override ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			if (PlistParticleFile.pairResourceProcesser == null)
			{
				PlistParticleFile.pairResourceProcesser = ProjectsService.Instance.GetCompositeResourceProcesser(this.FileName);
			}
			return PlistParticleFile.pairResourceProcesser;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000093F8 File Offset: 0x000075F8
		protected override DataError OnCheckDataError()
		{
			if (!PlistParticleReader.IsValid(this.FullPath))
			{
				return new DataError("Image file not find.");
			}
			return null;
		}

		// Token: 0x0400009B RID: 155
		private static ICompositeResourceProcesser pairResourceProcesser;
	}
}
