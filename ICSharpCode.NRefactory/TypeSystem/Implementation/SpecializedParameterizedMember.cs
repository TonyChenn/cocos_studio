using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000E0 RID: 224
	public abstract class SpecializedParameterizedMember : SpecializedMember, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x0600085C RID: 2140 RVA: 0x00015E4A File Offset: 0x00014E4A
		protected SpecializedParameterizedMember(IParameterizedMember memberDefinition) : base(memberDefinition)
		{
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00015E54 File Offset: 0x00014E54
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x00015E89 File Offset: 0x00014E89
		public IList<IParameter> Parameters
		{
			get
			{
				IList<IParameter> list = LazyInit.VolatileRead<IList<IParameter>>(ref this.parameters);
				if (list != null)
				{
					return list;
				}
				return LazyInit.GetOrSet<IList<IParameter>>(ref this.parameters, this.CreateParameters(base.Substitution));
			}
			protected set
			{
				this.parameters = value;
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00015E94 File Offset: 0x00014E94
		protected IList<IParameter> CreateParameters(TypeVisitor substitution)
		{
			IList<IParameter> list = ((IParameterizedMember)this.baseMember).Parameters;
			if (list.Count == 0)
			{
				return EmptyList<IParameter>.Instance;
			}
			IParameter[] array = new IParameter[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				IParameter parameter = list[i];
				IType type = parameter.Type.AcceptVisitor(substitution);
				array[i] = new DefaultParameter(type, parameter.Name, this, parameter.Region, parameter.Attributes, parameter.IsRef, parameter.IsOut, parameter.IsParams, parameter.IsOptional, parameter.ConstantValue);
			}
			return Array.AsReadOnly<IParameter>(array);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00015F34 File Offset: 0x00014F34
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(' ');
			stringBuilder.Append(base.DeclaringType.ReflectionName);
			stringBuilder.Append('.');
			stringBuilder.Append(base.Name);
			stringBuilder.Append('(');
			for (int i = 0; i < this.Parameters.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(this.Parameters[i].ToString());
			}
			stringBuilder.Append("):");
			stringBuilder.Append(base.ReturnType.ReflectionName);
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x04000264 RID: 612
		private IList<IParameter> parameters;
	}
}
