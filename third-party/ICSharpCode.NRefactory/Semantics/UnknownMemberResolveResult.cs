using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an unknown member.
	/// </summary>
	public class UnknownMemberResolveResult : ResolveResult
	{
		public UnknownMemberResolveResult(IType targetType, string memberName, IEnumerable<IType> typeArguments) : base(SpecialType.UnknownType)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			this.targetType = targetType;
			this.memberName = memberName;
			this.typeArguments = new ReadOnlyCollection<IType>(typeArguments.ToArray<IType>());
		}

		/// <summary>
		/// The type on which the method is being called.
		/// </summary>
		public IType TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		public ReadOnlyCollection<IType> TypeArguments
		{
			get
			{
				return this.typeArguments;
			}
		}

		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}.{2}]", new object[]
			{
				base.GetType().Name,
				this.targetType,
				this.memberName
			});
		}

		private readonly IType targetType;

		private readonly string memberName;

		private readonly ReadOnlyCollection<IType> typeArguments;
	}
}
