using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.UI.ComTool.Model;
using Modules.UI.ComTool.View;
using Stetic;

namespace Modules.UI.ComTool
{
	public class ComponentPanel : EventBox, IComToolPad, IService
	{
		public ComponentPanel()
		{
			this.Build();
			this.comRootVbox = new VBox();
			this.comRootVbox.Spacing = 6;
			this.hbx_root.Add(this.comRootVbox);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbx_root[this.comRootVbox];
			boxChild.Position = 1;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.comRootVbox.Show();
			Option.UserConfig.PropertyChanged += this.UserConfigChanged;
			base.SizeAllocated += this.HandleSizeAllocated;
			base.Shown += this.ComponentPanel_Shown;
			this.viewModel = new ComToolUCViewModel();
			this.InitPanel();
			Services.RegisterService<IComToolPad>(this);
		}

		private void RefreshControlsView()
		{
			foreach (KeyValuePair<string, CustomExpender> keyValuePair in this.groupDict)
			{
				if (this.viewModel.ViewFilter.CanShowCategory(keyValuePair.Key))
				{
					keyValuePair.Value.Show();
				}
				else
				{
					keyValuePair.Value.Hide();
				}
			}
			foreach (KeyValuePair<string, ComponentItem> keyValuePair2 in this.itemDict)
			{
				if (this.viewModel.ViewFilter.CanShowItem(keyValuePair2.Key))
				{
					keyValuePair2.Value.Show();
				}
				else
				{
					keyValuePair2.Value.Hide();
				}
			}
		}

		private void ComponentPanel_Shown(object sender, EventArgs e)
		{
			this.SetDeprecatedControl(true);
			this.RefreshControlsView();
		}

		private void UserConfigChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsShowDeprecatedControl")
			{
				this.SetDeprecatedControl(true);
			}
		}

		private void SetDeprecatedControl(bool isQueueResize = true)
		{
		}

		private void HandleSizeAllocated(object o, SizeAllocatedArgs args)
		{
			this.AutoSize();
		}

		public void AutoSize()
		{
			if (this.comRootVbox != null)
			{
				int width = base.Allocation.Width;
				int comSize = ComponentItem.ComSize;
				int num = (width - 10) / comSize;
				num = ((num == 0) ? 1 : num);
				foreach (object obj in this.comRootVbox)
				{
					CustomExpender customExpender = obj as CustomExpender;
					if (customExpender != null)
					{
						Alignment alignment = customExpender.Child as Alignment;
						Table table = alignment.Child as Table;
						if (table != null)
						{
							int num2 = table.Children.Count<Widget>();
							int num3 = 0;
							if (num != 0)
							{
								num3 = num2 % num;
							}
							int num4 = num2 / num;
							if (num3 != 0)
							{
								num4++;
							}
							this.AutoLayoutTable(table, num4, num);
						}
					}
				}
			}
		}

		public void AutoLayoutTable(Table table, int rows, int cloums)
		{
			table.NRows = (uint)rows;
			table.NColumns = (uint)cloums;
			int num = 0;
			int num2 = 0;
			foreach (object obj in table)
			{
				Widget w = (Widget)obj;
				Table.TableChild tableChild = (Table.TableChild)table[w];
				tableChild.TopAttach = (uint)num;
				tableChild.BottomAttach = (uint)(num + 1);
				tableChild.LeftAttach = (uint)num2;
				tableChild.RightAttach = (uint)(num2 + 1);
				num2++;
				if (num2 > cloums - 1)
				{
					num2 = 0;
					num++;
				}
				tableChild.XOptions = (AttachOptions)0;
				tableChild.YOptions = (AttachOptions)0;
			}
		}

		private void InitPanel()
		{
			if (this.viewModel.UIList != null)
			{
				var enumerable = from s in this.viewModel.UIList
				group s by s.ModelType into g
				select new
				{
					Key = g.Key,
					Value = g
				};
				foreach (var modelTypeGroup in enumerable)
				{
					var enumerable2 = from s in modelTypeGroup.Value
					group s by s.Group into g
					select new
					{
						Key = g.Key,
						Value = g
					};
					int num = enumerable2.Count();
					int num2 = 0;
					enumerable2 = from n in enumerable2
					orderby n.Key.Order
					select n;
					foreach (var controlGroup in enumerable2)
					{
						string valueBykey = LanguageOption.GetValueBykey(controlGroup.Key.GroupName);
						CustomExpender customExpender = new CustomExpender(valueBykey);
						if (!this.groupDict.ContainsKey(controlGroup.Key.GroupName))
						{
							this.groupDict.Add(controlGroup.Key.GroupName, customExpender);
						}
						customExpender.Expanded = true;
						Label label = new Label();
						label.LabelProp = valueBykey;
						customExpender.LabelWidget = label;
						label.Show();
						this.comRootVbox.Add(customExpender);
						Box.BoxChild boxChild = (Box.BoxChild)this.comRootVbox[customExpender];
						boxChild.Expand = false;
						boxChild.Fill = false;
						Table table = new Table((uint)controlGroup.Value.Count<ControlToolItem>(), 1U, false);
						table.RowSpacing = (uint)this.rowSpacing;
						table.ColumnSpacing = (uint)this.columnSpacing;
						List<ControlToolItem> list = controlGroup.Value.ToList<ControlToolItem>();
						for (int i = list.Count<ControlToolItem>() - 1; i >= 0; i--)
						{
							ControlToolItem controlToolItem = list[i];
							ComponentItem componentItem = new ComponentItem();
							componentItem.InitiCom(controlToolItem);
							table.Add(componentItem);
							string fullName = controlToolItem.ModelMedaData.Type.FullName;
							if (!this.itemDict.ContainsKey(fullName))
							{
								this.itemDict.Add(fullName, componentItem);
							}
							Table.TableChild tableChild = (Table.TableChild)table[componentItem];
							tableChild.XOptions = (AttachOptions)0;
							tableChild.YOptions = (AttachOptions)0;
							componentItem.Show();
						}
						Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
						alignment.SetPadding(10U, 0U, 0U, 0U);
						alignment.Add(table);
						customExpender.Add(alignment);
						customExpender.ShowAll();
						num2++;
					}
				}
			}
		}

		public IControlsViewFilter ControlsViewFilter
		{
			get
			{
				return this.viewModel.ViewFilter;
			}
			set
			{
				if (value != this.viewModel.ViewFilter)
				{
					if (value == null)
					{
						this.viewModel.ViewFilter = DefaultControlsViewFilter.DefaultFilterInstace;
					}
					else
					{
						this.viewModel.ViewFilter = value;
					}
					this.RefreshControlsView();
				}
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.UI.ComTool.ComponentPanel";
			this.scrolledwindow1 = new ScrolledWindow();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ShadowType.In;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			this.evtbx_root = new EventBox();
			this.evtbx_root.Name = "evtbx_root";
			this.hbx_root = new HBox();
			this.hbx_root.Name = "hbx_root";
			this.hbx_root.Spacing = 6;
			this.evtbx_root.Add(this.hbx_root);
			viewport.Add(this.evtbx_root);
			this.scrolledwindow1.Add(viewport);
			base.Add(this.scrolledwindow1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private VBox comRootVbox;

		private ComToolUCViewModel viewModel = null;

		private int columnSpacing = 0;

		private int rowSpacing = 0;

		private Dictionary<string, CustomExpender> groupDict = new Dictionary<string, CustomExpender>();

		private Dictionary<string, ComponentItem> itemDict = new Dictionary<string, ComponentItem>();

		private ScrolledWindow scrolledwindow1;

		private EventBox evtbx_root;

		private HBox hbx_root;
	}
}
