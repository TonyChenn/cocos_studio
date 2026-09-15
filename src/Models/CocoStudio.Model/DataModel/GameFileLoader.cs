using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.Model.DataModel
{
	internal static class GameFileLoader
	{
		public static bool IsLoading { get; private set; }

		public static GameFileLoadResult LoadProject(GameFileData objectData)
		{
			GameFileLoadResult result;
			if (objectData == null || objectData.ObjectData == null)
			{
				result = null;
			}
			else
			{
				GameFileLoader.IsLoading = true;
				GameFileLoadResult gameFileLoadResult = new GameFileLoadResult();
				Dictionary<int, VisualObject> dictionary = new Dictionary<int, VisualObject>();
				LayoutExtender.LayoutEnabled = false;
				bool isCreateDefaultRecorder = BaseRecorder.IsCreateDefaultRecorder;
				BaseRecorder.IsCreateDefaultRecorder = false;
				gameFileLoadResult.RootObject = GameFileLoader.ConvertObject(objectData.ObjectData, gameFileLoadResult, dictionary);
				gameFileLoadResult.RootObject.CanEdit = true;
				LayoutExtender.LayoutEnabled = true;
				BaseRecorder.IsCreateDefaultRecorder = isCreateDefaultRecorder;
				if (isCreateDefaultRecorder)
				{
					GameFileLoader.RefreshObjectsRecorder(gameFileLoadResult.RootObject);
				}
				if (objectData.Animation != null)
				{
					gameFileLoadResult.TimelineAction = GameFileLoader.ConvertTimelineAction(objectData.Animation, gameFileLoadResult.RootObject, dictionary);
					List<AnimationInfo> list = GameFileLoader.ConvertAnimationList(objectData);
					ObservableCollection<AnimationInfo> animationInfoList = gameFileLoadResult.TimelineAction.AnimationInfoList;
					foreach (AnimationInfo animationInfo in list)
					{
						animationInfoList.Add(animationInfo);
						if (animationInfo.Name == objectData.Animation.ActivedAnimationName)
						{
							gameFileLoadResult.TimelineAction.ActivedAnimationInfo = animationInfo;
						}
					}
				}
				GameFileLoader.IsLoading = false;
				result = gameFileLoadResult;
			}
			return result;
		}

		public static GameFileData SaveProject(AbstractNodeObject vObject, TimelineAction action)
		{
			GameFileData result;
			if (vObject == null)
			{
				result = null;
			}
			else
			{
				AbstractNodeObjectData objectData = GameFileLoader.ConvertObjectData(vObject);
				TimelineActionData timelineActionData = new TimelineActionData();
				timelineActionData.Duration = action.Duration;
				timelineActionData.Speed = action.Speed;
				if (action.ActivedAnimationInfo != null)
				{
					timelineActionData.ActivedAnimationName = action.ActivedAnimationInfo.Name;
				}
				GameFileLoader.ConvertTimelineActionData(vObject, timelineActionData);
				GameFileData gameFileData = new GameFileData();
				gameFileData.ObjectData = objectData;
				gameFileData.Animation = timelineActionData;
				List<AnimationInfoData> list = new List<AnimationInfoData>(action.AnimationInfoList.Count);
				GameFileLoader.ConvertAnimationListData(action, list);
				gameFileData.AnimationList = list;
				result = gameFileData;
			}
			return result;
		}

		private static void RefreshObjectsRecorder(AbstractNodeObject vObject)
		{
			vObject.BindingRecorder(null);
			Parallel.ForEach<AbstractNodeObject>(vObject.Children, delegate(AbstractNodeObject child)
			{
				GameFileLoader.RefreshObjectsRecorder(child);
			});
		}

		public static void FindProjectNodeToReload(AbstractNodeObject parentNode)
		{
			if (parentNode != null)
			{
				if (parentNode is FileNodeObject)
				{
					((FileNodeObject)parentNode).Reload();
				}
				if (parentNode.Children != null)
				{
					foreach (AbstractNodeObject parentNode2 in parentNode.Children)
					{
						GameFileLoader.FindProjectNodeToReload(parentNode2);
					}
				}
			}
		}

		private static AbstractNodeObject ConvertObject(AbstractNodeObjectData objectData, GameFileLoadResult gResult, Dictionary<int, VisualObject> objectDictionary)
		{
			IDataConvert dataConvert = objectData as IDataConvert;
			AbstractNodeObject abstractNodeObject = null;
			if (dataConvert != null)
			{
				abstractNodeObject = (dataConvert.CreateViewModel() as AbstractNodeObject);
			}
			else
			{
				Type viewModelType = Services.ProjectsService.DataModelManager.GetViewModelType(objectData.GetType());
				ModelMetaData metaData = ModelManager.Instance.GetMetaData(viewModelType, objectData.ScriptData);
				if (null != metaData)
				{
					abstractNodeObject = metaData.CreateObject();
				}
				else
				{
					abstractNodeObject = (Activator.CreateInstance(viewModelType) as AbstractNodeObject);
				}
			}
			AbstractNodeObject result;
			if (abstractNodeObject == null)
			{
				result = null;
			}
			else
			{
				PropertyAccessorHandler[] properties = objectData.GetProperties();
				foreach (PropertyAccessorHandler propertyAccessorHandler in properties)
				{
					string propertyName = propertyAccessorHandler.PropertyName;
					if (!(propertyName == "Children"))
					{
						PropertyInfo property = abstractNodeObject.GetType().GetProperty(propertyName);
						if (!(property == null))
						{
							object obj = propertyAccessorHandler.GetValue(objectData, null);
							object value = obj;
							if (obj != null)
							{
								if (!property.PropertyType.Equals(propertyAccessorHandler.PropertyType))
								{
									if (!(obj is IDataConvert))
									{
										string message = string.Format("Property type are not same, the item is {0}, ViewType is {1}, DataType is {2}, Can use IDataConvert interface to convert.", abstractNodeObject.GetType().Name, property.PropertyType.Name, propertyAccessorHandler.PropertyType.Name);
										throw new InvalidCastException(message);
									}
									value = ((IDataConvert)obj).CreateViewModel();
								}
							}
							property.SetValue(abstractNodeObject, value, null);
						}
					}
				}
				if (!objectDictionary.ContainsKey(abstractNodeObject.ActionTag))
				{
					objectDictionary.Add(abstractNodeObject.ActionTag, abstractNodeObject);
				}
				if (objectData != null)
				{
					((IDataInitialize)objectData).DataInitialize(abstractNodeObject);
				}
				gResult.Names.Add(abstractNodeObject.Name);
				if (objectData.Children != null)
				{
					foreach (AbstractNodeObjectData objectData2 in objectData.Children)
					{
						AbstractNodeObject abstractNodeObject2 = GameFileLoader.ConvertObject(objectData2, gResult, objectDictionary);
						if (abstractNodeObject2 != null)
						{
							abstractNodeObject.Children.Add(abstractNodeObject2);
						}
					}
				}
				result = abstractNodeObject;
			}
			return result;
		}

		private static AbstractNodeObjectData ConvertObjectData(AbstractNodeObject nObject)
		{
			Type dataModelType = Services.ProjectsService.DataModelManager.GetDataModelType(nObject.GetType());
			AbstractNodeObjectData abstractNodeObjectData = Activator.CreateInstance(dataModelType, true) as AbstractNodeObjectData;
			AbstractNodeObjectData result;
			if (abstractNodeObjectData == null)
			{
				result = null;
			}
			else
			{
				PropertyAccessorHandler[] properties = abstractNodeObjectData.GetProperties();
				foreach (PropertyAccessorHandler propertyAccessorHandler in properties)
				{
					string propertyName = propertyAccessorHandler.PropertyName;
					if (!(propertyName == "Children"))
					{
						PropertyInfo property = nObject.GetType().GetProperty(propertyName);
						if (!(property == null))
						{
							object value = property.GetValue(nObject, null);
							if (value != null)
							{
								object obj = value;
								if (!propertyAccessorHandler.PropertyType.Equals(property.PropertyType))
								{
									obj = Activator.CreateInstance(propertyAccessorHandler.PropertyType, true);
									if (!(obj is IDataConvert))
									{
										string message = string.Format("Property type are not same, the item is {0}, ViewType is {1}, DataType is {2}, Can use IDataConvert interface to convert.", nObject.GetType().Name, property.PropertyType.Name, propertyAccessorHandler.PropertyType.Name);
										throw new InvalidCastException(message);
									}
									((IDataConvert)obj).SetData(value);
								}
								propertyAccessorHandler.SetValue(abstractNodeObjectData, obj, null);
							}
						}
					}
				}
				if (nObject.Children == null || nObject.Children.Count <= 0)
				{
					result = abstractNodeObjectData;
				}
				else
				{
					abstractNodeObjectData.Children = new List<AbstractNodeObjectData>();
					foreach (AbstractNodeObject nObject2 in nObject.Children)
					{
						AbstractNodeObjectData item = GameFileLoader.ConvertObjectData(nObject2);
						abstractNodeObjectData.Children.Add(item);
					}
					result = abstractNodeObjectData;
				}
			}
			return result;
		}

		private static TimelineAction ConvertTimelineAction(TimelineActionData timelineObjectData, VisualObject vObject, Dictionary<int, VisualObject> objectTagDictionary)
		{
			TimelineAction timelineAction = new TimelineAction();
			timelineAction.Duration = timelineObjectData.Duration;
			timelineAction.Speed = timelineObjectData.Speed;
			TimelineAction result;
			if (timelineObjectData.Timelines == null || timelineObjectData.Timelines.Count <= 0)
			{
				result = timelineAction;
			}
			else
			{
				foreach (TimelineData timelineData in timelineObjectData.Timelines)
				{
					if (objectTagDictionary.ContainsKey(timelineData.ActionTag))
					{
						AbstractNodeObject abstractNodeObject = objectTagDictionary[timelineData.ActionTag] as AbstractNodeObject;
						if (abstractNodeObject != null)
						{
							string property = timelineData.Property;
							PropertyInfo property2 = abstractNodeObject.GetType().GetProperty(property);
							Timeline timeline = Timeline.CreateTimeline(property2, abstractNodeObject);
							if (timelineData.Frames != null && timelineData.Frames.Count > 0)
							{
								foreach (FrameData objectData in timelineData.Frames)
								{
									Frame frame = GameFileLoader.ConvertTimeLineFrame(objectData, property2);
									if (frame == null)
									{
										LogConfig.Logger.Error("Can not create frame, the frame type is " + property2.Name);
									}
									else
									{
										timeline.Frames.Add(frame);
									}
								}
							}
						}
					}
				}
				result = timelineAction;
			}
			return result;
		}

		private static Frame ConvertTimeLineFrame(FrameData objectData, PropertyInfo propertyInfo)
		{
			Frame frame = FrameTypeManager.Instance.CreateFrame(propertyInfo);
			Frame result;
			if (frame == null)
			{
				result = null;
			}
			else
			{
				PropertyInfo[] properties = objectData.GetType().GetProperties();
				foreach (PropertyInfo propertyInfo2 in properties)
				{
					string name = propertyInfo2.Name;
					PropertyInfo property = frame.GetType().GetProperty(name);
					object value = propertyInfo2.GetValue(objectData, null);
					if (property != null && value != null)
					{
						object value2 = value;
						if (!property.PropertyType.Equals(propertyInfo2.PropertyType))
						{
							if (!(value is IDataConvert))
							{
								string message = string.Format("Property type are not same, the item is {0}, ViewType is {1}, DataType is {2}, Can use IDataConvert interface to convert.", frame.GetType().Name, property.PropertyType.Name, propertyInfo2.PropertyType.Name);
								throw new InvalidCastException(message);
							}
							value2 = ((IDataConvert)value).CreateViewModel();
						}
						property.SetValue(frame, value2, null);
					}
				}
				frame.BindingRecorder(null);
				result = frame;
			}
			return result;
		}

		private static List<AnimationInfo> ConvertAnimationList(GameFileData objectData)
		{
			List<AnimationInfo> list = new List<AnimationInfo>();
			foreach (AnimationInfoData animationInfoData in objectData.AnimationList)
			{
				list.Add(new AnimationInfo(animationInfoData.Name, animationInfoData.StartIndex, animationInfoData.EndIndex)
				{
					RenderColor = animationInfoData.RenderColor
				});
			}
			return list;
		}

		private static void ConvertAnimationListData(TimelineAction action, List<AnimationInfoData> animadatalist)
		{
			foreach (AnimationInfo animationInfo in action.AnimationInfoList)
			{
				animadatalist.Add(new AnimationInfoData
				{
					Name = animationInfo.Name,
					StartIndex = animationInfo.StartIndex,
					EndIndex = animationInfo.EndIndex,
					RenderColor = animationInfo.RenderColor
				});
			}
		}

		private static void ConvertTimelineActionData(AbstractNodeObject nObject, TimelineActionData nTimelineActionData)
		{
			foreach (AbstractNodeObject abstractNodeObject in nObject.Children)
			{
				GameFileLoader.ConvertTimelineActionData(abstractNodeObject, nTimelineActionData);
				foreach (Timeline timeline in abstractNodeObject.Timelines)
				{
					if (timeline.Frames.Count > 0)
					{
						TimelineData timelineData = new TimelineData();
						timelineData.ActionTag = abstractNodeObject.ActionTag;
						timelineData.Property = timeline.PropertyInfo.Name;
						timelineData.Frames = new List<FrameData>();
						foreach (Frame frame in timeline.Frames)
						{
							Type dataModelType = Services.ProjectsService.DataModelManager.GetDataModelType(frame.GetType());
							FrameData frameData = Activator.CreateInstance(dataModelType, true) as FrameData;
							if (frameData != null)
							{
								PropertyInfo[] properties = frame.GetType().GetProperties();
								foreach (PropertyInfo propertyInfo in properties)
								{
									string name = propertyInfo.Name;
									if (!(name == "Children"))
									{
										PropertyInfo property = frameData.GetType().GetProperty(name);
										object value = propertyInfo.GetValue(frame, null);
										if (property != null && value != null)
										{
											object obj = value;
											if (!property.PropertyType.Equals(propertyInfo.PropertyType))
											{
												obj = Activator.CreateInstance(property.PropertyType, true);
												if (!(obj is IDataConvert))
												{
													string message = string.Format("Property type are not same, the item is {0}, ViewType is {1}, DataType is {2}, Can use IDataConvert interface to convert.", nObject.GetType().Name, propertyInfo.PropertyType.Name, property.PropertyType.Name);
													throw new InvalidCastException(message);
												}
												((IDataConvert)obj).SetData(value);
											}
											property.SetValue(frameData, obj, null);
										}
									}
								}
								timelineData.Frames.Add(frameData);
							}
						}
						nTimelineActionData.Timelines.Add(timelineData);
					}
				}
			}
		}
	}
}
