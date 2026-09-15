using System;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class BaseEntitySurrogate : ObjectSurrogate
	{
		[DataMember]
		public virtual string name { get; protected set; }

		[DataMember]
		public virtual string classname { get; protected set; }

		protected BaseEntitySurrogate()
		{
		}

		public virtual object ConvertToObject()
		{
			object obj = this.CreateModelObject();
			this.SetValue(obj);
			return obj;
		}

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

		protected virtual object CreateModelObject()
		{
			return new NodeObjectData();
		}
	}
}
