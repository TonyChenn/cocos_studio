using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x020001AD RID: 429
	internal class MD1FileFormat : IFileFormat
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0003BECE File Offset: 0x0003A0CE
		public bool SupportsMixedFormats
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003BED1 File Offset: 0x0003A0D1
		public FilePath GetValidFormatName(object obj, FilePath fileName)
		{
			if (obj is WorkspaceItem && !(obj is Solution))
			{
				return Path.ChangeExtension(fileName, ".mdw");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003BF00 File Offset: 0x0003A100
		public bool CanReadFile(FilePath file, Type expectedType)
		{
			string a = Path.GetExtension(file).ToLower();
			return a == ".mdw" && expectedType.IsAssignableFrom(typeof(WorkspaceItem));
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003BF3D File Offset: 0x0003A13D
		public bool CanWriteFile(object obj)
		{
			return obj is WorkspaceItem && !(obj is Solution);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003BF55 File Offset: 0x0003A155
		public List<FilePath> GetItemFiles(object obj)
		{
			return new List<FilePath>();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003BF5C File Offset: 0x0003A15C
		public void WriteFile(FilePath file, object node, IProgressMonitor monitor)
		{
			string text = null;
			try
			{
				try
				{
					if (File.Exists(file))
					{
						text = Path.GetTempFileName();
					}
				}
				catch (IOException)
				{
				}
				if (text == null)
				{
					this.WriteFileInternal(file, file, node, monitor);
				}
				else
				{
					this.WriteFileInternal(file, text, node, monitor);
					File.Delete(file);
					File.Move(text, file);
				}
			}
			catch
			{
				if (text != string.Empty && File.Exists(text))
				{
					File.Delete(text);
				}
				throw;
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003BFF4 File Offset: 0x0003A1F4
		private void WriteFileInternal(FilePath actualFile, FilePath outFile, object node, IProgressMonitor monitor)
		{
			this.WriteWorkspaceItem(actualFile, outFile, (WorkspaceItem)node, monitor);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003C008 File Offset: 0x0003A208
		private void WriteWorkspaceItem(FilePath actualFile, FilePath outFile, WorkspaceItem item, IProgressMonitor monitor)
		{
			Workspace workspace = item as Workspace;
			if (workspace != null)
			{
				monitor.BeginTask(null, workspace.Items.Count);
				try
				{
					foreach (WorkspaceItem workspaceItem in workspace.Items)
					{
						workspaceItem.Save(monitor);
						monitor.Step(1);
					}
				}
				finally
				{
					monitor.EndTask();
				}
			}
			StreamWriter streamWriter = new StreamWriter(outFile);
			try
			{
				monitor.BeginTask(GettextCatalog.GetString("Saving item: {0}", actualFile), 1);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(streamWriter);
				xmlTextWriter.Formatting = Formatting.Indented;
				new XmlDataSerializer(MD1ProjectService.DataContext)
				{
					SerializationContext = 
					{
						BaseFile = actualFile,
						ProgressMonitor = monitor
					}
				}.Serialize(streamWriter, item, typeof(WorkspaceItem));
			}
			catch (Exception exception)
			{
				monitor.ReportError(GettextCatalog.GetString("Could not save item: {0}", actualFile), exception);
				throw;
			}
			finally
			{
				monitor.EndTask();
				streamWriter.Close();
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003C150 File Offset: 0x0003A350
		public object ReadFile(FilePath fileName, Type expectedType, IProgressMonitor monitor)
		{
			string a = Path.GetExtension(fileName).ToLower();
			if (a != ".mdw")
			{
				throw new ArgumentException();
			}
			object obj = null;
			ProjectExtensionUtil.BeginLoadOperation();
			try
			{
				obj = this.ReadWorkspaceItemFile(fileName, monitor);
			}
			finally
			{
				ProjectExtensionUtil.EndLoadOperation();
			}
			IWorkspaceFileObject workspaceFileObject = obj as IWorkspaceFileObject;
			if (workspaceFileObject != null)
			{
				workspaceFileObject.ConvertToFormat(MD1ProjectService.FileFormat, false);
			}
			return obj;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003C1C0 File Offset: 0x0003A3C0
		private object ReadWorkspaceItemFile(FilePath fileName, IProgressMonitor monitor)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(new StreamReader(fileName));
			object result;
			try
			{
				monitor.BeginTask(string.Format(GettextCatalog.GetString("Loading workspace item: {0}"), fileName), 1);
				xmlTextReader.MoveToContent();
				WorkspaceItem workspaceItem = (WorkspaceItem)new XmlDataSerializer(MD1ProjectService.DataContext)
				{
					SerializationContext = 
					{
						BaseFile = fileName,
						ProgressMonitor = monitor
					}
				}.Deserialize(xmlTextReader, typeof(WorkspaceItem));
				workspaceItem.ConvertToFormat(MD1ProjectService.FileFormat, false);
				workspaceItem.FileName = fileName;
				result = workspaceItem;
			}
			catch (Exception exception)
			{
				monitor.ReportError(string.Format(GettextCatalog.GetString("Could not load solution item: {0}"), fileName), exception);
				throw;
			}
			finally
			{
				monitor.EndTask();
				xmlTextReader.Close();
			}
			return result;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0003C2A4 File Offset: 0x0003A4A4
		public void ConvertToFormat(object obj)
		{
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003C348 File Offset: 0x0003A548
		public IEnumerable<string> GetCompatibilityWarnings(object obj)
		{
			yield break;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003C365 File Offset: 0x0003A565
		public bool SupportsFramework(TargetFramework framework)
		{
			return true;
		}
	}
}
