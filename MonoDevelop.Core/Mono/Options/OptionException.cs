using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Mono.Options
{
	// Token: 0x020000F3 RID: 243
	[Serializable]
	public class OptionException : Exception
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00021E04 File Offset: 0x00020004
		public OptionException()
		{
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00021E0C File Offset: 0x0002000C
		public OptionException(string message, string optionName) : base(message)
		{
			this.option = optionName;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00021E1C File Offset: 0x0002001C
		public OptionException(string message, string optionName, Exception innerException) : base(message, innerException)
		{
			this.option = optionName;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00021E2D File Offset: 0x0002002D
		protected OptionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.option = info.GetString("OptionName");
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00021E48 File Offset: 0x00020048
		public string OptionName
		{
			get
			{
				return this.option;
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00021E50 File Offset: 0x00020050
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("OptionName", this.option);
		}

		// Token: 0x040002BB RID: 699
		private string option;
	}
}
