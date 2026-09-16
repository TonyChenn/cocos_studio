using CocoStudio.Core;

namespace Modules.UI.StructTree
{
	public class HierarchyPad : DefaultPadContent
	{
		public HierarchyPad() : this(new HierarchyWidget())
		{
		}

		private HierarchyPad(HierarchyWidget hierarchyWidget) : base(hierarchyWidget)
		{
			this.hierarchyWidget = hierarchyWidget;
		}

		public override void Dispose()
		{
			this.hierarchyWidget.DetachEvents();
			base.Dispose();
		}

		private readonly HierarchyWidget hierarchyWidget;
	}
}
