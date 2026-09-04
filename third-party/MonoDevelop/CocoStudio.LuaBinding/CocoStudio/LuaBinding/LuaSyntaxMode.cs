using System;
using System.Collections.Generic;
using System.IO;
using Mono.TextEditor.Highlighting;

namespace CocoStudio.LuaBinding
{
	internal class LuaSyntaxMode : SyntaxMode
	{
		private string GetSyntaxMode()
		{
			if (base.Document == null)
			{
				return "LuaSyntaxMode.xml";
			}
			return "LuaSyntaxMode.xml";
		}

		public LuaSyntaxMode()
		{
			EventHandler value = delegate
			{
				if (base.Document != null)
				{
					base.Document.FileNameChanged += delegate
					{
						ResourceStreamProvider resourceStreamProvider = new ResourceStreamProvider(typeof(LuaSyntaxMode).Assembly, GetSyntaxMode());
						Stream stream = resourceStreamProvider.Open();
						SyntaxMode syntaxMode = SyntaxMode.Read(stream);
						rules = new List<Rule>(syntaxMode.Rules);
						keywords = new List<Keywords>(syntaxMode.Keywords);
						spans = syntaxMode.Spans;
						matches = syntaxMode.Matches;
						prevMarker = syntaxMode.PrevMarker;
						SemanticRules = syntaxMode.SemanticRules;
						keywordTable = syntaxMode.keywordTable;
						properties = syntaxMode.Properties;
					};
				}
			};
			base.DocumentSet += value;
		}
	}
}
