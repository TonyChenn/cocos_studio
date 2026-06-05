using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000086 RID: 134
	public class SerializationContext : IDisposable
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0000F205 File Offset: 0x0000D405
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0000F20D File Offset: 0x0000D40D
		public string BaseFile
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000F216 File Offset: 0x0000D416
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000F21E File Offset: 0x0000D41E
		public IPropertyFilter PropertyFilter
		{
			get
			{
				return this.propertyFilter;
			}
			set
			{
				this.propertyFilter = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0000F227 File Offset: 0x0000D427
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0000F22F File Offset: 0x0000D42F
		public DataSerializer Serializer
		{
			get
			{
				return this.serializer;
			}
			internal set
			{
				this.serializer = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0000F238 File Offset: 0x0000D438
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0000F240 File Offset: 0x0000D440
		public char DirectorySeparatorChar
		{
			get
			{
				return this.directorySeparatorChar;
			}
			set
			{
				this.directorySeparatorChar = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000F249 File Offset: 0x0000D449
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0000F251 File Offset: 0x0000D451
		public IProgressMonitor ProgressMonitor
		{
			get
			{
				return this.monitor;
			}
			set
			{
				this.monitor = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000F25A File Offset: 0x0000D45A
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0000F262 File Offset: 0x0000D462
		public bool IncludeDefaultValues { get; set; }

		// Token: 0x06000461 RID: 1121 RVA: 0x0000F26B File Offset: 0x0000D46B
		public void ResetDefaultValueSerialization()
		{
			this.forcedSerializationProps = null;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000F274 File Offset: 0x0000D474
		public void ForceDefaultValueSerialization(ItemProperty prop)
		{
			if (this.forcedSerializationProps == null)
			{
				this.forcedSerializationProps = new HashSet<ItemProperty>();
			}
			this.forcedSerializationProps.Add(prop);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000F296 File Offset: 0x0000D496
		public bool IsDefaultValueSerializationForced(ItemProperty prop)
		{
			return this.IncludeDefaultValues || (this.forcedSerializationProps != null && this.forcedSerializationProps.Contains(prop));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		public virtual void Close()
		{
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000F2BA File Offset: 0x0000D4BA
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x04000173 RID: 371
		private string file;

		// Token: 0x04000174 RID: 372
		private IPropertyFilter propertyFilter;

		// Token: 0x04000175 RID: 373
		private DataSerializer serializer;

		// Token: 0x04000176 RID: 374
		private IProgressMonitor monitor;

		// Token: 0x04000177 RID: 375
		private char directorySeparatorChar = Path.DirectorySeparatorChar;

		// Token: 0x04000178 RID: 376
		private HashSet<ItemProperty> forcedSerializationProps;
	}
}
