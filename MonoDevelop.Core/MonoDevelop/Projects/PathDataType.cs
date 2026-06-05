using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000102 RID: 258
	public class PathDataType : PrimitiveDataType
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x0002567A File Offset: 0x0002387A
		public PathDataType(Type type) : base(type)
		{
			if (type != typeof(string) && type != typeof(FilePath))
			{
				throw new InvalidOperationException("ProjectPathItemProperty can only be applied to fields of type string and FilePath");
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000256B4 File Offset: 0x000238B4
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
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

		// Token: 0x0600095D RID: 2397 RVA: 0x00025734 File Offset: 0x00023934
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
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
			return text;
		}
	}
}
