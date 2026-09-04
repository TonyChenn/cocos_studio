using System;
using System.ComponentModel;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	// Token: 0x02000013 RID: 19
	[DataInclude(typeof(EnumResourceType))]
	[JsonObject(MemberSerialization.OptIn)]
	public class ResourceData
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002DDC File Offset: 0x00000FDC
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002DF3 File Offset: 0x00000FF3
		[JsonProperty]
		[ItemProperty]
		public EnumResourceType Type { get; protected internal set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002DFC File Offset: 0x00000FFC
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00002E13 File Offset: 0x00001013
		[JsonProperty]
		[ItemProperty]
		public string Path { get; protected internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002E1C File Offset: 0x0000101C
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00002E33 File Offset: 0x00001033
		[DefaultValue(null)]
		[ItemProperty(DefaultValue = null)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string Plist { get; protected internal set; }

		// Token: 0x0600007D RID: 125 RVA: 0x00002E3C File Offset: 0x0000103C
		protected ResourceData() : this(EnumResourceType.Normal, null, null)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002E4A File Offset: 0x0000104A
		public ResourceData(string path) : this(EnumResourceType.Normal, path)
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002E57 File Offset: 0x00001057
		public ResourceData(EnumResourceType type, string path) : this(type, path, string.Empty)
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002E69 File Offset: 0x00001069
		public ResourceData(EnumResourceType type, string path, string plist)
		{
			this.Type = type;
			this.Path = ((path == null) ? string.Empty : path);
			this.Plist = ((plist == null) ? string.Empty : plist);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002EA4 File Offset: 0x000010A4
		public override string ToString()
		{
			return this.Path;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002EBC File Offset: 0x000010BC
		public bool Equals(ResourceData others)
		{
			return !(others == null) && (object.ReferenceEquals(this, others) || (this.GetHashCode() == others.GetHashCode() && (this.Type == others.Type && string.Equals(this.Path, others.Path, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Plist, others.Plist, StringComparison.OrdinalIgnoreCase))));
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F50 File Offset: 0x00001150
		public override bool Equals(object obj)
		{
			ResourceData resourceData = obj as ResourceData;
			return resourceData != null && this.Equals(resourceData);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002F84 File Offset: 0x00001184
		public override int GetHashCode()
		{
			int result;
			if (this.hashCode != 0)
			{
				result = this.hashCode;
			}
			else
			{
				this.hashCode = (this.Type.GetHashCode() | (this.Path.GetHashCode() ^ this.Plist.GetHashCode()));
				result = this.hashCode;
			}
			return result;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002FE0 File Offset: 0x000011E0
		public static bool operator ==(ResourceData leftValue, ResourceData rightValue)
		{
			bool result;
			if (object.ReferenceEquals(leftValue, null))
			{
				result = object.ReferenceEquals(rightValue, null);
			}
			else
			{
				result = leftValue.Equals(rightValue);
			}
			return result;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003014 File Offset: 0x00001214
		public static bool operator !=(ResourceData leftValue, ResourceData rightValue)
		{
			return !(leftValue == rightValue);
		}

		// Token: 0x04000045 RID: 69
		protected internal int hashCode;

		// Token: 0x04000046 RID: 70
		public static readonly ResourceData Empty = new ResourceData(string.Empty);
	}
}
