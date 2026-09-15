using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Packer;
using Modules.Communal.Packer.PlistReader;
using Modules.Communal.Packer.PlistReader.Formates;
using Modules.Communal.PList;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.TexturePacker
{
	[Extension(typeof(IResource))]
	[DataItem("PlistInfo")]
	public class PlistInfoCocosItem : CocosItem
	{
		public PlistInfoModel PlistInfoModel
		{
			get
			{
				return (base.CocosFile as PlistInfoCocosFile).PlistInfoModel;
			}
		}

		public override bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		private PlistInfoCocosItem()
		{
		}

		public PlistInfoCocosItem(FilePath file) : base(file)
		{
		}

		public PlistInfoCocosItem(FilePath file, CocosFile cocosFile) : base(file, cocosFile)
		{
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			this.UnPackItem();
			base.OnDelete(monitor);
		}

		protected override void OnRemove(IProgressMonitor monitor)
		{
			this.UnPackItem();
			base.OnRemove(monitor);
		}

		private void UnPackItem()
		{
			if (base.CocosFile == null)
			{
				return;
			}
			List<FilePathData> imageFiles = (base.CocosFile as PlistInfoCocosFile).PlistInfoData.ImageFiles;
			foreach (FilePathData filePathData in imageFiles)
			{
				if (filePathData.File != null)
				{
					(filePathData.File as ImageFile).UnPackFrom(this);
				}
			}
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (base.CocosFile == null)
			{
				return;
			}
			string fullPath = this.FullPath;
			base.OnSetLocation(newFilePath, isRename);
			List<FilePathData> imageFiles = (base.CocosFile as PlistInfoCocosFile).PlistInfoData.ImageFiles;
			foreach (FilePathData filePathData in imageFiles)
			{
				if (filePathData != null)
				{
					ImageFile imageFile = filePathData.File as ImageFile;
					if (imageFile != null)
					{
						imageFile.PackedProjectNameChanged(this, fullPath);
					}
				}
			}
		}

		protected override void OnMove(FilePath newMovePath)
		{
			if (base.CocosFile == null)
			{
				return;
			}
			string fullPath = this.FullPath;
			List<FilePathData> imageFiles = (base.CocosFile as PlistInfoCocosFile).PlistInfoData.ImageFiles;
			foreach (FilePathData filePathData in imageFiles)
			{
				if (filePathData != null)
				{
					ImageFile imageFile = filePathData.File as ImageFile;
					if (imageFile != null)
					{
						imageFile.PackedProjectNameChanged(this, fullPath);
					}
				}
			}
			base.OnMove(newMovePath);
		}

		protected override void OnPublish(IProgressMonitor monitor, PublishInfo info)
		{
			bool flag = this.PlistInfoModel != null;
			try
			{
				if (!File.Exists(base.CocosFile.FileName.FullPath))
				{
					if (!flag)
					{
						string message = string.Format("{0} {1} : {2}", LanguageInfo.Command_ExportMergeImage, LanguageInfo.FailedToLoadFile, base.CocosFile.FileName.FullPath);
						LogConfig.OutputWithoutTip.Error(message);
						throw new InvalidOperationException(message);
					}
					base.CocosFile.Save(monitor);
				}
				if (!flag)
				{
					lock (PlistInfoCocosItem.lockTag)
					{
						base.CocosFile.Load(monitor);
					}
					this.PlistInfoModel.CalculateItemPosition();
				}
				this.ExportPlist(monitor, info.PublishDirectory, true);
				if (this.PlistInfoModel.UnpackedItems.Count > 0)
				{
					Services.Workbench.Pads.OutputPad.BringToFront();
					string empty = string.Empty;
					foreach (ImageFile imageFile in this.PlistInfoModel.UnpackedItems)
					{
						if (imageFile != null && !string.IsNullOrEmpty(imageFile.FullPath))
						{
							string message2 = string.Format(LanguageInfo.Output_TextureWontPublishWithPlist, imageFile.FullPath, base.CocosFile.CocosItem.FullPath);
							LogConfig.OutputWithoutTip.Info(message2, true);
						}
					}
				}
			}
			finally
			{
				if (!flag)
				{
					base.CocosFile.UnLoad(monitor);
					System.GC.Collect(0);
				}
			}
		}

		public string GetExportPathWithoutExtension(string exportPath, bool combineRelativePath = true)
		{
			string relativePath = base.RelativePath;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
			string text = exportPath;
			if (combineRelativePath)
			{
				text = Path.GetDirectoryName(Path.Combine(text, relativePath));
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return Path.Combine(text, fileNameWithoutExtension);
		}

		public void ExportPlist(IProgressMonitor monitor, string exportPath, bool combineRelativePath = true)
		{
			this.maxSize = this.PlistInfoModel.RealSize;
			string exportPathWithoutExtension = this.GetExportPathWithoutExtension(exportPath, combineRelativePath);
			Pixbuf gMap = null;
			try
			{
				System.GC.Collect();
				Bitmap bitmap = new Bitmap(Math.Min(this.maxSize.Width, this.maxSize.Width), Math.Min(this.maxSize.Height, this.maxSize.Height));
				byte[] buffer = new byte[this.maxSize.Width * this.maxSize.Height * 4 + 1024];
				MemoryStream memoryStream = new MemoryStream(buffer);
				if (this.PlistInfoModel.ExportType == ExportType.Png)
				{
					bitmap.Save(memoryStream, ImageFormat.Png);
				}
				else if (this.PlistInfoModel.ExportType == ExportType.Jpeg)
				{
					bitmap.Save(memoryStream, ImageFormat.Jpeg);
				}
				bitmap.Dispose();
				long position = memoryStream.Position;
				memoryStream.Seek(0L, SeekOrigin.Begin);
				gMap = new Pixbuf(memoryStream);
				Parallel.ForEach<PlistInfoItem>(this.PlistInfoModel.Items, delegate(PlistInfoItem item)
				{
					try
					{
						item.DrawToImage(gMap, this.PlistInfoModel.ExportType == ExportType.Jpeg);
					}
					catch (Exception ex2)
					{
						LogConfig.OutputWithoutTip.Error(ex2.Message);
						monitor.ReportError(ex2.Message, ex2);
					}
				});
				if (!monitor.AsyncOperation.Success)
				{
					return;
				}
			}
			catch (FileNotFoundException ex)
			{
				if (ApplicationCurrent.MainWindow == null)
				{
					LogConfig.Logger.Error(LanguageInfo.MessageBox_Content173 + ex.FileName, ex);
				}
				else
				{
					MessageBox.Show(LanguageInfo.MessageBox_Content173 + "\r\n" + ex.FileName, null, MessageBoxImage.Error, null, EnumMainButton.Yes, null);
				}
				return;
			}
			string text = exportPathWithoutExtension + ".plist";
			if (File.Exists(text))
			{
				LogConfig.Logger.Info(LanguageInfo.Dialog_ButtonReplace + ": " + text, true);
				LogConfig.Logger.Info(LanguageInfo.MessageBox_Content70, true);
				string publishDirectory = Services.ProjectOperations.CurrentSelectedSolution.PublishDirectory;
				if (!exportPath.Equals(publishDirectory, StringComparison.OrdinalIgnoreCase) && ApplicationCurrent.MainWindow != null)
				{
					MessageBoxResult messageBoxResult = MessageBox.Show(text + ":\r\n" + LanguageInfo.MessageBox_Content44, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null);
					if (messageBoxResult != MessageBoxResult.Yes)
					{
						return;
					}
				}
			}
			this.SaveImage(this.PlistInfoModel, gMap, exportPathWithoutExtension);
			this.SavePlist(this.PlistInfoModel, text);
			gMap.Dispose();
			System.GC.Collect();
		}

		private static string ConvertPlistPngToExport(PlistInfoModel model, string path)
		{
			switch (model.ExportType)
			{
			case ExportType.Png:
				path += ".png";
				break;
			case ExportType.Jpeg:
				path += ".jpg";
				break;
			}
			return path;
		}

		private void SaveImage(PlistInfoModel model, Pixbuf imageFile, string path)
		{
			path = PlistInfoCocosItem.ConvertPlistPngToExport(model, path);
			switch (model.ExportType)
			{
			case ExportType.Png:
				PixbufHelper.Save(imageFile, path, "png");
				return;
			case ExportType.Jpeg:
				PixbufHelper.Save(imageFile, path, "jpeg");
				return;
			default:
				return;
			}
		}

		private void SavePlist(PlistInfoModel model, string path)
		{
			List<ImageInfo> list = new List<ImageInfo>();
			foreach (PlistInfoItem plistInfoItem in model.Items)
			{
				if (!this.PlistInfoModel.UnpackedItems.Contains(plistInfoItem.ResourceItem))
				{
					ImageInfo imageInfo = plistInfoItem.GetImageInfo();
					list.Add(imageInfo);
				}
			}
			System.Drawing.Size size = new System.Drawing.Size(this.maxSize.Width, this.maxSize.Height);
			string text = Path.GetFileNameWithoutExtension(path);
			text = PlistInfoCocosItem.ConvertPlistPngToExport(model, text);
			PlistImageFormat plistImageFormat = PlistImageFormatFactory.CreatePlistFormat(PlistFormats.Cocos2d);
			PListRoot plistRoot = plistImageFormat.ToPlist(list, size, text);
			plistRoot.Save(path, PListFormat.Xml);
		}

		public const string PlistCocosFileSuffix = ".csi";

		private static readonly object lockTag = new object();

		private SizeValue maxSize;
	}
}
