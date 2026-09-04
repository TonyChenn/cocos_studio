using System.CodeDom;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;

namespace MonoDevelop.DesignerSupport
{
	public class MemberExistsException : ErrorInFileException
	{
		private string className;

		private string memberName;

		private MemberType existingMemberType;

		private MemberType newMemberType;

		public string ClassName
		{
			get
			{
				return className;
			}
			set
			{
				className = value;
			}
		}

		public string MemberName
		{
			get
			{
				return memberName;
			}
			set
			{
				memberName = value;
			}
		}

		public MemberType ExistingMemberType
		{
			get
			{
				return existingMemberType;
			}
			set
			{
				existingMemberType = value;
			}
		}

		public MemberType NewMemberType
		{
			get
			{
				return newMemberType;
			}
			set
			{
				newMemberType = value;
			}
		}

		public MemberExistsException(string className, string memberName)
			: base(DomRegion.Empty, null)
		{
			this.className = className;
			this.memberName = memberName;
		}

		public MemberExistsException(string className, string memberName, MemberType existingMemberType, MemberType newMemberType, DomRegion errorLocation, string fileName)
			: base(errorLocation, fileName)
		{
			this.className = className;
			this.memberName = memberName;
			this.existingMemberType = existingMemberType;
			this.newMemberType = newMemberType;
		}

		public MemberExistsException(string className, MemberType newMemberType, CodeTypeMember existingMember, DomRegion errorLocation, string fileName)
			: this(className, existingMember.Name, newMemberType, GetMemberTypeFromCodeTypeMember(existingMember), errorLocation, fileName)
		{
		}

		public MemberExistsException(string className, CodeTypeMember newMember, MemberType existingMemberType, DomRegion errorLocation, string fileName)
			: this(className, newMember.Name, GetMemberTypeFromCodeTypeMember(newMember), existingMemberType, errorLocation, fileName)
		{
		}

		protected static MemberType GetMemberTypeFromCodeTypeMember(CodeTypeMember mem)
		{
			if (mem is CodeMemberEvent)
			{
				return MemberType.Event;
			}
			if (mem is CodeMemberProperty)
			{
				return MemberType.Property;
			}
			if (mem is CodeMemberField)
			{
				return MemberType.Field;
			}
			if (mem is CodeMemberMethod)
			{
				return MemberType.Method;
			}
			return MemberType.Member;
		}

		public override string ToString()
		{
			string format = ((NewMemberType != ExistingMemberType) ? GettextCatalog.GetString("Cannot add {0} '{1}' to class '{2}', because there is already a {3} with that name.") : GettextCatalog.GetString("Cannot add {0} '{1}' to class '{2}', because there is already a {3} with that name with an incompatible return type."));
			return string.Format(format, newMemberType.ToString().ToLower(), memberName, className, existingMemberType.ToString().ToLower());
		}
	}
}
