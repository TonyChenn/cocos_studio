using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	[DataContract]
	public class ActionModel : BaseInfo
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

		[DataMember(Name = "dataitems")]
		[UndoProperty]
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

		public ActionModel()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		private ObservableCollection<DataItem> _DataItems;

		private string _ClassName;
	}
}
