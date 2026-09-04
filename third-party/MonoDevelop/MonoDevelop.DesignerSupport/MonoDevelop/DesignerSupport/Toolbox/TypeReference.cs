using System;
using System.ComponentModel;
using System.Reflection;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	[Serializable]
	[TypeConverter(typeof(TypeReferenceTypeConverter))]
	public class TypeReference
	{
		[ItemProperty("location")]
		private string assemblyLocation = string.Empty;

		[ItemProperty("assembly")]
		private string assemblyName = string.Empty;

		[ItemProperty("name")]
		private string typeName = string.Empty;

		[ReadOnly(true)]
		[LocalizedDescription("The assembly name.")]
		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Assembly Name")]
		public string AssemblyName
		{
			get
			{
				return assemblyName;
			}
			set
			{
				assemblyName = value;
			}
		}

		[ReadOnly(true)]
		[LocalizedCategory("Misc")]
		[LocalizedDisplayName("Type Name")]
		[LocalizedDescription("The fully-qualified type name.")]
		public string TypeName
		{
			get
			{
				return typeName;
			}
			set
			{
				typeName = value;
			}
		}

		[LocalizedDescription("The location of the assembly.")]
		[ReadOnly(true)]
		[LocalizedDisplayName("Assembly Location")]
		public string AssemblyLocation
		{
			get
			{
				return assemblyLocation;
			}
			set
			{
				assemblyLocation = value;
			}
		}

		public TypeReference()
		{
		}

		public override bool Equals(object obj)
		{
			if (obj is TypeReference typeReference && typeName == typeReference.typeName && assemblyName == typeReference.assemblyName)
			{
				return assemblyLocation == typeReference.assemblyLocation;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (typeName + assemblyName + assemblyLocation).GetHashCode();
		}

		public bool Equals(TypeReference other)
		{
			if (other != null && typeName == other.typeName && assemblyName == other.assemblyName)
			{
				return assemblyLocation == other.assemblyLocation;
			}
			return false;
		}

		public TypeReference(string typeName, string assemblyName)
		{
			this.typeName = typeName;
			this.assemblyName = assemblyName;
		}

		public TypeReference(Type type)
			: this(type.FullName, type.Assembly.FullName)
		{
			if (!type.Assembly.GlobalAssemblyCache)
			{
				assemblyLocation = type.Assembly.Location;
			}
		}

		public TypeReference(string typeName, string assemblyName, string assemblyLocation)
			: this(typeName, assemblyName)
		{
			this.assemblyLocation = assemblyLocation;
		}

		public Type Load()
		{
			Assembly assembly = null;
			assembly = ((!string.IsNullOrEmpty(assemblyLocation)) ? Assembly.LoadFile(assemblyLocation) : Assembly.Load(assemblyName));
			return assembly.GetType(typeName, throwOnError: true);
		}

		public ProjectReference GetProjectReference()
		{
			if (string.IsNullOrEmpty(assemblyLocation))
			{
				return new ProjectReference(ReferenceType.Package, assemblyName);
			}
			return new ProjectReference(ReferenceType.Assembly, assemblyLocation);
		}
	}
}
