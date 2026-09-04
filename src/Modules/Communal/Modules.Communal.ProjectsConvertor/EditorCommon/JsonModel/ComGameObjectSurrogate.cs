using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.JsonModel.Component;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x0200000C RID: 12
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGameObjectSurrogate : BaseGameObjectSurrogate
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000044EE File Offset: 0x000026EE
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000044F6 File Offset: 0x000026F6
		[DataMember(Order = 50)]
		public List<BaseComSurrogate> components { get; private set; }

		// Token: 0x06000082 RID: 130 RVA: 0x000044FF File Offset: 0x000026FF
		protected ComGameObjectSurrogate()
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004508 File Offset: 0x00002708
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			NodeObjectData nodeObjectData = obj as NodeObjectData;
			if (this.components != null && this.components.Count > 0 && nodeObjectData.Children == null)
			{
				nodeObjectData.Children = new List<AbstractNodeObjectData>();
			}
			foreach (BaseComSurrogate baseComSurrogate in this.components)
			{
				NodeObjectData item = (NodeObjectData)baseComSurrogate.ConvertToObject();
				nodeObjectData.Children.Add(item);
			}
		}
	}
}
