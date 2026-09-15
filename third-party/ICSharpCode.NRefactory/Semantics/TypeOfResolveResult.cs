using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the 'typeof'.
	/// </summary>
	public class TypeOfResolveResult : ResolveResult
	{
		public TypeOfResolveResult(IType systemType, IType referencedType) : base(systemType)
		{
			if (referencedType == null)
			{
				throw new ArgumentNullException("referencedType");
			}
			this.referencedType = referencedType;
		}

		/// <summary>
		/// The type referenced by the 'typeof'.
		/// </summary>
		public IType ReferencedType
		{
			get
			{
				return this.referencedType;
			}
		}

		private readonly IType referencedType;
	}
}
