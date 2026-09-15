using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Semantics
{
	/// <summary>
	/// Holds information about a conversion between two types.
	/// </summary>
	public abstract class Conversion : IEquatable<Conversion>
	{
		public static Conversion EnumerationConversion(bool isImplicit, bool isLifted)
		{
			return new Conversion.NumericOrEnumerationConversion(isImplicit, isLifted, true);
		}

		[Obsolete("Use UserDefinedConversion() instead")]
		public static Conversion UserDefinedImplicitConversion(IMethod operatorMethod, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(true, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, false);
		}

		[Obsolete("Use UserDefinedConversion() instead")]
		public static Conversion UserDefinedExplicitConversion(IMethod operatorMethod, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(false, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, false);
		}

		public static Conversion UserDefinedConversion(IMethod operatorMethod, bool isImplicit, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted = false, bool isAmbiguous = false)
		{
			if (operatorMethod == null)
			{
				throw new ArgumentNullException("operatorMethod");
			}
			return new Conversion.UserDefinedConv(isImplicit, operatorMethod, conversionBeforeUserDefinedOperator, conversionAfterUserDefinedOperator, isLifted, isAmbiguous);
		}

		public static Conversion MethodGroupConversion(IMethod chosenMethod, bool isVirtualMethodLookup, bool delegateCapturesFirstArgument)
		{
			if (chosenMethod == null)
			{
				throw new ArgumentNullException("chosenMethod");
			}
			return new Conversion.MethodGroupConv(chosenMethod, isVirtualMethodLookup, delegateCapturesFirstArgument, true);
		}

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
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		public virtual bool IsImplicit
		{
			get
			{
				return false;
			}
		}

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
		public virtual bool IsTryCast
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsIdentityConversion
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsNullLiteralConversion
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsConstantExpressionConversion
		{
			get
			{
				return false;
			}
		}

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
		public virtual IMethod Method
		{
			get
			{
				return null;
			}
		}

		public sealed override bool Equals(object obj)
		{
			return this.Equals(obj as Conversion);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public virtual bool Equals(Conversion other)
		{
			return this == other;
		}

		/// <summary>
		/// Not a valid conversion.
		/// </summary>
		public static readonly Conversion None = new Conversion.InvalidConversion();

		/// <summary>
		/// Identity conversion.
		/// </summary>
		public static readonly Conversion IdentityConversion = new Conversion.BuiltinConversion(true, 0);

		public static readonly Conversion ImplicitNumericConversion = new Conversion.NumericOrEnumerationConversion(true, false, false);

		public static readonly Conversion ExplicitNumericConversion = new Conversion.NumericOrEnumerationConversion(false, false, false);

		public static readonly Conversion ImplicitLiftedNumericConversion = new Conversion.NumericOrEnumerationConversion(true, true, false);

		public static readonly Conversion ExplicitLiftedNumericConversion = new Conversion.NumericOrEnumerationConversion(false, true, false);

		public static readonly Conversion NullLiteralConversion = new Conversion.BuiltinConversion(true, 1);

		/// <summary>
		/// The numeric conversion of a constant expression.
		/// </summary>
		public static readonly Conversion ImplicitConstantExpressionConversion = new Conversion.BuiltinConversion(true, 2);

		public static readonly Conversion ImplicitReferenceConversion = new Conversion.BuiltinConversion(true, 3);

		public static readonly Conversion ExplicitReferenceConversion = new Conversion.BuiltinConversion(false, 3);

		public static readonly Conversion ImplicitDynamicConversion = new Conversion.BuiltinConversion(true, 4);

		public static readonly Conversion ExplicitDynamicConversion = new Conversion.BuiltinConversion(false, 4);

		public static readonly Conversion ImplicitNullableConversion = new Conversion.BuiltinConversion(true, 5);

		public static readonly Conversion ExplicitNullableConversion = new Conversion.BuiltinConversion(false, 5);

		public static readonly Conversion ImplicitPointerConversion = new Conversion.BuiltinConversion(true, 6);

		public static readonly Conversion ExplicitPointerConversion = new Conversion.BuiltinConversion(false, 6);

		public static readonly Conversion BoxingConversion = new Conversion.BuiltinConversion(true, 7);

		public static readonly Conversion UnboxingConversion = new Conversion.BuiltinConversion(false, 8);

		/// <summary>
		/// C# 'as' cast.
		/// </summary>
		public static readonly Conversion TryCast = new Conversion.BuiltinConversion(false, 9);

		private sealed class InvalidConversion : Conversion
		{
			public override bool IsValid
			{
				get
				{
					return false;
				}
			}

			public override string ToString()
			{
				return "None";
			}
		}

		private sealed class NumericOrEnumerationConversion : Conversion
		{
			public NumericOrEnumerationConversion(bool isImplicit, bool isLifted, bool isEnumeration = false)
			{
				this.isImplicit = isImplicit;
				this.isLifted = isLifted;
				this.isEnumeration = isEnumeration;
			}

			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			public override bool IsNumericConversion
			{
				get
				{
					return !this.isEnumeration;
				}
			}

			public override bool IsEnumerationConversion
			{
				get
				{
					return this.isEnumeration;
				}
			}

			public override bool IsLifted
			{
				get
				{
					return this.isLifted;
				}
			}

			public override string ToString()
			{
				return (this.isImplicit ? "implicit" : "explicit") + (this.isLifted ? " lifted" : "") + (this.isEnumeration ? " enumeration" : " numeric") + " conversion";
			}

			public override bool Equals(Conversion other)
			{
				Conversion.NumericOrEnumerationConversion numericOrEnumerationConversion = other as Conversion.NumericOrEnumerationConversion;
				return numericOrEnumerationConversion != null && this.isImplicit == numericOrEnumerationConversion.isImplicit && this.isLifted == numericOrEnumerationConversion.isLifted && this.isEnumeration == numericOrEnumerationConversion.isEnumeration;
			}

			public override int GetHashCode()
			{
				return (this.isImplicit ? 1 : 0) + (this.isLifted ? 2 : 0) + (this.isEnumeration ? 4 : 0);
			}

			private readonly bool isImplicit;

			private readonly bool isLifted;

			private readonly bool isEnumeration;
		}

		private sealed class BuiltinConversion : Conversion
		{
			public BuiltinConversion(bool isImplicit, byte type)
			{
				this.isImplicit = isImplicit;
				this.type = type;
			}

			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			public override bool IsIdentityConversion
			{
				get
				{
					return this.type == 0;
				}
			}

			public override bool IsNullLiteralConversion
			{
				get
				{
					return this.type == 1;
				}
			}

			public override bool IsConstantExpressionConversion
			{
				get
				{
					return this.type == 2;
				}
			}

			public override bool IsReferenceConversion
			{
				get
				{
					return this.type == 3;
				}
			}

			public override bool IsDynamicConversion
			{
				get
				{
					return this.type == 4;
				}
			}

			public override bool IsNullableConversion
			{
				get
				{
					return this.type == 5;
				}
			}

			public override bool IsPointerConversion
			{
				get
				{
					return this.type == 6;
				}
			}

			public override bool IsBoxingConversion
			{
				get
				{
					return this.type == 7;
				}
			}

			public override bool IsUnboxingConversion
			{
				get
				{
					return this.type == 8;
				}
			}

			public override bool IsTryCast
			{
				get
				{
					return this.type == 9;
				}
			}

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

			private readonly bool isImplicit;

			private readonly byte type;
		}

		private sealed class UserDefinedConv : Conversion
		{
			public UserDefinedConv(bool isImplicit, IMethod method, Conversion conversionBeforeUserDefinedOperator, Conversion conversionAfterUserDefinedOperator, bool isLifted, bool isAmbiguous)
			{
				this.method = method;
				this.isLifted = isLifted;
				this.conversionBeforeUserDefinedOperator = conversionBeforeUserDefinedOperator;
				this.conversionAfterUserDefinedOperator = conversionAfterUserDefinedOperator;
				this.isImplicit = isImplicit;
				this.isValid = !isAmbiguous;
			}

			public override bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			public override bool IsImplicit
			{
				get
				{
					return this.isImplicit;
				}
			}

			public override bool IsExplicit
			{
				get
				{
					return !this.isImplicit;
				}
			}

			public override bool IsLifted
			{
				get
				{
					return this.isLifted;
				}
			}

			public override bool IsUserDefined
			{
				get
				{
					return true;
				}
			}

			public override Conversion ConversionBeforeUserDefinedOperator
			{
				get
				{
					return this.conversionBeforeUserDefinedOperator;
				}
			}

			public override Conversion ConversionAfterUserDefinedOperator
			{
				get
				{
					return this.conversionAfterUserDefinedOperator;
				}
			}

			public override IMethod Method
			{
				get
				{
					return this.method;
				}
			}

			public override bool Equals(Conversion other)
			{
				Conversion.UserDefinedConv userDefinedConv = other as Conversion.UserDefinedConv;
				return userDefinedConv != null && this.isLifted == userDefinedConv.isLifted && this.isImplicit == userDefinedConv.isImplicit && this.isValid == userDefinedConv.isValid && this.method.Equals(userDefinedConv.method);
			}

			public override int GetHashCode()
			{
				return this.method.GetHashCode() + (this.isLifted ? 31 : 27) + (this.isImplicit ? 71 : 61) + (this.isValid ? 107 : 109);
			}

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

			private readonly IMethod method;

			private readonly bool isLifted;

			private readonly Conversion conversionBeforeUserDefinedOperator;

			private readonly Conversion conversionAfterUserDefinedOperator;

			private readonly bool isImplicit;

			private readonly bool isValid;
		}

		private sealed class MethodGroupConv : Conversion
		{
			public MethodGroupConv(IMethod method, bool isVirtualMethodLookup, bool delegateCapturesFirstArgument, bool isValid)
			{
				this.method = method;
				this.isVirtualMethodLookup = isVirtualMethodLookup;
				this.delegateCapturesFirstArgument = delegateCapturesFirstArgument;
				this.isValid = isValid;
			}

			public override bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			public override bool IsImplicit
			{
				get
				{
					return true;
				}
			}

			public override bool IsMethodGroupConversion
			{
				get
				{
					return true;
				}
			}

			public override bool IsVirtualMethodLookup
			{
				get
				{
					return this.isVirtualMethodLookup;
				}
			}

			public override bool DelegateCapturesFirstArgument
			{
				get
				{
					return this.delegateCapturesFirstArgument;
				}
			}

			public override IMethod Method
			{
				get
				{
					return this.method;
				}
			}

			public override bool Equals(Conversion other)
			{
				Conversion.MethodGroupConv methodGroupConv = other as Conversion.MethodGroupConv;
				return methodGroupConv != null && this.method.Equals(methodGroupConv.method);
			}

			public override int GetHashCode()
			{
				return this.method.GetHashCode();
			}

			private readonly IMethod method;

			private readonly bool isVirtualMethodLookup;

			private readonly bool delegateCapturesFirstArgument;

			private readonly bool isValid;
		}
	}
}
