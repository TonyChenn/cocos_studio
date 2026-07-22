using System;
using System.IO;
using Newtonsoft.Json;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x0200007C RID: 124
	internal class CsdToJsonConvertor : JsonConverter
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x00013630 File Offset: 0x00011830
		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			ResourceItemData resourceItemData = (ResourceItemData)value;
			string path = Path.ChangeExtension(resourceItemData.Path, ".json");
			ResourceItemData value2 = new ResourceItemData(resourceItemData.Type, path, resourceItemData.Plist);
			serializer.Serialize(writer, value2);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00013674 File Offset: 0x00011874
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			return reader.Value;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001368C File Offset: 0x0001188C
		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
