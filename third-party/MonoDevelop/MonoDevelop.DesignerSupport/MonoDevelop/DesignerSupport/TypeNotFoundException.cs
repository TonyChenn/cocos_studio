using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;

namespace MonoDevelop.DesignerSupport
{
	public class TypeNotFoundException : ErrorInFileException
	{
		private string className;

		public string ClassName
		{
			get
			{
				return className;
			}
			set
			{
				className = value;
			}
		}

		public TypeNotFoundException(string className, DomRegion location, string fileName)
			: base(location, fileName)
		{
			ClassName = className;
		}

		public override string ToString()
		{
			return GettextCatalog.GetString("Could not find type '{0}'.", className);
		}
	}
}
