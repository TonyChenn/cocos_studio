using System;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	public class SolutionTypeInfo
	{
		public EnumSolutionType SolutionType { get; private set; }

		public string Name { get; private set; }

		public string Description { get; private set; }

		public bool NeedFramework { get; private set; }

		public bool LuaEnable { get; private set; }

		public bool CppEnable { get; private set; }

		public bool JsEnable { get; private set; }

		public Image Image { get; private set; }

		public SampleInfo SampleInfo { get; private set; }

		public string DefaultSolutionName
		{
			get
			{
				return this.defaultSolutionName;
			}
		}

		public SolutionTypeInfo(string name, string desc, EnumSolutionType type = EnumSolutionType.Custom, Image image = null, bool needFrame = false, bool luaEnable = false, bool cppEnable = false, bool jsEnable = false, SampleInfo sampleInfo = null)
		{
			this.Name = name;
			this.Description = desc;
			this.SolutionType = type;
			this.Image = image;
			this.NeedFramework = needFrame;
			this.LuaEnable = luaEnable;
			this.CppEnable = cppEnable;
			this.JsEnable = jsEnable;
			this.SampleInfo = sampleInfo;
			if (this.SampleInfo != null && !string.IsNullOrWhiteSpace(this.SampleInfo.SampleName))
			{
				this.defaultSolutionName = this.SampleInfo.SampleName;
			}
		}

		private string defaultSolutionName = "CocosProject";
	}
}
