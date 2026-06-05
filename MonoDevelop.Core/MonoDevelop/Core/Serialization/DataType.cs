using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200005F RID: 95
	public abstract class DataType
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0000B5D0 File Offset: 0x000097D0
		public DataType(Type type)
		{
			this.type = type;
			this.name = this.GetTypeName(type);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000B5EC File Offset: 0x000097EC
		private string GetTypeName(Type type)
		{
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments != null && genericArguments.Length > 0)
			{
				string text = type.Name;
				text = text.Substring(0, text.IndexOf('`'));
				text += "Of";
				foreach (Type type2 in genericArguments)
				{
					text += this.GetTypeName(type2);
				}
				return text;
			}
			return type.Name;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000B65B File Offset: 0x0000985B
		internal void SetContext(DataContext ctx)
		{
			this.ctx = ctx;
			this.Initialize();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000B66C File Offset: 0x0000986C
		protected ItemPropertyAttribute FindPropertyAttribute(object[] attributes, string scope)
		{
			scope = scope.TrimStart(new char[]
			{
				'/'
			});
			foreach (object obj in attributes)
			{
				ItemPropertyAttribute itemPropertyAttribute = obj as ItemPropertyAttribute;
				if (itemPropertyAttribute != null && itemPropertyAttribute.Scope == scope)
				{
					return itemPropertyAttribute;
				}
			}
			return null;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000B6C9 File Offset: 0x000098C9
		protected DataContext Context
		{
			get
			{
				return this.ctx;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000B6D1 File Offset: 0x000098D1
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000B6D9 File Offset: 0x000098D9
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000B6E2 File Offset: 0x000098E2
		public Type ValueType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000B6EA File Offset: 0x000098EA
		protected virtual void Initialize()
		{
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B6EC File Offset: 0x000098EC
		protected internal virtual object GetMapData(object[] attributes, string scope)
		{
			return null;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000B6EF File Offset: 0x000098EF
		public DataNode Serialize(SerializationContext serCtx, object mapData, object value)
		{
			return serCtx.Serializer.OnSerialize(this, serCtx, mapData, value);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000B700 File Offset: 0x00009900
		public object Deserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			return serCtx.Serializer.OnDeserialize(this, serCtx, mapData, data);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000B711 File Offset: 0x00009911
		public void Deserialize(SerializationContext serCtx, object mapData, DataNode data, object valueInstance)
		{
			serCtx.Serializer.OnDeserialize(this, serCtx, mapData, data, valueInstance);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B724 File Offset: 0x00009924
		public object CreateInstance(SerializationContext serCtx, DataNode data)
		{
			return serCtx.Serializer.OnCreateInstance(this, serCtx, data);
		}

		// Token: 0x0600031D RID: 797
		protected internal abstract DataNode OnSerialize(SerializationContext serCtx, object mapData, object value);

		// Token: 0x0600031E RID: 798
		protected internal abstract object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data);

		// Token: 0x0600031F RID: 799 RVA: 0x0000B734 File Offset: 0x00009934
		protected internal virtual void OnDeserialize(SerializationContext serCtx, object mapData, DataNode data, object valueInstance)
		{
			throw new InvalidOperationException("Could not create instance for type '" + this.ValueType + "'");
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000B750 File Offset: 0x00009950
		protected internal virtual object OnCreateInstance(SerializationContext serCtx, DataNode data)
		{
			throw new InvalidOperationException("Could not create instance for type '" + this.ValueType + "'");
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000321 RID: 801
		public abstract bool IsSimpleType { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000322 RID: 802
		public abstract bool CanCreateInstance { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000323 RID: 803
		public abstract bool CanReuseInstance { get; }

		// Token: 0x04000118 RID: 280
		private Type type;

		// Token: 0x04000119 RID: 281
		private DataContext ctx;

		// Token: 0x0400011A RID: 282
		private string name;
	}
}
