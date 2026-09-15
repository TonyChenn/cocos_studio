using System;
using System.Collections.Generic;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IParameter" />.
	/// </summary>
	public sealed class DefaultParameter : IParameter, IVariable, ISymbol
	{
		public DefaultParameter(IType type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.type = type;
			this.name = name;
		}

		public DefaultParameter(IType type, string name, IParameterizedMember owner = null, DomRegion region = default(DomRegion), IList<IAttribute> attributes = null, bool isRef = false, bool isOut = false, bool isParams = false, bool isOptional = false, object defaultValue = null)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.type = type;
			this.name = name;
			this.owner = owner;
			this.region = region;
			this.attributes = attributes;
			this.isRef = isRef;
			this.isOut = isOut;
			this.isParams = isParams;
			this.isOptional = isOptional;
			this.defaultValue = defaultValue;
		}

		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.Parameter;
			}
		}

		public IParameterizedMember Owner
		{
			get
			{
				return this.owner;
			}
		}

		public IList<IAttribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		public bool IsRef
		{
			get
			{
				return this.isRef;
			}
		}

		public bool IsOut
		{
			get
			{
				return this.isOut;
			}
		}

		public bool IsParams
		{
			get
			{
				return this.isParams;
			}
		}

		public bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		public IType Type
		{
			get
			{
				return this.type;
			}
		}

		bool IVariable.IsConst
		{
			get
			{
				return false;
			}
		}

		public object ConstantValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		public override string ToString()
		{
			return DefaultParameter.ToString(this);
		}

		public static string ToString(IParameter parameter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (parameter.IsRef)
			{
				stringBuilder.Append("ref ");
			}
			if (parameter.IsOut)
			{
				stringBuilder.Append("out ");
			}
			if (parameter.IsParams)
			{
				stringBuilder.Append("params ");
			}
			stringBuilder.Append(parameter.Name);
			stringBuilder.Append(':');
			stringBuilder.Append(parameter.Type.ReflectionName);
			if (parameter.IsOptional)
			{
				stringBuilder.Append(" = ");
				if (parameter.ConstantValue != null)
				{
					stringBuilder.Append(parameter.ConstantValue.ToString());
				}
				else
				{
					stringBuilder.Append("null");
				}
			}
			return stringBuilder.ToString();
		}

		public ISymbolReference ToReference()
		{
			if (this.owner == null)
			{
				return new ParameterReference(this.type.ToTypeReference(), this.name, this.region, this.isRef, this.isOut, this.isParams, this.isOptional, this.defaultValue);
			}
			return new OwnedParameterReference(this.owner.ToReference(), this.owner.Parameters.IndexOf(this));
		}

		private readonly IType type;

		private readonly string name;

		private readonly DomRegion region;

		private readonly IList<IAttribute> attributes;

		private readonly bool isRef;

		private readonly bool isOut;

		private readonly bool isParams;

		private readonly bool isOptional;

		private readonly object defaultValue;

		private readonly IParameterizedMember owner;
	}
}
