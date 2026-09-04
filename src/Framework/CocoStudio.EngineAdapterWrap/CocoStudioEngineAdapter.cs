using System;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000020 RID: 32
	public class CocoStudioEngineAdapter
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000711C File Offset: 0x0000531C
		public static float clampf(float value, float min_inclusive, float max_inclusive)
		{
			return CocoStudioEngineAdapterPINVOKE.clampf(value, min_inclusive, max_inclusive);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007138 File Offset: 0x00005338
		public static string STD_STRING_EMPTY
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.STD_STRING_EMPTY_get();
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007154 File Offset: 0x00005354
		public static string CocosGUIVersion()
		{
			return CocoStudioEngineAdapterPINVOKE.CocosGUIVersion();
		}

		// Token: 0x04000024 RID: 36
		public static readonly int CC_ENABLE_CACHE_TEXTURE_DATA = CocoStudioEngineAdapterPINVOKE.CC_ENABLE_CACHE_TEXTURE_DATA_get();

		// Token: 0x04000025 RID: 37
		public static readonly int CC_REBIND_INDICES_BUFFER = CocoStudioEngineAdapterPINVOKE.CC_REBIND_INDICES_BUFFER_get();

		// Token: 0x04000026 RID: 38
		public static readonly string CC_FORMAT_PRINTF_SIZE_T = CocoStudioEngineAdapterPINVOKE.CC_FORMAT_PRINTF_SIZE_T_get();

		// Token: 0x04000027 RID: 39
		public static readonly int NULL = CocoStudioEngineAdapterPINVOKE.NULL_get();

		// Token: 0x04000028 RID: 40
		public static readonly double MATH_FLOAT_SMALL = CocoStudioEngineAdapterPINVOKE.MATH_FLOAT_SMALL_get();

		// Token: 0x04000029 RID: 41
		public static readonly double MATH_TOLERANCE = CocoStudioEngineAdapterPINVOKE.MATH_TOLERANCE_get();

		// Token: 0x0400002A RID: 42
		public static readonly double MATH_PIOVER2 = CocoStudioEngineAdapterPINVOKE.MATH_PIOVER2_get();

		// Token: 0x0400002B RID: 43
		public static readonly double MATH_EPSILON = CocoStudioEngineAdapterPINVOKE.MATH_EPSILON_get();
	}
}
