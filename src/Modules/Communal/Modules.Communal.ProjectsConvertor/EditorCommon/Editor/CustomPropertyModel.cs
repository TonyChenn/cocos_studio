using System;
using System.Runtime.Serialization;
using EditorCommon.JsonModel;
using Mono.Addins;

namespace EditorCommon.Editor
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	public class CustomPropertyModel
	{
		[DataMember(Name = "key")]
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

		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
				if (this.Value == null || this.Value == "")
				{
					this.SetDefuleValue(value);
				}
			}
		}

		[DataMember(Name = "value")]
		public object Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		private void SetDefuleValue(string typeString)
		{
			if (typeString == "Double")
			{
				this.Value = 0.0;
				return;
			}
			if (typeString == "Int")
			{
				this.Value = 0;
				return;
			}
			if (typeString == "Boolean")
			{
				this.Value = false;
			}
		}

		private string _Name;

		private string _Type;

		private object _Value;
	}
}
