using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F8 RID: 248
	public enum ViewOrder
	{
		// Token: 0x04000340 RID: 832
		Visible,
		// Token: 0x04000341 RID: 833
		FrameVisiable,
		// Token: 0x04000342 RID: 834
		TouchEnable,
		// Token: 0x04000343 RID: 835
		Tag,
		// Token: 0x04000344 RID: 836
		CascadeColor,
		// Token: 0x04000345 RID: 837
		CascadeAlpha,
		// Token: 0x04000346 RID: 838
		PositionType,
		// Token: 0x04000347 RID: 839
		AnchorPoint,
		// Token: 0x04000348 RID: 840
		Position,
		// Token: 0x04000349 RID: 841
		Scale,
		// Token: 0x0400034A RID: 842
		Rotation,
		// Token: 0x0400034B RID: 843
		RotationSkew,
		// Token: 0x0400034C RID: 844
		Alpha,
		// Token: 0x0400034D RID: 845
		Color,
		// Token: 0x0400034E RID: 846
		Zorder,
		// Token: 0x0400034F RID: 847
		Flip,
		// Token: 0x04000350 RID: 848
		Size,
		// Token: 0x04000351 RID: 849
		BerthAndStretch,
		// Token: 0x04000352 RID: 850
		Fov,
		// Token: 0x04000353 RID: 851
		ViewSize,
		// Token: 0x04000354 RID: 852
		ClipPlane,
		// Token: 0x04000355 RID: 853
		CameraFlag,
		// Token: 0x04000356 RID: 854
		LightMask,
		// Token: 0x04000357 RID: 855
		SliceSize,
		// Token: 0x04000358 RID: 856
		AnimationSwitch,
		// Token: 0x04000359 RID: 857
		AnimationSpeed,
		// Token: 0x0400035A RID: 858
		TextureSwitch,
		// Token: 0x0400035B RID: 859
		TextureAnimation,
		// Token: 0x0400035C RID: 860
		framerate,
		// Token: 0x0400035D RID: 861
		BillBoardMode,
		// Token: 0x0400035E RID: 862
		LightType,
		// Token: 0x0400035F RID: 863
		LightFlag,
		// Token: 0x04000360 RID: 864
		LightEnable,
		// Token: 0x04000361 RID: 865
		Intensity,
		// Token: 0x04000362 RID: 866
		Range,
		// Token: 0x04000363 RID: 867
		SportAngle,
		// Token: 0x04000364 RID: 868
		SizeType,
		// Token: 0x04000365 RID: 869
		Scale9,
		// Token: 0x04000366 RID: 870
		Panel_ClipEnable,
		// Token: 0x04000367 RID: 871
		ScrollView_BounceType,
		// Token: 0x04000368 RID: 872
		ScrollView_InnerSize,
		// Token: 0x04000369 RID: 873
		ScrollView_DirectionType,
		// Token: 0x0400036A RID: 874
		ListView_ScrollOrientation,
		// Token: 0x0400036B RID: 875
		ListView_HAlignType,
		// Token: 0x0400036C RID: 876
		ListView_VAlignType,
		// Token: 0x0400036D RID: 877
		ListView_ChildSpacing,
		// Token: 0x0400036E RID: 878
		Panel_BackColorType,
		// Token: 0x0400036F RID: 879
		Panel_BackColor,
		// Token: 0x04000370 RID: 880
		Panel_StartColor,
		// Token: 0x04000371 RID: 881
		Panel_StartAlpha,
		// Token: 0x04000372 RID: 882
		Panel_EndColor,
		// Token: 0x04000373 RID: 883
		Panel_EndAlpha,
		// Token: 0x04000374 RID: 884
		Panel_BackAlpha,
		// Token: 0x04000375 RID: 885
		Panel_Vector,
		// Token: 0x04000376 RID: 886
		Panel_FileData,
		// Token: 0x04000377 RID: 887
		Text_Animation,
		// Token: 0x04000378 RID: 888
		TextField_StayText,
		// Token: 0x04000379 RID: 889
		Text_String,
		// Token: 0x0400037A RID: 890
		Text_AlignHorizontal,
		// Token: 0x0400037B RID: 891
		Text_AlignVertical,
		// Token: 0x0400037C RID: 892
		Text_FontSize,
		// Token: 0x0400037D RID: 893
		Text_FontFile,
		// Token: 0x0400037E RID: 894
		TextField_PassWord,
		// Token: 0x0400037F RID: 895
		TextField_TextLength,
		// Token: 0x04000380 RID: 896
		Slider_BackImage,
		// Token: 0x04000381 RID: 897
		Slider_BackInnerImage,
		// Token: 0x04000382 RID: 898
		Slider_BackFile,
		// Token: 0x04000383 RID: 899
		Slider_NodeNormal,
		// Token: 0x04000384 RID: 900
		Slider_NodePress,
		// Token: 0x04000385 RID: 901
		Slider_NodeDisable,
		// Token: 0x04000386 RID: 902
		Slider_NodeFile,
		// Token: 0x04000387 RID: 903
		Slider_State,
		// Token: 0x04000388 RID: 904
		Slider_Progress,
		// Token: 0x04000389 RID: 905
		LoadingBar_FileData,
		// Token: 0x0400038A RID: 906
		LoadingBar_Progress,
		// Token: 0x0400038B RID: 907
		LoadingBar_Direction,
		// Token: 0x0400038C RID: 908
		TextAtlas_FileData,
		// Token: 0x0400038D RID: 909
		TextAtlas_Label,
		// Token: 0x0400038E RID: 910
		TextAtlas_StartChar,
		// Token: 0x0400038F RID: 911
		TextAtlas_CharWidth,
		// Token: 0x04000390 RID: 912
		TextAtlas_CharHeight,
		// Token: 0x04000391 RID: 913
		TextAtlas_Text,
		// Token: 0x04000392 RID: 914
		ImageView_FileData,
		// Token: 0x04000393 RID: 915
		CheckBox_BackNormal,
		// Token: 0x04000394 RID: 916
		CheckBox_BackPress,
		// Token: 0x04000395 RID: 917
		CheckBox_BackDisable,
		// Token: 0x04000396 RID: 918
		CheckBox_NodeNormal,
		// Token: 0x04000397 RID: 919
		CheckBox_NodeDisable,
		// Token: 0x04000398 RID: 920
		CheckBox_BackFileData,
		// Token: 0x04000399 RID: 921
		CheckBox_NodeFileData,
		// Token: 0x0400039A RID: 922
		CheckBox_State,
		// Token: 0x0400039B RID: 923
		CheckBox_SelectState,
		// Token: 0x0400039C RID: 924
		Button_FileData,
		// Token: 0x0400039D RID: 925
		Button_Normal,
		// Token: 0x0400039E RID: 926
		Button_Press,
		// Token: 0x0400039F RID: 927
		Button_Disable,
		// Token: 0x040003A0 RID: 928
		Button_State,
		// Token: 0x040003A1 RID: 929
		Button_Text,
		// Token: 0x040003A2 RID: 930
		Button_TextColor,
		// Token: 0x040003A3 RID: 931
		Button_FontSize,
		// Token: 0x040003A4 RID: 932
		Button_Font,
		// Token: 0x040003A5 RID: 933
		Shadow_Enable,
		// Token: 0x040003A6 RID: 934
		Shadow_Color,
		// Token: 0x040003A7 RID: 935
		Shadow_Offset,
		// Token: 0x040003A8 RID: 936
		Outline_Enable,
		// Token: 0x040003A9 RID: 937
		Outline_Color,
		// Token: 0x040003AA RID: 938
		Outline_Size,
		// Token: 0x040003AB RID: 939
		Audio_File,
		// Token: 0x040003AC RID: 940
		Audio_Loop,
		// Token: 0x040003AD RID: 941
		Particle_File,
		// Token: 0x040003AE RID: 942
		BlendFunc,
		// Token: 0x040003AF RID: 943
		RunAction,
		// Token: 0x040003B0 RID: 944
		Map_File,
		// Token: 0x040003B1 RID: 945
		SkyBox_Enable,
		// Token: 0x040003B2 RID: 946
		SkyBox_FaceSize,
		// Token: 0x040003B3 RID: 947
		SkyBox_Left,
		// Token: 0x040003B4 RID: 948
		SkyBox_Right,
		// Token: 0x040003B5 RID: 949
		SkyBox_Up,
		// Token: 0x040003B6 RID: 950
		SkyBox_Down,
		// Token: 0x040003B7 RID: 951
		SkyBox_Front,
		// Token: 0x040003B8 RID: 952
		SkyBox_Back,
		// Token: 0x040003B9 RID: 953
		FrameData = 1000,
		// Token: 0x040003BA RID: 954
		CallBackType,
		// Token: 0x040003BB RID: 955
		FrameEvent,
		// Token: 0x040003BC RID: 956
		UserData
	}
}
