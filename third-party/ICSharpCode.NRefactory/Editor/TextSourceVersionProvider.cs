using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Provides ITextSourceVersion instances.
	/// </summary>
	public class TextSourceVersionProvider
	{
		public TextSourceVersionProvider()
		{
			this.currentVersion = new TextSourceVersionProvider.Version(this);
		}

		/// <summary>
		/// Gets the current version.
		/// </summary>
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

		private TextSourceVersionProvider.Version currentVersion;

		[DebuggerDisplay("Version #{id}")]
		private sealed class Version : ITextSourceVersion
		{
			internal Version(TextSourceVersionProvider provider)
			{
				this.provider = provider;
			}

			internal Version(TextSourceVersionProvider.Version prev)
			{
				this.provider = prev.provider;
				this.id = prev.id + 1;
			}

			public bool BelongsToSameDocumentAs(ITextSourceVersion other)
			{
				TextSourceVersionProvider.Version version = other as TextSourceVersionProvider.Version;
				return version != null && this.provider == version.provider;
			}

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

			private IEnumerable<TextChangeEventArgs> GetForwardChanges(TextSourceVersionProvider.Version other)
			{
				for (TextSourceVersionProvider.Version node = this; node != other; node = node.next)
				{
					yield return node.change;
				}
				yield break;
			}

			public int MoveOffsetTo(ITextSourceVersion other, int oldOffset, AnchorMovementType movement)
			{
				int num = oldOffset;
				foreach (TextChangeEventArgs textChangeEventArgs in this.GetChangesTo(other))
				{
					num = textChangeEventArgs.GetNewOffset(num, movement);
				}
				return num;
			}

			private readonly TextSourceVersionProvider provider;

			private readonly int id;

			internal TextChangeEventArgs change;

			internal TextSourceVersionProvider.Version next;
		}
	}
}
