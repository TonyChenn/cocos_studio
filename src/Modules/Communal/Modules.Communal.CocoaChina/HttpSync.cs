using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Cocos.Launcher.Library;
using GLib;

namespace Modules.Communal.CocoaChina
{
	public class HttpSync
	{
		public event EventHandler<HttpSync.HttpSyncArgs> OnResived;

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

		private const int BUFFER_SIZE = 1024;

		private const int DefaultTimeout = 10000;

		public bool IsPost = false;

		private string exceptionMessage;

		public ManualResetEvent allDone = new ManualResetEvent(false);

		public class HttpSyncArgs : EventArgs
		{
			public string Message { get; set; }
		}
	}
}
