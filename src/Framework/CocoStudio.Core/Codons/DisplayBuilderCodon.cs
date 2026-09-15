using System;
using Mono.Addins;

namespace CocoStudio.Core.Codons
{
	public class DisplayBuilderCodon : TypeExtensionNode
	{
		public object Binding
		{
			get
			{
				return base.GetInstance();
			}
		}
	}
}
