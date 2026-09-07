using System;
using System.Collections.Generic;
using System.Linq;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;

internal static class MonoTextEditorSmoke
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }

    private static void Result(string name, object value)
    {
        Console.WriteLine("RESULT " + name + "=" + value);
    }

    private static void TestDocument()
    {
        var document = new TextDocument("alpha\r\n中文\nlast");
        Check(document.LineCount == 3, "Line splitting changed");
        Check(document.GetLine(1).DelimiterLength == 2 && document.GetLine(2).DelimiterLength == 1, "Line delimiters changed");
        var location = document.OffsetToLocation(8);
        Check(location.Line == 2 && location.Column == 2 && document.LocationToOffset(location) == 8, "Location conversion changed");

        document.Insert(5, "!");
        document.Replace(0, 5, "A");
        document.Remove(document.TextLength - 4, 4);
        Check(document.Text == "A!\r\n中文\n", "Document editing changed");
        document.Undo();
        document.Undo();
        document.Undo();
        Check(document.Text == "alpha\r\n中文\nlast", "Undo changed");
        document.Redo();
        document.Redo();
        document.Redo();
        Check(document.Text == "A!\r\n中文\n", "Redo changed");

        using (document.OpenUndoGroup())
        {
            document.Insert(0, "[");
            document.Insert(document.TextLength, "]");
        }
        document.Undo();
        Check(document.Text == "A!\r\n中文\n", "Atomic undo changed");

        var fold = new FoldSegment(document, "body", 0, 4, FoldingType.Region) { IsFolded = true };
        document.UpdateFoldSegments(new List<FoldSegment> { fold });
        Check(document.HasFoldSegments && document.FoldSegments.Count() == 1 && document.FoldedSegments.Count() == 1, "Folding changed");
        Result("document", document.Text.Replace("\r", "<CR>").Replace("\n", "<LF>"));
        Result("lines", document.LineCount);
        Result("folds", document.FoldSegments.Count());
    }

    private static void TestEditorData()
    {
        using (var data = new TextEditorData())
        {
            data.Text = "one ONE one\r\nsecond";
            data.Caret.Offset = 0;
            data.SearchRequest.SearchPattern = "one";
            data.SearchRequest.CaseSensitive = false;
            var first = data.FindNext(false);
            Check(first != null && first.Offset == 0 && first.Length == 3, "Plain search changed");
            int replaced = data.SearchEngine.ReplaceAll("x");
            Check(replaced == 3 && data.Text == "x x x\r\nsecond", "Plain replacement changed");
            data.Document.Undo();
            Check(data.Text == "one ONE one\r\nsecond", "Search replacement undo changed");

            data.SearchEngine = new RegexSearchEngine();
            data.SearchRequest.SearchPattern = "o.e";
            data.SearchRequest.CaseSensitive = false;
            var regex = data.SearchEngine.SearchForward(0);
            Check(regex != null && regex.Offset == 0 && regex.Length == 3, "Regex search changed");

            data.MainSelection = new Selection(1, 1, 1, 4);
            Check(data.SelectedText == "one", "Selection changed");
            data.SelectedText = "ONE";
            Check(data.Text.StartsWith("ONE ONE one", StringComparison.Ordinal), "Selection replacement changed");
            Result("search", regex.Offset + ":" + regex.Length);
            Result("selection", data.SelectedText);
        }
    }

    private static void TestResources()
    {
        var document = new TextDocument("class C { }\n");
        var csharp = SyntaxModeService.GetSyntaxMode(document, "text/x-csharp");
        var lua = SyntaxModeService.GetSyntaxMode(document, "text/x-lua");
        Check(csharp != null && csharp.Name == "C#", "C# syntax resource changed");
        Check(lua != null && lua.Name == "Lua", "Lua syntax resource changed");
        var styles = SyntaxModeService.Styles.OrderBy(value => value, StringComparer.Ordinal).ToArray();
        Check(styles.Contains("Default") && styles.Contains("Monokai") && styles.Contains("Visual Studio"), "Style resources changed");
        Check(SyntaxModeService.GetColorStyle("Default") != null, "Default style failed to load");
        Result("syntax", csharp.Name + "," + lua.Name);
        Result("styles", string.Join(",", styles));
    }

    public static int Main()
    {
        try
        {
            TestDocument();
            TestEditorData();
            TestResources();
            Console.WriteLine("PASS Mono.TextEditor document, undo/redo, selection, search, folding and resources");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
