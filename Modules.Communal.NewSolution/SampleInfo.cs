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
	// Token: 0x02000007 RID: 7
	public class SampleInfo : IComparable, IEquatable<SampleInfo>
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000025B3 File Offset: 0x000007B3
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000025BB File Offset: 0x000007BB
		public string SampleName { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000025C4 File Offset: 0x000007C4
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000025CC File Offset: 0x000007CC
		public string SamplePath { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000025D5 File Offset: 0x000007D5
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000025DD File Offset: 0x000007DD
		public Xwt.Drawing.Image Image { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025E6 File Offset: 0x000007E6
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000025EE File Offset: 0x000007EE
		public string DisplayName { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025F7 File Offset: 0x000007F7
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000025FF File Offset: 0x000007FF
		public string Description { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002608 File Offset: 0x00000808
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002610 File Offset: 0x00000810
		public Version SampleVersion { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002619 File Offset: 0x00000819
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002621 File Offset: 0x00000821
		public DateTime UpdateTime { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000262A File Offset: 0x0000082A
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002632 File Offset: 0x00000832
		public int Index { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000263B File Offset: 0x0000083B
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002643 File Offset: 0x00000843
		public string DefaultScene { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000264C File Offset: 0x0000084C
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002654 File Offset: 0x00000854
		public bool IsCompleteSln { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000265D File Offset: 0x0000085D
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002665 File Offset: 0x00000865
		public string FrameworkVersion { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000266E File Offset: 0x0000086E
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002676 File Offset: 0x00000876
		public EnumProgramLanguage SlnLanguage { get; private set; }

		// Token: 0x06000031 RID: 49 RVA: 0x0000267F File Offset: 0x0000087F
		private SampleInfo()
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002688 File Offset: 0x00000888
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

		// Token: 0x06000033 RID: 51 RVA: 0x00002710 File Offset: 0x00000910
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

		// Token: 0x06000034 RID: 52 RVA: 0x00002918 File Offset: 0x00000B18
		public int CompareTo(object other)
		{
			return this.Index.CompareTo(((SampleInfo)other).Index);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000293E File Offset: 0x00000B3E
		public override int GetHashCode()
		{
			return this.SampleName.GetHashCode();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000294B File Offset: 0x00000B4B
		public override bool Equals(object obj)
		{
			return obj is SampleInfo && this.Equals((SampleInfo)obj);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002963 File Offset: 0x00000B63
		public bool Equals(SampleInfo other)
		{
			return other != null && this.SampleName.Equals(other.SampleName);
		}

		// Token: 0x04000008 RID: 8
		private const string SampleXmlName = "SampleInfo.xml";
	}
}
