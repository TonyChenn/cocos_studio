using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000003 RID: 3
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ObjectSurrogate : IExtensibleDataObject
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00003AD7 File Offset: 0x00001CD7
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00003ADF File Offset: 0x00001CDF
		public ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataObject_value;
			}
			set
			{
				this.extensionDataObject_value = value;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003AE8 File Offset: 0x00001CE8
		protected ObjectSurrogate()
		{
		}

		// Token: 0x04000012 RID: 18
		private ExtensionDataObject extensionDataObject_value;
	}
}
