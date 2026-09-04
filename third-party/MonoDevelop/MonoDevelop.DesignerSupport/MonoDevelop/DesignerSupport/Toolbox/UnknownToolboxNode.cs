using System.Collections;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class UnknownToolboxNode : ItemToolboxNode, IExtendedDataItem
	{
		private Hashtable dictionary;

		public IDictionary ExtendedProperties
		{
			get
			{
				if (dictionary == null)
				{
					dictionary = new Hashtable();
				}
				return dictionary;
			}
		}
	}
}
