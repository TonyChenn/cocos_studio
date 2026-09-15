using System;

namespace OpenDialogs
{
	public struct RECT
	{
		public POINT Location
		{
			get
			{
				return new POINT((int)this.left, (int)this.top);
			}
			set
			{
				this.right -= this.left - (uint)value.x;
				this.bottom -= this.bottom - (uint)value.y;
				this.left = (uint)value.x;
				this.top = (uint)value.y;
			}
		}

		public uint Width
		{
			get
			{
				return this.right - this.left;
			}
			set
			{
				this.right = this.left + value;
			}
		}

		public uint Height
		{
			get
			{
				return this.bottom - this.top;
			}
			set
			{
				this.bottom = this.top + value;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.left,
				":",
				this.top,
				":",
				this.right,
				":",
				this.bottom
			});
		}

		public uint left;

		public uint top;

		public uint right;

		public uint bottom;
	}
}
