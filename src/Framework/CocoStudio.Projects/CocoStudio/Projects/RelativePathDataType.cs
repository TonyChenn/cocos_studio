using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000077 RID: 119
	public class RelativePathDataType : PrimitiveDataType
	{
		// Token: 0x06000394 RID: 916 RVA: 0x0000CC45 File Offset: 0x0000AE45
		public RelativePathDataType(Type type) : base(type)
		{
			if (type != typeof(string) && type != typeof(FilePath))
			{
				throw new InvalidOperationException("ProjectPathItemProperty can only be applied to fields of type string and FilePath");
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000CC80 File Offset: 0x0000AE80
		protected override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			FilePath filePath = (value is string) ? new FilePath((string)value) : ((FilePath)value);
			if (filePath.IsNullOrEmpty)
			{
				return null;
			}
			string text = filePath;
			if (Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
			{
				text = text.Replace(Path.DirectorySeparatorChar, serCtx.DirectorySeparatorChar);
			}
			return new DataValue(base.Name, text);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		protected override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			string text = ((DataValue)data).Value;
			if (!string.IsNullOrEmpty(text) && Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
			{
				text = text.Replace(serCtx.DirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			if (base.ValueType == typeof(string))
			{
				return text;
			}
			return (FilePath)text;
		}
	}
}
