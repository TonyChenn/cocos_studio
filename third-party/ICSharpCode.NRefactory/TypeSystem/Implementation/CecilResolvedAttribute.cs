using System;
using System.Collections.Generic;
using System.Threading;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	internal sealed class CecilResolvedAttribute : IAttribute
	{
		public CecilResolvedAttribute(ITypeResolveContext context, UnresolvedAttributeBlob unresolved)
		{
			this.context = context;
			this.blob = unresolved.blob;
			this.ctorParameterTypes = unresolved.ctorParameterTypes;
			this.attributeType = unresolved.attributeType.Resolve(context);
		}

		public CecilResolvedAttribute(ITypeResolveContext context, IType attributeType)
		{
			this.context = context;
			this.attributeType = attributeType;
			this.ctorParameterTypes = EmptyList<ITypeReference>.Instance;
		}

		DomRegion IAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		public IType AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		public IMethod Constructor
		{
			get
			{
				if (!this.constructorResolved)
				{
					this.constructor = this.ResolveConstructor();
					this.constructorResolved = true;
				}
				return this.constructor;
			}
		}

		private IMethod ResolveConstructor()
		{
			IList<IType> parameterTypes = this.ctorParameterTypes.Resolve(this.context);
			foreach (IMethod method in this.attributeType.GetConstructors((IUnresolvedMethod m) => m.Parameters.Count == parameterTypes.Count, GetMemberOptions.IgnoreInheritedMembers))
			{
				bool flag = true;
				for (int i = 0; i < parameterTypes.Count; i++)
				{
					if (!method.Parameters[i].Type.Equals(parameterTypes[i]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return method;
				}
			}
			return null;
		}

		public IList<ResolveResult> PositionalArguments
		{
			get
			{
				IList<ResolveResult> list = LazyInit.VolatileRead<IList<ResolveResult>>(ref this.positionalArguments);
				if (list != null)
				{
					return list;
				}
				this.DecodeBlob();
				return this.positionalArguments;
			}
		}

		public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
		{
			get
			{
				IList<KeyValuePair<IMember, ResolveResult>> list = LazyInit.VolatileRead<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments);
				if (list != null)
				{
					return list;
				}
				this.DecodeBlob();
				return this.namedArguments;
			}
		}

		public override string ToString()
		{
			return "[" + this.attributeType.ToString() + "(...)]";
		}

		private void DecodeBlob()
		{
			List<ResolveResult> value = new List<ResolveResult>();
			List<KeyValuePair<IMember, ResolveResult>> value2 = new List<KeyValuePair<IMember, ResolveResult>>();
			this.DecodeBlob(value, value2);
			Interlocked.CompareExchange<IList<ResolveResult>>(ref this.positionalArguments, value, null);
			Interlocked.CompareExchange<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments, value2, null);
		}

		private void DecodeBlob(List<ResolveResult> positionalArguments, List<KeyValuePair<IMember, ResolveResult>> namedArguments)
		{
			if (this.blob == null)
			{
				return;
			}
			BlobReader blobReader = new BlobReader(this.blob, this.context.CurrentAssembly);
			if (blobReader.ReadUInt16() != 1)
			{
				return;
			}
			foreach (IType argType in this.ctorParameterTypes.Resolve(this.context))
			{
				bool flag;
				try
				{
					ResolveResult resolveResult = blobReader.ReadFixedArg(argType);
					positionalArguments.Add(resolveResult);
					flag = resolveResult.IsError;
				}
				catch (Exception)
				{
					flag = true;
				}
				if (flag)
				{
					while (positionalArguments.Count < this.ctorParameterTypes.Count)
					{
						positionalArguments.Add(ErrorResolveResult.UnknownError);
					}
					return;
				}
			}
			try
			{
				ushort num = blobReader.ReadUInt16();
				for (int i = 0; i < (int)num; i++)
				{
					KeyValuePair<IMember, ResolveResult> item = blobReader.ReadNamedArg(this.attributeType);
					if (item.Key != null)
					{
						namedArguments.Add(item);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private readonly ITypeResolveContext context;

		private readonly byte[] blob;

		private readonly IList<ITypeReference> ctorParameterTypes;

		private readonly IType attributeType;

		private IMethod constructor;

		private volatile bool constructorResolved;

		private IList<ResolveResult> positionalArguments;

		private IList<KeyValuePair<IMember, ResolveResult>> namedArguments;
	}
}
