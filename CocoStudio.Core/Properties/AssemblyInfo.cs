using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CocoStudio.Core.Codons;
using Mono.Addins;
using MonoDevelop.Ide.Codons;

[assembly: AssemblyVersion("2.3.3.0")]
[assembly: AssemblyTitle("CocoStudio.Core")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Beijing Chukong Aipu Technology Co., Ltd")]
[assembly: AssemblyProduct("Cocos Studio")]
[assembly: AssemblyCopyright("Copyright © Chukong Aipu 2015")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("136acbd6-1991-40a2-a040-0dbfca2f4946")]
[assembly: AssemblyFileVersion("2.3.3.0")]
[assembly: InternalsVisibleTo("CocoStudio.Projects.Test")]
[assembly: InternalsVisibleTo("CocoStudio.ProjectsPublish.Test")]
[assembly: InternalsVisibleTo("Cocos.Tool")]
[assembly: ExtensionPoint(Path = "/CocoStudio/Ide/Pads", Name = "Main Window Pads", NodeType = typeof(PadCodon))]
[assembly: ExtensionPoint(Path = "CocoStudio/Ide/DisplayBuilder", Name = "Display Builder", NodeType = typeof(DisplayBuilderCodon))]
[assembly: AddinRoot("CocoStudio.Core", "2.3", CompatVersion = "2.3", Namespace = "CocoStudio")]
[assembly: AddinDependency("CocoStudio.Basic", "2.3")]
[assembly: AddinDependency("CocoStudio.Projects", "2.3")]
[assembly: AddinDependency("::MonoDevelop.Core", "5.4.0")]
[assembly: AddinDependency("::MonoDevelop.Ide", "5.4.0")]
