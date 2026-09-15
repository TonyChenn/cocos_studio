using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTimeline : LuaBaseObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TimelineData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(objectData.Name)));
			base.Write(" = ccs.Timeline:create()\r\n\r\n");
		}

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
