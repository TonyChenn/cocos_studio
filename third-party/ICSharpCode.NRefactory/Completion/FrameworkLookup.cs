using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.Completion
{
	/// <summary>
	/// The framework lookup provides a fast lookup where an unknow type or extension method may be defined in.
	/// </summary>
	// Token: 0x02000141 RID: 321
	public sealed class FrameworkLookup
	{
		/// <summary>
		/// This method tries to get a matching extension method.
		/// </summary>
		/// <returns>The extension method lookups.</returns>
		/// <param name="resolveResult">The resolve result.</param>
		// Token: 0x06000B02 RID: 2818 RVA: 0x00021420 File Offset: 0x00020420
		public IEnumerable<FrameworkLookup.AssemblyLookup> GetExtensionMethodLookups(UnknownMemberResolveResult resolveResult)
		{
			return this.GetLookup(resolveResult.MemberName, this.extLookupTable, 15 + this.assemblyListTable.Length * 4 + this.typeLookupTable.Length * 8);
		}

		/// <summary>
		/// Tries to get a type out of an unknow identifier result.
		/// </summary>
		/// <returns>The assemblies the type may be defined (if any).</returns>
		/// <param name="resolveResult">The resolve result.</param>
		/// <param name="typeParameterCount">Type parameter count.</param>
		/// <param name="isInsideAttributeType">If set to <c>true</c> this resolve result may be inside an attribute.</param>
		// Token: 0x06000B03 RID: 2819 RVA: 0x0002144C File Offset: 0x0002044C
		public IEnumerable<FrameworkLookup.AssemblyLookup> GetLookups(UnknownIdentifierResolveResult resolveResult, int typeParameterCount, bool isInsideAttributeType)
		{
			string identifier = isInsideAttributeType ? (resolveResult.Identifier + "Attribute") : resolveResult.Identifier;
			string identifier2 = FrameworkLookup.GetIdentifier(identifier, typeParameterCount);
			return this.GetLookup(identifier2, this.typeLookupTable, 15 + this.assemblyListTable.Length * 4);
		}

		/// <summary>
		/// Tries to get a type out of an unknow member resolve result. (In case of fully qualified names)
		/// </summary>
		/// <returns>The assemblies the type may be defined (if any).</returns>
		/// <param name="resolveResult">The resolve result.</param>
		/// <param name="fullMemberName"></param>
		/// <param name="typeParameterCount">Type parameter count.</param>
		/// <param name="isInsideAttributeType">If set to <c>true</c> this resolve result may be inside an attribute.</param>
		// Token: 0x06000B04 RID: 2820 RVA: 0x000216F4 File Offset: 0x000206F4
		public IEnumerable<FrameworkLookup.AssemblyLookup> GetLookups(UnknownMemberResolveResult resolveResult, string fullMemberName, int typeParameterCount, bool isInsideAttributeType)
		{
			string name = isInsideAttributeType ? (resolveResult.MemberName + "Attribute") : resolveResult.MemberName;
			string identifier = FrameworkLookup.GetIdentifier(name, typeParameterCount);
			foreach (FrameworkLookup.AssemblyLookup lookup in this.GetLookup(identifier, this.typeLookupTable, 15 + this.assemblyListTable.Length * 4))
			{
				FrameworkLookup.AssemblyLookup assemblyLookup = lookup;
				if (fullMemberName.StartsWith(assemblyLookup.Namespace, StringComparison.Ordinal))
				{
					yield return lookup;
				}
			}
			yield break;
		}

		/// <summary>
		/// This method returns a new framework builder to build a new framework lookup data file.
		/// </summary>
		/// <param name="fileName">The file name of the data file.</param>
		// Token: 0x06000B05 RID: 2821 RVA: 0x0002172E File Offset: 0x0002072E
		public static FrameworkLookup.FrameworkBuilder Create(string fileName)
		{
			return new FrameworkLookup.FrameworkBuilder(fileName);
		}

		/// <summary>
		/// Loads a framework lookup object from a file. May return null, if the file wasn't found or has a version mismatch.
		/// </summary>
		/// <param name="fileName">File name.</param>
		// Token: 0x06000B06 RID: 2822 RVA: 0x00021738 File Offset: 0x00020738
		public static FrameworkLookup Load(string fileName)
		{
			try
			{
				if (!File.Exists(fileName))
				{
					return null;
				}
			}
			catch (Exception)
			{
				return null;
			}
			FrameworkLookup frameworkLookup = new FrameworkLookup();
			frameworkLookup.fileName = fileName;
			FileStream input = File.OpenRead(fileName);
			using (BinaryReader binaryReader = new BinaryReader(input, Encoding.UTF8))
			{
				byte major = binaryReader.ReadByte();
				byte minor = binaryReader.ReadByte();
				byte build = binaryReader.ReadByte();
				Version v = new Version((int)major, (int)minor, (int)build);
				if (v != FrameworkLookup.CurrentVersion)
				{
					return null;
				}
				int num = binaryReader.ReadInt32();
				int num2 = binaryReader.ReadInt32();
				int num3 = binaryReader.ReadInt32();
				frameworkLookup.assemblyListTable = new int[num3];
				for (int i = 0; i < num3; i++)
				{
					frameworkLookup.assemblyListTable[i] = binaryReader.ReadInt32();
				}
				frameworkLookup.typeLookupTable = new int[num];
				for (int j = 0; j < num; j++)
				{
					frameworkLookup.typeLookupTable[j] = binaryReader.ReadInt32();
					binaryReader.ReadInt32();
				}
				frameworkLookup.extLookupTable = new int[num2];
				for (int k = 0; k < num2; k++)
				{
					frameworkLookup.extLookupTable[k] = binaryReader.ReadInt32();
					binaryReader.ReadInt32();
				}
			}
			return frameworkLookup;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00021894 File Offset: 0x00020894
		private FrameworkLookup()
		{
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00021C70 File Offset: 0x00020C70
		private IEnumerable<FrameworkLookup.AssemblyLookup> GetLookup(string identifier, int[] lookupTable, int tableOffset)
		{
			if (lookupTable != null)
			{
				int index = Array.BinarySearch<int>(lookupTable, FrameworkLookup.GetStableHashCode(identifier));
				if (index >= 0)
				{
					using (BinaryReader reader = new BinaryReader(File.Open(this.fileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8))
					{
						reader.BaseStream.Seek((long)(tableOffset + index * 8 + 4), SeekOrigin.Begin);
						int listPtr = reader.ReadInt32();
						reader.BaseStream.Seek((long)listPtr, SeekOrigin.Begin);
						int b = reader.ReadInt32();
						List<ushort> assemblies = new List<ushort>();
						ushort num2;
						for (;;)
						{
							int num;
							b = (num = b) - 1;
							if (num <= 0)
							{
								goto Block_6;
							}
							num2 = reader.ReadUInt16();
							if (num2 < 0 || (int)num2 >= this.assemblyListTable.Length)
							{
								break;
							}
							assemblies.Add(num2);
						}
						throw new InvalidDataException(string.Concat(new object[]
						{
							"Assembly lookup was ",
							num2,
							" but only ",
							this.assemblyListTable.Length,
							" are known."
						}));
						Block_6:
						foreach (ushort assembly in assemblies)
						{
							reader.BaseStream.Seek((long)this.assemblyListTable[(int)assembly], SeekOrigin.Begin);
							string package = reader.ReadString();
							string fullName = reader.ReadString();
							string ns = reader.ReadString();
							yield return new FrameworkLookup.AssemblyLookup(package, fullName, ns);
						}
					}
				}
			}
			yield break;
		}

		/// <summary>
		/// Retrieves a hash code for the specified string that is stable across
		/// .NET upgrades.
		///
		/// Use this method instead of the normal <c>string.GetHashCode</c> if the hash code
		/// is persisted to disk.
		/// </summary>
		// Token: 0x06000B09 RID: 2825 RVA: 0x00021CA4 File Offset: 0x00020CA4
		private static int GetStableHashCode(string text)
		{
			int num = 0;
			foreach (char c in text)
			{
				num = (num << 5) - num + (int)c;
			}
			return num;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00021CD7 File Offset: 0x00020CD7
		private static string GetIdentifier(string identifier, int tc)
		{
			if (tc == 0)
			{
				return identifier;
			}
			return identifier + "`" + tc;
		}

		// Token: 0x040003D0 RID: 976
		private const int headerSize = 15;

		// Token: 0x040003D1 RID: 977
		public static readonly Version CurrentVersion = new Version(2, 0, 1);

		// Token: 0x040003D2 RID: 978
		public static readonly FrameworkLookup Empty = new FrameworkLookup();

		// Token: 0x040003D3 RID: 979
		private string fileName;

		// Token: 0x040003D4 RID: 980
		private int[] assemblyListTable;

		// Token: 0x040003D5 RID: 981
		private int[] typeLookupTable;

		// Token: 0x040003D6 RID: 982
		private int[] extLookupTable;

		/// <summary>
		/// The assembly lookup determines where a type might be defined.
		/// It contains the assembly &amp; the namespace.
		/// </summary>
		// Token: 0x02000142 RID: 322
		public struct AssemblyLookup
		{
			/// <summary>
			/// The namespace the requested type is in.
			/// </summary>
			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00021D08 File Offset: 0x00020D08
			public string Namespace
			{
				get
				{
					return this.nspace;
				}
			}

			/// <summary>
			/// Gets the full name af the assembly.
			/// </summary>
			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00021D10 File Offset: 0x00020D10
			public string FullName
			{
				get
				{
					return this.fullName;
				}
			}

			/// <summary>
			/// Gets the package the assembly is in.
			/// </summary>
			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00021D18 File Offset: 0x00020D18
			public string Package
			{
				get
				{
					return this.package;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="T:ICSharpCode.NRefactory.Completion.FrameworkLookup.AssemblyLookup" /> struct.
			/// </summary>
			/// <param name="package">The package name.</param>
			/// <param name="fullName">The full name of the assembly.</param>
			/// <param name="nspace">The namespace the type is in.</param>
			// Token: 0x06000B0F RID: 2831 RVA: 0x00021D20 File Offset: 0x00020D20
			internal AssemblyLookup(string package, string fullName, string nspace)
			{
				if (nspace == null)
				{
					throw new ArgumentNullException("nspace");
				}
				if (fullName == null)
				{
					throw new ArgumentNullException("fullName");
				}
				this.package = package;
				this.fullName = fullName;
				this.nspace = nspace;
			}

			// Token: 0x06000B10 RID: 2832 RVA: 0x00021D53 File Offset: 0x00020D53
			public override string ToString()
			{
				return string.Format("[AssemblyLookup: Namespace={0}, FullName={1}, Package={2}]", this.Namespace, this.FullName, this.Package);
			}

			// Token: 0x06000B11 RID: 2833 RVA: 0x00021D74 File Offset: 0x00020D74
			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() != typeof(FrameworkLookup.AssemblyLookup))
				{
					return false;
				}
				FrameworkLookup.AssemblyLookup assemblyLookup = (FrameworkLookup.AssemblyLookup)obj;
				return this.Namespace == assemblyLookup.Namespace && this.FullName == assemblyLookup.FullName && this.Package == assemblyLookup.Package;
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x00021DE4 File Offset: 0x00020DE4
			public override int GetHashCode()
			{
				return ((this.Namespace != null) ? this.Namespace.GetHashCode() : 0) ^ ((this.FullName != null) ? this.FullName.GetHashCode() : 0) ^ ((this.Package != null) ? this.Package.GetHashCode() : 0);
			}

			// Token: 0x040003D7 RID: 983
			private readonly string nspace;

			// Token: 0x040003D8 RID: 984
			private readonly string fullName;

			// Token: 0x040003D9 RID: 985
			private readonly string package;
		}

		// Token: 0x02000143 RID: 323
		public class FrameworkBuilder : IDisposable
		{
			// Token: 0x06000B13 RID: 2835 RVA: 0x00021E38 File Offset: 0x00020E38
			internal FrameworkBuilder(string fileName)
			{
				this.fileName = fileName;
			}

			// Token: 0x06000B14 RID: 2836 RVA: 0x00021EB8 File Offset: 0x00020EB8
			private static int[] WriteTable(MemoryStream stream, Dictionary<int, List<ushort>> table, out List<KeyValuePair<int, List<ushort>>> list)
			{
				list = new List<KeyValuePair<int, List<ushort>>>(table);
				list.Sort((KeyValuePair<int, List<ushort>> x, KeyValuePair<int, List<ushort>> y) => x.Key.CompareTo(y.Key));
				int[] array = new int[list.Count];
				using (BinaryWriter binaryWriter = new BinaryWriter(stream))
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = (int)stream.Length;
						binaryWriter.Write(list[i].Value.Count);
						foreach (ushort value in list[i].Value)
						{
							binaryWriter.Write(value);
						}
					}
				}
				return array;
			}

			// Token: 0x06000B15 RID: 2837 RVA: 0x00021FA4 File Offset: 0x00020FA4
			void IDisposable.Dispose()
			{
				MemoryStream memoryStream = new MemoryStream();
				List<KeyValuePair<int, List<ushort>>> list;
				int[] array = FrameworkLookup.FrameworkBuilder.WriteTable(memoryStream, this.typeLookup, out list);
				MemoryStream memoryStream2 = new MemoryStream();
				List<KeyValuePair<int, List<ushort>>> list2;
				int[] array2 = FrameworkLookup.FrameworkBuilder.WriteTable(memoryStream2, this.extensionMethodLookup, out list2);
				MemoryStream memoryStream3 = new MemoryStream();
				int[] array3 = new int[this.assemblyLookups.Count];
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream3, Encoding.UTF8))
				{
					for (int i = 0; i < this.assemblyLookups.Count; i++)
					{
						FrameworkLookup.AssemblyLookup assemblyLookup = this.assemblyLookups[i];
						array3[i] = (int)memoryStream3.Length;
						binaryWriter.Write(assemblyLookup.Package);
						binaryWriter.Write(assemblyLookup.FullName);
						binaryWriter.Write(assemblyLookup.Namespace);
					}
				}
				using (BinaryWriter binaryWriter2 = new BinaryWriter(File.OpenWrite(this.fileName), Encoding.UTF8))
				{
					binaryWriter2.Write((byte)FrameworkLookup.CurrentVersion.Major);
					binaryWriter2.Write((byte)FrameworkLookup.CurrentVersion.Minor);
					binaryWriter2.Write((byte)FrameworkLookup.CurrentVersion.Build);
					binaryWriter2.Write(list.Count);
					binaryWriter2.Write(list2.Count);
					binaryWriter2.Write(this.assemblyLookups.Count);
					byte[] array4 = memoryStream.ToArray();
					byte[] array5 = memoryStream2.ToArray();
					int num = 15 + this.assemblyLookups.Count * 4 + list.Count * 8 + list2.Count * 8;
					for (int j = 0; j < this.assemblyLookups.Count; j++)
					{
						binaryWriter2.Write(num + array4.Length + array5.Length + array3[j]);
					}
					for (int k = 0; k < list.Count; k++)
					{
						binaryWriter2.Write(list[k].Key);
						binaryWriter2.Write(num + array[k]);
					}
					for (int l = 0; l < list2.Count; l++)
					{
						binaryWriter2.Write(list2[l].Key);
						binaryWriter2.Write(num + array4.Length + array2[l]);
					}
					binaryWriter2.Write(array4);
					binaryWriter2.Write(array5);
					binaryWriter2.Write(memoryStream3.ToArray());
					binaryWriter2.Flush();
				}
			}

			// Token: 0x06000B16 RID: 2838 RVA: 0x0002223C File Offset: 0x0002123C
			private ushort GetLookup(string packageName, string assemblyName, string ns)
			{
				FrameworkLookup.FrameworkBuilder.FrameworkLookupId key = new FrameworkLookup.FrameworkBuilder.FrameworkLookupId
				{
					PackageName = packageName,
					AssemblyName = assemblyName,
					NameSpace = ns
				};
				ushort result;
				if (this.frameworkLookupTable.TryGetValue(key, out result))
				{
					return result;
				}
				FrameworkLookup.AssemblyLookup item = new FrameworkLookup.AssemblyLookup(packageName, assemblyName, ns);
				this.assemblyLookups.Add(item);
				int num = this.assemblyLookups.Count - 1;
				if (num > 65535)
				{
					throw new InvalidOperationException("Assembly lookup list overflow > " + ushort.MaxValue + " assemblies.");
				}
				this.frameworkLookupTable.Add(key, (ushort)num);
				return (ushort)num;
			}

			// Token: 0x06000B17 RID: 2839 RVA: 0x000222F0 File Offset: 0x000212F0
			private bool AddToTable(string packageName, string assemblyName, Dictionary<int, List<ushort>> table, Dictionary<int, string> checkTable, string id, string ns)
			{
				int stableHashCode = FrameworkLookup.GetStableHashCode(id);
				List<ushort> list;
				string text;
				if (!table.TryGetValue(stableHashCode, out list))
				{
					list = new List<ushort>();
					table[stableHashCode] = list;
				}
				else if (checkTable.TryGetValue(stableHashCode, out text))
				{
					if (text != id)
					{
						throw new InvalidOperationException("Duplicate hash for " + text + " and " + id);
					}
				}
				else
				{
					checkTable.Add(stableHashCode, id);
				}
				ushort assemblyLookup = this.GetLookup(packageName, assemblyName, ns);
				if (!list.Any((ushort a) => a.Equals(assemblyLookup)))
				{
					list.Add(assemblyLookup);
					return true;
				}
				return false;
			}

			/// <summary>
			/// Add a type to the framework lookup.
			/// </summary>
			/// <param name="packageName">The package the assembly of the type is defined (can be null).</param>
			/// <param name="fullAssemblyName">The full assembly name the type is defined (needs to be != null).</param>
			/// <param name="type">The type definition  (needs to be != null).</param>
			// Token: 0x06000B18 RID: 2840 RVA: 0x00022394 File Offset: 0x00021394
			public void AddLookup(string packageName, string fullAssemblyName, IUnresolvedTypeDefinition type)
			{
				if (fullAssemblyName == null)
				{
					throw new ArgumentNullException("fullAssemblyName");
				}
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				string identifier = FrameworkLookup.GetIdentifier(type.Name, type.TypeParameters.Count);
				if (this.AddToTable(packageName, fullAssemblyName, this.typeLookup, this.typeCheck, identifier, type.Namespace) && (type.IsSealed || type.IsStatic))
				{
					foreach (IUnresolvedMethod unresolvedMethod in type.Methods)
					{
						DefaultUnresolvedMethod defaultUnresolvedMethod = unresolvedMethod as DefaultUnresolvedMethod;
						if (defaultUnresolvedMethod != null && defaultUnresolvedMethod.IsExtensionMethod)
						{
							this.AddToTable(packageName, fullAssemblyName, this.extensionMethodLookup, this.methodCheck, unresolvedMethod.Name, unresolvedMethod.DeclaringTypeDefinition.Namespace);
						}
					}
				}
			}

			// Token: 0x040003DA RID: 986
			private readonly string fileName;

			// Token: 0x040003DB RID: 987
			private Dictionary<int, List<ushort>> typeLookup = new Dictionary<int, List<ushort>>();

			// Token: 0x040003DC RID: 988
			private Dictionary<int, List<ushort>> extensionMethodLookup = new Dictionary<int, List<ushort>>();

			// Token: 0x040003DD RID: 989
			private List<FrameworkLookup.AssemblyLookup> assemblyLookups = new List<FrameworkLookup.AssemblyLookup>();

			// Token: 0x040003DE RID: 990
			private Dictionary<int, string> methodCheck = new Dictionary<int, string>();

			// Token: 0x040003DF RID: 991
			private Dictionary<int, string> typeCheck = new Dictionary<int, string>();

			// Token: 0x040003E0 RID: 992
			private Dictionary<FrameworkLookup.FrameworkBuilder.FrameworkLookupId, ushort> frameworkLookupTable = new Dictionary<FrameworkLookup.FrameworkBuilder.FrameworkLookupId, ushort>();

			// Token: 0x02000144 RID: 324
			private struct FrameworkLookupId
			{
				// Token: 0x040003E2 RID: 994
				public string PackageName;

				// Token: 0x040003E3 RID: 995
				public string AssemblyName;

				// Token: 0x040003E4 RID: 996
				public string NameSpace;
			}
		}
	}
}
