using System;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000004 RID: 4
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class BaseEntitySurrogate : ObjectSurrogate
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00003AF0 File Offset: 0x00001CF0
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00003AF8 File Offset: 0x00001CF8
		[DataMember]
		public virtual string name { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00003B01 File Offset: 0x00001D01
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00003B09 File Offset: 0x00001D09
		[DataMember]
		public virtual string classname { get; protected set; }

		// Token: 0x06000025 RID: 37 RVA: 0x00003B12 File Offset: 0x00001D12
		protected BaseEntitySurrogate()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003B1C File Offset: 0x00001D1C
		public virtual object ConvertToObject()
		{
			object obj = this.CreateModelObject();
			this.SetValue(obj);
			return obj;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003B38 File Offset: 0x00001D38
		public virtual void SetValue(object obj)
		{
			AbstractNodeObjectData abstractNodeObjectData = obj as AbstractNodeObjectData;
			if (this.name == null)
			{
				this.name = "UiEntity" + this.GetHashCode();
			}
			if (abstractNodeObjectData != null)
			{
				abstractNodeObjectData.Name = this.name;
			}
			abstractNodeObjectData.Visible = true;
			abstractNodeObjectData.CanEdit = true;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003B8C File Offset: 0x00001D8C
		protected virtual object CreateModelObject()
		{
			return new NodeObjectData();
		}
	}
}
