using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Default implementation for IUnresolvedParameter.
	/// </summary>
	[Serializable]
	public sealed class DefaultUnresolvedParameter : IUnresolvedParameter, IFreezable, ISupportsInterning
	{
		public DefaultUnresolvedParameter()
		{
		}

		public DefaultUnresolvedParameter(ITypeReference type, string name)
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

		private void FreezeInternal()
		{
			this.attributes = FreezableHelper.FreezeListAndElements<IUnresolvedAttribute>(this.attributes);
			FreezableHelper.Freeze(this.defaultValue);
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.name = value;
			}
		}

		public ITypeReference Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				FreezableHelper.ThrowIfFrozen(this);
				this.type = value;
			}
		}

		public IList<IUnresolvedAttribute> Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new List<IUnresolvedAttribute>();
				}
				return this.attributes;
			}
		}

		public IConstantValue DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.defaultValue = value;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.region;
			}
			set
			{
				FreezableHelper.ThrowIfFrozen(this);
				this.region = value;
			}
		}

		private bool HasFlag(byte flag)
		{
			return (this.flags & flag) != 0;
		}

		private void SetFlag(byte flag, bool value)
		{
			FreezableHelper.ThrowIfFrozen(this);
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags = (byte)(this.flags & (byte)(~flag));
		}

		public bool IsFrozen
		{
			get
			{
				return this.HasFlag(1);
			}
		}

		public void Freeze()
		{
			if (!this.IsFrozen)
			{
				this.FreezeInternal();
				this.flags |= 1;
			}
		}

		public bool IsRef
		{
			get
			{
				return this.HasFlag(2);
			}
			set
			{
				this.SetFlag(2, value);
			}
		}

		public bool IsOut
		{
			get
			{
				return this.HasFlag(4);
			}
			set
			{
				this.SetFlag(4, value);
			}
		}

		public bool IsParams
		{
			get
			{
				return this.HasFlag(8);
			}
			set
			{
				this.SetFlag(8, value);
			}
		}

		public bool IsOptional
		{
			get
			{
				return this.DefaultValue != null;
			}
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			int num = 1919191 ^ ((int)this.flags & -2);
			num *= 31;
			num += this.type.GetHashCode();
			num *= 31;
			num += this.name.GetHashCode();
			if (this.attributes != null)
			{
				foreach (IUnresolvedAttribute unresolvedAttribute in this.attributes)
				{
					num ^= unresolvedAttribute.GetHashCode();
				}
			}
			if (this.defaultValue != null)
			{
				num ^= this.defaultValue.GetHashCode();
			}
			return num;
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			DefaultUnresolvedParameter defaultUnresolvedParameter = other as DefaultUnresolvedParameter;
			return defaultUnresolvedParameter != null && this.type == defaultUnresolvedParameter.type && this.name == defaultUnresolvedParameter.name && this.defaultValue == defaultUnresolvedParameter.defaultValue && this.region == defaultUnresolvedParameter.region && ((int)this.flags & -2) == ((int)defaultUnresolvedParameter.flags & -2) && DefaultUnresolvedParameter.ListEquals(this.attributes, defaultUnresolvedParameter.attributes);
		}

		private static bool ListEquals(IList<IUnresolvedAttribute> list1, IList<IUnresolvedAttribute> list2)
		{
			return (list1 ?? EmptyList<IUnresolvedAttribute>.Instance).SequenceEqual(list2 ?? EmptyList<IUnresolvedAttribute>.Instance);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsRef)
			{
				stringBuilder.Append("ref ");
			}
			if (this.IsOut)
			{
				stringBuilder.Append("out ");
			}
			if (this.IsParams)
			{
				stringBuilder.Append("params ");
			}
			stringBuilder.Append(this.name);
			stringBuilder.Append(':');
			stringBuilder.Append(this.type.ToString());
			if (this.defaultValue != null)
			{
				stringBuilder.Append(" = ");
				stringBuilder.Append(this.defaultValue.ToString());
			}
			return stringBuilder.ToString();
		}

		private static bool IsOptionalAttribute(IType attributeType)
		{
			return attributeType.Name == "OptionalAttribute" && attributeType.Namespace == "System.Runtime.InteropServices";
		}

		public IParameter CreateResolvedParameter(ITypeResolveContext context)
		{
			this.Freeze();
			if (this.defaultValue != null)
			{
				return new DefaultUnresolvedParameter.ResolvedParameterWithDefaultValue(this.defaultValue, context)
				{
					Type = this.type.Resolve(context),
					Name = this.name,
					Region = this.region,
					Attributes = this.attributes.CreateResolvedAttributes(context),
					IsRef = this.IsRef,
					IsOut = this.IsOut,
					IsParams = this.IsParams
				};
			}
			IParameterizedMember owner = context.CurrentMember as IParameterizedMember;
			IList<IAttribute> list = this.attributes.CreateResolvedAttributes(context);
			bool flag;
			if (list != null)
			{
				flag = list.Any((IAttribute a) => DefaultUnresolvedParameter.IsOptionalAttribute(a.AttributeType));
			}
			else
			{
				flag = false;
			}
			bool isOptional = flag;
			return new DefaultParameter(this.type.Resolve(context), this.name, owner, this.region, list, this.IsRef, this.IsOut, this.IsParams, isOptional, null);
		}

		private string name = string.Empty;

		private ITypeReference type = SpecialType.UnknownType;

		private IList<IUnresolvedAttribute> attributes;

		private IConstantValue defaultValue;

		private DomRegion region;

		private byte flags;

		private sealed class ResolvedParameterWithDefaultValue : IParameter, IVariable, ISymbol
		{
			public ResolvedParameterWithDefaultValue(IConstantValue defaultValue, ITypeResolveContext context)
			{
				this.defaultValue = defaultValue;
				this.context = context;
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
					return this.context.CurrentMember as IParameterizedMember;
				}
			}

			public IType Type { get; internal set; }

			public string Name { get; internal set; }

			public DomRegion Region { get; internal set; }

			public IList<IAttribute> Attributes { get; internal set; }

			public bool IsRef { get; internal set; }

			public bool IsOut { get; internal set; }

			public bool IsParams { get; internal set; }

			public bool IsOptional
			{
				get
				{
					return true;
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
					ResolveResult resolveResult = LazyInit.VolatileRead<ResolveResult>(ref this.resolvedDefaultValue);
					if (resolveResult == null)
					{
						resolveResult = this.defaultValue.Resolve(this.context);
						LazyInit.GetOrSet<ResolveResult>(ref this.resolvedDefaultValue, resolveResult);
					}
					return resolveResult.ConstantValue;
				}
			}

			public override string ToString()
			{
				return DefaultParameter.ToString(this);
			}

			public ISymbolReference ToReference()
			{
				if (this.Owner == null)
				{
					return new ParameterReference(this.Type.ToTypeReference(), this.Name, this.Region, this.IsRef, this.IsOut, this.IsParams, true, this.ConstantValue);
				}
				return new OwnedParameterReference(this.Owner.ToReference(), this.Owner.Parameters.IndexOf(this));
			}

			private readonly IConstantValue defaultValue;

			private readonly ITypeResolveContext context;

			private ResolveResult resolvedDefaultValue;
		}
	}
}
