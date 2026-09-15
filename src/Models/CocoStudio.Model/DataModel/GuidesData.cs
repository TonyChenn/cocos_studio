using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	[Extension(Type = typeof(IUserData))]
	public class GuidesData : IUserData
	{
		[ItemProperty]
		public List<GuidesObject> HorizontalList { get; set; }

		[ItemProperty]
		public List<GuidesObject> VerticalList { get; set; }

		public GuidesData()
		{
			this.HorizontalList = new List<GuidesObject>();
			this.VerticalList = new List<GuidesObject>();
		}

		public const string GuidesListKey = "GuidesList";
	}
}
