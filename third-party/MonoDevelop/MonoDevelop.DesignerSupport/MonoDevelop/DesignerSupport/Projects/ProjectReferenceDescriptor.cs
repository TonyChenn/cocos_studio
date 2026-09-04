using System.Reflection;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class ProjectReferenceDescriptor : CustomDescriptor
	{
		private ProjectReference pref;

		[LocalizedCategory("Reference")]
		[LocalizedDescription("Type of the reference.")]
		[LocalizedDisplayName("Type")]
		public string Type
		{
			get
			{
				switch (pref.ReferenceType)
				{
				case ReferenceType.Assembly:
					return GettextCatalog.GetString("Assembly");
				case ReferenceType.Package:
					return GettextCatalog.GetString("Package");
				case ReferenceType.Project:
					return GettextCatalog.GetString("Project");
				case ReferenceType.Custom:
					return GettextCatalog.GetString("Custom");
				default:
					return string.Empty;
				}
			}
		}

		[LocalizedDisplayName("Project")]
		[LocalizedCategory("Reference")]
		[LocalizedDescription("Referenced project, when the reference is of type 'Project'.")]
		public string ProjectName
		{
			get
			{
				if (pref.ReferenceType == ReferenceType.Project)
				{
					return pref.Reference;
				}
				return string.Empty;
			}
		}

		[LocalizedDisplayName("Assembly Name")]
		[LocalizedCategory("Reference")]
		[LocalizedDescription("Name of the assembly.")]
		public string FullName
		{
			get
			{
				if (Path.Length > 0)
				{
					try
					{
						return AssemblyName.GetAssemblyName(Path).Name;
					}
					catch
					{
					}
				}
				return string.Empty;
			}
		}

		[LocalizedDescription("Version of the assembly.")]
		[LocalizedDisplayName("Assembly Version")]
		[LocalizedCategory("Reference")]
		public string Version
		{
			get
			{
				if (Path.Length > 0)
				{
					try
					{
						return AssemblyName.GetAssemblyName(Path).Version.ToString();
					}
					catch
					{
					}
				}
				return string.Empty;
			}
		}

		[LocalizedDisplayName("Path")]
		[LocalizedCategory("Reference")]
		[LocalizedDescription("Path to the assembly.")]
		public string Path
		{
			get
			{
				string[] referencedFileNames = pref.GetReferencedFileNames(IdeApp.Workspace.ActiveConfiguration);
				if (referencedFileNames.Length > 0)
				{
					return referencedFileNames[0];
				}
				return string.Empty;
			}
		}

		[LocalizedCategory("Build")]
		[LocalizedDisplayName("Local Copy")]
		[LocalizedDescription("Copy the referenced assembly to the output directory.")]
		public bool LocalCopy
		{
			get
			{
				if (pref.LocalCopy)
				{
					return pref.CanSetLocalCopy;
				}
				return false;
			}
			set
			{
				pref.LocalCopy = value;
			}
		}

		[LocalizedDescription("Require a specific version of the assembly. A warning will be issued if the specific version is not found in the system.")]
		[LocalizedDisplayName("Specific Version")]
		[LocalizedCategory("Build")]
		public bool SpecificVersion
		{
			get
			{
				return pref.SpecificVersion;
			}
			set
			{
				pref.SpecificVersion = value;
			}
		}

		[LocalizedDescription("Package that provides this reference.")]
		[LocalizedCategory("Reference")]
		[LocalizedDisplayName("Package")]
		public string Package
		{
			get
			{
				if (pref.ReferenceType == ReferenceType.Package && pref.Package != null)
				{
					return pref.Package.GetDisplayName();
				}
				return string.Empty;
			}
		}

		public ProjectReferenceDescriptor(ProjectReference pref)
		{
			this.pref = pref;
		}

		protected override bool IsReadOnly(string propertyName)
		{
			if (propertyName == "SpecificVersion")
			{
				return !pref.CanSetSpecificVersion;
			}
			if (propertyName == "LocalCopy")
			{
				return !pref.CanSetLocalCopy;
			}
			return false;
		}
	}
}
