using System;
using CocoStudio.Core;

namespace Modules.Communal.MutualEditor
{
	internal abstract class BaseHandler : IUDPHandler, IDisposable
	{
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

		public BaseHandler()
		{
			if (!this.hasInit)
			{
				this.socket = new UDPSocket("127.0.0.1", this.GetStartPort());
				Services.IntinalizeCompleted += this.HandleInitializeComplete;
				this.hasInit = true;
			}
		}

		private void HandleInitializeComplete(EventArgs args)
		{
			if (!this.hasBindEvent)
			{
				this.Recived += this.HandleMessageRecived;
				this.hasBindEvent = true;
			}
		}

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

		private void HandleMessageRecived(object sender, MessageArgs args)
		{
			this.OnHandleMessageRecived(sender, args);
		}

		protected virtual void OnHandleMessageRecived(object sender, MessageArgs args)
		{
		}

		protected virtual int GetStartPort()
		{
			return 9000;
		}

		private bool hasInit = false;

		private bool hasBindEvent = false;

		protected UDPSocket socket = null;
	}
}
