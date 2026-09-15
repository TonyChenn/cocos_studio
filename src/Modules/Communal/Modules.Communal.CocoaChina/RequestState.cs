using System;
using System.IO;
using System.Net;
using System.Text;

namespace Modules.Communal.CocoaChina
{
	public class RequestState
	{
		public RequestState()
		{
			this.BufferRead = new byte[10240];
			this.requestData = new StringBuilder("");
			this.request = null;
			this.streamResponse = null;
		}

		private const int BUFFER_SIZE = 10240;

		public StringBuilder requestData;

		public byte[] BufferRead;

		public HttpWebRequest request;

		public HttpWebResponse response;

		public Stream streamResponse;
	}
}
