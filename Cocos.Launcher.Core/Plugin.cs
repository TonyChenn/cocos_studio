using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000031 RID: 49
	public class Plugin
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00008845 File Offset: 0x00006A45
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000884D File Offset: 0x00006A4D
		[XmlElement("Name")]
		public string PluginName { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00008856 File Offset: 0x00006A56
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000885E File Offset: 0x00006A5E
		public string PluginPath { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00008867 File Offset: 0x00006A67
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00008870 File Offset: 0x00006A70
		[XmlElement("DownloadUrl")]
		public string PluginUrl
		{
			get
			{
				return this.pluginUrl;
			}
			set
			{
				this.pluginUrl = value;
				if (!string.IsNullOrEmpty(this.pluginUrl))
				{
					this.PluginFullName = this.pluginUrl.Split(new char[]
					{
						'/'
					}).LastOrDefault<string>();
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000088B4 File Offset: 0x00006AB4
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000088BC File Offset: 0x00006ABC
		public string PluginFullName
		{
			get
			{
				return this.pluginFullName;
			}
			set
			{
				this.pluginFullName = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000088C5 File Offset: 0x00006AC5
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000088CD File Offset: 0x00006ACD
		public float PluginFraction { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000088D6 File Offset: 0x00006AD6
		// (set) Token: 0x0600019D RID: 413 RVA: 0x000088DE File Offset: 0x00006ADE
		public float PluginSize { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000088E7 File Offset: 0x00006AE7
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000088EF File Offset: 0x00006AEF
		public string ImageUrl { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000088F8 File Offset: 0x00006AF8
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00008900 File Offset: 0x00006B00
		public string ImagePath { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00008909 File Offset: 0x00006B09
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00008911 File Offset: 0x00006B11
		public bool IsLoading { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000891A File Offset: 0x00006B1A
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00008922 File Offset: 0x00006B22
		public string OpenType { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000892B File Offset: 0x00006B2B
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00008933 File Offset: 0x00006B33
		public double Score { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000893C File Offset: 0x00006B3C
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00008944 File Offset: 0x00006B44
		[XmlElement("Type")]
		public string PluginType { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000894D File Offset: 0x00006B4D
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00008955 File Offset: 0x00006B55
		public string PluginVersion { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000895E File Offset: 0x00006B5E
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00008966 File Offset: 0x00006B66
		public string Action { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000896F File Offset: 0x00006B6F
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00008977 File Offset: 0x00006B77
		public string DateTime { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00008980 File Offset: 0x00006B80
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00008988 File Offset: 0x00006B88
		public string UninstallName { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00008991 File Offset: 0x00006B91
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00008999 File Offset: 0x00006B99
		public bool IsUninstall { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000089A2 File Offset: 0x00006BA2
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x000089AA File Offset: 0x00006BAA
		public string MacPath { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x000089B3 File Offset: 0x00006BB3
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x000089BB File Offset: 0x00006BBB
		public bool IsInstalled { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000089C4 File Offset: 0x00006BC4
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x000089CC File Offset: 0x00006BCC
		public string ProcedureExeName { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000089D5 File Offset: 0x00006BD5
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000089DD File Offset: 0x00006BDD
		public string UnZipPath { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000089E6 File Offset: 0x00006BE6
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000089EE File Offset: 0x00006BEE
		[XmlIgnore]
		public string PluginMD5 { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000089F7 File Offset: 0x00006BF7
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000089FF File Offset: 0x00006BFF
		[XmlIgnore]
		public bool IsInstalling { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00008A08 File Offset: 0x00006C08
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00008A10 File Offset: 0x00006C10
		[XmlIgnore]
		public bool IsUninstalling { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00008A19 File Offset: 0x00006C19
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00008A21 File Offset: 0x00006C21
		[XmlIgnore]
		public string VersionFromService { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00008A2A File Offset: 0x00006C2A
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00008A32 File Offset: 0x00006C32
		[XmlIgnore]
		public string DownloadUrlFromService { get; set; }

		// Token: 0x060001C6 RID: 454 RVA: 0x00008A3C File Offset: 0x00006C3C
		public Plugin()
		{
			this.PluginName = (this.PluginUrl = (this.ImageUrl = (this.ImagePath = (this.PluginFullName = (this.PluginType = (this.PluginVersion = (this.DateTime = (this.PluginMD5 = string.Empty))))))));
			this.UninstallName = (this.MacPath = (this.ProcedureExeName = (this.UnZipPath = (this.VersionFromService = (this.DownloadUrlFromService = string.Empty)))));
			this.PluginFraction = 0f;
			this.PluginSize = 0f;
			this.IsLoading = false;
			this.OpenType = string.Empty;
			this.Action = ActionType.none.ToString();
			this.Score = 0.0;
		}

		// Token: 0x04000093 RID: 147
		private string pluginUrl;

		// Token: 0x04000094 RID: 148
		private string pluginFullName;
	}
}
