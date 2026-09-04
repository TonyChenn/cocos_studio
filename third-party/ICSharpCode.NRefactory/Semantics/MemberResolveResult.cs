using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents the result of a member invocation.
	/// Used for field/property/event access.
	/// Also, <see cref="T:ICSharpCode.NRefactory.Semantics.InvocationResolveResult" /> derives from MemberResolveResult.
	/// </summary>
	// Token: 0x02000036 RID: 54
	public class MemberResolveResult : ResolveResult
	{
		// Token: 0x0600019F RID: 415 RVA: 0x00005AC0 File Offset: 0x00004AC0
		public MemberResolveResult(ResolveResult targetResult, IMember member, IType returnTypeOverride = null) : base(returnTypeOverride ?? MemberResolveResult.ComputeType(member))
		{
			this.targetResult = targetResult;
			this.member = member;
			ThisResolveResult thisResolveResult = targetResult as ThisResolveResult;
			this.isVirtualCall = (member.IsOverridable && (thisResolveResult == null || !thisResolveResult.CausesNonVirtualInvocation));
			IField field = member as IField;
			if (field != null)
			{
				this.isConstant = field.IsConst;
				if (this.isConstant)
				{
					this.constantValue = field.ConstantValue;
				}
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005B40 File Offset: 0x00004B40
		public MemberResolveResult(ResolveResult targetResult, IMember member, bool isVirtualCall, IType returnTypeOverride = null) : base(returnTypeOverride ?? MemberResolveResult.ComputeType(member))
		{
			this.targetResult = targetResult;
			this.member = member;
			this.isVirtualCall = isVirtualCall;
			IField field = member as IField;
			if (field != null)
			{
				this.isConstant = field.IsConst;
				if (this.isConstant)
				{
					this.constantValue = field.ConstantValue;
				}
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005BA0 File Offset: 0x00004BA0
		private static IType ComputeType(IMember member)
		{
			SymbolKind symbolKind = member.SymbolKind;
			if (symbolKind != SymbolKind.Field)
			{
				if (symbolKind == SymbolKind.Constructor)
				{
					return member.DeclaringType;
				}
			}
			else if (((IField)member).IsFixed)
			{
				return new PointerType(member.ReturnType);
			}
			return member.ReturnType;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005BE2 File Offset: 0x00004BE2
		public MemberResolveResult(ResolveResult targetResult, IMember member, IType returnType, bool isConstant, object constantValue) : base(returnType)
		{
			this.targetResult = targetResult;
			this.member = member;
			this.isConstant = isConstant;
			this.constantValue = constantValue;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005C09 File Offset: 0x00004C09
		public MemberResolveResult(ResolveResult targetResult, IMember member, IType returnType, bool isConstant, object constantValue, bool isVirtualCall) : base(returnType)
		{
			this.targetResult = targetResult;
			this.member = member;
			this.isConstant = isConstant;
			this.constantValue = constantValue;
			this.isVirtualCall = isVirtualCall;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00005C38 File Offset: 0x00004C38
		public ResolveResult TargetResult
		{
			get
			{
				return this.targetResult;
			}
		}

		/// <summary>
		/// Gets the member.
		/// This property never returns null.
		/// </summary>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00005C40 File Offset: 0x00004C40
		public IMember Member
		{
			get
			{
				return this.member;
			}
		}

		/// <summary>
		/// Gets whether this MemberResolveResult is a virtual call.
		/// </summary>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00005C48 File Offset: 0x00004C48
		public bool IsVirtualCall
		{
			get
			{
				return this.isVirtualCall;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00005C50 File Offset: 0x00004C50
		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.isConstant;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00005C58 File Offset: 0x00004C58
		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005C60 File Offset: 0x00004C60
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			if (this.targetResult != null)
			{
				return new ResolveResult[]
				{
					this.targetResult
				};
			}
			return Enumerable.Empty<ResolveResult>();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005C8C File Offset: 0x00004C8C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.member
			});
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005CC7 File Offset: 0x00004CC7
		public override DomRegion GetDefinitionRegion()
		{
			return this.member.Region;
		}

		// Token: 0x0400005C RID: 92
		private readonly IMember member;

		// Token: 0x0400005D RID: 93
		private readonly bool isConstant;

		// Token: 0x0400005E RID: 94
		private readonly object constantValue;

		// Token: 0x0400005F RID: 95
		private readonly ResolveResult targetResult;

		// Token: 0x04000060 RID: 96
		private readonly bool isVirtualCall;
	}
}
