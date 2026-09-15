using System;

namespace ICSharpCode.NRefactory.Utils
{
	public class DefaultFormatStringError : IFormatStringError
	{
		public DefaultFormatStringError()
		{
			this.Message = "";
			this.OriginalText = "";
			this.SuggestedReplacementText = "";
		}

		public int StartLocation { get; set; }

		public int EndLocation { get; set; }

		public string Message { get; set; }

		public string OriginalText { get; set; }

		public string SuggestedReplacementText { get; set; }

		public override string ToString()
		{
			return string.Format("[DefaultFormatStringError: StartLocation={0}, EndLocation={1}, Message={2}, OriginalText={3}, SuggestedReplacementText={4}]", new object[]
			{
				this.StartLocation,
				this.EndLocation,
				this.Message,
				this.OriginalText,
				this.SuggestedReplacementText
			});
		}
	}
}
