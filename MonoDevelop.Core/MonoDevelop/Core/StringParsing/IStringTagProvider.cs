using System;
using System.Collections.Generic;
using Mono.Addins;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000111 RID: 273
	[TypeExtensionPoint(Name = "String providers")]
	public interface IStringTagProvider
	{
		/// <summary>
		/// Returns a list of tags that this provider can extract from objects
		/// of the provided type. If the provided type can be null, in which
		/// case it must return global tags (that is, tags which are not attached
		/// to a particular object).
		/// </summary>
		// Token: 0x06000A63 RID: 2659
		IEnumerable<StringTagDescription> GetTags(Type type);

		/// <summary>
		/// Returns the value of a tag. The instance is an object of a type supported
		/// by the provider, that is, the GetTags method returned tags for the object type.
		/// </summary>
		// Token: 0x06000A64 RID: 2660
		object GetTagValue(object instance, string tag);
	}
}
