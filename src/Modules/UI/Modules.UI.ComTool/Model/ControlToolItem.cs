using System;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.UI.ComTool.Model
{
	public class ControlToolItem
	{
		public ControlGroupAttribute Group { get; set; }

		public string DisplayName { get; private set; }

		public Image BitImage { get; private set; }

		public ModelMetaData ModelMedaData { get; private set; }

		public EnumModelType ModelType
		{
			get
			{
				return this.ModelMedaData.ModelType;
			}
		}

		public ControlToolItem(ModelMetaData uiMedaData, Image bitImage)
		{
			this.DisplayName = LanguageOption.GetValueBykey(uiMedaData.DisplayName);
			this.ModelMedaData = uiMedaData;
			this.BitImage = bitImage;
			this.Group = new ControlGroupAttribute("Control_Other", 0);
		}
	}
}
