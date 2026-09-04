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
	// Token: 0x02000079 RID: 121
	public abstract class BaseObject : NotificationObject, INotifyStateChanged, INotifyPropertyChanged
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000186D8 File Offset: 0x000168D8
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x000186EF File Offset: 0x000168EF
		[Browsable(false)]
		public bool IsRaisePropertyChanged { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000186F8 File Offset: 0x000168F8
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0001870F File Offset: 0x0001690F
		[Browsable(false)]
		public BaseRecorder Recorder { get; protected set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00018718 File Offset: 0x00016918
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00018730 File Offset: 0x00016930
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000187D4 File Offset: 0x000169D4
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

		// Token: 0x06000454 RID: 1108 RVA: 0x00018804 File Offset: 0x00016A04
		public BaseObject()
		{
			this.IsRaisePropertyChanged = true;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018817 File Offset: 0x00016A17
		public BaseObject(string name) : this()
		{
			this.name = name;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000456 RID: 1110 RVA: 0x0001882C File Offset: 0x00016A2C
		// (remove) Token: 0x06000457 RID: 1111 RVA: 0x00018868 File Offset: 0x00016A68
		public event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x06000458 RID: 1112 RVA: 0x000188A4 File Offset: 0x00016AA4
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

		// Token: 0x06000459 RID: 1113 RVA: 0x00018934 File Offset: 0x00016B34
		protected override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			bool isNotifyStateChanged = true;
			if (this.Recorder != null)
			{
				isNotifyStateChanged = this.Recorder.IsAutoRecord;
			}
			this.RaisePropertyChanged<T>(propertyExpression, isNotifyStateChanged);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018968 File Offset: 0x00016B68
		protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, bool isNotifyStateChanged)
		{
			PropertyInfo propertyInfo = PropertySupport.ExtractPropertyInfo<T>(propertyExpression);
			this.RaisePropertyChanged(propertyInfo, isNotifyStateChanged);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00018988 File Offset: 0x00016B88
		private void RaiseStateChanged(StateChangedEventArgs args)
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, args);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000189B3 File Offset: 0x00016BB3
		private void RaiseStateChanged(string propertyName)
		{
			this.RaiseStateChanged(new StateChangedEventArgs(propertyName));
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000189C4 File Offset: 0x00016BC4
		public void BindingRecorder(string taskGroupName = null)
		{
			if (this.Recorder == null && BaseRecorder.IsCreateDefaultRecorder)
			{
				this.Recorder = new DefaultRecorder(this, taskGroupName);
				this.OnBindingRecorder();
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00018A01 File Offset: 0x00016C01
		protected virtual void OnBindingRecorder()
		{
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00018A04 File Offset: 0x00016C04
		internal virtual void OnResourcePropertyChanged()
		{
		}

		// Token: 0x0400021B RID: 539
		protected string name;
	}
}
