using System;
using System.IO;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000193 RID: 403
	public class SerializationMapNode : ExtensionNode
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0003A470 File Offset: 0x00038670
		public string FileContent
		{
			get
			{
				string result;
				using (StreamReader streamReader = new StreamReader(base.Addin.GetResource(this.resource, true)))
				{
					result = streamReader.ReadToEnd();
				}
				return result;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0003A4BC File Offset: 0x000386BC
		public string FileId
		{
			get
			{
				return string.Concat(new string[]
				{
					base.Addin.Id,
					" v",
					base.Addin.Version,
					" - ",
					this.resource
				});
			}
		}

		// Token: 0x04000481 RID: 1153
		[NodeAttribute]
		protected string resource;
	}
}
