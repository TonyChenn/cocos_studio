using System;
using System.IO;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000084 RID: 132
	internal class FilePathDataType : PrimitiveDataType
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x0000F104 File Offset: 0x0000D304
		public FilePathDataType() : base(typeof(FilePath))
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000F118 File Offset: 0x0000D318
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			string text = value.ToString();
			if (Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
			{
				text = text.Replace(Path.DirectorySeparatorChar, serCtx.DirectorySeparatorChar);
			}
			return new DataValue(base.Name, text);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000F158 File Offset: 0x0000D358
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			string text = ((DataValue)data).Value;
			if (!string.IsNullOrEmpty(text) && Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
			{
				text = text.Replace(serCtx.DirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			return text;
		}
	}
}
