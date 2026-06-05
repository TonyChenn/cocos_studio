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
	// Token: 0x0200004F RID: 79
	public class UnknownMemberResolveResult : ResolveResult
	{
		// Token: 0x06000242 RID: 578 RVA: 0x00006A4F File Offset: 0x00005A4F
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
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00006A89 File Offset: 0x00005A89
		public IType TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00006A91 File Offset: 0x00005A91
		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00006A99 File Offset: 0x00005A99
		public ReadOnlyCollection<IType> TypeArguments
		{
			get
			{
				return this.typeArguments;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00006AA1 File Offset: 0x00005AA1
		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00006AA4 File Offset: 0x00005AA4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[{0} {1}.{2}]", new object[]
			{
				base.GetType().Name,
				this.targetType,
				this.memberName
			});
		}

		// Token: 0x040000A9 RID: 169
		private readonly IType targetType;

		// Token: 0x040000AA RID: 170
		private readonly string memberName;

		// Token: 0x040000AB RID: 171
		private readonly ReadOnlyCollection<IType> typeArguments;
	}
}
