using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;

namespace MonoDevelop.DesignerSupport
{
	internal class ClassOutlineSettings
	{
		private const string KEY_GROUP_ORDER = "MonoDevelop.DesignerSupport.ClassOutline.GroupOrder";

		private const string KEY_IS_GROUPED = "MonoDevelop.DesignerSupport.ClassOutline.IsGrouped";

		private const string KEY_IS_SORTED = "MonoDevelop.DesignerSupport.ClassOutline.IsSorted";

		public const string GroupRegions = "Regions";

		public const string GroupNamespaces = "Namespaces";

		public const string GroupTypes = "Types";

		public const string GroupFields = "Fields";

		public const string GroupProperties = "Properties";

		public const string GroupEvents = "Events";

		public const string GroupMethods = "Methods";

		private static Dictionary<string, string> groupNames = new Dictionary<string, string>
		{
			{
				"Regions",
				GettextCatalog.GetString("Regions")
			},
			{
				"Namespaces",
				GettextCatalog.GetString("Namespaces")
			},
			{
				"Types",
				GettextCatalog.GetString("Types")
			},
			{
				"Properties",
				GettextCatalog.GetString("Properties")
			},
			{
				"Fields",
				GettextCatalog.GetString("Fields")
			},
			{
				"Events",
				GettextCatalog.GetString("Events")
			},
			{
				"Methods",
				GettextCatalog.GetString("Methods")
			}
		};

		public IList<string> GroupOrder { get; set; }

		public bool IsGrouped { get; set; }

		public bool IsSorted { get; set; }

		private ClassOutlineSettings()
		{
		}

		public static ClassOutlineSettings Load()
		{
			ClassOutlineSettings classOutlineSettings = new ClassOutlineSettings();
			classOutlineSettings.IsGrouped = PropertyService.Get("MonoDevelop.DesignerSupport.ClassOutline.IsGrouped", defaultValue: false);
			classOutlineSettings.IsSorted = PropertyService.Get("MonoDevelop.DesignerSupport.ClassOutline.IsSorted", defaultValue: false);
			string text = PropertyService.Get("MonoDevelop.DesignerSupport.ClassOutline.GroupOrder", "");
			if (text.Length == 0)
			{
				classOutlineSettings.GroupOrder = groupNames.Keys.ToArray();
			}
			else
			{
				classOutlineSettings.GroupOrder = text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			}
			return classOutlineSettings;
		}

		public void Save()
		{
			PropertyService.Set("MonoDevelop.DesignerSupport.ClassOutline.IsGrouped", IsGrouped);
			PropertyService.Set("MonoDevelop.DesignerSupport.ClassOutline.IsSorted", IsSorted);
			PropertyService.Set("MonoDevelop.DesignerSupport.ClassOutline.GroupOrder", string.Join(",", GroupOrder));
		}

		public static string GetGroupName(string group)
		{
			return groupNames[group];
		}
	}
}
