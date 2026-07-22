using System;
using System.Runtime.Serialization;
using EditorCommon.JsonModel;
using Mono.Addins;

namespace EditorCommon.Editor
{
	// Token: 0x02000018 RID: 24
	[DataContract]
	[Extension(typeof(IJsonModel))]
	public class CustomPropertyModel
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004ABC File Offset: 0x00002CBC
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00004AC4 File Offset: 0x00002CC4
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004ACD File Offset: 0x00002CCD
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004AD5 File Offset: 0x00002CD5
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004AFA File Offset: 0x00002CFA
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00004B02 File Offset: 0x00002D02
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

		// Token: 0x060000B5 RID: 181 RVA: 0x00004B0C File Offset: 0x00002D0C
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

		// Token: 0x04000041 RID: 65
		private string _Name;

		// Token: 0x04000042 RID: 66
		private string _Type;

		// Token: 0x04000043 RID: 67
		private object _Value;
	}
}
