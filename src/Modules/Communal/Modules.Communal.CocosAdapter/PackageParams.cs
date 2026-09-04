using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000023 RID: 35
	[Extension(Type = typeof(IUserData))]
	public class PackageParams : IUserData
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006013 File Offset: 0x00004213
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000601B File Offset: 0x0000421B
		public FilePath Directory { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006024 File Offset: 0x00004224
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000602C File Offset: 0x0000422C
		public string ProjectName { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006035 File Offset: 0x00004235
		public Cocos2dxInfo EngineInfo
		{
			get
			{
				if (this.engineInfo == null)
				{
					this.engineInfo = Cocos2dxInfo.GetFramework(this.FrameworkVersion);
				}
				return this.engineInfo;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006056 File Offset: 0x00004256
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000605E File Offset: 0x0000425E
		[ItemProperty("LuaIsEncrypt/Value")]
		public bool LuaIsEncrypt { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006067 File Offset: 0x00004267
		// (set) Token: 0x0600011D RID: 285 RVA: 0x0000606F File Offset: 0x0000426F
		[ItemProperty("LuaEncryptKey/Value")]
		public string LuaEncryptKey
		{
			get
			{
				return this.luaEncryptKey;
			}
			set
			{
				this.luaEncryptKey = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006078 File Offset: 0x00004278
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00006080 File Offset: 0x00004280
		[ItemProperty("LuaEncryptSign/Value")]
		public string LuaEncryptSign
		{
			get
			{
				return this.luaEncryptSign;
			}
			set
			{
				this.luaEncryptSign = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006089 File Offset: 0x00004289
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00006091 File Offset: 0x00004291
		public FilePath AntProperties { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000609A File Offset: 0x0000429A
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000060A2 File Offset: 0x000042A2
		public FilePath AndroidManifest { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000060AB File Offset: 0x000042AB
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000060B3 File Offset: 0x000042B3
		[ItemProperty("Android_PackageName/Value")]
		public string Android_PackageName { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000060BC File Offset: 0x000042BC
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000060C4 File Offset: 0x000042C4
		[ItemProperty("AndroidkeyStore/Value")]
		public string AndroidkeyStore
		{
			get
			{
				return this.androidKeystore;
			}
			set
			{
				this.androidKeystore = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000060CD File Offset: 0x000042CD
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000060D5 File Offset: 0x000042D5
		[ItemProperty("AndroidVersion/Value")]
		public string AndroidVersion
		{
			get
			{
				return this.androidVersion;
			}
			set
			{
				this.androidVersion = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000060DE File Offset: 0x000042DE
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000060E6 File Offset: 0x000042E6
		[ItemProperty("iOS_Target/Value")]
		public string iOS_Target
		{
			get
			{
				return this.target_iOS;
			}
			set
			{
				this.target_iOS = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000060EF File Offset: 0x000042EF
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000060F7 File Offset: 0x000042F7
		[ItemProperty("iOS_BundleID/Value")]
		public string iOS_BundleID
		{
			get
			{
				return this.iOS_bundleID;
			}
			set
			{
				this.iOS_bundleID = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006100 File Offset: 0x00004300
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00006108 File Offset: 0x00004308
		[ItemProperty("Platform/Value")]
		public EnumPlatform Platform
		{
			get
			{
				return this.enumPlatform;
			}
			set
			{
				this.enumPlatform = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006111 File Offset: 0x00004311
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00006119 File Offset: 0x00004319
		[ItemProperty("RunPlatform/Value")]
		public EnumPlatform RunPlatform
		{
			get
			{
				return this.enumRunPlatform;
			}
			set
			{
				this.enumRunPlatform = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00006122 File Offset: 0x00004322
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000612A File Offset: 0x0000432A
		[ItemProperty("FrameworkVersion/Value")]
		public string FrameworkVersion
		{
			get
			{
				return this.frameworkVersion;
			}
			set
			{
				this.engineInfo = null;
				this.frameworkVersion = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000613A File Offset: 0x0000433A
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00006142 File Offset: 0x00004342
		[ItemProperty("IsDebugKeystore/Value")]
		public bool IsDebugKeystore
		{
			get
			{
				return this.isDebugKeystore;
			}
			set
			{
				this.isDebugKeystore = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000614B File Offset: 0x0000434B
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00006153 File Offset: 0x00004353
		[ItemProperty("KeystorePassword/Value")]
		public string KeystorePassword
		{
			get
			{
				return this.keystorePassword;
			}
			set
			{
				this.keystorePassword = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000615C File Offset: 0x0000435C
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00006164 File Offset: 0x00004364
		[ItemProperty("KeystoreAliasName/Value")]
		public string KeystoreAliasName
		{
			get
			{
				return this.keystoreAliasName;
			}
			set
			{
				this.keystoreAliasName = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000616D File Offset: 0x0000436D
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00006175 File Offset: 0x00004375
		[ItemProperty("KeystoreAliasPassword/Value")]
		public string KeystoreAliasPassword
		{
			get
			{
				return this.keystoreAliasPassword;
			}
			set
			{
				this.keystoreAliasPassword = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000617E File Offset: 0x0000437E
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00006186 File Offset: 0x00004386
		public bool IsComplete { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000618F File Offset: 0x0000438F
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00006197 File Offset: 0x00004397
		[ItemProperty("EnableSourceMap/Value")]
		public bool EnableSourceMap { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000061A0 File Offset: 0x000043A0
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000061A8 File Offset: 0x000043A8
		[ItemProperty("EnableHTML5Advanced/Value")]
		public bool EnableHTML5Advanced { get; set; }

		// Token: 0x06000142 RID: 322 RVA: 0x000061B4 File Offset: 0x000043B4
		public PackageParams()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution != null)
			{
				this.Directory = currentSolution.BaseDirectory;
				this.ProjectName = currentSolution.Name;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006236 File Offset: 0x00004436
		public void RefreshEngineInfo()
		{
			this.engineInfo = null;
		}

		// Token: 0x04000052 RID: 82
		public const string PackageParamsKey = "PackageParamsKey";

		// Token: 0x04000053 RID: 83
		private Cocos2dxInfo engineInfo;

		// Token: 0x04000054 RID: 84
		private string luaEncryptKey = string.Empty;

		// Token: 0x04000055 RID: 85
		private string luaEncryptSign = string.Empty;

		// Token: 0x04000056 RID: 86
		private string androidKeystore = string.Empty;

		// Token: 0x04000057 RID: 87
		private string androidVersion = string.Empty;

		// Token: 0x04000058 RID: 88
		private string target_iOS = string.Empty;

		// Token: 0x04000059 RID: 89
		private string iOS_bundleID = string.Empty;

		// Token: 0x0400005A RID: 90
		private EnumPlatform enumPlatform;

		// Token: 0x0400005B RID: 91
		private EnumPlatform enumRunPlatform;

		// Token: 0x0400005C RID: 92
		private string frameworkVersion;

		// Token: 0x0400005D RID: 93
		private bool isDebugKeystore = true;

		// Token: 0x0400005E RID: 94
		private string keystorePassword;

		// Token: 0x0400005F RID: 95
		private string keystoreAliasName;

		// Token: 0x04000060 RID: 96
		private string keystoreAliasPassword;
	}
}
