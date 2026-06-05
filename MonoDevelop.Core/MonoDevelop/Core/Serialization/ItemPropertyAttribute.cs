using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000080 RID: 128
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class ItemPropertyAttribute : Attribute
	{
		// Token: 0x0600042C RID: 1068 RVA: 0x0000EED9 File Offset: 0x0000D0D9
		public ItemPropertyAttribute()
		{
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000EEE1 File Offset: 0x0000D0E1
		public ItemPropertyAttribute(Type dataType)
		{
			this.dataType = dataType;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public ItemPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000EEFF File Offset: 0x0000D0FF
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0000EF07 File Offset: 0x0000D107
		public object DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0000EF10 File Offset: 0x0000D110
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x0000EF18 File Offset: 0x0000D118
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000EF21 File Offset: 0x0000D121
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0000EF3C File Offset: 0x0000D13C
		public string Scope
		{
			get
			{
				if (string.IsNullOrEmpty(this.scope))
				{
					return "";
				}
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000EF45 File Offset: 0x0000D145
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x0000EF4D File Offset: 0x0000D14D
		public Type SerializationDataType
		{
			get
			{
				return this.confType;
			}
			set
			{
				this.confType = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0000EF56 File Offset: 0x0000D156
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0000EF5E File Offset: 0x0000D15E
		public Type ValueType
		{
			get
			{
				return this.dataType;
			}
			set
			{
				this.dataType = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0000EF67 File Offset: 0x0000D167
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0000EF6F File Offset: 0x0000D16F
		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0000EF78 File Offset: 0x0000D178
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0000EF80 File Offset: 0x0000D180
		public bool WriteOnly
		{
			get
			{
				return this.writeOnly;
			}
			set
			{
				this.writeOnly = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000EF89 File Offset: 0x0000D189
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0000EF91 File Offset: 0x0000D191
		public Type FallbackType
		{
			get
			{
				return this.fallbackType;
			}
			set
			{
				this.fallbackType = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000EF9A File Offset: 0x0000D19A
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0000EFA2 File Offset: 0x0000D1A2
		public bool IsExternal
		{
			get
			{
				return this.isExternal;
			}
			set
			{
				this.isExternal = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000EFAB File Offset: 0x0000D1AB
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0000EFB3 File Offset: 0x0000D1B3
		public bool SkipEmpty { get; set; }

		// Token: 0x04000169 RID: 361
		private Type confType;

		// Token: 0x0400016A RID: 362
		private object defaultValue;

		// Token: 0x0400016B RID: 363
		private string name;

		// Token: 0x0400016C RID: 364
		private string scope;

		// Token: 0x0400016D RID: 365
		private Type dataType;

		// Token: 0x0400016E RID: 366
		private bool readOnly;

		// Token: 0x0400016F RID: 367
		private bool writeOnly;

		// Token: 0x04000170 RID: 368
		private Type fallbackType;

		// Token: 0x04000171 RID: 369
		private bool isExternal;
	}
}
