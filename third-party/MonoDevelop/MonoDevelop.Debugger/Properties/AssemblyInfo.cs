using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin("Debugger", Namespace = "MonoDevelop", Version = "5.4.0", Category = "Debugging")]
[assembly: AssemblyCopyright("MIT X11")]
[assembly: AddinDescription("Support for Debugging projects")]
[assembly: AssemblyDescription("Support for Debugging projects")]
[assembly: AddinName("Debugger support for MonoDevelop")]
[assembly: AddinFlags(AddinFlags.Hidden)]
[assembly: AddinDependency("Core", "5.4.0")]
[assembly: AddinDependency("Ide", "5.4.0")]
[assembly: AssemblyProduct("MonoDevelop")]
[assembly: AssemblyTitle("Debugger support for MonoDevelop")]
[assembly: AssemblyVersion("2.6.0.0")]
