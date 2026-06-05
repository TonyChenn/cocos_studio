using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Basic
{
	// Token: 0x0200000F RID: 15
	[DataItem("UserConfigService")]
	public class UserConfigService : IExtendedDataItem
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003BD0 File Offset: 0x00001DD0
		private static string FileName
		{
			get
			{
				return "Profile.config";
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003BE8 File Offset: 0x00001DE8
		private static string FilePath
		{
			get
			{
				return Option.GetUserConfigFileByName(UserConfigService.FileName);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003C04 File Offset: 0x00001E04
		[Obsolete]
		private static string OldFilePath
		{
			get
			{
				return Option.GetUserConfigFileByName("User.config");
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003C20 File Offset: 0x00001E20
		[Obsolete]
		private static string OldFilePath2
		{
			get
			{
				return Option.GetUserConfigFileByName("UserConfig.config");
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003C3C File Offset: 0x00001E3C
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003C53 File Offset: 0x00001E53
		[ItemProperty("Version")]
		public string Version { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003C5C File Offset: 0x00001E5C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003C73 File Offset: 0x00001E73
		[ItemProperty("IsUseMouseWheel/Value")]
		public bool IsUseMouseWheel { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003C7C File Offset: 0x00001E7C
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003C93 File Offset: 0x00001E93
		[ItemProperty("IsSimplifyDefaultRes/Value")]
		public bool IsSimplifyDefaultRes { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003C9C File Offset: 0x00001E9C
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003CB3 File Offset: 0x00001EB3
		[ItemProperty("MultiplySample/Value")]
		public int MultiplySample { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003CBC File Offset: 0x00001EBC
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003CD3 File Offset: 0x00001ED3
		[ItemProperty("IsShowAnchorPoint/Value")]
		public bool IsShowAnchorPoint { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003CDC File Offset: 0x00001EDC
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003CF3 File Offset: 0x00001EF3
		[ItemProperty("IsDragChangeSize/Value")]
		public bool IsDragChangeSize { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003CFC File Offset: 0x00001EFC
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003D13 File Offset: 0x00001F13
		[ItemProperty("IsShowSimulatorCmd/Value")]
		public bool IsShowSimulatorCmd { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003D1C File Offset: 0x00001F1C
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003D33 File Offset: 0x00001F33
		[ItemProperty("SimulatorInitScale/Value")]
		public int SimulatorInitScale { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003D3C File Offset: 0x00001F3C
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003D53 File Offset: 0x00001F53
		[ItemProperty("SDKPath/Value")]
		public string SDKPath { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003D5C File Offset: 0x00001F5C
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003D73 File Offset: 0x00001F73
		[ItemProperty("NDKPath/Value")]
		public string NDKPath { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003D7C File Offset: 0x00001F7C
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003D93 File Offset: 0x00001F93
		[ItemProperty("ANTPath/Value")]
		public string ANTPath { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003D9C File Offset: 0x00001F9C
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003DB3 File Offset: 0x00001FB3
		[ItemProperty("JDKPath/Value")]
		public string JDKPath { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003DBC File Offset: 0x00001FBC
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003DD3 File Offset: 0x00001FD3
		[ItemProperty("CocosStorePath/Value")]
		public string CocosStorePath { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003DDC File Offset: 0x00001FDC
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003DF3 File Offset: 0x00001FF3
		[ItemProperty("FeedBackEmail/Value")]
		public string FeedBackEmail { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003DFC File Offset: 0x00001FFC
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00003E13 File Offset: 0x00002013
		[ItemProperty("PaletteColors/Value")]
		public string PaletteColors { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003E1C File Offset: 0x0000201C
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003E34 File Offset: 0x00002034
		[ItemProperty("ResolutionList/Value")]
		public List<ResolutionConfig> ResolutionList
		{
			get
			{
				return this.resolutionList;
			}
			set
			{
				this.resolutionList = value;
				this.RaisePropertyChanged("ResolutionList");
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003E4C File Offset: 0x0000204C
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003E63 File Offset: 0x00002063
		[ItemProperty("GuidesColor/Value")]
		public string GuidesColor { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003E6C File Offset: 0x0000206C
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00003E83 File Offset: 0x00002083
		[ItemProperty("CustomConfigs")]
		public Dictionary<string, IUserConfig> CustomConfigs { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003E8C File Offset: 0x0000208C
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003EA2 File Offset: 0x000020A2
		public static UserConfigService Instance { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003EAC File Offset: 0x000020AC
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003EC3 File Offset: 0x000020C3
		public bool IsTestEnvironment { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003ECC File Offset: 0x000020CC
		public IDictionary ExtendedProperties
		{
			get
			{
				return this._extendedProperties;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000B7 RID: 183 RVA: 0x00003EE4 File Offset: 0x000020E4
		// (remove) Token: 0x060000B8 RID: 184 RVA: 0x00003F20 File Offset: 0x00002120
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060000B9 RID: 185 RVA: 0x00003F5C File Offset: 0x0000215C
		static UserConfigService()
		{
			UserConfigService.Initialize();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003F68 File Offset: 0x00002168
		private static void Initialize()
		{
			string filePath = UserConfigService.FilePath;
			if (File.Exists(filePath))
			{
				try
				{
					UserConfigService.Instance = UserConfigSerializer.ReadFile(filePath);
				}
				catch (Exception exception)
				{
					UserConfigService.Instance = null;
					LogConfig.Logger.Error(string.Format("读取用户配置信息时出错，文件路径：{0}", filePath), exception);
				}
			}
			if (UserConfigService.Instance == null)
			{
				UserConfigService.Instance = UserConfigService.LoadFromOldFile();
				UserConfigService.Instance.Save();
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003FFC File Offset: 0x000021FC
		private static UserConfigService LoadFromOldFile()
		{
			UserConfigService userConfigService = null;
			if (File.Exists(UserConfigService.OldFilePath2))
			{
				try
				{
					userConfigService = UserConfigSerializer.ReadFile(UserConfigService.OldFilePath2);
				}
				catch (Exception exception)
				{
					userConfigService = null;
					LogConfig.Logger.Error(string.Format("读取用户配置信息时出错，文件路径：{0}", UserConfigService.OldFilePath2), exception);
				}
			}
			if (userConfigService == null)
			{
				userConfigService = new UserConfigService();
				userConfigService.MigrateConfigData(UserConfigService.OldFilePath);
			}
			return userConfigService;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000408C File Offset: 0x0000228C
		private UserConfigService()
		{
			this.Version = string.Empty;
			this.InitDefaultValue();
			string userConfigFileByName = Option.GetUserConfigFileByName("Test.xml");
			this.IsTestEnvironment = File.Exists(userConfigFileByName);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000040D8 File Offset: 0x000022D8
		private void InitDefaultValue()
		{
			if (Platform.IsWindows)
			{
				this.IsUseMouseWheel = true;
				this.CocosStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cocos", "CocosStore");
			}
			else
			{
				this.IsUseMouseWheel = false;
				this.CocosStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Cocos", "CocosStore");
			}
			this.IsShowAnchorPoint = true;
			this.IsDragChangeSize = false;
			this.IsShowSimulatorCmd = false;
			this.SimulatorInitScale = 100;
			this.SDKPath = (this.NDKPath = (this.ANTPath = (this.JDKPath = (this.FeedBackEmail = (this.PaletteColors = string.Empty)))));
			this.GuidesColor = string.Empty;
			this.ResolutionList = ResolutionConfig.CreateDefaultList();
			this.CustomConfigs = new Dictionary<string, IUserConfig>();
			this.IsSimplifyDefaultRes = false;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000041D4 File Offset: 0x000023D4
		private void MigrateConfigData(string filePath)
		{
			if (File.Exists(filePath))
			{
				try
				{
					XElement xelement = XElement.Load(filePath);
					XElement xelement2 = xelement.Element("IsUseMouseWheel");
					if (xelement2 != null)
					{
						this.IsUseMouseWheel = (bool)xelement2;
					}
					xelement2 = xelement.Element("IsShowSimulatorCmd");
					if (xelement2 != null)
					{
						this.IsShowSimulatorCmd = (bool)xelement2;
					}
					xelement2 = xelement.Element("IsShowAnchorPoint");
					if (xelement2 != null)
					{
						this.IsShowAnchorPoint = (bool)xelement2;
					}
					xelement2 = xelement.Element("IsDragChangeSize");
					if (xelement2 != null)
					{
						this.IsDragChangeSize = (bool)xelement2;
					}
					xelement2 = xelement.Element("SimulatorInitScale");
					if (xelement2 != null)
					{
						this.SimulatorInitScale = (int)xelement2;
					}
					xelement2 = xelement.Element("SDKPath");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.SDKPath = xelement2.Value;
					}
					xelement2 = xelement.Element("NDKPath");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.NDKPath = xelement2.Value;
					}
					xelement2 = xelement.Element("ANTPath");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.ANTPath = xelement2.Value;
					}
					xelement2 = xelement.Element("JDKPath");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.JDKPath = xelement2.Value;
					}
					xelement2 = xelement.Element("CocosStorePath");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.CocosStorePath = xelement2.Value;
					}
					xelement2 = xelement.Element("FeedBackEmail");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.FeedBackEmail = xelement2.Value;
					}
					xelement2 = xelement.Element("PaletteColors");
					if (xelement2 != null)
					{
						this.PaletteColors = xelement2.Value;
					}
					xelement2 = xelement.Element("GuidesColor");
					if (xelement2 != null)
					{
						this.GuidesColor = xelement2.Value;
					}
					xelement2 = xelement.Element("MultiList");
					if (xelement2 != null && !string.IsNullOrWhiteSpace(xelement2.Value))
					{
						this.ResolutionList = new List<ResolutionConfig>();
						foreach (XElement xelement3 in xelement2.Elements("Content"))
						{
							this.ResolutionList.Add(new ResolutionConfig(xelement3.Element("Size").Value)
							{
								IsSelected = Convert.ToBoolean(xelement3.Element("IsChoice").Value),
								Order = Convert.ToInt32(xelement3.Element("Order").Value),
								Name = xelement3.Element("Name").Value
							});
						}
					}
					if (this.ResolutionList.Count == 0)
					{
						this.ResolutionList = ResolutionConfig.CreateDefaultList();
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("使用旧版本UserConfig配置文件升级时出错", exception);
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000045DC File Offset: 0x000027DC
		public void Save()
		{
			this.Version = "2.3.3.0";
			string filePath = UserConfigService.FilePath;
			try
			{
				UserConfigSerializer.WriteFile(UserConfigService.FilePath, this);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(string.Format("保存用户配置信息时出错，文件路径：{0}", filePath), exception);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004640 File Offset: 0x00002840
		private void RaisePropertyChanged(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("propertyNames");
			}
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x04000058 RID: 88
		public const string ExtendName = ".config";

		// Token: 0x04000059 RID: 89
		private List<ResolutionConfig> resolutionList;

		// Token: 0x0400005A RID: 90
		private Hashtable _extendedProperties = new Hashtable();
	}
}
