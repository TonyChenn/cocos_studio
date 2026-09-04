namespace MonoDevelop.DesignerSupport
{
	public static class DesignerSupport
	{
		private static DesignerSupportService designerSupportService;

		public static DesignerSupportService Service
		{
			get
			{
				if (designerSupportService == null)
				{
					designerSupportService = new DesignerSupportService();
				}
				return designerSupportService;
			}
		}
	}
}
