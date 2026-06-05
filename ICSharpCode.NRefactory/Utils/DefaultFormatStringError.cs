using System;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000138 RID: 312
	public class DefaultFormatStringError : IFormatStringError
	{
		// Token: 0x06000AD3 RID: 2771 RVA: 0x00020654 File Offset: 0x0001F654
		public DefaultFormatStringError()
		{
			this.Message = "";
			this.OriginalText = "";
			this.SuggestedReplacementText = "";
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0002067D File Offset: 0x0001F67D
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x00020685 File Offset: 0x0001F685
		public int StartLocation { get; set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002068E File Offset: 0x0001F68E
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00020696 File Offset: 0x0001F696
		public int EndLocation { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0002069F File Offset: 0x0001F69F
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x000206A7 File Offset: 0x0001F6A7
		public string Message { get; set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000206B0 File Offset: 0x0001F6B0
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000206B8 File Offset: 0x0001F6B8
		public string OriginalText { get; set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000206C1 File Offset: 0x0001F6C1
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x000206C9 File Offset: 0x0001F6C9
		public string SuggestedReplacementText { get; set; }

		// Token: 0x06000ADE RID: 2782 RVA: 0x000206D4 File Offset: 0x0001F6D4
		public override string ToString()
		{
			return string.Format("[DefaultFormatStringError: StartLocation={0}, EndLocation={1}, Message={2}, OriginalText={3}, SuggestedReplacementText={4}]", new object[]
			{
				this.StartLocation,
				this.EndLocation,
				this.Message,
				this.OriginalText,
				this.SuggestedReplacementText
			});
		}
	}
}
