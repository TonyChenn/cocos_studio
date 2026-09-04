using System;
using System.Runtime.Serialization;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200003D RID: 61
	[DataContract]
	public class EventModel : BaseInfo
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00009EB9 File Offset: 0x000080B9
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00009EC4 File Offset: 0x000080C4
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00009F12 File Offset: 0x00008112
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x00009F1C File Offset: 0x0000811C
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

		// Token: 0x060003DA RID: 986 RVA: 0x00009F6A File Offset: 0x0000816A
		public EventModel()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		// Token: 0x040001C4 RID: 452
		private int _ID;

		// Token: 0x040001C5 RID: 453
		private string _ClassName;
	}
}
