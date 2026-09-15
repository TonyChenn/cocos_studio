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
	[DataContract]
	[Extension(typeof(IJsonModel))]
	public class TriggerModel
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
			}
		}

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

		public event PropertyChangedEventHandler PropertyChanged;

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
