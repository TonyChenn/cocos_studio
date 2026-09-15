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
	public class ErrorResolveResult : ResolveResult
	{
		public ErrorResolveResult(IType type) : base(type)
		{
		}

		public ErrorResolveResult(IType type, string message, TextLocation location) : base(type)
		{
			this.Message = message;
			this.Location = location;
		}

		public override bool IsError
		{
			get
			{
				return true;
			}
		}

		public string Message { get; private set; }

		public TextLocation Location { get; private set; }

		/// <summary>
		/// Gets an ErrorResolveResult instance with <c>Type</c> = <c>SpecialType.UnknownType</c>.
		/// </summary>
		public static readonly ErrorResolveResult UnknownError = new ErrorResolveResult(SpecialType.UnknownType);
	}
}
