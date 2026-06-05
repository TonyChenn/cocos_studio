using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200007E RID: 126
	public class ItemMember
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x0000E80B File Offset: 0x0000CA0B
		public ItemMember()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000E813 File Offset: 0x0000CA13
		public ItemMember(Type declaringType, string name)
		{
			this.declaringType = declaringType;
			this.name = name;
			this.type = typeof(string);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000E839 File Offset: 0x0000CA39
		public ItemMember(Type declaringType, string name, Type memberType)
		{
			this.declaringType = declaringType;
			this.name = name;
			this.type = memberType;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000E856 File Offset: 0x0000CA56
		public ItemMember(Type declaringType, string name, bool isExternal)
		{
			this.declaringType = declaringType;
			this.name = name;
			this.isExternal = isExternal;
			this.type = typeof(string);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000E883 File Offset: 0x0000CA83
		public ItemMember(Type declaringType, string name, object[] customAttributes)
		{
			this.declaringType = declaringType;
			this.name = name;
			this.customAttributes = customAttributes;
			this.type = typeof(string);
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000E8C1 File Offset: 0x0000CAC1
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0000E8C9 File Offset: 0x0000CAC9
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000E8D2 File Offset: 0x0000CAD2
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0000E8DA File Offset: 0x0000CADA
		public Type DeclaringType
		{
			get
			{
				return this.declaringType;
			}
			set
			{
				this.declaringType = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0000E8E3 File Offset: 0x0000CAE3
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0000E8EB File Offset: 0x0000CAEB
		public object InitValue
		{
			get
			{
				return this.initValue;
			}
			set
			{
				this.initValue = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		public string InsertBefore
		{
			get
			{
				return this.insertBefore;
			}
			set
			{
				this.insertBefore = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000E905 File Offset: 0x0000CB05
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x0000E90D File Offset: 0x0000CB0D
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000E916 File Offset: 0x0000CB16
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0000E91E File Offset: 0x0000CB1E
		public object[] CustomAttributes
		{
			get
			{
				return this.customAttributes;
			}
			set
			{
				this.customAttributes = value;
			}
		}

		// Token: 0x04000151 RID: 337
		private string name;

		// Token: 0x04000152 RID: 338
		private Type type;

		// Token: 0x04000153 RID: 339
		private Type declaringType;

		// Token: 0x04000154 RID: 340
		private object initValue;

		// Token: 0x04000155 RID: 341
		private string insertBefore;

		// Token: 0x04000156 RID: 342
		private bool isExternal;

		// Token: 0x04000157 RID: 343
		private object[] customAttributes;
	}
}
