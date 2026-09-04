using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

internal static class GtkSharpSmoke
{
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            bool requireLocal = args.Length > 0 && args[0] == "local";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string name in new[] { "atk-sharp", "gdk-sharp", "glade-sharp", "glib-sharp", "gtk-dotnet", "gtk-sharp", "Mono.Cairo", "pango-sharp" })
            {
                Assembly assembly = Assembly.Load(AssemblyName.GetAssemblyName(Path.Combine(directory, name + ".dll")));
                Console.WriteLine("LOADED " + name + " " + assembly.Location);
                if (requireLocal && !assembly.Location.StartsWith(directory, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Candidate not used: " + assembly.Location);
            }
            Gtk.Application.Init();
            var button = new Gtk.Button("NuGet \u6d4b\u8bd5");
            var entry = new Gtk.Entry();
            entry.Text = "text \u4e2d\u6587";
            if (entry.Text != "text \u4e2d\u6587") throw new Exception("Entry UTF-8 mismatch");
            var buffer = new Gtk.TextBuffer(new Gtk.TextTagTable());
            buffer.Text = "line 1\nline 2";
            if (buffer.LineCount != 2) throw new Exception("TextBuffer line count mismatch");
            var store = new Gtk.ListStore(typeof(string));
            store.AppendValues("resource");
            var icons = new Gtk.IconView(store);
            icons.TextColumn = 0;
            icons.SelectPath(new Gtk.TreePath("0"));
            icons.ScrollToPath(new Gtk.TreePath("0"), 0.5f, 0.5f);
            Console.WriteLine("GTK_WIDGETS_TEXT_AND_ICONVIEW_OK");
            using (var pixbuf = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, true, 8, 4, 3))
            {
                pixbuf.Fill(0xff0000ff);
                if (pixbuf.Width != 4 || pixbuf.Height != 3) throw new Exception("Pixbuf mismatch");
            }
            using (var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, 4, 3))
            using (var context = new Cairo.Context(surface))
            {
                context.SetSourceRGB(1, 0, 0);
                context.Paint();
            }
            Console.WriteLine("GDK_PIXBUF_AND_CAIRO_OK");
            icons.Destroy();
            store.Dispose();
            buffer.Dispose();
            entry.Destroy();
            button.Destroy();
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                if (module.ModuleName.IndexOf("gtk", StringComparison.OrdinalIgnoreCase) >= 0 || module.ModuleName.IndexOf("sharpglue", StringComparison.OrdinalIgnoreCase) >= 0)
                    Console.WriteLine("NATIVE " + module.FileName);
            Console.WriteLine("GTK_SHARP_SMOKE_PASS");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
