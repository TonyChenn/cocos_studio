using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Cocos.Launcher.Library;
using GLib;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000005 RID: 5
	public class HttpSync
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000016 RID: 22 RVA: 0x000023E8 File Offset: 0x000005E8
		// (remove) Token: 0x06000017 RID: 23 RVA: 0x00002424 File Offset: 0x00000624
		public event EventHandler<HttpSync.HttpSyncArgs> OnResived;

		// Token: 0x06000018 RID: 24 RVA: 0x00002460 File Offset: 0x00000660
		protected void Resived(string msg)
		{
			if (this.OnResived != null)
			{
				this.OnResived(this, new HttpSync.HttpSyncArgs
				{
					Message = msg
				});
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000249C File Offset: 0x0000069C
		public void GetSyncResponseOfString(string url, string method = "get", string data = "", int? timeout = null)
		{
			try
			{
				HttpWebRequest httpWebRequest = url.CreatRequest(method);
				if (timeout != null)
				{
					httpWebRequest.Timeout = timeout.Value;
				}
				if (method.ToUpper() == "POST" && data != null)
				{
					Encoding encoding = Encoding.GetEncoding("UTF-8");
					byte[] bytes = encoding.GetBytes(data);
					httpWebRequest.ContentLength = (long)bytes.Length;
					Stream requestStream = httpWebRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				RequestState requestState = new RequestState();
				requestState.request = httpWebRequest;
				IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(this.RespCallback), requestState);
				ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.TimeoutCallback), httpWebRequest, 10000, true);
				this.allDone.WaitOne();
				requestState.response.Close();
			}
			catch (Exception ex)
			{
				this.exceptionMessage = ex.Message;
				this.Resived(this.exceptionMessage);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025C0 File Offset: 0x000007C0
		private void TimeoutCallback(object state, bool timedOut)
		{
			if (timedOut)
			{
				HttpWebRequest httpWebRequest = state as HttpWebRequest;
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002610 File Offset: 0x00000810
		private void RespCallback(IAsyncResult asynchronousResult)
		{
			try
			{
				RequestState requestState = (RequestState)asynchronousResult.AsyncState;
				HttpWebRequest request = requestState.request;
				requestState.response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
				Stream responseStream = requestState.response.GetResponseStream();
				requestState.streamResponse = responseStream;
				IAsyncResult asyncResult = responseStream.BeginRead(requestState.BufferRead, 0, 1024, new AsyncCallback(this.ReadCallBack), requestState);
				return;
			}
			catch (WebException ex)
			{
				this.exceptionMessage = ex.Message;
			}
			this.allDone.Set();
			GLib.Timeout.Add(0U, delegate
			{
				this.Resived(this.exceptionMessage);
				return false;
			});
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002718 File Offset: 0x00000918
		private void ReadCallBack(IAsyncResult asyncResult)
		{
			RequestState myRequestState = (RequestState)asyncResult.AsyncState;
			try
			{
				Stream streamResponse = myRequestState.streamResponse;
				int num = streamResponse.EndRead(asyncResult);
				if (num > 0)
				{
					myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.BufferRead, 0, num));
					IAsyncResult asyncResult2 = streamResponse.BeginRead(myRequestState.BufferRead, 0, 1024, new AsyncCallback(this.ReadCallBack), myRequestState);
					return;
				}
				streamResponse.Close();
			}
			catch (WebException ex)
			{
				this.exceptionMessage = ex.Message;
			}
			this.allDone.Set();
			GLib.Timeout.Add(0U, delegate
			{
				this.Resived(string.IsNullOrWhiteSpace(this.exceptionMessage) ? myRequestState.requestData.ToString() : this.exceptionMessage);
				return false;
			});
		}

		// Token: 0x04000010 RID: 16
		private const int BUFFER_SIZE = 1024;

		// Token: 0x04000011 RID: 17
		private const int DefaultTimeout = 10000;

		// Token: 0x04000013 RID: 19
		public bool IsPost = false;

		// Token: 0x04000014 RID: 20
		private string exceptionMessage;

		// Token: 0x04000015 RID: 21
		public ManualResetEvent allDone = new ManualResetEvent(false);

		// Token: 0x02000006 RID: 6
		public class HttpSyncArgs : EventArgs
		{
			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600001F RID: 31 RVA: 0x00002830 File Offset: 0x00000A30
			// (set) Token: 0x06000020 RID: 32 RVA: 0x00002847 File Offset: 0x00000A47
			public string Message { get; set; }
		}
	}
}
