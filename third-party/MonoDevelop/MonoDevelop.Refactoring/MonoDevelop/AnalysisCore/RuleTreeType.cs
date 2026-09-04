namespace MonoDevelop.AnalysisCore
{
	public class RuleTreeType
	{
		private string input;

		private string fileExtension;

		public string Input => input;

		public string FileExtension => fileExtension;

		public RuleTreeType(string input, string fileExtension)
		{
			this.input = input;
			this.fileExtension = fileExtension;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj is RuleTreeType ruleTreeType && input == ruleTreeType.input)
			{
				return fileExtension == ruleTreeType.fileExtension;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((input != null) ? input.GetHashCode() : 0) ^ ((fileExtension != null) ? fileExtension.GetHashCode() : 0);
		}
	}
}
