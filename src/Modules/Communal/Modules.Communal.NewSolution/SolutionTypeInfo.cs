using System;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001F RID: 31
	public class SolutionTypeInfo
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00008286 File Offset: 0x00006486
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000828E File Offset: 0x0000648E
		public EnumSolutionType SolutionType { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00008297 File Offset: 0x00006497
		// (set) Token: 0x060000EB RID: 235 RVA: 0x0000829F File Offset: 0x0000649F
		public string Name { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000082A8 File Offset: 0x000064A8
		// (set) Token: 0x060000ED RID: 237 RVA: 0x000082B0 File Offset: 0x000064B0
		public string Description { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000082B9 File Offset: 0x000064B9
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000082C1 File Offset: 0x000064C1
		public bool NeedFramework { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000082CA File Offset: 0x000064CA
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000082D2 File Offset: 0x000064D2
		public bool LuaEnable { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000082DB File Offset: 0x000064DB
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000082E3 File Offset: 0x000064E3
		public bool CppEnable { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000082EC File Offset: 0x000064EC
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x000082F4 File Offset: 0x000064F4
		public bool JsEnable { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000082FD File Offset: 0x000064FD
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00008305 File Offset: 0x00006505
		public Image Image { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000830E File Offset: 0x0000650E
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00008316 File Offset: 0x00006516
		public SampleInfo SampleInfo { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000831F File Offset: 0x0000651F
		public string DefaultSolutionName
		{
			get
			{
				return this.defaultSolutionName;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00008328 File Offset: 0x00006528
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

		// Token: 0x040000B9 RID: 185
		private string defaultSolutionName = "CocosProject";
	}
}
