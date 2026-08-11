using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using CocosStudio.ExternalImport.Protocol;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Newtonsoft.Json;

namespace CocosStudio.ExternalImport.StudioPlugin
{
	public sealed class ExternalImportServer : IDisposable
	{
		private ExternalImportServer()
		{
			this.instanceId = Guid.NewGuid().ToString("N");
			this.startedAtUtc = DateTime.UtcNow.ToString("o");
		}

		public static ExternalImportServer Instance
		{
			get
			{
				return instance;
			}
		}

		public void Start()
		{
			lock (this.syncRoot)
			{
				if (this.running)
				{
					return;
				}
				try
				{
					Directory.CreateDirectory(ExternalImportProtocol.InstanceDirectory);
					int processId = Process.GetCurrentProcess().Id;
					this.pipeName = "CocosStudio.ExternalImport." + processId + "." + this.instanceId;
					this.descriptorPath = Path.Combine(ExternalImportProtocol.InstanceDirectory, "studio-" + processId + ".json");
					this.running = true;
					Services.ProjectOperations.CurrentSelectedSolutionChanged += this.OnCurrentSolutionChanged;
					Services.ProjectOperations.CurrentSelectedSolutionClosed += this.OnCurrentSolutionChanged;
					this.WriteDescriptor();
					this.listenerThread = new Thread(new ThreadStart(this.Listen));
					this.listenerThread.IsBackground = true;
					this.listenerThread.Name = "Cocos Studio External Import";
					this.listenerThread.Start();
				}
				catch (Exception ex)
				{
					this.running = false;
					if (Services.ProjectOperations != null)
					{
						Services.ProjectOperations.CurrentSelectedSolutionChanged -= this.OnCurrentSolutionChanged;
						Services.ProjectOperations.CurrentSelectedSolutionClosed -= this.OnCurrentSolutionChanged;
					}
					this.DeleteDescriptor();
					LogConfig.Logger.Error("Start external import server failed.", ex);
				}
			}
		}

		public void Dispose()
		{
			Thread thread = null;
			lock (this.syncRoot)
			{
				if (!this.running && this.listenerThread == null)
				{
					this.DeleteDescriptor();
					return;
				}
				this.running = false;
				if (Services.ProjectOperations != null)
				{
					Services.ProjectOperations.CurrentSelectedSolutionChanged -= this.OnCurrentSolutionChanged;
					Services.ProjectOperations.CurrentSelectedSolutionClosed -= this.OnCurrentSolutionChanged;
				}
				if (this.activePipe != null)
				{
					try
					{
						this.activePipe.Dispose();
					}
					catch
					{
					}
					this.activePipe = null;
				}
				thread = this.listenerThread;
				this.listenerThread = null;
			}
			if (thread != null && thread != Thread.CurrentThread)
			{
				thread.Join(2000);
			}
			this.DeleteDescriptor();
		}

		private void Listen()
		{
			while (this.running)
			{
				NamedPipeServerStream pipe = null;
				try
				{
					pipe = new NamedPipeServerStream(this.pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None);
					lock (this.syncRoot)
					{
						if (!this.running)
						{
							pipe.Dispose();
							break;
						}
						this.activePipe = pipe;
					}
					pipe.WaitForConnection();
					if (this.running && pipe.IsConnected)
					{
						this.HandleConnection(pipe);
					}
				}
				catch (ObjectDisposedException)
				{
					if (this.running)
					{
						LogConfig.Logger.Error("External import pipe was disposed unexpectedly.");
					}
				}
				catch (Exception ex)
				{
					if (this.running)
					{
						LogConfig.Logger.Error("External import pipe failed.", ex);
					}
				}
				finally
				{
					lock (this.syncRoot)
					{
						if (this.activePipe == pipe)
						{
							this.activePipe = null;
						}
					}
					if (pipe != null)
					{
						pipe.Dispose();
					}
				}
			}
		}

