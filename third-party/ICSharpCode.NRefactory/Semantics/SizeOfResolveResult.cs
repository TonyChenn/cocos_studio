using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the 'sizeof'.
	/// </summary>
	public class SizeOfResolveResult : ResolveResult
	{
		public SizeOfResolveResult(IType int32, IType referencedType, int? constantValue) : base(int32)
		{
			if (referencedType == null)
			{
				throw new ArgumentNullException("referencedType");
			}
			this.referencedType = referencedType;
			this.constantValue = constantValue;
		}

		/// <summary>
		/// The type referenced by the 'sizeof'.
		/// </summary>
		public IType ReferencedType
		{
			get
			{
				return this.referencedType;
			}
		}

		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.constantValue != null;
			}
		}

		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		public override bool IsError
		{
			get
			{
				return this.referencedType.IsReferenceType != false;
			}
		}

		private readonly IType referencedType;

		private readonly int? constantValue;
	}
}
