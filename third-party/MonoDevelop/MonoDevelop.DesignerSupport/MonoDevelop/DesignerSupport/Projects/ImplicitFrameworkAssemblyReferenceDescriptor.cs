using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class ImplicitFrameworkAssemblyReferenceDescriptor : CustomDescriptor
	{
		private readonly ImplicitFrameworkAssemblyReference aref;

		[LocalizedDescription("Name of the assembly.")]
		[LocalizedDisplayName("Assembly Name")]
		[LocalizedCategory("Reference")]
		public string FullName => aref.Assembly.FullName;

		[LocalizedCategory("Reference")]
		[LocalizedDescription("Path to the assembly.")]
		[LocalizedDisplayName("Path")]
		public string Path => aref.Assembly.Location;

		[LocalizedCategory("Reference")]
		[LocalizedDisplayName("Framework")]
		[LocalizedDescription("Framework that provides this reference.")]
		public string Package => aref.Assembly.Package.TargetFramework.ToString();

		public ImplicitFrameworkAssemblyReferenceDescriptor(ImplicitFrameworkAssemblyReference aref)
		{
			this.aref = aref;
		}
	}
}
