using System;
using Newtonsoft.Json;

namespace CocoStudio.Model.ExtensionModel
{
	internal class FloatJsonConvert : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == this.floatType;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			return reader.Value;
		}

		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			float num = (float)value;
			writer.WriteValue(Math.Round((double)num, 4));
		}

		private Type floatType = typeof(float);
	}
}
