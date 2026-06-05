using System;

namespace MonoDevelop.Core
{
	// Token: 0x02000004 RID: 4
	public class UserException : ApplicationException
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000298E File Offset: 0x00000B8E
		public UserException(string message) : base(message)
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002997 File Offset: 0x00000B97
		public UserException(string message, string details) : base(message)
		{
			this.details = details;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000029A7 File Offset: 0x00000BA7
		public string Details
		{
			get
			{
				return this.details;
			}
		}

		/// <summary>
		/// If true, it means that the error has already been reported to the user (for example, by showing a dialog), so it doesn't have to be reported again.
		/// </summary>
		/// <value><c>true</c> if already reported to user; otherwise, <c>false</c>.</value>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000029AF File Offset: 0x00000BAF
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000029B7 File Offset: 0x00000BB7
		public bool AlreadyReportedToUser { get; set; }

		// Token: 0x0400000F RID: 15
		private string details;
	}
}
