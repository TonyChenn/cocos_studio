using MonoDevelop.Ide.Tasks;

namespace MonoDevelop.SourceEditor
{
	public class ErrorText
	{
		public Task Task { get; set; }

		public bool IsError { get; set; }

		public string ErrorMessage { get; set; }

		public ErrorText(Task task, bool isError, string errorMessage)
		{
			Task = task;
			IsError = isError;
			ErrorMessage = errorMessage;
		}

		public override string ToString()
		{
			return $"[ErrorText: IsError={IsError}, ErrorMessage={ErrorMessage}]";
		}
	}
}
