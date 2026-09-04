using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.Debugger
{
	internal class AsmLineMarker : TextLineMarker
	{
		public override ChunkStyle GetStyle(ChunkStyle baseStyle)
		{
			ChunkStyle chunkStyle = new ChunkStyle(baseStyle);
			chunkStyle.Foreground = new Color(125.0, 125.0, 125.0);
			return chunkStyle;
		}
	}
}
