using System;
using MonoDevelop.Ide.Gui.Components;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class ComponentNodeBuilder : NodeBuilderExtension
	{
		public override Type CommandHandlerType => typeof(ComponentNodeCommandHandler);

		public override bool CanBuildNode(Type dataType)
		{
			return true;
		}
	}
}
