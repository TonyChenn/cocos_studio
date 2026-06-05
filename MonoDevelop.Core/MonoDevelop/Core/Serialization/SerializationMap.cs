using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008D RID: 141
	internal class SerializationMap
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x00010859 File Offset: 0x0000EA59
		public SerializationMap(Type type)
		{
			this.Type = type;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001088C File Offset: 0x0000EA8C
		public void AddMemberAttribute(object mi, object attribute)
		{
			ArrayList arrayList;
			if (!this.MemberMap.TryGetValue(mi, out arrayList))
			{
				arrayList = new ArrayList();
				this.MemberMap[mi] = arrayList;
			}
			arrayList.Add(attribute);
		}

		// Token: 0x04000183 RID: 387
		public Type Type;

		// Token: 0x04000184 RID: 388
		public Dictionary<object, ArrayList> MemberMap = new Dictionary<object, ArrayList>();

		// Token: 0x04000185 RID: 389
		public ArrayList TypeAttributes = new ArrayList();

		// Token: 0x04000186 RID: 390
		public List<ItemMember> ExtendedMembers = new List<ItemMember>();

		// Token: 0x04000187 RID: 391
		public ICustomDataItemHandler CustomHandler;

		// Token: 0x04000188 RID: 392
		public string FileId;
	}
}
