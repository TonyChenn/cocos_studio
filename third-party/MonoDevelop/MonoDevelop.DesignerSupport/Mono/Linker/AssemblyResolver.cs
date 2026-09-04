using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Mono.Linker
{
	internal class AssemblyResolver : BaseAssemblyResolver
	{
		private Hashtable _assemblies;

		public IDictionary AssemblyCache => _assemblies;

		public AssemblyResolver()
		{
			_assemblies = new Hashtable();
		}

		public override AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			AssemblyDefinition assemblyDefinition = (AssemblyDefinition)_assemblies[name.Name];
			if (assemblyDefinition == null)
			{
				assemblyDefinition = base.Resolve(name);
				_assemblies[name.Name] = assemblyDefinition;
			}
			return assemblyDefinition;
		}

		public TypeDefinition Resolve(TypeReference type)
		{
			type = type.GetElementType();
			if (type is TypeDefinition)
			{
				return (TypeDefinition)type;
			}
			if (type.Scope is AssemblyNameReference name)
			{
				AssemblyDefinition assemblyDefinition = Resolve(name);
				return assemblyDefinition.MainModule.GetType(type.FullName);
			}
			if (type.Scope is ModuleDefinition moduleDefinition)
			{
				return moduleDefinition.GetType(type.FullName);
			}
			throw new NotImplementedException();
		}

		public FieldDefinition Resolve(FieldReference field)
		{
			TypeDefinition typeDefinition = Resolve(field.DeclaringType);
			return GetField(typeDefinition.Fields, field);
		}

		private static FieldDefinition GetField(ICollection collection, FieldReference reference)
		{
			foreach (FieldDefinition item in collection)
			{
				if (!(item.Name != reference.Name) && AreSame(item.FieldType, reference.FieldType))
				{
					return item;
				}
			}
			return null;
		}

		public MethodDefinition Resolve(MethodReference method)
		{
			TypeDefinition type = Resolve(method.DeclaringType);
			method = method.GetElementMethod();
			return GetMethod(type, method);
		}

		private MethodDefinition GetMethod(TypeDefinition type, MethodReference reference)
		{
			while (type != null)
			{
				MethodDefinition method = GetMethod(type.Methods, reference);
				if (method == null)
				{
					type = Resolve(type.BaseType);
					continue;
				}
				return method;
			}
			return null;
		}

		private static MethodDefinition GetMethod(ICollection collection, MethodReference reference)
		{
			foreach (MethodDefinition item in collection)
			{
				if (!(item.Name != reference.Name) && AreSame(item.ReturnType, reference.ReturnType) && AreSame(item.Parameters, reference.Parameters))
				{
					return item;
				}
			}
			return null;
		}

		private static bool AreSame(IList<ParameterDefinition> a, IList<ParameterDefinition> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			if (a.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!AreSame(a[i].ParameterType, b[i].ParameterType))
				{
					return false;
				}
			}
			return true;
		}

		private static bool AreSame(TypeReference a, TypeReference b)
		{
			while (a is TypeSpecification || b is TypeSpecification)
			{
				if (a.GetType() != b.GetType())
				{
					return false;
				}
				a = ((TypeSpecification)a).ElementType;
				b = ((TypeSpecification)b).ElementType;
			}
			if (a is GenericParameter || b is GenericParameter)
			{
				if (a.GetType() != b.GetType())
				{
					return false;
				}
				GenericParameter genericParameter = (GenericParameter)a;
				GenericParameter genericParameter2 = (GenericParameter)b;
				return genericParameter.Position == genericParameter2.Position;
			}
			return a.FullName == b.FullName;
		}
	}
}
