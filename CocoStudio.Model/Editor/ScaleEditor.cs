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
	// Token: 0x0200009B RID: 155
	internal class ScaleEditor : BaseEditor
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00016E98 File Offset: 0x00015098
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00016EAC File Offset: 0x000150AC
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

		// Token: 0x06000545 RID: 1349 RVA: 0x00016FBC File Offset: 0x000151BC
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

		// Token: 0x06000546 RID: 1350 RVA: 0x0001714C File Offset: 0x0001534C
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

		// Token: 0x06000547 RID: 1351 RVA: 0x000171F0 File Offset: 0x000153F0
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

		// Token: 0x06000548 RID: 1352 RVA: 0x0001730C File Offset: 0x0001550C
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

		// Token: 0x06000549 RID: 1353 RVA: 0x00017428 File Offset: 0x00015628
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == base.PropertyItem.Name || e.PropertyName == "UniformScale")
			{
				base.SetControl();
			}
		}

		// Token: 0x04000272 RID: 626
		private ScaleEditor.UniformToggleButton uniformBtn;

		// Token: 0x04000273 RID: 627
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x04000274 RID: 628
		private NoUndoNumEntry yInnerEntry;

		// Token: 0x0200009C RID: 156
		private class UniformToggleButton : EventBox
		{
			// Token: 0x1700016B RID: 363
			// (get) Token: 0x0600054E RID: 1358 RVA: 0x00017508 File Offset: 0x00015708
			// (set) Token: 0x0600054F RID: 1359 RVA: 0x00017520 File Offset: 0x00015720
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

			// Token: 0x14000008 RID: 8
			// (add) Token: 0x06000550 RID: 1360 RVA: 0x0001759C File Offset: 0x0001579C
			// (remove) Token: 0x06000551 RID: 1361 RVA: 0x000175D8 File Offset: 0x000157D8
			public event EventHandler CheckChanged;

			// Token: 0x06000552 RID: 1362 RVA: 0x00017614 File Offset: 0x00015814
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

			// Token: 0x06000553 RID: 1363 RVA: 0x00017692 File Offset: 0x00015892
			private void ButtonPressEventHandler(object o, ButtonPressEventArgs args)
			{
				this.IsUniformScale = !this.IsUniformScale;
			}

			// Token: 0x04000278 RID: 632
			private Xwt.Drawing.Image lockImg;

			// Token: 0x04000279 RID: 633
			private Xwt.Drawing.Image unlockImg;

			// Token: 0x0400027A RID: 634
			private ImageView imageView;

			// Token: 0x0400027B RID: 635
			private bool _isUniformScale = false;
		}
	}
}
