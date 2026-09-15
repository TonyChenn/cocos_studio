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
	public class ConversionResolveResult : ResolveResult
	{
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

		public ConversionResolveResult(IType targetType, ResolveResult input, Conversion conversion, bool checkForOverflow) : this(targetType, input, conversion)
		{
			this.CheckForOverflow = checkForOverflow;
		}

		public override bool IsError
		{
			get
			{
				return !this.Conversion.IsValid;
			}
		}

		public override IEnumerable<ResolveResult> GetChildResults()
		{
			return new ResolveResult[]
			{
				this.Input
			};
		}

		public readonly ResolveResult Input;

		public readonly Conversion Conversion;

		/// <summary>
		/// For numeric conversions, specifies whether overflow checking is enabled.
		/// </summary>
		public readonly bool CheckForOverflow;
	}
}
