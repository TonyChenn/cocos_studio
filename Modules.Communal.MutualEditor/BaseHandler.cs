using System;
using CocoStudio.Core;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000003 RID: 3
	internal abstract class BaseHandler : IUDPHandler, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		// (remove) Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
		protected event EventHandler<MessageArgs> Recived
		{
			add
			{
				if (this.socket != null)
				{
					this.socket.Recived += value;
				}
			}
			remove
			{
				if (this.socket != null)
				{
					this.socket.Recived -= value;
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A8 File Offset: 0x000002A8
		public BaseHandler()
		{
			if (!this.hasInit)
			{
				this.socket = new UDPSocket("127.0.0.1", this.GetStartPort());
				Services.IntinalizeCompleted += this.HandleInitializeComplete;
				this.hasInit = true;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002110 File Offset: 0x00000310
		private void HandleInitializeComplete(EventArgs args)
		{
			if (!this.hasBindEvent)
			{
				this.Recived += this.HandleMessageRecived;
				this.hasBindEvent = true;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002144 File Offset: 0x00000344
		public void SendMessage(string data, Action action = Action.Show)
		{
			if (this.socket != null)
			{
				int startPort = this.GetStartPort();
				Message message = new Message();
				message.SentIP = this.socket.Port.IP;
				Message message2 = message;
				int port = this.socket.Port.Port;
				message2.Sentport = port.ToString();
				message.ReciveIP = this.socket.Port.IP;
				Message message3 = message;
				message3.Data = data;
				message3.Action = action;
				for (int i = 0; i < 7; i++)
				{
					int num = startPort + i;
					if (this.socket.Port.Port != num)
					{
						message3.RecivePort = num.ToString();
						this.socket.Sent(message3);
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002220 File Offset: 0x00000420
		public void Dispose()
		{
			if (this.socket != null)
			{
				this.socket.Dispose();
			}
			if (this.hasInit)
			{
				Services.IntinalizeCompleted -= this.HandleInitializeComplete;
			}
			if (this.hasBindEvent)
			{
				this.Recived -= this.HandleMessageRecived;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002286 File Offset: 0x00000486
		private void HandleMessageRecived(object sender, MessageArgs args)
		{
			this.OnHandleMessageRecived(sender, args);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002292 File Offset: 0x00000492
		protected virtual void OnHandleMessageRecived(object sender, MessageArgs args)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002298 File Offset: 0x00000498
		protected virtual int GetStartPort()
		{
			return 9000;
		}

		// Token: 0x04000001 RID: 1
		private bool hasInit = false;

		// Token: 0x04000002 RID: 2
		private bool hasBindEvent = false;

		// Token: 0x04000003 RID: 3
		protected UDPSocket socket = null;
	}
}
