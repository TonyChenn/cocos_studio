using System;

namespace MonoDevelop.Core
{
	/// <summary>
	/// The Property wrapper wraps a global property service value as an easy to use object.
	/// </summary>
	// Token: 0x0200004B RID: 75
	public class PropertyWrapper<T>
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00009A15 File Offset: 0x00007C15
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00009A1D File Offset: 0x00007C1D
		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.Set(value);
			}
		}

		/// <summary>
		/// Set the property to the specified value.
		/// </summary>
		/// <param name="newValue">
		/// The new value.
		/// </param>
		/// <returns>
		/// true, if the property has changed, false otherwise.
		/// </returns>
		// Token: 0x06000262 RID: 610 RVA: 0x00009A28 File Offset: 0x00007C28
		public bool Set(T newValue)
		{
			if (!object.Equals(this.value, newValue))
			{
				this.value = newValue;
				PropertyService.Set(this.propertyName, this.value);
				this.OnChanged(EventArgs.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009A78 File Offset: 0x00007C78
		public PropertyWrapper(string propertyName, T defaultValue)
		{
			this.propertyName = propertyName;
			this.value = PropertyService.Get<T>(propertyName, defaultValue);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009A94 File Offset: 0x00007C94
		public static implicit operator T(PropertyWrapper<T> watch)
		{
			return watch.value;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009A9C File Offset: 0x00007C9C
		protected virtual void OnChanged(EventArgs e)
		{
			EventHandler changed = this.Changed;
			if (changed != null)
			{
				changed(this, e);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000266 RID: 614 RVA: 0x00009ABC File Offset: 0x00007CBC
		// (remove) Token: 0x06000267 RID: 615 RVA: 0x00009AF4 File Offset: 0x00007CF4
		public event EventHandler Changed;

		// Token: 0x040000DE RID: 222
		private T value;

		// Token: 0x040000DF RID: 223
		private readonly string propertyName;
	}
}
