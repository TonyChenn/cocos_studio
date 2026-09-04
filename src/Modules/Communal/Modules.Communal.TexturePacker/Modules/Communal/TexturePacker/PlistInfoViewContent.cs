using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.Event;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using Modules.UI.ComTool;
using Modules.UI.ComTool.Model;
using MonoDevelop.Core.ProgressMonitoring;

namespace Modules.Communal.TexturePacker
{
	// Token: 0x0200000F RID: 15
	internal class PlistInfoViewContent : ViewContent
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public PlistInfoViewContent()
		{
			this.widget = (this.plistInfoWidget = new PlistInfoWidget());
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004BDB File Offset: 0x00002DDB
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00004BE3 File Offset: 0x00002DE3
		public CocosItem CocosItem
		{
			get
			{
				return this.cocosItem;
			}
			set
			{
				this.cocosItem = (value as PlistInfoCocosItem);
				this.ContentName = this.cocosItem.FileName;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004C08 File Offset: 0x00002E08
		public void Initialize(CocosItem plistInfoCocosItem)
		{
			this.CocosItem = plistInfoCocosItem;
			this.cocosItem.Load(new ConsoleProgressMonitor());
			this.plistInfoWidget.Init(this.cocosItem.PlistInfoModel);
			this.plistInfoWidget.PlistInfoModel.ItemNameChanged += this.ModelItemNameChanged;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004C5E File Offset: 0x00002E5E
		private void ModelItemNameChanged(bool isdirty)
		{
			this.IsDirty = true;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004C68 File Offset: 0x00002E68
		protected override void OnClosed()
		{
			if (this.IsDirty)
			{
				List<FilePathData> imageFiles = (this.CocosItem.CocosFile as PlistInfoCocosFile).PlistInfoData.ImageFiles;
				IEnumerable<ImageFile> imageFiles2 = (this.CocosItem as PlistInfoCocosItem).PlistInfoModel.ImageFiles;
				foreach (ImageFile imageFile in imageFiles2)
				{
					imageFile.UnPackFrom(this.CocosItem);
				}
				foreach (FilePathData filePathData in imageFiles)
				{
					(filePathData.File as ImageFile).PackTo(this.CocosItem);
				}
			}
			this.plistInfoWidget.PlistInfoModel.ItemNameChanged -= this.ModelItemNameChanged;
			this.CocosItem.UnLoad(null);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004D6C File Offset: 0x00002F6C
		public override void AfterActivated()
		{
			this.eventAggregator = Services.EventsService;
			this.UpdatePropertyUC(true);
			this._cachedToolViewFilter = Services.GetService<IComToolPad>().ControlsViewFilter;
			if (this._cachedToolViewFilter != null)
			{
				Services.GetService<IComToolPad>().ControlsViewFilter = null;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004DA3 File Offset: 0x00002FA3
		protected override void OnDeactivated()
		{
			this.UpdatePropertyUC(false);
			this.eventAggregator = null;
			Services.GetService<IComToolPad>().ControlsViewFilter = this._cachedToolViewFilter;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004DC3 File Offset: 0x00002FC3
		public override void Reload()
		{
			if (!this.cocosItem.IsLoaded)
			{
				this.cocosItem.Load(Services.ProgressMonitors.Default);
			}
			this.UpdatePropertyUC(true);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004DF0 File Offset: 0x00002FF0
		private void UpdatePropertyUC(bool isShow)
		{
			List<object> list = new List<object>();
			if (isShow)
			{
				list.Add(this.cocosItem.PlistInfoModel);
			}
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			service.IsShowTitle = false;
			service.SelectedObjects = list;
			service.IsShowTitle = true;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004E32 File Offset: 0x00003032
		private void CanvasZoomChangeHandle(CanvasZoomChangeEventArgs args)
		{
			if (this.eventAggregator != null)
			{
				this.plistInfoWidget.CanvasZoom(args.ZoomDelta);
			}
		}

		// Token: 0x04000036 RID: 54
		private PlistInfoWidget plistInfoWidget;

		// Token: 0x04000037 RID: 55
		private IEventAggregator eventAggregator;

		// Token: 0x04000038 RID: 56
		private PlistInfoCocosItem cocosItem;

		// Token: 0x04000039 RID: 57
		private IControlsViewFilter _cachedToolViewFilter;
	}
}
