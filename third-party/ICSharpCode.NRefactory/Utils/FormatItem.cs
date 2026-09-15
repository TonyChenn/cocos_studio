using System;

namespace ICSharpCode.NRefactory.Utils
{
	public class FormatItem : FormatStringSegmentBase
	{
		public FormatItem(int index, int? alignment = null, string formatString = null)
		{
			this.Index = index;
			this.Alignment = alignment;
			this.FormatString = formatString;
		}

		public int Index { get; private set; }

		public int? Alignment { get; private set; }

		public string FormatString { get; private set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(FormatItem))
			{
				return false;
			}
			FormatItem other = (FormatItem)obj;
			return this.FieldsEquals(other);
		}

		public bool Equals(FormatItem other)
		{
			return other != null && this.FieldsEquals(other);
		}

		private bool FieldsEquals(FormatItem other)
		{
			return this.Index == other.Index && this.Alignment == other.Alignment && this.FormatString == other.FormatString && base.StartLocation == other.StartLocation && base.EndLocation == other.EndLocation;
		}

		public override int GetHashCode()
		{
			int num = 23;
			num = num * 37 + this.Index.GetHashCode();
			num = num * 37 + this.Alignment.GetHashCode();
			num = num * 37 + this.FormatString.GetHashCode();
			num = num * 37 + base.StartLocation.GetHashCode();
			return num * 37 + base.EndLocation.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[FormatItem: Index={0}, Alignment={1}, FormatString={2}, StartLocation={3}, EndLocation={4}]", new object[]
			{
				this.Index,
				this.Alignment,
				this.FormatString,
				base.StartLocation,
				base.EndLocation
			});
		}
	}
}
