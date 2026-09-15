using System;
using System.Collections.Generic;
using GLib;
using Gtk;
using Xwt.GtkBackend;

namespace Gdk
{
	public static class KeyboardExtend
	{
		internal static Gtk.Window MainWindow
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

		public static bool IsKeyDown(Key key)
		{
			return KeyboardExtend.pressKeys.Contains(key);
		}

		public static bool IsModifyKeyPressed(ModifierType modifierKey)
		{
			ModifierType currentKeyModifiers = GtkWorkarounds.GetCurrentKeyModifiers();
			return currentKeyModifiers.HasFlag(modifierKey);
		}

		public static bool IsMousePressed(ModifierType modifierKey)
		{
			return modifierKey.HasFlag(ModifierType.Button1Mask) || modifierKey.HasFlag(ModifierType.Button3Mask);
		}

		private static void SetMainWindow(Gtk.Window oldWindow, Gtk.Window newWindow)
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

		[ConnectBefore]
		private static void HandleFocusOutEvent(object o, FocusOutEventArgs args)
		{
			KeyboardExtend.ForceClear();
		}

		[ConnectBefore]
		private static void HandleKeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			KeyboardExtend.pressKeys.Remove(args.Event.Key);
		}

		[ConnectBefore]
		private static void HandleKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (!KeyboardExtend.pressKeys.Contains(args.Event.Key))
			{
				KeyboardExtend.pressKeys.Add(args.Event.Key);
			}
		}

		public static void ForceClear()
		{
			KeyboardExtend.pressKeys.Clear();
		}

		public static bool IsEnterKey(Key key)
		{
			return key == global::Gdk.Key.ISO_Enter || key == global::Gdk.Key.Key_3270_Enter || key == global::Gdk.Key.KP_Enter || key == global::Gdk.Key.Return;
		}

		private static Gtk.Window mainWindow;

		private static HashSet<Key> pressKeys = new HashSet<Key>();
	}
}
