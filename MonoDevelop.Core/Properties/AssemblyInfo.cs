using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Mono.Addins;

[assembly: AssemblyVersion("2.6.0.0")]
[assembly: AddinName("MonoDevelop Runtime")]
[assembly: AssemblyProduct("MonoDevelop")]
[assembly: AddinRoot("Core", Namespace = "MonoDevelop", Version = "5.4.0", CompatVersion = "5.0", Category = "MonoDevelop Core")]
[assembly: AddinDescription("Provides the core services of the MonoDevelop platform")]
[assembly: AssemblyTitle("MonoDevelop Runtime")]
[assembly: AssemblyDescription("Provides the core services of the MonoDevelop platform")]
[assembly: AssemblyCopyright("MIT/X11")]
[assembly: InternalsVisibleTo("CocosStudio.Launcher")]
[assembly: InternalsVisibleTo("CocoStudio.Core")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
