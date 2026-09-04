using System;
using System.ComponentModel;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Completion
{
	// Token: 0x02000139 RID: 313
	public static class CompletionExtensionMethods
	{
		/// <summary>
		/// Gets the EditorBrowsableState of an entity.
		/// </summary>
		/// <returns>
		/// The editor browsable state.
		/// </returns>
		/// <param name="entity">
		/// Entity.
		/// </param>
		// Token: 0x06000ADF RID: 2783 RVA: 0x0002075C File Offset: 0x0001F75C
		public static EditorBrowsableState GetEditorBrowsableState(this IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			IAttribute attribute = entity.Attributes.FirstOrDefault((IAttribute attr) => attr.AttributeType.Name == "EditorBrowsableAttribute" && attr.AttributeType.Namespace == "System.ComponentModel");
			if (attribute != null && attribute.PositionalArguments.Count == 1 && attribute.PositionalArguments[0].ConstantValue is int)
			{
				return (EditorBrowsableState)((int)attribute.PositionalArguments[0].ConstantValue);
			}
			return EditorBrowsableState.Always;
		}

		/// <summary>
		/// Determines if an entity should be shown in the code completion window. This is the same as:
		/// <c>GetEditorBrowsableState (entity) != System.ComponentModel.EditorBrowsableState.Never</c>
		/// </summary>
		/// <returns>
		/// <c>true</c> if the entity should be shown; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="entity">
		/// The entity.
		/// </param>
		// Token: 0x06000AE0 RID: 2784 RVA: 0x000207E1 File Offset: 0x0001F7E1
		public static bool IsBrowsable(this IEntity entity)
		{
			return entity.GetEditorBrowsableState() != EditorBrowsableState.Never;
		}
	}
}
