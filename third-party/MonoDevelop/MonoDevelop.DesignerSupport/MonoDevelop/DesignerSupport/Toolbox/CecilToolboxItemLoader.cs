using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Linker;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal abstract class CecilToolboxItemLoader : IToolboxLoader
	{
		private static string[] filetypes = new string[2] { "dll", "exe" };

		public string[] FileTypes => filetypes;

		public IList<ItemToolboxNode> Load(LoaderContext ctx, string filename)
		{
			AssemblyResolver assemblyResolver = new AssemblyResolver();
			AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(filename, new ReaderParameters
			{
				AssemblyResolver = assemblyResolver
			});
			Version version;
			switch (assemblyDefinition.MainModule.Runtime)
			{
			case TargetRuntime.Net_1_0:
			case TargetRuntime.Net_1_1:
				version = new Version(1, 0, 5000, 0);
				break;
			case TargetRuntime.Net_2_0:
				version = new Version(2, 0, 0, 0);
				break;
			default:
				throw new NotSupportedException(string.Concat("Runtime '", assemblyDefinition.MainModule.Runtime, "' is not supported."));
			}
			AssemblyNameReference scope = new AssemblyNameReference("System", version);
			Mono.Cecil.TypeReference typeReference = new Mono.Cecil.TypeReference("System.ComponentModel", "ToolboxItemAttribute", assemblyDefinition.MainModule, scope, valueType: false);
			Mono.Cecil.TypeReference typeReference2 = new Mono.Cecil.TypeReference("ToolboxItemAttribute", "CategoryAttribute", assemblyDefinition.MainModule, scope, valueType: false);
			if (assemblyResolver.Resolve(typeReference) == null)
			{
				return null;
			}
			List<ItemToolboxNode> result = new List<ItemToolboxNode>();
			foreach (ModuleDefinition module in assemblyDefinition.Modules)
			{
				foreach (TypeDefinition type in module.Types)
				{
					if (type.IsAbstract || !type.IsClass || !type.IsPublic)
					{
						continue;
					}
					TypeDefinition typeDefinition = null;
					string text = null;
					foreach (CustomAttribute item in AllCustomAttributes(assemblyResolver, type))
					{
						if (typeDefinition == null && IsEqualOrSubclass(assemblyResolver, item.Constructor.DeclaringType, typeReference))
						{
							IList argumentsToBaseConstructor = GetArgumentsToBaseConstructor(item, typeReference);
							if (argumentsToBaseConstructor != null)
							{
								if (argumentsToBaseConstructor.Count != 1)
								{
									throw new InvalidOperationException("Malformed toolboxitem attribute");
								}
								object obj = argumentsToBaseConstructor[0];
								if (obj == null || (obj is bool && !(bool)obj))
								{
									break;
								}
								string fullname = (string)obj;
								typeDefinition = null;
								try
								{
									assemblyResolver.Resolve(TypeReferenceFromString(module, fullname));
								}
								catch (Exception)
								{
								}
								if (typeDefinition == null)
								{
									break;
								}
							}
						}
						if (text == null && IsEqualOrSubclass(assemblyResolver, item.Constructor.DeclaringType, typeReference2))
						{
							IList argumentsToBaseConstructor2 = GetArgumentsToBaseConstructor(item, typeReference2);
							if (argumentsToBaseConstructor2 == null || argumentsToBaseConstructor2.Count != 1)
							{
								throw new InvalidOperationException("Malformed category attribute");
							}
							text = (string)argumentsToBaseConstructor2[0];
						}
						if (typeDefinition != null && text != null)
						{
							break;
						}
					}
					if (typeDefinition != null)
					{
						Load(assemblyDefinition, type, typeDefinition, text);
					}
				}
			}
			return result;
		}

		protected static Mono.Cecil.TypeReference TypeReferenceFromString(ModuleDefinition module, string fullname)
		{
			string[] array = fullname.Split(',');
			int num = array[0].LastIndexOf('.');
			string name = array[0].Substring(0, num).Trim();
			string text = array[0].Substring(num + 1).Trim();
			string name2 = array[1].Trim();
			string culture = string.Empty;
			Version version = new Version();
			for (int i = 2; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (text2.StartsWith("Version="))
				{
					version = new Version(text2.Substring(8));
				}
				else if (text2.StartsWith("Culture="))
				{
					culture = text2.Substring(8);
				}
			}
			AssemblyNameReference assemblyNameReference = new AssemblyNameReference(name2, version);
			assemblyNameReference.Culture = culture;
			AssemblyNameReference scope = assemblyNameReference;
			return new Mono.Cecil.TypeReference(text, name, module, scope, valueType: false);
		}

		protected static IList GetArgumentsToBaseConstructor(CustomAttribute att, Mono.Cecil.TypeReference baseType)
		{
			if (att.AttributeType.FullName == baseType.FullName)
			{
				return att.ConstructorArguments.Select((CustomAttributeArgument arg) => arg.Value).ToList();
			}
			throw new NotImplementedException();
		}

		protected static IEnumerable<CustomAttribute> AllCustomAttributes(AssemblyResolver resolver, TypeDefinition typedef)
		{
			TypeDefinition currentType = typedef;
			while (true)
			{
				foreach (CustomAttribute customAttribute in currentType.CustomAttributes)
				{
					yield return customAttribute;
				}
				if (!(currentType.BaseType.FullName == "System.Object"))
				{
					currentType = resolver.Resolve(currentType.BaseType);
					continue;
				}
				break;
			}
		}

		protected static bool IsEqualOrSubclass(AssemblyResolver resolver, Mono.Cecil.TypeReference inspect, Mono.Cecil.TypeReference baseClass)
		{
			TypeDefinition typeDefinition = resolver.Resolve(inspect);
			while (true)
			{
				if (baseClass.FullName == typeDefinition.FullName)
				{
					return true;
				}
				if (typeDefinition.BaseType.FullName == "System.Object")
				{
					break;
				}
				typeDefinition = resolver.Resolve(typeDefinition.BaseType);
			}
			return false;
		}

		protected static bool ImplementsInterface(AssemblyResolver resolver, Mono.Cecil.TypeReference inspect, Mono.Cecil.TypeReference interf)
		{
			TypeDefinition typeDefinition = resolver.Resolve(inspect);
			foreach (Mono.Cecil.TypeReference @interface in typeDefinition.Interfaces)
			{
				if (@interface.FullName == interf.FullName)
				{
					return true;
				}
				if (ImplementsInterface(resolver, @interface, interf))
				{
					return true;
				}
			}
			if (ImplementsInterface(resolver, typeDefinition.BaseType, interf))
			{
				return true;
			}
			return false;
		}

		protected abstract ItemToolboxNode Load(AssemblyDefinition assem, TypeDefinition componentType, TypeDefinition itemType, string category);
	}
}
