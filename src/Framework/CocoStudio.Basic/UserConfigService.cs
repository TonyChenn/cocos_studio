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
	[DataItem("UserConfigService")]
	public class UserConfigService : IExtendedDataItem
	{
		private static string FileName
		{
			get
			{
				return "Profile.config";
			}
		}

		private static string FilePath
		{
			get
			{
				return Option.GetUserConfigFileByName(UserConfigService.FileName);
			}
		}

		[Obsolete]
		private static string OldFilePath
		{
			get
			{
				return Option.GetUserConfigFileByName("User.config");
			}
		}

		[Obsolete]
		private static string OldFilePath2
		{
			get
			{
				return Option.GetUserConfigFileByName("UserConfig.config");
			}
		}

		[ItemProperty("Version")]
		public string Version { get; private set; }

		[ItemProperty("IsUseMouseWheel/Value")]
		public bool IsUseMouseWheel { get; set; }

		[ItemProperty("IsSimplifyDefaultRes/Value")]
		public bool IsSimplifyDefaultRes { get; set; }

		[ItemProperty("MultiplySample/Value")]
		public int MultiplySample { get; set; }

		[ItemProperty("IsShowAnchorPoint/Value")]
		public bool IsShowAnchorPoint { get; set; }

		[ItemProperty("IsDragChangeSize/Value")]
		public bool IsDragChangeSize { get; set; }

		[ItemProperty("IsShowSimulatorCmd/Value")]
		public bool IsShowSimulatorCmd { get; set; }

		[ItemProperty("SimulatorInitScale/Value")]
		public int SimulatorInitScale { get; set; }

		[ItemProperty("SDKPath/Value")]
		public string SDKPath { get; set; }

		[ItemProperty("NDKPath/Value")]
		public string NDKPath { get; set; }

		[ItemProperty("ANTPath/Value")]
		public string ANTPath { get; set; }

		[ItemProperty("JDKPath/Value")]
		public string JDKPath { get; set; }

		[ItemProperty("CocosStorePath/Value")]
		public string CocosStorePath { get; set; }

		[ItemProperty("FeedBackEmail/Value")]
		public string FeedBackEmail { get; set; }

		[ItemProperty("PaletteColors/Value")]
		public string PaletteColors { get; set; }

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

		[ItemProperty("GuidesColor/Value")]
		public string GuidesColor { get; set; }

		[ItemProperty("CustomConfigs")]
		public Dictionary<string, IUserConfig> CustomConfigs { get; private set; }

		public static UserConfigService Instance { get; private set; }

		public bool IsTestEnvironment { get; private set; }

		public IDictionary ExtendedProperties
		{
			get
			{
				return this._extendedProperties;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		static UserConfigService()
		{
			UserConfigService.Initialize();
		}

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

		private UserConfigService()
		{
			this.Version = string.Empty;
			this.InitDefaultValue();
			string userConfigFileByName = Option.GetUserConfigFileByName("Test.xml");
			this.IsTestEnvironment = File.Exists(userConfigFileByName);
		}

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

		public const string ExtendName = ".config";

		private List<ResolutionConfig> resolutionList;

		private Hashtable _extendedProperties = new Hashtable();
	}
}
