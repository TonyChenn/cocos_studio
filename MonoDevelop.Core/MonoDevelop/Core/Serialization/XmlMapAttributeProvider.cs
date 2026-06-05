using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Mono.Addins;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008C RID: 140
	public class XmlMapAttributeProvider : ISerializationAttributeProvider
	{
		// Token: 0x0600049C RID: 1180 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		public void AddMap(RuntimeAddin addin, string xmlMap, string fileId)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlMap);
			foreach (object obj in xmlDocument.DocumentElement.SelectNodes("DataItem"))
			{
				XmlElement xmlElement = (XmlElement)obj;
				string attribute = xmlElement.GetAttribute("class");
				Type type = addin.GetType(attribute);
				if (type == null)
				{
					LoggingService.LogError(string.Concat(new string[]
					{
						"[SerializationMap ",
						fileId,
						"] Type not found: '",
						attribute,
						"'"
					}));
				}
				else
				{
					string attribute2 = xmlElement.GetAttribute("name");
					string attribute3 = xmlElement.GetAttribute("fallbackType");
					SerializationMap serializationMap;
					if (!this.maps.TryGetValue(type, out serializationMap))
					{
						serializationMap = new SerializationMap(type);
						this.maps[type] = serializationMap;
						serializationMap.FileId = fileId;
						if (attribute2.Length > 0 || attribute3.Length > 0)
						{
							DataItemAttribute dataItemAttribute = new DataItemAttribute();
							if (attribute2.Length > 0)
							{
								dataItemAttribute.Name = attribute2;
							}
							if (attribute3.Length > 0)
							{
								dataItemAttribute.FallbackType = addin.GetType(attribute3, true);
							}
							serializationMap.TypeAttributes.Add(dataItemAttribute);
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(attribute2))
						{
							throw new InvalidOperationException(string.Format("Type name for type '{0}' in map '{1}' already specified in another serialization map for the same type ({2}).", type, fileId, serializationMap.FileId));
						}
						if (!string.IsNullOrEmpty(attribute3))
						{
							throw new InvalidOperationException(string.Format("Fallback type for type '{0}' in map '{1}' already specified in another serialization map for the same type ({2}).", type, fileId, serializationMap.FileId));
						}
					}
					string attribute4 = xmlElement.GetAttribute("customDataItem");
					if (attribute4.Length > 0)
					{
						ICustomDataItemHandler customDataItemHandler = (ICustomDataItemHandler)addin.CreateInstance(attribute4, true);
						if (serializationMap.CustomHandler != null)
						{
							serializationMap.CustomHandler = new CustomDataItemHandlerChain(serializationMap.CustomHandler, customDataItemHandler);
						}
						else
						{
							serializationMap.CustomHandler = customDataItemHandler;
						}
					}
					ItemMember itemMember = null;
					int num = 0;
					foreach (object obj2 in xmlElement.SelectNodes("ItemProperty|ExpandedCollection|LiteralProperty|ItemMember"))
					{
						XmlElement xmlElement2 = (XmlElement)obj2;
						ItemMember itemMember2 = itemMember;
						itemMember = null;
						if (xmlElement2.Name == "LiteralProperty")
						{
							ItemMember itemMember3 = new ItemMember();
							string text = itemMember3.Name = "_literal_" + ++num;
							itemMember3.Type = typeof(string);
							itemMember3.InitValue = xmlElement2.GetAttribute("value");
							itemMember3.DeclaringType = serializationMap.Type;
							serializationMap.ExtendedMembers.Add(itemMember3);
							ItemPropertyAttribute itemPropertyAttribute = new ItemPropertyAttribute();
							itemPropertyAttribute.Name = xmlElement2.GetAttribute("name");
							serializationMap.AddMemberAttribute(itemMember3, itemPropertyAttribute);
							itemMember = itemMember3;
						}
						else if (xmlElement2.Name == "ItemMember")
						{
							ItemMember itemMember4 = new ItemMember();
							string text2 = itemMember4.Name = xmlElement2.GetAttribute("name");
							itemMember4.Type = addin.GetType(xmlElement2.GetAttribute("type"), true);
							itemMember4.DeclaringType = serializationMap.Type;
							serializationMap.ExtendedMembers.Add(itemMember4);
							itemMember = itemMember4;
						}
						else
						{
							string attribute5 = xmlElement2.GetAttribute("member");
							object mi;
							Type type2;
							if (!this.FindMember(serializationMap, attribute5, out mi, out type2))
							{
								LoggingService.LogError(string.Concat(new string[]
								{
									"[SerializationMap ",
									fileId,
									"] Member '",
									attribute5,
									"' not found in type '",
									attribute,
									"'"
								}));
							}
							else
							{
								if (xmlElement2.Name == "ItemProperty")
								{
									ItemPropertyAttribute itemPropertyAttribute2 = new ItemPropertyAttribute();
									string attribute6 = xmlElement2.GetAttribute("name");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.Name = attribute6;
									}
									attribute6 = xmlElement2.GetAttribute("scope");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.Scope = attribute6;
									}
									if (xmlElement2.Attributes["defaultValue"] != null)
									{
										if (type2.IsEnum)
										{
											itemPropertyAttribute2.DefaultValue = Enum.Parse(type2, xmlElement2.GetAttribute("defaultValue"));
										}
										else
										{
											itemPropertyAttribute2.DefaultValue = Convert.ChangeType(xmlElement2.GetAttribute("defaultValue"), type2);
										}
									}
									attribute6 = xmlElement2.GetAttribute("serializationDataType");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.SerializationDataType = addin.GetType(attribute6, true);
									}
									attribute6 = xmlElement2.GetAttribute("valueType");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.ValueType = addin.GetType(attribute6, true);
									}
									attribute6 = xmlElement2.GetAttribute("readOnly");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.ReadOnly = bool.Parse(attribute6);
									}
									attribute6 = xmlElement2.GetAttribute("writeOnly");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.WriteOnly = bool.Parse(attribute6);
									}
									attribute6 = xmlElement2.GetAttribute("fallbackType");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.FallbackType = addin.GetType(attribute6, true);
									}
									attribute6 = xmlElement2.GetAttribute("isExternal");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.IsExternal = bool.Parse(attribute6);
									}
									attribute6 = xmlElement2.GetAttribute("skipEmpty");
									if (attribute6.Length > 0)
									{
										itemPropertyAttribute2.SkipEmpty = bool.Parse(attribute6);
									}
									serializationMap.AddMemberAttribute(mi, itemPropertyAttribute2);
								}
								else if (xmlElement2.Name == "ExpandedCollection")
								{
									ExpandedCollectionAttribute attribute7 = new ExpandedCollectionAttribute();
									serializationMap.AddMemberAttribute(mi, attribute7);
								}
								if (itemMember2 != null)
								{
									itemMember2.InsertBefore = attribute5;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public void RemoveMap(string fileId)
		{
			List<Type> list = new List<Type>();
			foreach (KeyValuePair<Type, SerializationMap> keyValuePair in this.maps)
			{
				if (keyValuePair.Value.FileId == fileId)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (Type key in list)
			{
				this.maps.Remove(key);
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000104AC File Offset: 0x0000E6AC
		private bool FindMember(SerializationMap map, string name, out object member, out Type memberType)
		{
			FieldInfo field = map.Type.GetField(name, this.bindingFlags);
			if (field != null)
			{
				memberType = field.FieldType;
				member = field;
				return true;
			}
			PropertyInfo property = map.Type.GetProperty(name, this.bindingFlags);
			if (property != null)
			{
				memberType = property.PropertyType;
				member = property;
				return true;
			}
			foreach (ItemMember itemMember in map.ExtendedMembers)
			{
				if (itemMember.Name == name)
				{
					member = itemMember;
					memberType = itemMember.Type;
					return true;
				}
			}
			member = null;
			memberType = null;
			return false;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00010578 File Offset: 0x0000E778
		public object GetCustomAttribute(object ob, Type type, bool inherit)
		{
			ArrayList arrayList;
			if (!this.GetAttributeList(ob, out arrayList))
			{
				return TypeAttributeProvider.Instance.GetCustomAttribute(ob, type, inherit);
			}
			if (arrayList == null)
			{
				return null;
			}
			foreach (object obj in arrayList)
			{
				if (type.IsInstanceOfType(obj))
				{
					return obj;
				}
			}
			return null;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000105F4 File Offset: 0x0000E7F4
		public object[] GetCustomAttributes(object ob, Type type, bool inherit)
		{
			ArrayList arrayList;
			if (!this.GetAttributeList(ob, out arrayList))
			{
				return TypeAttributeProvider.Instance.GetCustomAttributes(ob, type, inherit);
			}
			if (arrayList == null)
			{
				return new object[0];
			}
			ArrayList arrayList2 = new ArrayList();
			foreach (object obj in arrayList)
			{
				if (type.IsInstanceOfType(obj))
				{
					arrayList2.Add(obj);
				}
			}
			return arrayList2.ToArray();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00010680 File Offset: 0x0000E880
		public bool IsDefined(object ob, Type type, bool inherit)
		{
			ArrayList arrayList;
			if (!this.GetAttributeList(ob, out arrayList))
			{
				return TypeAttributeProvider.Instance.IsDefined(ob, type, inherit);
			}
			if (arrayList == null)
			{
				return false;
			}
			foreach (object o in arrayList)
			{
				if (type.IsInstanceOfType(o))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000106FC File Offset: 0x0000E8FC
		public ICustomDataItem GetCustomDataItem(object ob)
		{
			foreach (SerializationMap serializationMap in this.maps.Values)
			{
				if (serializationMap.Type.IsInstanceOfType(ob) && serializationMap.CustomHandler != null)
				{
					return new CustomDataItemWrapper(serializationMap.CustomHandler, ob);
				}
			}
			return TypeAttributeProvider.Instance.GetCustomDataItem(ob);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00010780 File Offset: 0x0000E980
		public ItemMember[] GetItemMembers(Type type)
		{
			SerializationMap serializationMap;
			if (!this.maps.TryGetValue(type, out serializationMap))
			{
				return new ItemMember[0];
			}
			return serializationMap.ExtendedMembers.ToArray();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000107B0 File Offset: 0x0000E9B0
		private bool GetAttributeList(object ob, out ArrayList list)
		{
			list = null;
			if (ob is Type)
			{
				SerializationMap serializationMap;
				if (!this.maps.TryGetValue((Type)ob, out serializationMap))
				{
					return false;
				}
				list = serializationMap.TypeAttributes;
			}
			else if (ob is MemberInfo)
			{
				MemberInfo memberInfo = (MemberInfo)ob;
				SerializationMap serializationMap2;
				if (!this.maps.TryGetValue(memberInfo.DeclaringType, out serializationMap2))
				{
					return false;
				}
				if (!serializationMap2.MemberMap.TryGetValue(memberInfo, out list))
				{
					return true;
				}
			}
			else if (ob is ItemMember)
			{
				ItemMember itemMember = (ItemMember)ob;
				SerializationMap serializationMap3;
				if (!this.maps.TryGetValue(itemMember.DeclaringType, out serializationMap3))
				{
					return false;
				}
				if (!serializationMap3.MemberMap.TryGetValue(itemMember, out list))
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x04000181 RID: 385
		private Dictionary<Type, SerializationMap> maps = new Dictionary<Type, SerializationMap>();

		// Token: 0x04000182 RID: 386
		private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
