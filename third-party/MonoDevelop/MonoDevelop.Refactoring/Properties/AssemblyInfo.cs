using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Addins;

[assembly: AssemblyTitle("Refactoring Support")]
[assembly: AssemblyDescription("Provides refactoring support to MonoDevelop")]
[assembly: AssemblyCopyright("X11")]
[assembly: AssemblyProduct("MonoDevelop")]
[assembly: AddinDependency("SourceEditor2", "5.4.0")]
[assembly: Addin("Refactoring", Namespace = "MonoDevelop", Version = "5.4.0", Category = "IDE extensions")]
[assembly: AddinDependency("DesignerSupport", "5.4.0")]
[assembly: AddinName("Refactoring Support")]
[assembly: AddinDescription("Provides refactoring support to MonoDevelop")]
[assembly: AddinDependency("Core", "5.4.0")]
[assembly: AddinDependency("Ide", "5.4.0")]
[assembly: AssemblyVersion("2.6.0.0")]
