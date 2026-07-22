using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using EditorCommon.JsonModel;
using Gdk;
using Newtonsoft.Json.Linq;

namespace Modules.Communal.ProjectsConvertor
{
	// Token: 0x02000002 RID: 2
	public class AnimationConveter
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		private void Initialize()
		{
			this.rootData = new SkeletonNodeObjectData();
			this.timelineData = new TimelineActionData();
			this.boneMap.Clear();
			this.textureList.Clear();
			this.totalFrameCount = 0;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public void ConvertJson(GameFileData gameFileData, string filePath)
		{
			this.Initialize();
			this.gameFileData = gameFileData;
			gameFileData.ObjectData = this.rootData;
			this.timelineData = gameFileData.Animation;
			if (gameFileData.AnimationList == null)
			{
				gameFileData.AnimationList = new List<AnimationInfoData>();
			}
			this.rootData.CanEdit = false;
			if (this.rootData.Children == null)
			{
				this.rootData.Children = new List<AbstractNodeObjectData>();
			}
			if (!File.Exists(filePath))
			{
				return;
			}
			string json = File.ReadAllText(filePath);
			JObject jobject = JObject.Parse(json);
			try
			{
				JToken jtoken = jobject["content_scale"];
				if (jtoken != null)
				{
					this.rootData.Scale = new ScaleValue((float)jtoken, (float)jtoken, 0.1, -99999999.0, 99999999.0);
				}
				JsonFileHelp.plistfilehelper.Clear();
				JArray jarray = jobject["config_file_path"] as JArray;
				if (jarray != null)
				{
					this.absjsondir = Path.GetDirectoryName(filePath);
					for (int i = 0; i < jarray.Count; i++)
					{
						string text = Path.Combine(this.absjsondir, (string)jarray[i]);
						if (!File.Exists(text))
						{
							text = Path.Combine(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath, (string)jarray[i]);
						}
						try
						{
							JsonFileHelp.plistfilehelper.AddPlistConfigFile(text);
						}
						catch (ArgumentException)
						{
						}
					}
				}
				JArray textureData = jobject["texture_data"] as JArray;
				this.ConvertTextureData(textureData);
				JArray armatureDataArray = jobject["armature_data"] as JArray;
				this.ConvertArmatureData(armatureDataArray);
				JArray animationData = jobject["animation_data"] as JArray;
				this.ConvertAnimationData(animationData);
				this.timelineData.Duration = this.maxRealFrameIndex + 1;
				AnimationConveter.ClearRepeatFrames(this.timelineData);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
				throw ex;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000022E8 File Offset: 0x000004E8
		private void ConvertArmatureData(JArray armatureDataArray)
		{
			if (armatureDataArray == null)
			{
				return;
			}
			this.rootData.Name = (string)armatureDataArray[0]["name"];
			JArray boneDataArray = armatureDataArray[0]["bone_data"] as JArray;
			this.ConvertBoneData(boneDataArray);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002338 File Offset: 0x00000538
		private void ConvertBoneData(JArray boneDataArray)
		{
			if (boneDataArray == null)
			{
				return;
			}
			if (this.rootData.Size == null)
			{
				this.rootData.Size = CocoStudio.Model.SizeF.Empty;
			}
			for (int i = 0; i < boneDataArray.Count<JToken>(); i++)
			{
				JObject jobject = boneDataArray[i] as JObject;
				BoneNodeObjectData boneNodeObjectData = new BoneNodeObjectData();
				this.InitNodeProp(boneNodeObjectData);
				boneNodeObjectData.Name = (string)jobject["name"];
				float num = (float)jobject["x"];
				float num2 = (float)jobject["y"];
				boneNodeObjectData.Position = new Gdk.Point((int)num, (int)num2);
				boneNodeObjectData.ZOrder = (int)jobject["z"];
				boneNodeObjectData.Scale = new ScaleValue((float)jobject["cX"], (float)jobject["cY"], 0.1, -99999999.0, 99999999.0);
				boneNodeObjectData.RotationSkewX = (float)jobject["kX"] * 180f / 3.1415927f;
				boneNodeObjectData.RotationSkewY = -(float)jobject["kY"] * 180f / 3.1415927f;
				double num3 = (double)jobject["arrow_x"];
				double num4 = (double)jobject["arrow_y"];
				float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
				boneNodeObjectData.Length = ((num5 < (float)BoneObject.MINLENGHT) ? ((float)BoneObject.MINLENGHT) : num5);
				string key = (string)jobject["parent"];
				_ = (bool)jobject["effectbyskeleton"];
				if (this.boneMap.ContainsKey(key))
				{
					BoneNodeObjectData boneNodeObjectData2 = this.boneMap[key];
					boneNodeObjectData2.Children.Add(boneNodeObjectData);
				}
				else
				{
					this.rootData.Children.Add(boneNodeObjectData);
				}
				this.boneMap[boneNodeObjectData.Name] = boneNodeObjectData;
				JArray jarray = jobject["display_data"] as JArray;
				if (jarray != null)
				{
					for (int j = 0; j < jarray.Count<JToken>(); j++)
					{
						JToken jtoken = jarray[j];
						int num6 = (int)jtoken["displayType"];
						if (num6 == 0)
						{
							SpriteObjectData spriteObjectData = new SpriteObjectData();
							this.InitNodeProp(spriteObjectData);
							string text = (string)jtoken["name"];
							spriteObjectData.Name = text;
							if (string.IsNullOrEmpty(Path.GetExtension(text)))
							{
								text += ".png";
							}
							if (this.textureFileMap.ContainsKey(text))
							{
								Tuple<CocoStudio.Model.SizeF, ScaleValue, ResourceItemData> tuple = this.textureFileMap[text];
								spriteObjectData.FileData = tuple.Item3;
								spriteObjectData.AnchorPoint = tuple.Item2;
								spriteObjectData.Size = tuple.Item1;
								if (spriteObjectData.Size == null || (spriteObjectData.Size.Width == 0f && spriteObjectData.Size.Height == 0f))
								{
									string filename = Services.ProjectsService.GetFullPath(spriteObjectData.FileData);
									int num7 = 0;
									int num8 = 0;
									Pixbuf.GetFileInfo(filename, out num7, out num8);
									spriteObjectData.Size = new CocoStudio.Model.SizeF((float)num7, (float)num8);
								}
							}
							else
							{
								spriteObjectData.FileData = new ResourceItemData(text);
							}
							JArray jarray2 = jtoken["skin_data"] as JArray;
							if (jarray2 != null)
							{
								JObject jobject2 = jarray2[0] as JObject;
								float x = (float)jobject2["x"];
								float y = (float)jobject2["y"];
								spriteObjectData.Position = new CocoStudio.Model.PointF(x, y);
								spriteObjectData.Scale = new ScaleValue((float)jobject2["cX"], (float)jobject2["cY"], 0.1, -99999999.0, 99999999.0);
								spriteObjectData.RotationSkewX = (float)jobject2["kX"] * 180f / 3.1415927f;
								spriteObjectData.RotationSkewY = -(float)jobject2["kY"] * 180f / 3.1415927f;
							}
							boneNodeObjectData.Children.Add(spriteObjectData);
							string name = text.Substring(0, text.LastIndexOf("."));
							spriteObjectData.Name = name;
							this.textureList.Add(spriteObjectData);
						}
						else if (2 == num6)
						{
							ParticleObjectData particleObjectData = new ParticleObjectData();
							this.InitNodeProp(particleObjectData);
							string text2 = (string)jtoken["plist"];
							particleObjectData.FileData = new ResourceItemData(text2);
							particleObjectData.Name = text2;
							boneNodeObjectData.Children.Add(particleObjectData);
							this.textureList.Add(particleObjectData);
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002848 File Offset: 0x00000A48
		private void ConvertAnimationData(JArray animationData)
		{
			if (animationData == null)
			{
				return;
			}
			JArray movData = animationData[0]["mov_data"] as JArray;
			this.ConvertMovData(movData);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002878 File Offset: 0x00000A78
		private void ConvertMovData(JArray movData)
		{
			if (movData == null)
			{
				return;
			}
			this.gameFileData.AnimationList.Clear();
			int i = 0;
			int num = movData.Count<JToken>();
			this.totalFrameCount = 0;
			while (i < num)
			{
				string name = (string)movData[i]["name"];
				JArray movBoneData = movData[i]["mov_bone_data"] as JArray;
				float speed = (float)movData[i]["sc"];
				this.gameFileData.Animation.Speed = speed;
				if (this.maxRealFrameIndex != 0)
				{
					this.totalFrameCount = this.maxRealFrameIndex + 1;
				}
				this.animationstart = this.totalFrameCount;
				this.StopAnimation(this.animationstart);
				this.ConvertMovBoneData(movBoneData);
				AnimationInfoData animationInfoData = new AnimationInfoData();
				animationInfoData.Name = name;
				animationInfoData.StartIndex = this.animationstart;
				animationInfoData.EndIndex = this.maxRealFrameIndex;
				animationInfoData.RenderColor = AnimationInfo.GetRandomColor();
				this.gameFileData.AnimationList.Add(animationInfoData);
				i++;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002994 File Offset: 0x00000B94
		private void ConvertMovBoneData(JArray movBoneData)
		{
			int i = 0;
			int num = movBoneData.Count<JToken>();
			BoneNodeObjectData boneNodeObjectData = null;
			this.minRealFrameIndex = (this.maxRealFrameIndex = this.animationstart);
			this.setAnimationClipFrameSpan(movBoneData);
			while (i < num)
			{
				string key = (string)movBoneData[i]["name"];
				if (this.boneMap.ContainsKey(key))
				{
					boneNodeObjectData = this.boneMap[key];
				}
				if (boneNodeObjectData != null)
				{
					JArray frameData = movBoneData[i]["frame_data"] as JArray;
					this.ConvertFrameData(boneNodeObjectData, frameData);
				}
				i++;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002A2C File Offset: 0x00000C2C
		private void setAnimationClipFrameSpan(JArray movBoneData)
		{
			int i = 0;
			int num = movBoneData.Count<JToken>();
			BoneNodeObjectData boneNodeObjectData = null;
			while (i < num)
			{
				string key = (string)movBoneData[i]["name"];
				if (this.boneMap.ContainsKey(key))
				{
					boneNodeObjectData = this.boneMap[key];
				}
				if (boneNodeObjectData != null)
				{
					JArray jarray = movBoneData[i]["frame_data"] as JArray;
					int j = 0;
					int num2 = jarray.Count<JToken>();
					int num3 = 0;
					int num4 = 0;
					while (j < num2)
					{
						JToken jtoken = jarray[j];
						JToken jtoken2 = jtoken["fi"];
						int num5;
						if (jtoken2 != null)
						{
							num5 = (int)jtoken["fi"];
						}
						else
						{
							num5 = j;
						}
						if (j == 0)
						{
							num3 = num5;
							num4 = num5;
						}
						else
						{
							if (num5 > num4)
							{
								num4 = num5;
							}
							if (num5 < num3)
							{
								num3 = num5;
							}
						}
						j++;
					}
					num3 += this.totalFrameCount;
					num4 += this.totalFrameCount;
					if (i == 0)
					{
						this.minRealFrameIndex = num3;
						this.maxRealFrameIndex = num4;
					}
					else
					{
						this.minRealFrameIndex = ((this.minRealFrameIndex > num3) ? num3 : this.minRealFrameIndex);
						this.maxRealFrameIndex = ((this.maxRealFrameIndex < num4) ? num4 : this.maxRealFrameIndex);
					}
				}
				i++;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002B81 File Offset: 0x00000D81
		private EasingValue ConvertTweenData(int oldTweenType, List<float> oldCustomTween)
		{
			if (oldTweenType == 22)
			{
				oldTweenType += 2;
			}
			else if (oldTweenType > 23)
			{
				oldTweenType += 3;
			}
			return new EasingValue((TweenType)oldTweenType, oldCustomTween);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002BA0 File Offset: 0x00000DA0
		private void InitAndSetFrameData(int i, int frameCount, AbstractNodeObjectData display, string type, int frameIndex, object copiedValue, bool isTween, EasingValue tween)
		{
			if (i == 0 && this.animationstart != frameIndex)
			{
				this.SetFrameData(this.animationstart, display, type, copiedValue, isTween, tween);
			}
			if (i == frameCount - 1)
			{
				isTween = false;
				tween = null;
			}
			this.SetFrameData(frameIndex, display, type, copiedValue, isTween, tween);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002BEC File Offset: 0x00000DEC
		private void ConvertFrameData(BoneNodeObjectData bone, JArray frameData)
		{
			int i = 0;
			int num = frameData.Count<JToken>();
			JToken jtoken = null;
			while (i < num)
			{
				jtoken = frameData[i];
				bool flag = true;
				JToken jtoken2 = jtoken["tweenFrame"];
				if (jtoken2 != null)
				{
					flag = (bool)jtoken2;
				}
				EasingValue tween = null;
				if (flag)
				{
					int oldTweenType = 0;
					jtoken2 = jtoken["twE"];
					if (jtoken2 != null)
					{
						oldTweenType = (int)jtoken2;
					}
					jtoken2 = jtoken["twEP"];
					List<float> list = new List<float>();
					if (jtoken2 != null)
					{
						JArray jarray = (JArray)jtoken2;
						foreach (JToken value in ((IEnumerable<JToken>)jarray))
						{
							list.Add((float)value);
						}
					}
					tween = this.ConvertTweenData(oldTweenType, list);
				}
				JToken jtoken3 = jtoken["fi"];
				int num2;
				if (jtoken3 != null)
				{
					num2 = (int)jtoken["fi"];
				}
				else
				{
					num2 = i;
				}
				num2 += this.totalFrameCount;
				int num3 = (int)jtoken["dI"];
				for (int j = 0; j < bone.Children.Count<AbstractNodeObjectData>(); j++)
				{
					AbstractNodeObjectData abstractNodeObjectData = bone.Children.ElementAt(j);
					if (!this.IsBone(abstractNodeObjectData))
					{
						this.InitAndSetFrameData(i, num, abstractNodeObjectData, "VisibleForFrame", num2, num3 == j, false, null);
					}
				}
				int num4 = (int)jtoken["z"] + bone.ZOrder;
				this.InitAndSetFrameData(i, num, bone, "ZOrder", num2, num4, false, null);
				JToken jtoken4 = jtoken["color"];
				if (jtoken4 == null)
				{
					jtoken4 = new JObject();
					jtoken4["a"] = 255;
					jtoken4["r"] = 255;
					jtoken4["g"] = 255;
					jtoken4["b"] = 255;
				}
				TimelineData nodeTimeline = this.GetNodeTimeline(bone, "CColor");
				JArray jarray2 = jtoken4 as JArray;
				if (jarray2 != null)
				{
					jtoken4 = jarray2[0];
				}
				byte b = (byte)jtoken4["a"];
				byte red = (byte)jtoken4["r"];
				byte green = (byte)jtoken4["g"];
				byte blue = (byte)jtoken4["b"];
				System.Drawing.Color color = System.Drawing.Color.FromArgb((int)b, (int)red, (int)green, (int)blue);
				AnimationConveter.GetTimelineFrame(nodeTimeline, this.animationstart, AnimationConveter.colorframedataType);
				this.InitAndSetFrameData(i, num, bone, "CColor", num2, color, flag, tween);
				this.InitAndSetFrameData(i, num, bone, "Alpha", num2, (int)b, flag, tween);
				if (jtoken["bd_src"] != null)
				{
					int src = (int)jtoken["bd_src"];
					int dst = (int)jtoken["bd_dst"];
					BlendFuncValue copiedValue = new BlendFuncValue((BlendSrc)src, (BlendDst)dst);
					this.InitAndSetFrameData(i, num, bone, "BlendFunc", num2, copiedValue, flag, tween);
				}
				float x = (float)jtoken["x"] + bone.Position.X;
				float y = (float)jtoken["y"] + bone.Position.Y;
				this.InitAndSetFrameData(i, num, bone, "Position", num2, new CocoStudio.Model.PointF(x, y), flag, tween);
				x = (float)jtoken["cX"] + bone.Scale.ScaleX - 1f;
				y = (float)jtoken["cY"] + bone.Scale.ScaleY - 1f;
				this.InitAndSetFrameData(i, num, bone, "Scale", num2, new CocoStudio.Model.PointF(x, y), flag, tween);
				x = (float)jtoken["kX"] * 180f / 3.1415927f + bone.RotationSkewX;
				y = -(float)jtoken["kY"] * 180f / 3.1415927f + bone.RotationSkewY;
				this.InitAndSetFrameData(i, num, bone, "RotationSkew", num2, new CocoStudio.Model.PointF(x, y), flag, tween);
				JToken jtoken5 = jtoken["evt"];
				if (jtoken5 != null)
				{
					this.SetFrameData(num2, bone, "FrameEvent", (string)jtoken5, false, null);
				}
				i++;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003074 File Offset: 0x00001274
		private bool IsHasFrame(TimelineData timeLine)
		{
			return timeLine.Frames != null && 0 < timeLine.Frames.Count<FrameData>();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003090 File Offset: 0x00001290
		private void ConvertTextureData(JArray textureData)
		{
			if (textureData == null)
			{
				return;
			}
			int i = 0;
			int num = textureData.Count<JToken>();
			while (i < num)
			{
				JToken jtoken = textureData[i];
				string str = (string)jtoken["name"];
				CocoStudio.Model.SizeF item = new CocoStudio.Model.SizeF((float)jtoken["width"], (float)jtoken["height"]);
				ScaleValue item2 = new ScaleValue((float)jtoken["pX"], (float)jtoken["pY"], 0.1, -99999999.0, 99999999.0);
				string text = str + ".png";
				JToken jtoken2 = jtoken["plistFile"];
				string text2 = (jtoken2 == null) ? "" : ((string)jtoken2);
				string text3 = Path.Combine(this.absjsondir, text2);
				ResourceItemData item3;
				if (!string.IsNullOrEmpty(text3) && File.Exists(text3))
				{
					text3 = JsonFileHelp.GetResRelativePath(text3);
					item3 = new ResourceItemData(EnumResourceType.PlistSubImage, text, text3);
				}
				else if (!string.IsNullOrEmpty(JsonFileHelp.plistfilehelper.FindPlistFile(text)))
				{
					text2 = JsonFileHelp.plistfilehelper.FindPlistFile(text);
					item3 = new ResourceItemData(EnumResourceType.PlistSubImage, text, text2);
				}
				else if (!JsonFileHelp.isBasedProject)
				{
					string absPath = Path.Combine(this.absjsondir, text);
					text = JsonFileHelp.GetResRelativePath(absPath);
					item3 = new ResourceItemData(text);
				}
				else
				{
					item3 = new ResourceItemData(text);
				}
				this.textureFileMap[text] = new Tuple<CocoStudio.Model.SizeF, ScaleValue, ResourceItemData>(item, item2, item3);
				i++;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003228 File Offset: 0x00001428
		private void StopAnimation(int animationstopindex)
		{
			foreach (NodeObjectData node in this.textureList)
			{
				this.SetFrameData(animationstopindex, node, "VisibleForFrame", false, false, null);
			}
			foreach (KeyValuePair<string, BoneNodeObjectData> keyValuePair in this.boneMap)
			{
				this.SetFrameData(animationstopindex, keyValuePair.Value, "Position", keyValuePair.Value.Position, false, null);
				this.SetFrameData(animationstopindex, keyValuePair.Value, "Scale", new CocoStudio.Model.PointF(keyValuePair.Value.Scale.ScaleX, keyValuePair.Value.Scale.ScaleY), false, null);
				this.SetFrameData(animationstopindex, keyValuePair.Value, "RotationSkew", new CocoStudio.Model.PointF(keyValuePair.Value.RotationSkewX, keyValuePair.Value.RotationSkewY), false, null);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000335C File Offset: 0x0000155C
		private void MakeSureWhiteOpFrame(TimelineData timeLine, int frameIndex)
		{
			ColorData colorData = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			ColorFrameData colorFrameData = timeLine.Frames.Last<FrameData>() as ColorFrameData;
			bool flag = !colorFrameData.Color.Equals(colorData);
			bool flag2 = colorFrameData.Alpha != 255;
			if (flag || flag2)
			{
				colorFrameData = (AnimationConveter.GetTimelineFrame(timeLine, frameIndex, AnimationConveter.colorframedataType) as ColorFrameData);
				if (flag)
				{
					colorFrameData.Color = colorData;
				}
				if (flag2)
				{
					colorFrameData.Alpha = 255;
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000033E5 File Offset: 0x000015E5
		private TimelineData GetNodeTimeline(AbstractNodeObjectData node, string property)
		{
			return AnimationConveter.GetNodeTimeline(this.timelineData, property, node.ActionTag);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000033F9 File Offset: 0x000015F9
		private void SetFrameData(int frameIndex, AbstractNodeObjectData node, string property, object frameDataValue, bool isTween = false, EasingValue tween = null)
		{
			AnimationConveter.SetFrameData(this.timelineData, frameIndex, node.ActionTag, property, frameDataValue, isTween, tween);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003414 File Offset: 0x00001614
		public static void SetFrameData(TimelineActionData actdata, int frameIndex, int actionTag, string property, object frameDataValue, bool isTween = true, EasingValue tween = null)
		{
			if (frameDataValue == null)
			{
				return;
			}
			try
			{
				TimelineData nodeTimeline = AnimationConveter.GetNodeTimeline(actdata, property, actionTag);
				FrameData frameData;
				if (property == "VisibleForFrame")
				{
					bool flag = (bool)frameDataValue;
					BoolFrameData boolFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.boolframedataType) as BoolFrameData;
					if (boolFrameData.Value != flag)
					{
						boolFrameData.Value = flag;
					}
					frameData = boolFrameData;
				}
				else if (property == "CColor")
				{
					System.Drawing.Color color = (System.Drawing.Color)frameDataValue;
					ColorFrameData colorFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.colorframedataType) as ColorFrameData;
					colorFrameData.Color = color;
					colorFrameData.Alpha = (int)color.A;
					frameData = colorFrameData;
				}
				else if (property == "Alpha")
				{
					int value = (int)frameDataValue;
					IntFrameData intFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.intframedataType) as IntFrameData;
					intFrameData.Value = value;
					frameData = intFrameData;
				}
				else if (property == "ZOrder")
				{
					int value2 = (int)frameDataValue;
					IntFrameData intFrameData2 = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.intframedataType) as IntFrameData;
					intFrameData2.Value = value2;
					frameData = intFrameData2;
				}
				else if (property == "Position" || property == "Scale" || property == "RotationSkew")
				{
					CocoStudio.Model.PointF pointF = (CocoStudio.Model.PointF)frameDataValue;
					PointFrameData pointFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.pointframedataType) as PointFrameData;
					pointFrameData.X = pointF.X;
					pointFrameData.Y = pointF.Y;
					frameData = pointFrameData;
				}
				else if (property == "FrameEvent")
				{
					string value3 = (string)frameDataValue;
					StringFrameData stringFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.stringframedataType) as StringFrameData;
					stringFrameData.Value = value3;
					frameData = stringFrameData;
				}
				else
				{
					if (!(property == "BlendFunc"))
					{
						throw new InvalidOperationException("none of this frame type!");
					}
					BlendFuncValue blendFuncValue = (BlendFuncValue)frameDataValue;
					BlendFuncFrameData blendFuncFrameData = AnimationConveter.GetTimelineFrame(nodeTimeline, frameIndex, AnimationConveter.blendfuncframeDataType) as BlendFuncFrameData;
					blendFuncFrameData.Src = (int)blendFuncValue.BlendSrc;
					blendFuncFrameData.Dst = (int)blendFuncValue.BlendDst;
					frameData = blendFuncFrameData;
				}
				frameData.Tween = isTween;
				frameData.EasingData = tween;
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("not the right frameDataValue type for frameType ", innerException);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003660 File Offset: 0x00001860
		public static FrameData GetLastFrame(TimelineData timeline)
		{
			FrameData result = null;
			if (timeline.Frames != null && timeline.Frames.Count > 0)
			{
				result = timeline.Frames.Last<FrameData>();
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003694 File Offset: 0x00001894
		public static TimelineData GetNodeTimeline(TimelineActionData actData, string property, int actionTag)
		{
			TimelineData timelineData = null;
			foreach (TimelineData timelineData2 in actData.Timelines)
			{
				if (timelineData2.Property == property && timelineData2.ActionTag == actionTag)
				{
					timelineData = timelineData2;
					break;
				}
			}
			if (timelineData == null)
			{
				timelineData = new TimelineData();
				timelineData.ActionTag = actionTag;
				timelineData.Property = property;
				actData.Timelines.Add(timelineData);
			}
			return timelineData;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003724 File Offset: 0x00001924
		private static bool IsTimeLineHasFrameAt(TimelineData timeline, int frameIndex, Type frameValueType)
		{
			bool result = false;
			if (timeline.Frames == null)
			{
				return result;
			}
			foreach (FrameData frameData in timeline.Frames)
			{
				if (frameData.FrameIndex == frameIndex)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000378C File Offset: 0x0000198C
		public static void ClearRepeatFrames(TimelineActionData actionData)
		{
			foreach (TimelineData timelineData in actionData.Timelines)
			{
				AnimationConveter.ClearRepeatFrames(timelineData);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000037E0 File Offset: 0x000019E0
		public static void ClearRepeatFrames(TimelineData timelineData)
		{
			if (timelineData.Frames != null)
			{
				FrameData frameData = new FrameData();
				int count = timelineData.Frames.Count;
				for (int i = count - 2; i > 0; i--)
				{
					FrameData frameData2 = timelineData.Frames[i];
					frameData = timelineData.Frames[i - 1];
					FrameData frameData3 = timelineData.Frames[i + 1];
					bool flag;
					if (timelineData.Property == "FrameEvent")
					{
						StringFrameData stringFrameData = (StringFrameData)frameData2;
						flag = string.IsNullOrEmpty(stringFrameData.Value);
					}
					else if (timelineData.Property == "ZOrder" || timelineData.Property == "VisibleForFrame" || (!frameData2.Tween && !frameData.Tween && !frameData3.Tween))
					{
						flag = FrameDataEqualHelper.FrameDataEquals(frameData2, frameData);
					}
					else
					{
						flag = (FrameDataEqualHelper.FrameDataEquals(frameData2, frameData) && FrameDataEqualHelper.FrameDataEquals(frameData2, frameData3));
					}
					if (flag)
					{
						timelineData.Frames.RemoveAt(i);
					}
				}
				count = timelineData.Frames.Count;
				if (count > 1)
				{
					FrameData frameData4 = timelineData.Frames[count - 1];
					FrameData frameData5 = timelineData.Frames[count - 2];
					if ((timelineData.Property == "ZOrder" || timelineData.Property == "VisibleForFrame" || (timelineData.Property != "FrameEvent" && !frameData4.Tween && !frameData5.Tween)) && FrameDataEqualHelper.FrameDataEquals(frameData4, frameData5))
					{
						timelineData.Frames.RemoveAt(count - 1);
					}
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003984 File Offset: 0x00001B84
		public static FrameData GetTimelineFrame(TimelineData timeline, int frameIndex, Type frameValueType)
		{
			if (timeline.Frames == null)
			{
				timeline.Frames = new List<FrameData>();
			}
			foreach (FrameData frameData in timeline.Frames)
			{
				if (frameData.FrameIndex == frameIndex)
				{
					return frameData;
				}
			}
			FrameData frameData2 = Activator.CreateInstance(frameValueType) as FrameData;
			ColorFrameData colorFrameData = frameData2 as ColorFrameData;
			if (colorFrameData != null)
			{
				colorFrameData.Color = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				colorFrameData.Alpha = 255;
			}
			frameData2.FrameIndex = frameIndex;
			timeline.Frames.Add(frameData2);
			return frameData2;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003A48 File Offset: 0x00001C48
		private void SignAsBone(NodeObjectData node)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003A4A File Offset: 0x00001C4A
		private bool IsBone(AbstractNodeObjectData node)
		{
			return node is BoneNodeObjectData;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003A55 File Offset: 0x00001C55
		private void InitNodeProp(AbstractNodeObjectData node)
		{
			node.ActionTag = node.GetHashCode();
			node.Children = new List<AbstractNodeObjectData>();
		}

		// Token: 0x04000001 RID: 1
		private static Type boolframedataType = typeof(BoolFrameData);

		// Token: 0x04000002 RID: 2
		private static Type intframedataType = typeof(IntFrameData);

		// Token: 0x04000003 RID: 3
		private static Type pointframedataType = typeof(PointFrameData);

		// Token: 0x04000004 RID: 4
		private static Type colorframedataType = typeof(ColorFrameData);

		// Token: 0x04000005 RID: 5
		private static Type stringframedataType = typeof(StringFrameData);

		// Token: 0x04000006 RID: 6
		private static Type blendfuncframeDataType = typeof(BlendFuncFrameData);

		// Token: 0x04000007 RID: 7
		private string absjsondir = "";

		// Token: 0x04000008 RID: 8
		private GameFileData gameFileData;

		// Token: 0x04000009 RID: 9
		private SkeletonNodeObjectData rootData;

		// Token: 0x0400000A RID: 10
		private TimelineActionData timelineData;

		// Token: 0x0400000B RID: 11
		private Dictionary<string, BoneNodeObjectData> boneMap = new Dictionary<string, BoneNodeObjectData>();

		// Token: 0x0400000C RID: 12
		private List<NodeObjectData> textureList = new List<NodeObjectData>();

		// Token: 0x0400000D RID: 13
		private Dictionary<string, Tuple<CocoStudio.Model.SizeF, ScaleValue, ResourceItemData>> textureFileMap = new Dictionary<string, Tuple<CocoStudio.Model.SizeF, ScaleValue, ResourceItemData>>();

		// Token: 0x0400000E RID: 14
		private int totalFrameCount;

		// Token: 0x0400000F RID: 15
		private int animationstart;

		// Token: 0x04000010 RID: 16
		private int minRealFrameIndex;

		// Token: 0x04000011 RID: 17
		private int maxRealFrameIndex;
	}
}
