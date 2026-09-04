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
	// Token: 0x02000113 RID: 275
	[EngineClassName("PageView")]
	[ControlGroup("Control_Container", 2)]
	[DisplayName("Display_Component_UIPageView")]
	[ModelExtension(true, 53)]
	public class PageViewObject : PanelObject, ICallBackEvent
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x00028C04 File Offset: 0x00026E04
		private CSPageView GetInnerWidget()
		{
			return (CSPageView)this.innerNode;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00028C21 File Offset: 0x00026E21
		public PageViewObject()
		{
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00028C2C File Offset: 0x00026E2C
		public PageViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00028C38 File Offset: 0x00026E38
		protected override void CreateCSObject()
		{
			this.innerNode = new CSPageView();
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00028C48 File Offset: 0x00026E48
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

		// Token: 0x06000A39 RID: 2617 RVA: 0x00028CC8 File Offset: 0x00026EC8
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

		// Token: 0x06000A3A RID: 2618 RVA: 0x00028D2E File Offset: 0x00026F2E
		internal override void RemoveChild(AbstractNodeObject nObject)
		{
			base.RemoveChild(nObject);
			nObject.InitOperation();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00028D40 File Offset: 0x00026F40
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

		// Token: 0x06000A3C RID: 2620 RVA: 0x00028DA0 File Offset: 0x00026FA0
		public override bool CanReceiveDragResource(ResourceInfoDragData objectData, bool showLog)
		{
			if (showLog)
			{
				LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
			}
			return false;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00028DCC File Offset: 0x00026FCC
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

		// Token: 0x06000A3E RID: 2622 RVA: 0x00028E28 File Offset: 0x00027028
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}
	}
}
