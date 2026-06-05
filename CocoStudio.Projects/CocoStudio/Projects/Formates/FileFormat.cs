using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000011 RID: 17
	public abstract class FileFormat : IFileFormat
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B3D File Offset: 0x00000D3D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002B44 File Offset: 0x00000D44
		internal static ResourceTypeManager ResourceTypeManager { get; private set; } = new ResourceTypeManager();

		// Token: 0x06000045 RID: 69 RVA: 0x00002B58 File Offset: 0x00000D58
		public bool CanReadFile(FilePath file, Type expectedObjectType)
		{
			bool result;
			try
			{
				if (!File.Exists(file))
				{
					result = false;
				}
				else
				{
					result = this.OnCanReadFile(file, expectedObjectType);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Check file formate failed.", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public bool CanWriteFile(object obj)
		{
			return this.OnCanWriteFile(obj);
		}

		// Token: 0x06000047 RID: 71
		protected abstract bool OnCanWriteFile(object obj);

		// Token: 0x06000048 RID: 72
		protected abstract bool OnCanReadFile(FilePath file, Type expectedObjectType);

		// Token: 0x06000049 RID: 73 RVA: 0x00002BB4 File Offset: 0x00000DB4
		protected static bool CheckFileSuffix(FilePath file, params string[] suffixs)
		{
			string extension = file.Extension;
			if (suffixs == null)
			{
				return false;
			}
			foreach (string value in suffixs)
			{
				if (extension.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public void WriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			try
			{
				this.OnWriteFile(file, obj, monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Write file failed.", exception);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C30 File Offset: 0x00000E30
		protected virtual void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C34 File Offset: 0x00000E34
		public object ReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			object result;
			try
			{
				result = this.OnReadFile(file, expectedType, monitor);
			}
			catch (Exception exception)
			{
				this.ReportTypeNotFoundException(exception);
				monitor.ReportError("Read file failed.", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C78 File Offset: 0x00000E78
		private void ReportTypeNotFoundException(Exception exception)
		{
			for (Exception ex = exception; ex != null; ex = ex.InnerException)
			{
				if (ex.Message.StartsWith("Type not found: "))
				{
					LogConfig.Output.Error(string.Format(LanguageInfo.MessageBox260_TypeNotFound, ex.Message.Replace("Type not found: ", null)));
					LogConfig.Output.Error(LanguageInfo.MessageBox261_TypeNotFoundHint);
				}
			}
		}

		// Token: 0x0600004E RID: 78
		protected abstract object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor);
	}
}
