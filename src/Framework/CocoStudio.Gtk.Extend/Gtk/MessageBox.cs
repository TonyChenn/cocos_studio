using System;
using System.Diagnostics;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Ide;

namespace Gtk
{
	public class MessageBox
	{
		public static void Show(string info, MessageBoxImage image = MessageBoxImage.Other, Window parentWnd = null, string title = null)
		{
			MessageBox.Show(info, MessageBoxButton.Yes, image, parentWnd, EnumMainButton.Yes, title);
		}

		public static MessageBoxResult Show(string info, MessageBoxButton btnType, MessageBoxImage image = MessageBoxImage.Other, Window parentWnd = null, EnumMainButton mainBtn = EnumMainButton.Yes, string title = null)
		{
			ButtonText btnText = null;
			switch (btnType)
			{
			case MessageBoxButton.Yes:
				btnText = new ButtonText(LanguageInfo.Dialog_ButtonOK, false);
				break;
			case MessageBoxButton.YesNo:
				btnText = new ButtonText(LanguageInfo.Dialog_ButtonYes, LanguageInfo.Dialog_ButtonNo, false, false);
				break;
			case MessageBoxButton.YesNoCancel:
				btnText = new ButtonText(LanguageInfo.Dialog_ButtonYes, LanguageInfo.Dialog_ButtonNo, LanguageInfo.Dialog_ButtonCancel, false, false, false);
				break;
			}
			return MessageBox.Show(info, btnText, image, parentWnd, mainBtn, title);
		}

		public static MessageBoxResult Show(string info, ButtonText btnText, MessageBoxImage image = MessageBoxImage.Other, Window parentWnd = null, EnumMainButton mainBtn = EnumMainButton.Yes, string title = null)
		{
			MessageBoxResult result;
			if (Option.CurrentApp == EnumApp.Tool)
			{
				StackTrace stackTrace = new StackTrace();
				LogConfig.Logger.Info("在命令行中调用了MessageBox.Show()", true);
				LogConfig.Logger.Info(stackTrace.ToString(), true);
				result = MessageBoxResult.Yes;
			}
			else
			{
				info = info.Replace("{", "&#123;").Replace("}", "&#125;");
				if (btnText == null)
				{
					btnText = new ButtonText(LanguageInfo.Dialog_ButtonOK, false);
				}
				if (parentWnd == null)
				{
					parentWnd = MessageService.GetDefaultModalParent();
					if (parentWnd == null)
					{
						parentWnd = ApplicationCurrent.MainWindow;
					}
				}
				if (title == null)
				{
					if (image == MessageBoxImage.Error)
					{
						title = LanguageInfo.MessageBox_Error;
					}
					else if (image == MessageBoxImage.Warning)
					{
						title = LanguageInfo.MessageBox_Warning;
					}
					else
					{
						title = LanguageInfo.MessageBox_Notification;
					}
				}
				MessageBoxImage image2;
				if (image == MessageBoxImage.Other)
				{
					if (btnText.ButtonType == MessageBoxButton.Yes)
					{
						image2 = MessageBoxImage.Info;
					}
					else
					{
						image2 = MessageBoxImage.Question;
					}
				}
				else
				{
					image2 = image;
				}
				result = MessageBox.ShowMessageBox(info, btnText, parentWnd, image2, mainBtn, title);
			}
			return result;
		}

		private static MessageBoxResult ShowMessageBox(string info, ButtonText btnText, Window parentWnd, MessageBoxImage image, EnumMainButton mainBtn, string title)
		{
			bool modal = parentWnd.Modal;
			parentWnd.Modal = false;
			CsMessageDialog csMessageDialog = new CsMessageDialog(info, btnText, parentWnd, image, mainBtn, title);
			int num = csMessageDialog.Run();
			csMessageDialog.Destroy();
			parentWnd.Modal = modal;
			ResponseType responseType = (ResponseType)num;
			if (responseType == ResponseType.DeleteEvent)
			{
				num = -6;
			}
			return (MessageBoxResult)num;
		}
	}
}
