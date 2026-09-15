using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Modules.Communal.MutualEditor
{
	public class UDPSocket : IDisposable
	{
		public event EventHandler<MessageArgs> Recived;

		public UDPPort Port { get; private set; }

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

		public UDPSocket(string iP, int port)
		{
			this.Port = this.Start(iP, port);
		}

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

		~UDPSocket()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.runningFlag = false;
			if (this.socket != null)
			{
				this.Close();
			}
		}

		private bool runningFlag = false;

		private Socket socket;

		private EndPoint remotePoint;

		private Thread witeThread;
	}
}
