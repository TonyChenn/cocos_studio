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
	public sealed class FrameworkLookup
	{
		/// <summary>
		/// This method tries to get a matching extension method.
		/// </summary>
		/// <returns>The extension method lookups.</returns>
		/// <param name="resolveResult">The resolve result.</param>
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
		public static FrameworkLookup.FrameworkBuilder Create(string fileName)
		{
			return new FrameworkLookup.FrameworkBuilder(fileName);
		}

		/// <summary>
		/// Loads a framework lookup object from a file. May return null, if the file wasn't found or has a version mismatch.
		/// </summary>
		/// <param name="fileName">File name.</param>
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

		private FrameworkLookup()
		{
		}

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
		private static int GetStableHashCode(string text)
		{
			int num = 0;
			foreach (char c in text)
			{
				num = (num << 5) - num + (int)c;
			}
			return num;
		}

		private static string GetIdentifier(string identifier, int tc)
		{
			if (tc == 0)
			{
				return identifier;
			}
			return identifier + "`" + tc;
		}

		private const int headerSize = 15;

		public static readonly Version CurrentVersion = new Version(2, 0, 1);

		public static readonly FrameworkLookup Empty = new FrameworkLookup();

		private string fileName;

		private int[] assemblyListTable;

		private int[] typeLookupTable;

		private int[] extLookupTable;

		/// <summary>
		/// The assembly lookup determines where a type might be defined.
		/// It contains the assembly &amp; the namespace.
		/// </summary>
		public struct AssemblyLookup
		{
			/// <summary>
			/// The namespace the requested type is in.
			/// </summary>
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

			public override string ToString()
			{
				return string.Format("[AssemblyLookup: Namespace={0}, FullName={1}, Package={2}]", this.Namespace, this.FullName, this.Package);
			}

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

			public override int GetHashCode()
			{
				return ((this.Namespace != null) ? this.Namespace.GetHashCode() : 0) ^ ((this.FullName != null) ? this.FullName.GetHashCode() : 0) ^ ((this.Package != null) ? this.Package.GetHashCode() : 0);
			}

			private readonly string nspace;

			private readonly string fullName;

			private readonly string package;
		}

		public class FrameworkBuilder : IDisposable
		{
			internal FrameworkBuilder(string fileName)
			{
				this.fileName = fileName;
			}

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

			private readonly string fileName;

			private Dictionary<int, List<ushort>> typeLookup = new Dictionary<int, List<ushort>>();

			private Dictionary<int, List<ushort>> extensionMethodLookup = new Dictionary<int, List<ushort>>();

			private List<FrameworkLookup.AssemblyLookup> assemblyLookups = new List<FrameworkLookup.AssemblyLookup>();

			private Dictionary<int, string> methodCheck = new Dictionary<int, string>();

			private Dictionary<int, string> typeCheck = new Dictionary<int, string>();

			private Dictionary<FrameworkLookup.FrameworkBuilder.FrameworkLookupId, ushort> frameworkLookupTable = new Dictionary<FrameworkLookup.FrameworkBuilder.FrameworkLookupId, ushort>();

			private struct FrameworkLookupId
			{
				public string PackageName;

				public string AssemblyName;

				public string NameSpace;
			}
		}
	}
}
