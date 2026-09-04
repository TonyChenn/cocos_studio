using System;
using System.Collections.Generic;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000025 RID: 37
	public class Light3DHelper
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006079 File Offset: 0x00004279
		public static Light3DHelper Instance
		{
			get
			{
				if (Light3DHelper._lightHelper == null)
				{
					Light3DHelper._lightHelper = new Light3DHelper();
				}
				return Light3DHelper._lightHelper;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006091 File Offset: 0x00004291
		private Light3DHelper()
		{
			this.LightList = new List<Light3DObject>();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000060A4 File Offset: 0x000042A4
		~Light3DHelper()
		{
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000060CC File Offset: 0x000042CC
		// (set) Token: 0x0600017A RID: 378 RVA: 0x000060D4 File Offset: 0x000042D4
		public bool Enbaled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (this._enabled == value)
				{
					return;
				}
				this._enabled = value;
				if (this._enabled)
				{
					this.RefreshAllLightsIndex();
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000060F5 File Offset: 0x000042F5
		// (set) Token: 0x0600017C RID: 380 RVA: 0x000060FD File Offset: 0x000042FD
		private List<Light3DObject> LightList { get; set; }

		// Token: 0x0600017D RID: 381 RVA: 0x00006106 File Offset: 0x00004306
		public void Clear()
		{
			this.LightList.Clear();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006113 File Offset: 0x00004313
		public void AddLight(Light3DObject light)
		{
			if (GameFileLoader.IsLoading)
			{
				return;
			}
			this.LightList.Add(light);
			this.RefreshAllLightsIndex();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000612F File Offset: 0x0000432F
		public void RemoveLight(Light3DObject light)
		{
			this.LightList.Remove(light);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006140 File Offset: 0x00004340
		public void RefreshAllLightsIndex()
		{
			if (!this._enabled)
			{
				return;
			}
			foreach (Light3DObject light in this.LightList)
			{
				this.RefreshLightIndex(light);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000619C File Offset: 0x0000439C
		private void RefreshLightIndex(Light3DObject light)
		{
			int globalIndex = this.GetGlobalIndex(light);
			light.RefreshLightIndex(globalIndex);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000061B8 File Offset: 0x000043B8
		private int GetGlobalIndex(AbstractNodeObject vObject)
		{
			if (vObject == null || vObject is GameNode3DObject)
			{
				return 0;
			}
			int orderOfArrival = vObject.OrderOfArrival;
			return orderOfArrival + this.GetGlobalIndex(vObject.Parent);
		}

		// Token: 0x04000087 RID: 135
		private static Light3DHelper _lightHelper;

		// Token: 0x04000088 RID: 136
		private bool _enabled;
	}
}
