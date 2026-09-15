using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class WidgetTreeSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public WidgetSurrogate options { get; set; }

		[DataMember]
		public List<WidgetTreeSurrogate> children { get; set; }

		protected WidgetTreeSurrogate()
		{
		}

		public WidgetTreeSurrogate(WidgetSurrogate guiControlSurrogate)
		{
			this.classname = guiControlSurrogate.classname;
			this.options = guiControlSurrogate;
			this.ConvertChildren();
		}

		private void ConvertChildren()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			this.options.ConvertToObject();
		}
	}
}
