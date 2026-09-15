using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an unknown method.
	/// </summary>
	public class UnknownMethodResolveResult : UnknownMemberResolveResult
	{
		public UnknownMethodResolveResult(IType targetType, string methodName, IEnumerable<IType> typeArguments, IEnumerable<IParameter> parameters) : base(targetType, methodName, typeArguments)
		{
			this.parameters = new ReadOnlyCollection<IParameter>(parameters.ToArray<IParameter>());
		}

		public ReadOnlyCollection<IParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		private readonly ReadOnlyCollection<IParameter> parameters;
	}
}
