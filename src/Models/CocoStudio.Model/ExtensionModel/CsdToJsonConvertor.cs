using System;
using System.IO;
using Newtonsoft.Json;

namespace CocoStudio.Model.ExtensionModel
{
	internal class CsdToJsonConvertor : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			ResourceItemData resourceItemData = (ResourceItemData)value;
			string path = Path.ChangeExtension(resourceItemData.Path, ".json");
			ResourceItemData value2 = new ResourceItemData(resourceItemData.Type, path, resourceItemData.Plist);
			serializer.Serialize(writer, value2);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			return reader.Value;
		}

		public override bool CanConvert(Type objectType)
		{
			return true;
		}
	}
}
