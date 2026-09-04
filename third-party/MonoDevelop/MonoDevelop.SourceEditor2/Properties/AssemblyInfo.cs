using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: AssemblyProduct("MonoDevelop")]
[assembly: AssemblyDescription("Provides a text editor for the MonoDevelop based on Mono.TextEditor")]
[assembly: AddinDependency("Debugger", "5.4.0")]
[assembly: AddinDependency("Ide", "5.4.0")]
[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("CocoStudio.SourceEditor")]
[assembly: AssemblyTitle("MonoDevelop Source Editor")]
[assembly: Addin("SourceEditor2", Namespace = "MonoDevelop", Version = "5.4.0", Category = "MonoDevelop Core", Flags = AddinFlags.Hidden)]
[assembly: AddinName("MonoDevelop Source Editor")]
[assembly: AddinDescription("Provides a text editor for the MonoDevelop based on Mono.TextEditor")]
[assembly: AddinDependency("Core", "5.4.0")]
[assembly: AssemblyVersion("2.6.0.0")]
