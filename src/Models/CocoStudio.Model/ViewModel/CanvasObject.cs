using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.Projects.Visiter;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200011A RID: 282
	[DisplayName("Display_Component_Canvas")]
	public class CanvasObject : VisualObject
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0002AC74 File Offset: 0x00028E74
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0002AC8B File Offset: 0x00028E8B
		public ObservableCollection<AbstractNodeObject> Children { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002AC94 File Offset: 0x00028E94
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x0002ACB4 File Offset: 0x00028EB4
		[Category("Group_Routine")]
		public override SizeF Size
		{
			get
			{
				return this.canvasEntity.GetSize();
			}
			set
			{
				this.canvasEntity.SetSize(value);
				this.RaisePropertyChanged<SizeF>(() => this.Size);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0002AD0C File Offset: 0x00028F0C
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x0002AD2C File Offset: 0x00028F2C
		public override ScaleValue Scale
		{
			get
			{
				return this.canvasEntity.GetScale();
			}
			set
			{
				if (this.CheckScaleValue(value) && !this.scale.Equals(value))
				{
					this.scale = value;
					this.canvasEntity.SetScale(this.scale);
					this.RaisePropertyChanged<ScaleValue>(() => this.Scale);
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0002ADB0 File Offset: 0x00028FB0
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x0002ADD0 File Offset: 0x00028FD0
		public override PointF Position
		{
			get
			{
				return this.GetCSVisual().GetPosition();
			}
			set
			{
				this.GetCSVisual().SetPosition(value);
				this.RaisePropertyChanged<PointF>(() => this.Position, false);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0002AE28 File Offset: 0x00029028
		private AbstractNodeObject CurrentNodeObject
		{
			get
			{
				return Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			}
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002AE4C File Offset: 0x0002904C
		internal CanvasObject(CSCanvas canvasEntity)
		{
			this.canvasEntity = canvasEntity;
			this.CanEdit = false;
			this.IsSelected = false;
			this.sceneSize = Services.ProjectOperations.CurrentSelectedSolution.GetSceneSize();
			this.Size = new SizeF(this.sceneSize.Width, this.sceneSize.Height);
			base.BindingRecorder(null);
			this.Children = new ObservableCollection<AbstractNodeObject>();
			this.Children.CollectionChanged += this.ChildrenCollectionChangedHandle;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002AEFC File Offset: 0x000290FC
		protected override void OnDragDrop(DragDropArgs e)
		{
			DragOperationManager.Current.DragDrop(e, this.CurrentNodeObject);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002AF14 File Offset: 0x00029114
		internal override CSVisualObject GetCSVisual()
		{
			return this.canvasEntity;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002AF2C File Offset: 0x0002912C
		public override IEnumerable<VisualObject> GetVisualChildren()
		{
			return this.Children;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002AF44 File Offset: 0x00029144
		private void ChildrenCollectionChangedHandle(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				int num = e.NewStartingIndex;
				foreach (object obj in e.NewItems)
				{
					AbstractNodeObject abstractNodeObject = obj as AbstractNodeObject;
					this.GetCSVisual().InsertChild(num, abstractNodeObject.GetCSVisual());
					num++;
					abstractNodeObject.AncestorObjectChanged(abstractNodeObject, NotifyCollectionChangedAction.Add);
					abstractNodeObject.IsHitTestVisible = false;
					AbstractNodeObject abstractNodeObject2 = abstractNodeObject;
					OperationMask operationFlag = abstractNodeObject2.OperationFlag;
					abstractNodeObject2.OperationFlag = OperationMask.NoneFlag;
					abstractNodeObject.BindingRecorder(null);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (object obj in e.OldItems)
				{
					AbstractNodeObject abstractNodeObject = obj as AbstractNodeObject;
					abstractNodeObject.AncestorObjectChanged(abstractNodeObject, NotifyCollectionChangedAction.Remove);
					this.GetCSVisual().RemoveChild(abstractNodeObject.GetCSVisual());
				}
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002B094 File Offset: 0x00029294
		protected override void OnMouseMove(MouseEventArgs args)
		{
			if (base.lastClickPoint != null)
			{
				PointF pointF = this.canvasEntity.TransformToParent(args.Point);
				float x = this.Position.X + pointF.X - base.lastClickPoint.X;
				float y = this.Position.Y + pointF.Y - base.lastClickPoint.Y;
				this.Position = new PointF(x, y);
				base.lastClickPoint = pointF;
				args.Handled = true;
			}
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002B128 File Offset: 0x00029328
		public bool CheckScaleValue(ScaleValue scale)
		{
			return scale.ScaleX >= 0.1f && scale.ScaleY >= 0.1f && scale.ScaleX <= 5f && scale.ScaleY <= 5f;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002B17C File Offset: 0x0002937C
		public ScaleValue ComputeScaleValue(float delta)
		{
			ScaleValue scaleValue = new ScaleValue(this.Scale.ScaleX + delta, this.Scale.ScaleY + delta, 0.1, -99999999.0, 99999999.0);
			ScaleValue result;
			if (this.CheckScaleValue(scaleValue))
			{
				result = scaleValue;
			}
			else if (delta > 0f && this.CanZoom())
			{
				scaleValue.ScaleX = (scaleValue.ScaleY = 5f);
				result = scaleValue;
			}
			else if (delta < 0f && this.CanDecreaseZoom())
			{
				scaleValue.ScaleX = (scaleValue.ScaleY = 0.1f);
				result = scaleValue;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002B248 File Offset: 0x00029448
		public bool CanZoom()
		{
			return this.Scale.ScaleX < 5f && this.Scale.ScaleY < 5f;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002B284 File Offset: 0x00029484
		public bool CanDecreaseZoom()
		{
			return this.Scale.ScaleX > 0.1f && this.Scale.ScaleY > 0.1f;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002B2C0 File Offset: 0x000294C0
		public SizeF GetSceneSize()
		{
			return this.sceneSize;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002B2D8 File Offset: 0x000294D8
		public void SetSceneSize(SizeF value, bool refresh = true)
		{
			this.sceneSize = value;
			if (refresh)
			{
				this.Size = value;
				this.RaiseCanvasSizeChangeEvent(value);
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002B308 File Offset: 0x00029508
		private void RaiseCanvasSizeChangeEvent(SizeF newSize)
		{
			CanvasSizeChangeEvent @event = EventAggregator.Instance.GetEvent<CanvasSizeChangeEvent>();
			@event.Unsubscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeEventHandle));
			@event.Publish(new CanvasSizeChangeEventArgs(string.Empty, newSize));
			@event.Subscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeEventHandle));
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002B35C File Offset: 0x0002955C
		private void CanvasSizeChangeEventHandle(CanvasSizeChangeEventArgs args)
		{
			this.sceneSize = args.NewSize;
			CocosItem currentSelectedProject = Services.ProjectOperations.CurrentSelectedProject;
			if (currentSelectedProject != null && currentSelectedProject.GetFileType() != NodeType.Node.ToString())
			{
				this.Size = args.NewSize;
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002B3B2 File Offset: 0x000295B2
		public void SetLayerColorVisible(bool visible)
		{
			this.canvasEntity.SetLayerColorVisible(visible);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002B3C2 File Offset: 0x000295C2
		public void SetCenterLineVisible(bool visible)
		{
			this.canvasEntity.SetCenterLineVisible(visible);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002B3D2 File Offset: 0x000295D2
		public void SetBackgroundVisible(bool visible)
		{
			this.canvasEntity.SetBackgroundVisible(visible);
		}

		// Token: 0x04000474 RID: 1140
		public const float MaxZoom = 5f;

		// Token: 0x04000475 RID: 1141
		public const float MinZoom = 0.1f;

		// Token: 0x04000476 RID: 1142
		public const float DeltaZoom = 0.1f;

		// Token: 0x04000477 RID: 1143
		private CSCanvas canvasEntity;

		// Token: 0x04000478 RID: 1144
		private ScaleValue scale = new ScaleValue();

		// Token: 0x04000479 RID: 1145
		private SizeF sceneSize = new SizeF(480f, 320f);
	}
}
