using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x0200002F RID: 47
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTimeline : LuaBaseObject
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00008361 File Offset: 0x00006561
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00008379 File Offset: 0x00006579
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TimelineData) == objectData.GetType();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00008390 File Offset: 0x00006590
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccs.Timeline:create()\r\n\r\n");
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000083BC File Offset: 0x000065BC
		public override void InitializeObject(BaseObjectData objectData)
		{
			TimelineData timelineData = objectData as TimelineData;
			if (timelineData == null || timelineData.Frames == null)
			{
				return;
			}
			if (timelineData.Frames != null)
			{
				foreach (FrameData frameData in timelineData.Frames)
				{
					((IFrameDataLuaExtend)frameData).Property = timelineData.Property;
					ILuaObjectSerializer serializer = LuaObjectManager.GetSerializer(frameData);
					if (serializer != null)
					{
						base.Write("localFrame = ");
						serializer.CreateObject(frameData);
						serializer.InitializeObject(frameData);
						base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(objectData.Name)));
						base.Write(":addFrame(localFrame)\r\n\r\n");
					}
				}
			}
		}
	}
}
