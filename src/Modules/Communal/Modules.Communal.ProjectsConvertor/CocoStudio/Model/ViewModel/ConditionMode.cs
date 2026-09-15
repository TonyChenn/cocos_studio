using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	[DataContract]
	public class ConditionMode : BaseInfo
	{
		[DataMember(Name = "classname")]
		public string ClassName
		{
			get
			{
				return this._ClassName;
			}
			set
			{
				this._ClassName = value;
				this.RaisePropertyChanged<string>(() => this.ClassName);
			}
		}

		[UndoProperty]
		[DataMember(Name = "dataitems")]
		public ObservableCollection<DataItem> DataItems
		{
			get
			{
				return this._DataItems;
			}
			set
			{
				this._DataItems = value;
				this.RaisePropertyChanged<ObservableCollection<DataItem>>(() => this.DataItems);
			}
		}

		public ConditionMode()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		private ObservableCollection<DataItem> _DataItems;

		private string _ClassName;
	}
}
