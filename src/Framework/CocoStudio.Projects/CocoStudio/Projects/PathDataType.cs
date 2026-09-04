using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000075 RID: 117
	public class PathDataType : PrimitiveDataType
	{
		// Token: 0x0600038F RID: 911 RVA: 0x0000CAE3 File Offset: 0x0000ACE3
		public PathDataType(Type type) : base(type)
		{
			if (type != typeof(string) && type != typeof(FilePath))
			{
				throw new InvalidOperationException("ProjectPathItemProperty can only be applied to fields of type string and FilePath");
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		protected override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			FilePath filePath = (value is string) ? new FilePath((string)value) : ((FilePath)value);
			if (filePath.IsNullOrEmpty)
			{
				return null;
			}
			FilePath basePath = Path.GetDirectoryName(serCtx.BaseFile);
			string text = filePath.ToRelative(basePath);
			if (Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
			{
				text = text.Replace(Path.DirectorySeparatorChar, serCtx.DirectorySeparatorChar);
			}
			return new DataValue(base.Name, text);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		protected override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			string text = ((DataValue)data).Value;
			if (!string.IsNullOrEmpty(text))
			{
				if (Path.DirectorySeparatorChar != serCtx.DirectorySeparatorChar)
				{
					text = text.Replace(serCtx.DirectorySeparatorChar, Path.DirectorySeparatorChar);
				}
				string directoryName = Path.GetDirectoryName(serCtx.BaseFile);
				text = FileService.RelativeToAbsolutePath(directoryName, text);
			}
			if (base.ValueType == typeof(string))
			{
				return text;
			}
			return (FilePath)text;
		}
	}
}
