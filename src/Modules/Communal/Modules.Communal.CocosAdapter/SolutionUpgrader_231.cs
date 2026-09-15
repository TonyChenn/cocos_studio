using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using Gdk;
using Modules.Communal.CocosAdapter.Platform;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_231 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_231.version;
			}
		}

		protected override bool OnUpgrade(Solution sln)
		{
			bool result = false;
			if (this.FormatPlatformEnum(sln))
			{
				result = true;
			}
			if (this.UpgradeSolutionCfgFile(sln))
			{
				result = true;
			}
			if (this.Upgrader_210(sln))
			{
				result = true;
			}
			return result;
		}

		private bool FormatPlatformEnum(Solution sln)
		{
			bool result;
			try
			{
				string filePath = UserData.GetFilePath(sln.FileName);
				if (!File.Exists(filePath))
				{
					result = false;
				}
				else
				{
					XElement xelement = XElement.Load(filePath);
					if (xelement == null)
					{
						result = false;
					}
					else
					{
						XElement xelement2 = xelement.Element("Properties");
						if (xelement2 == null)
						{
							result = false;
						}
						else
						{
							IEnumerable<XElement> enumerable = xelement2.Descendants("Item");
							xelement2 = null;
							foreach (XElement xelement3 in enumerable)
							{
								if (xelement3.Attribute("Key").Value.Equals("PackageParamsKey"))
								{
									xelement2 = xelement3;
									break;
								}
							}
							if (xelement2 == null)
							{
								result = false;
							}
							else
							{
								xelement2 = xelement2.Element("Value");
								if (xelement2 == null)
								{
									result = false;
								}
								else
								{
									enumerable = xelement2.Elements();
									xelement2 = null;
									foreach (XElement xelement4 in enumerable)
									{
										if (xelement4.Name.LocalName.Equals("Platform"))
										{
											xelement2 = xelement4;
											break;
										}
									}
									if (xelement2 == null)
									{
										result = false;
									}
									else
									{
										string value = xelement2.Attribute("Value").Value;
										if (string.IsNullOrEmpty(value))
										{
											result = false;
										}
										else if (value.Equals("IOS"))
										{
											xelement2.Attribute("Value").Value = "iOS";
											xelement.Save(filePath);
											result = true;
										}
										else if (value.Equals("HTML5"))
										{
											xelement2.Attribute("Value").Value = "Web";
											xelement.Save(filePath);
											result = true;
										}
										else if (value.Equals("All"))
										{
											int num = 0;
											foreach (IPlatform platform in Cocos2dxServices.PlatformServices.PlatformList)
											{
												num |= (int)platform.PlatformType;
											}
											xelement2.Attribute("Value").Value = num.ToString();
											xelement.Save(filePath);
											result = true;
										}
										else
										{
											result = false;
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("升级平台枚举时出错", exception);
				result = false;
			}
			return result;
		}

		private bool UpgradeSolutionCfgFile(Solution sln)
		{
			bool result;
			try
			{
				string filePath = SolutionConfig.GetFilePath(sln);
				bool? flag = this.CheckIsOldVersionProperties(filePath);
				if (flag == null || !flag.Value)
				{
					result = false;
				}
				else
				{
					PropertyBag propertyBag = this.LoadOldVersionProperties(filePath);
					if (propertyBag == null)
					{
						result = false;
					}
					else
					{
						sln.Config = this.UpgradeUserProperties(propertyBag, filePath);
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("升级.cfg文件时出错", exception);
				result = false;
			}
			return result;
		}

		private bool? CheckIsOldVersionProperties(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return null;
			}
			XElement xelement = XElement.Load(filePath);
			if (xelement == null)
			{
				return null;
			}
			XAttribute xattribute = xelement.Attribute("Version");
			if (xattribute == null)
			{
				return new bool?(true);
			}
			string value = xattribute.Value;
			return new bool?(string.IsNullOrEmpty(value));
		}

		private PropertyBag LoadOldVersionProperties(string filePath)
		{
			PropertyBag result = null;
			XmlTextReader xmlTextReader = new XmlTextReader(filePath);
			try
			{
				xmlTextReader.MoveToContent();
				if (xmlTextReader.LocalName != "Properties")
				{
					return null;
				}
				result = (PropertyBag)new XmlDataSerializer(new DataContext())
				{
					SerializationContext = 
					{
						BaseFile = filePath
					}
				}.Deserialize(xmlTextReader, typeof(PropertyBag));
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Failed to load user solution properties.", ex);
				return null;
			}
			finally
			{
				xmlTextReader.Close();
			}
			return result;
		}

		private SolutionConfig UpgradeUserProperties(PropertyBag oldProp, string cfgFilePath)
		{
			SolutionConfig solutionConfig = new SolutionConfig(cfgFilePath);
			if (oldProp == null)
			{
				return solutionConfig;
			}
			if (oldProp.HasValue("PublishDirectory"))
			{
				solutionConfig.PublishDirectory = oldProp.GetValue<string>("PublishDirectory");
			}
			if (oldProp.HasValue("PackageDirectory"))
			{
				solutionConfig.PackageDirectory = oldProp.GetValue<string>("PackageDirectory");
			}
			if (oldProp.HasValue("IsPublishResource"))
			{
				bool value = oldProp.GetValue<bool>("IsPublishResource");
				solutionConfig.PublishType = (PublishType)Convert.ToInt32(value);
			}
			if (oldProp.HasValue("SolutionSize"))
			{
				solutionConfig.SolutionSize = oldProp.GetValue<string>("SolutionSize");
			}
			if (oldProp.HasValue("DefaultSerializer"))
			{
				solutionConfig.DefaultSerializer = oldProp.GetValue<string>("DefaultSerializer");
			}
			if (oldProp.HasValue("CustomSerializer"))
			{
				solutionConfig.CustomSerializer = oldProp.GetValue<string>("CustomSerializer");
			}
			if (oldProp.HasValue("IsNameStandardized"))
			{
				solutionConfig.IsNameStandardized = oldProp.GetValue<bool>("IsNameStandardized");
			}
			if (oldProp.HasValue("ResolutationName"))
			{
				solutionConfig.ResolutionName = oldProp.GetValue<string>("ResolutationName");
			}
			else
			{
				Size currentSize = Size.Empty;
				if (!string.IsNullOrWhiteSpace(solutionConfig.SolutionSize))
				{
					string[] array = solutionConfig.SolutionSize.Split(new char[]
					{
						'*'
					});
					int width;
					int height;
					if (array.Length == 2 && int.TryParse(array[0], out width) && int.TryParse(array[1], out height))
					{
						currentSize = new Size(width, height);
					}
				}
				ResolutionConfig resolutionConfig = Option.UserConfig.ResolutionList.FirstOrDefault((ResolutionConfig w) => (w.Width == currentSize.Width && w.Height == currentSize.Height) || (w.Width == currentSize.Height && w.Height == currentSize.Width));
				if (resolutionConfig != null)
				{
					solutionConfig.ResolutionName = resolutionConfig.Name;
				}
				else
				{
					solutionConfig.ResolutionName = "Default";
				}
			}
			CocosProperties cocosProperties = new CocosProperties();
			if (oldProp.HasValue("publishCocos2dxVersionText"))
			{
				cocosProperties.CreateFrameworkVersion = oldProp.GetValue<string>("publishCocos2dxVersionText");
			}
			if (oldProp.HasValue("CurrentFrameworkVersion"))
			{
				cocosProperties.CurrentFrameworkVersion = oldProp.GetValue<string>("CurrentFrameworkVersion");
			}
			if (oldProp.HasValue("publishCocos2dxCodeLanguage"))
			{
				cocosProperties.ProgramLanguage = oldProp.GetValue<EnumProgramLanguage>("publishCocos2dxCodeLanguage");
			}
			if (oldProp.HasValue("SolutionCodeType"))
			{
				cocosProperties.SolutionCodeType = oldProp.GetValue<EnumSolutionCodeType>("SolutionCodeType");
			}
			else if (oldProp.HasValue("IsCompleteSolution"))
			{
				if (oldProp.GetValue<bool>("IsCompleteSolution"))
				{
					cocosProperties.SolutionCodeType = EnumSolutionCodeType.Complete;
				}
				else
				{
					cocosProperties.SolutionCodeType = EnumSolutionCodeType.Resource;
				}
			}
			solutionConfig.CustomProperties["CCS_CocosPropertis"] = cocosProperties;
			return solutionConfig;
		}

		private bool Upgrader_210(Solution sln)
		{
			if (!sln.Config.CustomProperties.Keys.Contains("CCS_CocosPropertis"))
			{
				return false;
			}
			CocosProperties cocosProperties = sln.Config.CustomProperties["CCS_CocosPropertis"] as CocosProperties;
			string currentFrameworkVersion = cocosProperties.CurrentFrameworkVersion;
			bool result = false;
			if (cocosProperties.CurrentFrameworkVersion.Equals("cocos2d-x 3.4"))
			{
				cocosProperties.CurrentFrameworkVersion = "cocos2d-x-3.4";
				result = true;
			}
			else if (cocosProperties.CurrentFrameworkVersion.Equals("cocos2d-x 3.4rc1"))
			{
				cocosProperties.CurrentFrameworkVersion = "cocos2d-x-3.4rc1";
				result = true;
			}
			if (cocosProperties.CreateFrameworkVersion.Equals("cocos2d-x 3.4"))
			{
				cocosProperties.CreateFrameworkVersion = "cocos2d-x-3.4";
				result = true;
			}
			else if (cocosProperties.CreateFrameworkVersion.Equals("cocos2d-x 3.4rc1"))
			{
				cocosProperties.CreateFrameworkVersion = "cocos2d-x-3.4rc1";
				result = true;
			}
			return result;
		}

		private static readonly Version version = new Version("2.3.1.0");
	}
}
