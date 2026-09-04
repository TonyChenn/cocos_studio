using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.UndoManager;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200003B RID: 59
	[DataContract]
	public class DataItem : BaseObject
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000985D File Offset: 0x00007A5D
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00009868 File Offset: 0x00007A68
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

		// Token: 0x060003B5 RID: 949 RVA: 0x000098CC File Offset: 0x00007ACC
		private void SetShowName()
		{
			LanguageType currentLanguage = LanguageOption.CurrentLanguage;
			if (currentLanguage == LanguageType.Chinese && !string.IsNullOrWhiteSpace(this.OtherName))
			{
				this.Name = this.OtherName;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x000098FC File Offset: 0x00007AFC
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00009904 File Offset: 0x00007B04
		public bool IsShowToolTip
		{
			get
			{
				return this._IsShowToolTip;
			}
			set
			{
				this._IsShowToolTip = value;
				this.RaisePropertyChanged<bool>(() => this.IsShowToolTip);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00009952 File Offset: 0x00007B52
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000995C File Offset: 0x00007B5C
		public string ToolTipContent
		{
			get
			{
				return this._ToolTipContent;
			}
			set
			{
				this._ToolTipContent = value;
				this.RaisePropertyChanged<string>(() => this.ToolTipContent);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000099AA File Offset: 0x00007BAA
		// (set) Token: 0x060003BB RID: 955 RVA: 0x000099B4 File Offset: 0x00007BB4
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
				this.RaisePropertyChanged<string>(() => this.Type);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00009A02 File Offset: 0x00007C02
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00009A0C File Offset: 0x00007C0C
		[DataMember(Name = "key")]
		public string Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				this._Key = value;
				this.RaisePropertyChanged<string>(() => this.Key);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00009A5A File Offset: 0x00007C5A
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00009A64 File Offset: 0x00007C64
		public override string Name
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

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00009AB2 File Offset: 0x00007CB2
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00009ABC File Offset: 0x00007CBC
		public string DefaultValue
		{
			get
			{
				return this._DefaultValue;
			}
			set
			{
				this._DefaultValue = value;
				this.SetDefaultValue(this._DefaultValue);
				this.RaisePropertyChanged<string>(() => this.DefaultValue);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00009B16 File Offset: 0x00007D16
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00009B20 File Offset: 0x00007D20
		public string Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				this.RaisePropertyChanged<string>(() => this.Data);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00009B6E File Offset: 0x00007D6E
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00009B78 File Offset: 0x00007D78
		public int Min
		{
			get
			{
				return this._Min;
			}
			set
			{
				this._Min = value;
				this.RaisePropertyChanged<int>(() => this.Min);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00009BC6 File Offset: 0x00007DC6
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public int Max
		{
			get
			{
				return this._Max;
			}
			set
			{
				this._Max = value;
				this.RaisePropertyChanged<int>(() => this.Max);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00009C1E File Offset: 0x00007E1E
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x00009C28 File Offset: 0x00007E28
		[UndoProperty]
		public List<ChildItem> Childes
		{
			get
			{
				return this._Childes;
			}
			set
			{
				this._Childes = value;
				this.RaisePropertyChanged<List<ChildItem>>(() => this.Childes);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00009C76 File Offset: 0x00007E76
		// (set) Token: 0x060003CB RID: 971 RVA: 0x00009C80 File Offset: 0x00007E80
		[DataMember(Name = "value")]
		[UndoProperty]
		public object Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				if (this.IsNotify)
				{
					this.RaisePropertyChanged<object>(() => this.Value);
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00009CD6 File Offset: 0x00007ED6
		// (set) Token: 0x060003CD RID: 973 RVA: 0x00009CDE File Offset: 0x00007EDE
		public Guid SID
		{
			get
			{
				return this._SID;
			}
			set
			{
				this._SID = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00009CE7 File Offset: 0x00007EE7
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00009CF0 File Offset: 0x00007EF0
		[UndoProperty]
		public ChildItem Selecte
		{
			get
			{
				return this._Selecte;
			}
			set
			{
				if (this._Selecte != value)
				{
					this._Selecte = value;
					this.IsNotify = false;
					if (this._Selecte != null)
					{
						this.Value = this._Selecte.SaveValue;
					}
					this.IsNotify = true;
					this.RaisePropertyChanged<ChildItem>(() => this.Selecte);
				}
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00009D6E File Offset: 0x00007F6E
		public DataItem()
		{
			base.BindingRecorder("TriggerUndoAndRedo");
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00009D88 File Offset: 0x00007F88
		private void SetDefaultValue(string defaultValue)
		{
			if (this.Type == "IntegerUpDown")
			{
				int num = 0;
				int.TryParse(defaultValue, out num);
				this.Value = num;
				return;
			}
			if (this.Type == "DoubleUpDown")
			{
				double num2 = 0.0;
				double.TryParse(defaultValue, out num2);
				this.Value = num2;
				return;
			}
			this.Value = defaultValue;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00009DF8 File Offset: 0x00007FF8
		public override bool Equals(object obj)
		{
			DataItem dataItem = obj as DataItem;
			if (dataItem != null)
			{
				return this.SID == dataItem.SID;
			}
			return base.Equals(obj);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00009E28 File Offset: 0x00008028
		public override int GetHashCode()
		{
			return this.SID.GetHashCode();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060003D4 RID: 980 RVA: 0x00009E4C File Offset: 0x0000804C
		// (remove) Token: 0x060003D5 RID: 981 RVA: 0x00009E84 File Offset: 0x00008084
		public new event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040001AF RID: 431
		private string _Type;

		// Token: 0x040001B0 RID: 432
		private string _Key;

		// Token: 0x040001B1 RID: 433
		private string _Name;

		// Token: 0x040001B2 RID: 434
		private string _DefaultValue;

		// Token: 0x040001B3 RID: 435
		private string _Data;

		// Token: 0x040001B4 RID: 436
		private int _Min;

		// Token: 0x040001B5 RID: 437
		private int _Max;

		// Token: 0x040001B6 RID: 438
		private List<ChildItem> _Childes;

		// Token: 0x040001B7 RID: 439
		private object _Value;

		// Token: 0x040001B8 RID: 440
		private Guid _SID;

		// Token: 0x040001B9 RID: 441
		private ChildItem _Selecte;

		// Token: 0x040001BA RID: 442
		private bool IsNotify = true;

		// Token: 0x040001BB RID: 443
		private bool _IsShowToolTip;

		// Token: 0x040001BC RID: 444
		private string _OtherName;

		// Token: 0x040001BD RID: 445
		private string _ToolTipContent;
	}
}
