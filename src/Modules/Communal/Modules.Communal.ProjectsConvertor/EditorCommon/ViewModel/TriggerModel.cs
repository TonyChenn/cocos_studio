using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using EditorCommon.JsonModel;
using Mono.Addins;

namespace EditorCommon.ViewModel
{
	// Token: 0x02000036 RID: 54
	[DataContract]
	[Extension(typeof(IJsonModel))]
	public class TriggerModel
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00009217 File Offset: 0x00007417
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000921F File Offset: 0x0000741F
		[DataMember(Name = "id")]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00009228 File Offset: 0x00007428
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00009230 File Offset: 0x00007430
		public bool IsRepair
		{
			get
			{
				return this._IsRepair;
			}
			set
			{
				this._IsRepair = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00009239 File Offset: 0x00007439
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00009241 File Offset: 0x00007441
		public bool IsEditor
		{
			get
			{
				return this._IsEditor;
			}
			set
			{
				this._IsEditor = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000924A File Offset: 0x0000744A
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00009252 File Offset: 0x00007452
		[DataMember(Name = "name")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000925B File Offset: 0x0000745B
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00009276 File Offset: 0x00007476
		[UndoProperty]
		[DataMember(Name = "conditions")]
		public ObservableCollection<ConditionMode> Conditions
		{
			get
			{
				if (this._Conditions == null)
				{
					this._Conditions = new ObservableCollection<ConditionMode>();
				}
				return this._Conditions;
			}
			set
			{
				this._Conditions = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000927F File Offset: 0x0000747F
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000929A File Offset: 0x0000749A
		[UndoProperty]
		[DataMember(Name = "actions")]
		public ObservableCollection<ActionModel> Actions
		{
			get
			{
				if (this._Actions == null)
				{
					this._Actions = new ObservableCollection<ActionModel>();
				}
				return this._Actions;
			}
			set
			{
				this._Actions = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000092A3 File Offset: 0x000074A3
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000092BE File Offset: 0x000074BE
		[UndoProperty]
		[DataMember(Name = "events")]
		public ObservableCollection<EventModel> Events
		{
			get
			{
				if (this._Events == null)
				{
					this._Events = new ObservableCollection<EventModel>();
				}
				return this._Events;
			}
			set
			{
				this._Events = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000092C7 File Offset: 0x000074C7
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000092CF File Offset: 0x000074CF
		public bool IsEditMode
		{
			get
			{
				return this._IsEditMode;
			}
			set
			{
				this._IsEditMode = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000390 RID: 912 RVA: 0x000092D8 File Offset: 0x000074D8
		// (remove) Token: 0x06000391 RID: 913 RVA: 0x00009310 File Offset: 0x00007510
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000393 RID: 915 RVA: 0x00009350 File Offset: 0x00007550
		public override bool Equals(object obj)
		{
			TriggerModel triggerModel = obj as TriggerModel;
			if (triggerModel != null)
			{
				return this.ID == triggerModel.ID;
			}
			return base.Equals(obj);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00009380 File Offset: 0x00007580
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x0400019A RID: 410
		public const string TaskGroupName = "TriggerUndoAndRedo";

		// Token: 0x0400019B RID: 411
		private string _Name;

		// Token: 0x0400019C RID: 412
		private ObservableCollection<ConditionMode> _Conditions;

		// Token: 0x0400019D RID: 413
		private ObservableCollection<ActionModel> _Actions;

		// Token: 0x0400019E RID: 414
		private ObservableCollection<EventModel> _Events;

		// Token: 0x0400019F RID: 415
		private bool _IsEditMode;

		// Token: 0x040001A0 RID: 416
		private int _ID;

		// Token: 0x040001A1 RID: 417
		private bool _IsRepair;

		// Token: 0x040001A2 RID: 418
		public bool _IsEditor;
	}
}
