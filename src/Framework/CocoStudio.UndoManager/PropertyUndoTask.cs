using System;
using System.Reflection;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200002F RID: 47
	public class PropertyUndoTask : UndoTask
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007548 File Offset: 0x00005748
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000755F File Offset: 0x0000575F
		public object NewValue { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007568 File Offset: 0x00005768
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000757F File Offset: 0x0000577F
		public object OldValue { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007588 File Offset: 0x00005788
		public string PropertyName
		{
			get
			{
				return this.prop.Name;
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000075A8 File Offset: 0x000057A8
		public PropertyUndoTask(DefaultRecorder recorder, object sender, PropertyInfo prop, object oldValue, object newValue) : base(recorder)
		{
			this.objectItem = sender;
			this.prop = prop;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.execute = new Action<object>(this.Redoing);
			this.unExecute = new Action<object>(this.Undoing);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007603 File Offset: 0x00005803
		private void Undoing(object temp)
		{
			this.SetPropertyValue(this.OldValue);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007613 File Offset: 0x00005813
		private void Redoing(object temp)
		{
			this.SetPropertyValue(this.NewValue);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007623 File Offset: 0x00005823
		private void SetPropertyValue(object value)
		{
			this.prop.SetValue(this.objectItem, value, DefaultRecorder.EmptyArray);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007640 File Offset: 0x00005840
		public override string DescriptionForUser
		{
			get
			{
				return "PropertyUndoTask";
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007658 File Offset: 0x00005858
		~PropertyUndoTask()
		{
			this.Dispose();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000768C File Offset: 0x0000588C
		public override void Dispose()
		{
			this.objectItem = null;
			this.prop = null;
			this.NewValue = null;
			this.OldValue = null;
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x04000063 RID: 99
		private object objectItem;

		// Token: 0x04000064 RID: 100
		private PropertyInfo prop;
	}
}
