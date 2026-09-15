using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(IUserData))]
	public class PackageParams : IUserData
	{
		public FilePath Directory { get; private set; }

		public string ProjectName { get; private set; }

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

		[ItemProperty("LuaIsEncrypt/Value")]
		public bool LuaIsEncrypt { get; set; }

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

		public FilePath AntProperties { get; set; }

		public FilePath AndroidManifest { get; set; }

		[ItemProperty("Android_PackageName/Value")]
		public string Android_PackageName { get; set; }

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

		public bool IsComplete { get; set; }

		[ItemProperty("EnableSourceMap/Value")]
		public bool EnableSourceMap { get; set; }

		[ItemProperty("EnableHTML5Advanced/Value")]
		public bool EnableHTML5Advanced { get; set; }

		public PackageParams()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution != null)
			{
				this.Directory = currentSolution.BaseDirectory;
				this.ProjectName = currentSolution.Name;
			}
		}

		public void RefreshEngineInfo()
		{
			this.engineInfo = null;
		}

		public const string PackageParamsKey = "PackageParamsKey";

		private Cocos2dxInfo engineInfo;

		private string luaEncryptKey = string.Empty;

		private string luaEncryptSign = string.Empty;

		private string androidKeystore = string.Empty;

		private string androidVersion = string.Empty;

		private string target_iOS = string.Empty;

		private string iOS_bundleID = string.Empty;

		private EnumPlatform enumPlatform;

		private EnumPlatform enumRunPlatform;

		private string frameworkVersion;

		private bool isDebugKeystore = true;

		private string keystorePassword;

		private string keystoreAliasName;

		private string keystoreAliasPassword;
	}
}
