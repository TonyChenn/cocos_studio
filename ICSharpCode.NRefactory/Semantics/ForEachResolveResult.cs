using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Resolve result representing a 'foreach' loop.
	/// </summary>
	// Token: 0x02000045 RID: 69
	public class ForEachResolveResult : ResolveResult
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000666C File Offset: 0x0000566C
		public ForEachResolveResult(ResolveResult getEnumeratorCall, IType collectionType, IType enumeratorType, IType elementType, IVariable elementVariable, IProperty currentProperty, IMethod moveNextMethod, IType voidType) : base(voidType)
		{
			if (getEnumeratorCall == null)
			{
				throw new ArgumentNullException("getEnumeratorCall");
			}
			if (collectionType == null)
			{
				throw new ArgumentNullException("collectionType");
			}
			if (enumeratorType == null)
			{
				throw new ArgumentNullException("enumeratorType");
			}
			if (elementType == null)
			{
				throw new ArgumentNullException("elementType");
			}
			if (elementVariable == null)
			{
				throw new ArgumentNullException("elementVariable");
			}
			this.GetEnumeratorCall = getEnumeratorCall;
			this.CollectionType = collectionType;
			this.EnumeratorType = enumeratorType;
			this.ElementType = elementType;
			this.ElementVariable = elementVariable;
			this.CurrentProperty = currentProperty;
			this.MoveNextMethod = moveNextMethod;
		}

		/// <summary>
		/// Gets the semantic tree for the call to GetEnumerator.
		/// </summary>
		// Token: 0x04000092 RID: 146
		public readonly ResolveResult GetEnumeratorCall;

		/// <summary>
		/// Gets the collection type.
		/// </summary>
		// Token: 0x04000093 RID: 147
		public readonly IType CollectionType;

		/// <summary>
		/// Gets the enumerator type.
		/// </summary>
		// Token: 0x04000094 RID: 148
		public readonly IType EnumeratorType;

		/// <summary>
		/// Gets the element type.
		/// This is the type that would be inferred for an implicitly-typed element variable.
		/// For explicitly-typed element variables, this type may differ from <c>ElementVariable.Type</c>.
		/// </summary>
		// Token: 0x04000095 RID: 149
		public readonly IType ElementType;

		/// <summary>
		/// Gets the element variable.
		/// </summary>
		// Token: 0x04000096 RID: 150
		public readonly IVariable ElementVariable;

		/// <summary>
		/// Gets the Current property on the IEnumerator.
		/// Returns null if the property is not found.
		/// </summary>
		// Token: 0x04000097 RID: 151
		public readonly IProperty CurrentProperty;

		/// <summary>
		/// Gets the MoveNext() method on the IEnumerator.
		/// Returns null if the method is not found.
		/// </summary>
		// Token: 0x04000098 RID: 152
		public readonly IMethod MoveNextMethod;
	}
}
