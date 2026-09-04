using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	public class ProjectItemPropertyProvider : IPropertyProvider
	{
		public object CreateProvider(object obj)
		{
			if (obj is ProjectFile file)
			{
				return new ProjectFileDescriptor(file);
			}
			if (obj is ProjectReference pref)
			{
				return new ProjectReferenceDescriptor(pref);
			}
			return new ImplicitFrameworkAssemblyReferenceDescriptor((ImplicitFrameworkAssemblyReference)obj);
		}

		public bool SupportsObject(object obj)
		{
			if (!(obj is ProjectFile) && !(obj is ProjectReference))
			{
				return obj is ImplicitFrameworkAssemblyReference;
			}
			return true;
		}
	}
}
