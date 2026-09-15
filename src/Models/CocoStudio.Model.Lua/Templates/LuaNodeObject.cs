using System;
using System.CodeDom.Compiler;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;

namespace CocoStudio.Model.Lua.Templates
{
	[GeneratedCode("Microsoft.VisualStudio.TextTemplating", "12.0.0.0")]
	public class LuaNodeObject : LuaVisualObject
	{
		public override string TransformText()
		{
			base.Write("\r\n");
			return base.GenerationEnvironment.ToString();
		}

		public bool WriteObject(AbstractNodeObjectData objectData)
		{
			ILuaObjectSerializer serializer = LuaObjectManager.GetSerializer(objectData);
			if (serializer != null)
			{
				LuaObjectSerializer.NodeCollection[objectData.ActionTag] = objectData;
				base.Write("\r\n--Create ");
				base.Write(base.ToStringHelper.ToStringWithCulture(objectData.Name));
				base.Write("\r\n");
				serializer.CreateObject(objectData);
				serializer.InitializeObject(objectData);
				return true;
			}
			return false;
		}

		public void WriteChildren(AbstractNodeObjectData parent)
		{
			if (parent == null || parent.Children == null)
			{
				return;
			}
			if (parent.Children != null)
			{
				ILuaObjectSerializer serializer = LuaObjectManager.GetSerializer(parent);
				if (serializer == null)
				{
					return;
				}
				foreach (AbstractNodeObjectData abstractNodeObjectData in parent.Children)
				{
					bool flag = this.WriteObject(abstractNodeObjectData);
					if (flag)
					{
						serializer.AddChild(parent, abstractNodeObjectData);
						this.WriteChildren(abstractNodeObjectData);
					}
				}
			}
		}

		public override bool CanSerialize(BaseObjectData objectData)
		{
			return typeof(NodeObjectData) == objectData.GetType();
		}

		public override void CreateObject(BaseObjectData objectData)
		{
			AbstractNodeObjectData abstractNodeObjectData = objectData as AbstractNodeObjectData;
			if (abstractNodeObjectData.ScriptData == null)
			{
				this.OnCreateObject(objectData);
				return;
			}
			string pathString = Path.GetFileName(abstractNodeObjectData.ScriptData.RelativeScriptFile).Replace(".lua", null);
			base.Write("localLuaFile=require(\"");
			base.Write(base.ToStringHelper.ToStringWithCulture("LuaScript"));
			base.Write(".");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.LuaPathFormat(pathString)));
			base.Write("\")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(abstractNodeObjectData.Name)));
			base.Write("=localLuaFile.CreateCustomNode()\r\n");
		}

