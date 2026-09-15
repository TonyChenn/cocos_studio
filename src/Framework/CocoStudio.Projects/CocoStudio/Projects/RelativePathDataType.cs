using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public class RelativePathDataType : PrimitiveDataType
	{
		public RelativePathDataType(Type type) : base(type)
		{
			if (type != typeof(string) && type != typeof(FilePath))
			{
				throw new InvalidOperationException("ProjectPathItemProperty can only be applied to fields of type string and FilePath");
			}
		}

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
