using System;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;

namespace MonoDevelop.DesignerSupport
{
	public class ErrorInFileException : Exception
	{
		private int line;

		private int column;

		private string fileName;

		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
			}
		}

		public int Column
		{
			get
			{
				return column;
			}
			set
			{
				column = value;
			}
		}

		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
			}
		}

		public ErrorInFileException(DomRegion region, string fileName)
		{
			line = region.BeginLine;
			column = region.BeginColumn;
			this.fileName = fileName;
		}

		public ErrorInFileException(string fileName, int line, int column)
		{
			this.line = line;
			this.column = column;
			this.fileName = fileName;
		}

		public override string ToString()
		{
			return GettextCatalog.GetString("Error in file '{0}' at line {1}, column {2}.", fileName, line, column);
		}
	}
}
