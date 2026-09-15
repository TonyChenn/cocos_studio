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
	[ToolboxItem(true)]
	public class SerializerWidget : Bin
	{
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

		public bool IsFitDefaultSerializer { get; private set; }

		public event EventHandler Selected;

		public IGameFileSerializer Serializer
		{
			get
			{
				return this.currentSerializer;
			}
		}

		public string SolutionLink
		{
			get
			{
				return this.currentSerializer.SolutionLink;
			}
		}

		public SerializerWidget()
		{
			throw new Exception("请使用有参构造");
		}

		public SerializerWidget(BaseCocosFileSerializer serializer, SList group)
		{
			this.Build();
			this.currentSerializer = serializer;
			this.radiobutton.Group = group;
			this.radiobutton.Label = serializer.Label;
			this.label_des.Text = serializer.Description;
		}

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

		protected void HandleVBoxMainSizeAllocated(object o, SizeAllocatedArgs args)
		{
			int widthRequest = args.Allocation.Width - 25;
			this.label_des.WidthRequest = widthRequest;
			this.label_des.LineWrapMode = Pango.WrapMode.WordChar;
			this.label_des.Wrap = true;
		}

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

		private BaseCocosFileSerializer currentSerializer;

		private VBox vbox_main;

		private RadioButton radiobutton;

		private VBox vbox_des;

		private HBox hbox_des;

		private Gtk.Alignment alignment_des;

		private Label label_des;
	}
}
