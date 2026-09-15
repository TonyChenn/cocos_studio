using System;
using Mono.Addins;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ModelExtensionAttribute : CustomExtensionAttribute
	{
		[NodeAttribute]
		public int Order { get; private set; }

		[NodeAttribute]
		public bool IsDefault { get; private set; }

		[NodeAttribute]
		public EnumModelType ModelType { get; private set; }

		public Type MetaDataType { get; private set; }

		public ModelExtensionAttribute()
		{
		}

		public ModelExtensionAttribute([NodeAttribute("Order")] int order) : this(false, order)
		{
		}

		public ModelExtensionAttribute([NodeAttribute("Order")] int order, Type metaDataType) : this(false, order, metaDataType)
		{
		}

		public ModelExtensionAttribute(int order, EnumModelType modelType) : this(false, order, modelType)
		{
		}

		internal ModelExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault, [NodeAttribute("Order")] int order)
		{
			this.Order = order;
			this.IsDefault = isDefault;
		}

		internal ModelExtensionAttribute(bool isDefault, int order, Type metaDataType) : this(isDefault, order)
		{
			this.MetaDataType = metaDataType;
		}

		internal ModelExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault, [NodeAttribute("Order")] int order, [NodeAttribute("ModelType")] EnumModelType modelType) : this(isDefault, order)
		{
			this.ModelType = modelType;
		}
	}
}
