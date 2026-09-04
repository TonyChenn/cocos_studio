using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.Editor;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000014 RID: 20
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComPropertySurrogate : BaseComSurrogate
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000048F3 File Offset: 0x00002AF3
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000048FB File Offset: 0x00002AFB
		[DataMember]
		public ObservableCollection<CustomPropertyModel> keyvalues { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004904 File Offset: 0x00002B04
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000490C File Offset: 0x00002B0C
		[DataMember]
		public ResourceDataSurrogate fileData { get; protected set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x00004915 File Offset: 0x00002B15
		public ComPropertySurrogate()
		{
			this.classname = "CCComAttribute";
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004928 File Offset: 0x00002B28
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004934 File Offset: 0x00002B34
		protected override object CreateModelObject()
		{
			return new NodeObjectData();
		}
	}
}
