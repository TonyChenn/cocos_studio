using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents a local variable or parameter.
	/// </summary>
	public class LocalResolveResult : ResolveResult
	{
		public LocalResolveResult(IVariable variable) : base(LocalResolveResult.UnpackTypeIfByRefParameter(variable))
		{
			this.variable = variable;
		}

		private static IType UnpackTypeIfByRefParameter(IVariable variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			IType type = variable.Type;
			if (type.Kind == TypeKind.ByReference)
			{
				IParameter parameter = variable as IParameter;
				if (parameter != null && (parameter.IsRef || parameter.IsOut))
				{
					return ((ByReferenceType)type).ElementType;
				}
			}
			return type;
		}

		public IVariable Variable
		{
			get
			{
				return this.variable;
			}
		}

		public bool IsParameter
		{
			get
			{
				return this.variable is IParameter;
			}
		}

		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.variable.IsConst;
			}
		}

		public override object ConstantValue
		{
			get
			{
				if (!this.IsParameter)
				{
					return this.variable.ConstantValue;
				}
				return null;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[LocalResolveResult {0}]", new object[]
			{
				this.variable
			});
		}

		public override DomRegion GetDefinitionRegion()
		{
			return this.variable.Region;
		}

		private readonly IVariable variable;
	}
}
