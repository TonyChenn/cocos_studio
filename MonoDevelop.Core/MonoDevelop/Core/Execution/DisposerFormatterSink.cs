using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000C5 RID: 197
	internal class DisposerFormatterSink : IClientFormatterSink, IMessageSink, IClientChannelSink, IChannelSinkBase
	{
		// Token: 0x060006B3 RID: 1715 RVA: 0x0001AB10 File Offset: 0x00018D10
		public DisposerFormatterSink(IClientChannelSink nextSink)
		{
			this.nextSink = nextSink;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001AB4C File Offset: 0x00018D4C
		public IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			int num = -1;
			bool timedOut = false;
			RemotingService.CallbackData callbackData = RemotingService.GetCallbackData(methodCallMessage.Uri, methodCallMessage.MethodName);
			if (callbackData != null)
			{
				num = callbackData.Timeout;
				if (callbackData.Calling != null)
				{
					IMessage message = callbackData.Calling(callbackData.Target, methodCallMessage);
					if (message != null)
					{
						return message;
					}
				}
			}
			IMessage res = null;
			if (num != -1)
			{
				ManualResetEvent manualResetEvent = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(delegate(object param0)
				{
					res = ((IMessageSink)this.nextSink).SyncProcessMessage(msg);
				});
				if (!manualResetEvent.WaitOne(num, false))
				{
					timedOut = true;
					res = new ReturnMessage(null, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
				}
			}
			else
			{
				res = ((IMessageSink)this.nextSink).SyncProcessMessage(msg);
			}
			if (callbackData != null && callbackData.Called != null)
			{
				IMessage message2 = callbackData.Called(callbackData.Target, methodCallMessage, res as IMethodReturnMessage, timedOut);
				if (message2 != null)
				{
					res = message2;
				}
			}
			return res;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001AC75 File Offset: 0x00018E75
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return ((IMessageSink)this.nextSink).AsyncProcessMessage(msg, replySink);
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001AC89 File Offset: 0x00018E89
		public IMessageSink NextSink
		{
			get
			{
				return (IMessageSink)this.nextSink;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001AC96 File Offset: 0x00018E96
		public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
		{
			this.nextSink.AsyncProcessResponse(sinkStack, state, headers, stream);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001ACA8 File Offset: 0x00018EA8
		public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001ACAF File Offset: 0x00018EAF
		public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001ACB6 File Offset: 0x00018EB6
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001ACBD File Offset: 0x00018EBD
		public IClientChannelSink NextChannelSink
		{
			get
			{
				return this.nextSink;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0001ACC5 File Offset: 0x00018EC5
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000232 RID: 562
		private IClientChannelSink nextSink;
	}
}
