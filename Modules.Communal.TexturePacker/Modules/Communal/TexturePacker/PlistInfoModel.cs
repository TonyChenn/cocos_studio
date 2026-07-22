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
	// Token: 0x02000017 RID: 23
	public class PlistInfoModel : BaseObject, IDisposable
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005D29 File Offset: 0x00003F29
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00005D20 File Offset: 0x00003F20
		internal PlistInfoCocosItem CocosItem { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00005D31 File Offset: 0x00003F31
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00005D39 File Offset: 0x00003F39
		internal PlistInfoItemRender Render { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005D42 File Offset: 0x00003F42
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00005D4A File Offset: 0x00003F4A
		public List<PlistInfoItem> Items { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005D53 File Offset: 0x00003F53
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00005D5B File Offset: 0x00003F5B
		public ObservableCollection<PlistInfoItem> SelectedItems { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005D64 File Offset: 0x00003F64
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00005D6C File Offset: 0x00003F6C
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005D6E File Offset: 0x00003F6E
		public List<ImageFile> UnpackedItems
		{
			get
			{
				return this.unpackedItems;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005D76 File Offset: 0x00003F76
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00005D7E File Offset: 0x00003F7E
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005D9B File Offset: 0x00003F9B
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00005DA3 File Offset: 0x00003FA3
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005DAC File Offset: 0x00003FAC
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005DB4 File Offset: 0x00003FB4
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00005E3C File Offset: 0x0000403C
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00005E44 File Offset: 0x00004044
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005E92 File Offset: 0x00004092
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00005E9C File Offset: 0x0000409C
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005F81 File Offset: 0x00004181
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005F8C File Offset: 0x0000418C
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005FDA File Offset: 0x000041DA
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005FE4 File Offset: 0x000041E4
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006039 File Offset: 0x00004239
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00006044 File Offset: 0x00004244
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006099 File Offset: 0x00004299
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000060A4 File Offset: 0x000042A4
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000060F9 File Offset: 0x000042F9
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00006104 File Offset: 0x00004304
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00006159 File Offset: 0x00004359
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00006164 File Offset: 0x00004364
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000061B9 File Offset: 0x000043B9
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000061C4 File Offset: 0x000043C4
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006219 File Offset: 0x00004419
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00006224 File Offset: 0x00004424
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

		// Token: 0x06000102 RID: 258 RVA: 0x0000627C File Offset: 0x0000447C
		public PlistInfoModel(bool isBindingRecorder = true)
		{
			this.Init();
			if (isBindingRecorder)
			{
				base.BindingRecorder(null);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006308 File Offset: 0x00004508
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

		// Token: 0x06000104 RID: 260 RVA: 0x00006404 File Offset: 0x00004604
		private void Init()
		{
			this.Items = new List<PlistInfoItem>();
			this.SelectedItems = new ObservableCollection<PlistInfoItem>();
			this.imageFiles.CollectionChanged += this.ImageFiles_CollectionChanged;
			this.PictureRenderScale = 1.0;
			this.RealSize = new SizeValue();
			this.inited = true;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006460 File Offset: 0x00004660
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

		// Token: 0x06000106 RID: 262 RVA: 0x00006524 File Offset: 0x00004724
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000107 RID: 263 RVA: 0x00006588 File Offset: 0x00004788
		// (remove) Token: 0x06000108 RID: 264 RVA: 0x000065C0 File Offset: 0x000047C0
		public event Action<bool> ItemNameChanged;

		// Token: 0x06000109 RID: 265 RVA: 0x000065F8 File Offset: 0x000047F8
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

		// Token: 0x0600010A RID: 266 RVA: 0x00006694 File Offset: 0x00004894
		public void AddItem(ImageFile item)
		{
			if (this.ImageFiles.Contains(item))
			{
				return;
			}
			this.imageFiles.Add(item);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000066B1 File Offset: 0x000048B1
		public void RemoveItem(ImageFile item)
		{
			this.imageFiles.Remove(item);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000066C0 File Offset: 0x000048C0
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

		// Token: 0x0600010D RID: 269 RVA: 0x00006780 File Offset: 0x00004980
		public void item_Deleted(object sender, EventArgs e)
		{
			ImageFile item = sender as ImageFile;
			this.RemoveItem(item);
			this.Refresh(true);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000067A4 File Offset: 0x000049A4
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

		// Token: 0x0600010F RID: 271 RVA: 0x00006804 File Offset: 0x00004A04
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

		// Token: 0x06000110 RID: 272 RVA: 0x00006874 File Offset: 0x00004A74
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

		// Token: 0x06000111 RID: 273 RVA: 0x00006B3C File Offset: 0x00004D3C
		private bool CheckoutIsEmpty()
		{
			int num = this.ImageFiles.Count<ImageFile>() - this.UnpackedItems.Count;
			return num <= 0;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006B68 File Offset: 0x00004D68
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

		// Token: 0x0400004A RID: 74
		private ObservableCollection<ImageFile> imageFiles = new ObservableCollection<ImageFile>();

		// Token: 0x0400004B RID: 75
		private List<ImageFile> unpackedItems = new List<ImageFile>();

		// Token: 0x0400004C RID: 76
		private double picturerenderscale = 1.0;

		// Token: 0x0400004D RID: 77
		private SizeValue sizeRequest = new SizeValue();

		// Token: 0x0400004E RID: 78
		private bool inited;

		// Token: 0x0400004F RID: 79
		private SizeValue realSize = new SizeValue(32, 32);

		// Token: 0x04000050 RID: 80
		private ExportType exportType;

		// Token: 0x04000051 RID: 81
		private SortAlgorithm sortAlgorithm;

		// Token: 0x04000052 RID: 82
		private SizeValue maxSize = new SizeValue(1024, 1024);

		// Token: 0x04000053 RID: 83
		private float contentScale = 1f;

		// Token: 0x04000054 RID: 84
		private int picturePadding;

		// Token: 0x04000055 RID: 85
		private bool allowRotation = true;

		// Token: 0x04000056 RID: 86
		private bool allowAnySize;

		// Token: 0x04000057 RID: 87
		private bool allowTrim;
	}
}
