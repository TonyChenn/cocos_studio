using System;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Static helper method for converting between primitive types.
	/// </summary>
	// Token: 0x0200010B RID: 267
	public static class CSharpPrimitiveCast
	{
		/// <summary>
		/// Performs a conversion between primitive types.
		/// Unfortunately we cannot use Convert.ChangeType because it has different semantics
		/// (e.g. rounding behavior for floats, overflow, etc.), so we write down every possible primitive C# cast
		/// and let the compiler figure out the exact semantics.
		/// And we have to do everything twice, once in a checked-block, once in an unchecked-block.
		/// </summary>
		/// <exception cref="T:System.OverflowException">Overflow checking is enabled and an overflow occurred.</exception>
		/// <exception cref="T:System.InvalidCastException">The cast is invalid, e.g. casting a boolean to an integer.</exception>
		// Token: 0x06000994 RID: 2452 RVA: 0x000199DF File Offset: 0x000189DF
		public static object Cast(TypeCode targetType, object input, bool checkForOverflow)
		{
			if (input == null)
			{
				return null;
			}
			if (checkForOverflow)
			{
				return CSharpPrimitiveCast.CSharpPrimitiveCastChecked(targetType, input);
			}
			return CSharpPrimitiveCast.CSharpPrimitiveCastUnchecked(targetType, input);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000199F8 File Offset: 0x000189F8
		private static object CSharpPrimitiveCastChecked(TypeCode targetType, object input)
		{
			TypeCode typeCode = Type.GetTypeCode(input.GetType());
			if (typeCode == targetType)
			{
				return input;
			}
			checked
			{
				switch (targetType)
				{
				case TypeCode.Char:
					switch (typeCode)
					{
					case TypeCode.SByte:
						return (char)((sbyte)input);
					case TypeCode.Byte:
						return (char)((byte)input);
					case TypeCode.Int16:
						return (char)((short)input);
					case TypeCode.UInt16:
						return (char)((ushort)input);
					case TypeCode.Int32:
						return (char)((int)input);
					case TypeCode.UInt32:
						return (char)((uint)input);
					case TypeCode.Int64:
						return (char)((long)input);
					case TypeCode.UInt64:
						return (char)((ulong)input);
					case TypeCode.Single:
						return (char)((float)input);
					case TypeCode.Double:
						return (char)((double)input);
					case TypeCode.Decimal:
						return (char)((decimal)input);
					}
					break;
				case TypeCode.SByte:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (sbyte)((char)input);
					case TypeCode.Byte:
						return (sbyte)((byte)input);
					case TypeCode.Int16:
						return (sbyte)((short)input);
					case TypeCode.UInt16:
						return (sbyte)((ushort)input);
					case TypeCode.Int32:
						return (sbyte)((int)input);
					case TypeCode.UInt32:
						return (sbyte)((uint)input);
					case TypeCode.Int64:
						return (sbyte)((long)input);
					case TypeCode.UInt64:
						return (sbyte)((ulong)input);
					case TypeCode.Single:
						return (sbyte)((float)input);
					case TypeCode.Double:
						return (sbyte)((double)input);
					case TypeCode.Decimal:
						return (sbyte)((decimal)input);
					}
					break;
				case TypeCode.Byte:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (byte)((char)input);
					case TypeCode.SByte:
						return (byte)((sbyte)input);
					case TypeCode.Int16:
						return (byte)((short)input);
					case TypeCode.UInt16:
						return (byte)((ushort)input);
					case TypeCode.Int32:
						return (byte)((int)input);
					case TypeCode.UInt32:
						return (byte)((uint)input);
					case TypeCode.Int64:
						return (byte)((long)input);
					case TypeCode.UInt64:
						return (byte)((ulong)input);
					case TypeCode.Single:
						return (byte)((float)input);
					case TypeCode.Double:
						return (byte)((double)input);
					case TypeCode.Decimal:
						return (byte)((decimal)input);
					}
					break;
				case TypeCode.Int16:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (short)((char)input);
					case TypeCode.SByte:
						return (short)((sbyte)input);
					case TypeCode.Byte:
						return (short)((byte)input);
					case TypeCode.UInt16:
						return (short)((ushort)input);
					case TypeCode.Int32:
						return (short)((int)input);
					case TypeCode.UInt32:
						return (short)((uint)input);
					case TypeCode.Int64:
						return (short)((long)input);
					case TypeCode.UInt64:
						return (short)((ulong)input);
					case TypeCode.Single:
						return (short)((float)input);
					case TypeCode.Double:
						return (short)((double)input);
					case TypeCode.Decimal:
						return (short)((decimal)input);
					}
					break;
				case TypeCode.UInt16:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (ushort)((char)input);
					case TypeCode.SByte:
						return (ushort)((sbyte)input);
					case TypeCode.Byte:
						return (ushort)((byte)input);
					case TypeCode.Int16:
						return (ushort)((short)input);
					case TypeCode.Int32:
						return (ushort)((int)input);
					case TypeCode.UInt32:
						return (ushort)((uint)input);
					case TypeCode.Int64:
						return (ushort)((long)input);
					case TypeCode.UInt64:
						return (ushort)((ulong)input);
					case TypeCode.Single:
						return (ushort)((float)input);
					case TypeCode.Double:
						return (ushort)((double)input);
					case TypeCode.Decimal:
						return (ushort)((decimal)input);
					}
					break;
				case TypeCode.Int32:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (int)((char)input);
					case TypeCode.SByte:
						return (int)((sbyte)input);
					case TypeCode.Byte:
						return (int)((byte)input);
					case TypeCode.Int16:
						return (int)((short)input);
					case TypeCode.UInt16:
						return (int)((ushort)input);
					case TypeCode.UInt32:
						return (int)((uint)input);
					case TypeCode.Int64:
						return (int)((long)input);
					case TypeCode.UInt64:
						return (int)((ulong)input);
					case TypeCode.Single:
						return (int)((float)input);
					case TypeCode.Double:
						return (int)((double)input);
					case TypeCode.Decimal:
						return (int)((decimal)input);
					}
					break;
				case TypeCode.UInt32:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (uint)((char)input);
					case TypeCode.SByte:
						return (uint)((sbyte)input);
					case TypeCode.Byte:
						return (uint)((byte)input);
					case TypeCode.Int16:
						return (uint)((short)input);
					case TypeCode.UInt16:
						return (uint)((ushort)input);
					case TypeCode.Int32:
						return (uint)((int)input);
					case TypeCode.Int64:
						return (uint)((long)input);
					case TypeCode.UInt64:
						return (uint)((ulong)input);
					case TypeCode.Single:
						return (uint)((float)input);
					case TypeCode.Double:
						return (uint)((double)input);
					case TypeCode.Decimal:
						return (uint)((decimal)input);
					}
					break;
				case TypeCode.Int64:
					unchecked
					{
						switch (typeCode)
						{
						case TypeCode.Char:
							return (long)((char)input);
						case TypeCode.SByte:
							return (long)((sbyte)input);
						case TypeCode.Byte:
							return (long)((ulong)((byte)input));
						case TypeCode.Int16:
							return (long)((short)input);
						case TypeCode.UInt16:
							return (long)((ulong)((ushort)input));
						case TypeCode.Int32:
							return (long)((int)input);
						case TypeCode.UInt32:
							return (long)((ulong)((uint)input));
						case TypeCode.UInt64:
							return checked((long)((ulong)input));
						case TypeCode.Single:
							return checked((long)((float)input));
						case TypeCode.Double:
							return checked((long)((double)input));
						case TypeCode.Decimal:
							return (long)((decimal)input);
						}
						break;
					}
				case TypeCode.UInt64:
					switch (typeCode)
					{
					case TypeCode.Char:
						return unchecked((ulong)((char)input));
					case TypeCode.SByte:
						return (ulong)((sbyte)input);
					case TypeCode.Byte:
						return unchecked((ulong)((byte)input));
					case TypeCode.Int16:
						return (ulong)((short)input);
					case TypeCode.UInt16:
						return unchecked((ulong)((ushort)input));
					case TypeCode.Int32:
						return (ulong)((int)input);
					case TypeCode.UInt32:
						return unchecked((ulong)((uint)input));
					case TypeCode.Int64:
						return (ulong)((long)input);
					case TypeCode.Single:
						return (ulong)((float)input);
					case TypeCode.Double:
						return (ulong)((double)input);
					case TypeCode.Decimal:
						return (ulong)((decimal)input);
					}
					break;
				case TypeCode.Single:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (float)((char)input);
					case TypeCode.SByte:
						return (float)((sbyte)input);
					case TypeCode.Byte:
						return (float)((byte)input);
					case TypeCode.Int16:
						return (float)((short)input);
					case TypeCode.UInt16:
						return (float)((ushort)input);
					case TypeCode.Int32:
						return (float)((int)input);
					case TypeCode.UInt32:
						return (uint)input;
					case TypeCode.Int64:
						return (float)((long)input);
					case TypeCode.UInt64:
						return (ulong)input;
					case TypeCode.Double:
						return (float)((double)input);
					case TypeCode.Decimal:
						return (float)((decimal)input);
					}
					break;
				case TypeCode.Double:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (double)((char)input);
					case TypeCode.SByte:
						return (double)((sbyte)input);
					case TypeCode.Byte:
						return (double)((byte)input);
					case TypeCode.Int16:
						return (double)((short)input);
					case TypeCode.UInt16:
						return (double)((ushort)input);
					case TypeCode.Int32:
						return (double)((int)input);
					case TypeCode.UInt32:
						return (uint)input;
					case TypeCode.Int64:
						return (double)((long)input);
					case TypeCode.UInt64:
						return (ulong)input;
					case TypeCode.Single:
						return (double)((float)input);
					case TypeCode.Decimal:
						return (double)((decimal)input);
					}
					break;
				case TypeCode.Decimal:
					switch (typeCode)
					{
					case TypeCode.Char:
						return (char)input;
					case TypeCode.SByte:
						return (sbyte)input;
					case TypeCode.Byte:
						return (byte)input;
					case TypeCode.Int16:
						return (short)input;
					case TypeCode.UInt16:
						return (ushort)input;
					case TypeCode.Int32:
						return (int)input;
					case TypeCode.UInt32:
						return (uint)input;
					case TypeCode.Int64:
						return (long)input;
					case TypeCode.UInt64:
						return (ulong)input;
					case TypeCode.Single:
						return (decimal)((float)input);
					case TypeCode.Double:
						return (decimal)((double)input);
					}
					break;
				}
				throw new InvalidCastException(string.Concat(new object[]
				{
					"Cast from ",
					typeCode,
					" to ",
					targetType,
					"not supported."
				}));
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001A4A0 File Offset: 0x000194A0
		private static object CSharpPrimitiveCastUnchecked(TypeCode targetType, object input)
		{
			TypeCode typeCode = Type.GetTypeCode(input.GetType());
			if (typeCode == targetType)
			{
				return input;
			}
			switch (targetType)
			{
			case TypeCode.Char:
				switch (typeCode)
				{
				case TypeCode.SByte:
					return (char)((sbyte)input);
				case TypeCode.Byte:
					return (char)((byte)input);
				case TypeCode.Int16:
					return (char)((short)input);
				case TypeCode.UInt16:
					return (char)((ushort)input);
				case TypeCode.Int32:
					return (char)((int)input);
				case TypeCode.UInt32:
					return (char)((uint)input);
				case TypeCode.Int64:
					return (char)((long)input);
				case TypeCode.UInt64:
					return (char)((ulong)input);
				case TypeCode.Single:
					return (char)((float)input);
				case TypeCode.Double:
					return (char)((double)input);
				case TypeCode.Decimal:
					return (char)((decimal)input);
				}
				break;
			case TypeCode.SByte:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (sbyte)((char)input);
				case TypeCode.Byte:
					return (sbyte)((byte)input);
				case TypeCode.Int16:
					return (sbyte)((short)input);
				case TypeCode.UInt16:
					return (sbyte)((ushort)input);
				case TypeCode.Int32:
					return (sbyte)((int)input);
				case TypeCode.UInt32:
					return (sbyte)((uint)input);
				case TypeCode.Int64:
					return (sbyte)((long)input);
				case TypeCode.UInt64:
					return (sbyte)((ulong)input);
				case TypeCode.Single:
					return (sbyte)((float)input);
				case TypeCode.Double:
					return (sbyte)((double)input);
				case TypeCode.Decimal:
					return (sbyte)((decimal)input);
				}
				break;
			case TypeCode.Byte:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (byte)((char)input);
				case TypeCode.SByte:
					return (byte)((sbyte)input);
				case TypeCode.Int16:
					return (byte)((short)input);
				case TypeCode.UInt16:
					return (byte)((ushort)input);
				case TypeCode.Int32:
					return (byte)((int)input);
				case TypeCode.UInt32:
					return (byte)((uint)input);
				case TypeCode.Int64:
					return (byte)((long)input);
				case TypeCode.UInt64:
					return (byte)((ulong)input);
				case TypeCode.Single:
					return (byte)((float)input);
				case TypeCode.Double:
					return (byte)((double)input);
				case TypeCode.Decimal:
					return (byte)((decimal)input);
				}
				break;
			case TypeCode.Int16:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (short)((char)input);
				case TypeCode.SByte:
					return (short)((sbyte)input);
				case TypeCode.Byte:
					return (short)((byte)input);
				case TypeCode.UInt16:
					return (short)((ushort)input);
				case TypeCode.Int32:
					return (short)((int)input);
				case TypeCode.UInt32:
					return (short)((uint)input);
				case TypeCode.Int64:
					return (short)((long)input);
				case TypeCode.UInt64:
					return (short)((ulong)input);
				case TypeCode.Single:
					return (short)((float)input);
				case TypeCode.Double:
					return (short)((double)input);
				case TypeCode.Decimal:
					return (short)((decimal)input);
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (ushort)((char)input);
				case TypeCode.SByte:
					return (ushort)((sbyte)input);
				case TypeCode.Byte:
					return (ushort)((byte)input);
				case TypeCode.Int16:
					return (ushort)((short)input);
				case TypeCode.Int32:
					return (ushort)((int)input);
				case TypeCode.UInt32:
					return (ushort)((uint)input);
				case TypeCode.Int64:
					return (ushort)((long)input);
				case TypeCode.UInt64:
					return (ushort)((ulong)input);
				case TypeCode.Single:
					return (ushort)((float)input);
				case TypeCode.Double:
					return (ushort)((double)input);
				case TypeCode.Decimal:
					return (ushort)((decimal)input);
				}
				break;
			case TypeCode.Int32:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (int)((char)input);
				case TypeCode.SByte:
					return (int)((sbyte)input);
				case TypeCode.Byte:
					return (int)((byte)input);
				case TypeCode.Int16:
					return (int)((short)input);
				case TypeCode.UInt16:
					return (int)((ushort)input);
				case TypeCode.UInt32:
					return (int)((uint)input);
				case TypeCode.Int64:
					return (int)((long)input);
				case TypeCode.UInt64:
					return (int)((ulong)input);
				case TypeCode.Single:
					return (int)((float)input);
				case TypeCode.Double:
					return (int)((double)input);
				case TypeCode.Decimal:
					return (int)((decimal)input);
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (uint)((char)input);
				case TypeCode.SByte:
					return (uint)((sbyte)input);
				case TypeCode.Byte:
					return (uint)((byte)input);
				case TypeCode.Int16:
					return (uint)((short)input);
				case TypeCode.UInt16:
					return (uint)((ushort)input);
				case TypeCode.Int32:
					return (uint)((int)input);
				case TypeCode.Int64:
					return (uint)((long)input);
				case TypeCode.UInt64:
					return (uint)((ulong)input);
				case TypeCode.Single:
					return (uint)((float)input);
				case TypeCode.Double:
					return (uint)((double)input);
				case TypeCode.Decimal:
					return (uint)((decimal)input);
				}
				break;
			case TypeCode.Int64:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (long)((ulong)((char)input));
				case TypeCode.SByte:
					return (long)((sbyte)input);
				case TypeCode.Byte:
					return (long)((ulong)((byte)input));
				case TypeCode.Int16:
					return (long)((short)input);
				case TypeCode.UInt16:
					return (long)((ulong)((ushort)input));
				case TypeCode.Int32:
					return (long)((int)input);
				case TypeCode.UInt32:
					return (long)((ulong)((uint)input));
				case TypeCode.UInt64:
					return (long)((ulong)input);
				case TypeCode.Single:
					return (long)((float)input);
				case TypeCode.Double:
					return (long)((double)input);
				case TypeCode.Decimal:
					return (long)((decimal)input);
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (ulong)((char)input);
				case TypeCode.SByte:
					return (ulong)((long)((sbyte)input));
				case TypeCode.Byte:
					return (ulong)((byte)input);
				case TypeCode.Int16:
					return (ulong)((long)((short)input));
				case TypeCode.UInt16:
					return (ulong)((ushort)input);
				case TypeCode.Int32:
					return (ulong)((long)((int)input));
				case TypeCode.UInt32:
					return (ulong)((uint)input);
				case TypeCode.Int64:
					return (ulong)((long)input);
				case TypeCode.Single:
					return (ulong)((float)input);
				case TypeCode.Double:
					return (ulong)((double)input);
				case TypeCode.Decimal:
					return (ulong)((decimal)input);
				}
				break;
			case TypeCode.Single:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (float)((char)input);
				case TypeCode.SByte:
					return (float)((sbyte)input);
				case TypeCode.Byte:
					return (float)((byte)input);
				case TypeCode.Int16:
					return (float)((short)input);
				case TypeCode.UInt16:
					return (float)((ushort)input);
				case TypeCode.Int32:
					return (float)((int)input);
				case TypeCode.UInt32:
					return (uint)input;
				case TypeCode.Int64:
					return (float)((long)input);
				case TypeCode.UInt64:
					return (ulong)input;
				case TypeCode.Double:
					return (float)((double)input);
				case TypeCode.Decimal:
					return (float)((decimal)input);
				}
				break;
			case TypeCode.Double:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (double)((char)input);
				case TypeCode.SByte:
					return (double)((sbyte)input);
				case TypeCode.Byte:
					return (double)((byte)input);
				case TypeCode.Int16:
					return (double)((short)input);
				case TypeCode.UInt16:
					return (double)((ushort)input);
				case TypeCode.Int32:
					return (double)((int)input);
				case TypeCode.UInt32:
					return (uint)input;
				case TypeCode.Int64:
					return (double)((long)input);
				case TypeCode.UInt64:
					return (ulong)input;
				case TypeCode.Single:
					return (double)((float)input);
				case TypeCode.Decimal:
					return (double)((decimal)input);
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode)
				{
				case TypeCode.Char:
					return (char)input;
				case TypeCode.SByte:
					return (sbyte)input;
				case TypeCode.Byte:
					return (byte)input;
				case TypeCode.Int16:
					return (short)input;
				case TypeCode.UInt16:
					return (ushort)input;
				case TypeCode.Int32:
					return (int)input;
				case TypeCode.UInt32:
					return (uint)input;
				case TypeCode.Int64:
					return (long)input;
				case TypeCode.UInt64:
					return (ulong)input;
				case TypeCode.Single:
					return (decimal)((float)input);
				case TypeCode.Double:
					return (decimal)((double)input);
				}
				break;
			}
			throw new InvalidCastException(string.Concat(new object[]
			{
				"Cast from ",
				typeCode,
				" to ",
				targetType,
				" not supported."
			}));
		}
	}
}
