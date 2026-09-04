using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000087 RID: 135
	public class FrameTypeManager
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00013EB8 File Offset: 0x000120B8
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x00013ECE File Offset: 0x000120CE
		public static FrameTypeManager Instance { get; private set; } = new FrameTypeManager();

		// Token: 0x06000497 RID: 1175 RVA: 0x00013EE4 File Offset: 0x000120E4
		private FrameTypeManager()
		{
			this.LoadFrameTypes();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00013EF8 File Offset: 0x000120F8
		private void LoadFrameTypes()
		{
			try
			{
				ExtensionNodeList<FrameExtensionNode> extensionNodes = AddinManager.GetExtensionNodes<FrameExtensionNode>(typeof(Frame));
				this.frameCollection = new Dictionary<Type, Type>();
				foreach (FrameExtensionNode frameExtensionNode in extensionNodes)
				{
					this.frameCollection.Add(frameExtensionNode.Data.DataType, frameExtensionNode.Type);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load model type failed.", exception);
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00013FAC File Offset: 0x000121AC
		private Type GetFrameType(Type dataType)
		{
			Type result;
			this.frameCollection.TryGetValue(dataType, out result);
			return result;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00013FD0 File Offset: 0x000121D0
		private FramePropertyAttribute GetFrameAttribute(PropertyInfo propertyInfo)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FramePropertyAttribute), false);
			FramePropertyAttribute result;
			if (customAttributes.Length > 0)
			{
				FramePropertyAttribute framePropertyAttribute = customAttributes[0] as FramePropertyAttribute;
				result = framePropertyAttribute;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00014010 File Offset: 0x00012210
		public bool IsAutoCreateFrame(PropertyInfo propertyInfo)
		{
			FramePropertyAttribute frameAttribute = this.GetFrameAttribute(propertyInfo);
			return frameAttribute != null && frameAttribute.IsAutoCreate;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001403C File Offset: 0x0001223C
		public bool CanCreateFrame(PropertyInfo propertyInfo)
		{
			FramePropertyAttribute frameAttribute = this.GetFrameAttribute(propertyInfo);
			return frameAttribute != null;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00014068 File Offset: 0x00012268
		public bool CanCreateFrame(PropertyInfo propertyInfo, out Frame frame)
		{
			frame = this.CreateFrame(propertyInfo);
			return frame != null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00014094 File Offset: 0x00012294
		public Frame CreateFrame(PropertyInfo propertyInfo)
		{
			FramePropertyAttribute frameAttribute = this.GetFrameAttribute(propertyInfo);
			Frame result;
			if (frameAttribute == null)
			{
				result = null;
			}
			else
			{
				Type frameType = frameAttribute.FrameType;
				if (frameType == null)
				{
					frameType = this.GetFrameType(propertyInfo.PropertyType);
				}
				Frame frame = null;
				try
				{
					if (frameType != null)
					{
						frame = (Activator.CreateInstance(frameType) as Frame);
						frame.PropertyHandler = new PropertyAccessorHandler(propertyInfo);
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Create frame failed.", exception);
				}
				result = frame;
			}
			return result;
		}

		// Token: 0x0400022D RID: 557
		private Dictionary<Type, Type> frameCollection;
	}
}
