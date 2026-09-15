using System;

namespace ICSharpCode.NRefactory.Utils
{
	public class TextSegment : FormatStringSegmentBase
	{
		public TextSegment(string text, int startLocation = 0, int? endLocation = null)
		{
			this.Text = text;
			base.StartLocation = startLocation;
			base.EndLocation = (endLocation ?? (startLocation + text.Length));
		}

		public string Text { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(TextSegment))
			{
				return false;
			}
			TextSegment textSegment = (TextSegment)obj;
			return object.Equals(this.Text, textSegment.Text);
		}

		public bool Equals(TextSegment other)
		{
			return other != null && (object.Equals(this.Text, other.Text) && base.StartLocation == other.StartLocation) && base.EndLocation == other.EndLocation;
		}

		public override int GetHashCode()
		{
			int num = 23;
			num = num * 37 + this.Text.GetHashCode();
			num = num * 37 + base.StartLocation.GetHashCode();
			return num * 37 + base.EndLocation.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[TextSegment: Text={0}, StartLocation={1}, EndLocation={2}]", this.Text, base.StartLocation, base.EndLocation);
		}
	}
}
