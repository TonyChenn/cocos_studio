using System;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.Projects;
using GLib;
using Gtk;
using Modules.Communal.CocosAdapter;
using Mono.Unix;
using Pango;
using Stetic;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x0200000B RID: 11
	[ToolboxItem(true)]
	public class SerializerWidget : Bin
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000043A8 File Offset: 0x000025A8
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000043B5 File Offset: 0x000025B5
		public bool IsSelected
		{
			get
			{
				return this.radiobutton.Active;
			}
			set
			{
				this.radiobutton.Active = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000043C3 File Offset: 0x000025C3
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000043CB File Offset: 0x000025CB
		public bool IsFitDefaultSerializer { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003B RID: 59 RVA: 0x000043D4 File Offset: 0x000025D4
		// (remove) Token: 0x0600003C RID: 60 RVA: 0x0000440C File Offset: 0x0000260C
		public event EventHandler Selected;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00004441 File Offset: 0x00002641
		public IGameFileSerializer Serializer
		{
			get
			{
				return this.currentSerializer;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00004449 File Offset: 0x00002649
		public string SolutionLink
		{
			get
			{
				return this.currentSerializer.SolutionLink;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00004456 File Offset: 0x00002656
		public SerializerWidget()
		{
			throw new Exception("请使用有参构造");
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004468 File Offset: 0x00002668
		public SerializerWidget(BaseCocosFileSerializer serializer, SList group)
		{
			this.Build();
			this.currentSerializer = serializer;
			this.radiobutton.Group = group;
			this.radiobutton.Label = serializer.Label;
			this.label_des.Text = serializer.Description;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000044B8 File Offset: 0x000026B8
		protected void HandleRadioButtonToggled(object sender, EventArgs e)
		{
			if (this.radiobutton.Active)
			{
				if (this.currentSerializer.ID.Equals(Services.ProjectsService.CurrentSolution.Config.DefaultSerializer))
				{
					this.IsFitDefaultSerializer = true;
				}
				else if (this.currentSerializer.ID.Equals("Serializer_Lua") && Cocos2dxServices.CocosProperties.ProgramLanguage == EnumProgramLanguage.lua)
				{
					this.IsFitDefaultSerializer = true;
				}
				else
				{
					this.IsFitDefaultSerializer = false;
				}
				if (this.Selected != null)
				{
					this.Selected(this, new EventArgs());
					return;
				}
			}
			else
			{
				this.IsFitDefaultSerializer = true;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004558 File Offset: 0x00002758
		protected void HandleVBoxMainSizeAllocated(object o, SizeAllocatedArgs args)
		{
			int widthRequest = args.Allocation.Width - 25;
			this.label_des.WidthRequest = widthRequest;
			this.label_des.LineWrapMode = Pango.WrapMode.WordChar;
			this.label_des.Wrap = true;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004598 File Offset: 0x00002798
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.ProjectSetting.SerializerWidget";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 4;
			this.radiobutton = new RadioButton(Catalog.GetString("序列化器"));
			this.radiobutton.CanFocus = true;
			this.radiobutton.Name = "radiobutton";
			this.radiobutton.Active = true;
			this.radiobutton.DrawIndicator = true;
			this.radiobutton.UseUnderline = true;
			this.radiobutton.Group = new SList(IntPtr.Zero);
			this.vbox_main.Add(this.radiobutton);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.radiobutton];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.vbox_des = new VBox();
			this.vbox_des.Name = "vbox_des";
			this.vbox_des.Spacing = 6;
			this.hbox_des = new HBox();
			this.hbox_des.Name = "hbox_des";
			this.hbox_des.Spacing = 6;
			this.alignment_des = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_des.Name = "alignment_des";
			this.alignment_des.LeftPadding = 20U;
			this.alignment_des.BottomPadding = 5U;
			this.label_des = new Label();
			this.label_des.Name = "label_des";
			this.label_des.Xalign = 0f;
			this.label_des.LabelProp = Catalog.GetString("序列化器说明文本");
			this.alignment_des.Add(this.label_des);
			this.hbox_des.Add(this.alignment_des);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_des[this.alignment_des];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.vbox_des.Add(this.hbox_des);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_des[this.hbox_des];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.vbox_main.Add(this.vbox_des);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.vbox_main[this.vbox_des];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			base.Add(this.vbox_main);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.vbox_main.SizeAllocated += this.HandleVBoxMainSizeAllocated;
			this.radiobutton.Toggled += this.HandleRadioButtonToggled;
		}

		// Token: 0x0400003F RID: 63
		private BaseCocosFileSerializer currentSerializer;

		// Token: 0x04000041 RID: 65
		private VBox vbox_main;

		// Token: 0x04000042 RID: 66
		private RadioButton radiobutton;

		// Token: 0x04000043 RID: 67
		private VBox vbox_des;

		// Token: 0x04000044 RID: 68
		private HBox hbox_des;

		// Token: 0x04000045 RID: 69
		private Gtk.Alignment alignment_des;

		// Token: 0x04000046 RID: 70
		private Label label_des;
	}
}
