using System;
using System.ComponentModel;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000008 RID: 8
	public interface IPropertyEditor : IComparable<IPropertyEditor>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000017 RID: 23
		string DisplayName { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000018 RID: 24
		string Group { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25
		int Order { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26
		// (set) Token: 0x0600001B RID: 27
		bool Visible { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28
		// (set) Token: 0x0600001D RID: 29
		bool Enable { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001E RID: 30
		Widget EditorWidget { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001F RID: 31
		PropertyItem PropertyItem { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000020 RID: 32
		bool SupportMultiSelect { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000021 RID: 33
		bool CanCaching { get; }

		// Token: 0x06000022 RID: 34
		void Initialize(PropertyItem propItem);

		// Token: 0x06000023 RID: 35
		void HandlePropertyChanged(PropertyChangedEventArgs e);
	}
}
