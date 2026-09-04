using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CocosStudio.ExternalImport.Protocol
{
	public static class ExternalImportProtocol
	{
		public const int CurrentVersion = 1;

		public const int MaximumMessageBytes = 1024 * 1024;

		public static string InstanceDirectory
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CocosStudio", "ExternalImport");
			}
		}

		public static void WriteMessage<T>(Stream stream, T message)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
			if (payload.Length > MaximumMessageBytes)
			{
				throw new InvalidDataException("External import message is too large.");
			}
			byte[] length = BitConverter.GetBytes(payload.Length);
			stream.Write(length, 0, length.Length);
			stream.Write(payload, 0, payload.Length);
			stream.Flush();
		}

		public static T ReadMessage<T>(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			byte[] lengthBytes = ReadExactly(stream, sizeof(int));
			int length = BitConverter.ToInt32(lengthBytes, 0);
			if (length <= 0 || length > MaximumMessageBytes)
			{
				throw new InvalidDataException("External import message length is invalid.");
			}
			byte[] payload = ReadExactly(stream, length);
			return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(payload));
		}

		private static byte[] ReadExactly(Stream stream, int length)
		{
			byte[] buffer = new byte[length];
			int offset = 0;
			while (offset < length)
			{
				int count = stream.Read(buffer, offset, length - offset);
				if (count <= 0)
				{
					throw new EndOfStreamException("External import pipe closed before the message was complete.");
				}
				offset += count;
			}
			return buffer;
		}
	}

	public sealed class ExternalImportRequest
	{
		public ExternalImportRequest()
		{
			this.ProtocolVersion = ExternalImportProtocol.CurrentVersion;
			this.RequestId = Guid.NewGuid().ToString("N");
			this.ProjectFile = string.Empty;
			this.CsdFile = string.Empty;
			this.ResourceFolder = string.Empty;
			this.TargetDirectory = string.Empty;
			this.OpenAfterImport = true;
		}

		public int ProtocolVersion { get; set; }

		public string RequestId { get; set; }

		public string ProjectFile { get; set; }

		public string CsdFile { get; set; }

		public string ResourceFolder { get; set; }

		public string TargetDirectory { get; set; }

		public bool Overwrite { get; set; }

		public bool OpenAfterImport { get; set; }
	}

	public sealed class ExternalImportResponse
	{
		public ExternalImportResponse()
		{
			this.RequestId = string.Empty;
			this.Code = string.Empty;
			this.Message = string.Empty;
			this.ImportedCsd = string.Empty;
			this.ImportedResourceFolder = string.Empty;
			this.OpenedCsd = string.Empty;
			this.Warnings = new List<string>();
		}

		public bool Success { get; set; }

		public string RequestId { get; set; }

		public string Code { get; set; }

		public string Message { get; set; }

		public string ImportedCsd { get; set; }

		public string ImportedResourceFolder { get; set; }

		public string OpenedCsd { get; set; }

		public List<string> Warnings { get; set; }
	}

	public sealed class ExternalImportInstance
	{
		public ExternalImportInstance()
		{
			this.InstanceId = string.Empty;
			this.PipeName = string.Empty;
			this.ProjectFile = string.Empty;
			this.ResourceRoot = string.Empty;
			this.StartedAtUtc = string.Empty;
		}

		public int ProtocolVersion { get; set; }

		public string InstanceId { get; set; }

		public int ProcessId { get; set; }

		public string PipeName { get; set; }

		public string ProjectFile { get; set; }

		public string ResourceRoot { get; set; }

		public string StartedAtUtc { get; set; }
	}
}
