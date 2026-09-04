using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Mono.Addins;

[assembly: AssemblyVersion("2.3.3.0")]
[assembly: InternalsVisibleTo("Addins.LuaExtend")]
[assembly: AddinDependency("CocoStudio.Projects", "2.3")]
[assembly: AddinDependency("CocoStudio.PropertyGrid", "2.3")]
[assembly: InternalsVisibleTo("Modules.UI.ComTool")]
[assembly: InternalsVisibleTo("Addins.GAFExtend")]
[assembly: InternalsVisibleTo("Modules.Communal.Skeleton")]
[assembly: Addin("CocoStudio.Model", "2.3", Namespace = "CocoStudio")]
[assembly: AddinDependency("CocoStudio.Core", "2.3")]
[assembly: InternalsVisibleTo("Addins.ModelExtend")]
[assembly: AssemblyTitle("CocoStudio.Model")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Beijing Chukong Aipu Technology Co., Ltd")]
[assembly: AssemblyProduct("Cocos Studio")]
[assembly: AssemblyCopyright("Copyright © Chukong Aipu 2015")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("1ee85fa3-4f51-4102-a560-bb416ac9b3f6")]
[assembly: AssemblyFileVersion("2.3.3.0")]
[assembly: InternalsVisibleTo("CocoStudio.Model3D")]
[assembly: InternalsVisibleTo("Modules.Communal.Render")]
