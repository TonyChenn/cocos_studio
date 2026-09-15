using System;
using Gdk;

namespace Gtk
{
	public static class ModifierTypeExtend
	{
		public static MouseButton GetMouseButton(this ModifierType modifierType)
		{
			MouseButton result = MouseButton.None;
			if (modifierType.HasFlag(ModifierType.Button1Mask))
			{
				result = MouseButton.Left;
			}
			else if (modifierType.HasFlag(ModifierType.Button2Mask))
			{
				result = MouseButton.Middle;
			}
			else if (modifierType.HasFlag(ModifierType.Button3Mask))
			{
				result = MouseButton.Right;
			}
			return result;
		}
	}
}