		private void HandleConnection(Stream pipe)
		{
			ExternalImportRequest request = null;
			ExternalImportResponse response;
			try
			{
				request = ExternalImportProtocol.ReadMessage<ExternalImportRequest>(pipe);
				if (request == null)
				{
					response = Fail(string.Empty, "INVALID_REQUEST", "External import request is empty.");
				}
				else if (request.ProtocolVersion != ExternalImportProtocol.CurrentVersion)
				{
					response = Fail(request.RequestId, "PROTOCOL_MISMATCH", "The external import protocol version is not supported.");
				}
				else
				{
					response = this.InvokeImport(request);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Handle external import request failed.", ex);
				response = Fail(request == null ? string.Empty : request.RequestId, "SERVER_ERROR", ex.Message);
			}

			try
			{
				ExternalImportProtocol.WriteMessage(pipe, response);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Write external import response failed.", ex);
			}
		}

		private ExternalImportResponse InvokeImport(ExternalImportRequest request)
		{
			ExternalImportResponse response = null;
			Exception importException = null;
			ManualResetEvent completed = new ManualResetEvent(false);
			GLib.Idle.Add(delegate
			{
				try
				{
					ExternalCsdImportResult result = ExternalCsdImportService.Instance.Import(new ExternalCsdImportRequest
					{
						ProjectFile = request.ProjectFile,
						CsdFile = request.CsdFile,
						ResourceFolder = request.ResourceFolder,
						TargetDirectory = request.TargetDirectory,
						Overwrite = request.Overwrite,
						OpenAfterImport = request.OpenAfterImport
					});
					response = MapResponse(request.RequestId, result);
				}
				catch (Exception ex)
				{
					importException = ex;
				}
				finally
				{
					completed.Set();
				}
				return false;
			});
			if (!completed.WaitOne(TimeSpan.FromMinutes(10.0)))
			{
				return Fail(request.RequestId, "IMPORT_TIMEOUT", "Cocos Studio did not finish the import within ten minutes.");
			}
			completed.Dispose();
			if (importException != null)
			{
				throw importException;
			}
			return response ?? Fail(request.RequestId, "SERVER_ERROR", "Cocos Studio returned no import result.");
		}

		private void OnCurrentSolutionChanged(object sender, SolutionEventArgs e)
		{
			this.WriteDescriptor();
		}

		private void WriteDescriptor()
		{
			try
			{
				ExternalImportInstance descriptor = new ExternalImportInstance
				{
					ProtocolVersion = ExternalImportProtocol.CurrentVersion,
					InstanceId = this.instanceId,
					ProcessId = Process.GetCurrentProcess().Id,
					PipeName = this.pipeName,
					StartedAtUtc = this.startedAtUtc
				};
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					descriptor.ProjectFile = Path.GetFullPath(Services.ProjectOperations.CurrentSelectedSolution.FileName.ToString());
				}
				if (Services.ProjectOperations.CurrentResourceGroup != null)
				{
					descriptor.ResourceRoot = Path.GetFullPath(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath);
				}
				string descriptorContent = JsonConvert.SerializeObject(descriptor, Formatting.Indented);
				string temporaryPath = this.descriptorPath + "." + this.instanceId + ".tmp";
				File.WriteAllText(temporaryPath, descriptorContent);
				if (File.Exists(this.descriptorPath))
				{
					try
					{
						File.Replace(temporaryPath, this.descriptorPath, null);
					}
					catch
					{
						File.Copy(temporaryPath, this.descriptorPath, true);
						File.Delete(temporaryPath);
					}
				}
				else
				{
					File.Move(temporaryPath, this.descriptorPath);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Write external import instance descriptor failed.", ex);
			}
		}

		private void DeleteDescriptor()
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(this.descriptorPath) && File.Exists(this.descriptorPath))
				{
					File.Delete(this.descriptorPath);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Delete external import instance descriptor failed.", ex);
			}
		}

		private static ExternalImportResponse MapResponse(string requestId, ExternalCsdImportResult result)
		{
			return new ExternalImportResponse
			{
				Success = result.Success,
				RequestId = requestId ?? string.Empty,
				Code = result.Code,
				Message = result.Message,
				ImportedCsd = result.ImportedCsd,
				ImportedResourceFolder = result.ImportedResourceFolder,
				OpenedCsd = result.OpenedCsd,
				Warnings = result.Warnings
			};
		}

		private static ExternalImportResponse Fail(string requestId, string code, string message)
		{
			return new ExternalImportResponse
			{
				Success = false,
				RequestId = requestId ?? string.Empty,
				Code = code,
				Message = message ?? string.Empty
			};
		}

		private readonly object syncRoot = new object();

		private readonly string instanceId;

		private readonly string startedAtUtc;

		private volatile bool running;

		private string pipeName;

		private string descriptorPath;

		private Thread listenerThread;

		private NamedPipeServerStream activePipe;

		private static readonly ExternalImportServer instance = new ExternalImportServer();
	}
}