		protected virtual void OnCreateObject(BaseObjectData objectData)
		{
			AbstractNodeObjectData abstractNodeObjectData = objectData as AbstractNodeObjectData;
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameDeclaration(abstractNodeObjectData.Name)));
			base.Write("=cc.Node:create()\r\n");
		}

		public override void InitializeObject(BaseObjectData objectData)
		{
			base.InitializeObject(objectData);
			this.InitializeNode(objectData);
			this.InitializeCallback(objectData);
			this.InitializeLayout(objectData);
		}

		private ScaleValue getAnchorPointDefaultValue(NodeObjectData objectData)
		{
			if (objectData is PanelObjectData)
			{
				return LuaNodeObject.anchorPointLeftDown;
			}
			if (objectData is WidgetObjectData || objectData is SpriteObjectData)
			{
				return LuaNodeObject.anchorPointMiddle;
			}
			return LuaNodeObject.anchorPointLeftDown;
		}

		private void InitializeNode(BaseObjectData objectData)
		{
			NodeObjectData nodeObjectData = objectData as NodeObjectData;
			if (nodeObjectData == null)
			{
				return;
			}
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
			base.Write(":setTag(");
			base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Tag));
			base.Write(")\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
			base.Write(":setCascadeColorEnabled(true)\r\n");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
			base.Write(":setCascadeOpacityEnabled(true)\r\n");
			if (base.CanExport<bool>(nodeObjectData.VisibleForFrame, true))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setVisible(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.VisibleForFrame));
				base.Write(")\r\n");
			}
			if (nodeObjectData.AnchorPoint != null && base.CanExport<ScaleValue>(nodeObjectData.AnchorPoint, this.getAnchorPointDefaultValue(nodeObjectData)))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setAnchorPoint(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.AnchorPoint));
				base.Write(")\r\n");
			}
			if (base.CanExport<PointF>(nodeObjectData.Position, PointF.Empty))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setPosition(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Position));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.Scale.ScaleX, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setScaleX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Scale.ScaleX));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.Scale.ScaleY, 1f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setScaleY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Scale.ScaleY));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.RotationSkewX, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setRotationSkewX(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.RotationSkewX));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.RotationSkewY, 0f))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setRotationSkewY(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.RotationSkewY));
				base.Write(")\r\n");
			}
			if (base.CanExport<int>(nodeObjectData.Alpha, 255))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setOpacity(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Alpha));
				base.Write(")\r\n");
			}
			if (base.CanExport<ColorData>(nodeObjectData.CColor, ColorData.White))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
				base.Write(":setColor(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.CColor));
				base.Write(")\r\n");
			}
		}

		protected virtual void InitializeLayout(BaseObjectData objectData)
		{
			NodeObjectData nodeObjectData = objectData as NodeObjectData;
			if (nodeObjectData == null)
			{
				return;
			}
			base.Write("layout = ccui.LayoutComponent:bindLayoutComponent(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(nodeObjectData.Name)));
			base.Write(")\r\n");
			if (base.CanExport<bool>(nodeObjectData.PositionPercentXEnabled, false))
			{
				base.Write("layout:setPositionPercentXEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PositionPercentXEnabled));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(nodeObjectData.PositionPercentYEnabled, false))
			{
				base.Write("layout:setPositionPercentYEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PositionPercentYEnabled));
				base.Write(")\r\n");
			}
			if (nodeObjectData.PrePosition != null)
			{
				if (base.CanExport<float>(nodeObjectData.PrePosition.X, 0f))
				{
					base.Write("layout:setPositionPercentX(");
					base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PrePosition.X));
					base.Write(")\r\n");
				}
				if (base.CanExport<float>(nodeObjectData.PrePosition.Y, 0f))
				{
					base.Write("layout:setPositionPercentY(");
					base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PrePosition.Y));
					base.Write(")\r\n");
				}
			}
			if (base.CanExport<bool>(nodeObjectData.PercentWidthEnable, false))
			{
				base.Write("layout:setPercentWidthEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PercentWidthEnable));
				base.Write(")\r\n");
			}
			if (base.CanExport<bool>(nodeObjectData.PercentHeightEnable, false))
			{
				base.Write("layout:setPercentHeightEnabled(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PercentHeightEnable));
				base.Write(")\r\n");
			}
			if (nodeObjectData.PreSize != null)
			{
				if (base.CanExport<float>(nodeObjectData.PreSize.Width, 0f))
				{
					base.Write("layout:setPercentWidth(");
					base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PreSize.Width));
					base.Write(")\r\n");
				}
				if (base.CanExport<float>(nodeObjectData.PreSize.Height, 0f))
				{
					base.Write("layout:setPercentHeight(");
					base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.PreSize.Height));
					base.Write(")\r\n");
				}
			}
			if (nodeObjectData.Size != null && base.CanExport<SizeF>(nodeObjectData.Size, SizeF.Zero))
			{
				base.Write("layout:setSize(cc.size(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.Size));
				base.Write("))\r\n");
			}
			if (base.CanExport<HorizontalBerthEdge>(nodeObjectData.HorizontalEdge, HorizontalBerthEdge.None))
			{
				base.Write("layout:setHorizontalEdge(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)nodeObjectData.HorizontalEdge));
				base.Write(")\r\n");
			}
			if (base.CanExport<VerticalBerthEdge>(nodeObjectData.VerticalEdge, VerticalBerthEdge.None))
			{
				base.Write("layout:setVerticalEdge(");
				base.Write(base.ToStringHelper.ToStringWithCulture((int)nodeObjectData.VerticalEdge));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.LeftMargin, 0f))
			{
				base.Write("layout:setLeftMargin(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.LeftMargin));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.RightMargin, 0f))
			{
				base.Write("layout:setRightMargin(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.RightMargin));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.TopMargin, 0f))
			{
				base.Write("layout:setTopMargin(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.TopMargin));
				base.Write(")\r\n");
			}
			if (base.CanExport<float>(nodeObjectData.BottomMargin, 0f))
			{
				base.Write("layout:setBottomMargin(");
				base.Write(base.ToStringHelper.ToStringWithCulture(nodeObjectData.BottomMargin));
				base.Write(")\r\n");
			}
		}

		protected void InitializeCallback(BaseObjectData objectData)
		{
			AbstractNodeObjectData abstractNodeObjectData = objectData as AbstractNodeObjectData;
			if (abstractNodeObjectData == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(abstractNodeObjectData.UserData))
			{
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(".UserData = {}\r\n");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(".UserData[1] = [==========[");
				base.Write(base.ToStringHelper.ToStringWithCulture(abstractNodeObjectData.UserData));
				base.Write("]==========]\r\n");
			}
			if (string.IsNullOrEmpty(abstractNodeObjectData.CallBackName))
			{
				return;
			}
			if (abstractNodeObjectData.CallBackType == EnumCallBack.None)
			{
				return;
			}
			base.Write("if callBackProvider~=nil then\r\n");
			switch (abstractNodeObjectData.CallBackType)
			{
			case EnumCallBack.Touch:
				base.Write("      ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(":addTouchEventListener(callBackProvider(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(LuaObjectSerializer.FileName));
				base.Write("\", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(abstractNodeObjectData.CallBackName));
				base.Write("\"))\r\n");
				break;
			case EnumCallBack.Click:
				base.Write("      ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(":addClickEventListener(callBackProvider(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(LuaObjectSerializer.FileName));
				base.Write("\", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(abstractNodeObjectData.CallBackName));
				base.Write("\"))\r\n");
				break;
			case EnumCallBack.Event:
				base.Write("      ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(":addEventListener(callBackProvider(\"");
				base.Write(base.ToStringHelper.ToStringWithCulture(LuaObjectSerializer.FileName));
				base.Write("\", ");
				base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(abstractNodeObjectData.Name)));
				base.Write(", \"");
				base.Write(base.ToStringHelper.ToStringWithCulture(abstractNodeObjectData.CallBackName));
				base.Write("\"))\r\n");
				break;
			}
			base.Write("end\r\n");
		}

		public override void AddChild(BaseObjectData parent, BaseObjectData child)
		{
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(parent.Name)));
			base.Write(":addChild(");
			base.Write(base.ToStringHelper.ToStringWithCulture(base.GetNameString(child.Name)));
			base.Write(")\r\n");
		}

		private static readonly ScaleValue anchorPointMiddle = ScaleValue.Half;

		private static readonly ScaleValue anchorPointLeftDown = ScaleValue.Empty;
	}
}
