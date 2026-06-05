using System;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Holds the name of a top-level type.
	/// This struct cannot refer to nested classes.
	/// </summary>
	// Token: 0x020000FA RID: 250
	[Serializable]
	public struct TopLevelTypeName : IEquatable<TopLevelTypeName>
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x00018AA0 File Offset: 0x00017AA0
		public TopLevelTypeName(string namespaceName, string name, int typeParameterCount = 0)
		{
			if (namespaceName == null)
			{
				throw new ArgumentNullException("namespaceName");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.namespaceName = namespaceName;
			this.name = name;
			this.typeParameterCount = typeParameterCount;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00018AD4 File Offset: 0x00017AD4
		public TopLevelTypeName(string reflectionName)
		{
			int num = reflectionName.LastIndexOf('.');
			if (num < 0)
			{
				this.namespaceName = string.Empty;
				this.name = reflectionName;
			}
			else
			{
				this.namespaceName = reflectionName.Substring(0, num);
				this.name = reflectionName.Substring(num + 1);
			}
			this.name = ReflectionHelper.SplitTypeParameterCountFromReflectionName(this.name, out this.typeParameterCount);
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00018B36 File Offset: 0x00017B36
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x00018B3E File Offset: 0x00017B3E
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00018B46 File Offset: 0x00017B46
		public int TypeParameterCount
		{
			get
			{
				return this.typeParameterCount;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x00018B50 File Offset: 0x00017B50
		public string ReflectionName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(this.namespaceName))
				{
					stringBuilder.Append(this.namespaceName);
					stringBuilder.Append('.');
				}
				stringBuilder.Append(this.name);
				if (this.typeParameterCount > 0)
				{
					stringBuilder.Append('`');
					stringBuilder.Append(this.typeParameterCount);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00018BB8 File Offset: 0x00017BB8
		public override string ToString()
		{
			return this.ReflectionName;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00018BC0 File Offset: 0x00017BC0
		public override bool Equals(object obj)
		{
			return obj is TopLevelTypeName && this.Equals((TopLevelTypeName)obj);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00018BD8 File Offset: 0x00017BD8
		public bool Equals(TopLevelTypeName other)
		{
			return this.namespaceName == other.namespaceName && this.name == other.name && this.typeParameterCount == other.typeParameterCount;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00018C13 File Offset: 0x00017C13
		public override int GetHashCode()
		{
			return ((this.name != null) ? this.name.GetHashCode() : 0) ^ ((this.namespaceName != null) ? this.namespaceName.GetHashCode() : 0) ^ this.typeParameterCount;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00018C49 File Offset: 0x00017C49
		public static bool operator ==(TopLevelTypeName lhs, TopLevelTypeName rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00018C53 File Offset: 0x00017C53
		public static bool operator !=(TopLevelTypeName lhs, TopLevelTypeName rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x040002F1 RID: 753
		private readonly string namespaceName;

		// Token: 0x040002F2 RID: 754
		private readonly string name;

		// Token: 0x040002F3 RID: 755
		private readonly int typeParameterCount;
	}
}
