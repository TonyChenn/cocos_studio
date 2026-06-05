using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Provides ITextSourceVersion instances.
	/// </summary>
	// Token: 0x0200001C RID: 28
	public class TextSourceVersionProvider
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00003FA5 File Offset: 0x00002FA5
		public TextSourceVersionProvider()
		{
			this.currentVersion = new TextSourceVersionProvider.Version(this);
		}

		/// <summary>
		/// Gets the current version.
		/// </summary>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00003FB9 File Offset: 0x00002FB9
		public ITextSourceVersion CurrentVersion
		{
			get
			{
				return this.currentVersion;
			}
		}

		/// <summary>
		/// Replaces the current version with a new version.
		/// </summary>
		/// <param name="change">Change from current version to new version</param>
		// Token: 0x0600011E RID: 286 RVA: 0x00003FC4 File Offset: 0x00002FC4
		public void AppendChange(TextChangeEventArgs change)
		{
			if (change == null)
			{
				throw new ArgumentNullException("change");
			}
			this.currentVersion.change = change;
			this.currentVersion.next = new TextSourceVersionProvider.Version(this.currentVersion);
			this.currentVersion = this.currentVersion.next;
		}

		// Token: 0x04000037 RID: 55
		private TextSourceVersionProvider.Version currentVersion;

		// Token: 0x0200001D RID: 29
		[DebuggerDisplay("Version #{id}")]
		private sealed class Version : ITextSourceVersion
		{
			// Token: 0x0600011F RID: 287 RVA: 0x00004012 File Offset: 0x00003012
			internal Version(TextSourceVersionProvider provider)
			{
				this.provider = provider;
			}

			// Token: 0x06000120 RID: 288 RVA: 0x00004021 File Offset: 0x00003021
			internal Version(TextSourceVersionProvider.Version prev)
			{
				this.provider = prev.provider;
				this.id = prev.id + 1;
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00004044 File Offset: 0x00003044
			public bool BelongsToSameDocumentAs(ITextSourceVersion other)
			{
				TextSourceVersionProvider.Version version = other as TextSourceVersionProvider.Version;
				return version != null && this.provider == version.provider;
			}

			// Token: 0x06000122 RID: 290 RVA: 0x0000406C File Offset: 0x0000306C
			public int CompareAge(ITextSourceVersion other)
			{
				if (other == null)
				{
					throw new ArgumentNullException("other");
				}
				TextSourceVersionProvider.Version version = other as TextSourceVersionProvider.Version;
				if (version == null || this.provider != version.provider)
				{
					throw new ArgumentException("Versions do not belong to the same document.");
				}
				return Math.Sign(this.id - version.id);
			}

			// Token: 0x06000123 RID: 291 RVA: 0x000040C4 File Offset: 0x000030C4
			public IEnumerable<TextChangeEventArgs> GetChangesTo(ITextSourceVersion other)
			{
				int num = this.CompareAge(other);
				TextSourceVersionProvider.Version version = (TextSourceVersionProvider.Version)other;
				if (num < 0)
				{
					return this.GetForwardChanges(version);
				}
				if (num > 0)
				{
					return from change in version.GetForwardChanges(this).Reverse<TextChangeEventArgs>()
					select change.Invert();
				}
				return EmptyList<TextChangeEventArgs>.Instance;
			}

			// Token: 0x06000124 RID: 292 RVA: 0x00004234 File Offset: 0x00003234
			private IEnumerable<TextChangeEventArgs> GetForwardChanges(TextSourceVersionProvider.Version other)
			{
				for (TextSourceVersionProvider.Version node = this; node != other; node = node.next)
				{
					yield return node.change;
				}
				yield break;
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00004258 File Offset: 0x00003258
			public int MoveOffsetTo(ITextSourceVersion other, int oldOffset, AnchorMovementType movement)
			{
				int num = oldOffset;
				foreach (TextChangeEventArgs textChangeEventArgs in this.GetChangesTo(other))
				{
					num = textChangeEventArgs.GetNewOffset(num, movement);
				}
				return num;
			}

			// Token: 0x04000038 RID: 56
			private readonly TextSourceVersionProvider provider;

			// Token: 0x04000039 RID: 57
			private readonly int id;

			// Token: 0x0400003A RID: 58
			internal TextChangeEventArgs change;

			// Token: 0x0400003B RID: 59
			internal TextSourceVersionProvider.Version next;
		}
	}
}
