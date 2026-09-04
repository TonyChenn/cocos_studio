using System;
using System.IO;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Implements the ITextSource interface using a string.
	/// </summary>
	// Token: 0x0200001B RID: 27
	[Serializable]
	public class StringTextSource : ITextSource
	{
		/// <summary>
		/// Creates a new StringTextSource with the given text.
		/// </summary>
		// Token: 0x06000108 RID: 264 RVA: 0x00003E3B File Offset: 0x00002E3B
		public StringTextSource(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.text = text;
		}

		/// <summary>
		/// Creates a new StringTextSource with the given text.
		/// </summary>
		// Token: 0x06000109 RID: 265 RVA: 0x00003E58 File Offset: 0x00002E58
		public StringTextSource(string text, ITextSourceVersion version)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.text = text;
			this.version = version;
		}

		/// <inheritdoc />
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003E7C File Offset: 0x00002E7C
		public ITextSourceVersion Version
		{
			get
			{
				return this.version;
			}
		}

		/// <inheritdoc />
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003E84 File Offset: 0x00002E84
		public int TextLength
		{
			get
			{
				return this.text.Length;
			}
		}

		/// <inheritdoc />
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003E91 File Offset: 0x00002E91
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		/// <inheritdoc />
		// Token: 0x0600010D RID: 269 RVA: 0x00003E99 File Offset: 0x00002E99
		public ITextSource CreateSnapshot()
		{
			return this;
		}

		/// <inheritdoc />
		// Token: 0x0600010E RID: 270 RVA: 0x00003E9C File Offset: 0x00002E9C
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return new StringTextSource(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x0600010F RID: 271 RVA: 0x00003EB0 File Offset: 0x00002EB0
		public TextReader CreateReader()
		{
			return new StringReader(this.text);
		}

		/// <inheritdoc />
		// Token: 0x06000110 RID: 272 RVA: 0x00003EBD File Offset: 0x00002EBD
		public TextReader CreateReader(int offset, int length)
		{
			return new StringReader(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x06000111 RID: 273 RVA: 0x00003ED1 File Offset: 0x00002ED1
		public void WriteTextTo(TextWriter writer)
		{
			writer.Write(this.text);
		}

		/// <inheritdoc />
		// Token: 0x06000112 RID: 274 RVA: 0x00003EDF File Offset: 0x00002EDF
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			writer.Write(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x06000113 RID: 275 RVA: 0x00003EF4 File Offset: 0x00002EF4
		public char GetCharAt(int offset)
		{
			return this.text[offset];
		}

		/// <inheritdoc />
		// Token: 0x06000114 RID: 276 RVA: 0x00003F02 File Offset: 0x00002F02
		public string GetText(int offset, int length)
		{
			return this.text.Substring(offset, length);
		}

		/// <inheritdoc />
		// Token: 0x06000115 RID: 277 RVA: 0x00003F11 File Offset: 0x00002F11
		public string GetText(ISegment segment)
		{
			if (segment == null)
			{
				throw new ArgumentNullException("segment");
			}
			return this.text.Substring(segment.Offset, segment.Length);
		}

		/// <inheritdoc />
		// Token: 0x06000116 RID: 278 RVA: 0x00003F38 File Offset: 0x00002F38
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.text.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x06000117 RID: 279 RVA: 0x00003F48 File Offset: 0x00002F48
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.text.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x06000118 RID: 280 RVA: 0x00003F58 File Offset: 0x00002F58
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.text.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		// Token: 0x06000119 RID: 281 RVA: 0x00003F6A File Offset: 0x00002F6A
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.text.LastIndexOf(c, startIndex + count - 1, count);
		}

		/// <inheritdoc />
		// Token: 0x0600011A RID: 282 RVA: 0x00003F7E File Offset: 0x00002F7E
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.text.LastIndexOf(searchText, startIndex + count - 1, count, comparisonType);
		}

		/// <summary>
		/// Gets a text source containing the empty string.
		/// </summary>
		// Token: 0x04000034 RID: 52
		public static readonly StringTextSource Empty = new StringTextSource(string.Empty);

		// Token: 0x04000035 RID: 53
		private readonly string text;

		// Token: 0x04000036 RID: 54
		private readonly ITextSourceVersion version;
	}
}
