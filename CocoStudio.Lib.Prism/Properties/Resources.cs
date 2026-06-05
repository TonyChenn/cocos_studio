using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace CocoStudio.Lib.Prism.Properties
{
	// Token: 0x02000018 RID: 24
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002F80 File Offset: 0x00001180
		internal Resources()
		{
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002F8C File Offset: 0x0000118C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("CocoStudio.Lib.Prism.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002FD8 File Offset: 0x000011D8
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002FEF File Offset: 0x000011EF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002FF8 File Offset: 0x000011F8
		internal static string AdapterInvalidTypeException
		{
			get
			{
				return Resources.ResourceManager.GetString("AdapterInvalidTypeException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003020 File Offset: 0x00001220
		internal static string CannotChangeRegionNameException
		{
			get
			{
				return Resources.ResourceManager.GetString("CannotChangeRegionNameException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003048 File Offset: 0x00001248
		internal static string CannotCreateNavigationTarget
		{
			get
			{
				return Resources.ResourceManager.GetString("CannotCreateNavigationTarget", Resources.resourceCulture);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003070 File Offset: 0x00001270
		internal static string CannotRegisterCompositeCommandInItself
		{
			get
			{
				return Resources.ResourceManager.GetString("CannotRegisterCompositeCommandInItself", Resources.resourceCulture);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003098 File Offset: 0x00001298
		internal static string CannotRegisterSameCommandTwice
		{
			get
			{
				return Resources.ResourceManager.GetString("CannotRegisterSameCommandTwice", Resources.resourceCulture);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000030C0 File Offset: 0x000012C0
		internal static string CanOnlyAddTypesThatInheritIFromRegionBehavior
		{
			get
			{
				return Resources.ResourceManager.GetString("CanOnlyAddTypesThatInheritIFromRegionBehavior", Resources.resourceCulture);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000030E8 File Offset: 0x000012E8
		internal static string ConfigurationStoreCannotBeNull
		{
			get
			{
				return Resources.ResourceManager.GetString("ConfigurationStoreCannotBeNull", Resources.resourceCulture);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003110 File Offset: 0x00001310
		internal static string ContentControlHasContentException
		{
			get
			{
				return Resources.ResourceManager.GetString("ContentControlHasContentException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003138 File Offset: 0x00001338
		internal static string CyclicDependencyFound
		{
			get
			{
				return Resources.ResourceManager.GetString("CyclicDependencyFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003160 File Offset: 0x00001360
		internal static string DeactiveNotPossibleException
		{
			get
			{
				return Resources.ResourceManager.GetString("DeactiveNotPossibleException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003188 File Offset: 0x00001388
		internal static string DefaultTextLoggerPattern
		{
			get
			{
				return Resources.ResourceManager.GetString("DefaultTextLoggerPattern", Resources.resourceCulture);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000031B0 File Offset: 0x000013B0
		internal static string DelegateCommandDelegatesCannotBeNull
		{
			get
			{
				return Resources.ResourceManager.GetString("DelegateCommandDelegatesCannotBeNull", Resources.resourceCulture);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000031D8 File Offset: 0x000013D8
		internal static string DelegateCommandInvalidGenericPayloadType
		{
			get
			{
				return Resources.ResourceManager.GetString("DelegateCommandInvalidGenericPayloadType", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003200 File Offset: 0x00001400
		internal static string DependencyForUnknownModule
		{
			get
			{
				return Resources.ResourceManager.GetString("DependencyForUnknownModule", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003228 File Offset: 0x00001428
		internal static string DependencyOnMissingModule
		{
			get
			{
				return Resources.ResourceManager.GetString("DependencyOnMissingModule", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003250 File Offset: 0x00001450
		internal static string DirectoryNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("DirectoryNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003278 File Offset: 0x00001478
		internal static string DuplicatedModule
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicatedModule", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000032A0 File Offset: 0x000014A0
		internal static string DuplicatedModuleGroup
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicatedModuleGroup", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000032C8 File Offset: 0x000014C8
		internal static string FailedToGetType
		{
			get
			{
				return Resources.ResourceManager.GetString("FailedToGetType", Resources.resourceCulture);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000032F0 File Offset: 0x000014F0
		internal static string FailedToLoadModule
		{
			get
			{
				return Resources.ResourceManager.GetString("FailedToLoadModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003318 File Offset: 0x00001518
		internal static string FailedToLoadModuleNoAssemblyInfo
		{
			get
			{
				return Resources.ResourceManager.GetString("FailedToLoadModuleNoAssemblyInfo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003340 File Offset: 0x00001540
		internal static string FailedToRetrieveModule
		{
			get
			{
				return Resources.ResourceManager.GetString("FailedToRetrieveModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003368 File Offset: 0x00001568
		internal static string HostControlCannotBeNull
		{
			get
			{
				return Resources.ResourceManager.GetString("HostControlCannotBeNull", Resources.resourceCulture);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003390 File Offset: 0x00001590
		internal static string HostControlCannotBeSetAfterAttach
		{
			get
			{
				return Resources.ResourceManager.GetString("HostControlCannotBeSetAfterAttach", Resources.resourceCulture);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000033B8 File Offset: 0x000015B8
		internal static string HostControlMustBeATabControl
		{
			get
			{
				return Resources.ResourceManager.GetString("HostControlMustBeATabControl", Resources.resourceCulture);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000033E0 File Offset: 0x000015E0
		internal static string IEnumeratorObsolete
		{
			get
			{
				return Resources.ResourceManager.GetString("IEnumeratorObsolete", Resources.resourceCulture);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003408 File Offset: 0x00001608
		internal static string InvalidArgumentAssemblyUri
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidArgumentAssemblyUri", Resources.resourceCulture);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003430 File Offset: 0x00001630
		internal static string InvalidDelegateRerefenceTypeException
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidDelegateRerefenceTypeException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003458 File Offset: 0x00001658
		internal static string ItemsControlHasItemsSourceException
		{
			get
			{
				return Resources.ResourceManager.GetString("ItemsControlHasItemsSourceException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003480 File Offset: 0x00001680
		internal static string MappingExistsException
		{
			get
			{
				return Resources.ResourceManager.GetString("MappingExistsException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000034A8 File Offset: 0x000016A8
		internal static string ModuleDependenciesNotMetInGroup
		{
			get
			{
				return Resources.ResourceManager.GetString("ModuleDependenciesNotMetInGroup", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000034D0 File Offset: 0x000016D0
		internal static string ModuleNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("ModuleNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000034F8 File Offset: 0x000016F8
		internal static string ModulePathCannotBeNullOrEmpty
		{
			get
			{
				return Resources.ResourceManager.GetString("ModulePathCannotBeNullOrEmpty", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003520 File Offset: 0x00001720
		internal static string ModuleTypeNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("ModuleTypeNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003548 File Offset: 0x00001748
		internal static string NavigationInProgress
		{
			get
			{
				return Resources.ResourceManager.GetString("NavigationInProgress", Resources.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003570 File Offset: 0x00001770
		internal static string NavigationServiceHasNoRegion
		{
			get
			{
				return Resources.ResourceManager.GetString("NavigationServiceHasNoRegion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003598 File Offset: 0x00001798
		internal static string NoRegionAdapterException
		{
			get
			{
				return Resources.ResourceManager.GetString("NoRegionAdapterException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000035C0 File Offset: 0x000017C0
		internal static string NoRetrieverCanRetrieveModule
		{
			get
			{
				return Resources.ResourceManager.GetString("NoRetrieverCanRetrieveModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000035E8 File Offset: 0x000017E8
		internal static string OnViewRegisteredException
		{
			get
			{
				return Resources.ResourceManager.GetString("OnViewRegisteredException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003610 File Offset: 0x00001810
		internal static string PropertySupport_ExpressionNotProperty_Exception
		{
			get
			{
				return Resources.ResourceManager.GetString("PropertySupport_ExpressionNotProperty_Exception", Resources.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003638 File Offset: 0x00001838
		internal static string PropertySupport_NotMemberAccessExpression_Exception
		{
			get
			{
				return Resources.ResourceManager.GetString("PropertySupport_NotMemberAccessExpression_Exception", Resources.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003660 File Offset: 0x00001860
		internal static string PropertySupport_StaticExpression_Exception
		{
			get
			{
				return Resources.ResourceManager.GetString("PropertySupport_StaticExpression_Exception", Resources.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003688 File Offset: 0x00001888
		internal static string RegionBehaviorAttachCannotBeCallWithNullRegion
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionBehaviorAttachCannotBeCallWithNullRegion", Resources.resourceCulture);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000036B0 File Offset: 0x000018B0
		internal static string RegionBehaviorRegionCannotBeSetAfterAttach
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionBehaviorRegionCannotBeSetAfterAttach", Resources.resourceCulture);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000036D8 File Offset: 0x000018D8
		internal static string RegionCreationException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionCreationException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003700 File Offset: 0x00001900
		internal static string RegionManagerWithDifferentNameException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionManagerWithDifferentNameException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003728 File Offset: 0x00001928
		internal static string RegionNameCannotBeEmptyException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionNameCannotBeEmptyException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003750 File Offset: 0x00001950
		internal static string RegionNameExistsException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionNameExistsException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003778 File Offset: 0x00001978
		internal static string RegionNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000037A0 File Offset: 0x000019A0
		internal static string RegionNotInRegionManagerException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionNotInRegionManagerException", Resources.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000037C8 File Offset: 0x000019C8
		internal static string RegionViewExistsException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionViewExistsException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000037F0 File Offset: 0x000019F0
		internal static string RegionViewNameExistsException
		{
			get
			{
				return Resources.ResourceManager.GetString("RegionViewNameExistsException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003818 File Offset: 0x00001A18
		internal static string StartupModuleDependsOnAnOnDemandModule
		{
			get
			{
				return Resources.ResourceManager.GetString("StartupModuleDependsOnAnOnDemandModule", Resources.resourceCulture);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003840 File Offset: 0x00001A40
		internal static string StringCannotBeNullOrEmpty
		{
			get
			{
				return Resources.ResourceManager.GetString("StringCannotBeNullOrEmpty", Resources.resourceCulture);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003868 File Offset: 0x00001A68
		internal static string StringCannotBeNullOrEmpty1
		{
			get
			{
				return Resources.ResourceManager.GetString("StringCannotBeNullOrEmpty1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003890 File Offset: 0x00001A90
		internal static string TypeWithKeyNotRegistered
		{
			get
			{
				return Resources.ResourceManager.GetString("TypeWithKeyNotRegistered", Resources.resourceCulture);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000038B8 File Offset: 0x00001AB8
		internal static string UpdateRegionException
		{
			get
			{
				return Resources.ResourceManager.GetString("UpdateRegionException", Resources.resourceCulture);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000038E0 File Offset: 0x00001AE0
		internal static string ValueMustBeOfTypeModuleInfo
		{
			get
			{
				return Resources.ResourceManager.GetString("ValueMustBeOfTypeModuleInfo", Resources.resourceCulture);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003908 File Offset: 0x00001B08
		internal static string ValueNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("ValueNotFound", Resources.resourceCulture);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003930 File Offset: 0x00001B30
		internal static string ViewNotInRegionException
		{
			get
			{
				return Resources.ResourceManager.GetString("ViewNotInRegionException", Resources.resourceCulture);
			}
		}

		// Token: 0x04000024 RID: 36
		private static ResourceManager resourceMan;

		// Token: 0x04000025 RID: 37
		private static CultureInfo resourceCulture;
	}
}
