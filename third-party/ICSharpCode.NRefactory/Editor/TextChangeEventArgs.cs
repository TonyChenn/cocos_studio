using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Describes a change of the document text.
	/// This class is thread-safe.
	/// </summary>
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class TextChangeEventArgs : EventArgs
	{
		/// <summary>
		/// The offset at which the change occurs.
		/// </summary>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003B26 File Offset: 0x00002B26
		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		/// <summary>
		/// The text that was removed.
		/// </summary>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00003B2E File Offset: 0x00002B2E
		public ITextSource RemovedText
		{
			get
			{
				return this.removedText;
			}
		}

		/// <summary>
		/// The number of characters removed.
		/// </summary>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003B36 File Offset: 0x00002B36
		public int RemovalLength
		{
			get
			{
				return this.removedText.TextLength;
			}
		}

		/// <summary>
		/// The text that was inserted.
		/// </summary>
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00003B43 File Offset: 0x00002B43
		public ITextSource InsertedText
		{
			get
			{
				return this.insertedText;
			}
		}

		/// <summary>
		/// The number of characters inserted.
		/// </summary>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003B4B File Offset: 0x00002B4B
		public int InsertionLength
		{
			get
			{
				return this.insertedText.TextLength;
			}
		}

		/// <summary>
		/// Creates a new TextChangeEventArgs object.
		/// </summary>
		// Token: 0x060000F4 RID: 244 RVA: 0x00003B58 File Offset: 0x00002B58
		public TextChangeEventArgs(int offset, string removedText, string insertedText)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "offset must not be negative");
			}
			this.offset = offset;
			this.removedText = ((removedText != null) ? new StringTextSource(removedText) : StringTextSource.Empty);
			this.insertedText = ((insertedText != null) ? new StringTextSource(insertedText) : StringTextSource.Empty);
		}

		/// <summary>
		/// Creates a new TextChangeEventArgs object.
		/// </summary>
		// Token: 0x060000F5 RID: 245 RVA: 0x00003BB8 File Offset: 0x00002BB8
		public TextChangeEventArgs(int offset, ITextSource removedText, ITextSource insertedText)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "offset must not be negative");
			}
			this.offset = offset;
			this.removedText = (removedText ?? StringTextSource.Empty);
			this.insertedText = (insertedText ?? StringTextSource.Empty);
		}

		/// <summary>
		/// Gets the new offset where the specified offset moves after this document change.
		/// </summary>
		// Token: 0x060000F6 RID: 246 RVA: 0x00003C0C File Offset: 0x00002C0C
		public virtual int GetNewOffset(int offset, AnchorMovementType movementType = AnchorMovementType.Default)
		{
			if (offset >= this.Offset && offset <= this.Offset + this.RemovalLength)
			{
				if (movementType == AnchorMovementType.BeforeInsertion)
				{
					return this.Offset;
				}
				return this.Offset + this.InsertionLength;
			}
			else
			{
				if (offset > this.Offset)
				{
					return offset + this.InsertionLength - this.RemovalLength;
				}
				return offset;
			}
		}

		/// <summary>
		/// Creates TextChangeEventArgs for the reverse change.
		/// </summary>
		// Token: 0x060000F7 RID: 247 RVA: 0x00003C65 File Offset: 0x00002C65
		public virtual TextChangeEventArgs Invert()
		{
			return new TextChangeEventArgs(this.offset, this.insertedText, this.removedText);
		}

		// Token: 0x0400002C RID: 44
		private readonly int offset;

		// Token: 0x0400002D RID: 45
		private readonly ITextSource removedText;

		// Token: 0x0400002E RID: 46
		private readonly ITextSource insertedText;
	}
}
