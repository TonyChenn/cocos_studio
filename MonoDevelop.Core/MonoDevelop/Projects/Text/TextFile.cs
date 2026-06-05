using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MonoDevelop.Core;
using MonoDevelop.Projects.Utility;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001FA RID: 506
	public class TextFile : IEditableTextFile, ITextFile
	{
		// Token: 0x06001331 RID: 4913 RVA: 0x0004EF7E File Offset: 0x0004D17E
		public TextFile()
		{
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0004EF86 File Offset: 0x0004D186
		public TextFile(FilePath name)
		{
			this.Read(name);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0004EF95 File Offset: 0x0004D195
		public void Read(FilePath fileName)
		{
			this.Read(fileName, null);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0004EFA0 File Offset: 0x0004D1A0
		public static TextFile ReadFile(FilePath fileName)
		{
			TextFile textFile = new TextFile();
			textFile.Read(fileName);
			return textFile;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0004EFBC File Offset: 0x0004D1BC
		public static TextFile ReadFile(FilePath fileName, string encoding)
		{
			TextFile textFile = new TextFile();
			textFile.Read(fileName, encoding);
			return textFile;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0004EFD8 File Offset: 0x0004D1D8
		public static TextFile ReadFile(string path, Stream content)
		{
			TextFile textFile = new TextFile();
			textFile.name = path;
			textFile.Read(content, null);
			return textFile;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0004F000 File Offset: 0x0004D200
		public void Read(Stream stream, string encoding)
		{
			ByteOrderMark byteOrderMark = null;
			byte[] array;
			long num;
			string text;
			for (;;)
			{
				stream.Seek(0L, SeekOrigin.Begin);
				if (encoding == null)
				{
					if (ByteOrderMark.TryParse(stream, out byteOrderMark))
					{
						stream.Seek((long)byteOrderMark.Length, SeekOrigin.Begin);
					}
					else
					{
						stream.Seek(0L, SeekOrigin.Begin);
					}
				}
				array = new byte[(byteOrderMark != null) ? (stream.Length - (long)byteOrderMark.Length) : stream.Length];
				num = 0L;
				int num2;
				while ((num2 = stream.Read(array, (int)num, array.Length - (int)num)) > 0)
				{
					num += (long)num2;
				}
				if (encoding == null)
				{
					goto IL_A4;
				}
				text = TextFile.ConvertFromEncoding(array, num, encoding);
				if (text != null)
				{
					break;
				}
				encoding = null;
			}
			this.text = new StringBuilder(text);
			this.sourceEncoding = encoding;
			return;
			IL_A4:
			if (byteOrderMark != null)
			{
				string text2 = TextFile.ConvertFromEncoding(array, num, byteOrderMark.Name);
				if (text2 != null)
				{
					this.HadBOM = true;
					this.sourceEncoding = byteOrderMark.Name;
					this.text = new StringBuilder(text2);
					return;
				}
			}
			foreach (TextEncoding textEncoding in TextEncoding.ConversionEncodings)
			{
				string text3 = TextFile.ConvertFromEncoding(array, num, textEncoding.Id);
				if (text3 != null)
				{
					this.sourceEncoding = textEncoding.Id;
					this.text = new StringBuilder(text3);
					return;
				}
			}
			throw new Exception("Unknown text file encoding");
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0004F144 File Offset: 0x0004D344
		public void Read(FilePath fileName, string encoding)
		{
			this.name = fileName;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.Read(fileStream, encoding);
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0004F18C File Offset: 0x0004D38C
		public static string GetFileEncoding(FilePath fileName)
		{
			TextFile textFile = TextFile.ReadFile(fileName);
			return textFile.SourceEncoding;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0004F1A8 File Offset: 0x0004D3A8
		private static string ConvertFromEncoding(byte[] content, long nread, string fromEncoding)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(TextFile.ConvertToBytes(content, nread, "UTF-8", fromEncoding));
			}
			catch (Exception ex)
			{
				LoggingService.LogWarning("Fail to use encoding " + fromEncoding, ex);
				result = null;
			}
			return result;
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0004F1F8 File Offset: 0x0004D3F8
		private unsafe static int strlen(IntPtr str)
		{
			byte* ptr = (byte*)((void*)str);
			int num = 0;
			while (*ptr != 0)
			{
				ptr++;
				num++;
			}
			return num;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0004F220 File Offset: 0x0004D420
		public static string Utf8PtrToString(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			int num = TextFile.strlen(ptr);
			byte[] array = new byte[num];
			Marshal.Copy(ptr, array, 0, num);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0004F260 File Offset: 0x0004D460
		private static byte[] ConvertToBytes(byte[] content, long nread, string toEncoding, string fromEncoding)
		{
			if (nread > 2147483647L)
			{
				throw new Exception("Content too large.");
			}
			if (toEncoding == fromEncoding)
			{
				if (content.LongLength == nread)
				{
					return content;
				}
				byte[] array = new byte[nread];
				Array.Copy(content, array, nread);
				return array;
			}
			else
			{
				IntPtr zero = IntPtr.Zero;
				IntPtr zero2 = IntPtr.Zero;
				IntPtr textLength = new IntPtr(nread);
				IntPtr zero3 = IntPtr.Zero;
				IntPtr intPtr = TextFile.g_convert(content, textLength, toEncoding, fromEncoding, ref zero, ref zero2, ref zero3);
				if (intPtr != IntPtr.Zero)
				{
					int num = (int)((uint)zero2.ToInt64());
					byte[] array2 = new byte[num];
					Marshal.Copy(intPtr, array2, 0, array2.Length);
					TextFile.g_free(intPtr);
					return array2;
				}
				string arg = TextFile.Utf8PtrToString(((TextFile.GError)Marshal.PtrToStructure(zero3, typeof(TextFile.GError))).Msg);
				string message = string.Format("Failed to convert content from {0} to {1}: {2}.", fromEncoding, toEncoding, arg);
				InvalidEncodingException ex = new InvalidEncodingException(message);
				TextFile.g_error_free(zero3);
				throw ex;
			}
		}

		// Token: 0x0600133E RID: 4926
		[DllImport("libglib-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr g_convert(byte[] text, IntPtr textLength, string toCodeset, string fromCodeset, ref IntPtr read, ref IntPtr written, ref IntPtr err);

		// Token: 0x0600133F RID: 4927
		[DllImport("libglib-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void g_free(IntPtr ptr);

		// Token: 0x06001340 RID: 4928
		[DllImport("libglib-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern void g_error_free(IntPtr err);

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0004F354 File Offset: 0x0004D554
		public FilePath Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x0004F35C File Offset: 0x0004D55C
		// (set) Token: 0x06001343 RID: 4931 RVA: 0x0004F364 File Offset: 0x0004D564
		public bool HadBOM { get; private set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x0004F36D File Offset: 0x0004D56D
		public bool Modified
		{
			get
			{
				return this.modified;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x0004F375 File Offset: 0x0004D575
		// (set) Token: 0x06001346 RID: 4934 RVA: 0x0004F37D File Offset: 0x0004D57D
		public string SourceEncoding
		{
			get
			{
				return this.sourceEncoding;
			}
			set
			{
				this.sourceEncoding = value;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x0004F386 File Offset: 0x0004D586
		// (set) Token: 0x06001348 RID: 4936 RVA: 0x0004F393 File Offset: 0x0004D593
		public string Text
		{
			get
			{
				return this.text.ToString();
			}
			set
			{
				this.text = new StringBuilder(value);
				this.modified = true;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x0004F3A8 File Offset: 0x0004D5A8
		public int Length
		{
			get
			{
				return this.text.Length;
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0004F3B5 File Offset: 0x0004D5B5
		public string GetText(int startPosition, int endPosition)
		{
			return this.text.ToString(startPosition, endPosition - startPosition);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004F3C6 File Offset: 0x0004D5C6
		public char GetCharAt(int position)
		{
			if (position < this.text.Length)
			{
				return this.text[position];
			}
			return '\0';
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004F3E4 File Offset: 0x0004D5E4
		public int GetPositionFromLineColumn(int line, int column)
		{
			int num = 1;
			int num2 = 1;
			int num3 = 0;
			while (num3 < this.text.Length && num <= line)
			{
				if (line == num && column == num2)
				{
					return num3;
				}
				if (this.text[num3] == '\r')
				{
					if (num3 + 1 < this.text.Length && this.text[num3 + 1] == '\n')
					{
						num3++;
					}
					num++;
					num2 = 1;
				}
				else if (this.text[num3] == '\n')
				{
					num++;
					num2 = 1;
				}
				else
				{
					num2++;
				}
				num3++;
			}
			return -1;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004F474 File Offset: 0x0004D674
		public void GetLineColumnFromPosition(int position, out int line, out int column)
		{
			int num = 1;
			int num2 = 1;
			for (int i = 0; i < position; i++)
			{
				if (this.text[i] == '\r')
				{
					if (i + 1 < position && this.text[i + 1] == '\n')
					{
						i++;
					}
					num++;
					num2 = 1;
				}
				else if (this.text[i] == '\n')
				{
					num++;
					num2 = 1;
				}
				else
				{
					num2++;
				}
			}
			line = num;
			column = num2;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0004F4E8 File Offset: 0x0004D6E8
		public int GetLineLength(int line)
		{
			int num = this.GetPositionFromLineColumn(line, 1);
			if (num == -1)
			{
				return 0;
			}
			int num2 = 0;
			while (num < this.text.Length && this.text[num] != '\n' && this.text[num] != '\r')
			{
				num++;
				num2++;
			}
			return num2;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0004F53F File Offset: 0x0004D73F
		public int InsertText(int position, string textIn)
		{
			this.text.Insert(position, textIn);
			this.modified = true;
			if (textIn == null)
			{
				return 0;
			}
			return textIn.Length;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0004F561 File Offset: 0x0004D761
		public void DeleteText(int position, int length)
		{
			this.text.Remove(position, length);
			this.modified = true;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004F578 File Offset: 0x0004D778
		public void Save()
		{
			TextFile.WriteFile(this.name, this.text.ToString(), this.sourceEncoding, this.HadBOM);
			this.modified = false;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0004F5A4 File Offset: 0x0004D7A4
		public static void WriteFile(FilePath fileName, byte[] content, string encoding, ByteOrderMark bom, bool onlyIfChanged)
		{
			int num = content.Length + ((bom != null) ? bom.Length : 0);
			byte[] array;
			if (encoding != null)
			{
				array = TextFile.ConvertToBytes(content, content.LongLength, encoding, "UTF-8");
			}
			else
			{
				array = content;
			}
			if (onlyIfChanged)
			{
				FileInfo fileInfo = new FileInfo(fileName);
				if (fileInfo.Exists && fileInfo.Length == (long)num)
				{
					bool flag = false;
					using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						byte[] array2 = new byte[4096];
						int num2 = 0;
						int num3 = 0;
						int num4;
						while (!flag && (num4 = fileStream.Read(array2, 0, array2.Length)) > 0)
						{
							int num5 = 0;
							if (bom != null && num2 < bom.Length)
							{
								while (num5 < num4 && num2 < bom.Length)
								{
									if (bom.Bytes[num2] != array2[num5])
									{
										flag = true;
										break;
									}
									num2++;
									num5++;
								}
								if (flag)
								{
									break;
								}
							}
							while (num5 < num4 && num3 < array.Length)
							{
								if (array[num3] != array2[num5])
								{
									flag = true;
									break;
								}
								num3++;
								num5++;
							}
							if (num3 == array.Length && num5 < num4)
							{
								flag = true;
							}
						}
						if (num3 < array.Length)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						return;
					}
				}
			}
			string text = string.Concat(new object[]
			{
				Path.GetDirectoryName(fileName),
				Path.DirectorySeparatorChar,
				".#",
				Path.GetFileName(fileName)
			});
			FileStream fileStream2 = new FileStream(text, FileMode.Create, FileAccess.Write);
			if (bom != null)
			{
				fileStream2.Write(bom.Bytes, 0, bom.Length);
			}
			fileStream2.Write(array, 0, array.Length);
			fileStream2.Flush();
			fileStream2.Close();
			FileService.SystemRename(text, fileName);
			FileService.NotifyFileChanged(fileName);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0004F780 File Offset: 0x0004D980
		public static void WriteFile(FilePath fileName, string content, string encoding, ByteOrderMark bom, bool onlyIfChanged)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(content);
			TextFile.WriteFile(fileName, bytes, encoding, bom, onlyIfChanged);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0004F7A4 File Offset: 0x0004D9A4
		public static void WriteFile(FilePath fileName, string content, ByteOrderMark bom, bool onlyIfChanged)
		{
			TextFile.WriteFile(fileName, content, (bom != null) ? bom.Name : null, bom, onlyIfChanged);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004F7BB File Offset: 0x0004D9BB
		public static void WriteFile(FilePath fileName, string content, string encoding)
		{
			TextFile.WriteFile(fileName, content, encoding, false);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0004F7C8 File Offset: 0x0004D9C8
		public static void WriteFile(FilePath fileName, string content, string encoding, bool saveBOM)
		{
			ByteOrderMark bom = (saveBOM && encoding != null) ? ByteOrderMark.GetByName(encoding) : null;
			TextFile.WriteFile(fileName, content, encoding, bom, false);
		}

		// Token: 0x0400059C RID: 1436
		private const string LIBGLIB = "libglib-2.0-0.dll";

		// Token: 0x0400059D RID: 1437
		private FilePath name;

		// Token: 0x0400059E RID: 1438
		private StringBuilder text;

		// Token: 0x0400059F RID: 1439
		private string sourceEncoding;

		// Token: 0x040005A0 RID: 1440
		private bool modified;

		// Token: 0x020001FB RID: 507
		private struct GError
		{
			// Token: 0x040005A2 RID: 1442
			public int Domain;

			// Token: 0x040005A3 RID: 1443
			public int Code;

			// Token: 0x040005A4 RID: 1444
			public IntPtr Msg;
		}
	}
}
