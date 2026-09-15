using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTimelineAction : LuaBaseObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TimelineActionData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("result['animation'] = ccs.ActionTimeline:create()\r\n  \r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			TimelineActionData timelineActionData = objectData as TimelineActionData;
			base.Write("result['animation']:setDuration(");
			base.Write(base.ToStringHelper.ToStringWithCulture(timelineActionData.Duration));
			base.Write(")\r\nresult['animation']:setTimeSpeed(");
			base.Write(base.ToStringHelper.ToStringWithCulture(timelineActionData.Speed));
			base.Write(")\r\n");
			if (timelineActionData == null || timelineActionData.Timelines == null)
			{
				return;
			}
			if (timelineActionData.Timelines != null)
			{
				int num = 0;
				foreach (TimelineData timelineData in timelineActionData.Timelines)
				{
					timelineData.Name = timelineData.Property + "Timeline";
					bool flag = this.WriteTimeLine(timelineData);
					if (flag)
					{
						num++;
						base.Write("result['animation']:addTimeline(");
						base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(timelineData.Name)));
						base.Write(")\r\n");
						base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(timelineData.Name)));
						base.Write(":setNode(");
						base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(LuaObjectSerializer.NodeCollection[timelineData.ActionTag].Name)));
						base.Write(")\r\n");
					}
				}
			}
		}

		private bool WriteTimeLine(TimelineData timelineData)
		{
			ILuaObjectSerializer serializer = LuaObjectManager.GetSerializer(timelineData);
			if (serializer != null)
			{
				base.Write("\r\n--Create ");
				base.Write(base.ToStringHelper.ToStringWithCulture(timelineData.Name));
				base.Write("\r\n");
				serializer.CreateObject(timelineData);
				serializer.InitializeObject(timelineData);
				return true;
			}
			return false;
		}
	}
}
