using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	public class FrameTypeManager
	{
		public static FrameTypeManager Instance { get; private set; } = new FrameTypeManager();

		private FrameTypeManager()
		{
			this.LoadFrameTypes();
		}

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

		private Type GetFrameType(Type dataType)
		{
			Type result;
			this.frameCollection.TryGetValue(dataType, out result);
			return result;
		}

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

		public bool IsAutoCreateFrame(PropertyInfo propertyInfo)
		{
			FramePropertyAttribute frameAttribute = this.GetFrameAttribute(propertyInfo);
			return frameAttribute != null && frameAttribute.IsAutoCreate;
		}

		public bool CanCreateFrame(PropertyInfo propertyInfo)
		{
			FramePropertyAttribute frameAttribute = this.GetFrameAttribute(propertyInfo);
			return frameAttribute != null;
		}

		public bool CanCreateFrame(PropertyInfo propertyInfo, out Frame frame)
		{
			frame = this.CreateFrame(propertyInfo);
			return frame != null;
		}

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

		private Dictionary<Type, Type> frameCollection;
	}
}
