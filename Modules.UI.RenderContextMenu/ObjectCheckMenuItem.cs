using System;
using System.Reflection;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000009 RID: 9
	internal class ObjectCheckMenuItem : CheckMenuItem, IDisposable, IObjectMenuItem
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002988 File Offset: 0x00000B88
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002964 File Offset: 0x00000B64
		public VisualObject TriggerObject
		{
			get
			{
				return this.triggerObject;
			}
			set
			{
				if (this.triggerObject != value)
				{
					this.triggerObject = value;
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000029A0 File Offset: 0x00000BA0
		public ObjectCheckMenuItem(string displayText, string funcName = "") : base(displayText)
		{
			this.funcName = funcName;
			base.Toggled += this.MenuItemClick;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000029D2 File Offset: 0x00000BD2
		public override void Dispose()
		{
			base.Toggled -= this.MenuItemClick;
			base.Dispose();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000029F0 File Offset: 0x00000BF0
		protected virtual void MenuItemClick(object sender, EventArgs e)
		{
			using (CompositeTask.Run(this.triggerObject.GetType().Name + this.funcName, null))
			{
				if (!string.IsNullOrEmpty(this.funcName))
				{
					PropertyInfo property = this.triggerObject.GetType().GetProperty(this.funcName);
					if (property != null)
					{
						property.SetValue(this.triggerObject, base.Active, null);
					}
				}
				else
				{
					IPlayControl playControl = this.TriggerObject as IPlayControl;
					if (playControl != null)
					{
						playControl.IsPlaying = base.Active;
					}
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002ABC File Offset: 0x00000CBC
		private bool IsEnable()
		{
			if (this.triggerObject != null)
			{
				IPlayControl playControl = this.triggerObject as IPlayControl;
				if (playControl == null)
				{
					return true;
				}
				if (!playControl.HasData())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B08 File Offset: 0x00000D08
		public virtual void UpdateMenuItemState()
		{
			if (!this.IsEnable())
			{
				base.Sensitive = false;
			}
			else
			{
				base.Sensitive = true;
				if (!string.IsNullOrEmpty(this.funcName))
				{
					PropertyInfo property = this.triggerObject.GetType().GetProperty(this.funcName);
					if (property != null)
					{
						bool active = (bool)property.GetValue(this.triggerObject, null);
						base.Active = active;
					}
				}
				else
				{
					IDisplayState displayState = this.triggerObject as IDisplayState;
					if (displayState != null)
					{
						base.Active = displayState.DisplayState;
					}
					else
					{
						IPlayControl playControl = this.TriggerObject as IPlayControl;
						if (playControl != null)
						{
							base.Active = playControl.IsPlaying;
						}
					}
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private VisualObject triggerObject;

		// Token: 0x04000018 RID: 24
		private string funcName = string.Empty;
	}
}
