using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.ControlLib;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000055 RID: 85
	public class LoginInfo
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000B1F5 File Offset: 0x000093F5
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000B1FD File Offset: 0x000093FD
		public bool IsRememberPassWord { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000B206 File Offset: 0x00009406
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000B20E File Offset: 0x0000940E
		public bool IsAutoLogin { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000B217 File Offset: 0x00009417
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000B21F File Offset: 0x0000941F
		public string UserName { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000B228 File Offset: 0x00009428
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000B230 File Offset: 0x00009430
		public string UserPassword { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000B239 File Offset: 0x00009439
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x0000B241 File Offset: 0x00009441
		public string Access_token { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000B24A File Offset: 0x0000944A
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000B252 File Offset: 0x00009452
		public string Refresh_token { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000B25B File Offset: 0x0000945B
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000B263 File Offset: 0x00009463
		public string UserID { get; set; }

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B26C File Offset: 0x0000946C
		public LoginInfo()
		{
			this.ExportConfigFolder = Path.Combine(Option.UserCustomerConfigFolder, "LoginInfo");
			this.LoginInfoConfigPath = Path.Combine(this.ExportConfigFolder, "LoginInfo.config");
			this.Load();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B2A8 File Offset: 0x000094A8
		private void Load()
		{
			if (File.Exists(this.LoginInfoConfigPath))
			{
				try
				{
					XmlDocument node = XmlAnalysis.ReaderXmlFile(this.LoginInfoConfigPath);
					XmlNode node2 = XmlAnalysis.GetNode(node, "LoginInfoConfig");
					this.IsRememberPassWord = Convert.ToBoolean(XmlAnalysis.GetNode(node2, "IsRememberPassWord").InnerText);
					this.UserName = XmlAnalysis.GetNode(node2, "UserName").InnerText;
					string innerText = XmlAnalysis.GetNode(node2, "UserPassword").InnerText;
					this.IsAutoLogin = Convert.ToBoolean(XmlAnalysis.GetNode(node2, "IsAutoLogin").InnerText);
					this.UserPassword = LoginEncrypt.Decrypt(innerText, "E99F9354-BC29-48A2-9839-F3D0DD83CCE5", Encoding.Default);
					return;
				}
				catch (Exception)
				{
					this.InitDefaulValue();
					File.Delete(this.LoginInfoConfigPath);
					return;
				}
			}
			this.InitDefaulValue();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B37C File Offset: 0x0000957C
		private void InitDefaulValue()
		{
			this.IsRememberPassWord = true;
			this.IsAutoLogin = true;
			this.UserName = string.Empty;
			this.UserPassword = string.Empty;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B3A4 File Offset: 0x000095A4
		public void Save()
		{
			try
			{
				if (!Directory.Exists(this.ExportConfigFolder))
				{
					Directory.CreateDirectory(this.ExportConfigFolder);
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(this.ExportConfigFolder);
				directoryInfo.Attributes = FileAttributes.Hidden;
				if (File.Exists(this.LoginInfoConfigPath))
				{
					File.Delete(this.LoginInfoConfigPath);
				}
				string content = LoginEncrypt.Encrypt(this.UserPassword, "E99F9354-BC29-48A2-9839-F3D0DD83CCE5");
				XDocument xdocument = new XDocument(new object[]
				{
					new XElement("LoginInfoConfig", new object[]
					{
						new XElement("UserName", this.UserName),
						new XElement("UserPassword", content),
						new XElement("IsRememberPassWord", this.IsRememberPassWord),
						new XElement("IsAutoLogin", this.IsAutoLogin)
					})
				});
				xdocument.Save(this.LoginInfoConfigPath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Save login info failed.", exception);
			}
		}

		// Token: 0x0400010B RID: 267
		private readonly string ExportConfigFolder;

		// Token: 0x0400010C RID: 268
		private readonly string LoginInfoConfigPath;
	}
}
