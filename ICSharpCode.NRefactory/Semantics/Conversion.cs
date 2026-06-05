using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Holds information about a conversion between two types.
	/// </summary>
	// Token: 0x0200003F RID: 63
	public abstract class Conversion : IEquatable<Conversion>
	{
		// Token: 0x060001CF RID: 463 RVA: 0x00006014 File Offset: 0x00005014
		public static Conversion EnumerationConversion(bool isImplicit, bool isLifted)
		{
			return new Conversion.NumericOrEnumerationConversion(isImplicit, isLifted, true);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000601E File Offset: 0x0000501E
		[Obsolete("Use UserDefinedConversion() instead")]
		public static Conversion UserDefinedImplicitConversion(IMethod operatorMethod, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(true, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, false);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006039 File Offset: 0x00005039
		[Obsolete("Use UserDefinedConversion() instead")]
		public static Conversion UserDefinedExplicitConversion(IMethod operatorMethod, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(false, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, false);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006054 File Offset: 0x00005054
		public static Conversion UserDefinedConversion(IMethod operatorMethod, bool isImplicit, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted = false, bool isAmbiguous = false)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(isImplicit, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, isAmbiguous);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006071 File Offset: 0x00005071
		public static Conversion MethodGroupConversion(IMethod chosenMethod, bool isVirtualMethodLookup, bool delegateCapturesFirstArgument)
		{
			if (chosenMethod == null)
			{
				throw new ArgumentNullException("chosenMethod");
			}
			return new Conversion.MethodGroupConv(chosenMethod, isVirtualMethodLookup, delegateCapturesFirstArgument, true);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000608A File Offset: 0x0000508A
		public static Conversion InvalidMethodGroupConversion(IMethod chosenMethod, bool isVirtualMethodLookup, bool delegateCapturesFirstArgument)
		{
			if (chosenMethod == null)
			{
				throw new ArgumentNullException("chosenMethod");
			}
			return new Conversion.MethodGroupConv(chosenMethod, isVirtualMethodLookup, delegateCapturesFirstArgument, false);
		}

		/// <summary>
		/// Gets whether the conversion is valid.
		/// </summary>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000060A3 File Offset: 0x000050A3
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000060A6 File Offset: 0x000050A6
		public virtual bool IsImplicit
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000060A9 File Offset: 0x000050A9
		public virtual bool IsExplicit
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether the conversion is an '<c>as</c>' cast.
		/// </summary>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x000060AC File Offset: 0x000050AC
		public virtual bool IsTryCast
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000060AF File Offset: 0x000050AF
		public virtual bool IsIdentityConversion
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001DA RID: 474 RVA: 0x000060B2 File Offset: 0x000050B2
		public virtual bool IsNullLiteralConversion
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000060B5 File Offset: 0x000050B5
		public virtual bool IsConstantExpressionConversion
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001DC RID: 476 RVA: 0x000060B8 File Offset: 0x000050B8
		public virtual bool IsNumericConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is a lifted version of another conversion.
		/// </summary>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000060BB File Offset: 0x000050BB
		public virtual bool IsLifted
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether the conversion is dynamic.
		/// </summary>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000060BE File Offset: 0x000050BE
		public virtual bool IsDynamicConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether the conversion is a reference conversion.
		/// </summary>
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000060C1 File Offset: 0x000050C1
		public virtual bool IsReferenceConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether the conversion is an enumeration conversion.
		/// </summary>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000060C4 File Offset: 0x000050C4
		public virtual bool IsEnumerationConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether the conversion is a nullable conversion
		/// (conversion between a nullable type and the regular type).
		/// </summary>
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000060C7 File Offset: 0x000050C7
		public virtual bool IsNullableConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is user-defined (op_Implicit or op_Explicit).
		/// </summary>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000060CA File Offset: 0x000050CA
		public virtual bool IsUserDefined
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// The conversion that is applied to the input before the user-defined conversion operator is invoked.
		/// </summary>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000060CD File Offset: 0x000050CD
		public virtual Conversion ConversionBeforeUserDefinedOperator
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The conversion that is applied to the result of the user-defined conversion operator.
		/// </summary>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x000060D0 File Offset: 0x000050D0
		public virtual Conversion ConversionAfterUserDefinedOperator
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets whether this conversion is a boxing conversion.
		/// </summary>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000060D3 File Offset: 0x000050D3
		public virtual bool IsBoxingConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is an unboxing conversion.
		/// </summary>
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000060D6 File Offset: 0x000050D6
		public virtual bool IsUnboxingConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is a pointer conversion.
		/// </summary>
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000060D9 File Offset: 0x000050D9
		public virtual bool IsPointerConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is a method group conversion.
		/// </summary>
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000060DC File Offset: 0x000050DC
		public virtual bool IsMethodGroupConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// For method-group conversions, gets whether to perform a virtual method lookup at runtime.
		/// </summary>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000060DF File Offset: 0x000050DF
		public virtual bool IsVirtualMethodLookup
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// For method-group conversions, gets whether the conversion captures the first argument.
		///
		/// For instance methods, this property always returns true for C# method-group conversions.
		/// For static methods, this property returns true for method-group conversions of an extension method performed on an instance (eg. <c>Func&lt;int&gt; f = myEnumerable.Single</c>).
		/// </summary>
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000060E2 File Offset: 0x000050E2
		public virtual bool DelegateCapturesFirstArgument
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets whether this conversion is an anonymous function conversion.
		/// </summary>
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000060E5 File Offset: 0x000050E5
		public virtual bool IsAnonymousFunctionConversion
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the method associated with this conversion.
		/// For user-defined conversions, this is the method being called.
		/// For method-group conversions, this is the method that was chosen from the group.
		/// </summary>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000060E8 File Offset: 0x000050E8
		public virtual IMethod Method
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000060EB File Offset: 0x000050EB
		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as Conversion);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000060F9 File Offset: 0x000050F9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00006101 File Offset: 0x00005101
		public virtual bool Equals(Conversion other)
		{
			return this == other;
		}

		/// <summary>
		/// Not a valid conversion.
		/// </summary>
		// Token: 0x04000070 RID: 112
		public static readonly Conversion None = new Conversion.InvalidConversion();

		/// <summary>
		/// Identity conversion.
		/// </summary>
		// Token: 0x04000071 RID: 113
		public static readonly Conversion IdentityConversion = new Conversion.BuiltinConversion(true, 0);

		// Token: 0x04000072 RID: 114
		public static readonly Conversion ImplicitNumericConversion = new Conversion.NumericOrEnumerationConversion(true, false, false);

		// Token: 0x04000073 RID: 115
		public static readonly Conversion ExplicitNumericConversion = new Conversion.NumericOrEnumerationConversion(false, false, false);

		// Token: 0x04000074 RID: 116
		public static readonly Conversion ImplicitLiftedNumericConversion = new Conversion.NumericOrEnumerationConversion(true, true, false);

		// Token: 0x04000075 RID: 117
		public static readonly Conversion ExplicitLiftedNumericConversion = new Conversion.NumericOrEnumerationConversion(false, true, false);

		// Token: 0x04000076 RID: 118
		public static readonly Conversion NullLiteralConversion = new Conversion.BuiltinConversion(true, 1);

		/// <summary>
		/// The numeric conversion of a constant expression.
		/// </summary>
		// Token: 0x04000077 RID: 119
		public static readonly Conversion ImplicitConstantExpressionConversion = new Conversion.BuiltinConversion(true, 2);

		// Token: 0x04000078 RID: 120
		public static readonly Conversion ImplicitReferenceConversion = new Conversion.BuiltinConversion(true, 3);

		// Token: 0x04000079 RID: 121
		public static readonly Conversion ExplicitReferenceConversion = new Conversion.BuiltinConversion(false, 3);

		// Token: 0x0400007A RID: 122
		public static readonly Conversion ImplicitDynamicConversion = new Conversion.BuiltinConversion(true, 4);

		// Token: 0x0400007B RID: 123
		public static readonly Conversion ExplicitDynamicConversion = new Conversion.BuiltinConversion(false, 4);

		// Token: 0x0400007C RID: 124
		public static readonly Conversion ImplicitNullableConversion = new Conversion.BuiltinConversion(true, 5);

		// Token: 0x0400007D RID: 125
		public static readonly Conversion ExplicitNullableConversion = new Conversion.BuiltinConversion(false, 5);

		// Token: 0x0400007E RID: 126
		public static readonly Conversion ImplicitPointerConversion = new Conversion.BuiltinConversion(true, 6);

		// Token: 0x0400007F RID: 127
		public static readonly Conversion ExplicitPointerConversion = new Conversion.BuiltinConversion(false, 6);

		// Token: 0x04000080 RID: 128
		public static readonly Conversion BoxingConversion = new Conversion.BuiltinConversion(true, 7);

		// Token: 0x04000081 RID: 129
		public static readonly Conversion UnboxingConversion = new Conversion.BuiltinConversion(false, 8);

		/// <summary>
		/// C# 'as' cast.
		/// </summary>
		// Token: 0x04000082 RID: 130
		public static readonly Conversion TryCast = new Conversion.BuiltinConversion(false, 9);

		// Token: 0x02000040 RID: 64
		private sealed class InvalidConversion : Conversion
		{
			// Token: 0x1700008D RID: 141
			// (get) Token: 0x060001F2 RID: 498 RVA: 0x00006204 File Offset: 0x00005204
			public override bool IsValid
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x00006207 File Offset: 0x00005207
			public override string ToString()
			{
				return "None";
			}
		}

		// Token: 0x02000041 RID: 65
		private sealed class NumericOrEnumerationConversion : Conversion
		{
			// Token: 0x060001F5 RID: 501 RVA: 0x00006216 File Offset: 0x00005216
			public NumericOrEnumerationConversion(bool isImplicit, bool isLifted, bool isEnumeration = false)
			{
				this.isImplicit = isImplicit;
				this.isLifted = isLifted;
				this.isEnumeration = isEnumeration;
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x060001F6 RID: 502 RVA: 0x00006233 File Offset: 0x00005233
			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000623B File Offset: 0x0000523B
			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x060001F8 RID: 504 RVA: 0x00006246 File Offset: 0x00005246
			public override bool IsNumericConversion
			{
				get
				{
					return !this.isEnumeration;
				}
			}

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x060001F9 RID: 505 RVA: 0x00006251 File Offset: 0x00005251
			public override bool IsEnumerationConversion
			{
				get
				{
					return this.isEnumeration;
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x060001FA RID: 506 RVA: 0x00006259 File Offset: 0x00005259
			public override bool IsLifted
			{
				get
				{
					return this.isLifted;
				}
			}

			// Token: 0x060001FB RID: 507 RVA: 0x00006264 File Offset: 0x00005264
			public override string ToString()
			{
				return (this.isImplicit ? "implicit" : "explicit") + (this.isLifted ? " lifted" : "") + (this.isEnumeration ? " enumeration" : " numeric") + " conversion";
			}

			// Token: 0x060001FC RID: 508 RVA: 0x000062B8 File Offset: 0x000052B8
			public override bool Equals(Conversion other)
			{
				Conversion.NumericOrEnumerationConversion numericOrEnumerationConversion = other as Conversion.NumericOrEnumerationConversion;
				return numericOrEnumerationConversion != null && this.isImplicit == numericOrEnumerationConversion.isImplicit && this.isLifted == numericOrEnumerationConversion.isLifted && this.isEnumeration == numericOrEnumerationConversion.isEnumeration;
			}

			// Token: 0x060001FD RID: 509 RVA: 0x000062FB File Offset: 0x000052FB
			public override int GetHashCode()
			{
				return (this.isImplicit ? 1 : 0) + (this.isLifted ? 2 : 0) + (this.isEnumeration ? 4 : 0);
			}

			// Token: 0x04000083 RID: 131
			private readonly bool isImplicit;

			// Token: 0x04000084 RID: 132
			private readonly bool isLifted;

			// Token: 0x04000085 RID: 133
			private readonly bool isEnumeration;
		}

		// Token: 0x02000042 RID: 66
		private sealed class BuiltinConversion : Conversion
		{
			// Token: 0x060001FE RID: 510 RVA: 0x00006323 File Offset: 0x00005323
			public BuiltinConversion(bool isImplicit, byte type)
			{
				this.isImplicit = isImplicit;
				this.type = type;
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x060001FF RID: 511 RVA: 0x00006339 File Offset: 0x00005339
			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x06000200 RID: 512 RVA: 0x00006341 File Offset: 0x00005341
			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x06000201 RID: 513 RVA: 0x0000634C File Offset: 0x0000534C
			public override bool IsIdentityConversion
			{
				get
				{
					return this.type == 0;
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000202 RID: 514 RVA: 0x00006357 File Offset: 0x00005357
			public override bool IsNullLiteralConversion
			{
				get
				{
					return this.type == 1;
				}
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x06000203 RID: 515 RVA: 0x00006362 File Offset: 0x00005362
			public override bool IsConstantExpressionConversion
			{
				get
				{
					return this.type == 2;
				}
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x06000204 RID: 516 RVA: 0x0000636D File Offset: 0x0000536D
			public override bool IsReferenceConversion
			{
				get
				{
					return this.type == 3;
				}
			}

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x06000205 RID: 517 RVA: 0x00006378 File Offset: 0x00005378
			public override bool IsDynamicConversion
			{
				get
				{
					return this.type == 4;
				}
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x06000206 RID: 518 RVA: 0x00006383 File Offset: 0x00005383
			public override bool IsNullableConversion
			{
				get
				{
					return this.type == 5;
				}
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x06000207 RID: 519 RVA: 0x0000638E File Offset: 0x0000538E
			public override bool IsPointerConversion
			{
				get
				{
					return this.type == 6;
				}
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x06000208 RID: 520 RVA: 0x00006399 File Offset: 0x00005399
			public override bool IsBoxingConversion
			{
				get
				{
					return this.type == 7;
				}
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x06000209 RID: 521 RVA: 0x000063A4 File Offset: 0x000053A4
			public override bool IsUnboxingConversion
			{
				get
				{
					return this.type == 8;
				}
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x0600020A RID: 522 RVA: 0x000063AF File Offset: 0x000053AF
			public override bool IsTryCast
			{
				get
				{
					return this.type == 9;
				}
			}

			// Token: 0x0600020B RID: 523 RVA: 0x000063BC File Offset: 0x000053BC
			public override string ToString()
			{
				string str = null;
				switch (this.type)
				{
				case 0:
					return "identity conversion";
				case 1:
					return "null-literal conversion";
				case 2:
					str = "constant-expression";
					break;
				case 3:
					str = "reference";
					break;
				case 4:
					str = "dynamic";
					break;
				case 5:
					str = "nullable";
					break;
				case 6:
					str = "pointer";
					break;
				case 7:
					return "boxing conversion";
				case 8:
					return "unboxing conversion";
				case 9:
					return "try cast";
				}
				return (this.isImplicit ? "implicit " : "explicit ") + str + " conversion";
			}

			// Token: 0x04000086 RID: 134
			private readonly bool isImplicit;

			// Token: 0x04000087 RID: 135
			private readonly byte type;
		}

		// Token: 0x02000043 RID: 67
		private sealed class UserDefinedConv : Conversion
		{
			// Token: 0x0600020C RID: 524 RVA: 0x00006467 File Offset: 0x00005467
			public UserDefinedConv(bool isImplicit, IMethod method, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted, bool isAmbiguous)
			{
				this.method = method;
				this.isLifted = isLifted;
				this.conversionBeforeUserDefinedOperator = conversionBeforeUserDefinedOperator;
				this.conversionAfterUserDefinedOperator = conversionAfterUserDefinedOperator;
				this.isImplicit = isImplicit;
				this.isValid = !isAmbiguous;
			}

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x0600020D RID: 525 RVA: 0x0000649F File Offset: 0x0000549F
			public override bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x0600020E RID: 526 RVA: 0x000064A7 File Offset: 0x000054A7
			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x0600020F RID: 527 RVA: 0x000064AF File Offset: 0x000054AF
			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x06000210 RID: 528 RVA: 0x000064BA File Offset: 0x000054BA
			public override bool IsLifted
			{
				get
				{
					return this.isLifted;
				}
			}

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000211 RID: 529 RVA: 0x000064C2 File Offset: 0x000054C2
			public override bool IsUserDefined
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000212 RID: 530 RVA: 0x000064C5 File Offset: 0x000054C5
			public override Conversion ConversionBeforeUserDefinedOperator
			{
				get
				{
					return this.conversionBeforeUserDefinedOperator;
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000213 RID: 531 RVA: 0x000064CD File Offset: 0x000054CD
			public override Conversion ConversionAfterUserDefinedOperator
			{
				get
				{
					return this.conversionAfterUserDefinedOperator;
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000214 RID: 532 RVA: 0x000064D5 File Offset: 0x000054D5
			public override IMethod Method
			{
				get
				{
					return this.method;
				}
			}

			// Token: 0x06000215 RID: 533 RVA: 0x000064E0 File Offset: 0x000054E0
			public override bool Equals(Conversion other)
			{
				Conversion.UserDefinedConv userDefinedConv = other as Conversion.UserDefinedConv;
				return userDefinedConv != null && this.isLifted == userDefinedConv.isLifted && this.isImplicit == userDefinedConv.isImplicit && this.isValid == userDefinedConv.isValid && this.method.Equals(userDefinedConv.method);
			}

			// Token: 0x06000216 RID: 534 RVA: 0x00006534 File Offset: 0x00005534
			public override int GetHashCode()
			{
				return this.method.GetHashCode() + (this.isLifted ? 31 : 27) + (this.isImplicit ? 71 : 61) + (this.isValid ? 107 : 109);
			}

			// Token: 0x06000217 RID: 535 RVA: 0x00006570 File Offset: 0x00005570
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					this.isImplicit ? "implicit" : "explicit",
					this.isLifted ? " lifted" : "",
					this.isValid ? "" : " ambiguous",
					"user-defined conversion (",
					this.method,
					")"
				});
			}

			// Token: 0x04000088 RID: 136
			private readonly IMethod method;

			// Token: 0x04000089 RID: 137
			private readonly bool isLifted;

			// Token: 0x0400008A RID: 138
			private readonly Conversion conversionBeforeUserDefinedOperator;

			// Token: 0x0400008B RID: 139
			private readonly Conversion conversionAfterUserDefinedOperator;

			// Token: 0x0400008C RID: 140
			private readonly bool isImplicit;

			// Token: 0x0400008D RID: 141
			private readonly bool isValid;
		}

		// Token: 0x02000044 RID: 68
		private sealed class MethodGroupConv : Conversion
		{
			// Token: 0x06000218 RID: 536 RVA: 0x000065E8 File Offset: 0x000055E8
			public MethodGroupConv(IMethod method, bool isVirtualMethodLookup, bool delegateCapturesFirstArgument, bool isValid)
			{
				this.method = method;
				this.isVirtualMethodLookup = isVirtualMethodLookup;
				this.delegateCapturesFirstArgument = delegateCapturesFirstArgument;
				this.isValid = isValid;
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000219 RID: 537 RVA: 0x0000660D File Offset: 0x0000560D
			public override bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x0600021A RID: 538 RVA: 0x00006615 File Offset: 0x00005615
			public override bool IsImplicit
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x0600021B RID: 539 RVA: 0x00006618 File Offset: 0x00005618
			public override bool IsMethodGroupConversion
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x0600021C RID: 540 RVA: 0x0000661B File Offset: 0x0000561B
			public override bool IsVirtualMethodLookup
			{
				get
				{
					return this.isVirtualMethodLookup;
				}
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x0600021D RID: 541 RVA: 0x00006623 File Offset: 0x00005623
			public override bool DelegateCapturesFirstArgument
			{
				get
				{
					return this.delegateCapturesFirstArgument;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x0600021E RID: 542 RVA: 0x0000662B File Offset: 0x0000562B
			public override IMethod Method
			{
				get
				{
					return this.method;
				}
			}

			// Token: 0x0600021F RID: 543 RVA: 0x00006634 File Offset: 0x00005634
			public override bool Equals(Conversion other)
			{
				Conversion.MethodGroupConv methodGroupConv = other as Conversion.MethodGroupConv;
				return methodGroupConv != null && this.method.Equals(methodGroupConv.method);
			}

			// Token: 0x06000220 RID: 544 RVA: 0x0000665E File Offset: 0x0000565E
			public override int GetHashCode()
			{
				return this.method.GetHashCode();
			}

			// Token: 0x0400008E RID: 142
			private readonly IMethod method;

			// Token: 0x0400008F RID: 143
			private readonly bool isVirtualMethodLookup;

			// Token: 0x04000090 RID: 144
			private readonly bool delegateCapturesFirstArgument;

			// Token: 0x04000091 RID: 145
			private readonly bool isValid;
		}
	}
}
