using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Represents an implicit or explicit type conversion.
	/// <c>conversionResolveResult.Input.Type</c> is the source type;
	/// <c>conversionResolveResult.Type</c> is the target type.
	/// The <see cref="F:ICSharpCode.NRefactory.Semantics.ConversionResolveResult.Conversion" /> property provides details about the kind of conversion.
	/// </summary>
	// Token: 0x0200003C RID: 60
	public class ConversionResolveResult : ResolveResult
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00005EC4 File Offset: 0x00004EC4
		public ConversionResolveResult(IType targetType, ResolveResult input, Conversion conversion) : base(targetType)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (conversion == null)
			{
				throw new ArgumentNullException("conversion");
			}
			this.Input = input;
			this.Conversion = conversion;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005EF7 File Offset: 0x00004EF7
		public ConversionResolveResult(IType targetType, ResolveResult input, Conversion conversion, bool checkForOverflow) : this(targetType, input, conversion)
		{
			this.CheckForOverflow = checkForOverflow;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005F0A File Offset: 0x00004F0A
		public override bool IsError
		{
			get
			{
				return !this.Conversion.IsValid;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00005F1C File Offset: 0x00004F1C
		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return new ResolveResult[]
			{
				this.Input
			};
		}

		// Token: 0x04000068 RID: 104
		public readonly ResolveResult Input;

		// Token: 0x04000069 RID: 105
		public readonly Conversion Conversion;

		/// <summary>
		/// For numeric conversions, specifies whether overflow checking is enabled.
		/// </summary>
		// Token: 0x0400006A RID: 106
		public readonly bool CheckForOverflow;
	}
}
