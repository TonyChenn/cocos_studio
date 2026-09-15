using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua
{
	public abstract class LuaObjectSerializer : ILuaObjectSerializer
	{
		public static StringBuilder TextWriter { get; private set; }

		public static GameFileData GameFileData { get; private set; }

		public static string FileName { get; private set; }

		protected static Dictionary<int, AbstractNodeObjectData> NodeCollection { get; private set; }

		protected static int ObjectCount { get; private set; }

		public static void Prepare(StringBuilder sb, GameFileData gameFileData, string fileName, int objectCount)
		{
			LuaObjectSerializer.NodeCollection = new Dictionary<int, AbstractNodeObjectData>();
			LuaObjectSerializer.TextWriter = sb;
			LuaObjectSerializer.GameFileData = gameFileData;
			LuaObjectSerializer.FileName = fileName;
			LuaObjectSerializer.ObjectCount = objectCount;
		}

		public static void Dispose()
		{
			LuaObjectSerializer.NodeCollection = null;
			LuaObjectSerializer.TextWriter = null;
			LuaObjectSerializer.GameFileData = null;
			LuaObjectSerializer.FileName = null;
		}

		public static AbstractNodeObjectData GetNode(int actionTag)
		{
			AbstractNodeObjectData result;
			LuaObjectSerializer.NodeCollection.TryGetValue(actionTag, out result);
			return result;
		}

		public abstract string TransformText();

		public abstract void CreateObject(BaseObjectData objectData);

		public abstract void InitializeObject(BaseObjectData objectData);

		public abstract bool CanSerialize(BaseObjectData objectData);

		public abstract void AddChild(BaseObjectData parent, BaseObjectData child);

		public StringBuilder GenerationEnvironment
		{
			get
			{
				return LuaObjectSerializer.TextWriter;
			}
		}

		public CompilerErrorCollection Errors
		{
			get
			{
				if (this.errorsField == null)
				{
					this.errorsField = new CompilerErrorCollection();
				}
				return this.errorsField;
			}
		}

		private List<int> indentLengths
		{
			get
			{
				if (this.indentLengthsField == null)
				{
					this.indentLengthsField = new List<int>();
				}
				return this.indentLengthsField;
			}
		}

		public string CurrentIndent
		{
			get
			{
				return this.currentIndentField;
			}
		}

		public virtual IDictionary<string, object> Session
		{
			get
			{
				return this.sessionField;
			}
			set
			{
				this.sessionField = value;
			}
		}

		public void Write(string textToAppend)
		{
			if (string.IsNullOrEmpty(textToAppend))
			{
				return;
			}
			if (this.GenerationEnvironment.Length == 0 || this.endsWithNewline)
			{
				this.GenerationEnvironment.Append(this.currentIndentField);
				this.endsWithNewline = false;
			}
			if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
			{
				this.endsWithNewline = true;
			}
			if (this.currentIndentField.Length == 0)
			{
				this.GenerationEnvironment.Append(textToAppend);
				return;
			}
			textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + this.currentIndentField);
			if (this.endsWithNewline)
			{
				this.GenerationEnvironment.Append(textToAppend, 0, textToAppend.Length - this.currentIndentField.Length);
				return;
			}
			this.GenerationEnvironment.Append(textToAppend);
		}

		public void WriteLine(string textToAppend)
		{
			this.Write(textToAppend);
			this.GenerationEnvironment.AppendLine();
			this.endsWithNewline = true;
		}

		public void Write(string format, params object[] args)
		{
			this.Write(string.Format(CultureInfo.CurrentCulture, format, args));
		}

		public void WriteLine(string format, params object[] args)
		{
			this.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
		}

		public void Error(string message)
		{
			CompilerError compilerError = new CompilerError();
			compilerError.ErrorText = message;
			this.Errors.Add(compilerError);
		}

		public void Warning(string message)
		{
			CompilerError compilerError = new CompilerError();
			compilerError.ErrorText = message;
			compilerError.IsWarning = true;
			this.Errors.Add(compilerError);
		}

		public void PushIndent(string indent)
		{
			if (indent == null)
			{
				throw new ArgumentNullException("indent");
			}
			this.currentIndentField += indent;
			this.indentLengths.Add(indent.Length);
		}

		public string PopIndent()
		{
			string result = "";
			if (this.indentLengths.Count > 0)
			{
				int num = this.indentLengths[this.indentLengths.Count - 1];
				this.indentLengths.RemoveAt(this.indentLengths.Count - 1);
				if (num > 0)
				{
					result = this.currentIndentField.Substring(this.currentIndentField.Length - num);
					this.currentIndentField = this.currentIndentField.Remove(this.currentIndentField.Length - num);
				}
			}
			return result;
		}

		public void ClearIndent()
		{
			this.indentLengths.Clear();
			this.currentIndentField = "";
		}

		public LuaObjectSerializer.ToStringInstanceHelper ToStringHelper
		{
			get
			{
				return LuaObjectSerializer.toStringHelperField;
			}
		}

		private CompilerErrorCollection errorsField;

		private List<int> indentLengthsField;

		private string currentIndentField = "";

		private bool endsWithNewline;

		private IDictionary<string, object> sessionField;

		private static LuaObjectSerializer.ToStringInstanceHelper toStringHelperField = new LuaObjectSerializer.ToStringInstanceHelper();

		public class ToStringInstanceHelper
		{
			public IFormatProvider FormatProvider
			{
				get
				{
					return this.formatProviderField;
				}
				set
				{
					if (value != null)
					{
						this.formatProviderField = value;
					}
				}
			}

			public string ToStringWithCulture(object objectToConvert)
			{
				if (objectToConvert == null)
				{
					throw new ArgumentNullException("objectToConvert");
				}
				return string.Format(this.formatProviderField, "{0}", new object[]
				{
					objectToConvert
				});
			}

			private IFormatProvider formatProviderField = new LuaDataFormatProvider();
		}
	}
}
