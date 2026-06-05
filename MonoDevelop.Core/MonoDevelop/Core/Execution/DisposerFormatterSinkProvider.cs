using System;
using System.Runtime.Remoting.Channels;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000C6 RID: 198
	internal class DisposerFormatterSinkProvider : IClientFormatterSinkProvider, IClientChannelSinkProvider
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
		{
			IClientChannelSink nextSink = null;
			if (this.next != null)
			{
				nextSink = this.next.CreateSink(channel, url, remoteChannelData);
			}
			return new DisposerFormatterSink(nextSink);
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001ACF4 File Offset: 0x00018EF4
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001ACFC File Offset: 0x00018EFC
		public IClientChannelSinkProvider Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x04000233 RID: 563
		private IClientChannelSinkProvider next;
	}
}
