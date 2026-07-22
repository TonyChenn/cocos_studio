using System;
using System.Runtime.Serialization;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000037 RID: 55
	[DataContract]
	public class BaseInfo : BaseObject, ICloneable
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000939B File Offset: 0x0000759B
		// (set) Token: 0x06000396 RID: 918 RVA: 0x000093A4 File Offset: 0x000075A4
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

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00009406 File Offset: 0x00007606
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00009410 File Offset: 0x00007610
		public override string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged<string>(() => this.Name);
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000946C File Offset: 0x0000766C
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00009474 File Offset: 0x00007674
		public EIdentify Identify
		{
			get
			{
				return this._Identify;
			}
			set
			{
				if (this._Identify != value)
				{
					this._Identify = value;
					this.RaisePropertyChanged<EIdentify>(() => this.Identify);
				}
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000094CB File Offset: 0x000076CB
		private EIdentify GetIdentify()
		{
			if (this is EventModel)
			{
				return EIdentify.Event;
			}
			if (this is ConditionMode)
			{
				return EIdentify.Condition;
			}
			if (this is ActionModel)
			{
				return EIdentify.Action;
			}
			return EIdentify.None;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000094EC File Offset: 0x000076EC
		private void SetShowName()
		{
			LanguageType currentLanguage = LanguageOption.CurrentLanguage;
			if (currentLanguage == LanguageType.Chinese && !string.IsNullOrWhiteSpace(this.OtherName))
			{
				this.Name = this.OtherName;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000951C File Offset: 0x0000771C
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040001A4 RID: 420
		private string _Name;

		// Token: 0x040001A5 RID: 421
		private EIdentify _Identify;

		// Token: 0x040001A6 RID: 422
		private string _OtherName;
	}
}
