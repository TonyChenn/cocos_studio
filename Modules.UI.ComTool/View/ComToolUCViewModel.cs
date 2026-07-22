using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.UI.ComTool.Model;
using Xwt.Drawing;

namespace Modules.UI.ComTool.View
{
	// Token: 0x0200000A RID: 10
	internal class ComToolUCViewModel : NotificationObject
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002760 File Offset: 0x00000960
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002778 File Offset: 0x00000978
		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				this.isExpanded = value;
				this.RaisePropertyChanged<bool>(() => this.IsExpanded);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027C8 File Offset: 0x000009C8
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000027E0 File Offset: 0x000009E0
		internal IControlsViewFilter ViewFilter
		{
			get
			{
				return this._viewFilter;
			}
			set
			{
				this._viewFilter = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000027EC File Offset: 0x000009EC
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002803 File Offset: 0x00000A03
		public List<ControlToolItem> UIList { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000024 RID: 36 RVA: 0x0000280C File Offset: 0x00000A0C
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x00002848 File Offset: 0x00000A48
		public event EventHandler<EventArgs> DataInitied;

		// Token: 0x06000026 RID: 38 RVA: 0x00002884 File Offset: 0x00000A84
		public ComToolUCViewModel()
		{
			IEventAggregator eventsService = Services.EventsService;
			try
			{
				this.CreateModelList();
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error("Load model list failed.", exception);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002900 File Offset: 0x00000B00
		private void CreateModelList()
		{
			this.UIList = new List<ControlToolItem>();
			IEnumerable<ModelMetaData> modelCollection = ModelManager.Instance.ModelCollection;
			string empty = string.Empty;
			foreach (ModelMetaData modelMetaData in modelCollection)
			{
				Xwt.Drawing.Image bitImage = this.CreateBitmap(modelMetaData);
				ControlToolItem controlToolItem = new ControlToolItem(modelMetaData, bitImage);
				object[] customAttributes = modelMetaData.Type.GetCustomAttributes(typeof(ControlGroupAttribute), false);
				object obj = customAttributes.FirstOrDefault((object n) => n is ControlGroupAttribute);
				if (obj != null)
				{
					controlToolItem.Group = (ControlGroupAttribute)obj;
				}
				if (controlToolItem.Group == null || !modelMetaData.IsDefault)
				{
					if (controlToolItem.ModelType == EnumModelType.TwoDimension)
					{
						controlToolItem.Group = new ControlGroupAttribute("Control_Custom", 2);
					}
					else
					{
						controlToolItem.Group = new ControlGroupAttribute("Control_3DCustom", 2);
					}
				}
				this.UIList.Add(controlToolItem);
			}
			if (this.DataInitied != null)
			{
				this.DataInitied(this, null);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A68 File Offset: 0x00000C68
		private Xwt.Drawing.Image CreateBitmap(ModelMetaData modelMetaData)
		{
			return this.CreateDefaultUIBitmapSource(modelMetaData.Type.Name);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A8C File Offset: 0x00000C8C
		private Xwt.Drawing.Image CreateDefaultUIBitmapSource(string uiName)
		{
			string str = uiName.Substring(0, uiName.Length - 6);
			Xwt.Drawing.Image result;
			try
			{
				string resourceID = "CocoStudio.DefaultResource.ComponentResource." + str + ".png";
				result = ImageIcon.GetCustomControlIcon(resourceID);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400001D RID: 29
		public const string ResourceFolderID = "CocoStudio.DefaultResource.ComponentResource.";

		// Token: 0x0400001E RID: 30
		public bool isExpanded = false;

		// Token: 0x0400001F RID: 31
		private IControlsViewFilter _viewFilter = DefaultControlsViewFilter.DefaultFilterInstace;
	}
}
