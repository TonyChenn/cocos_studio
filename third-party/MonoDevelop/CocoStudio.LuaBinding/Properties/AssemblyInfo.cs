using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Addins;

[assembly: AddinDependency("CocoStudio.Core", "2.3")]
[assembly: AddinDependency("CocoStudio.Projects", "2.3")]
[assembly: AddinDependency("::MonoDevelop.Debugger", "5.4.0")]
[assembly: AddinDependency("::MonoDevelop.SourceEditor2", "5.4.0")]
[assembly: Addin("namespace CocoStudio.LuaBinding", "2.3", CompatVersion = "2.3", Namespace = "CocoStudio")]
[assembly: AssemblyTitle("CocoStudio.LuaBinding")]
[assembly: AddinDependency("CocoStudio.SourceEditor", "2.3")]
[assembly: AddinDependency("::MonoDevelop.Core", "5.4.0")]
[assembly: AddinDependency("::MonoDevelop.Ide", "5.4.0")]
[assembly: AssemblyCompany("Beijing Chukong Aipu Technology Co., Ltd")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("Cocos Studio")]
[assembly: AssemblyCopyright("Copyright © Chukong Aipu 2015")]
[assembly: AssemblyVersion("0.0.0.0")]
