using System;
using Newtonsoft.Json;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x0200007D RID: 125
	internal class FloatJsonConvert : JsonConverter
	{
		// Token: 0x0600046C RID: 1132 RVA: 0x000136A8 File Offset: 0x000118A8
		public override bool CanConvert(Type objectType)
		{
			return objectType == this.floatType;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000136C8 File Offset: 0x000118C8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			return reader.Value;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000136E0 File Offset: 0x000118E0
		public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			float num = (float)value;
			writer.WriteValue(Math.Round((double)num, 4));
		}

		// Token: 0x04000221 RID: 545
		private Type floatType = typeof(float);
	}
}
