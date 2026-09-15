using System;
using System.Collections.Generic;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.ViewModel
{
	public class Light3DHelper
	{
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

		private Light3DHelper()
		{
			this.LightList = new List<Light3DObject>();
		}

		~Light3DHelper()
		{
		}

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

		private List<Light3DObject> LightList { get; set; }

		public void Clear()
		{
			this.LightList.Clear();
		}

		public void AddLight(Light3DObject light)
		{
			if (GameFileLoader.IsLoading)
			{
				return;
			}
			this.LightList.Add(light);
			this.RefreshAllLightsIndex();
		}

		public void RemoveLight(Light3DObject light)
		{
			this.LightList.Remove(light);
		}

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

		private void RefreshLightIndex(Light3DObject light)
		{
			int globalIndex = this.GetGlobalIndex(light);
			light.RefreshLightIndex(globalIndex);
		}

		private int GetGlobalIndex(AbstractNodeObject vObject)
		{
			if (vObject == null || vObject is GameNode3DObject)
			{
				return 0;
			}
			int orderOfArrival = vObject.OrderOfArrival;
			return orderOfArrival + this.GetGlobalIndex(vObject.Parent);
		}

		private static Light3DHelper _lightHelper;

		private bool _enabled;
	}
}
