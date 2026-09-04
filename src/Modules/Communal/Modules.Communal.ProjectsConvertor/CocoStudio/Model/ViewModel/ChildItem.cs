using System;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000039 RID: 57
	public class ChildItem : NotificationObject
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000095ED File Offset: 0x000077ED
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x000095F8 File Offset: 0x000077F8
		public string OtherName
		{
			get
			{
				return this._OtherName;
			}
			set
			{
				if (this._Name != value)
				{
					this._OtherName = value;
					this.SetShowName();
					this.RaisePropertyChanged<string>(() => this.OtherName);
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000965A File Offset: 0x0000785A
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00009664 File Offset: 0x00007864
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000096B2 File Offset: 0x000078B2
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x000096BC File Offset: 0x000078BC
		public string Name
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000970A File Offset: 0x0000790A
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00009714 File Offset: 0x00007914
		public object SaveValue
		{
			get
			{
				return this._SaveValue;
			}
			set
			{
				this._SaveValue = value;
				this.RaisePropertyChanged<object>(() => this.SaveValue);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00009764 File Offset: 0x00007964
		private void SetShowName()
		{
			LanguageType currentLanguage = LanguageOption.CurrentLanguage;
			if (currentLanguage == LanguageType.Chinese && !string.IsNullOrWhiteSpace(this.OtherName))
			{
				this.Name = this.OtherName;
			}
		}

		// Token: 0x040001A9 RID: 425
		private string _Name;

		// Token: 0x040001AA RID: 426
		private int _ID;

		// Token: 0x040001AB RID: 427
		private object _SaveValue;

		// Token: 0x040001AC RID: 428
		private string _OtherName;
	}
}
