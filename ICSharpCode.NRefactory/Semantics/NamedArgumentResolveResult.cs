using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents a named argument.
	/// </summary>
	// Token: 0x02000049 RID: 73
	public class NamedArgumentResolveResult : ResolveResult
	{
		// Token: 0x0600022F RID: 559 RVA: 0x00006848 File Offset: 0x00005848
		public NamedArgumentResolveResult(IParameter parameter, ResolveResult argument, IParameterizedMember member = null) : base(argument.Type)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			this.Member = member;
			this.Parameter = parameter;
			this.ParameterName = parameter.Name;
			this.Argument = argument;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000689E File Offset: 0x0000589E
		public NamedArgumentResolveResult(string parameterName, ResolveResult argument) : base(argument.Type)
		{
			if (parameterName == null)
			{
				throw new ArgumentNullException("parameterName");
			}
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			this.ParameterName = parameterName;
			this.Argument = argument;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000068D8 File Offset: 0x000058D8
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return new ResolveResult[]
			{
				this.Argument
			};
		}

		/// <summary>
		/// Gets the member to which the parameter belongs.
		/// This field can be null.
		/// </summary>
		// Token: 0x0400009C RID: 156
		public readonly IParameterizedMember Member;

		/// <summary>
		/// Gets the parameter.
		/// This field can be null.
		/// </summary>
		// Token: 0x0400009D RID: 157
		public readonly IParameter Parameter;

		/// <summary>
		/// Gets the parameter name.
		/// </summary>
		// Token: 0x0400009E RID: 158
		public readonly string ParameterName;

		/// <summary>
		/// Gets the argument passed to the parameter.
		/// </summary>
		// Token: 0x0400009F RID: 159
		public readonly ResolveResult Argument;
	}
}
