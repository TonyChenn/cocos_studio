using System.Reflection;
using Mono.Addins;

[assembly: AssemblyTitle("CocosStudio.ExternalImport.StudioPlugin")]
[assembly: AssemblyDescription("External CSD and resource folder import service for Cocos Studio")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Cocos Studio External Import")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyVersion("2.3.3.0")]
[assembly: AssemblyFileVersion("2.3.3.0")]
[assembly: Addin("ExternalImport", "2.3", Namespace = "CocoStudio")]
[assembly: AddinDependency("CocoStudio.Basic", "2.3")]
[assembly: AddinDependency("CocoStudio.Core", "2.3")]
[assembly: AddinDependency("CocoStudio.Projects", "2.3")]
[assembly: AddinDependency("::MonoDevelop.Core", "5.4.0")]
