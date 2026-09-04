using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	[DataInclude(typeof(TypeReference))]
	public class TypeToolboxNode : ItemToolboxNode
	{
		[ItemProperty("type")]
		private TypeReference type;

		public TypeReference Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		public TypeToolboxNode()
		{
		}

		public TypeToolboxNode(TypeReference typeRef)
		{
			type = typeRef;
		}

		public TypeToolboxNode(string typeName, string assemblyName)
			: this(typeName, assemblyName, string.Empty)
		{
		}

		public TypeToolboxNode(string typeName, string assemblyName, string assemblyLocation)
		{
			type = new TypeReference(typeName, assemblyName, assemblyLocation);
		}

		public TypeToolboxNode(Type type)
		{
			this.type = new TypeReference(type);
		}

		public override bool Equals(object o)
		{
			if (o is TypeToolboxNode typeToolboxNode && ((type == null) ? (typeToolboxNode.type == null) : type.Equals(typeToolboxNode.type)))
			{
				return base.Equals((object)typeToolboxNode);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (type != null)
			{
				num ^= type.GetHashCode();
			}
			return num;
		}
	}
}
