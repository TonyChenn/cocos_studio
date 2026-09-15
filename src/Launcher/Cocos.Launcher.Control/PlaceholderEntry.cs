using System;
using System.ComponentModel;
using Gtk;

namespace Cocos.Launcher.Control
{
	[ToolboxItem(true)]
	public class PlaceholderEntry : Entry
	{
		public string PlaceholderText
		{
			get
			{
				return this.placeholderText;
			}
			set
			{
				this.placeholderText = value;
				base.Text = value;
			}
		}

		public bool IsPlaceholder
		{
			get
			{
				return this.isPlaceholder;
			}
			set
			{
				this.isPlaceholder = value;
				if (value)
				{
					base.ModifyText(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
					base.Visibility = true;
					return;
				}
				base.ModifyText(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
				if (this.isPassword)
				{
					base.Visibility = false;
				}
			}
		}

		public bool IsPassword
		{
			get
			{
				return this.isPassword;
			}
			set
			{
				this.isPassword = value;
				base.InvisibleChar = '*';
			}
		}

		public PlaceholderEntry()
		{
			base.ActivatesDefault = true;
			base.FocusInEvent += this.PlaceholderEntry_FocusInEvent;
			base.FocusOutEvent += this.PlaceholderEntry_FocusOutEvent;
		}

		private void PlaceholderEntry_FocusInEvent(object o, FocusInEventArgs args)
		{
			if (this.isPlaceholder)
			{
				base.Text = string.Empty;
				this.IsPlaceholder = false;
			}
		}

		private void PlaceholderEntry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (base.Text.Trim() == string.Empty)
			{
				base.Text = this.PlaceholderText;
				this.IsPlaceholder = true;
				return;
			}
			this.IsPlaceholder = false;
		}

		protected override void OnClipboardCopied()
		{
			if (!this.isPassword)
			{
				base.OnClipboardCopied();
			}
		}

		protected override void OnClipboardCut()
		{
			if (!this.isPassword)
			{
				base.OnClipboardCut();
			}
		}

		protected override void OnClipboardPasted()
		{
			if (!this.isPassword)
			{
				base.OnClipboardPasted();
			}
		}

		private string placeholderText = string.Empty;

		private bool isPlaceholder;

		private bool isPassword;
	}
}
