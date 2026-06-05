using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000087 RID: 135
	public static class ModifierTypeExtend
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
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
