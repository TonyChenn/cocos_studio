using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: AssemblyDescription("Supporting services and pads for visual design tools.")]
[assembly: AssemblyTitle("Visual Designer Support")]
[assembly: AssemblyCopyright("MIT X11")]
[assembly: AddinName("Visual Designer Support")]
[assembly: AddinDescription("Supporting services and pads for visual design tools")]
[assembly: AssemblyProduct("MonoDevelop")]
[assembly: Addin("DesignerSupport", Namespace = "MonoDevelop", Version = "5.4.0", Flags = AddinFlags.Hidden, Category = "MonoDevelop Core")]
[assembly: AddinDependency("Core", "5.4.0")]
[assembly: AddinDependency("Ide", "5.4.0")]
[assembly: AssemblyVersion("2.6.0.0")]
