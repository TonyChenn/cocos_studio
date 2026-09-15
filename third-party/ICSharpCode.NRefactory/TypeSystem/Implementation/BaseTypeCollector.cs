using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Helper class for the GetAllBaseTypes() implementation.
	/// </summary>
	internal sealed class BaseTypeCollector : List<IType>
	{
		public void CollectBaseTypes(IType type)
		{
			IType type2 = type.GetDefinition() ?? type;
			if (this.activeTypes.Contains(type2))
			{
				return;
			}
			this.activeTypes.Push(type2);
			if (!base.Contains(type))
			{
				foreach (IType type3 in type.DirectBaseTypes)
				{
					if (!this.SkipImplementedInterfaces || type2 == null || type2.Kind == TypeKind.Interface || type2.Kind == TypeKind.TypeParameter || type3.Kind != TypeKind.Interface)
					{
						this.CollectBaseTypes(type3);
					}
				}
				base.Add(type);
			}
			this.activeTypes.Pop();
		}

		private readonly Stack<IType> activeTypes = new Stack<IType>();

		/// <summary>
		/// If this option is enabled, the list will not contain interfaces when retrieving the base types
		/// of a class.
		/// </summary>
		internal bool SkipImplementedInterfaces;
	}
}
