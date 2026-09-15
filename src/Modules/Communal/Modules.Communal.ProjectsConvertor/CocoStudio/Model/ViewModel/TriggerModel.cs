using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	[DataContract]
	public class TriggerModel : BaseObject
	{
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

		public new event PropertyChangedEventHandler PropertyChanged;

		public TriggerModel()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		public override bool Equals(object obj)
		{
			TriggerModel triggerModel = obj as TriggerModel;
			if (triggerModel != null)
			{
				return this.ID == triggerModel.ID;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		public const string TaskGroupName = "TriggerUndoAndRedo";

		private string _Name;

		private ObservableCollection<ConditionMode> _Conditions;

		private ObservableCollection<ActionModel> _Actions;

		private ObservableCollection<EventModel> _Events;

		private bool _IsEditMode;

		private int _ID;

		private bool _IsRepair;

		public bool _IsEditor;
	}
}
