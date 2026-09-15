using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.Model.ViewModel
{
	public abstract class BaseObject : NotificationObject, INotifyStateChanged, INotifyPropertyChanged
	{
		[Browsable(false)]
		public bool IsRaisePropertyChanged { get; set; }

		[Browsable(false)]
		public BaseRecorder Recorder { get; protected set; }

		[Category("Group_Routine")]
		[Editor(typeof(ValidTextEditor), typeof(ValidTextEditor))]
		[UndoProperty]
		[DisplayName("Display_Name")]
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.RaisePropertyChanged<string>(() => this.Name);
					this.RaisePropertyChanged<string>(() => this.DisplayName);
				}
			}
		}

		[Browsable(false)]
		public virtual string DisplayName
		{
			get
			{
				string result;
				if (!string.IsNullOrEmpty(this.Name))
				{
					result = this.Name;
				}
				else
				{
					result = "[Node]";
				}
				return result;
			}
		}

		public BaseObject()
		{
			this.IsRaisePropertyChanged = true;
		}

		public BaseObject(string name) : this()
		{
			this.name = name;
		}

		public event EventHandler<StateChangedEventArgs> StateChanged;

		protected virtual void RaisePropertyChanged(PropertyInfo propertyInfo, bool isNotifyStateChanged)
		{
			if (this.IsRaisePropertyChanged)
			{
				string taskName = base.GetType().Name + propertyInfo.Name;
				using (CompositeTask.Run(taskName, null))
				{
					if (isNotifyStateChanged && Services.TaskService.Enable)
					{
						this.RaiseStateChanged(propertyInfo.Name);
					}
					base.RaisePropertyChanged(propertyInfo);
				}
			}
		}

		protected override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			bool isNotifyStateChanged = true;
			if (this.Recorder != null)
			{
				isNotifyStateChanged = this.Recorder.IsAutoRecord;
			}
			this.RaisePropertyChanged<T>(propertyExpression, isNotifyStateChanged);
		}

		protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, bool isNotifyStateChanged)
		{
			PropertyInfo propertyInfo = PropertySupport.ExtractPropertyInfo<T>(propertyExpression);
			this.RaisePropertyChanged(propertyInfo, isNotifyStateChanged);
		}

		private void RaiseStateChanged(StateChangedEventArgs args)
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, args);
			}
		}

		private void RaiseStateChanged(string propertyName)
		{
			this.RaiseStateChanged(new StateChangedEventArgs(propertyName));
		}

		public void BindingRecorder(string taskGroupName = null)
		{
			if (this.Recorder == null && BaseRecorder.IsCreateDefaultRecorder)
			{
				this.Recorder = new DefaultRecorder(this, taskGroupName);
				this.OnBindingRecorder();
			}
		}

		protected virtual void OnBindingRecorder()
		{
		}

		internal virtual void OnResourcePropertyChanged()
		{
		}

		protected string name;
	}
}
