using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000104 RID: 260
	public class RelativePathDataType : PrimitiveDataType
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x000257DD File Offset: 0x000239DD
		public RelativePathDataType(Type type) : base(type)
		{
			if (type != typeof(string) && type != typeof(FilePath))
			{
				throw new InvalidOperationException("ProjectPathItemProperty can only be applied to fields of type string and FilePath");
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00025818 File Offset: 0x00023A18
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
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

		// Token: 0x06000962 RID: 2402 RVA: 0x00025880 File Offset: 0x00023A80
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
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
			return text;
		}
	}
}
