using System;
using System.Collections;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.ExtensionModel
{
	public class MenuArrayInfo : IEnumerable
	{
		internal MenuArrayInfo(CommandArrayInfo info)
		{
			this.cmdArrayInfo = info;
		}

		internal MenuArrayInfo(CommandInfo cmdInfo)
		{
			this.cmdArrayInfo = new CommandArrayInfo(cmdInfo);
		}

		public void Clear()
		{
			this.cmdArrayInfo.Clear();
		}

		public MenuInfo FindCommandInfo(object dataItem)
		{
			return new MenuInfo(this.cmdArrayInfo.FindCommandInfo(dataItem));
		}

		public void Insert(int index, MenuInfo info, object dataItem)
		{
			this.cmdArrayInfo.Insert(index, info.cmdInfo, dataItem);
		}

		public void Add(MenuInfo info, object dataItem)
		{
			this.cmdArrayInfo.Add(info.cmdInfo, dataItem);
		}

		public MenuInfo this[int n]
		{
			get
			{
				return new MenuInfo(this.cmdArrayInfo[n]);
			}
		}

		public int Count
		{
			get
			{
				return this.cmdArrayInfo.Count;
			}
		}

		public void AddSeparator()
		{
			this.cmdArrayInfo.AddSeparator();
		}

		public MenuInfo DefaultCommandInfo
		{
			get
			{
				return new MenuInfo(this.cmdArrayInfo.DefaultCommandInfo);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return this.cmdArrayInfo.GetEnumerator();
		}

		public bool Bypass
		{
			get
			{
				return this.cmdArrayInfo.Bypass;
			}
			set
			{
				this.cmdArrayInfo.Bypass = value;
			}
		}

		private CommandArrayInfo cmdArrayInfo;
	}
}
