using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.CocosAdapter;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	public class SampleInfo : IComparable, IEquatable<SampleInfo>
	{
		public string SampleName { get; private set; }

		public string SamplePath { get; private set; }

		public Xwt.Drawing.Image Image { get; private set; }

		public string DisplayName { get; private set; }

		public string Description { get; private set; }

		public Version SampleVersion { get; private set; }

		public DateTime UpdateTime { get; private set; }

		public int Index { get; private set; }

		public string DefaultScene { get; private set; }

		public bool IsCompleteSln { get; private set; }

		public string FrameworkVersion { get; private set; }

		public EnumProgramLanguage SlnLanguage { get; private set; }

		private SampleInfo()
		{
		}

		public static SampleInfo CreateInstance(string sampleName, string samplePath)
		{
			string text = Path.Combine(samplePath, "SampleInfo.xml");
			if (!File.Exists(text))
			{
				return null;
			}
			string path = Path.Combine(samplePath, sampleName, sampleName) + ".ccs";
			if (!File.Exists(path))
			{
				return null;
			}
			SampleInfo sampleInfo = new SampleInfo();
			sampleInfo.SampleName = sampleName;
			sampleInfo.SamplePath = samplePath;
			string text2 = Path.Combine(samplePath, sampleName) + ".png";
			if (File.Exists(text2))
			{
				sampleInfo.Image = ImageIcon.GetIconFromFile(text2);
			}
			if (sampleInfo.Init(text))
			{
				return sampleInfo;
			}
			return null;
		}

		private bool Init(string xmlFilePath)
		{
			bool result;
			try
			{
				XElement xelement = XElement.Load(xmlFilePath);
				string currentName = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, false);
				XElement xelement2 = xelement.Element("DisplayName");
				if (xelement2 == null)
				{
					result = false;
				}
				else
				{
					xelement2 = xelement2.Element(currentName);
					if (xelement2 == null)
					{
						result = false;
					}
					else
					{
						this.DisplayName = (string)xelement2;
						xelement2 = xelement.Element("Description");
						if (xelement2 == null)
						{
							result = false;
						}
						else
						{
							xelement2 = xelement2.Element(currentName);
							if (xelement2 == null)
							{
								result = false;
							}
							else
							{
								this.Description = (string)xelement2;
								xelement2 = xelement.Element("Version");
								if (xelement2 == null)
								{
									result = false;
								}
								else
								{
									this.SampleVersion = Version.Parse((string)xelement2);
									xelement2 = xelement.Element("UpdateTime");
									if (xelement2 == null)
									{
										result = false;
									}
									else
									{
										this.UpdateTime = DateTime.Parse((string)xelement2);
										xelement2 = xelement.Element("Index");
										if (xelement2 == null)
										{
											result = false;
										}
										else
										{
											this.Index = (int)xelement2;
											xelement2 = xelement.Element("DefaultScene");
											if (xelement2 == null)
											{
												result = false;
											}
											else
											{
												this.DefaultScene = (string)xelement2;
												xelement2 = xelement.Element("IsCompleteSln");
												if (xelement2 == null)
												{
													result = false;
												}
												else
												{
													this.IsCompleteSln = (bool)xelement2;
													xelement2 = xelement.Element("FrameworkVersion");
													if (xelement2 == null)
													{
														result = false;
													}
													else
													{
														this.FrameworkVersion = (string)xelement2;
														xelement2 = xelement.Element("Language");
														if (xelement2 == null)
														{
															result = false;
														}
														else
														{
															this.SlnLanguage = (EnumProgramLanguage)Enum.Parse(typeof(EnumProgramLanguage), (string)xelement2);
															result = true;
														}
													}
												}
											}
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
				LogConfig.Logger.Error("创建示例信息时出错", exception);
				result = false;
			}
			return result;
		}

		public int CompareTo(object other)
		{
			return this.Index.CompareTo(((SampleInfo)other).Index);
		}

		public override int GetHashCode()
		{
			return this.SampleName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is SampleInfo && this.Equals((SampleInfo)obj);
		}

		public bool Equals(SampleInfo other)
		{
			return other != null && this.SampleName.Equals(other.SampleName);
		}

		private const string SampleXmlName = "SampleInfo.xml";
	}
}
