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
	public class MemberResolveResult : ResolveResult
	{
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

		public MemberResolveResult(ResolveResult targetResult, IMember member, IType returnType, bool isConstant, object constantValue) : base(returnType)
		{
			this.targetResult = targetResult;
			this.member = member;
			this.isConstant = isConstant;
			this.constantValue = constantValue;
		}

		public MemberResolveResult(ResolveResult targetResult, IMember member, IType returnType, bool isConstant, object constantValue, bool isVirtualCall) : base(returnType)
		{
			this.targetResult = targetResult;
			this.member = member;
			this.isConstant = isConstant;
			this.constantValue = constantValue;
			this.isVirtualCall = isVirtualCall;
		}

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
		public bool IsVirtualCall
		{
			get
			{
				return this.isVirtualCall;
			}
		}

		public override bool IsCompileTimeConstant
		{
			get
			{
				return this.isConstant;
			}
		}

		public override object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
		}

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

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}]", new object[]
			{
				base.GetType().Name,
				this.member
			});
		}

		public override DomRegion GetDefinitionRegion()
		{
			return this.member.Region;
		}

		private readonly IMember member;

		private readonly bool isConstant;

		private readonly object constantValue;

		private readonly ResolveResult targetResult;

		private readonly bool isVirtualCall;
	}
}
