using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000009 RID: 9
	public class MenuInfo
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003240 File Offset: 0x00001440
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003257 File Offset: 0x00001457
		internal CommandInfo cmdInfo { get; private set; }

		// Token: 0x06000032 RID: 50 RVA: 0x00003260 File Offset: 0x00001460
		internal MenuInfo(CommandInfo info)
		{
			this.cmdInfo = info;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003273 File Offset: 0x00001473
		internal MenuInfo(Command cmd)
		{
			this.cmdInfo = new CommandInfo(cmd);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000328B File Offset: 0x0000148B
		public MenuInfo()
		{
			this.cmdInfo = new CommandInfo();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000032A2 File Offset: 0x000014A2
		public MenuInfo(string text)
		{
			this.cmdInfo = new CommandInfo(text);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000032BA File Offset: 0x000014BA
		public MenuInfo(string text, bool enabled, bool checkd)
		{
			this.cmdInfo = new CommandInfo(text, enabled, checkd);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000032D4 File Offset: 0x000014D4
		public Command Command
		{
			get
			{
				return this.cmdInfo.Command;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000032F4 File Offset: 0x000014F4
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00003311 File Offset: 0x00001511
		public string Text
		{
			get
			{
				return this.cmdInfo.Text;
			}
			set
			{
				this.cmdInfo.Text = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003324 File Offset: 0x00001524
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00003341 File Offset: 0x00001541
		public IconId Icon
		{
			get
			{
				return this.cmdInfo.Icon;
			}
			set
			{
				this.cmdInfo.Icon = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003354 File Offset: 0x00001554
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00003371 File Offset: 0x00001571
		public string AccelKey
		{
			get
			{
				return this.cmdInfo.AccelKey;
			}
			set
			{
				this.cmdInfo.AccelKey = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003384 File Offset: 0x00001584
		// (set) Token: 0x0600003F RID: 63 RVA: 0x000033A1 File Offset: 0x000015A1
		public string Description
		{
			get
			{
				return this.cmdInfo.Description;
			}
			set
			{
				this.cmdInfo.Description = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000033B4 File Offset: 0x000015B4
		// (set) Token: 0x06000041 RID: 65 RVA: 0x000033D1 File Offset: 0x000015D1
		public bool Enabled
		{
			get
			{
				return this.cmdInfo.Enabled;
			}
			set
			{
				this.cmdInfo.Enabled = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000033E4 File Offset: 0x000015E4
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003401 File Offset: 0x00001601
		public bool Visible
		{
			get
			{
				return this.cmdInfo.Visible;
			}
			set
			{
				this.cmdInfo.Visible = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003414 File Offset: 0x00001614
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003431 File Offset: 0x00001631
		public bool Checked
		{
			get
			{
				return this.cmdInfo.Checked;
			}
			set
			{
				this.cmdInfo.Checked = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003444 File Offset: 0x00001644
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003461 File Offset: 0x00001661
		public bool CheckedInconsistent
		{
			get
			{
				return this.cmdInfo.CheckedInconsistent;
			}
			set
			{
				this.cmdInfo.CheckedInconsistent = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003474 File Offset: 0x00001674
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003491 File Offset: 0x00001691
		public bool UseMarkup
		{
			get
			{
				return this.cmdInfo.UseMarkup;
			}
			set
			{
				this.cmdInfo.UseMarkup = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000034A4 File Offset: 0x000016A4
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000034C1 File Offset: 0x000016C1
		public bool Bypass
		{
			get
			{
				return this.cmdInfo.Bypass;
			}
			set
			{
				this.cmdInfo.Bypass = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000034D4 File Offset: 0x000016D4
		// (set) Token: 0x0600004D RID: 77 RVA: 0x000034F1 File Offset: 0x000016F1
		public CommandArrayInfo ArrayInfo
		{
			get
			{
				return this.cmdInfo.ArrayInfo;
			}
			internal set
			{
				this.cmdInfo.ArrayInfo = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003504 File Offset: 0x00001704
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003521 File Offset: 0x00001721
		public object DataItem
		{
			get
			{
				return this.cmdInfo.DataItem;
			}
			internal set
			{
				this.cmdInfo.DataItem = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003534 File Offset: 0x00001734
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003551 File Offset: 0x00001751
		public bool IsArraySeparator
		{
			get
			{
				return this.cmdInfo.IsArraySeparator;
			}
			internal set
			{
				this.cmdInfo.IsArraySeparator = value;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003564 File Offset: 0x00001764
		public bool HandlesItem(object item)
		{
			return this.cmdInfo.HandlesItem(item);
		}
	}
}
