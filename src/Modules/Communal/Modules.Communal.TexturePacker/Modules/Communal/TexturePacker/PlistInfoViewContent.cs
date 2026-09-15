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
	internal class PlistInfoViewContent : ViewContent
	{
		public PlistInfoViewContent()
		{
			this.widget = (this.plistInfoWidget = new PlistInfoWidget());
		}

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

		public void Initialize(CocosItem plistInfoCocosItem)
		{
			this.CocosItem = plistInfoCocosItem;
			this.cocosItem.Load(new ConsoleProgressMonitor());
			this.plistInfoWidget.Init(this.cocosItem.PlistInfoModel);
			this.plistInfoWidget.PlistInfoModel.ItemNameChanged += this.ModelItemNameChanged;
		}

		private void ModelItemNameChanged(bool isdirty)
		{
			this.IsDirty = true;
		}

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

		protected override void OnDeactivated()
		{
			this.UpdatePropertyUC(false);
			this.eventAggregator = null;
			Services.GetService<IComToolPad>().ControlsViewFilter = this._cachedToolViewFilter;
		}

		public override void Reload()
		{
			if (!this.cocosItem.IsLoaded)
			{
				this.cocosItem.Load(Services.ProgressMonitors.Default);
			}
			this.UpdatePropertyUC(true);
		}

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

		private void CanvasZoomChangeHandle(CanvasZoomChangeEventArgs args)
		{
			if (this.eventAggregator != null)
			{
				this.plistInfoWidget.CanvasZoom(args.ZoomDelta);
			}
		}

		private PlistInfoWidget plistInfoWidget;

		private IEventAggregator eventAggregator;

		private PlistInfoCocosItem cocosItem;

		private IControlsViewFilter _cachedToolViewFilter;
	}
}
