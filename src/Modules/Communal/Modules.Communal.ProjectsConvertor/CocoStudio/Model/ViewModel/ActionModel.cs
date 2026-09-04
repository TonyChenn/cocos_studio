using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000038 RID: 56
	[DataContract]
	public class ActionModel : BaseInfo
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000952C File Offset: 0x0000772C
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00009534 File Offset: 0x00007734
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

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00009582 File Offset: 0x00007782
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000958C File Offset: 0x0000778C
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

		// Token: 0x060003A3 RID: 931 RVA: 0x000095DA File Offset: 0x000077DA
		public ActionModel()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		// Token: 0x040001A7 RID: 423
		private ObservableCollection<DataItem> _DataItems;

		// Token: 0x040001A8 RID: 424
		private string _ClassName;
	}
}
