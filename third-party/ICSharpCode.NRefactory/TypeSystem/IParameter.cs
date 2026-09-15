using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public interface IParameter : IVariable, ISymbol
	{
		/// <summary>
		/// Gets the list of attributes.
		/// </summary>
		IList<IAttribute> Attributes { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'ref' parameter.
		/// </summary>
		bool IsRef { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'out' parameter.
		/// </summary>
		bool IsOut { get; }

		/// <summary>
		/// Gets whether this parameter is a C# 'params' parameter.
		/// </summary>
		bool IsParams { get; }

		/// <summary>
		/// Gets whether this parameter is optional.
		/// The default value is given by the <see cref="P:ICSharpCode.NRefactory.TypeSystem.IVariable.ConstantValue" /> property.
		/// </summary>
		bool IsOptional { get; }

		/// <summary>
		/// Gets the owner of this parameter.
		/// May return null; for example when parameters belong to lambdas or anonymous methods.
		/// </summary>
		IParameterizedMember Owner { get; }
	}
}
