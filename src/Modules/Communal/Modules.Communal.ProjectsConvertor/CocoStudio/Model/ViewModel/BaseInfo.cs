using System;
using System.Runtime.Serialization;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	[DataContract]
	public class BaseInfo : BaseObject, ICloneable
	{
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

		private void SetShowName()
		{
			LanguageType currentLanguage = LanguageOption.CurrentLanguage;
			if (currentLanguage == LanguageType.Chinese && !string.IsNullOrWhiteSpace(this.OtherName))
			{
				this.Name = this.OtherName;
			}
		}

		public object Clone()
		{
			return base.MemberwiseClone();
		}

		private string _Name;

		private EIdentify _Identify;

		private string _OtherName;
	}
}
