using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents a resolve error.
	///
	/// Note: some errors are represented by other classes; for example a <see cref="T:ICSharpCode.NRefactory.Semantics.ConversionResolveResult" /> may
	/// be erroneous if the conversion is invalid.
	/// </summary>
	/// <seealso cref="P:ICSharpCode.NRefactory.Semantics.ResolveResult.IsError" />.
	// Token: 0x0200003E RID: 62
	public class ErrorResolveResult : ResolveResult
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x00005FBE File Offset: 0x00004FBE
		public ErrorResolveResult(IType type) : base(type)
		{
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005FC7 File Offset: 0x00004FC7
		public ErrorResolveResult(IType type, string message, TextLocation location) : base(type)
		{
			this.Message = message;
			this.Location = location;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00005FDE File Offset: 0x00004FDE
		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00005FE1 File Offset: 0x00004FE1
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00005FE9 File Offset: 0x00004FE9
		public string Message { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00005FF2 File Offset: 0x00004FF2
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00005FFA File Offset: 0x00004FFA
		public TextLocation Location { get; private set; }

		/// <summary>
		/// Gets an ErrorResolveResult instance with <c>Type</c> = <c>SpecialType.UnknownType</c>.
		/// </summary>
		// Token: 0x0400006D RID: 109
		public static readonly ErrorResolveResult UnknownError = new ErrorResolveResult(SpecialType.UnknownType);
	}
}
