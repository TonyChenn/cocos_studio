using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000002 RID: 2
	public static class ArgumentValidator
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static T AssertNotNull<T>(T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			return value;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
		public static string AssertNotNullOrEmpty(string value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException("Argument should not be an empty string.", parameterName);
			}
			return value;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public static string AssertNotNullOrWhiteSpace(string value, string parameterName)
		{
			if (value == null || value.Trim().Length == 0)
			{
				throw new ArgumentException("Parameter should not be null or white space.", parameterName);
			}
			return value;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
		public static Guid AssertNotEmpty(Guid value, string parameterName)
		{
			if (value == Guid.Empty)
			{
				throw new ArgumentException("Parameter should not be an empty Guid.", parameterName);
			}
			return value;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000212C File Offset: 0x0000032C
		public static int AssertGreaterThan(int value, int expected, string parameterName)
		{
			if (value > expected)
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("Argument should be greater than " + expected, parameterName);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002164 File Offset: 0x00000364
		public static double AssertGreaterThan(double value, double expected, string parameterName)
		{
			if (value > expected)
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("Argument should be greater than " + expected, parameterName);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000219C File Offset: 0x0000039C
		public static int AssertGreaterThanOrEqual(int value, int expected, string parameterName)
		{
			if (value >= expected)
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("Argument should be greater than or equal to " + expected, parameterName);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021D0 File Offset: 0x000003D0
		public static double AssertGreaterThanOrEqual(double value, double expected, string parameterName)
		{
			if (value >= expected)
			{
				return value;
			}
			throw new ArgumentOutOfRangeException("Argument should be greater than or equal to " + expected, parameterName);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002204 File Offset: 0x00000404
		public static int AssertLessThan(int value, int expected, string parameterName)
		{
			if (value >= expected)
			{
				throw new ArgumentOutOfRangeException("Argument should be less than " + expected, parameterName);
			}
			return value;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002238 File Offset: 0x00000438
		public static double AssertLessThan(double value, double expected, string parameterName)
		{
			if (value >= expected)
			{
				throw new ArgumentOutOfRangeException("Argument should be less than " + expected, parameterName);
			}
			return value;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000226C File Offset: 0x0000046C
		public static int AssertLessThanOrEqual(int value, int expected, string parameterName)
		{
			if (value > expected)
			{
				throw new ArgumentOutOfRangeException("Argument should be less than or equal to " + expected, parameterName);
			}
			return value;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022A4 File Offset: 0x000004A4
		public static double AssertLessThanOrEqual(double value, double expected, string parameterName)
		{
			if (value > expected)
			{
				throw new ArgumentOutOfRangeException("Argument should be less than or equal to " + expected, parameterName);
			}
			return value;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022DC File Offset: 0x000004DC
		public static T AssertNotNullAndOfType<T>(object value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			T t = value as T;
			if (t == null)
			{
				throw new ArgumentException(string.Format(string.Concat(new object[]
				{
					"Expected argument of type ",
					typeof(T),
					", but was ",
					value.GetType()
				}), typeof(T), value.GetType()), parameterName);
			}
			return t;
		}
	}
}
