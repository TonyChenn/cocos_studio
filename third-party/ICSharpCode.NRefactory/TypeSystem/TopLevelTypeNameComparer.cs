using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Serializable]
	public sealed class TopLevelTypeNameComparer : IEqualityComparer<TopLevelTypeName>
	{
		public TopLevelTypeNameComparer(StringComparer nameComparer)
		{
			this.NameComparer = nameComparer;
		}

		public bool Equals(TopLevelTypeName x, TopLevelTypeName y)
		{
			return x.TypeParameterCount == y.TypeParameterCount && this.NameComparer.Equals(x.Name, y.Name) && this.NameComparer.Equals(x.Namespace, y.Namespace);
		}

		public int GetHashCode(TopLevelTypeName obj)
		{
			return this.NameComparer.GetHashCode(obj.Name) ^ this.NameComparer.GetHashCode(obj.Namespace) ^ obj.TypeParameterCount;
		}

		public static readonly TopLevelTypeNameComparer Ordinal = new TopLevelTypeNameComparer(StringComparer.Ordinal);

		public static readonly TopLevelTypeNameComparer OrdinalIgnoreCase = new TopLevelTypeNameComparer(StringComparer.OrdinalIgnoreCase);

		public readonly StringComparer NameComparer;
	}
}
