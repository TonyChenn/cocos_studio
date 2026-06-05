using System;
using System.Collections.Generic;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x020000FB RID: 251
	public class FileFormatManager
	{
		// Token: 0x060008DA RID: 2266 RVA: 0x000231A0 File Offset: 0x000213A0
		public void RegisterFileFormat(IFileFormat format, string id, string name, bool canDefault)
		{
			FileFormat item = new FileFormat(format, id, name, canDefault);
			this.fileFormats.Add(item);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000231C4 File Offset: 0x000213C4
		public void UnregisterFileFormat(IFileFormat format)
		{
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.Format == format)
				{
					this.fileFormats.Remove(fileFormat);
					break;
				}
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00023228 File Offset: 0x00021428
		public FileFormat[] GetFileFormats(string fileName, Type expectedType)
		{
			List<FileFormat> list = new List<FileFormat>();
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.Format.CanReadFile(fileName, expectedType))
				{
					list.Add(fileFormat);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002329C File Offset: 0x0002149C
		public FileFormat GetFileFormatForFile(string fileName, Type expectedType)
		{
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.Format.CanReadFile(fileName, expectedType))
				{
					return fileFormat;
				}
			}
			return null;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00023304 File Offset: 0x00021504
		public FileFormat[] GetFileFormatsForObject(object obj)
		{
			List<FileFormat> list = new List<FileFormat>();
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.CanWrite(obj))
				{
					list.Add(fileFormat);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002336C File Offset: 0x0002156C
		public FileFormat[] GetAllFileFormats()
		{
			List<FileFormat> list = new List<FileFormat>();
			list.AddRange(this.fileFormats);
			return list.ToArray();
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00023394 File Offset: 0x00021594
		public FileFormat GetFileFormat(string id)
		{
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.Id == id)
				{
					return fileFormat;
				}
			}
			return null;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000233F8 File Offset: 0x000215F8
		internal FileFormat GetFileFormat(IFileFormat format)
		{
			foreach (FileFormat fileFormat in this.fileFormats)
			{
				if (fileFormat.Format == format)
				{
					return fileFormat;
				}
			}
			return null;
		}

		// Token: 0x040002CA RID: 714
		private List<FileFormat> fileFormats = new List<FileFormat>();
	}
}
