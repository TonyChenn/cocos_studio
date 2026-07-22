using System;
using System.CodeDom.Compiler;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	// Token: 0x02000030 RID: 48
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaTimelineAction : LuaBaseObject
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00008488 File Offset: 0x00006688
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000084A0 File Offset: 0x000066A0
		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(TimelineActionData) == objectData.GetType();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000084B7 File Offset: 0x000066B7
		public override void CreateObject(BaseObjectData objectData)
		{
			base.Write("result['animation'] = ccs.ActionTimeline:create()\r\n  \r\n");
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000084C4 File Offset: 0x000066C4
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

		// Token: 0x0600011D RID: 285 RVA: 0x0000864C File Offset: 0x0000684C
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
