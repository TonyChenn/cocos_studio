using System;
using System.Reflection;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	internal class ObjectCheckMenuItem : CheckMenuItem, IDisposable, IObjectMenuItem
	{
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

		public ObjectCheckMenuItem(string displayText, string funcName = "") : base(displayText)
		{
			this.funcName = funcName;
			base.Toggled += this.MenuItemClick;
		}

		public override void Dispose()
		{
			base.Toggled -= this.MenuItemClick;
			base.Dispose();
		}

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

		private VisualObject triggerObject;

		private string funcName = string.Empty;
	}
}
