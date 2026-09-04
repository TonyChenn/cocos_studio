using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Descibes an error during parsing.
	/// </summary>
	// Token: 0x02000128 RID: 296
	[Serializable]
	public class Error
	{
		/// <summary>
		/// The type of the error.
		/// </summary>
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0001F108 File Offset: 0x0001E108
		public ErrorType ErrorType
		{
			get
			{
				return this.errorType;
			}
		}

		/// <summary>
		/// The error description.
		/// </summary>
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0001F110 File Offset: 0x0001E110
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		/// <summary>
		/// The region of the error.
		/// </summary>
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0001F118 File Offset: 0x0001E118
		public DomRegion Region
		{
			get
			{
				return this.region;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ICSharpCode.NRefactory.TypeSystem.Error" /> class.
		/// </summary>
		/// <param name="errorType">
		/// The error type.
		/// </param>
		/// <param name="message">
		/// The description of the error.
		/// </param>
		/// <param name="region">
		/// The region of the error.
		/// </param>
		// Token: 0x06000A66 RID: 2662 RVA: 0x0001F120 File Offset: 0x0001E120
		public Error(ErrorType errorType, string message, DomRegion region)
		{
			this.errorType = errorType;
			this.message = message;
			this.region = region;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ICSharpCode.NRefactory.TypeSystem.Error" /> class.
		/// </summary>
		/// <param name="errorType">
		/// The error type.
		/// </param>
		/// <param name="message">
		/// The description of the error.
		/// </param>
		/// <param name="location">
		/// The location of the error.
		/// </param>
		// Token: 0x06000A67 RID: 2663 RVA: 0x0001F13D File Offset: 0x0001E13D
		public Error(ErrorType errorType, string message, TextLocation location)
		{
			this.errorType = errorType;
			this.message = message;
			this.region = new DomRegion(location, location);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ICSharpCode.NRefactory.TypeSystem.Error" /> class.
		/// </summary>
		/// <param name="errorType">
		/// The error type.
		/// </param>
		/// <param name="message">
		/// The description of the error.
		/// </param>
		/// <param name="line">
		/// The line of the error.
		/// </param>
		/// <param name="col">
		/// The column of the error.
		/// </param>
		// Token: 0x06000A68 RID: 2664 RVA: 0x0001F160 File Offset: 0x0001E160
		public Error(ErrorType errorType, string message, int line, int col) : this(errorType, message, new TextLocation(line, col))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ICSharpCode.NRefactory.TypeSystem.Error" /> class.
		/// </summary>
		/// <param name="errorType">
		/// The error type.
		/// </param>
		/// <param name="message">
		/// The description of the error.
		/// </param>
		// Token: 0x06000A69 RID: 2665 RVA: 0x0001F172 File Offset: 0x0001E172
		public Error(ErrorType errorType, string message)
		{
			this.errorType = errorType;
			this.message = message;
			this.region = DomRegion.Empty;
		}

		// Token: 0x04000387 RID: 903
		private readonly ErrorType errorType;

		// Token: 0x04000388 RID: 904
		private readonly string message;

		// Token: 0x04000389 RID: 905
		private readonly DomRegion region;
	}
}
