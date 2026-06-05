using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Ambiences are used to convert type system symbols to text (usually for displaying the symbol to the user; e.g. in editor tooltips).
	/// </summary>
	// Token: 0x0200008C RID: 140
	public interface IAmbience
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000481 RID: 1153
		// (set) Token: 0x06000482 RID: 1154
		ConversionFlags ConversionFlags { get; set; }

		// Token: 0x06000483 RID: 1155
		[Obsolete("Use ConvertSymbol() instead")]
		string ConvertEntity(IEntity entity);

		// Token: 0x06000484 RID: 1156
		string ConvertSymbol(ISymbol symbol);

		// Token: 0x06000485 RID: 1157
		string ConvertType(IType type);

		// Token: 0x06000486 RID: 1158
		[Obsolete("Use ConvertSymbol() instead")]
		string ConvertVariable(IVariable variable);

		// Token: 0x06000487 RID: 1159
		string ConvertConstantValue(object constantValue);

		// Token: 0x06000488 RID: 1160
		string WrapComment(string comment);
	}
}
