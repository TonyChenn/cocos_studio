using System;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Specifies the version of the class.
	/// The <see cref="T:ICSharpCode.NRefactory.Utils.FastSerializer" /> will refuse to deserialize an instance that was stored by
	/// a different version of the class than the current one.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
	public class FastSerializerVersionAttribute : Attribute
	{
		public FastSerializerVersionAttribute(int versionNumber)
		{
			this.versionNumber = versionNumber;
		}

		public int VersionNumber
		{
			get
			{
				return this.versionNumber;
			}
		}

		internal static int GetVersionNumber(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(FastSerializerVersionAttribute), false);
			if (customAttributes.Length == 0)
			{
				return 0;
			}
			return ((FastSerializerVersionAttribute)customAttributes[0]).VersionNumber;
		}

		private readonly int versionNumber;
	}
}
