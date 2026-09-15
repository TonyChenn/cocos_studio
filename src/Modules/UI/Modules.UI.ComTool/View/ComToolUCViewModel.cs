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
	internal class ComToolUCViewModel : NotificationObject
	{
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

		public List<ControlToolItem> UIList { get; private set; }

		public event EventHandler<EventArgs> DataInitied;

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

		private Xwt.Drawing.Image CreateBitmap(ModelMetaData modelMetaData)
		{
			return this.CreateDefaultUIBitmapSource(modelMetaData.Type.Name);
		}

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

		public const string ResourceFolderID = "CocoStudio.DefaultResource.ComponentResource.";

		public bool isExpanded = false;

		private IControlsViewFilter _viewFilter = DefaultControlsViewFilter.DefaultFilterInstace;
	}
}
