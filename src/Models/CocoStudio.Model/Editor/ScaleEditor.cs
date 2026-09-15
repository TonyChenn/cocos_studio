using System;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	internal class ScaleEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.uniformBtn = new ScaleEditor.UniformToggleButton();
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			this.xInnerEntry.CanZero = false;
			FullEntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry, "%");
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yInnerEntry.CanZero = false;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry, "%");
			HBox hbox = new HBox();
			hbox.PackStart(widget);
			hbox.PackStart(this.uniformBtn, false, false, 0U);
			hbox.PackStart(widget2);
			hbox.ShowAll();
			base.SetControl();
			this.uniformBtn.CheckChanged += this.UniformButtonCheckChangedHandler;
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			return hbox;
		}

		protected override void OnSetControl()
		{
			VisualObject visualObject = PropertyItem.FirstObject as VisualObject;
			ScaleValue scale = visualObject.Scale;
			bool uniformScale = visualObject.UniformScale;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<VisualObject, VisualObject, bool> func = (VisualObject a, VisualObject b) => Math.Round((double)a.Scale.ScaleX, 2) == Math.Round((double)b.Scale.ScaleX, 2);
				Func<VisualObject, VisualObject, bool> func2 = (VisualObject a, VisualObject b) => Math.Round((double)a.Scale.ScaleY, 2) == Math.Round((double)b.Scale.ScaleY, 2);
				Func<VisualObject, VisualObject, bool> func3 = (VisualObject a, VisualObject b) => a.UniformScale == b.UniformScale;
				if (base.IsWhipNode<VisualObject>(func))
				{
					this.xInnerEntry.SetToSubState();
				}
				else
				{
					this.xInnerEntry.Value = scale.ScaleX * 100f;
				}
				if (base.IsWhipNode<VisualObject>(func2))
				{
					this.yInnerEntry.SetToSubState();
				}
				else
				{
					this.yInnerEntry.Value = scale.ScaleY * 100f;
				}
				if (base.IsWhipNode<VisualObject>(func3))
				{
					this.uniformBtn.IsUniformScale = false;
				}
				else
				{
					this.uniformBtn.IsUniformScale = uniformScale;
				}
			}
			else
			{
				if (this.uniformBtn.IsUniformScale != uniformScale)
				{
					this.uniformBtn.IsUniformScale = uniformScale;
				}
				this.xInnerEntry.Value = scale.ScaleX * 100f;
				this.yInnerEntry.Value = scale.ScaleY * 100f;
			}
		}

		private void UniformButtonCheckChangedHandler(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					VisualObject visualObject = PropertyItem.Objects[i] as VisualObject;
					if (visualObject != null)
					{
						visualObject.UniformScale = this.uniformBtn.IsUniformScale;
					}
				}
			}
			Services.EventsService.GetEvent<ScaleLockedChangeEvent>().Publish(this.uniformBtn.IsUniformScale);
		}

		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			float num = e.Value / 100f;
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ScaleValue scaleValue = base.PropertyItem.Values[i] as ScaleValue;
					if (this.uniformBtn.IsUniformScale)
					{
						float num2 = scaleValue.ScaleX * num;
						if (num2 == 0f || scaleValue.ScaleY == 0f)
						{
							scaleValue.ScaleY = num;
						}
						else
						{
							scaleValue.ScaleY = scaleValue.ScaleY / scaleValue.ScaleX * num;
						}
						this.yInnerEntry.Value = scaleValue.ScaleY * 100f;
					}
					scaleValue.ScaleX = num;
					base.PropertyItem.Values[i] = scaleValue;
				}
			}
		}

		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			float num = e.Value / 100f;
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ScaleValue scaleValue = base.PropertyItem.Values[i] as ScaleValue;
					if (this.uniformBtn.IsUniformScale)
					{
						float num2 = scaleValue.ScaleY * num;
						if (num2 == 0f || scaleValue.ScaleX == 0f)
						{
							scaleValue.ScaleX = num;
						}
						else
						{
							scaleValue.ScaleX = scaleValue.ScaleX / scaleValue.ScaleY * num;
						}
						this.xInnerEntry.Value = scaleValue.ScaleX * 100f;
					}
					scaleValue.ScaleY = num;
					base.PropertyItem.Values[i] = scaleValue;
				}
			}
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == base.PropertyItem.Name || e.PropertyName == "UniformScale")
			{
				base.SetControl();
			}
		}

		private ScaleEditor.UniformToggleButton uniformBtn;

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;

		private class UniformToggleButton : EventBox
		{
			public bool IsUniformScale
			{
				get
				{
					return this._isUniformScale;
				}
				set
				{
					bool isUniformScale = this._isUniformScale;
					this._isUniformScale = value;
					if (this._isUniformScale)
					{
						this.imageView.Image = this.lockImg;
					}
					else
					{
						this.imageView.Image = this.unlockImg;
					}
					if (this._isUniformScale != isUniformScale && this.CheckChanged != null)
					{
						this.CheckChanged(this, new EventArgs());
					}
				}
			}

			public event EventHandler CheckChanged;

			public UniformToggleButton()
			{
				this.lockImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.lock.png");
				this.unlockImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.unLock.png");
				this.imageView = new ImageView(this.unlockImg);
				base.WidthRequest = 24;
				base.Add(this.imageView);
				base.ShowAll();
				base.ButtonPressEvent += this.ButtonPressEventHandler;
			}

			private void ButtonPressEventHandler(object o, ButtonPressEventArgs args)
			{
				this.IsUniformScale = !this.IsUniformScale;
			}

			private Xwt.Drawing.Image lockImg;

			private Xwt.Drawing.Image unlockImg;

			private ImageView imageView;

			private bool _isUniformScale = false;
		}
	}
}
