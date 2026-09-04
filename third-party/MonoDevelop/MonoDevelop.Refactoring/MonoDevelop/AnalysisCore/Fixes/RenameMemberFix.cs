using ICSharpCode.NRefactory.TypeSystem;

namespace MonoDevelop.AnalysisCore.Fixes
{
	public class RenameMemberFix : IAnalysisFix
	{
		public string NewName { get; private set; }

		public string OldName { get; private set; }

		public string IdString { get; set; }

		public IEntity Item { get; private set; }

		public string FixType => "RenameMember";

		public RenameMemberFix(IEntity item, string oldName, string newName)
		{
			OldName = oldName;
			NewName = newName;
			Item = item;
		}
	}
}
