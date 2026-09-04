using System;
using System.Collections.Generic;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200000F RID: 15
	public class RadioGroup
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002DFB File Offset: 0x00000FFB
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002E03 File Offset: 0x00001003
		public string Name { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002E0C File Offset: 0x0000100C
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002E14 File Offset: 0x00001014
		public List<IRadioItem> RadioItemList { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000055 RID: 85 RVA: 0x00002E20 File Offset: 0x00001020
		// (remove) Token: 0x06000056 RID: 86 RVA: 0x00002E58 File Offset: 0x00001058
		public event EventHandler<RadioItemArgs> RadioItemSelected;

		// Token: 0x06000057 RID: 87 RVA: 0x00002E8D File Offset: 0x0000108D
		public RadioGroup(string name = "未命名")
		{
			this.Name = name;
			this.RadioItemList = new List<IRadioItem>();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002EA7 File Offset: 0x000010A7
		public void AddItem(IRadioItem item)
		{
			this.RadioItemList.Add(item);
			item.Selected += this.OnItemSelected;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002EC8 File Offset: 0x000010C8
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
