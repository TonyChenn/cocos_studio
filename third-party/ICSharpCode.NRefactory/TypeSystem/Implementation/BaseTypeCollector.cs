using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Helper class for the GetAllBaseTypes() implementation.
	/// </summary>
	// Token: 0x020000A7 RID: 167
	internal sealed class BaseTypeCollector : List<IType>
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x0000D1D0 File Offset: 0x0000C1D0
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

		// Token: 0x04000180 RID: 384
		private readonly Stack<IType> activeTypes = new Stack<IType>();

		/// <summary>
		/// If this option is enabled, the list will not contain interfaces when retrieving the base types
		/// of a class.
		/// </summary>
		// Token: 0x04000181 RID: 385
		internal bool SkipImplementedInterfaces;
	}
}
