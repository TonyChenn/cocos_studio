using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200003A RID: 58
	[DataContract]
	public class ConditionMode : BaseInfo
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000979C File Offset: 0x0000799C
		// (set) Token: 0x060003AF RID: 943 RVA: 0x000097A4 File Offset: 0x000079A4
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

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x000097F2 File Offset: 0x000079F2
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x000097FC File Offset: 0x000079FC
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

		// Token: 0x060003B2 RID: 946 RVA: 0x0000984A File Offset: 0x00007A4A
		public ConditionMode()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		// Token: 0x040001AD RID: 429
		private ObservableCollection<DataItem> _DataItems;

		// Token: 0x040001AE RID: 430
		private string _ClassName;
	}
}
