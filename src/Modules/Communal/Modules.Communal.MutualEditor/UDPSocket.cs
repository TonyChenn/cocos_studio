using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x0200000E RID: 14
	public class UDPSocket : IDisposable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000025 RID: 37 RVA: 0x000027C0 File Offset: 0x000009C0
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x000027FC File Offset: 0x000009FC
		public event EventHandler<MessageArgs> Recived;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002838 File Offset: 0x00000A38
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000284F File Offset: 0x00000A4F
		public UDPPort Port { get; private set; }

		// Token: 0x06000029 RID: 41 RVA: 0x00002858 File Offset: 0x00000A58
		protected virtual void OnRecived(Message msg)
		{
			if (this.Recived != null)
			{
				this.Recived(this, new MessageArgs
				{
					Message = msg
				});
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002891 File Offset: 0x00000A91
		public UDPSocket(string iP, int port)
		{
			this.Port = this.Start(iP, port);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028B4 File Offset: 0x00000AB4
		private UDPPort Start(string iP, int port)
		{
			UDPPort result = new UDPPort(iP, port);
			try
			{
				IPAddress address = IPAddress.Parse(iP);
				IPEndPoint ipendPoint = new IPEndPoint(address, 0);
				this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				this.socket.ReceiveBufferSize = 1024;
				this.socket.SendBufferSize = 1024;
				for (;;)
				{
					ipendPoint.Port = port;
					try
					{
						this.socket.Bind(ipendPoint);
						result.Port = ipendPoint.Port;
						break;
					}
					catch
					{
					}
					port++;
				}
				this.runningFlag = true;
				this.remotePoint = new IPEndPoint(IPAddress.Parse(result.IP), result.Port);
				this.witeThread = new Thread(new ThreadStart(this.Listen));
				this.witeThread.Start();
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000029BC File Offset: 0x00000BBC
		private void Listen()
		{
			byte[] buffer = new byte[102400];
			while (this.runningFlag)
			{
				if (this.socket == null || this.socket.Available < 1)
				{
					Thread.Sleep(200);
				}
				else
				{
					try
					{
						int num = this.socket.ReceiveFrom(buffer, ref this.remotePoint);
						if (num > 0)
						{
							this.OnRecived(new MemoryStream(buffer).DeSerializeBinary<Message>());
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A60 File Offset: 0x00000C60
		public bool Sent(Message message)
		{
			IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(message.ReciveIP), int.Parse(message.RecivePort));
			byte[] array = message.SerializeBinary().ToArray();
			try
			{
				this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 100);
				this.socket.SendTo(array, array.Length, SocketFlags.None, remoteEP);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002AE4 File Offset: 0x00000CE4
		private void Close()
		{
			this.runningFlag = false;
			if (this.socket != null)
			{
				this.socket.Close();
				this.witeThread.Abort();
				this.socket = null;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002B28 File Offset: 0x00000D28
		~UDPSocket()
		{
			this.Dispose();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002B5C File Offset: 0x00000D5C
		public void Dispose()
		{
			this.runningFlag = false;
			if (this.socket != null)
			{
				this.Close();
			}
		}

		// Token: 0x04000018 RID: 24
		private bool runningFlag = false;

		// Token: 0x04000019 RID: 25
		private Socket socket;

		// Token: 0x0400001A RID: 26
		private EndPoint remotePoint;

		// Token: 0x0400001B RID: 27
		private Thread witeThread;
	}
}
