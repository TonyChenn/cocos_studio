using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.Editor;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Packer.Model;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace Modules.Communal.TexturePacker
{
	public class PlistInfoModel : BaseObject, IDisposable
	{
		internal PlistInfoCocosItem CocosItem { get; set; }

		internal PlistInfoItemRender Render { get; set; }

		public List<PlistInfoItem> Items { get; private set; }

		public ObservableCollection<PlistInfoItem> SelectedItems { get; private set; }

		[UndoProperty]
		public IEnumerable<ImageFile> ImageFiles
		{
			get
			{
				return this.imageFiles;
			}
			set
			{
			}
		}

		public List<ImageFile> UnpackedItems
		{
			get
			{
				return this.unpackedItems;
			}
		}

		public double PictureRenderScale
		{
			get
			{
				return this.picturerenderscale;
			}
			set
			{
				this.picturerenderscale = value;
				if (this.Render != null)
				{
					this.Render.Scale = value;
				}
			}
		}

		public SizeValue SizeRequest
		{
			get
			{
				return this.sizeRequest;
			}
			set
			{
				this.sizeRequest = value;
			}
		}

		[Category("Group_Routine")]
		[UndoProperty]
		[DisplayName("Display_Name")]
		[Browsable(false)]
		public override string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.RaisePropertyChanged<string>(() => this.Name);
				this.RaisePropertyChanged<string>(() => this.DisplayName);
			}
		}

		[PropertyOrder(-1)]
		[Editor(typeof(SizeLabelEditor), typeof(SizeLabelEditor))]
		[DisplayName("UI_ControlLayout_txtSize")]
		[Category("Group_Routine")]
		public SizeValue RealSize
		{
			get
			{
				return this.realSize;
			}
			set
			{
				this.realSize = value;
				this.RaisePropertyChanged<SizeValue>(() => this.RealSize);
			}
		}

		[Editor(typeof(ExportButtonEditor), typeof(ExportButtonEditor))]
		[DisplayName("")]
		[Category("Group_Routine")]
		[PropertyOrder(8)]
		public string Export
		{
			get
			{
				return "";
			}
			set
			{
				if (this.CheckoutIsEmpty())
				{
					MessageBox.Show(LanguageInfo.MessageBox230_ProjectEmptyNoExport, MessageBoxImage.Other, null, null);
					return;
				}
				SelectFolderDialog selectFolderDialog = new SelectFolderDialog();
				selectFolderDialog.TransientFor = MessageService.GetDefaultModalParent();
				selectFolderDialog.Action = FileChooserAction.SelectFolder;
				selectFolderDialog.SelectMultiple = false;
				selectFolderDialog.CurrentFolder = Services.RecentFileService.LastExportDir;
				if (selectFolderDialog.Run())
				{
					if (!string.IsNullOrEmpty(selectFolderDialog.SelectedFile))
					{
						Services.RecentFileService.LastExportDir = selectFolderDialog.SelectedFile;
					}
					string text = selectFolderDialog.SelectedFile;
					if (Directory.Exists(text))
					{
						IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
						PublishInfo info = new PublishInfo(new FilePath(text), PublishType.Reference);
						this.CocosItem.Save(consoleProgressMonitor);
						Services.Workbench.ActiveDocument.IsDirty = false;
						this.CocosItem.Publish(consoleProgressMonitor, info);
					}
				}
			}
		}

		[DisplayName("Display_ExportType")]
		[UndoProperty]
		[DefaultValue(ExportType.Png)]
		[Category("Group_Routine")]
		[PropertyOrder(4)]
		public ExportType ExportType
		{
			get
			{
				return this.exportType;
			}
			set
			{
				this.exportType = value;
				this.RaisePropertyChanged<ExportType>(() => this.ExportType);
			}
		}

		public SortAlgorithm SortAlgorithm
		{
			get
			{
				return this.sortAlgorithm;
			}
			set
			{
				this.sortAlgorithm = value;
				this.RaisePropertyChanged<SortAlgorithm>(() => this.SortAlgorithm);
				this.Refresh(false);
			}
		}

		[UndoProperty]
		[PropertyOrder(3)]
		[DisplayName("Display_MaxSize")]
		[Category("Group_Routine")]
		[ValueRange(32, 4096, 1f, 10f)]
		[Editor(typeof(SpriteSheetSizeEditor), typeof(SpriteSheetSizeEditor))]
		public SizeValue MaxSize
		{
			get
			{
				return this.maxSize;
			}
			set
			{
				this.maxSize = value;
				this.Refresh(false);
				this.RaisePropertyChanged<SizeValue>(() => this.MaxSize);
			}
		}

		public float ContentScale
		{
			get
			{
				return this.contentScale;
			}
			set
			{
				this.contentScale = value;
				this.Refresh(false);
				this.RaisePropertyChanged<float>(() => this.ContentScale);
			}
		}

		[ValueRange(0, 200, 1f, 10f)]
		[PropertyOrder(5)]
		[UndoProperty]
		[Editor(typeof(PaddingEditor), typeof(PaddingEditor))]
		[DisplayName("Display_PicturePadding")]
		[Category("Group_Routine")]
		public int PicturePadding
		{
			get
			{
				return this.picturePadding;
			}
			set
			{
				this.picturePadding = value;
				this.Refresh(false);
				this.RaisePropertyChanged<int>(() => this.PicturePadding);
			}
		}

		[UndoProperty]
		[PropertyOrder(6)]
		[DisplayName("Display_AllowRotation")]
		[Category("Group_Routine")]
		public bool AllowRotation
		{
			get
			{
				return this.allowRotation;
			}
			set
			{
				this.allowRotation = value;
				this.Refresh(false);
				this.RaisePropertyChanged<bool>(() => this.AllowRotation);
			}
		}

		[PropertyOrder(1)]
		[UndoProperty]
		[DisplayName("Display_AllowAnySize")]
		[Category("Group_Routine")]
		public bool AllowAnySize
		{
			get
			{
				return this.allowAnySize;
			}
			set
			{
				this.allowAnySize = value;
				this.Refresh(false);
				this.RaisePropertyChanged<bool>(() => this.AllowAnySize);
			}
		}

		[UndoProperty]
		[Category("Group_Routine")]
		[PropertyOrder(2)]
		[DisplayName("Display_AllowTrim")]
		public bool AllowTrim
		{
			get
			{
				return this.allowTrim;
			}
			set
			{
				this.allowTrim = value;
				this.Refresh(false);
				this.RaisePropertyChanged<bool>(() => this.AllowTrim);
			}
		}

		public PlistInfoModel(bool isBindingRecorder = true)
		{
			this.Init();
			if (isBindingRecorder)
			{
				base.BindingRecorder(null);
			}
		}

		public PlistInfoModel(PlistInfoData data) : this(false)
		{
			this.AllowAnySize = data.AllowAnySize;
			this.AllowRotation = data.AllowRotation;
			this.AllowTrim = data.AllowTrim;
			this.ExportType = data.ExportType;
			this.MaxSize = data.MaxSize;
			this.SortAlgorithm = data.SortAlgorithm;
			this.PicturePadding = data.PicturePadding;
			foreach (FilePathData filePathData in data.ImageFiles)
			{
				if (!File.Exists(filePathData.File.FullPath))
				{
					LogConfig.Logger.Error("PlistFlie Load Error: " + filePathData.File.FullPath + " Can not be Found!");
				}
				this.imageFiles.Add(filePathData.File as ImageFile);
			}
			base.BindingRecorder(null);
		}

		private void Init()
		{
			this.Items = new List<PlistInfoItem>();
			this.SelectedItems = new ObservableCollection<PlistInfoItem>();
			this.imageFiles.CollectionChanged += this.ImageFiles_CollectionChanged;
			this.PictureRenderScale = 1.0;
			this.RealSize = new SizeValue();
			this.inited = true;
		}

		private void ImageFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (object obj in e.NewItems)
				{
					ImageFile item = obj as ImageFile;
					this.OnAddItem(item);
				}
			}
			if (e.OldItems != null)
			{
				foreach (object obj2 in e.OldItems)
				{
					ImageFile item2 = obj2 as ImageFile;
					this.OnRemoveItem(item2);
				}
			}
		}

		private void OnAddItem(ImageFile item)
		{
			item.Deleted += this.item_Deleted;
			item.ContentChanged += this.OnItem_NameChanged;
			item.PackTo(this.CocosItem);
			PlistInfoItem item2 = new PlistInfoItem(this, item);
			this.Items.Add(item2);
			if (Services.TaskService.IsUndoing)
			{
				this.Refresh(true);
			}
		}

		public event Action<bool> ItemNameChanged;

		private void OnItem_NameChanged(object sender, EventArgs e)
		{
			if (this.ItemNameChanged != null)
			{
				this.ItemNameChanged(true);
			}
			ImageFile imageFile = sender as ImageFile;
			if (imageFile != null && this.ImageFiles.Contains(imageFile))
			{
				foreach (PlistInfoItem plistInfoItem in this.Items)
				{
					if ((plistInfoItem.ResourceItem as ImageFile).Equals(imageFile))
					{
						plistInfoItem.ReloadFile();
						this.Refresh(true);
						break;
					}
				}
			}
		}

		public void AddItem(ImageFile item)
		{
			if (this.ImageFiles.Contains(item))
			{
				return;
			}
			this.imageFiles.Add(item);
		}

		public void RemoveItem(ImageFile item)
		{
			this.imageFiles.Remove(item);
		}

		private void OnRemoveItem(ImageFile item)
		{
			item.UnPackFrom(this.CocosItem);
			item.Deleted -= this.item_Deleted;
			item.ContentChanged -= this.OnItem_NameChanged;
			PlistInfoItem plistInfoItem = null;
			foreach (PlistInfoItem plistInfoItem2 in this.Items)
			{
				if ((plistInfoItem2.ResourceItem as ImageFile).Equals(item))
				{
					plistInfoItem = plistInfoItem2;
					break;
				}
			}
			if (plistInfoItem != null)
			{
				this.Items.Remove(plistInfoItem);
			}
			plistInfoItem.ReleaseRef();
			if (Services.TaskService.IsUndoing)
			{
				this.Refresh(true);
			}
		}

		public void item_Deleted(object sender, EventArgs e)
		{
			ImageFile item = sender as ImageFile;
			this.RemoveItem(item);
			this.Refresh(true);
		}

		public void Refresh(bool imageFilesChanged = true)
		{
			if (!this.inited)
			{
				return;
			}
			if (imageFilesChanged)
			{
				this.RaisePropertyChanged<IEnumerable<ImageFile>>(() => this.ImageFiles);
			}
			this.CalculateItemPosition();
			this.RefreshRender();
		}

		public void RefreshRender()
		{
			if (this.Render == null)
			{
				return;
			}
			this.Render.WidthRequest = Math.Max(this.SizeRequest.Width, this.MaxSize.Width);
			this.Render.HeightRequest = Math.Max(this.SizeRequest.Height, this.MaxSize.Height);
			this.Render.QueueDraw();
		}

		public void CalculateItemPosition()
		{
			int num = this.PicturePadding * 2;
			MaxRectsBinPack maxRectsBinPack = new MaxRectsBinPack(this.MaxSize.Width, this.MaxSize.Height, this.AllowRotation);
			List<CustomRectangle> list = new List<CustomRectangle>();
			for (int i = 0; i < this.Items.Count; i++)
			{
				PlistInfoItem plistInfoItem = this.Items[i];
				int num2 = this.allowTrim ? plistInfoItem.Width : plistInfoItem.SourceWidth;
				int num3 = this.allowTrim ? plistInfoItem.Height : plistInfoItem.SourceHeight;
				list.Add(new CustomRectangle(0, 0, num2 + num, num3 + num, plistInfoItem));
			}
			maxRectsBinPack.Insert(list, new List<CustomRectangle>(), MaxRectsBinPack.FreeRectChoiceHeuristic.RectBestAreaFit);
			int num4 = maxRectsBinPack.RightEdge + 1;
			int num5 = maxRectsBinPack.BottomEdge + 1;
			for (int j = 0; j < maxRectsBinPack.usedRectangles.Count; j++)
			{
				CustomRectangle customRectangle = maxRectsBinPack.usedRectangles[j];
				PlistInfoItem plistInfoItem2 = customRectangle.UserData as PlistInfoItem;
				plistInfoItem2.SourceLocation = new Point(customRectangle.Location.X + num / 2, customRectangle.Location.Y + num / 2);
				plistInfoItem2.Scale = (double)this.ContentScale;
				plistInfoItem2.Rotate = customRectangle.Rotated;
			}
			if (!this.AllowAnySize)
			{
				num4 = PlistInfoWidget.GetBinaryNum(num4);
				num5 = PlistInfoWidget.GetBinaryNum(num5);
			}
			if (num4 == 0)
			{
				num4 = 32;
			}
			if (num5 == 0)
			{
				num5 = 32;
			}
			this.RealSize = new SizeValue(num4, num5);
			this.UnpackedItems.Clear();
			SizeValue sizeValue = new SizeValue(0, num5);
			int num6 = num5;
			for (int k = 0; k < list.Count; k++)
			{
				PlistInfoItem plistInfoItem3 = list[k].UserData as PlistInfoItem;
				this.UnpackedItems.Add(plistInfoItem3.ResourceItem as ImageFile);
				plistInfoItem3.SourceLocation = new Point(sizeValue.Width, sizeValue.Height);
				sizeValue.Width = plistInfoItem3.RenderRect.Right + 1;
				if (sizeValue.Width > num4)
				{
					plistInfoItem3.SourceLocation = new Point(0, num6);
					sizeValue.Width = plistInfoItem3.RenderRect.Right + 1;
					sizeValue.Height = num6;
				}
				if (plistInfoItem3.RenderRect.Bottom + 1 > num6)
				{
					num6 = plistInfoItem3.RenderRect.Bottom + 1;
				}
			}
			sizeValue.Height = num6;
			int width = (sizeValue.Width > num4) ? sizeValue.Width : num4;
			int height = (sizeValue.Height > num5) ? sizeValue.Height : num5;
			this.SizeRequest = new SizeValue(width, height);
		}

		private bool CheckoutIsEmpty()
		{
			int num = this.ImageFiles.Count<ImageFile>() - this.UnpackedItems.Count;
			return num <= 0;
		}

		public void Dispose()
		{
			for (int i = this.imageFiles.Count - 1; i >= 0; i--)
			{
				ImageFile imageFile = this.imageFiles[i];
				imageFile.Deleted -= this.item_Deleted;
				imageFile.ContentChanged -= this.OnItem_NameChanged;
			}
			this.imageFiles.CollectionChanged -= this.ImageFiles_CollectionChanged;
			this.imageFiles.Clear();
			foreach (PlistInfoItem plistInfoItem in this.Items)
			{
				plistInfoItem.ReleaseRef();
			}
			this.Items.Clear();
			this.SelectedItems.Clear();
			this.unpackedItems.Clear();
		}

		private ObservableCollection<ImageFile> imageFiles = new ObservableCollection<ImageFile>();

		private List<ImageFile> unpackedItems = new List<ImageFile>();

		private double picturerenderscale = 1.0;

		private SizeValue sizeRequest = new SizeValue();

		private bool inited;

		private SizeValue realSize = new SizeValue(32, 32);

		private ExportType exportType;

		private SortAlgorithm sortAlgorithm;

		private SizeValue maxSize = new SizeValue(1024, 1024);

		private float contentScale = 1f;

		private int picturePadding;

		private bool allowRotation = true;

		private bool allowAnySize;

		private bool allowTrim;
	}
}
