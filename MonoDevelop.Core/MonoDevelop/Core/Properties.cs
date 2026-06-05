using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MonoDevelop.Core
{
	// Token: 0x02000048 RID: 72
	public class Properties : ICustomXmlSerializer
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00008FD0 File Offset: 0x000071D0
		public ICollection<string> Keys
		{
			get
			{
				return this.properties.Keys;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009008 File Offset: 0x00007208
		private T Convert<T>(object o)
		{
			if (o is T)
			{
				return (T)((object)o);
			}
			if (o == null)
			{
				return (T)((object)o);
			}
			TypeConverter converter = this.GetConverter(typeof(T));
			if (o is string)
			{
				try
				{
					return (T)((object)converter.ConvertFromInvariantString(o.ToString()));
				}
				catch (Exception)
				{
					return default(T);
				}
			}
			T result;
			try
			{
				result = (T)((object)converter.ConvertFrom(o));
			}
			catch (Exception)
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000090A4 File Offset: 0x000072A4
		private string ConvertToString(object o)
		{
			if (o == null)
			{
				return null;
			}
			TypeConverter converter = this.GetConverter(o.GetType());
			return converter.ConvertToInvariantString(o);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000090CC File Offset: 0x000072CC
		private TypeConverter GetConverter(Type type)
		{
			TypeConverter converter;
			if (!this.cachedConverters.TryGetValue(type, out converter))
			{
				converter = TypeDescriptor.GetConverter(type);
				this.cachedConverters[type] = converter;
			}
			return converter;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009100 File Offset: 0x00007300
		public T Get<T>(string property, T defaultValue)
		{
			this.defaultValues[property] = defaultValue;
			object o;
			if (this.GetPropertyValue<T>(property, out o))
			{
				return this.Convert<T>(o);
			}
			this.properties[property] = defaultValue;
			return defaultValue;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009148 File Offset: 0x00007348
		public T Get<T>(string property)
		{
			object o;
			if (this.GetPropertyValue<T>(property, out o))
			{
				return this.Convert<T>(o);
			}
			if (this.defaultValues.TryGetValue(property, out o))
			{
				return this.Convert<T>(o);
			}
			return default(T);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000918C File Offset: 0x0000738C
		private bool GetPropertyValue<T>(string property, out object val)
		{
			if (this.properties.TryGetValue(property, out val))
			{
				if (val is Properties.LazyXmlDeserializer)
				{
					val = ((Properties.LazyXmlDeserializer)val).Deserialize<T>();
					this.properties[property] = val;
				}
				return true;
			}
			val = null;
			return false;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000091D8 File Offset: 0x000073D8
		public bool HasValue(string key)
		{
			return this.properties.ContainsKey(key);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000091E8 File Offset: 0x000073E8
		public void Set(string key, object val)
		{
			object obj = this.Get<object>(key);
			if (val == null)
			{
				if (obj == null)
				{
					return;
				}
				if (this.properties.ContainsKey(key))
				{
					this.properties.Remove(key);
				}
			}
			else
			{
				if (val.Equals(obj))
				{
					return;
				}
				this.properties[key] = val;
				if ((!val.GetType().IsClass || val is string) && this.defaultValues.ContainsKey(key) && this.defaultValues[key] == val)
				{
					this.properties.Remove(key);
				}
			}
			this.OnPropertyChanged(new PropertyChangedEventArgs(key, obj, val));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009285 File Offset: 0x00007485
		void ICustomXmlSerializer.WriteTo(XmlWriter writer)
		{
			this.Write(writer, false);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000928F File Offset: 0x0000748F
		ICustomXmlSerializer ICustomXmlSerializer.ReadFrom(XmlReader reader)
		{
			return Properties.Read(reader);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009297 File Offset: 0x00007497
		public void Write(XmlWriter writer)
		{
			this.Write(writer, true);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000092A4 File Offset: 0x000074A4
		public void Write(XmlWriter writer, bool createPropertyParent)
		{
			if (createPropertyParent)
			{
				writer.WriteStartElement("Properties");
			}
			foreach (KeyValuePair<string, object> keyValuePair in this.properties)
			{
				if (keyValuePair.Value != null)
				{
					writer.WriteStartElement("Property");
					writer.WriteAttributeString("key", keyValuePair.Key);
					if (keyValuePair.Value is Properties.LazyXmlDeserializer)
					{
						writer.WriteRaw(((Properties.LazyXmlDeserializer)keyValuePair.Value).Xml);
					}
					else if (keyValuePair.Value is ICustomXmlSerializer)
					{
						((ICustomXmlSerializer)keyValuePair.Value).WriteTo(writer);
					}
					else if (!(keyValuePair.Value is string) && keyValuePair.Value.GetType().IsClass)
					{
						XmlSerializer xmlSerializer = new XmlSerializer(keyValuePair.Value.GetType());
						xmlSerializer.Serialize(writer, keyValuePair.Value);
					}
					else
					{
						writer.WriteAttributeString("value", this.ConvertToString(keyValuePair.Value));
					}
					writer.WriteEndElement();
				}
			}
			if (createPropertyParent)
			{
				writer.WriteEndElement();
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000093E0 File Offset: 0x000075E0
		public void Save(string fileName)
		{
			string destFileName = fileName + ".previous";
			string text = string.Concat(new object[]
			{
				Path.GetDirectoryName(fileName),
				Path.DirectorySeparatorChar,
				".#",
				Path.GetFileName(fileName)
			});
			try
			{
				if (File.Exists(fileName))
				{
					File.Copy(fileName, destFileName, true);
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error copying properties file '{0}' to backup\n{1}", new object[]
				{
					fileName,
					ex
				});
			}
			try
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(text, Encoding.UTF8))
				{
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.WriteStartElement("MonoDevelopProperties");
					xmlTextWriter.WriteAttributeString("version", "2.0");
					this.Write(xmlTextWriter, false);
					xmlTextWriter.WriteEndElement();
				}
				FileService.SystemRename(text, fileName);
			}
			catch (Exception ex2)
			{
				LoggingService.LogError("Error writing properties file '{0}'\n{1}", new object[]
				{
					text,
					ex2
				});
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009590 File Offset: 0x00007790
		public static Properties Read(XmlReader reader)
		{
			Properties result = new Properties();
			XmlReadHelper.ReadList(reader, new string[]
			{
				"Properties",
				"Serialized",
				"MonoDevelopProperties"
			}, delegate()
			{
				string localName;
				if ((localName = reader.LocalName) != null && localName == "Property")
				{
					string attribute = reader.GetAttribute("key");
					if (!reader.IsEmptyElement)
					{
						result.Set(attribute, new Properties.LazyXmlDeserializer(reader.ReadInnerXml()));
					}
					else
					{
						result.Set(attribute, reader.GetAttribute("value"));
					}
					return true;
				}
				return false;
			});
			return result;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000095F4 File Offset: 0x000077F4
		public static Properties Load(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return null;
			}
			XmlReader xmlReader = XmlReader.Create(fileName);
			try
			{
				while (xmlReader.Read())
				{
					string localName;
					if (xmlReader.IsStartElement() && (localName = xmlReader.LocalName) != null && localName == "MonoDevelopProperties" && xmlReader.GetAttribute("version") == "2.0")
					{
						return Properties.Read(xmlReader);
					}
				}
			}
			finally
			{
				xmlReader.Close();
			}
			return null;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009678 File Offset: 0x00007878
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Properties:");
			foreach (KeyValuePair<string, object> keyValuePair in this.properties)
			{
				stringBuilder.Append(keyValuePair.Key);
				stringBuilder.Append("=");
				stringBuilder.Append(keyValuePair.Value);
				stringBuilder.Append(",");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000971C File Offset: 0x0000791C
		public Properties Clone()
		{
			return new Properties
			{
				properties = new Dictionary<string, object>(this.properties)
			};
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009744 File Offset: 0x00007944
		public void AddPropertyHandler(string propertyName, EventHandler<PropertyChangedEventArgs> handler)
		{
			if (this.propertyListeners == null)
			{
				this.propertyListeners = new Dictionary<string, EventHandler<PropertyChangedEventArgs>>();
			}
			EventHandler<PropertyChangedEventArgs> a = null;
			this.propertyListeners.TryGetValue(propertyName, out a);
			this.propertyListeners[propertyName] = (EventHandler<PropertyChangedEventArgs>)Delegate.Combine(a, handler);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009790 File Offset: 0x00007990
		public void RemovePropertyHandler(string propertyName, EventHandler<PropertyChangedEventArgs> handler)
		{
			if (this.propertyListeners == null)
			{
				return;
			}
			EventHandler<PropertyChangedEventArgs> eventHandler = null;
			this.propertyListeners.TryGetValue(propertyName, out eventHandler);
			eventHandler = (EventHandler<PropertyChangedEventArgs>)Delegate.Remove(eventHandler, handler);
			if (eventHandler != null)
			{
				this.propertyListeners[propertyName] = eventHandler;
				return;
			}
			this.propertyListeners.Remove(propertyName);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x000097E4 File Offset: 0x000079E4
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, args);
			}
			if (this.propertyListeners != null)
			{
				EventHandler<PropertyChangedEventArgs> eventHandler = null;
				this.propertyListeners.TryGetValue(args.Key, out eventHandler);
				if (eventHandler != null)
				{
					eventHandler(this, args);
				}
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000257 RID: 599 RVA: 0x00009830 File Offset: 0x00007A30
		// (remove) Token: 0x06000258 RID: 600 RVA: 0x00009868 File Offset: 0x00007A68
		public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		// Token: 0x040000CC RID: 204
		public const string Node = "Properties";

		// Token: 0x040000CD RID: 205
		public const string SerializedNode = "Serialized";

		// Token: 0x040000CE RID: 206
		public const string PropertyNode = "Property";

		// Token: 0x040000CF RID: 207
		public const string KeyAttribute = "key";

		// Token: 0x040000D0 RID: 208
		public const string ValueAttribute = "value";

		// Token: 0x040000D1 RID: 209
		public const string XmlSerialized = "XmlSerialized";

		// Token: 0x040000D2 RID: 210
		public const string PropertiesRootNode = "MonoDevelopProperties";

		// Token: 0x040000D3 RID: 211
		public const string PropertiesVersionAttribute = "version";

		// Token: 0x040000D4 RID: 212
		public const string PropertiesVersion = "2.0";

		// Token: 0x040000D5 RID: 213
		private Dictionary<string, object> properties = new Dictionary<string, object>();

		// Token: 0x040000D6 RID: 214
		private Dictionary<string, object> defaultValues = new Dictionary<string, object>();

		// Token: 0x040000D7 RID: 215
		private Dictionary<Type, TypeConverter> cachedConverters = new Dictionary<Type, TypeConverter>();

		// Token: 0x040000D8 RID: 216
		private Dictionary<string, EventHandler<PropertyChangedEventArgs>> propertyListeners;

		// Token: 0x02000049 RID: 73
		private class LazyXmlDeserializer
		{
			// Token: 0x17000073 RID: 115
			// (get) Token: 0x06000259 RID: 601 RVA: 0x0000989D File Offset: 0x00007A9D
			public string Xml
			{
				get
				{
					return this.xml;
				}
			}

			// Token: 0x0600025A RID: 602 RVA: 0x000098A5 File Offset: 0x00007AA5
			public LazyXmlDeserializer(string xml)
			{
				this.xml = xml;
			}

			// Token: 0x0600025B RID: 603 RVA: 0x000098B4 File Offset: 0x00007AB4
			public T Deserialize<T>()
			{
				T result;
				try
				{
					if (typeof(ICustomXmlSerializer).IsAssignableFrom(typeof(T)))
					{
						using (XmlReader xmlReader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes("<Serialized>" + this.xml + "</Serialized>"))))
						{
							return (T)((object)((ICustomXmlSerializer)typeof(T).Assembly.CreateInstance(typeof(T).FullName)).ReadFrom(xmlReader));
						}
					}
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					using (StreamReader streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(this.xml))))
					{
						result = (T)((object)xmlSerializer.Deserialize(streamReader));
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogWarning("Caught exception while deserializing:" + typeof(T), ex);
					result = default(T);
				}
				return result;
			}

			// Token: 0x040000DA RID: 218
			private string xml;
		}
	}
}
