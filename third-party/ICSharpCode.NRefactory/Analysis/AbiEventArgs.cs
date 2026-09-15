using System;

namespace ICSharpCode.NRefactory.Analysis
{
	[Serializable]
	public sealed class AbiEventArgs : EventArgs
	{
		public string Message { get; set; }

		public AbiEventArgs(string message)
		{
			this.Message = message;
		}
	}
}
