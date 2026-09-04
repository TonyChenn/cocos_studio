using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200003E RID: 62
	[DataContract]
	public class TriggerModel : BaseObject
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00009F7D File Offset: 0x0000817D
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00009F88 File Offset: 0x00008188
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
				this.RaisePropertyChanged<int>(() => this.ID);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00009FD6 File Offset: 0x000081D6
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00009FE0 File Offset: 0x000081E0
		public bool IsRepair
		{
			get
			{
				return this._IsRepair;
			}
			set
			{
				this._IsRepair = value;
				this.RaisePropertyChanged<bool>(() => this.IsRepair);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000A02E File Offset: 0x0000822E
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000A038 File Offset: 0x00008238
		public bool IsEditor
		{
			get
			{
				return this._IsEditor;
			}
			set
			{
				this._IsEditor = value;
				this.RaisePropertyChanged<bool>(() => this.IsEditor);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000A086 File Offset: 0x00008286
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0000A090 File Offset: 0x00008290
		[DataMember(Name = "name")]
		public new string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.RaisePropertyChanged<string>(() => this.Name);
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000A0DE File Offset: 0x000082DE
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x0000A0FC File Offset: 0x000082FC
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
				this.RaisePropertyChanged<ObservableCollection<ConditionMode>>(() => this.Conditions);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0000A14A File Offset: 0x0000834A
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x0000A168 File Offset: 0x00008368
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
				this.RaisePropertyChanged<ObservableCollection<ActionModel>>(() => this.Actions);
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000A1B6 File Offset: 0x000083B6
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x0000A1D4 File Offset: 0x000083D4
		[DataMember(Name = "events")]
		[UndoProperty]
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
				this.RaisePropertyChanged<ObservableCollection<EventModel>>(() => this.Events);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000A222 File Offset: 0x00008422
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x0000A22C File Offset: 0x0000842C
		public bool IsEditMode
		{
			get
			{
				return this._IsEditMode;
			}
			set
			{
				this._IsEditMode = value;
				this.RaisePropertyChanged<bool>(() => this.IsEditMode);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060003EB RID: 1003 RVA: 0x0000A27C File Offset: 0x0000847C
		// (remove) Token: 0x060003EC RID: 1004 RVA: 0x0000A2B4 File Offset: 0x000084B4
		public new event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060003ED RID: 1005 RVA: 0x0000A2E9 File Offset: 0x000084E9
		public TriggerModel()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A2FC File Offset: 0x000084FC
		public override bool Equals(object obj)
		{
			TriggerModel triggerModel = obj as TriggerModel;
			if (triggerModel != null)
			{
				return this.ID == triggerModel.ID;
			}
			return base.Equals(obj);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000A32C File Offset: 0x0000852C
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x040001C6 RID: 454
		public const string TaskGroupName = "TriggerUndoAndRedo";

		// Token: 0x040001C7 RID: 455
		private string _Name;

		// Token: 0x040001C8 RID: 456
		private ObservableCollection<ConditionMode> _Conditions;

		// Token: 0x040001C9 RID: 457
		private ObservableCollection<ActionModel> _Actions;

		// Token: 0x040001CA RID: 458
		private ObservableCollection<EventModel> _Events;

		// Token: 0x040001CB RID: 459
		private bool _IsEditMode;

		// Token: 0x040001CC RID: 460
		private int _ID;

		// Token: 0x040001CD RID: 461
		private bool _IsRepair;

		// Token: 0x040001CE RID: 462
		public bool _IsEditor;
	}
}
