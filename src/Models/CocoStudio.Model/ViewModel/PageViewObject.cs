using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[EngineClassName("PageView")]
	[ControlGroup("Control_Container", 2)]
	[DisplayName("Display_Component_UIPageView")]
	[ModelExtension(true, 53)]
	public class PageViewObject : PanelObject, ICallBackEvent
	{
		private CSPageView GetInnerWidget()
		{
			return (CSPageView)this.innerNode;
		}

		public PageViewObject()
		{
		}

		public PageViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSPageView();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				base.SingleColor = Color.FromArgb(255, 150, 150, 100);
				base.FirstColor = Color.FromArgb(255, 150, 150, 100);
				base.EndColor = Color.FromArgb(255, 255, 255, 255);
				base.ComboBoxType = PanelColorFillType.Color_solid;
			}
		}

		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			PanelObject panelObject = nObject as PanelObject;
			if (panelObject != null)
			{
				panelObject.HorizontalEdge = HorizontalBerthEdge.None;
				panelObject.VerticalEdge = VerticalBerthEdge.None;
				panelObject.StretchWidthEnable = false;
				panelObject.StretchHeightEnable = false;
				this.GetCSVisual().InsertChild(index, nObject.GetCSVisual());
				nObject.OperationFlag = OperationMask.VisibleFlag;
				nObject.Parent = this;
			}
		}

		internal override void RemoveChild(AbstractNodeObject nObject)
		{
			base.RemoveChild(nObject);
			nObject.InitOperation();
		}

		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			bool result;
			if (!base.CanReceiveDragObject(objectData, showLog))
			{
				result = false;
			}
			else if (!objectData.MetaData.Type.Equals(typeof(PanelObject)))
			{
				if (showLog)
				{
					LogConfig.Logger.Error(LanguageInfo.OutputMessage);
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public override bool CanReceiveDragResource(ResourceInfoDragData objectData, bool showLog)
		{
			if (showLog)
			{
				LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
			}
			return false;
		}

		public override bool CanDrop(object node, TreeViewDropPosition mode, bool copy)
		{
			if (mode != TreeViewDropPosition.After && mode != TreeViewDropPosition.Before)
			{
				if (!node.GetType().Equals(typeof(PanelObject)))
				{
					LogConfig.Logger.Error(LanguageInfo.OutputMessage);
					return false;
				}
			}
			return base.CanDrop(node, mode, copy);
		}

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}
	}
}
