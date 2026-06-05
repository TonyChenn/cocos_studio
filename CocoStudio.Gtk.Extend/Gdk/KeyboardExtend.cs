using System;
using System.Collections.Generic;
using GLib;
using Gtk;
using Xwt.GtkBackend;

namespace Gdk
{
	// Token: 0x02000082 RID: 130
	public static class KeyboardExtend
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000BB7C File Offset: 0x00009D7C
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000BB93 File Offset: 0x00009D93
		internal static Window MainWindow
		{
			get
			{
				return KeyboardExtend.mainWindow;
			}
			set
			{
				KeyboardExtend.SetMainWindow(KeyboardExtend.mainWindow, value);
				KeyboardExtend.mainWindow = value;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		public static bool IsKeyDown(Key key)
		{
			return KeyboardExtend.pressKeys.Contains(key);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000BBC8 File Offset: 0x00009DC8
		public static bool IsModifyKeyPressed(ModifierType modifierKey)
		{
			ModifierType currentKeyModifiers = GtkWorkarounds.GetCurrentKeyModifiers();
			return currentKeyModifiers.HasFlag(modifierKey);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public static bool IsMousePressed(ModifierType modifierKey)
		{
			return modifierKey.HasFlag(ModifierType.Button1Mask) || modifierKey.HasFlag(ModifierType.Button3Mask);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BC38 File Offset: 0x00009E38
		private static void SetMainWindow(Window oldWindow, Window newWindow)
		{
			if (oldWindow != null)
			{
				oldWindow.KeyPressEvent -= KeyboardExtend.HandleKeyPressEvent;
				oldWindow.KeyReleaseEvent -= KeyboardExtend.HandleKeyReleaseEvent;
				oldWindow.FocusOutEvent -= KeyboardExtend.HandleFocusOutEvent;
			}
			if (newWindow != null)
			{
				newWindow.KeyPressEvent += KeyboardExtend.HandleKeyPressEvent;
				newWindow.KeyReleaseEvent += KeyboardExtend.HandleKeyReleaseEvent;
				newWindow.FocusOutEvent += KeyboardExtend.HandleFocusOutEvent;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BCCC File Offset: 0x00009ECC
		[ConnectBefore]
		private static void HandleFocusOutEvent(object o, FocusOutEventArgs args)
		{
			KeyboardExtend.ForceClear();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BCD5 File Offset: 0x00009ED5
		[ConnectBefore]
		private static void HandleKeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			KeyboardExtend.pressKeys.Remove(args.Event.Key);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		[ConnectBefore]
		private static void HandleKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (!KeyboardExtend.pressKeys.Contains(args.Event.Key))
			{
				KeyboardExtend.pressKeys.Add(args.Event.Key);
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BD2F File Offset: 0x00009F2F
		public static void ForceClear()
		{
			KeyboardExtend.pressKeys.Clear();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000BD40 File Offset: 0x00009F40
		public static bool IsEnterKey(Key key)
		{
			return key == Key.ISO_Enter || key == Key.Key_3270_Enter || key == Key.KP_Enter || key == Key.Return;
		}

		// Token: 0x0400035B RID: 859
		private static Window mainWindow;

		// Token: 0x0400035C RID: 860
		private static HashSet<Key> pressKeys = new HashSet<Key>();
	}
}
