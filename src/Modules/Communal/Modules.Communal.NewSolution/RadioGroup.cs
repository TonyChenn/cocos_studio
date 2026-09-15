using System;
using System.Collections.Generic;

namespace Modules.Communal.NewSolution
{
	public class RadioGroup
	{
		public string Name { get; private set; }

		public List<IRadioItem> RadioItemList { get; private set; }

		public event EventHandler<RadioItemArgs> RadioItemSelected;

		public RadioGroup(string name = "未命名")
		{
			this.Name = name;
			this.RadioItemList = new List<IRadioItem>();
		}

		public void AddItem(IRadioItem item)
		{
			this.RadioItemList.Add(item);
			item.Selected += this.OnItemSelected;
		}

		private void OnItemSelected(object sender, RadioItemArgs e)
		{
			foreach (IRadioItem radioItem in this.RadioItemList)
			{
				if (radioItem != sender)
				{
					radioItem.Unselect();
				}
			}
			if (this.RadioItemSelected != null)
			{
				this.RadioItemSelected(this, e);
			}
		}
	}
}
