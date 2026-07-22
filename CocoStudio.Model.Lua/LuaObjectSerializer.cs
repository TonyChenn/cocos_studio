using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua
{
	// Token: 0x02000004 RID: 4
	public abstract class LuaObjectSerializer : ILuaObjectSerializer
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002212 File Offset: 0x00000412
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002219 File Offset: 0x00000419
		public static StringBuilder TextWriter { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002221 File Offset: 0x00000421
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002228 File Offset: 0x00000428
		public static GameFileData GameFileData { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002230 File Offset: 0x00000430
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002237 File Offset: 0x00000437
		public static string FileName { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000223F File Offset: 0x0000043F
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002246 File Offset: 0x00000446
		protected static Dictionary<int, AbstractNodeObjectData> NodeCollection { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000224E File Offset: 0x0000044E
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002255 File Offset: 0x00000455
		protected static int ObjectCount { get; private set; }

		// Token: 0x06000012 RID: 18 RVA: 0x0000225D File Offset: 0x0000045D
		public static void Prepare(StringBuilder sb, GameFileData gameFileData, string fileName, int objectCount)
		{
			LuaObjectSerializer.NodeCollection = new Dictionary<int, AbstractNodeObjectData>();
			LuaObjectSerializer.TextWriter = sb;
			LuaObjectSerializer.GameFileData = gameFileData;
			LuaObjectSerializer.FileName = fileName;
			LuaObjectSerializer.ObjectCount = objectCount;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002281 File Offset: 0x00000481
		public static void Dispose()
		{
			LuaObjectSerializer.NodeCollection = null;
			LuaObjectSerializer.TextWriter = null;
			LuaObjectSerializer.GameFileData = null;
			LuaObjectSerializer.FileName = null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000229C File Offset: 0x0000049C
		public static AbstractNodeObjectData GetNode(int actionTag)
		{
			AbstractNodeObjectData result;
			LuaObjectSerializer.NodeCollection.TryGetValue(actionTag, out result);
			return result;
		}

		// Token: 0x06000015 RID: 21
		public abstract string TransformText();

		// Token: 0x06000016 RID: 22
		public abstract void CreateObject(BaseObjectData objectData);

		// Token: 0x06000017 RID: 23
		public abstract void InitializeObject(BaseObjectData objectData);

		// Token: 0x06000018 RID: 24
		public abstract bool CanSerialize(BaseObjectData objectData);

		// Token: 0x06000019 RID: 25
		public abstract void AddChild(BaseObjectData parent, BaseObjectData child);

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000022B8 File Offset: 0x000004B8
		public StringBuilder GenerationEnvironment
		{
			get
			{
				return LuaObjectSerializer.TextWriter;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000022BF File Offset: 0x000004BF
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000022DA File Offset: 0x000004DA
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000022F5 File Offset: 0x000004F5
		public string CurrentIndent
		{
			get
			{
				return this.currentIndentField;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000022FD File Offset: 0x000004FD
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002305 File Offset: 0x00000505
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

		// Token: 0x06000020 RID: 32 RVA: 0x00002310 File Offset: 0x00000510
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

		// Token: 0x06000021 RID: 33 RVA: 0x000023D7 File Offset: 0x000005D7
		public void WriteLine(string textToAppend)
		{
			this.Write(textToAppend);
			this.GenerationEnvironment.AppendLine();
			this.endsWithNewline = true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023F3 File Offset: 0x000005F3
		public void Write(string format, params object[] args)
		{
			this.Write(string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002407 File Offset: 0x00000607
		public void WriteLine(string format, params object[] args)
		{
			this.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000241C File Offset: 0x0000061C
		public void Error(string message)
		{
			CompilerError compilerError = new CompilerError();
			compilerError.ErrorText = message;
			this.Errors.Add(compilerError);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002444 File Offset: 0x00000644
		public void Warning(string message)
		{
			CompilerError compilerError = new CompilerError();
			compilerError.ErrorText = message;
			compilerError.IsWarning = true;
			this.Errors.Add(compilerError);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002472 File Offset: 0x00000672
		public void PushIndent(string indent)
		{
			if (indent == null)
			{
				throw new ArgumentNullException("indent");
			}
			this.currentIndentField += indent;
			this.indentLengths.Add(indent.Length);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024A8 File Offset: 0x000006A8
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

		// Token: 0x06000028 RID: 40 RVA: 0x00002536 File Offset: 0x00000736
		public void ClearIndent()
		{
			this.indentLengths.Clear();
			this.currentIndentField = "";
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000254E File Offset: 0x0000074E
		public LuaObjectSerializer.ToStringInstanceHelper ToStringHelper
		{
			get
			{
				return LuaObjectSerializer.toStringHelperField;
			}
		}

		// Token: 0x04000001 RID: 1
		private CompilerErrorCollection errorsField;

		// Token: 0x04000002 RID: 2
		private List<int> indentLengthsField;

		// Token: 0x04000003 RID: 3
		private string currentIndentField = "";

		// Token: 0x04000004 RID: 4
		private bool endsWithNewline;

		// Token: 0x04000005 RID: 5
		private IDictionary<string, object> sessionField;

		// Token: 0x04000006 RID: 6
		private static LuaObjectSerializer.ToStringInstanceHelper toStringHelperField = new LuaObjectSerializer.ToStringInstanceHelper();

		// Token: 0x02000005 RID: 5
		public class ToStringInstanceHelper
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600002C RID: 44 RVA: 0x00002574 File Offset: 0x00000774
			// (set) Token: 0x0600002D RID: 45 RVA: 0x0000257C File Offset: 0x0000077C
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

			// Token: 0x0600002E RID: 46 RVA: 0x00002588 File Offset: 0x00000788
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

			// Token: 0x0400000C RID: 12
			private IFormatProvider formatProviderField = new LuaDataFormatProvider();
		}
	}
}
