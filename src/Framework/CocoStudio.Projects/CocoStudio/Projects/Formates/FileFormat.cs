using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	public abstract class FileFormat : IFileFormat
	{
		internal static ResourceTypeManager ResourceTypeManager { get; private set; } = new ResourceTypeManager();

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

		public bool CanWriteFile(object obj)
		{
			return this.OnCanWriteFile(obj);
		}

		protected abstract bool OnCanWriteFile(object obj);

		protected abstract bool OnCanReadFile(FilePath file, Type expectedObjectType);

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

		protected virtual void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}

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

		protected abstract object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor);
	}
}
