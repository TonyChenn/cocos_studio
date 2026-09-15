using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	[Serializable]
	public abstract class AbstractFreezable : IFreezable
	{
		/// <summary>
		/// Gets if this instance is frozen. Frozen instances are immutable and thus thread-safe.
		/// </summary>
		public bool IsFrozen
		{
			get
			{
				return this.isFrozen;
			}
		}

		/// <summary>
		/// Freezes this instance.
		/// </summary>
		public void Freeze()
		{
			if (!this.isFrozen)
			{
				this.FreezeInternal();
				this.isFrozen = true;
			}
		}

		protected virtual void FreezeInternal()
		{
		}

		private bool isFrozen;
	}
}
