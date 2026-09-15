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
	[DisplayName("Display_Component_Canvas")]
	public class CanvasObject : VisualObject
	{
		public ObservableCollection<AbstractNodeObject> Children { get; set; }

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

		private AbstractNodeObject CurrentNodeObject
		{
			get
			{
				return Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			}
		}

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

		protected override void OnDragDrop(DragDropArgs e)
		{
			DragOperationManager.Current.DragDrop(e, this.CurrentNodeObject);
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.canvasEntity;
		}

		public override IEnumerable<VisualObject> GetVisualChildren()
		{
			return this.Children;
		}

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

		public bool CheckScaleValue(ScaleValue scale)
		{
			return scale.ScaleX >= 0.1f && scale.ScaleY >= 0.1f && scale.ScaleX <= 5f && scale.ScaleY <= 5f;
		}

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

		public bool CanZoom()
		{
			return this.Scale.ScaleX < 5f && this.Scale.ScaleY < 5f;
		}

		public bool CanDecreaseZoom()
		{
			return this.Scale.ScaleX > 0.1f && this.Scale.ScaleY > 0.1f;
		}

		public SizeF GetSceneSize()
		{
			return this.sceneSize;
		}

		public void SetSceneSize(SizeF value, bool refresh = true)
		{
			this.sceneSize = value;
			if (refresh)
			{
				this.Size = value;
				this.RaiseCanvasSizeChangeEvent(value);
			}
		}

		private void RaiseCanvasSizeChangeEvent(SizeF newSize)
		{
			CanvasSizeChangeEvent @event = EventAggregator.Instance.GetEvent<CanvasSizeChangeEvent>();
			@event.Unsubscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeEventHandle));
			@event.Publish(new CanvasSizeChangeEventArgs(string.Empty, newSize));
			@event.Subscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeEventHandle));
		}

		private void CanvasSizeChangeEventHandle(CanvasSizeChangeEventArgs args)
		{
			this.sceneSize = args.NewSize;
			CocosItem currentSelectedProject = Services.ProjectOperations.CurrentSelectedProject;
			if (currentSelectedProject != null && currentSelectedProject.GetFileType() != NodeType.Node.ToString())
			{
				this.Size = args.NewSize;
			}
		}

		public void SetLayerColorVisible(bool visible)
		{
			this.canvasEntity.SetLayerColorVisible(visible);
		}

		public void SetCenterLineVisible(bool visible)
		{
			this.canvasEntity.SetCenterLineVisible(visible);
		}

		public void SetBackgroundVisible(bool visible)
		{
			this.canvasEntity.SetBackgroundVisible(visible);
		}

		public const float MaxZoom = 5f;

		public const float MinZoom = 0.1f;

		public const float DeltaZoom = 0.1f;

		private CSCanvas canvasEntity;

		private ScaleValue scale = new ScaleValue();

		private SizeF sceneSize = new SizeF(480f, 320f);
	}
}
