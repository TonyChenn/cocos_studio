using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	internal sealed class OwnedParameterReference : ISymbolReference
	{
		public OwnedParameterReference(IMemberReference member, int index)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.memberReference = member;
			this.index = index;
		}

		public ISymbol Resolve(ITypeResolveContext context)
		{
			IParameterizedMember parameterizedMember = this.memberReference.Resolve(context) as IParameterizedMember;
			if (parameterizedMember != null && this.index >= 0 && this.index < parameterizedMember.Parameters.Count)
			{
				return parameterizedMember.Parameters[this.index];
			}
			return null;
		}

		private readonly IMemberReference memberReference;

		private readonly int index;
	}
}
