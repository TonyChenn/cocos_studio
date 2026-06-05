using System;
using System.Globalization;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents a local variable or parameter.
	/// </summary>
	// Token: 0x02000048 RID: 72
	public class LocalResolveResult : ResolveResult
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00006767 File Offset: 0x00005767
		public LocalResolveResult(IVariable variable) : base(LocalResolveResult.UnpackTypeIfByRefParameter(variable))
		{
			this.variable = variable;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000677C File Offset: 0x0000577C
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000067CF File Offset: 0x000057CF
		public IVariable Variable
		{
			get
			{
				return this.variable;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000067D7 File Offset: 0x000057D7
		public bool IsParameter
		{
			get
			{
				return this.variable is IParameter;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000067E7 File Offset: 0x000057E7
		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.variable.IsConst;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000067F4 File Offset: 0x000057F4
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

		// Token: 0x0600022D RID: 557 RVA: 0x0000680C File Offset: 0x0000580C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[LocalResolveResult {0}]", new object[]
			{
				this.variable
			});
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00006839 File Offset: 0x00005839
		public override DomRegion GetDefinitionRegion()
		{
			return this.variable.Region;
		}

		// Token: 0x0400009B RID: 155
		private readonly IVariable variable;
	}
}
