using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.ControlLib;

namespace Cocos.Launcher.Core
{
	public class LoginInfo
	{
		public bool IsRememberPassWord { get; set; }

		public bool IsAutoLogin { get; set; }

		public string UserName { get; set; }

		public string UserPassword { get; set; }

		public string Access_token { get; set; }

		public string Refresh_token { get; set; }

		public string UserID { get; set; }

		public LoginInfo()
		{
			this.ExportConfigFolder = Path.Combine(Option.UserCustomerConfigFolder, "LoginInfo");
			this.LoginInfoConfigPath = Path.Combine(this.ExportConfigFolder, "LoginInfo.config");
			this.Load();
		}

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

		private void InitDefaulValue()
		{
			this.IsRememberPassWord = true;
			this.IsAutoLogin = true;
			this.UserName = string.Empty;
			this.UserPassword = string.Empty;
		}

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

		private readonly string ExportConfigFolder;

		private readonly string LoginInfoConfigPath;
	}
}
