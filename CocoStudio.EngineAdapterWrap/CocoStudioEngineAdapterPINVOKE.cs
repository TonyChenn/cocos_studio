using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000021 RID: 33
	internal class CocoStudioEngineAdapterPINVOKE
	{
		// Token: 0x060001A5 RID: 421
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Clear")]
		public static extern void CSVectorInt_Clear(HandleRef jarg1);

		// Token: 0x060001A6 RID: 422
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Add")]
		public static extern void CSVectorInt_Add(HandleRef jarg1, int jarg2);

		// Token: 0x060001A7 RID: 423
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_size")]
		public static extern uint CSVectorInt_size(HandleRef jarg1);

		// Token: 0x060001A8 RID: 424
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_capacity")]
		public static extern uint CSVectorInt_capacity(HandleRef jarg1);

		// Token: 0x060001A9 RID: 425
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_reserve")]
		public static extern void CSVectorInt_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060001AA RID: 426
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorInt__SWIG_0")]
		public static extern IntPtr new_CSVectorInt__SWIG_0();

		// Token: 0x060001AB RID: 427
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorInt__SWIG_1")]
		public static extern IntPtr new_CSVectorInt__SWIG_1(HandleRef jarg1);

		// Token: 0x060001AC RID: 428
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorInt__SWIG_2")]
		public static extern IntPtr new_CSVectorInt__SWIG_2(int jarg1);

		// Token: 0x060001AD RID: 429
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_getitemcopy")]
		public static extern int CSVectorInt_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060001AE RID: 430
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_getitem")]
		public static extern int CSVectorInt_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060001AF RID: 431
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_setitem")]
		public static extern void CSVectorInt_setitem(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001B0 RID: 432
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_AddRange")]
		public static extern void CSVectorInt_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060001B1 RID: 433
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_GetRange")]
		public static extern IntPtr CSVectorInt_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001B2 RID: 434
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Insert")]
		public static extern void CSVectorInt_Insert(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001B3 RID: 435
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_InsertRange")]
		public static extern void CSVectorInt_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001B4 RID: 436
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_RemoveAt")]
		public static extern void CSVectorInt_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x060001B5 RID: 437
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_RemoveRange")]
		public static extern void CSVectorInt_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001B6 RID: 438
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Repeat")]
		public static extern IntPtr CSVectorInt_Repeat(int jarg1, int jarg2);

		// Token: 0x060001B7 RID: 439
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Reverse__SWIG_0")]
		public static extern void CSVectorInt_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x060001B8 RID: 440
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Reverse__SWIG_1")]
		public static extern void CSVectorInt_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001B9 RID: 441
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_SetRange")]
		public static extern void CSVectorInt_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001BA RID: 442
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Contains")]
		public static extern bool CSVectorInt_Contains(HandleRef jarg1, int jarg2);

		// Token: 0x060001BB RID: 443
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_IndexOf")]
		public static extern int CSVectorInt_IndexOf(HandleRef jarg1, int jarg2);

		// Token: 0x060001BC RID: 444
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_LastIndexOf")]
		public static extern int CSVectorInt_LastIndexOf(HandleRef jarg1, int jarg2);

		// Token: 0x060001BD RID: 445
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorInt_Remove")]
		public static extern bool CSVectorInt_Remove(HandleRef jarg1, int jarg2);

		// Token: 0x060001BE RID: 446
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorInt")]
		public static extern void delete_CSVectorInt(HandleRef jarg1);

		// Token: 0x060001BF RID: 447
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Clear")]
		public static extern void CSVectorFloat_Clear(HandleRef jarg1);

		// Token: 0x060001C0 RID: 448
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Add")]
		public static extern void CSVectorFloat_Add(HandleRef jarg1, float jarg2);

		// Token: 0x060001C1 RID: 449
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_size")]
		public static extern uint CSVectorFloat_size(HandleRef jarg1);

		// Token: 0x060001C2 RID: 450
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_capacity")]
		public static extern uint CSVectorFloat_capacity(HandleRef jarg1);

		// Token: 0x060001C3 RID: 451
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_reserve")]
		public static extern void CSVectorFloat_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060001C4 RID: 452
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorFloat__SWIG_0")]
		public static extern IntPtr new_CSVectorFloat__SWIG_0();

		// Token: 0x060001C5 RID: 453
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorFloat__SWIG_1")]
		public static extern IntPtr new_CSVectorFloat__SWIG_1(HandleRef jarg1);

		// Token: 0x060001C6 RID: 454
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorFloat__SWIG_2")]
		public static extern IntPtr new_CSVectorFloat__SWIG_2(int jarg1);

		// Token: 0x060001C7 RID: 455
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_getitemcopy")]
		public static extern float CSVectorFloat_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060001C8 RID: 456
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_getitem")]
		public static extern float CSVectorFloat_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060001C9 RID: 457
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_setitem")]
		public static extern void CSVectorFloat_setitem(HandleRef jarg1, int jarg2, float jarg3);

		// Token: 0x060001CA RID: 458
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_AddRange")]
		public static extern void CSVectorFloat_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060001CB RID: 459
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_GetRange")]
		public static extern IntPtr CSVectorFloat_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001CC RID: 460
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Insert")]
		public static extern void CSVectorFloat_Insert(HandleRef jarg1, int jarg2, float jarg3);

		// Token: 0x060001CD RID: 461
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_InsertRange")]
		public static extern void CSVectorFloat_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001CE RID: 462
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_RemoveAt")]
		public static extern void CSVectorFloat_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x060001CF RID: 463
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_RemoveRange")]
		public static extern void CSVectorFloat_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001D0 RID: 464
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Repeat")]
		public static extern IntPtr CSVectorFloat_Repeat(float jarg1, int jarg2);

		// Token: 0x060001D1 RID: 465
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Reverse__SWIG_0")]
		public static extern void CSVectorFloat_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x060001D2 RID: 466
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Reverse__SWIG_1")]
		public static extern void CSVectorFloat_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001D3 RID: 467
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_SetRange")]
		public static extern void CSVectorFloat_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001D4 RID: 468
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Contains")]
		public static extern bool CSVectorFloat_Contains(HandleRef jarg1, float jarg2);

		// Token: 0x060001D5 RID: 469
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_IndexOf")]
		public static extern int CSVectorFloat_IndexOf(HandleRef jarg1, float jarg2);

		// Token: 0x060001D6 RID: 470
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_LastIndexOf")]
		public static extern int CSVectorFloat_LastIndexOf(HandleRef jarg1, float jarg2);

		// Token: 0x060001D7 RID: 471
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorFloat_Remove")]
		public static extern bool CSVectorFloat_Remove(HandleRef jarg1, float jarg2);

		// Token: 0x060001D8 RID: 472
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorFloat")]
		public static extern void delete_CSVectorFloat(HandleRef jarg1);

		// Token: 0x060001D9 RID: 473
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Clear")]
		public static extern void CSVectorDouble_Clear(HandleRef jarg1);

		// Token: 0x060001DA RID: 474
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Add")]
		public static extern void CSVectorDouble_Add(HandleRef jarg1, double jarg2);

		// Token: 0x060001DB RID: 475
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_size")]
		public static extern uint CSVectorDouble_size(HandleRef jarg1);

		// Token: 0x060001DC RID: 476
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_capacity")]
		public static extern uint CSVectorDouble_capacity(HandleRef jarg1);

		// Token: 0x060001DD RID: 477
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_reserve")]
		public static extern void CSVectorDouble_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060001DE RID: 478
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorDouble__SWIG_0")]
		public static extern IntPtr new_CSVectorDouble__SWIG_0();

		// Token: 0x060001DF RID: 479
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorDouble__SWIG_1")]
		public static extern IntPtr new_CSVectorDouble__SWIG_1(HandleRef jarg1);

		// Token: 0x060001E0 RID: 480
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorDouble__SWIG_2")]
		public static extern IntPtr new_CSVectorDouble__SWIG_2(int jarg1);

		// Token: 0x060001E1 RID: 481
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_getitemcopy")]
		public static extern double CSVectorDouble_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060001E2 RID: 482
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_getitem")]
		public static extern double CSVectorDouble_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060001E3 RID: 483
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_setitem")]
		public static extern void CSVectorDouble_setitem(HandleRef jarg1, int jarg2, double jarg3);

		// Token: 0x060001E4 RID: 484
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_AddRange")]
		public static extern void CSVectorDouble_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060001E5 RID: 485
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_GetRange")]
		public static extern IntPtr CSVectorDouble_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001E6 RID: 486
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Insert")]
		public static extern void CSVectorDouble_Insert(HandleRef jarg1, int jarg2, double jarg3);

		// Token: 0x060001E7 RID: 487
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_InsertRange")]
		public static extern void CSVectorDouble_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001E8 RID: 488
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_RemoveAt")]
		public static extern void CSVectorDouble_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x060001E9 RID: 489
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_RemoveRange")]
		public static extern void CSVectorDouble_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001EA RID: 490
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Repeat")]
		public static extern IntPtr CSVectorDouble_Repeat(double jarg1, int jarg2);

		// Token: 0x060001EB RID: 491
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Reverse__SWIG_0")]
		public static extern void CSVectorDouble_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x060001EC RID: 492
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Reverse__SWIG_1")]
		public static extern void CSVectorDouble_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060001ED RID: 493
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_SetRange")]
		public static extern void CSVectorDouble_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060001EE RID: 494
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Contains")]
		public static extern bool CSVectorDouble_Contains(HandleRef jarg1, double jarg2);

		// Token: 0x060001EF RID: 495
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_IndexOf")]
		public static extern int CSVectorDouble_IndexOf(HandleRef jarg1, double jarg2);

		// Token: 0x060001F0 RID: 496
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_LastIndexOf")]
		public static extern int CSVectorDouble_LastIndexOf(HandleRef jarg1, double jarg2);

		// Token: 0x060001F1 RID: 497
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorDouble_Remove")]
		public static extern bool CSVectorDouble_Remove(HandleRef jarg1, double jarg2);

		// Token: 0x060001F2 RID: 498
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorDouble")]
		public static extern void delete_CSVectorDouble(HandleRef jarg1);

		// Token: 0x060001F3 RID: 499
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Clear")]
		public static extern void CSVectorString_Clear(HandleRef jarg1);

		// Token: 0x060001F4 RID: 500
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Add")]
		public static extern void CSVectorString_Add(HandleRef jarg1, string jarg2);

		// Token: 0x060001F5 RID: 501
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_size")]
		public static extern uint CSVectorString_size(HandleRef jarg1);

		// Token: 0x060001F6 RID: 502
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_capacity")]
		public static extern uint CSVectorString_capacity(HandleRef jarg1);

		// Token: 0x060001F7 RID: 503
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_reserve")]
		public static extern void CSVectorString_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060001F8 RID: 504
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorString__SWIG_0")]
		public static extern IntPtr new_CSVectorString__SWIG_0();

		// Token: 0x060001F9 RID: 505
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorString__SWIG_1")]
		public static extern IntPtr new_CSVectorString__SWIG_1(HandleRef jarg1);

		// Token: 0x060001FA RID: 506
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorString__SWIG_2")]
		public static extern IntPtr new_CSVectorString__SWIG_2(int jarg1);

		// Token: 0x060001FB RID: 507
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_getitemcopy")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSVectorString_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060001FC RID: 508
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_getitem")]
		public static extern string CSVectorString_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060001FD RID: 509
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_setitem")]
		public static extern void CSVectorString_setitem(HandleRef jarg1, int jarg2, string jarg3);

		// Token: 0x060001FE RID: 510
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_AddRange")]
		public static extern void CSVectorString_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060001FF RID: 511
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_GetRange")]
		public static extern IntPtr CSVectorString_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000200 RID: 512
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Insert")]
		public static extern void CSVectorString_Insert(HandleRef jarg1, int jarg2, string jarg3);

		// Token: 0x06000201 RID: 513
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_InsertRange")]
		public static extern void CSVectorString_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000202 RID: 514
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_RemoveAt")]
		public static extern void CSVectorString_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x06000203 RID: 515
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_RemoveRange")]
		public static extern void CSVectorString_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000204 RID: 516
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Repeat")]
		public static extern IntPtr CSVectorString_Repeat(string jarg1, int jarg2);

		// Token: 0x06000205 RID: 517
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Reverse__SWIG_0")]
		public static extern void CSVectorString_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x06000206 RID: 518
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Reverse__SWIG_1")]
		public static extern void CSVectorString_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000207 RID: 519
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_SetRange")]
		public static extern void CSVectorString_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000208 RID: 520
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Contains")]
		public static extern bool CSVectorString_Contains(HandleRef jarg1, string jarg2);

		// Token: 0x06000209 RID: 521
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_IndexOf")]
		public static extern int CSVectorString_IndexOf(HandleRef jarg1, string jarg2);

		// Token: 0x0600020A RID: 522
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_LastIndexOf")]
		public static extern int CSVectorString_LastIndexOf(HandleRef jarg1, string jarg2);

		// Token: 0x0600020B RID: 523
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorString_Remove")]
		public static extern bool CSVectorString_Remove(HandleRef jarg1, string jarg2);

		// Token: 0x0600020C RID: 524
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorString")]
		public static extern void delete_CSVectorString(HandleRef jarg1);

		// Token: 0x0600020D RID: 525
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CC_ENABLE_CACHE_TEXTURE_DATA_get")]
		public static extern int CC_ENABLE_CACHE_TEXTURE_DATA_get();

		// Token: 0x0600020E RID: 526
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CC_REBIND_INDICES_BUFFER_get")]
		public static extern int CC_REBIND_INDICES_BUFFER_get();

		// Token: 0x0600020F RID: 527
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CC_FORMAT_PRINTF_SIZE_T_get")]
		public static extern string CC_FORMAT_PRINTF_SIZE_T_get();

		// Token: 0x06000210 RID: 528
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_NULL_get")]
		public static extern int NULL_get();

		// Token: 0x06000211 RID: 529
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MATH_FLOAT_SMALL_get")]
		public static extern double MATH_FLOAT_SMALL_get();

		// Token: 0x06000212 RID: 530
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MATH_TOLERANCE_get")]
		public static extern double MATH_TOLERANCE_get();

		// Token: 0x06000213 RID: 531
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MATH_PIOVER2_get")]
		public static extern double MATH_PIOVER2_get();

		// Token: 0x06000214 RID: 532
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MATH_EPSILON_get")]
		public static extern double MATH_EPSILON_get();

		// Token: 0x06000215 RID: 533
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_clampf")]
		public static extern float clampf(float jarg1, float jarg2, float jarg3);

		// Token: 0x06000216 RID: 534
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_x_set")]
		public static extern void Vec2_x_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000217 RID: 535
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_x_get")]
		public static extern float Vec2_x_get(HandleRef jarg1);

		// Token: 0x06000218 RID: 536
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_y_set")]
		public static extern void Vec2_y_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000219 RID: 537
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_y_get")]
		public static extern float Vec2_y_get(HandleRef jarg1);

		// Token: 0x0600021A RID: 538
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec2__SWIG_0")]
		public static extern IntPtr new_Vec2__SWIG_0();

		// Token: 0x0600021B RID: 539
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec2__SWIG_1")]
		public static extern IntPtr new_Vec2__SWIG_1(float jarg1, float jarg2);

		// Token: 0x0600021C RID: 540
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec2__SWIG_2")]
		public static extern IntPtr new_Vec2__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600021D RID: 541
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec2__SWIG_3")]
		public static extern IntPtr new_Vec2__SWIG_3(HandleRef jarg1);

		// Token: 0x0600021E RID: 542
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Vec2")]
		public static extern void delete_Vec2(HandleRef jarg1);

		// Token: 0x0600021F RID: 543
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isZero")]
		public static extern bool Vec2_isZero(HandleRef jarg1);

		// Token: 0x06000220 RID: 544
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isOne")]
		public static extern bool Vec2_isOne(HandleRef jarg1);

		// Token: 0x06000221 RID: 545
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_angle")]
		public static extern float Vec2_angle(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000222 RID: 546
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_add__SWIG_0")]
		public static extern void Vec2_add__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000223 RID: 547
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_add__SWIG_1")]
		public static extern void Vec2_add__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000224 RID: 548
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_clamp__SWIG_0")]
		public static extern void Vec2_clamp__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000225 RID: 549
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_clamp__SWIG_1")]
		public static extern void Vec2_clamp__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000226 RID: 550
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_distance")]
		public static extern float Vec2_distance(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000227 RID: 551
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_distanceSquared")]
		public static extern float Vec2_distanceSquared(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000228 RID: 552
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_dot__SWIG_0")]
		public static extern float Vec2_dot__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000229 RID: 553
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_length")]
		public static extern float Vec2_length(HandleRef jarg1);

		// Token: 0x0600022A RID: 554
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_lengthSquared")]
		public static extern float Vec2_lengthSquared(HandleRef jarg1);

		// Token: 0x0600022B RID: 555
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_negate")]
		public static extern void Vec2_negate(HandleRef jarg1);

		// Token: 0x0600022C RID: 556
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_normalize")]
		public static extern void Vec2_normalize(HandleRef jarg1);

		// Token: 0x0600022D RID: 557
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getNormalized")]
		public static extern IntPtr Vec2_getNormalized(HandleRef jarg1);

		// Token: 0x0600022E RID: 558
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_scale__SWIG_0")]
		public static extern void Vec2_scale__SWIG_0(HandleRef jarg1, float jarg2);

		// Token: 0x0600022F RID: 559
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_scale__SWIG_1")]
		public static extern void Vec2_scale__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000230 RID: 560
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_rotate__SWIG_0")]
		public static extern void Vec2_rotate__SWIG_0(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x06000231 RID: 561
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_set__SWIG_0")]
		public static extern void Vec2_set__SWIG_0(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x06000232 RID: 562
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_set__SWIG_1")]
		public static extern void Vec2_set__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000233 RID: 563
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_set__SWIG_2")]
		public static extern void Vec2_set__SWIG_2(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000234 RID: 564
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_setZero")]
		public static extern void Vec2_setZero(HandleRef jarg1);

		// Token: 0x06000235 RID: 565
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_subtract__SWIG_0")]
		public static extern void Vec2_subtract__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000236 RID: 566
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_subtract__SWIG_1")]
		public static extern void Vec2_subtract__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000237 RID: 567
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_smooth")]
		public static extern void Vec2_smooth(HandleRef jarg1, HandleRef jarg2, float jarg3, float jarg4);

		// Token: 0x06000238 RID: 568
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_setPoint")]
		public static extern void Vec2_setPoint(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x06000239 RID: 569
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_equals")]
		public static extern bool Vec2_equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600023A RID: 570
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_fuzzyEquals")]
		public static extern bool Vec2_fuzzyEquals(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x0600023B RID: 571
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getLength")]
		public static extern float Vec2_getLength(HandleRef jarg1);

		// Token: 0x0600023C RID: 572
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getLengthSq")]
		public static extern float Vec2_getLengthSq(HandleRef jarg1);

		// Token: 0x0600023D RID: 573
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getDistanceSq")]
		public static extern float Vec2_getDistanceSq(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600023E RID: 574
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getDistance")]
		public static extern float Vec2_getDistance(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600023F RID: 575
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getAngle__SWIG_0")]
		public static extern float Vec2_getAngle__SWIG_0(HandleRef jarg1);

		// Token: 0x06000240 RID: 576
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getAngle__SWIG_1")]
		public static extern float Vec2_getAngle__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000241 RID: 577
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_cross")]
		public static extern float Vec2_cross(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000242 RID: 578
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getPerp")]
		public static extern IntPtr Vec2_getPerp(HandleRef jarg1);

		// Token: 0x06000243 RID: 579
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getMidpoint")]
		public static extern IntPtr Vec2_getMidpoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000244 RID: 580
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getClampPoint")]
		public static extern IntPtr Vec2_getClampPoint(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000245 RID: 581
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getRPerp")]
		public static extern IntPtr Vec2_getRPerp(HandleRef jarg1);

		// Token: 0x06000246 RID: 582
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_project")]
		public static extern IntPtr Vec2_project(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000247 RID: 583
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_rotate__SWIG_1")]
		public static extern IntPtr Vec2_rotate__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000248 RID: 584
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_unrotate")]
		public static extern IntPtr Vec2_unrotate(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000249 RID: 585
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_lerp")]
		public static extern IntPtr Vec2_lerp(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x0600024A RID: 586
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_rotateByAngle")]
		public static extern IntPtr Vec2_rotateByAngle(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x0600024B RID: 587
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_forAngle")]
		public static extern IntPtr Vec2_forAngle(float jarg1);

		// Token: 0x0600024C RID: 588
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isLineOverlap")]
		public static extern bool Vec2_isLineOverlap(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x0600024D RID: 589
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isLineParallel")]
		public static extern bool Vec2_isLineParallel(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x0600024E RID: 590
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isSegmentOverlap__SWIG_0")]
		public static extern bool Vec2_isSegmentOverlap__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, HandleRef jarg5, HandleRef jarg6);

		// Token: 0x0600024F RID: 591
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isSegmentOverlap__SWIG_1")]
		public static extern bool Vec2_isSegmentOverlap__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, HandleRef jarg5);

		// Token: 0x06000250 RID: 592
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isSegmentOverlap__SWIG_2")]
		public static extern bool Vec2_isSegmentOverlap__SWIG_2(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000251 RID: 593
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_isSegmentIntersect")]
		public static extern bool Vec2_isSegmentIntersect(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000252 RID: 594
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_getIntersectPoint")]
		public static extern IntPtr Vec2_getIntersectPoint(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000253 RID: 595
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ZERO_get")]
		public static extern IntPtr Vec2_ZERO_get();

		// Token: 0x06000254 RID: 596
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ONE_get")]
		public static extern IntPtr Vec2_ONE_get();

		// Token: 0x06000255 RID: 597
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_UNIT_X_get")]
		public static extern IntPtr Vec2_UNIT_X_get();

		// Token: 0x06000256 RID: 598
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_UNIT_Y_get")]
		public static extern IntPtr Vec2_UNIT_Y_get();

		// Token: 0x06000257 RID: 599
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_MIDDLE_get")]
		public static extern IntPtr Vec2_ANCHOR_MIDDLE_get();

		// Token: 0x06000258 RID: 600
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_BOTTOM_LEFT_get")]
		public static extern IntPtr Vec2_ANCHOR_BOTTOM_LEFT_get();

		// Token: 0x06000259 RID: 601
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_TOP_LEFT_get")]
		public static extern IntPtr Vec2_ANCHOR_TOP_LEFT_get();

		// Token: 0x0600025A RID: 602
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_BOTTOM_RIGHT_get")]
		public static extern IntPtr Vec2_ANCHOR_BOTTOM_RIGHT_get();

		// Token: 0x0600025B RID: 603
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_TOP_RIGHT_get")]
		public static extern IntPtr Vec2_ANCHOR_TOP_RIGHT_get();

		// Token: 0x0600025C RID: 604
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_MIDDLE_RIGHT_get")]
		public static extern IntPtr Vec2_ANCHOR_MIDDLE_RIGHT_get();

		// Token: 0x0600025D RID: 605
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_MIDDLE_LEFT_get")]
		public static extern IntPtr Vec2_ANCHOR_MIDDLE_LEFT_get();

		// Token: 0x0600025E RID: 606
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_MIDDLE_TOP_get")]
		public static extern IntPtr Vec2_ANCHOR_MIDDLE_TOP_get();

		// Token: 0x0600025F RID: 607
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec2_ANCHOR_MIDDLE_BOTTOM_get")]
		public static extern IntPtr Vec2_ANCHOR_MIDDLE_BOTTOM_get();

		// Token: 0x06000260 RID: 608
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_x_set")]
		public static extern void Vec3_x_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000261 RID: 609
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_x_get")]
		public static extern float Vec3_x_get(HandleRef jarg1);

		// Token: 0x06000262 RID: 610
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_y_set")]
		public static extern void Vec3_y_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000263 RID: 611
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_y_get")]
		public static extern float Vec3_y_get(HandleRef jarg1);

		// Token: 0x06000264 RID: 612
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_z_set")]
		public static extern void Vec3_z_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000265 RID: 613
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_z_get")]
		public static extern float Vec3_z_get(HandleRef jarg1);

		// Token: 0x06000266 RID: 614
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec3__SWIG_0")]
		public static extern IntPtr new_Vec3__SWIG_0();

		// Token: 0x06000267 RID: 615
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec3__SWIG_1")]
		public static extern IntPtr new_Vec3__SWIG_1(float jarg1, float jarg2, float jarg3);

		// Token: 0x06000268 RID: 616
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec3__SWIG_2")]
		public static extern IntPtr new_Vec3__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000269 RID: 617
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Vec3__SWIG_3")]
		public static extern IntPtr new_Vec3__SWIG_3(HandleRef jarg1);

		// Token: 0x0600026A RID: 618
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_fromColor")]
		public static extern IntPtr Vec3_fromColor(uint jarg1);

		// Token: 0x0600026B RID: 619
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Vec3")]
		public static extern void delete_Vec3(HandleRef jarg1);

		// Token: 0x0600026C RID: 620
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_isZero")]
		public static extern bool Vec3_isZero(HandleRef jarg1);

		// Token: 0x0600026D RID: 621
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_isOne")]
		public static extern bool Vec3_isOne(HandleRef jarg1);

		// Token: 0x0600026E RID: 622
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_angle")]
		public static extern float Vec3_angle(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600026F RID: 623
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_add__SWIG_0")]
		public static extern void Vec3_add__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000270 RID: 624
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_add__SWIG_1")]
		public static extern void Vec3_add__SWIG_1(HandleRef jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x06000271 RID: 625
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_add__SWIG_2")]
		public static extern void Vec3_add__SWIG_2(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000272 RID: 626
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_clamp__SWIG_0")]
		public static extern void Vec3_clamp__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000273 RID: 627
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_clamp__SWIG_1")]
		public static extern void Vec3_clamp__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000274 RID: 628
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_cross__SWIG_0")]
		public static extern void Vec3_cross__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000275 RID: 629
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_cross__SWIG_1")]
		public static extern void Vec3_cross__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000276 RID: 630
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_distance")]
		public static extern float Vec3_distance(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000277 RID: 631
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_distanceSquared")]
		public static extern float Vec3_distanceSquared(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000278 RID: 632
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_dot__SWIG_0")]
		public static extern float Vec3_dot__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000279 RID: 633
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_length")]
		public static extern float Vec3_length(HandleRef jarg1);

		// Token: 0x0600027A RID: 634
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_lengthSquared")]
		public static extern float Vec3_lengthSquared(HandleRef jarg1);

		// Token: 0x0600027B RID: 635
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_negate")]
		public static extern void Vec3_negate(HandleRef jarg1);

		// Token: 0x0600027C RID: 636
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_normalize")]
		public static extern void Vec3_normalize(HandleRef jarg1);

		// Token: 0x0600027D RID: 637
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_getNormalized")]
		public static extern IntPtr Vec3_getNormalized(HandleRef jarg1);

		// Token: 0x0600027E RID: 638
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_scale")]
		public static extern void Vec3_scale(HandleRef jarg1, float jarg2);

		// Token: 0x0600027F RID: 639
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_set__SWIG_0")]
		public static extern void Vec3_set__SWIG_0(HandleRef jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x06000280 RID: 640
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_set__SWIG_1")]
		public static extern void Vec3_set__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000281 RID: 641
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_set__SWIG_2")]
		public static extern void Vec3_set__SWIG_2(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000282 RID: 642
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_setZero")]
		public static extern void Vec3_setZero(HandleRef jarg1);

		// Token: 0x06000283 RID: 643
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_subtract__SWIG_0")]
		public static extern void Vec3_subtract__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000284 RID: 644
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_subtract__SWIG_1")]
		public static extern void Vec3_subtract__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000285 RID: 645
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_smooth")]
		public static extern void Vec3_smooth(HandleRef jarg1, HandleRef jarg2, float jarg3, float jarg4);

		// Token: 0x06000286 RID: 646
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_lerp")]
		public static extern IntPtr Vec3_lerp(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x06000287 RID: 647
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_ZERO_get")]
		public static extern IntPtr Vec3_ZERO_get();

		// Token: 0x06000288 RID: 648
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_ONE_get")]
		public static extern IntPtr Vec3_ONE_get();

		// Token: 0x06000289 RID: 649
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_UNIT_X_get")]
		public static extern IntPtr Vec3_UNIT_X_get();

		// Token: 0x0600028A RID: 650
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_UNIT_Y_get")]
		public static extern IntPtr Vec3_UNIT_Y_get();

		// Token: 0x0600028B RID: 651
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Vec3_UNIT_Z_get")]
		public static extern IntPtr Vec3_UNIT_Z_get();

		// Token: 0x0600028C RID: 652
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_width_set")]
		public static extern void Size_width_set(HandleRef jarg1, float jarg2);

		// Token: 0x0600028D RID: 653
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_width_get")]
		public static extern float Size_width_get(HandleRef jarg1);

		// Token: 0x0600028E RID: 654
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_height_set")]
		public static extern void Size_height_set(HandleRef jarg1, float jarg2);

		// Token: 0x0600028F RID: 655
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_height_get")]
		public static extern float Size_height_get(HandleRef jarg1);

		// Token: 0x06000290 RID: 656
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Size__SWIG_0")]
		public static extern IntPtr new_Size__SWIG_0();

		// Token: 0x06000291 RID: 657
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Size__SWIG_1")]
		public static extern IntPtr new_Size__SWIG_1(float jarg1, float jarg2);

		// Token: 0x06000292 RID: 658
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Size__SWIG_2")]
		public static extern IntPtr new_Size__SWIG_2(HandleRef jarg1);

		// Token: 0x06000293 RID: 659
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Size__SWIG_3")]
		public static extern IntPtr new_Size__SWIG_3(HandleRef jarg1);

		// Token: 0x06000294 RID: 660
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_setSize")]
		public static extern void Size_setSize(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x06000295 RID: 661
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_equals")]
		public static extern bool Size_equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000296 RID: 662
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Size_ZERO_get")]
		public static extern IntPtr Size_ZERO_get();

		// Token: 0x06000297 RID: 663
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Size")]
		public static extern void delete_Size(HandleRef jarg1);

		// Token: 0x06000298 RID: 664
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_origin_set")]
		public static extern void Rect_origin_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000299 RID: 665
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_origin_get")]
		public static extern IntPtr Rect_origin_get(HandleRef jarg1);

		// Token: 0x0600029A RID: 666
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_size_set")]
		public static extern void Rect_size_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600029B RID: 667
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_size_get")]
		public static extern IntPtr Rect_size_get(HandleRef jarg1);

		// Token: 0x0600029C RID: 668
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Rect__SWIG_0")]
		public static extern IntPtr new_Rect__SWIG_0();

		// Token: 0x0600029D RID: 669
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Rect__SWIG_1")]
		public static extern IntPtr new_Rect__SWIG_1(float jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x0600029E RID: 670
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Rect__SWIG_2")]
		public static extern IntPtr new_Rect__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600029F RID: 671
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Rect__SWIG_3")]
		public static extern IntPtr new_Rect__SWIG_3(HandleRef jarg1);

		// Token: 0x060002A0 RID: 672
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_setRect")]
		public static extern void Rect_setRect(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

		// Token: 0x060002A1 RID: 673
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMinX")]
		public static extern float Rect_getMinX(HandleRef jarg1);

		// Token: 0x060002A2 RID: 674
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMidX")]
		public static extern float Rect_getMidX(HandleRef jarg1);

		// Token: 0x060002A3 RID: 675
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMaxX")]
		public static extern float Rect_getMaxX(HandleRef jarg1);

		// Token: 0x060002A4 RID: 676
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMinY")]
		public static extern float Rect_getMinY(HandleRef jarg1);

		// Token: 0x060002A5 RID: 677
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMidY")]
		public static extern float Rect_getMidY(HandleRef jarg1);

		// Token: 0x060002A6 RID: 678
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_getMaxY")]
		public static extern float Rect_getMaxY(HandleRef jarg1);

		// Token: 0x060002A7 RID: 679
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_equals")]
		public static extern bool Rect_equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002A8 RID: 680
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_containsPoint")]
		public static extern bool Rect_containsPoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002A9 RID: 681
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_intersectsRect")]
		public static extern bool Rect_intersectsRect(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002AA RID: 682
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_intersectsCircle")]
		public static extern bool Rect_intersectsCircle(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x060002AB RID: 683
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_unionWithRect")]
		public static extern IntPtr Rect_unionWithRect(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002AC RID: 684
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_merge")]
		public static extern void Rect_merge(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002AD RID: 685
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Rect_ZERO_get")]
		public static extern IntPtr Rect_ZERO_get();

		// Token: 0x060002AE RID: 686
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Rect")]
		public static extern void delete_Rect(HandleRef jarg1);

		// Token: 0x060002AF RID: 687
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_x_set")]
		public static extern void Quaternion_x_set(HandleRef jarg1, float jarg2);

		// Token: 0x060002B0 RID: 688
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_x_get")]
		public static extern float Quaternion_x_get(HandleRef jarg1);

		// Token: 0x060002B1 RID: 689
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_y_set")]
		public static extern void Quaternion_y_set(HandleRef jarg1, float jarg2);

		// Token: 0x060002B2 RID: 690
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_y_get")]
		public static extern float Quaternion_y_get(HandleRef jarg1);

		// Token: 0x060002B3 RID: 691
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_z_set")]
		public static extern void Quaternion_z_set(HandleRef jarg1, float jarg2);

		// Token: 0x060002B4 RID: 692
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_z_get")]
		public static extern float Quaternion_z_get(HandleRef jarg1);

		// Token: 0x060002B5 RID: 693
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_w_set")]
		public static extern void Quaternion_w_set(HandleRef jarg1, float jarg2);

		// Token: 0x060002B6 RID: 694
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_w_get")]
		public static extern float Quaternion_w_get(HandleRef jarg1);

		// Token: 0x060002B7 RID: 695
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quaternion__SWIG_0")]
		public static extern IntPtr new_Quaternion__SWIG_0();

		// Token: 0x060002B8 RID: 696
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quaternion__SWIG_1")]
		public static extern IntPtr new_Quaternion__SWIG_1(float jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x060002B9 RID: 697
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quaternion__SWIG_2")]
		public static extern IntPtr new_Quaternion__SWIG_2(HandleRef jarg1, float jarg2);

		// Token: 0x060002BA RID: 698
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quaternion__SWIG_3")]
		public static extern IntPtr new_Quaternion__SWIG_3(HandleRef jarg1);

		// Token: 0x060002BB RID: 699
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Quaternion")]
		public static extern void delete_Quaternion(HandleRef jarg1);

		// Token: 0x060002BC RID: 700
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_identity")]
		public static extern IntPtr Quaternion_identity();

		// Token: 0x060002BD RID: 701
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_zero")]
		public static extern IntPtr Quaternion_zero();

		// Token: 0x060002BE RID: 702
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_isIdentity")]
		public static extern bool Quaternion_isIdentity(HandleRef jarg1);

		// Token: 0x060002BF RID: 703
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_isZero")]
		public static extern bool Quaternion_isZero(HandleRef jarg1);

		// Token: 0x060002C0 RID: 704
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_createFromAxisAngle")]
		public static extern void Quaternion_createFromAxisAngle(HandleRef jarg1, float jarg2, HandleRef jarg3);

		// Token: 0x060002C1 RID: 705
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_conjugate")]
		public static extern void Quaternion_conjugate(HandleRef jarg1);

		// Token: 0x060002C2 RID: 706
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_getConjugated")]
		public static extern IntPtr Quaternion_getConjugated(HandleRef jarg1);

		// Token: 0x060002C3 RID: 707
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_inverse")]
		public static extern bool Quaternion_inverse(HandleRef jarg1);

		// Token: 0x060002C4 RID: 708
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_getInversed")]
		public static extern IntPtr Quaternion_getInversed(HandleRef jarg1);

		// Token: 0x060002C5 RID: 709
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_multiply__SWIG_0")]
		public static extern void Quaternion_multiply__SWIG_0(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002C6 RID: 710
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_multiply__SWIG_1")]
		public static extern void Quaternion_multiply__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x060002C7 RID: 711
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_normalize")]
		public static extern void Quaternion_normalize(HandleRef jarg1);

		// Token: 0x060002C8 RID: 712
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_getNormalized")]
		public static extern IntPtr Quaternion_getNormalized(HandleRef jarg1);

		// Token: 0x060002C9 RID: 713
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_set__SWIG_0")]
		public static extern void Quaternion_set__SWIG_0(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

		// Token: 0x060002CA RID: 714
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_set__SWIG_1")]
		public static extern void Quaternion_set__SWIG_1(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x060002CB RID: 715
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_set__SWIG_2")]
		public static extern void Quaternion_set__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002CC RID: 716
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_setIdentity")]
		public static extern void Quaternion_setIdentity(HandleRef jarg1);

		// Token: 0x060002CD RID: 717
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_toAxisAngle")]
		public static extern float Quaternion_toAxisAngle(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002CE RID: 718
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_lerp")]
		public static extern void Quaternion_lerp(HandleRef jarg1, HandleRef jarg2, float jarg3, HandleRef jarg4);

		// Token: 0x060002CF RID: 719
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_slerp")]
		public static extern void Quaternion_slerp(HandleRef jarg1, HandleRef jarg2, float jarg3, HandleRef jarg4);

		// Token: 0x060002D0 RID: 720
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_squad")]
		public static extern void Quaternion_squad(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, float jarg5, HandleRef jarg6);

		// Token: 0x060002D1 RID: 721
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quaternion_ZERO_get")]
		public static extern IntPtr Quaternion_ZERO_get();

		// Token: 0x060002D2 RID: 722
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color3B__SWIG_0")]
		public static extern IntPtr new_Color3B__SWIG_0();

		// Token: 0x060002D3 RID: 723
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color3B__SWIG_1")]
		public static extern IntPtr new_Color3B__SWIG_1(byte jarg1, byte jarg2, byte jarg3);

		// Token: 0x060002D4 RID: 724
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color3B__SWIG_2")]
		public static extern IntPtr new_Color3B__SWIG_2(HandleRef jarg1);

		// Token: 0x060002D5 RID: 725
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color3B__SWIG_3")]
		public static extern IntPtr new_Color3B__SWIG_3(HandleRef jarg1);

		// Token: 0x060002D6 RID: 726
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_equals")]
		public static extern bool Color3B_equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060002D7 RID: 727
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_r_set")]
		public static extern void Color3B_r_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002D8 RID: 728
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_r_get")]
		public static extern byte Color3B_r_get(HandleRef jarg1);

		// Token: 0x060002D9 RID: 729
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_g_set")]
		public static extern void Color3B_g_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002DA RID: 730
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_g_get")]
		public static extern byte Color3B_g_get(HandleRef jarg1);

		// Token: 0x060002DB RID: 731
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_b_set")]
		public static extern void Color3B_b_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002DC RID: 732
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_b_get")]
		public static extern byte Color3B_b_get(HandleRef jarg1);

		// Token: 0x060002DD RID: 733
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_WHITE_get")]
		public static extern IntPtr Color3B_WHITE_get();

		// Token: 0x060002DE RID: 734
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_YELLOW_get")]
		public static extern IntPtr Color3B_YELLOW_get();

		// Token: 0x060002DF RID: 735
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_BLUE_get")]
		public static extern IntPtr Color3B_BLUE_get();

		// Token: 0x060002E0 RID: 736
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_GREEN_get")]
		public static extern IntPtr Color3B_GREEN_get();

		// Token: 0x060002E1 RID: 737
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_RED_get")]
		public static extern IntPtr Color3B_RED_get();

		// Token: 0x060002E2 RID: 738
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_MAGENTA_get")]
		public static extern IntPtr Color3B_MAGENTA_get();

		// Token: 0x060002E3 RID: 739
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_BLACK_get")]
		public static extern IntPtr Color3B_BLACK_get();

		// Token: 0x060002E4 RID: 740
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_ORANGE_get")]
		public static extern IntPtr Color3B_ORANGE_get();

		// Token: 0x060002E5 RID: 741
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color3B_GRAY_get")]
		public static extern IntPtr Color3B_GRAY_get();

		// Token: 0x060002E6 RID: 742
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Color3B")]
		public static extern void delete_Color3B(HandleRef jarg1);

		// Token: 0x060002E7 RID: 743
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4B__SWIG_0")]
		public static extern IntPtr new_Color4B__SWIG_0();

		// Token: 0x060002E8 RID: 744
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4B__SWIG_1")]
		public static extern IntPtr new_Color4B__SWIG_1(byte jarg1, byte jarg2, byte jarg3, byte jarg4);

		// Token: 0x060002E9 RID: 745
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4B__SWIG_2")]
		public static extern IntPtr new_Color4B__SWIG_2(HandleRef jarg1);

		// Token: 0x060002EA RID: 746
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4B__SWIG_3")]
		public static extern IntPtr new_Color4B__SWIG_3(HandleRef jarg1);

		// Token: 0x060002EB RID: 747
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_r_set")]
		public static extern void Color4B_r_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002EC RID: 748
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_r_get")]
		public static extern byte Color4B_r_get(HandleRef jarg1);

		// Token: 0x060002ED RID: 749
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_g_set")]
		public static extern void Color4B_g_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002EE RID: 750
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_g_get")]
		public static extern byte Color4B_g_get(HandleRef jarg1);

		// Token: 0x060002EF RID: 751
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_b_set")]
		public static extern void Color4B_b_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002F0 RID: 752
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_b_get")]
		public static extern byte Color4B_b_get(HandleRef jarg1);

		// Token: 0x060002F1 RID: 753
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_a_set")]
		public static extern void Color4B_a_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060002F2 RID: 754
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_a_get")]
		public static extern byte Color4B_a_get(HandleRef jarg1);

		// Token: 0x060002F3 RID: 755
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_WHITE_get")]
		public static extern IntPtr Color4B_WHITE_get();

		// Token: 0x060002F4 RID: 756
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_YELLOW_get")]
		public static extern IntPtr Color4B_YELLOW_get();

		// Token: 0x060002F5 RID: 757
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_BLUE_get")]
		public static extern IntPtr Color4B_BLUE_get();

		// Token: 0x060002F6 RID: 758
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_GREEN_get")]
		public static extern IntPtr Color4B_GREEN_get();

		// Token: 0x060002F7 RID: 759
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_RED_get")]
		public static extern IntPtr Color4B_RED_get();

		// Token: 0x060002F8 RID: 760
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_MAGENTA_get")]
		public static extern IntPtr Color4B_MAGENTA_get();

		// Token: 0x060002F9 RID: 761
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_BLACK_get")]
		public static extern IntPtr Color4B_BLACK_get();

		// Token: 0x060002FA RID: 762
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_ORANGE_get")]
		public static extern IntPtr Color4B_ORANGE_get();

		// Token: 0x060002FB RID: 763
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4B_GRAY_get")]
		public static extern IntPtr Color4B_GRAY_get();

		// Token: 0x060002FC RID: 764
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Color4B")]
		public static extern void delete_Color4B(HandleRef jarg1);

		// Token: 0x060002FD RID: 765
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4F__SWIG_0")]
		public static extern IntPtr new_Color4F__SWIG_0();

		// Token: 0x060002FE RID: 766
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4F__SWIG_1")]
		public static extern IntPtr new_Color4F__SWIG_1(float jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x060002FF RID: 767
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4F__SWIG_2")]
		public static extern IntPtr new_Color4F__SWIG_2(HandleRef jarg1);

		// Token: 0x06000300 RID: 768
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Color4F__SWIG_3")]
		public static extern IntPtr new_Color4F__SWIG_3(HandleRef jarg1);

		// Token: 0x06000301 RID: 769
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_equals")]
		public static extern bool Color4F_equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000302 RID: 770
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_r_set")]
		public static extern void Color4F_r_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000303 RID: 771
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_r_get")]
		public static extern float Color4F_r_get(HandleRef jarg1);

		// Token: 0x06000304 RID: 772
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_g_set")]
		public static extern void Color4F_g_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000305 RID: 773
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_g_get")]
		public static extern float Color4F_g_get(HandleRef jarg1);

		// Token: 0x06000306 RID: 774
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_b_set")]
		public static extern void Color4F_b_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000307 RID: 775
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_b_get")]
		public static extern float Color4F_b_get(HandleRef jarg1);

		// Token: 0x06000308 RID: 776
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_a_set")]
		public static extern void Color4F_a_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000309 RID: 777
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_a_get")]
		public static extern float Color4F_a_get(HandleRef jarg1);

		// Token: 0x0600030A RID: 778
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_WHITE_get")]
		public static extern IntPtr Color4F_WHITE_get();

		// Token: 0x0600030B RID: 779
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_YELLOW_get")]
		public static extern IntPtr Color4F_YELLOW_get();

		// Token: 0x0600030C RID: 780
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_BLUE_get")]
		public static extern IntPtr Color4F_BLUE_get();

		// Token: 0x0600030D RID: 781
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_GREEN_get")]
		public static extern IntPtr Color4F_GREEN_get();

		// Token: 0x0600030E RID: 782
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_RED_get")]
		public static extern IntPtr Color4F_RED_get();

		// Token: 0x0600030F RID: 783
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_MAGENTA_get")]
		public static extern IntPtr Color4F_MAGENTA_get();

		// Token: 0x06000310 RID: 784
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_BLACK_get")]
		public static extern IntPtr Color4F_BLACK_get();

		// Token: 0x06000311 RID: 785
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_ORANGE_get")]
		public static extern IntPtr Color4F_ORANGE_get();

		// Token: 0x06000312 RID: 786
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Color4F_GRAY_get")]
		public static extern IntPtr Color4F_GRAY_get();

		// Token: 0x06000313 RID: 787
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Color4F")]
		public static extern void delete_Color4F(HandleRef jarg1);

		// Token: 0x06000314 RID: 788
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Tex2F__SWIG_0")]
		public static extern IntPtr new_Tex2F__SWIG_0(float jarg1, float jarg2);

		// Token: 0x06000315 RID: 789
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Tex2F__SWIG_1")]
		public static extern IntPtr new_Tex2F__SWIG_1();

		// Token: 0x06000316 RID: 790
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Tex2F_u_set")]
		public static extern void Tex2F_u_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000317 RID: 791
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Tex2F_u_get")]
		public static extern float Tex2F_u_get(HandleRef jarg1);

		// Token: 0x06000318 RID: 792
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Tex2F_v_set")]
		public static extern void Tex2F_v_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000319 RID: 793
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Tex2F_v_get")]
		public static extern float Tex2F_v_get(HandleRef jarg1);

		// Token: 0x0600031A RID: 794
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Tex2F")]
		public static extern void delete_Tex2F(HandleRef jarg1);

		// Token: 0x0600031B RID: 795
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_pos_set")]
		public static extern void PointSprite_pos_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600031C RID: 796
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_pos_get")]
		public static extern IntPtr PointSprite_pos_get(HandleRef jarg1);

		// Token: 0x0600031D RID: 797
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_color_set")]
		public static extern void PointSprite_color_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600031E RID: 798
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_color_get")]
		public static extern IntPtr PointSprite_color_get(HandleRef jarg1);

		// Token: 0x0600031F RID: 799
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_size_set")]
		public static extern void PointSprite_size_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000320 RID: 800
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_PointSprite_size_get")]
		public static extern float PointSprite_size_get(HandleRef jarg1);

		// Token: 0x06000321 RID: 801
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_PointSprite")]
		public static extern IntPtr new_PointSprite();

		// Token: 0x06000322 RID: 802
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_PointSprite")]
		public static extern void delete_PointSprite(HandleRef jarg1);

		// Token: 0x06000323 RID: 803
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_tl_set")]
		public static extern void Quad2_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000324 RID: 804
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_tl_get")]
		public static extern IntPtr Quad2_tl_get(HandleRef jarg1);

		// Token: 0x06000325 RID: 805
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_tr_set")]
		public static extern void Quad2_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000326 RID: 806
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_tr_get")]
		public static extern IntPtr Quad2_tr_get(HandleRef jarg1);

		// Token: 0x06000327 RID: 807
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_bl_set")]
		public static extern void Quad2_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000328 RID: 808
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_bl_get")]
		public static extern IntPtr Quad2_bl_get(HandleRef jarg1);

		// Token: 0x06000329 RID: 809
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_br_set")]
		public static extern void Quad2_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600032A RID: 810
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad2_br_get")]
		public static extern IntPtr Quad2_br_get(HandleRef jarg1);

		// Token: 0x0600032B RID: 811
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quad2")]
		public static extern IntPtr new_Quad2();

		// Token: 0x0600032C RID: 812
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Quad2")]
		public static extern void delete_Quad2(HandleRef jarg1);

		// Token: 0x0600032D RID: 813
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_bl_set")]
		public static extern void Quad3_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600032E RID: 814
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_bl_get")]
		public static extern IntPtr Quad3_bl_get(HandleRef jarg1);

		// Token: 0x0600032F RID: 815
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_br_set")]
		public static extern void Quad3_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000330 RID: 816
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_br_get")]
		public static extern IntPtr Quad3_br_get(HandleRef jarg1);

		// Token: 0x06000331 RID: 817
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_tl_set")]
		public static extern void Quad3_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000332 RID: 818
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_tl_get")]
		public static extern IntPtr Quad3_tl_get(HandleRef jarg1);

		// Token: 0x06000333 RID: 819
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_tr_set")]
		public static extern void Quad3_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000334 RID: 820
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Quad3_tr_get")]
		public static extern IntPtr Quad3_tr_get(HandleRef jarg1);

		// Token: 0x06000335 RID: 821
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Quad3")]
		public static extern IntPtr new_Quad3();

		// Token: 0x06000336 RID: 822
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Quad3")]
		public static extern void delete_Quad3(HandleRef jarg1);

		// Token: 0x06000337 RID: 823
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_vertices_set")]
		public static extern void V2F_C4B_T2F_vertices_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000338 RID: 824
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_vertices_get")]
		public static extern IntPtr V2F_C4B_T2F_vertices_get(HandleRef jarg1);

		// Token: 0x06000339 RID: 825
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_colors_set")]
		public static extern void V2F_C4B_T2F_colors_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600033A RID: 826
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_colors_get")]
		public static extern IntPtr V2F_C4B_T2F_colors_get(HandleRef jarg1);

		// Token: 0x0600033B RID: 827
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_texCoords_set")]
		public static extern void V2F_C4B_T2F_texCoords_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600033C RID: 828
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_texCoords_get")]
		public static extern IntPtr V2F_C4B_T2F_texCoords_get(HandleRef jarg1);

		// Token: 0x0600033D RID: 829
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4B_T2F")]
		public static extern IntPtr new_V2F_C4B_T2F();

		// Token: 0x0600033E RID: 830
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4B_T2F")]
		public static extern void delete_V2F_C4B_T2F(HandleRef jarg1);

		// Token: 0x0600033F RID: 831
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_vertices_set")]
		public static extern void V2F_C4B_PF_vertices_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000340 RID: 832
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_vertices_get")]
		public static extern IntPtr V2F_C4B_PF_vertices_get(HandleRef jarg1);

		// Token: 0x06000341 RID: 833
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_colors_set")]
		public static extern void V2F_C4B_PF_colors_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000342 RID: 834
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_colors_get")]
		public static extern IntPtr V2F_C4B_PF_colors_get(HandleRef jarg1);

		// Token: 0x06000343 RID: 835
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_pointSize_set")]
		public static extern void V2F_C4B_PF_pointSize_set(HandleRef jarg1, float jarg2);

		// Token: 0x06000344 RID: 836
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_PF_pointSize_get")]
		public static extern float V2F_C4B_PF_pointSize_get(HandleRef jarg1);

		// Token: 0x06000345 RID: 837
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4B_PF")]
		public static extern IntPtr new_V2F_C4B_PF();

		// Token: 0x06000346 RID: 838
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4B_PF")]
		public static extern void delete_V2F_C4B_PF(HandleRef jarg1);

		// Token: 0x06000347 RID: 839
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_vertices_set")]
		public static extern void V2F_C4F_T2F_vertices_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000348 RID: 840
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_vertices_get")]
		public static extern IntPtr V2F_C4F_T2F_vertices_get(HandleRef jarg1);

		// Token: 0x06000349 RID: 841
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_colors_set")]
		public static extern void V2F_C4F_T2F_colors_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600034A RID: 842
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_colors_get")]
		public static extern IntPtr V2F_C4F_T2F_colors_get(HandleRef jarg1);

		// Token: 0x0600034B RID: 843
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_texCoords_set")]
		public static extern void V2F_C4F_T2F_texCoords_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600034C RID: 844
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_texCoords_get")]
		public static extern IntPtr V2F_C4F_T2F_texCoords_get(HandleRef jarg1);

		// Token: 0x0600034D RID: 845
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4F_T2F")]
		public static extern IntPtr new_V2F_C4F_T2F();

		// Token: 0x0600034E RID: 846
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4F_T2F")]
		public static extern void delete_V2F_C4F_T2F(HandleRef jarg1);

		// Token: 0x0600034F RID: 847
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_vertices_set")]
		public static extern void V3F_C4B_T2F_vertices_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000350 RID: 848
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_vertices_get")]
		public static extern IntPtr V3F_C4B_T2F_vertices_get(HandleRef jarg1);

		// Token: 0x06000351 RID: 849
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_colors_set")]
		public static extern void V3F_C4B_T2F_colors_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000352 RID: 850
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_colors_get")]
		public static extern IntPtr V3F_C4B_T2F_colors_get(HandleRef jarg1);

		// Token: 0x06000353 RID: 851
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_texCoords_set")]
		public static extern void V3F_C4B_T2F_texCoords_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000354 RID: 852
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_texCoords_get")]
		public static extern IntPtr V3F_C4B_T2F_texCoords_get(HandleRef jarg1);

		// Token: 0x06000355 RID: 853
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V3F_C4B_T2F")]
		public static extern IntPtr new_V3F_C4B_T2F();

		// Token: 0x06000356 RID: 854
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V3F_C4B_T2F")]
		public static extern void delete_V3F_C4B_T2F(HandleRef jarg1);

		// Token: 0x06000357 RID: 855
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_vertices_set")]
		public static extern void V3F_T2F_vertices_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000358 RID: 856
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_vertices_get")]
		public static extern IntPtr V3F_T2F_vertices_get(HandleRef jarg1);

		// Token: 0x06000359 RID: 857
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_texCoords_set")]
		public static extern void V3F_T2F_texCoords_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600035A RID: 858
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_texCoords_get")]
		public static extern IntPtr V3F_T2F_texCoords_get(HandleRef jarg1);

		// Token: 0x0600035B RID: 859
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V3F_T2F")]
		public static extern IntPtr new_V3F_T2F();

		// Token: 0x0600035C RID: 860
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V3F_T2F")]
		public static extern void delete_V3F_T2F(HandleRef jarg1);

		// Token: 0x0600035D RID: 861
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_a_set")]
		public static extern void V2F_C4B_T2F_Triangle_a_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600035E RID: 862
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_a_get")]
		public static extern IntPtr V2F_C4B_T2F_Triangle_a_get(HandleRef jarg1);

		// Token: 0x0600035F RID: 863
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_b_set")]
		public static extern void V2F_C4B_T2F_Triangle_b_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000360 RID: 864
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_b_get")]
		public static extern IntPtr V2F_C4B_T2F_Triangle_b_get(HandleRef jarg1);

		// Token: 0x06000361 RID: 865
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_c_set")]
		public static extern void V2F_C4B_T2F_Triangle_c_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000362 RID: 866
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Triangle_c_get")]
		public static extern IntPtr V2F_C4B_T2F_Triangle_c_get(HandleRef jarg1);

		// Token: 0x06000363 RID: 867
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4B_T2F_Triangle")]
		public static extern IntPtr new_V2F_C4B_T2F_Triangle();

		// Token: 0x06000364 RID: 868
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4B_T2F_Triangle")]
		public static extern void delete_V2F_C4B_T2F_Triangle(HandleRef jarg1);

		// Token: 0x06000365 RID: 869
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_bl_set")]
		public static extern void V2F_C4B_T2F_Quad_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000366 RID: 870
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_bl_get")]
		public static extern IntPtr V2F_C4B_T2F_Quad_bl_get(HandleRef jarg1);

		// Token: 0x06000367 RID: 871
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_br_set")]
		public static extern void V2F_C4B_T2F_Quad_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000368 RID: 872
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_br_get")]
		public static extern IntPtr V2F_C4B_T2F_Quad_br_get(HandleRef jarg1);

		// Token: 0x06000369 RID: 873
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_tl_set")]
		public static extern void V2F_C4B_T2F_Quad_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600036A RID: 874
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_tl_get")]
		public static extern IntPtr V2F_C4B_T2F_Quad_tl_get(HandleRef jarg1);

		// Token: 0x0600036B RID: 875
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_tr_set")]
		public static extern void V2F_C4B_T2F_Quad_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600036C RID: 876
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4B_T2F_Quad_tr_get")]
		public static extern IntPtr V2F_C4B_T2F_Quad_tr_get(HandleRef jarg1);

		// Token: 0x0600036D RID: 877
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4B_T2F_Quad")]
		public static extern IntPtr new_V2F_C4B_T2F_Quad();

		// Token: 0x0600036E RID: 878
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4B_T2F_Quad")]
		public static extern void delete_V2F_C4B_T2F_Quad(HandleRef jarg1);

		// Token: 0x0600036F RID: 879
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_tl_set")]
		public static extern void V3F_C4B_T2F_Quad_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000370 RID: 880
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_tl_get")]
		public static extern IntPtr V3F_C4B_T2F_Quad_tl_get(HandleRef jarg1);

		// Token: 0x06000371 RID: 881
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_bl_set")]
		public static extern void V3F_C4B_T2F_Quad_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000372 RID: 882
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_bl_get")]
		public static extern IntPtr V3F_C4B_T2F_Quad_bl_get(HandleRef jarg1);

		// Token: 0x06000373 RID: 883
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_tr_set")]
		public static extern void V3F_C4B_T2F_Quad_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000374 RID: 884
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_tr_get")]
		public static extern IntPtr V3F_C4B_T2F_Quad_tr_get(HandleRef jarg1);

		// Token: 0x06000375 RID: 885
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_br_set")]
		public static extern void V3F_C4B_T2F_Quad_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000376 RID: 886
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_C4B_T2F_Quad_br_get")]
		public static extern IntPtr V3F_C4B_T2F_Quad_br_get(HandleRef jarg1);

		// Token: 0x06000377 RID: 887
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V3F_C4B_T2F_Quad")]
		public static extern IntPtr new_V3F_C4B_T2F_Quad();

		// Token: 0x06000378 RID: 888
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V3F_C4B_T2F_Quad")]
		public static extern void delete_V3F_C4B_T2F_Quad(HandleRef jarg1);

		// Token: 0x06000379 RID: 889
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_bl_set")]
		public static extern void V2F_C4F_T2F_Quad_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600037A RID: 890
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_bl_get")]
		public static extern IntPtr V2F_C4F_T2F_Quad_bl_get(HandleRef jarg1);

		// Token: 0x0600037B RID: 891
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_br_set")]
		public static extern void V2F_C4F_T2F_Quad_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600037C RID: 892
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_br_get")]
		public static extern IntPtr V2F_C4F_T2F_Quad_br_get(HandleRef jarg1);

		// Token: 0x0600037D RID: 893
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_tl_set")]
		public static extern void V2F_C4F_T2F_Quad_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600037E RID: 894
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_tl_get")]
		public static extern IntPtr V2F_C4F_T2F_Quad_tl_get(HandleRef jarg1);

		// Token: 0x0600037F RID: 895
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_tr_set")]
		public static extern void V2F_C4F_T2F_Quad_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000380 RID: 896
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V2F_C4F_T2F_Quad_tr_get")]
		public static extern IntPtr V2F_C4F_T2F_Quad_tr_get(HandleRef jarg1);

		// Token: 0x06000381 RID: 897
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V2F_C4F_T2F_Quad")]
		public static extern IntPtr new_V2F_C4F_T2F_Quad();

		// Token: 0x06000382 RID: 898
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V2F_C4F_T2F_Quad")]
		public static extern void delete_V2F_C4F_T2F_Quad(HandleRef jarg1);

		// Token: 0x06000383 RID: 899
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_bl_set")]
		public static extern void V3F_T2F_Quad_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000384 RID: 900
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_bl_get")]
		public static extern IntPtr V3F_T2F_Quad_bl_get(HandleRef jarg1);

		// Token: 0x06000385 RID: 901
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_br_set")]
		public static extern void V3F_T2F_Quad_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000386 RID: 902
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_br_get")]
		public static extern IntPtr V3F_T2F_Quad_br_get(HandleRef jarg1);

		// Token: 0x06000387 RID: 903
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_tl_set")]
		public static extern void V3F_T2F_Quad_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000388 RID: 904
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_tl_get")]
		public static extern IntPtr V3F_T2F_Quad_tl_get(HandleRef jarg1);

		// Token: 0x06000389 RID: 905
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_tr_set")]
		public static extern void V3F_T2F_Quad_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600038A RID: 906
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_V3F_T2F_Quad_tr_get")]
		public static extern IntPtr V3F_T2F_Quad_tr_get(HandleRef jarg1);

		// Token: 0x0600038B RID: 907
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_V3F_T2F_Quad")]
		public static extern IntPtr new_V3F_T2F_Quad();

		// Token: 0x0600038C RID: 908
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_V3F_T2F_Quad")]
		public static extern void delete_V3F_T2F_Quad(HandleRef jarg1);

		// Token: 0x0600038D RID: 909
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_src_set")]
		public static extern void BlendFunc_src_set(HandleRef jarg1, uint jarg2);

		// Token: 0x0600038E RID: 910
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_src_get")]
		public static extern uint BlendFunc_src_get(HandleRef jarg1);

		// Token: 0x0600038F RID: 911
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_dst_set")]
		public static extern void BlendFunc_dst_set(HandleRef jarg1, uint jarg2);

		// Token: 0x06000390 RID: 912
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_dst_get")]
		public static extern uint BlendFunc_dst_get(HandleRef jarg1);

		// Token: 0x06000391 RID: 913
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_DISABLE_get")]
		public static extern IntPtr BlendFunc_DISABLE_get();

		// Token: 0x06000392 RID: 914
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_ALPHA_PREMULTIPLIED_get")]
		public static extern IntPtr BlendFunc_ALPHA_PREMULTIPLIED_get();

		// Token: 0x06000393 RID: 915
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_ALPHA_NON_PREMULTIPLIED_get")]
		public static extern IntPtr BlendFunc_ALPHA_NON_PREMULTIPLIED_get();

		// Token: 0x06000394 RID: 916
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_BlendFunc_ADDITIVE_get")]
		public static extern IntPtr BlendFunc_ADDITIVE_get();

		// Token: 0x06000395 RID: 917
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_BlendFunc")]
		public static extern IntPtr new_BlendFunc();

		// Token: 0x06000396 RID: 918
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_BlendFunc")]
		public static extern void delete_BlendFunc(HandleRef jarg1);

		// Token: 0x06000397 RID: 919
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_bl_set")]
		public static extern void T2F_Quad_bl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000398 RID: 920
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_bl_get")]
		public static extern IntPtr T2F_Quad_bl_get(HandleRef jarg1);

		// Token: 0x06000399 RID: 921
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_br_set")]
		public static extern void T2F_Quad_br_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600039A RID: 922
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_br_get")]
		public static extern IntPtr T2F_Quad_br_get(HandleRef jarg1);

		// Token: 0x0600039B RID: 923
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_tl_set")]
		public static extern void T2F_Quad_tl_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600039C RID: 924
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_tl_get")]
		public static extern IntPtr T2F_Quad_tl_get(HandleRef jarg1);

		// Token: 0x0600039D RID: 925
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_tr_set")]
		public static extern void T2F_Quad_tr_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600039E RID: 926
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_T2F_Quad_tr_get")]
		public static extern IntPtr T2F_Quad_tr_get(HandleRef jarg1);

		// Token: 0x0600039F RID: 927
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_T2F_Quad")]
		public static extern IntPtr new_T2F_Quad();

		// Token: 0x060003A0 RID: 928
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_T2F_Quad")]
		public static extern void delete_T2F_Quad(HandleRef jarg1);

		// Token: 0x060003A1 RID: 929
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_texCoords_set")]
		public static extern void AnimationFrameData_texCoords_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003A2 RID: 930
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_texCoords_get")]
		public static extern IntPtr AnimationFrameData_texCoords_get(HandleRef jarg1);

		// Token: 0x060003A3 RID: 931
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_delay_set")]
		public static extern void AnimationFrameData_delay_set(HandleRef jarg1, float jarg2);

		// Token: 0x060003A4 RID: 932
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_delay_get")]
		public static extern float AnimationFrameData_delay_get(HandleRef jarg1);

		// Token: 0x060003A5 RID: 933
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_size_set")]
		public static extern void AnimationFrameData_size_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003A6 RID: 934
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationFrameData_size_get")]
		public static extern IntPtr AnimationFrameData_size_get(HandleRef jarg1);

		// Token: 0x060003A7 RID: 935
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_AnimationFrameData")]
		public static extern IntPtr new_AnimationFrameData();

		// Token: 0x060003A8 RID: 936
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_AnimationFrameData")]
		public static extern void delete_AnimationFrameData(HandleRef jarg1);

		// Token: 0x060003A9 RID: 937
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_FontShadow")]
		public static extern IntPtr new_FontShadow();

		// Token: 0x060003AA RID: 938
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowEnabled_set")]
		public static extern void FontShadow__shadowEnabled_set(HandleRef jarg1, bool jarg2);

		// Token: 0x060003AB RID: 939
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowEnabled_get")]
		public static extern bool FontShadow__shadowEnabled_get(HandleRef jarg1);

		// Token: 0x060003AC RID: 940
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowOffset_set")]
		public static extern void FontShadow__shadowOffset_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003AD RID: 941
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowOffset_get")]
		public static extern IntPtr FontShadow__shadowOffset_get(HandleRef jarg1);

		// Token: 0x060003AE RID: 942
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowBlur_set")]
		public static extern void FontShadow__shadowBlur_set(HandleRef jarg1, float jarg2);

		// Token: 0x060003AF RID: 943
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowBlur_get")]
		public static extern float FontShadow__shadowBlur_get(HandleRef jarg1);

		// Token: 0x060003B0 RID: 944
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowOpacity_set")]
		public static extern void FontShadow__shadowOpacity_set(HandleRef jarg1, float jarg2);

		// Token: 0x060003B1 RID: 945
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontShadow__shadowOpacity_get")]
		public static extern float FontShadow__shadowOpacity_get(HandleRef jarg1);

		// Token: 0x060003B2 RID: 946
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_FontShadow")]
		public static extern void delete_FontShadow(HandleRef jarg1);

		// Token: 0x060003B3 RID: 947
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_FontStroke")]
		public static extern IntPtr new_FontStroke();

		// Token: 0x060003B4 RID: 948
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeEnabled_set")]
		public static extern void FontStroke__strokeEnabled_set(HandleRef jarg1, bool jarg2);

		// Token: 0x060003B5 RID: 949
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeEnabled_get")]
		public static extern bool FontStroke__strokeEnabled_get(HandleRef jarg1);

		// Token: 0x060003B6 RID: 950
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeColor_set")]
		public static extern void FontStroke__strokeColor_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003B7 RID: 951
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeColor_get")]
		public static extern IntPtr FontStroke__strokeColor_get(HandleRef jarg1);

		// Token: 0x060003B8 RID: 952
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeAlpha_set")]
		public static extern void FontStroke__strokeAlpha_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060003B9 RID: 953
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeAlpha_get")]
		public static extern byte FontStroke__strokeAlpha_get(HandleRef jarg1);

		// Token: 0x060003BA RID: 954
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeSize_set")]
		public static extern void FontStroke__strokeSize_set(HandleRef jarg1, float jarg2);

		// Token: 0x060003BB RID: 955
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontStroke__strokeSize_get")]
		public static extern float FontStroke__strokeSize_get(HandleRef jarg1);

		// Token: 0x060003BC RID: 956
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_FontStroke")]
		public static extern void delete_FontStroke(HandleRef jarg1);

		// Token: 0x060003BD RID: 957
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_FontDefinition")]
		public static extern IntPtr new_FontDefinition();

		// Token: 0x060003BE RID: 958
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontName_set")]
		public static extern void FontDefinition__fontName_set(HandleRef jarg1, string jarg2);

		// Token: 0x060003BF RID: 959
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontName_get")]
		public static extern string FontDefinition__fontName_get(HandleRef jarg1);

		// Token: 0x060003C0 RID: 960
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontSize_set")]
		public static extern void FontDefinition__fontSize_set(HandleRef jarg1, int jarg2);

		// Token: 0x060003C1 RID: 961
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontSize_get")]
		public static extern int FontDefinition__fontSize_get(HandleRef jarg1);

		// Token: 0x060003C2 RID: 962
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__alignment_set")]
		public static extern void FontDefinition__alignment_set(HandleRef jarg1, int jarg2);

		// Token: 0x060003C3 RID: 963
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__alignment_get")]
		public static extern int FontDefinition__alignment_get(HandleRef jarg1);

		// Token: 0x060003C4 RID: 964
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__vertAlignment_set")]
		public static extern void FontDefinition__vertAlignment_set(HandleRef jarg1, int jarg2);

		// Token: 0x060003C5 RID: 965
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__vertAlignment_get")]
		public static extern int FontDefinition__vertAlignment_get(HandleRef jarg1);

		// Token: 0x060003C6 RID: 966
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__dimensions_set")]
		public static extern void FontDefinition__dimensions_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003C7 RID: 967
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__dimensions_get")]
		public static extern IntPtr FontDefinition__dimensions_get(HandleRef jarg1);

		// Token: 0x060003C8 RID: 968
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontFillColor_set")]
		public static extern void FontDefinition__fontFillColor_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003C9 RID: 969
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontFillColor_get")]
		public static extern IntPtr FontDefinition__fontFillColor_get(HandleRef jarg1);

		// Token: 0x060003CA RID: 970
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontAlpha_set")]
		public static extern void FontDefinition__fontAlpha_set(HandleRef jarg1, byte jarg2);

		// Token: 0x060003CB RID: 971
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__fontAlpha_get")]
		public static extern byte FontDefinition__fontAlpha_get(HandleRef jarg1);

		// Token: 0x060003CC RID: 972
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__shadow_set")]
		public static extern void FontDefinition__shadow_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003CD RID: 973
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__shadow_get")]
		public static extern IntPtr FontDefinition__shadow_get(HandleRef jarg1);

		// Token: 0x060003CE RID: 974
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__stroke_set")]
		public static extern void FontDefinition__stroke_set(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003CF RID: 975
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_FontDefinition__stroke_get")]
		public static extern IntPtr FontDefinition__stroke_get(HandleRef jarg1);

		// Token: 0x060003D0 RID: 976
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_FontDefinition")]
		public static extern void delete_FontDefinition(HandleRef jarg1);

		// Token: 0x060003D1 RID: 977
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_x_set")]
		public static extern void Acceleration_x_set(HandleRef jarg1, double jarg2);

		// Token: 0x060003D2 RID: 978
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_x_get")]
		public static extern double Acceleration_x_get(HandleRef jarg1);

		// Token: 0x060003D3 RID: 979
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_y_set")]
		public static extern void Acceleration_y_set(HandleRef jarg1, double jarg2);

		// Token: 0x060003D4 RID: 980
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_y_get")]
		public static extern double Acceleration_y_get(HandleRef jarg1);

		// Token: 0x060003D5 RID: 981
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_z_set")]
		public static extern void Acceleration_z_set(HandleRef jarg1, double jarg2);

		// Token: 0x060003D6 RID: 982
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_z_get")]
		public static extern double Acceleration_z_get(HandleRef jarg1);

		// Token: 0x060003D7 RID: 983
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_timestamp_set")]
		public static extern void Acceleration_timestamp_set(HandleRef jarg1, double jarg2);

		// Token: 0x060003D8 RID: 984
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_Acceleration_timestamp_get")]
		public static extern double Acceleration_timestamp_get(HandleRef jarg1);

		// Token: 0x060003D9 RID: 985
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_Acceleration")]
		public static extern IntPtr new_Acceleration();

		// Token: 0x060003DA RID: 986
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_Acceleration")]
		public static extern void delete_Acceleration(HandleRef jarg1);

		// Token: 0x060003DB RID: 987
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_STD_STRING_EMPTY_get")]
		public static extern string STD_STRING_EMPTY_get();

		// Token: 0x060003DC RID: 988
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CocosGUIVersion")]
		public static extern string CocosGUIVersion();

		// Token: 0x060003DD RID: 989
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Clear")]
		public static extern void CSVectorPoint_Clear(HandleRef jarg1);

		// Token: 0x060003DE RID: 990
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Add")]
		public static extern void CSVectorPoint_Add(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003DF RID: 991
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_size")]
		public static extern uint CSVectorPoint_size(HandleRef jarg1);

		// Token: 0x060003E0 RID: 992
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_capacity")]
		public static extern uint CSVectorPoint_capacity(HandleRef jarg1);

		// Token: 0x060003E1 RID: 993
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_reserve")]
		public static extern void CSVectorPoint_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060003E2 RID: 994
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorPoint__SWIG_0")]
		public static extern IntPtr new_CSVectorPoint__SWIG_0();

		// Token: 0x060003E3 RID: 995
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorPoint__SWIG_1")]
		public static extern IntPtr new_CSVectorPoint__SWIG_1(HandleRef jarg1);

		// Token: 0x060003E4 RID: 996
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorPoint__SWIG_2")]
		public static extern IntPtr new_CSVectorPoint__SWIG_2(int jarg1);

		// Token: 0x060003E5 RID: 997
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_getitemcopy")]
		public static extern IntPtr CSVectorPoint_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060003E6 RID: 998
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_getitem")]
		public static extern IntPtr CSVectorPoint_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060003E7 RID: 999
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_setitem")]
		public static extern void CSVectorPoint_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060003E8 RID: 1000
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_AddRange")]
		public static extern void CSVectorPoint_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003E9 RID: 1001
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_GetRange")]
		public static extern IntPtr CSVectorPoint_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060003EA RID: 1002
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Insert")]
		public static extern void CSVectorPoint_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060003EB RID: 1003
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_InsertRange")]
		public static extern void CSVectorPoint_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060003EC RID: 1004
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_RemoveAt")]
		public static extern void CSVectorPoint_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x060003ED RID: 1005
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_RemoveRange")]
		public static extern void CSVectorPoint_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060003EE RID: 1006
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Repeat")]
		public static extern IntPtr CSVectorPoint_Repeat(HandleRef jarg1, int jarg2);

		// Token: 0x060003EF RID: 1007
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Reverse__SWIG_0")]
		public static extern void CSVectorPoint_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x060003F0 RID: 1008
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_Reverse__SWIG_1")]
		public static extern void CSVectorPoint_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x060003F1 RID: 1009
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorPoint_SetRange")]
		public static extern void CSVectorPoint_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060003F2 RID: 1010
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorPoint")]
		public static extern void delete_CSVectorPoint(HandleRef jarg1);

		// Token: 0x060003F3 RID: 1011
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Clear")]
		public static extern void CSVectorRect_Clear(HandleRef jarg1);

		// Token: 0x060003F4 RID: 1012
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Add")]
		public static extern void CSVectorRect_Add(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003F5 RID: 1013
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_size")]
		public static extern uint CSVectorRect_size(HandleRef jarg1);

		// Token: 0x060003F6 RID: 1014
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_capacity")]
		public static extern uint CSVectorRect_capacity(HandleRef jarg1);

		// Token: 0x060003F7 RID: 1015
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_reserve")]
		public static extern void CSVectorRect_reserve(HandleRef jarg1, uint jarg2);

		// Token: 0x060003F8 RID: 1016
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorRect__SWIG_0")]
		public static extern IntPtr new_CSVectorRect__SWIG_0();

		// Token: 0x060003F9 RID: 1017
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorRect__SWIG_1")]
		public static extern IntPtr new_CSVectorRect__SWIG_1(HandleRef jarg1);

		// Token: 0x060003FA RID: 1018
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSVectorRect__SWIG_2")]
		public static extern IntPtr new_CSVectorRect__SWIG_2(int jarg1);

		// Token: 0x060003FB RID: 1019
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_getitemcopy")]
		public static extern IntPtr CSVectorRect_getitemcopy(HandleRef jarg1, int jarg2);

		// Token: 0x060003FC RID: 1020
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_getitem")]
		public static extern IntPtr CSVectorRect_getitem(HandleRef jarg1, int jarg2);

		// Token: 0x060003FD RID: 1021
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_setitem")]
		public static extern void CSVectorRect_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x060003FE RID: 1022
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_AddRange")]
		public static extern void CSVectorRect_AddRange(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060003FF RID: 1023
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_GetRange")]
		public static extern IntPtr CSVectorRect_GetRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000400 RID: 1024
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Insert")]
		public static extern void CSVectorRect_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000401 RID: 1025
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_InsertRange")]
		public static extern void CSVectorRect_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000402 RID: 1026
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_RemoveAt")]
		public static extern void CSVectorRect_RemoveAt(HandleRef jarg1, int jarg2);

		// Token: 0x06000403 RID: 1027
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_RemoveRange")]
		public static extern void CSVectorRect_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000404 RID: 1028
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Repeat")]
		public static extern IntPtr CSVectorRect_Repeat(HandleRef jarg1, int jarg2);

		// Token: 0x06000405 RID: 1029
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Reverse__SWIG_0")]
		public static extern void CSVectorRect_Reverse__SWIG_0(HandleRef jarg1);

		// Token: 0x06000406 RID: 1030
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_Reverse__SWIG_1")]
		public static extern void CSVectorRect_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

		// Token: 0x06000407 RID: 1031
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVectorRect_SetRange")]
		public static extern void CSVectorRect_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000408 RID: 1032
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSVectorRect")]
		public static extern void delete_CSVectorRect(HandleRef jarg1);

		// Token: 0x06000409 RID: 1033
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScale__SWIG_0")]
		public static extern IntPtr new_CSScale__SWIG_0();

		// Token: 0x0600040A RID: 1034
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSScale")]
		public static extern void delete_CSScale(HandleRef jarg1);

		// Token: 0x0600040B RID: 1035
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScale__SWIG_1")]
		public static extern IntPtr new_CSScale__SWIG_1(HandleRef jarg1);

		// Token: 0x0600040C RID: 1036
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScale__SWIG_2")]
		public static extern IntPtr new_CSScale__SWIG_2(float jarg1, float jarg2);

		// Token: 0x0600040D RID: 1037
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScale_SetScale")]
		public static extern void CSScale_SetScale(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x0600040E RID: 1038
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScale_SetScaleX")]
		public static extern void CSScale_SetScaleX(HandleRef jarg1, float jarg2);

		// Token: 0x0600040F RID: 1039
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScale_GetScaleX")]
		public static extern float CSScale_GetScaleX(HandleRef jarg1);

		// Token: 0x06000410 RID: 1040
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScale_SetScaleY")]
		public static extern void CSScale_SetScaleY(HandleRef jarg1, float jarg2);

		// Token: 0x06000411 RID: 1041
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScale_GetScaleY")]
		public static extern float CSScale_GetScaleY(HandleRef jarg1);

		// Token: 0x06000412 RID: 1042
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScriptFileData__SWIG_0")]
		public static extern IntPtr new_CSScriptFileData__SWIG_0();

		// Token: 0x06000413 RID: 1043
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScriptFileData__SWIG_1")]
		public static extern IntPtr new_CSScriptFileData__SWIG_1(string jarg1, int jarg2);

		// Token: 0x06000414 RID: 1044
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScriptFileData__SWIG_2")]
		public static extern IntPtr new_CSScriptFileData__SWIG_2(HandleRef jarg1);

		// Token: 0x06000415 RID: 1045
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSScriptFileData")]
		public static extern void delete_CSScriptFileData(HandleRef jarg1);

		// Token: 0x06000416 RID: 1046
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScriptFileData_GetScriptType")]
		public static extern int CSScriptFileData_GetScriptType(HandleRef jarg1);

		// Token: 0x06000417 RID: 1047
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScriptFileData_GetPath")]
		public static extern string CSScriptFileData_GetPath(HandleRef jarg1);

		// Token: 0x06000418 RID: 1048
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScriptFileData_EndsWith")]
		public static extern bool CSScriptFileData_EndsWith(HandleRef jarg1, string jarg2);

		// Token: 0x06000419 RID: 1049
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScriptFileData_Equals")]
		public static extern bool CSScriptFileData_Equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600041A RID: 1050
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScriptFileData_IsEmpty")]
		public static extern bool CSScriptFileData_IsEmpty(HandleRef jarg1);

		// Token: 0x0600041B RID: 1051
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSResourceData__SWIG_0")]
		public static extern IntPtr new_CSResourceData__SWIG_0();

		// Token: 0x0600041C RID: 1052
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSResourceData__SWIG_1")]
		public static extern IntPtr new_CSResourceData__SWIG_1([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x0600041D RID: 1053
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSResourceData__SWIG_2")]
		public static extern IntPtr new_CSResourceData__SWIG_2([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1, int jarg2);

		// Token: 0x0600041E RID: 1054
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSResourceData__SWIG_3")]
		public static extern IntPtr new_CSResourceData__SWIG_3([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1, int jarg2, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg3);

		// Token: 0x0600041F RID: 1055
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSResourceData__SWIG_4")]
		public static extern IntPtr new_CSResourceData__SWIG_4(HandleRef jarg1);

		// Token: 0x06000420 RID: 1056
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSResourceData")]
		public static extern void delete_CSResourceData(HandleRef jarg1);

		// Token: 0x06000421 RID: 1057
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_GetResourceType")]
		public static extern int CSResourceData_GetResourceType(HandleRef jarg1);

		// Token: 0x06000422 RID: 1058
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_GetPath")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSResourceData_GetPath(HandleRef jarg1);

		// Token: 0x06000423 RID: 1059
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_GetPathC")]
		public static extern string CSResourceData_GetPathC(HandleRef jarg1);

		// Token: 0x06000424 RID: 1060
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_GetPlistFile")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSResourceData_GetPlistFile(HandleRef jarg1);

		// Token: 0x06000425 RID: 1061
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_EndsWith")]
		public static extern bool CSResourceData_EndsWith(HandleRef jarg1, string jarg2);

		// Token: 0x06000426 RID: 1062
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_Equals")]
		public static extern bool CSResourceData_Equals(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000427 RID: 1063
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSResourceData_IsEmpty")]
		public static extern bool CSResourceData_IsEmpty(HandleRef jarg1);

		// Token: 0x06000428 RID: 1064
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSMatrix")]
		public static extern IntPtr new_CSMatrix();

		// Token: 0x06000429 RID: 1065
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_M11")]
		public static extern float CSMatrix_M11(HandleRef jarg1);

		// Token: 0x0600042A RID: 1066
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_M12")]
		public static extern float CSMatrix_M12(HandleRef jarg1);

		// Token: 0x0600042B RID: 1067
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_M21")]
		public static extern float CSMatrix_M21(HandleRef jarg1);

		// Token: 0x0600042C RID: 1068
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_M22")]
		public static extern float CSMatrix_M22(HandleRef jarg1);

		// Token: 0x0600042D RID: 1069
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_X")]
		public static extern float CSMatrix_X(HandleRef jarg1);

		// Token: 0x0600042E RID: 1070
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_Y")]
		public static extern float CSMatrix_Y(HandleRef jarg1);

		// Token: 0x0600042F RID: 1071
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetM11")]
		public static extern void CSMatrix_SetM11(HandleRef jarg1, float jarg2);

		// Token: 0x06000430 RID: 1072
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetM12")]
		public static extern void CSMatrix_SetM12(HandleRef jarg1, float jarg2);

		// Token: 0x06000431 RID: 1073
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetM21")]
		public static extern void CSMatrix_SetM21(HandleRef jarg1, float jarg2);

		// Token: 0x06000432 RID: 1074
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetM22")]
		public static extern void CSMatrix_SetM22(HandleRef jarg1, float jarg2);

		// Token: 0x06000433 RID: 1075
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetX")]
		public static extern void CSMatrix_SetX(HandleRef jarg1, float jarg2);

		// Token: 0x06000434 RID: 1076
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSMatrix_SetY")]
		public static extern void CSMatrix_SetY(HandleRef jarg1, float jarg2);

		// Token: 0x06000435 RID: 1077
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSMatrix")]
		public static extern void delete_CSMatrix(HandleRef jarg1);

		// Token: 0x06000436 RID: 1078
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSRect__SWIG_0")]
		public static extern IntPtr new_CSRect__SWIG_0(float jarg1, float jarg2, float jarg3, float jarg4);

		// Token: 0x06000437 RID: 1079
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSRect__SWIG_1")]
		public static extern IntPtr new_CSRect__SWIG_1();

		// Token: 0x06000438 RID: 1080
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRect_MinX")]
		public static extern float CSRect_MinX(HandleRef jarg1);

		// Token: 0x06000439 RID: 1081
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRect_MinY")]
		public static extern float CSRect_MinY(HandleRef jarg1);

		// Token: 0x0600043A RID: 1082
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRect_MaxX")]
		public static extern float CSRect_MaxX(HandleRef jarg1);

		// Token: 0x0600043B RID: 1083
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRect_MaxY")]
		public static extern float CSRect_MaxY(HandleRef jarg1);

		// Token: 0x0600043C RID: 1084
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSRect")]
		public static extern void delete_CSRect(HandleRef jarg1);

		// Token: 0x0600043D RID: 1085
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_init")]
		public static extern void MatrixNode_init(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600043E RID: 1086
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_print")]
		public static extern void MatrixNode_print(HandleRef jarg1);

		// Token: 0x0600043F RID: 1087
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_X")]
		public static extern float MatrixNode_X(HandleRef jarg1);

		// Token: 0x06000440 RID: 1088
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_Y")]
		public static extern float MatrixNode_Y(HandleRef jarg1);

		// Token: 0x06000441 RID: 1089
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_ScaleX")]
		public static extern float MatrixNode_ScaleX(HandleRef jarg1);

		// Token: 0x06000442 RID: 1090
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_ScaleY")]
		public static extern float MatrixNode_ScaleY(HandleRef jarg1);

		// Token: 0x06000443 RID: 1091
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_SkewX")]
		public static extern float MatrixNode_SkewX(HandleRef jarg1);

		// Token: 0x06000444 RID: 1092
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_SkewY")]
		public static extern float MatrixNode_SkewY(HandleRef jarg1);

		// Token: 0x06000445 RID: 1093
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_AnchorPointX")]
		public static extern float MatrixNode_AnchorPointX(HandleRef jarg1);

		// Token: 0x06000446 RID: 1094
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_MatrixNode_AnchorPointY")]
		public static extern float MatrixNode_AnchorPointY(HandleRef jarg1);

		// Token: 0x06000447 RID: 1095
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_MatrixNode")]
		public static extern IntPtr new_MatrixNode();

		// Token: 0x06000448 RID: 1096
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_MatrixNode")]
		public static extern void delete_MatrixNode(HandleRef jarg1);

		// Token: 0x06000449 RID: 1097
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetTag")]
		public static extern int CSVisualObject_GetTag(HandleRef jarg1);

		// Token: 0x0600044A RID: 1098
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetTag")]
		public static extern void CSVisualObject_SetTag(HandleRef jarg1, int jarg2);

		// Token: 0x0600044B RID: 1099
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetName")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSVisualObject_GetName(HandleRef jarg1);

		// Token: 0x0600044C RID: 1100
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetName")]
		public static extern void CSVisualObject_SetName(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x0600044D RID: 1101
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetVisible")]
		public static extern bool CSVisualObject_GetVisible(HandleRef jarg1);

		// Token: 0x0600044E RID: 1102
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetVisible")]
		public static extern void CSVisualObject_SetVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x0600044F RID: 1103
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetPosition")]
		public static extern IntPtr CSVisualObject_GetPosition(HandleRef jarg1);

		// Token: 0x06000450 RID: 1104
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetPosition")]
		public static extern void CSVisualObject_SetPosition(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000451 RID: 1105
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetAnchorPoint")]
		public static extern IntPtr CSVisualObject_GetAnchorPoint(HandleRef jarg1);

		// Token: 0x06000452 RID: 1106
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetAnchorPointInPoints")]
		public static extern IntPtr CSVisualObject_GetAnchorPointInPoints(HandleRef jarg1);

		// Token: 0x06000453 RID: 1107
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetAnchorPoint")]
		public static extern void CSVisualObject_SetAnchorPoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000454 RID: 1108
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetScale")]
		public static extern IntPtr CSVisualObject_GetScale(HandleRef jarg1);

		// Token: 0x06000455 RID: 1109
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetScale")]
		public static extern void CSVisualObject_SetScale(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000456 RID: 1110
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetRotation")]
		public static extern float CSVisualObject_GetRotation(HandleRef jarg1);

		// Token: 0x06000457 RID: 1111
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetRotation")]
		public static extern void CSVisualObject_SetRotation(HandleRef jarg1, float jarg2);

		// Token: 0x06000458 RID: 1112
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetRotationSkewX")]
		public static extern float CSVisualObject_GetRotationSkewX(HandleRef jarg1);

		// Token: 0x06000459 RID: 1113
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetRotationSkewX")]
		public static extern void CSVisualObject_SetRotationSkewX(HandleRef jarg1, float jarg2);

		// Token: 0x0600045A RID: 1114
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetRotationSkewY")]
		public static extern float CSVisualObject_GetRotationSkewY(HandleRef jarg1);

		// Token: 0x0600045B RID: 1115
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetRotationSkewY")]
		public static extern void CSVisualObject_SetRotationSkewY(HandleRef jarg1, float jarg2);

		// Token: 0x0600045C RID: 1116
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetZOrder")]
		public static extern int CSVisualObject_GetZOrder(HandleRef jarg1);

		// Token: 0x0600045D RID: 1117
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetZOrder")]
		public static extern void CSVisualObject_SetZOrder(HandleRef jarg1, int jarg2);

		// Token: 0x0600045E RID: 1118
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetOrderOfArrival")]
		public static extern int CSVisualObject_GetOrderOfArrival(HandleRef jarg1);

		// Token: 0x0600045F RID: 1119
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetDisplayColor")]
		public static extern IntPtr CSVisualObject_GetDisplayColor(HandleRef jarg1);

		// Token: 0x06000460 RID: 1120
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetColor")]
		public static extern IntPtr CSVisualObject_GetColor(HandleRef jarg1);

		// Token: 0x06000461 RID: 1121
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetColor")]
		public static extern void CSVisualObject_SetColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000462 RID: 1122
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetAlpha")]
		public static extern int CSVisualObject_GetAlpha(HandleRef jarg1);

		// Token: 0x06000463 RID: 1123
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetAlpha")]
		public static extern void CSVisualObject_SetAlpha(HandleRef jarg1, int jarg2);

		// Token: 0x06000464 RID: 1124
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetCascadeColorEnabled")]
		public static extern bool CSVisualObject_GetCascadeColorEnabled(HandleRef jarg1);

		// Token: 0x06000465 RID: 1125
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetCascadeColorEnabled")]
		public static extern void CSVisualObject_SetCascadeColorEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000466 RID: 1126
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetCascadeOpacityEnabled")]
		public static extern bool CSVisualObject_GetCascadeOpacityEnabled(HandleRef jarg1);

		// Token: 0x06000467 RID: 1127
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetCascadeOpacityEnabled")]
		public static extern void CSVisualObject_SetCascadeOpacityEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000468 RID: 1128
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetBoundingRect")]
		public static extern IntPtr CSVisualObject_GetBoundingRect(HandleRef jarg1);

		// Token: 0x06000469 RID: 1129
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetSize")]
		public static extern IntPtr CSVisualObject_GetSize(HandleRef jarg1);

		// Token: 0x0600046A RID: 1130
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetSize")]
		public static extern void CSVisualObject_SetSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600046B RID: 1131
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetPosition3D")]
		public static extern void CSVisualObject_SetPosition3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600046C RID: 1132
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetPosition3D")]
		public static extern IntPtr CSVisualObject_GetPosition3D(HandleRef jarg1);

		// Token: 0x0600046D RID: 1133
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetWorldPosition")]
		public static extern IntPtr CSVisualObject_GetWorldPosition(HandleRef jarg1);

		// Token: 0x0600046E RID: 1134
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetRotation3D")]
		public static extern void CSVisualObject_SetRotation3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600046F RID: 1135
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetRotation3D")]
		public static extern IntPtr CSVisualObject_GetRotation3D(HandleRef jarg1);

		// Token: 0x06000470 RID: 1136
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetScale3D")]
		public static extern void CSVisualObject_SetScale3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000471 RID: 1137
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetScale3D")]
		public static extern IntPtr CSVisualObject_GetScale3D(HandleRef jarg1);

		// Token: 0x06000472 RID: 1138
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetOrientation")]
		public static extern void CSVisualObject_SetOrientation(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000473 RID: 1139
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetOrientation")]
		public static extern IntPtr CSVisualObject_GetOrientation(HandleRef jarg1);

		// Token: 0x06000474 RID: 1140
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetObjectState")]
		public static extern int CSVisualObject_GetObjectState(HandleRef jarg1);

		// Token: 0x06000475 RID: 1141
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SetObjectState")]
		public static extern void CSVisualObject_SetObjectState(HandleRef jarg1, int jarg2);

		// Token: 0x06000476 RID: 1142
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_AddChild")]
		public static extern void CSVisualObject_AddChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000477 RID: 1143
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_InsertChild")]
		public static extern void CSVisualObject_InsertChild(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000478 RID: 1144
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_RemoveChild")]
		public static extern void CSVisualObject_RemoveChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000479 RID: 1145
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_HitTest")]
		public static extern int CSVisualObject_HitTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047A RID: 1146
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_RectTest")]
		public static extern bool CSVisualObject_RectTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047B RID: 1147
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_HitTest3D")]
		public static extern float CSVisualObject_HitTest3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047C RID: 1148
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_RectTest3D")]
		public static extern bool CSVisualObject_RectTest3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047D RID: 1149
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_TransformToSelf")]
		public static extern IntPtr CSVisualObject_TransformToSelf(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047E RID: 1150
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_TransformToScene")]
		public static extern IntPtr CSVisualObject_TransformToScene(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600047F RID: 1151
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_TransformToParent")]
		public static extern IntPtr CSVisualObject_TransformToParent(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000480 RID: 1152
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetWorldMatrix")]
		public static extern IntPtr CSVisualObject_GetWorldMatrix(HandleRef jarg1);

		// Token: 0x06000481 RID: 1153
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetAnchorWorldMatrix")]
		public static extern IntPtr CSVisualObject_GetAnchorWorldMatrix(HandleRef jarg1);

		// Token: 0x06000482 RID: 1154
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_GetParentWorldMatrix")]
		public static extern IntPtr CSVisualObject_GetParentWorldMatrix(HandleRef jarg1);

		// Token: 0x06000483 RID: 1155
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_ConvertToNodeMatrix")]
		public static extern IntPtr CSVisualObject_ConvertToNodeMatrix(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000484 RID: 1156
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_ApplySelfWorldMatirx")]
		public static extern void CSVisualObject_ApplySelfWorldMatirx(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000485 RID: 1157
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSCanvas")]
		public static extern IntPtr new_CSCanvas();

		// Token: 0x06000486 RID: 1158
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSCanvas")]
		public static extern void delete_CSCanvas(HandleRef jarg1);

		// Token: 0x06000487 RID: 1159
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetVisible")]
		public static extern void CSCanvas_SetVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000488 RID: 1160
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetPosition")]
		public static extern void CSCanvas_SetPosition(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000489 RID: 1161
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetScale")]
		public static extern void CSCanvas_SetScale(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600048A RID: 1162
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_HitTest")]
		public static extern int CSCanvas_HitTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600048B RID: 1163
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_GetSize")]
		public static extern IntPtr CSCanvas_GetSize(HandleRef jarg1);

		// Token: 0x0600048C RID: 1164
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetSize")]
		public static extern void CSCanvas_SetSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600048D RID: 1165
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_ResetCanvas")]
		public static extern void CSCanvas_ResetCanvas(HandleRef jarg1);

		// Token: 0x0600048E RID: 1166
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetLayerColorVisible")]
		public static extern void CSCanvas_SetLayerColorVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x0600048F RID: 1167
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetCenterLineVisible")]
		public static extern void CSCanvas_SetCenterLineVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000490 RID: 1168
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetBackgroundVisible")]
		public static extern void CSCanvas_SetBackgroundVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000491 RID: 1169
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SetGridVisible")]
		public static extern void CSCanvas_SetGridVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000492 RID: 1170
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_RefreshGrid")]
		public static extern void CSCanvas_RefreshGrid(HandleRef jarg1, float jarg2);

		// Token: 0x06000493 RID: 1171
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSScene")]
		public static extern void delete_CSScene(HandleRef jarg1);

		// Token: 0x06000494 RID: 1172
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScene_ChangeMode")]
		public static extern void CSScene_ChangeMode(HandleRef jarg1, bool jarg2);

		// Token: 0x06000495 RID: 1173
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScene_GetCamera")]
		public static extern IntPtr CSScene_GetCamera(HandleRef jarg1);

		// Token: 0x06000496 RID: 1174
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScene_OnViewSizeChange")]
		public static extern void CSScene_OnViewSizeChange(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x06000497 RID: 1175
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSNode")]
		public static extern IntPtr new_CSNode();

		// Token: 0x06000498 RID: 1176
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSNode")]
		public static extern void delete_CSNode(HandleRef jarg1);

		// Token: 0x06000499 RID: 1177
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode_Init")]
		public static extern void CSNode_Init(HandleRef jarg1, bool jarg2);

		// Token: 0x0600049A RID: 1178
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode_SetScriptFile")]
		public static extern void CSNode_SetScriptFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600049B RID: 1179
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSNode2D")]
		public static extern IntPtr new_CSNode2D();

		// Token: 0x0600049C RID: 1180
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSNode2D")]
		public static extern void delete_CSNode2D(HandleRef jarg1);

		// Token: 0x0600049D RID: 1181
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_InitIcon__SWIG_0")]
		public static extern void CSNode2D_InitIcon__SWIG_0(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x0600049E RID: 1182
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_InitIcon__SWIG_1")]
		public static extern void CSNode2D_InitIcon__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600049F RID: 1183
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetIconVisible")]
		public static extern void CSNode2D_SetIconVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x060004A0 RID: 1184
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetIconVisible")]
		public static extern bool CSNode2D_GetIconVisible(HandleRef jarg1);

		// Token: 0x060004A1 RID: 1185
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_RefreshLayout")]
		public static extern void CSNode2D_RefreshLayout(HandleRef jarg1);

		// Token: 0x060004A2 RID: 1186
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_RefreshChildrenLayout")]
		public static extern void CSNode2D_RefreshChildrenLayout(HandleRef jarg1);

		// Token: 0x060004A3 RID: 1187
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetBoxAnchorPoint")]
		public static extern IntPtr CSNode2D_GetBoxAnchorPoint(HandleRef jarg1);

		// Token: 0x060004A4 RID: 1188
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetBoxSize")]
		public static extern IntPtr CSNode2D_GetBoxSize(HandleRef jarg1);

		// Token: 0x060004A5 RID: 1189
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetBoundingRect")]
		public static extern IntPtr CSNode2D_GetBoundingRect(HandleRef jarg1);

		// Token: 0x060004A6 RID: 1190
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetHorizontalEdge")]
		public static extern void CSNode2D_SetHorizontalEdge(HandleRef jarg1, int jarg2);

		// Token: 0x060004A7 RID: 1191
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetHorizontalEdge")]
		public static extern int CSNode2D_GetHorizontalEdge(HandleRef jarg1);

		// Token: 0x060004A8 RID: 1192
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetVerticalEdge")]
		public static extern void CSNode2D_SetVerticalEdge(HandleRef jarg1, int jarg2);

		// Token: 0x060004A9 RID: 1193
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetVerticalEdge")]
		public static extern int CSNode2D_GetVerticalEdge(HandleRef jarg1);

		// Token: 0x060004AA RID: 1194
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPositionPercentXEnabled")]
		public static extern void CSNode2D_SetPositionPercentXEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060004AB RID: 1195
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_IsUsingPositionPercentX")]
		public static extern bool CSNode2D_IsUsingPositionPercentX(HandleRef jarg1);

		// Token: 0x060004AC RID: 1196
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPositionPercentYEnabled")]
		public static extern void CSNode2D_SetPositionPercentYEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060004AD RID: 1197
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_IsUsingPositionPercentY")]
		public static extern bool CSNode2D_IsUsingPositionPercentY(HandleRef jarg1);

		// Token: 0x060004AE RID: 1198
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPositionPercentX")]
		public static extern void CSNode2D_SetPositionPercentX(HandleRef jarg1, float jarg2);

		// Token: 0x060004AF RID: 1199
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPositionPercentX")]
		public static extern float CSNode2D_GetPositionPercentX(HandleRef jarg1);

		// Token: 0x060004B0 RID: 1200
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPositionPercentY")]
		public static extern void CSNode2D_SetPositionPercentY(HandleRef jarg1, float jarg2);

		// Token: 0x060004B1 RID: 1201
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPositionPercentY")]
		public static extern float CSNode2D_GetPositionPercentY(HandleRef jarg1);

		// Token: 0x060004B2 RID: 1202
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetLeftMargin")]
		public static extern void CSNode2D_SetLeftMargin(HandleRef jarg1, float jarg2);

		// Token: 0x060004B3 RID: 1203
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetLeftMargin")]
		public static extern float CSNode2D_GetLeftMargin(HandleRef jarg1);

		// Token: 0x060004B4 RID: 1204
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetRightMargin")]
		public static extern void CSNode2D_SetRightMargin(HandleRef jarg1, float jarg2);

		// Token: 0x060004B5 RID: 1205
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetRightMargin")]
		public static extern float CSNode2D_GetRightMargin(HandleRef jarg1);

		// Token: 0x060004B6 RID: 1206
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetTopMargin")]
		public static extern void CSNode2D_SetTopMargin(HandleRef jarg1, float jarg2);

		// Token: 0x060004B7 RID: 1207
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetTopMargin")]
		public static extern float CSNode2D_GetTopMargin(HandleRef jarg1);

		// Token: 0x060004B8 RID: 1208
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetBottomMargin")]
		public static extern void CSNode2D_SetBottomMargin(HandleRef jarg1, float jarg2);

		// Token: 0x060004B9 RID: 1209
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetBottomMargin")]
		public static extern float CSNode2D_GetBottomMargin(HandleRef jarg1);

		// Token: 0x060004BA RID: 1210
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPercentWidthEnable")]
		public static extern void CSNode2D_SetPercentWidthEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x060004BB RID: 1211
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPercentWidthEnable")]
		public static extern bool CSNode2D_GetPercentWidthEnable(HandleRef jarg1);

		// Token: 0x060004BC RID: 1212
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPercentHeightEnable")]
		public static extern void CSNode2D_SetPercentHeightEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x060004BD RID: 1213
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPercentHeightEnable")]
		public static extern bool CSNode2D_GetPercentHeightEnable(HandleRef jarg1);

		// Token: 0x060004BE RID: 1214
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPercentWidth")]
		public static extern void CSNode2D_SetPercentWidth(HandleRef jarg1, float jarg2);

		// Token: 0x060004BF RID: 1215
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPercentWidth")]
		public static extern float CSNode2D_GetPercentWidth(HandleRef jarg1);

		// Token: 0x060004C0 RID: 1216
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPercentHeight")]
		public static extern void CSNode2D_SetPercentHeight(HandleRef jarg1, float jarg2);

		// Token: 0x060004C1 RID: 1217
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetPercentHeight")]
		public static extern float CSNode2D_GetPercentHeight(HandleRef jarg1);

		// Token: 0x060004C2 RID: 1218
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetSizeWidth")]
		public static extern void CSNode2D_SetSizeWidth(HandleRef jarg1, float jarg2);

		// Token: 0x060004C3 RID: 1219
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetSizeWidth")]
		public static extern float CSNode2D_GetSizeWidth(HandleRef jarg1);

		// Token: 0x060004C4 RID: 1220
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetSizeHeight")]
		public static extern void CSNode2D_SetSizeHeight(HandleRef jarg1, float jarg2);

		// Token: 0x060004C5 RID: 1221
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetSizeHeight")]
		public static extern float CSNode2D_GetSizeHeight(HandleRef jarg1);

		// Token: 0x060004C6 RID: 1222
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetStretchWidthEnable")]
		public static extern void CSNode2D_SetStretchWidthEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x060004C7 RID: 1223
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetStretchWidthEnable")]
		public static extern bool CSNode2D_GetStretchWidthEnable(HandleRef jarg1);

		// Token: 0x060004C8 RID: 1224
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetStretchHeightEnable")]
		public static extern void CSNode2D_SetStretchHeightEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x060004C9 RID: 1225
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetStretchHeightEnable")]
		public static extern bool CSNode2D_GetStretchHeightEnable(HandleRef jarg1);

		// Token: 0x060004CA RID: 1226
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetPosition")]
		public static extern void CSNode2D_SetPosition(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004CB RID: 1227
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetScale")]
		public static extern void CSNode2D_SetScale(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004CC RID: 1228
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetAnchorPoint")]
		public static extern void CSNode2D_SetAnchorPoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004CD RID: 1229
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetSize")]
		public static extern void CSNode2D_SetSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004CE RID: 1230
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_HitTest")]
		public static extern int CSNode2D_HitTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004CF RID: 1231
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_RectTest")]
		public static extern bool CSNode2D_RectTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004D0 RID: 1232
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetWorldMatrix")]
		public static extern IntPtr CSNode2D_GetWorldMatrix(HandleRef jarg1);

		// Token: 0x060004D1 RID: 1233
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_GetAnchorWorldMatrix")]
		public static extern IntPtr CSNode2D_GetAnchorWorldMatrix(HandleRef jarg1);

		// Token: 0x060004D2 RID: 1234
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SetObjectState")]
		public static extern void CSNode2D_SetObjectState(HandleRef jarg1, int jarg2);

		// Token: 0x060004D3 RID: 1235
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSNode3D")]
		public static extern IntPtr new_CSNode3D();

		// Token: 0x060004D4 RID: 1236
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSNode3D")]
		public static extern void delete_CSNode3D(HandleRef jarg1);

		// Token: 0x060004D5 RID: 1237
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_IsFlipped")]
		public static extern bool CSNode3D_IsFlipped(HandleRef jarg1);

		// Token: 0x060004D6 RID: 1238
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_RefreshObjectFace")]
		public static extern void CSNode3D_RefreshObjectFace(HandleRef jarg1);

		// Token: 0x060004D7 RID: 1239
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_SetPixelRenderMode")]
		public static extern void CSNode3D_SetPixelRenderMode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004D8 RID: 1240
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_RestoreRenderMode")]
		public static extern void CSNode3D_RestoreRenderMode(HandleRef jarg1);

		// Token: 0x060004D9 RID: 1241
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_SetNodeCameraMask")]
		public static extern void CSNode3D_SetNodeCameraMask(HandleRef jarg1, uint jarg2, bool jarg3);

		// Token: 0x060004DA RID: 1242
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_GetNodeCameraMask")]
		public static extern uint CSNode3D_GetNodeCameraMask(HandleRef jarg1);

		// Token: 0x060004DB RID: 1243
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Roll__SWIG_0")]
		public static extern void CSNode3D_Roll__SWIG_0(HandleRef jarg1, float jarg2, int jarg3);

		// Token: 0x060004DC RID: 1244
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Roll__SWIG_1")]
		public static extern void CSNode3D_Roll__SWIG_1(HandleRef jarg1, float jarg2);

		// Token: 0x060004DD RID: 1245
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Pitch__SWIG_0")]
		public static extern void CSNode3D_Pitch__SWIG_0(HandleRef jarg1, float jarg2, int jarg3);

		// Token: 0x060004DE RID: 1246
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Pitch__SWIG_1")]
		public static extern void CSNode3D_Pitch__SWIG_1(HandleRef jarg1, float jarg2);

		// Token: 0x060004DF RID: 1247
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Yaw__SWIG_0")]
		public static extern void CSNode3D_Yaw__SWIG_0(HandleRef jarg1, float jarg2, int jarg3);

		// Token: 0x060004E0 RID: 1248
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Yaw__SWIG_1")]
		public static extern void CSNode3D_Yaw__SWIG_1(HandleRef jarg1, float jarg2);

		// Token: 0x060004E1 RID: 1249
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Rotate3D__SWIG_0")]
		public static extern void CSNode3D_Rotate3D__SWIG_0(HandleRef jarg1, HandleRef jarg2, float jarg3, int jarg4);

		// Token: 0x060004E2 RID: 1250
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Rotate3D__SWIG_1")]
		public static extern void CSNode3D_Rotate3D__SWIG_1(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x060004E3 RID: 1251
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Rotate3D__SWIG_2")]
		public static extern void CSNode3D_Rotate3D__SWIG_2(HandleRef jarg1, HandleRef jarg2, int jarg3);

		// Token: 0x060004E4 RID: 1252
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Rotate3D__SWIG_3")]
		public static extern void CSNode3D_Rotate3D__SWIG_3(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004E5 RID: 1253
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_Rotate3D__SWIG_4")]
		public static extern void CSNode3D_Rotate3D__SWIG_4(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x060004E6 RID: 1254
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_SetObjectState")]
		public static extern void CSNode3D_SetObjectState(HandleRef jarg1, int jarg2);

		// Token: 0x060004E7 RID: 1255
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_HitTest3D")]
		public static extern float CSNode3D_HitTest3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004E8 RID: 1256
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_RectTest3D")]
		public static extern bool CSNode3D_RectTest3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060004E9 RID: 1257
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSLayer")]
		public static extern IntPtr new_CSLayer();

		// Token: 0x060004EA RID: 1258
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSLayer")]
		public static extern void delete_CSLayer(HandleRef jarg1);

		// Token: 0x060004EB RID: 1259
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLayer_IsTouchEnabled")]
		public static extern bool CSLayer_IsTouchEnabled(HandleRef jarg1);

		// Token: 0x060004EC RID: 1260
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLayer_SetTouchEnabled")]
		public static extern void CSLayer_SetTouchEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060004ED RID: 1261
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSWindow")]
		public static extern void delete_CSWindow(HandleRef jarg1);

		// Token: 0x060004EE RID: 1262
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSWindow__SWIG_0")]
		public static extern IntPtr new_CSWindow__SWIG_0(int jarg1, int jarg2, int jarg3, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg4, int jarg5);

		// Token: 0x060004EF RID: 1263
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSWindow__SWIG_1")]
		public static extern IntPtr new_CSWindow__SWIG_1(IntPtr jarg1, IntPtr jarg2, int jarg3, int jarg4, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg5, int jarg6);

		// Token: 0x060004F0 RID: 1264
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_Draw")]
		public static extern void CSWindow_Draw(HandleRef jarg1, int jarg2);

		// Token: 0x060004F1 RID: 1265
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_SetViewRect")]
		public static extern void CSWindow_SetViewRect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x060004F2 RID: 1266
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_Close")]
		public static extern void CSWindow_Close(HandleRef jarg1);

		// Token: 0x060004F3 RID: 1267
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_GetCanvas")]
		public static extern IntPtr CSWindow_GetCanvas(HandleRef jarg1);

		// Token: 0x060004F4 RID: 1268
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_GetScene")]
		public static extern IntPtr CSWindow_GetScene(HandleRef jarg1);

		// Token: 0x060004F5 RID: 1269
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_UpdateOpenGLContext")]
		public static extern void CSWindow_UpdateOpenGLContext(HandleRef jarg1, bool jarg2, int jarg3);

		// Token: 0x060004F6 RID: 1270
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWindow_SetSceneMode")]
		public static extern void CSWindow_SetSceneMode(HandleRef jarg1, bool jarg2);

		// Token: 0x060004F7 RID: 1271
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSCocosHelp")]
		public static extern IntPtr new_CSCocosHelp();

		// Token: 0x060004F8 RID: 1272
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSCocosHelp")]
		public static extern void delete_CSCocosHelp(HandleRef jarg1);

		// Token: 0x060004F9 RID: 1273
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ConvertPath")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSCocosHelp_ConvertPath([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x060004FA RID: 1274
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ClearResurceCache")]
		public static extern void CSCocosHelp_ClearResurceCache();

		// Token: 0x060004FB RID: 1275
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_LoadPListFileToCache")]
		public static extern void CSCocosHelp_LoadPListFileToCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x060004FC RID: 1276
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_IsPngLoadedFromCache")]
		public static extern bool CSCocosHelp_IsPngLoadedFromCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x060004FD RID: 1277
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_RenamePngFileToCache")]
		public static extern void CSCocosHelp_RenamePngFileToCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x060004FE RID: 1278
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ReloadPngFileToCache")]
		public static extern void CSCocosHelp_ReloadPngFileToCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x060004FF RID: 1279
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ReloadPlistFileToCache")]
		public static extern void CSCocosHelp_ReloadPlistFileToCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000500 RID: 1280
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ReloadFntFileToCache")]
		public static extern void CSCocosHelp_ReloadFntFileToCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000501 RID: 1281
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_UnloadTTFFileFromCache")]
		public static extern void CSCocosHelp_UnloadTTFFileFromCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000502 RID: 1282
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_RemovePngFileFromCache")]
		public static extern void CSCocosHelp_RemovePngFileFromCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000503 RID: 1283
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_RemovePlistFileFromCache")]
		public static extern void CSCocosHelp_RemovePlistFileFromCache([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000504 RID: 1284
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetTmxMapImageArray")]
		public static extern IntPtr CSCocosHelp_GetTmxMapImageArray([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000505 RID: 1285
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetMeshResourceArray")]
		public static extern IntPtr CSCocosHelp_GetMeshResourceArray([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000506 RID: 1286
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetSprite3DTextureArray")]
		public static extern IntPtr CSCocosHelp_GetSprite3DTextureArray([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000507 RID: 1287
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ConvertToBinProto")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSCocosHelp_ConvertToBinProto(string jarg1, string jarg2, string jarg3);

		// Token: 0x06000508 RID: 1288
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_ConvertToBinByFlat")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSCocosHelp_ConvertToBinByFlat(string jarg1, string jarg2, string jarg3);

		// Token: 0x06000509 RID: 1289
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_RefreshLayoutSystemState")]
		public static extern void CSCocosHelp_RefreshLayoutSystemState(bool jarg1);

		// Token: 0x0600050A RID: 1290
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_SetResourcePath")]
		public static extern void CSCocosHelp_SetResourcePath([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x0600050B RID: 1291
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_AddSearchPath")]
		public static extern void CSCocosHelp_AddSearchPath([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x0600050C RID: 1292
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_CheckOpenGLVersion")]
		public static extern bool CSCocosHelp_CheckOpenGLVersion();

		// Token: 0x0600050D RID: 1293
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_CheckBMFontResource")]
		public static extern bool CSCocosHelp_CheckBMFontResource([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x0600050E RID: 1294
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_CheckImageFormat")]
		public static extern bool CSCocosHelp_CheckImageFormat([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x0600050F RID: 1295
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_IsEngineInitialized")]
		public static extern bool CSCocosHelp_IsEngineInitialized();

		// Token: 0x06000510 RID: 1296
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_SetLocalPath")]
		public static extern void CSCocosHelp_SetLocalPath(string jarg1);

		// Token: 0x06000511 RID: 1297
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetNodeFromScript")]
		public static extern IntPtr CSCocosHelp_GetNodeFromScript(HandleRef jarg1);

		// Token: 0x06000512 RID: 1298
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetNodeFromJS")]
		public static extern IntPtr CSCocosHelp_GetNodeFromJS(string jarg1);

		// Token: 0x06000513 RID: 1299
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetNodeFromLua")]
		public static extern IntPtr CSCocosHelp_GetNodeFromLua(string jarg1);

		// Token: 0x06000514 RID: 1300
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_GetBaseTypeFromLua")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSCocosHelp_GetBaseTypeFromLua(string jarg1);

		// Token: 0x06000515 RID: 1301
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_IsUseDefaultRenderFromLua")]
		public static extern bool CSCocosHelp_IsUseDefaultRenderFromLua(string jarg1);

		// Token: 0x06000516 RID: 1302
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_StopAllEffects")]
		public static extern void CSCocosHelp_StopAllEffects();

		// Token: 0x06000517 RID: 1303
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_CheckSprite3DFile")]
		public static extern bool CSCocosHelp_CheckSprite3DFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000518 RID: 1304
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCocosHelp_CheckParticle3DFile")]
		public static extern bool CSCocosHelp_CheckParticle3DFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg1);

		// Token: 0x06000519 RID: 1305
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSkyBox")]
		public static extern IntPtr new_CSSkyBox();

		// Token: 0x0600051A RID: 1306
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSkyBox")]
		public static extern void delete_CSSkyBox(HandleRef jarg1);

		// Token: 0x0600051B RID: 1307
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkyBox_IsEnabled")]
		public static extern bool CSSkyBox_IsEnabled(HandleRef jarg1);

		// Token: 0x0600051C RID: 1308
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkyBox_SetEnabled")]
		public static extern void CSSkyBox_SetEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x0600051D RID: 1309
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkyBox_ResetSkyBox")]
		public static extern void CSSkyBox_ResetSkyBox(HandleRef jarg1);

		// Token: 0x0600051E RID: 1310
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkyBox_RefreshSkyBox")]
		public static extern void CSSkyBox_RefreshSkyBox(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, HandleRef jarg5, HandleRef jarg6, HandleRef jarg7);

		// Token: 0x0600051F RID: 1311
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSCamera")]
		public static extern IntPtr new_CSCamera();

		// Token: 0x06000520 RID: 1312
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSCamera")]
		public static extern void delete_CSCamera(HandleRef jarg1);

		// Token: 0x06000521 RID: 1313
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetCameraFlag")]
		public static extern void CSCamera_SetCameraFlag(HandleRef jarg1, ushort jarg2);

		// Token: 0x06000522 RID: 1314
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_LookAt")]
		public static extern void CSCamera_LookAt(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000523 RID: 1315
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetScreenSize")]
		public static extern IntPtr CSCamera_GetScreenSize(HandleRef jarg1);

		// Token: 0x06000524 RID: 1316
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetScreenSize")]
		public static extern void CSCamera_SetScreenSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000525 RID: 1317
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetNearPlane")]
		public static extern float CSCamera_GetNearPlane(HandleRef jarg1);

		// Token: 0x06000526 RID: 1318
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetNearPlane")]
		public static extern void CSCamera_SetNearPlane(HandleRef jarg1, float jarg2);

		// Token: 0x06000527 RID: 1319
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetFarPlane")]
		public static extern float CSCamera_GetFarPlane(HandleRef jarg1);

		// Token: 0x06000528 RID: 1320
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetFarPlane")]
		public static extern void CSCamera_SetFarPlane(HandleRef jarg1, float jarg2);

		// Token: 0x06000529 RID: 1321
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetFov")]
		public static extern float CSCamera_GetFov(HandleRef jarg1);

		// Token: 0x0600052A RID: 1322
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetFov")]
		public static extern void CSCamera_SetFov(HandleRef jarg1, float jarg2);

		// Token: 0x0600052B RID: 1323
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetUp")]
		public static extern IntPtr CSCamera_GetUp(HandleRef jarg1);

		// Token: 0x0600052C RID: 1324
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetRight")]
		public static extern IntPtr CSCamera_GetRight(HandleRef jarg1);

		// Token: 0x0600052D RID: 1325
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetLookAt")]
		public static extern IntPtr CSCamera_GetLookAt(HandleRef jarg1);

		// Token: 0x0600052E RID: 1326
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetProjection__SWIG_0")]
		public static extern void CSCamera_SetProjection__SWIG_0(HandleRef jarg1, int jarg2, bool jarg3);

		// Token: 0x0600052F RID: 1327
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetProjection__SWIG_1")]
		public static extern void CSCamera_SetProjection__SWIG_1(HandleRef jarg1, int jarg2);

		// Token: 0x06000530 RID: 1328
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_Unproject")]
		public static extern IntPtr CSCamera_Unproject(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000531 RID: 1329
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_ScreenToWorld__SWIG_0")]
		public static extern IntPtr CSCamera_ScreenToWorld__SWIG_0(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x06000532 RID: 1330
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_ScreenToWorld__SWIG_1")]
		public static extern IntPtr CSCamera_ScreenToWorld__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000533 RID: 1331
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_ScreenToWorld__SWIG_2")]
		public static extern IntPtr CSCamera_ScreenToWorld__SWIG_2(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x06000534 RID: 1332
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_UpdateFrustumPlane")]
		public static extern void CSCamera_UpdateFrustumPlane(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000535 RID: 1333
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SetSkyBox")]
		public static extern void CSCamera_SetSkyBox(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000536 RID: 1334
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_GetSkyBox")]
		public static extern IntPtr CSCamera_GetSkyBox(HandleRef jarg1);

		// Token: 0x06000537 RID: 1335
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetMode")]
		public static extern void CSSceneCamera_SetMode(HandleRef jarg1, int jarg2);

		// Token: 0x06000538 RID: 1336
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_MoveTo")]
		public static extern void CSSceneCamera_MoveTo(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000539 RID: 1337
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_StartMove")]
		public static extern void CSSceneCamera_StartMove(HandleRef jarg1);

		// Token: 0x0600053A RID: 1338
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_Paused")]
		public static extern void CSSceneCamera_Paused(HandleRef jarg1);

		// Token: 0x0600053B RID: 1339
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetMoveSpeed__SWIG_0")]
		public static extern void CSSceneCamera_SetMoveSpeed__SWIG_0(HandleRef jarg1, float jarg2, bool jarg3);

		// Token: 0x0600053C RID: 1340
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetMoveSpeed__SWIG_1")]
		public static extern void CSSceneCamera_SetMoveSpeed__SWIG_1(HandleRef jarg1, float jarg2);

		// Token: 0x0600053D RID: 1341
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_GetMoveSpeed")]
		public static extern float CSSceneCamera_GetMoveSpeed(HandleRef jarg1);

		// Token: 0x0600053E RID: 1342
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetMoveAccelerate")]
		public static extern void CSSceneCamera_SetMoveAccelerate(HandleRef jarg1, float jarg2);

		// Token: 0x0600053F RID: 1343
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_MoveRelative__SWIG_0")]
		public static extern void CSSceneCamera_MoveRelative__SWIG_0(HandleRef jarg1, HandleRef jarg2, int jarg3);

		// Token: 0x06000540 RID: 1344
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_MoveRelative__SWIG_1")]
		public static extern void CSSceneCamera_MoveRelative__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000541 RID: 1345
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_MoveDirect")]
		public static extern void CSSceneCamera_MoveDirect(HandleRef jarg1, float jarg2);

		// Token: 0x06000542 RID: 1346
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_AddMoveFlag")]
		public static extern void CSSceneCamera_AddMoveFlag(HandleRef jarg1, ushort jarg2, bool jarg3);

		// Token: 0x06000543 RID: 1347
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_RemoveFlag")]
		public static extern void CSSceneCamera_RemoveFlag(HandleRef jarg1, ushort jarg2);

		// Token: 0x06000544 RID: 1348
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_GetMoveFlag")]
		public static extern ushort CSSceneCamera_GetMoveFlag(HandleRef jarg1);

		// Token: 0x06000545 RID: 1349
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_ClearFlag")]
		public static extern void CSSceneCamera_ClearFlag(HandleRef jarg1);

		// Token: 0x06000546 RID: 1350
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_Update")]
		public static extern void CSSceneCamera_Update(HandleRef jarg1, float jarg2);

		// Token: 0x06000547 RID: 1351
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_OnViewSizeChange")]
		public static extern void CSSceneCamera_OnViewSizeChange(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x06000548 RID: 1352
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_Rotate3D__SWIG_0")]
		public static extern void CSSceneCamera_Rotate3D__SWIG_0(HandleRef jarg1, HandleRef jarg2, float jarg3, int jarg4);

		// Token: 0x06000549 RID: 1353
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_Rotate3D__SWIG_1")]
		public static extern void CSSceneCamera_Rotate3D__SWIG_1(HandleRef jarg1, HandleRef jarg2, float jarg3);

		// Token: 0x0600054A RID: 1354
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetPosition3D")]
		public static extern void CSSceneCamera_SetPosition3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600054B RID: 1355
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetRotation3D")]
		public static extern void CSSceneCamera_SetRotation3D(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600054C RID: 1356
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_GetUserCameraPreview")]
		public static extern IntPtr CSSceneCamera_GetUserCameraPreview(HandleRef jarg1);

		// Token: 0x0600054D RID: 1357
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_GetPickColor")]
		public static extern IntPtr CSSceneCamera_GetPickColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600054E RID: 1358
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_OnMove")]
		public static extern void CSSceneCamera_OnMove(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x0600054F RID: 1359
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_OnChangeViewpoint")]
		public static extern void CSSceneCamera_OnChangeViewpoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000550 RID: 1360
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_RefreshLookAt")]
		public static extern void CSSceneCamera_RefreshLookAt(HandleRef jarg1);

		// Token: 0x06000551 RID: 1361
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SetRectDrawNode")]
		public static extern void CSSceneCamera_SetRectDrawNode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000552 RID: 1362
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_CameraListener_OnSizeChange")]
		public static extern void CSUserCamera_CameraListener_OnSizeChange(HandleRef jarg1);

		// Token: 0x06000553 RID: 1363
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSUserCamera_CameraListener")]
		public static extern void delete_CSUserCamera_CameraListener(HandleRef jarg1);

		// Token: 0x06000554 RID: 1364
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSUserCamera")]
		public static extern IntPtr new_CSUserCamera();

		// Token: 0x06000555 RID: 1365
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSUserCamera")]
		public static extern void delete_CSUserCamera(HandleRef jarg1);

		// Token: 0x06000556 RID: 1366
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_GetCameraMask")]
		public static extern uint CSUserCamera_GetCameraMask(HandleRef jarg1);

		// Token: 0x06000557 RID: 1367
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SetCameraMask")]
		public static extern void CSUserCamera_SetCameraMask(HandleRef jarg1, uint jarg2);

		// Token: 0x06000558 RID: 1368
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SetObjectState")]
		public static extern void CSUserCamera_SetObjectState(HandleRef jarg1, int jarg2);

		// Token: 0x06000559 RID: 1369
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SetProjection__SWIG_0")]
		public static extern void CSUserCamera_SetProjection__SWIG_0(HandleRef jarg1, int jarg2, bool jarg3);

		// Token: 0x0600055A RID: 1370
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SetProjection__SWIG_1")]
		public static extern void CSUserCamera_SetProjection__SWIG_1(HandleRef jarg1, int jarg2);

		// Token: 0x0600055B RID: 1371
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_ResetFrustum")]
		public static extern void CSUserCamera_ResetFrustum(HandleRef jarg1);

		// Token: 0x0600055C RID: 1372
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_GetCSCamera")]
		public static extern IntPtr CSUserCamera_GetCSCamera(HandleRef jarg1);

		// Token: 0x0600055D RID: 1373
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SetPixelRenderMode")]
		public static extern void CSUserCamera_SetPixelRenderMode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600055E RID: 1374
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_RestoreRenderMode")]
		public static extern void CSUserCamera_RestoreRenderMode(HandleRef jarg1);

		// Token: 0x0600055F RID: 1375
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_GetDisplayColor")]
		public static extern IntPtr CSUserCamera_GetDisplayColor(HandleRef jarg1);

		// Token: 0x06000560 RID: 1376
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_GetCSSkyBox")]
		public static extern IntPtr CSUserCamera_GetCSSkyBox(HandleRef jarg1);

		// Token: 0x06000561 RID: 1377
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSDumyNode")]
		public static extern IntPtr new_CSDumyNode();

		// Token: 0x06000562 RID: 1378
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSDumyNode")]
		public static extern void delete_CSDumyNode(HandleRef jarg1);

		// Token: 0x06000563 RID: 1379
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDumyNode_EffectResult")]
		public static extern void CSDumyNode_EffectResult(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000564 RID: 1380
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDumyNode_EffectToTarget")]
		public static extern void CSDumyNode_EffectToTarget(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, bool jarg4, bool jarg5);

		// Token: 0x06000565 RID: 1381
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSprite3D")]
		public static extern IntPtr new_CSSprite3D();

		// Token: 0x06000566 RID: 1382
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSprite3D")]
		public static extern void delete_CSSprite3D(HandleRef jarg1);

		// Token: 0x06000567 RID: 1383
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_SetFileData")]
		public static extern void CSSprite3D_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000568 RID: 1384
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_SetLightMask")]
		public static extern void CSSprite3D_SetLightMask(HandleRef jarg1, int jarg2);

		// Token: 0x06000569 RID: 1385
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_GetLightMask")]
		public static extern int CSSprite3D_GetLightMask(HandleRef jarg1);

		// Token: 0x0600056A RID: 1386
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_LoadAnimation")]
		public static extern bool CSSprite3D_LoadAnimation(HandleRef jarg1);

		// Token: 0x0600056B RID: 1387
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_RunAction")]
		public static extern void CSSprite3D_RunAction(HandleRef jarg1);

		// Token: 0x0600056C RID: 1388
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_StopAction")]
		public static extern void CSSprite3D_StopAction(HandleRef jarg1);

		// Token: 0x0600056D RID: 1389
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_RunAnimationIfPosible")]
		public static extern void CSSprite3D_RunAnimationIfPosible(HandleRef jarg1);

		// Token: 0x0600056E RID: 1390
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_SetBlendFunc")]
		public static extern void CSSprite3D_SetBlendFunc(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600056F RID: 1391
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_GetBlendFunc")]
		public static extern IntPtr CSSprite3D_GetBlendFunc(HandleRef jarg1);

		// Token: 0x06000570 RID: 1392
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_RestoreRenderMode")]
		public static extern void CSSprite3D_RestoreRenderMode(HandleRef jarg1);

		// Token: 0x06000571 RID: 1393
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSParticle3D")]
		public static extern IntPtr new_CSParticle3D();

		// Token: 0x06000572 RID: 1394
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSParticle3D")]
		public static extern void delete_CSParticle3D(HandleRef jarg1);

		// Token: 0x06000573 RID: 1395
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_InitParticle3DSystem")]
		public static extern void CSParticle3D_InitParticle3DSystem(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x06000574 RID: 1396
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_StartParticleIfPossible")]
		public static extern void CSParticle3D_StartParticleIfPossible(HandleRef jarg1);

		// Token: 0x06000575 RID: 1397
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_SetPixelRenderMode")]
		public static extern void CSParticle3D_SetPixelRenderMode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000576 RID: 1398
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_RestoreRenderMode")]
		public static extern void CSParticle3D_RestoreRenderMode(HandleRef jarg1);

		// Token: 0x06000577 RID: 1399
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_GetDisplayColor")]
		public static extern IntPtr CSParticle3D_GetDisplayColor(HandleRef jarg1);

		// Token: 0x06000578 RID: 1400
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_startParticle")]
		public static extern void CSParticle3D_startParticle(HandleRef jarg1);

		// Token: 0x06000579 RID: 1401
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_stopParticle")]
		public static extern void CSParticle3D_stopParticle(HandleRef jarg1);

		// Token: 0x0600057A RID: 1402
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_isWorldPosition")]
		public static extern bool CSParticle3D_isWorldPosition(HandleRef jarg1);

		// Token: 0x0600057B RID: 1403
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSUserCameraPreview")]
		public static extern IntPtr new_CSUserCameraPreview();

		// Token: 0x0600057C RID: 1404
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSUserCameraPreview")]
		public static extern void delete_CSUserCameraPreview(HandleRef jarg1);

		// Token: 0x0600057D RID: 1405
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCameraPreview_IsKeep")]
		public static extern bool CSUserCameraPreview_IsKeep(HandleRef jarg1);

		// Token: 0x0600057E RID: 1406
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCameraPreview_SetCamera")]
		public static extern void CSUserCameraPreview_SetCamera(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600057F RID: 1407
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCameraPreview_Render")]
		public static extern void CSUserCameraPreview_Render(HandleRef jarg1, bool jarg2);

		// Token: 0x06000580 RID: 1408
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCameraPreview_OnPreviewChanged")]
		public static extern void CSUserCameraPreview_OnPreviewChanged(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000581 RID: 1409
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCameraPreview_SetVisible")]
		public static extern void CSUserCameraPreview_SetVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000582 RID: 1410
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSlice3D")]
		public static extern IntPtr new_CSSlice3D();

		// Token: 0x06000583 RID: 1411
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSlice3D")]
		public static extern void delete_CSSlice3D(HandleRef jarg1);

		// Token: 0x06000584 RID: 1412
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetFileData")]
		public static extern void CSSlice3D_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000585 RID: 1413
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetSliceSize")]
		public static extern IntPtr CSSlice3D_GetSliceSize(HandleRef jarg1);

		// Token: 0x06000586 RID: 1414
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetSliceSize")]
		public static extern void CSSlice3D_SetSliceSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000587 RID: 1415
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetFlipX")]
		public static extern bool CSSlice3D_GetFlipX(HandleRef jarg1);

		// Token: 0x06000588 RID: 1416
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetFlipX")]
		public static extern void CSSlice3D_SetFlipX(HandleRef jarg1, bool jarg2);

		// Token: 0x06000589 RID: 1417
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetFlipY")]
		public static extern bool CSSlice3D_GetFlipY(HandleRef jarg1);

		// Token: 0x0600058A RID: 1418
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetFlipY")]
		public static extern void CSSlice3D_SetFlipY(HandleRef jarg1, bool jarg2);

		// Token: 0x0600058B RID: 1419
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_getUVActive")]
		public static extern bool CSSlice3D_getUVActive(HandleRef jarg1);

		// Token: 0x0600058C RID: 1420
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_setUVActive")]
		public static extern void CSSlice3D_setUVActive(HandleRef jarg1, bool jarg2);

		// Token: 0x0600058D RID: 1421
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetAnimationSpeed")]
		public static extern void CSSlice3D_SetAnimationSpeed(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600058E RID: 1422
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetAnimationSpeed")]
		public static extern IntPtr CSSlice3D_GetAnimationSpeed(HandleRef jarg1);

		// Token: 0x0600058F RID: 1423
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetRowAndColumn")]
		public static extern IntPtr CSSlice3D_GetRowAndColumn(HandleRef jarg1);

		// Token: 0x06000590 RID: 1424
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetRowAndColumn")]
		public static extern void CSSlice3D_SetRowAndColumn(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000591 RID: 1425
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetTextureActive")]
		public static extern bool CSSlice3D_GetTextureActive(HandleRef jarg1);

		// Token: 0x06000592 RID: 1426
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetTextureActive")]
		public static extern void CSSlice3D_SetTextureActive(HandleRef jarg1, bool jarg2);

		// Token: 0x06000593 RID: 1427
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetFramerate")]
		public static extern void CSSlice3D_SetFramerate(HandleRef jarg1, float jarg2);

		// Token: 0x06000594 RID: 1428
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_GetFramerate")]
		public static extern float CSSlice3D_GetFramerate(HandleRef jarg1);

		// Token: 0x06000595 RID: 1429
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SetBillBoardMode")]
		public static extern void CSSlice3D_SetBillBoardMode(HandleRef jarg1, int jarg2);

		// Token: 0x06000596 RID: 1430
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_getBillBoardMode")]
		public static extern int CSSlice3D_getBillBoardMode(HandleRef jarg1);

		// Token: 0x06000597 RID: 1431
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSLight")]
		public static extern IntPtr new_CSLight();

		// Token: 0x06000598 RID: 1432
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSLight")]
		public static extern void delete_CSLight(HandleRef jarg1);

		// Token: 0x06000599 RID: 1433
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetLightType")]
		public static extern void CSLight_SetLightType(HandleRef jarg1, int jarg2);

		// Token: 0x0600059A RID: 1434
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetLightType")]
		public static extern int CSLight_GetLightType(HandleRef jarg1);

		// Token: 0x0600059B RID: 1435
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetLightFlag")]
		public static extern void CSLight_SetLightFlag(HandleRef jarg1, int jarg2);

		// Token: 0x0600059C RID: 1436
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetLightFlag")]
		public static extern int CSLight_GetLightFlag(HandleRef jarg1);

		// Token: 0x0600059D RID: 1437
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetIntensity")]
		public static extern void CSLight_SetIntensity(HandleRef jarg1, float jarg2);

		// Token: 0x0600059E RID: 1438
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetIntensity")]
		public static extern float CSLight_GetIntensity(HandleRef jarg1);

		// Token: 0x0600059F RID: 1439
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetEnabled")]
		public static extern void CSLight_SetEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060005A0 RID: 1440
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_IsEnabled")]
		public static extern bool CSLight_IsEnabled(HandleRef jarg1);

		// Token: 0x060005A1 RID: 1441
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetRange")]
		public static extern void CSLight_SetRange(HandleRef jarg1, float jarg2);

		// Token: 0x060005A2 RID: 1442
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetRange")]
		public static extern float CSLight_GetRange(HandleRef jarg1);

		// Token: 0x060005A3 RID: 1443
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetInnerAngle")]
		public static extern void CSLight_SetInnerAngle(HandleRef jarg1, float jarg2);

		// Token: 0x060005A4 RID: 1444
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetInnerAngle")]
		public static extern float CSLight_GetInnerAngle(HandleRef jarg1);

		// Token: 0x060005A5 RID: 1445
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetOuterAngle")]
		public static extern void CSLight_SetOuterAngle(HandleRef jarg1, float jarg2);

		// Token: 0x060005A6 RID: 1446
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetOuterAngle")]
		public static extern float CSLight_GetOuterAngle(HandleRef jarg1);

		// Token: 0x060005A7 RID: 1447
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_OnMouseMove")]
		public static extern bool CSLight_OnMouseMove(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005A8 RID: 1448
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_OnMouseDown")]
		public static extern bool CSLight_OnMouseDown(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005A9 RID: 1449
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_OnMouseUp")]
		public static extern bool CSLight_OnMouseUp(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005AA RID: 1450
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_HitControlPoint")]
		public static extern bool CSLight_HitControlPoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005AB RID: 1451
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_RefreshLightState")]
		public static extern void CSLight_RefreshLightState(HandleRef jarg1, bool jarg2);

		// Token: 0x060005AC RID: 1452
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_RefreshLightIndex")]
		public static extern void CSLight_RefreshLightIndex(HandleRef jarg1, int jarg2);

		// Token: 0x060005AD RID: 1453
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_GetDisplayColor")]
		public static extern IntPtr CSLight_GetDisplayColor(HandleRef jarg1);

		// Token: 0x060005AE RID: 1454
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_RestoreRenderMode")]
		public static extern void CSLight_RestoreRenderMode(HandleRef jarg1);

		// Token: 0x060005AF RID: 1455
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SetPixelRenderMode")]
		public static extern void CSLight_SetPixelRenderMode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005B0 RID: 1456
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_AnimationAction")]
		public static extern void delete_AnimationAction(HandleRef jarg1);

		// Token: 0x060005B1 RID: 1457
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_step")]
		public static extern void AnimationAction_step(HandleRef jarg1, float jarg2);

		// Token: 0x060005B2 RID: 1458
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_isDone")]
		public static extern bool AnimationAction_isDone(HandleRef jarg1);

		// Token: 0x060005B3 RID: 1459
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_setUVSpeed")]
		public static extern void AnimationAction_setUVSpeed(HandleRef jarg1, float jarg2, float jarg3);

		// Token: 0x060005B4 RID: 1460
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_getUVSpeed")]
		public static extern IntPtr AnimationAction_getUVSpeed(HandleRef jarg1);

		// Token: 0x060005B5 RID: 1461
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_setActive")]
		public static extern void AnimationAction_setActive(HandleRef jarg1, bool jarg2);

		// Token: 0x060005B6 RID: 1462
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_AnimationAction_getActive")]
		public static extern bool AnimationAction_getActive(HandleRef jarg1);

		// Token: 0x060005B7 RID: 1463
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSprite")]
		public static extern IntPtr new_CSSprite();

		// Token: 0x060005B8 RID: 1464
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSprite")]
		public static extern void delete_CSSprite(HandleRef jarg1);

		// Token: 0x060005B9 RID: 1465
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_GetFlipX")]
		public static extern bool CSSprite_GetFlipX(HandleRef jarg1);

		// Token: 0x060005BA RID: 1466
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_SetFlipX")]
		public static extern void CSSprite_SetFlipX(HandleRef jarg1, bool jarg2);

		// Token: 0x060005BB RID: 1467
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_GetFlipY")]
		public static extern bool CSSprite_GetFlipY(HandleRef jarg1);

		// Token: 0x060005BC RID: 1468
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_SetFlipY")]
		public static extern void CSSprite_SetFlipY(HandleRef jarg1, bool jarg2);

		// Token: 0x060005BD RID: 1469
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_GetFileData")]
		public static extern IntPtr CSSprite_GetFileData(HandleRef jarg1);

		// Token: 0x060005BE RID: 1470
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_SetFileData")]
		public static extern void CSSprite_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005BF RID: 1471
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_SetBlendFunc")]
		public static extern void CSSprite_SetBlendFunc(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005C0 RID: 1472
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_GetBlendFunc")]
		public static extern IntPtr CSSprite_GetBlendFunc(HandleRef jarg1);

		// Token: 0x060005C1 RID: 1473
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGameMap_TMXLayerTag_get")]
		public static extern int CSGameMap_TMXLayerTag_get();

		// Token: 0x060005C2 RID: 1474
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSGameMap")]
		public static extern IntPtr new_CSGameMap();

		// Token: 0x060005C3 RID: 1475
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSGameMap")]
		public static extern void delete_CSGameMap(HandleRef jarg1);

		// Token: 0x060005C4 RID: 1476
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGameMap_GetFileData")]
		public static extern IntPtr CSGameMap_GetFileData(HandleRef jarg1);

		// Token: 0x060005C5 RID: 1477
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGameMap_SetFileData")]
		public static extern void CSGameMap_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005C6 RID: 1478
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSParticleSystem")]
		public static extern IntPtr new_CSParticleSystem();

		// Token: 0x060005C7 RID: 1479
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSParticleSystem")]
		public static extern void delete_CSParticleSystem(HandleRef jarg1);

		// Token: 0x060005C8 RID: 1480
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_Start")]
		public static extern void CSParticleSystem_Start(HandleRef jarg1);

		// Token: 0x060005C9 RID: 1481
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_Stop")]
		public static extern void CSParticleSystem_Stop(HandleRef jarg1);

		// Token: 0x060005CA RID: 1482
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_IsPlaying")]
		public static extern bool CSParticleSystem_IsPlaying(HandleRef jarg1);

		// Token: 0x060005CB RID: 1483
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_GetFileData")]
		public static extern IntPtr CSParticleSystem_GetFileData(HandleRef jarg1);

		// Token: 0x060005CC RID: 1484
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_SetFileData")]
		public static extern void CSParticleSystem_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005CD RID: 1485
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_SetBlendFunc")]
		public static extern void CSParticleSystem_SetBlendFunc(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005CE RID: 1486
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_GetBlendFunc")]
		public static extern IntPtr CSParticleSystem_GetBlendFunc(HandleRef jarg1);

		// Token: 0x060005CF RID: 1487
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSProjectNode")]
		public static extern IntPtr new_CSProjectNode();

		// Token: 0x060005D0 RID: 1488
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSProjectNode")]
		public static extern void delete_CSProjectNode(HandleRef jarg1);

		// Token: 0x060005D1 RID: 1489
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_GetWidth")]
		public static extern float CSProjectNode_GetWidth(HandleRef jarg1);

		// Token: 0x060005D2 RID: 1490
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_GetHeight")]
		public static extern float CSProjectNode_GetHeight(HandleRef jarg1);

		// Token: 0x060005D3 RID: 1491
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_GetFileData")]
		public static extern IntPtr CSProjectNode_GetFileData(HandleRef jarg1);

		// Token: 0x060005D4 RID: 1492
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_SetFileData")]
		public static extern void CSProjectNode_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005D5 RID: 1493
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_SetProjectNode")]
		public static extern void CSProjectNode_SetProjectNode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005D6 RID: 1494
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSimpleAudio")]
		public static extern IntPtr new_CSSimpleAudio();

		// Token: 0x060005D7 RID: 1495
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSimpleAudio")]
		public static extern void delete_CSSimpleAudio(HandleRef jarg1);

		// Token: 0x060005D8 RID: 1496
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_GetName")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSSimpleAudio_GetName(HandleRef jarg1);

		// Token: 0x060005D9 RID: 1497
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_SetName")]
		public static extern void CSSimpleAudio_SetName(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x060005DA RID: 1498
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_GetVolume")]
		public static extern float CSSimpleAudio_GetVolume(HandleRef jarg1);

		// Token: 0x060005DB RID: 1499
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_SetVolume")]
		public static extern void CSSimpleAudio_SetVolume(HandleRef jarg1, float jarg2);

		// Token: 0x060005DC RID: 1500
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_GetIsLoop")]
		public static extern bool CSSimpleAudio_GetIsLoop(HandleRef jarg1);

		// Token: 0x060005DD RID: 1501
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_SetIsLoop")]
		public static extern void CSSimpleAudio_SetIsLoop(HandleRef jarg1, bool jarg2);

		// Token: 0x060005DE RID: 1502
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_Start")]
		public static extern void CSSimpleAudio_Start(HandleRef jarg1);

		// Token: 0x060005DF RID: 1503
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_Stop")]
		public static extern void CSSimpleAudio_Stop(HandleRef jarg1);

		// Token: 0x060005E0 RID: 1504
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_GetFileData")]
		public static extern IntPtr CSSimpleAudio_GetFileData(HandleRef jarg1);

		// Token: 0x060005E1 RID: 1505
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_SetFileData")]
		public static extern void CSSimpleAudio_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005E2 RID: 1506
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSDrawNode")]
		public static extern IntPtr new_CSDrawNode();

		// Token: 0x060005E3 RID: 1507
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSDrawNode")]
		public static extern void delete_CSDrawNode(HandleRef jarg1);

		// Token: 0x060005E4 RID: 1508
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawDot")]
		public static extern void CSDrawNode_DrawDot(HandleRef jarg1, HandleRef jarg2, float jarg3, HandleRef jarg4);

		// Token: 0x060005E5 RID: 1509
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawSegment")]
		public static extern void CSDrawNode_DrawSegment(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, float jarg4, HandleRef jarg5);

		// Token: 0x060005E6 RID: 1510
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawTriangle")]
		public static extern void CSDrawNode_DrawTriangle(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, HandleRef jarg5);

		// Token: 0x060005E7 RID: 1511
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawLine")]
		public static extern void CSDrawNode_DrawLine(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x060005E8 RID: 1512
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawRectangle")]
		public static extern void CSDrawNode_DrawRectangle(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, float jarg4, HandleRef jarg5, bool jarg6);

		// Token: 0x060005E9 RID: 1513
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_DrawQuadraticBezier")]
		public static extern void CSDrawNode_DrawQuadraticBezier(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4, int jarg5, HandleRef jarg6);

		// Token: 0x060005EA RID: 1514
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_Clear")]
		public static extern void CSDrawNode_Clear(HandleRef jarg1);

		// Token: 0x060005EB RID: 1515
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_SetBlendFunc")]
		public static extern void CSDrawNode_SetBlendFunc(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005EC RID: 1516
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_GetBlendFunc")]
		public static extern IntPtr CSDrawNode_GetBlendFunc(HandleRef jarg1);

		// Token: 0x060005ED RID: 1517
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_Src_set")]
		public static extern void CSBlendFunc_Src_set(HandleRef jarg1, uint jarg2);

		// Token: 0x060005EE RID: 1518
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_Src_get")]
		public static extern uint CSBlendFunc_Src_get(HandleRef jarg1);

		// Token: 0x060005EF RID: 1519
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_Dst_set")]
		public static extern void CSBlendFunc_Dst_set(HandleRef jarg1, uint jarg2);

		// Token: 0x060005F0 RID: 1520
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_Dst_get")]
		public static extern uint CSBlendFunc_Dst_get(HandleRef jarg1);

		// Token: 0x060005F1 RID: 1521
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSBlendFunc__SWIG_0")]
		public static extern IntPtr new_CSBlendFunc__SWIG_0();

		// Token: 0x060005F2 RID: 1522
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSBlendFunc__SWIG_1")]
		public static extern IntPtr new_CSBlendFunc__SWIG_1(uint jarg1, uint jarg2);

		// Token: 0x060005F3 RID: 1523
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_DISABLE_set")]
		public static extern void CSBlendFunc_DISABLE_set(HandleRef jarg1);

		// Token: 0x060005F4 RID: 1524
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_DISABLE_get")]
		public static extern IntPtr CSBlendFunc_DISABLE_get();

		// Token: 0x060005F5 RID: 1525
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ALPHA_PREMULTIPLIED_set")]
		public static extern void CSBlendFunc_ALPHA_PREMULTIPLIED_set(HandleRef jarg1);

		// Token: 0x060005F6 RID: 1526
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ALPHA_PREMULTIPLIED_get")]
		public static extern IntPtr CSBlendFunc_ALPHA_PREMULTIPLIED_get();

		// Token: 0x060005F7 RID: 1527
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ALPHA_NON_PREMULTIPLIED_set")]
		public static extern void CSBlendFunc_ALPHA_NON_PREMULTIPLIED_set(HandleRef jarg1);

		// Token: 0x060005F8 RID: 1528
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ALPHA_NON_PREMULTIPLIED_get")]
		public static extern IntPtr CSBlendFunc_ALPHA_NON_PREMULTIPLIED_get();

		// Token: 0x060005F9 RID: 1529
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ADDITIVE_set")]
		public static extern void CSBlendFunc_ADDITIVE_set(HandleRef jarg1);

		// Token: 0x060005FA RID: 1530
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBlendFunc_ADDITIVE_get")]
		public static extern IntPtr CSBlendFunc_ADDITIVE_get();

		// Token: 0x060005FB RID: 1531
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSBlendFunc")]
		public static extern void delete_CSBlendFunc(HandleRef jarg1);

		// Token: 0x060005FC RID: 1532
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSComControlNode")]
		public static extern IntPtr new_CSComControlNode();

		// Token: 0x060005FD RID: 1533
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSComControlNode")]
		public static extern void delete_CSComControlNode(HandleRef jarg1);

		// Token: 0x060005FE RID: 1534
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_HitTest")]
		public static extern int CSComControlNode_HitTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060005FF RID: 1535
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetCanvasObject")]
		public static extern void CSComControlNode_SetCanvasObject(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000600 RID: 1536
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_RectApplyTransform")]
		public static extern IntPtr CSComControlNode_RectApplyTransform(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000601 RID: 1537
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetMat")]
		public static extern void CSComControlNode_SetMat(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000602 RID: 1538
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_GetMat")]
		public static extern IntPtr CSComControlNode_GetMat(HandleRef jarg1);

		// Token: 0x06000603 RID: 1539
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetEnable")]
		public static extern void CSComControlNode_SetEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x06000604 RID: 1540
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_IsEnable")]
		public static extern bool CSComControlNode_IsEnable(HandleRef jarg1);

		// Token: 0x06000605 RID: 1541
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_GetControlPointType")]
		public static extern int CSComControlNode_GetControlPointType(HandleRef jarg1);

		// Token: 0x06000606 RID: 1542
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_GetMatrixWithoutReCalculate")]
		public static extern IntPtr CSComControlNode_GetMatrixWithoutReCalculate(HandleRef jarg1);

		// Token: 0x06000607 RID: 1543
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_Mat4Multiply")]
		public static extern IntPtr CSComControlNode_Mat4Multiply(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000608 RID: 1544
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_Mat4Inverse")]
		public static extern IntPtr CSComControlNode_Mat4Inverse(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000609 RID: 1545
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_Mat4Identity")]
		public static extern IntPtr CSComControlNode_Mat4Identity(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600060A RID: 1546
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_Mat4ToMatrixNode")]
		public static extern IntPtr CSComControlNode_Mat4ToMatrixNode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600060B RID: 1547
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_TransformPoint")]
		public static extern IntPtr CSComControlNode_TransformPoint(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x0600060C RID: 1548
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetAnchorPointVisible")]
		public static extern void CSComControlNode_SetAnchorPointVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x0600060D RID: 1549
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetControlPointVisible")]
		public static extern void CSComControlNode_SetControlPointVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x0600060E RID: 1550
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SetAttachNode")]
		public static extern void CSComControlNode_SetAttachNode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600060F RID: 1551
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_GetAnchorPointInPoints")]
		public static extern IntPtr CSComControlNode_GetAnchorPointInPoints(HandleRef jarg1);

		// Token: 0x06000610 RID: 1552
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_GetBoundingBox")]
		public static extern IntPtr CSComControlNode_GetBoundingBox(HandleRef jarg1);

		// Token: 0x06000611 RID: 1553
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_ControlOpt")]
		public static extern IntPtr new_ControlOpt();

		// Token: 0x06000612 RID: 1554
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_ControlOpt")]
		public static extern void delete_ControlOpt(HandleRef jarg1);

		// Token: 0x06000613 RID: 1555
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_ControlResult")]
		public static extern IntPtr new_ControlResult();

		// Token: 0x06000614 RID: 1556
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_ControlResult")]
		public static extern void delete_ControlResult(HandleRef jarg1);

		// Token: 0x06000615 RID: 1557
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSComControlNode3D")]
		public static extern IntPtr new_CSComControlNode3D();

		// Token: 0x06000616 RID: 1558
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSComControlNode3D")]
		public static extern void delete_CSComControlNode3D(HandleRef jarg1);

		// Token: 0x06000617 RID: 1559
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SetOpt")]
		public static extern void CSComControlNode3D_SetOpt(HandleRef jarg1, int jarg2);

		// Token: 0x06000618 RID: 1560
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_GetOpt")]
		public static extern int CSComControlNode3D_GetOpt(HandleRef jarg1);

		// Token: 0x06000619 RID: 1561
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SetSpace")]
		public static extern void CSComControlNode3D_SetSpace(HandleRef jarg1, bool jarg2);

		// Token: 0x0600061A RID: 1562
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_GetSpace")]
		public static extern bool CSComControlNode3D_GetSpace(HandleRef jarg1);

		// Token: 0x0600061B RID: 1563
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SetSelect")]
		public static extern void CSComControlNode3D_SetSelect(HandleRef jarg1, bool jarg2);

		// Token: 0x0600061C RID: 1564
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_OnSelect")]
		public static extern bool CSComControlNode3D_OnSelect(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600061D RID: 1565
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SetTarget")]
		public static extern void CSComControlNode3D_SetTarget(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600061E RID: 1566
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_OnMouseDown")]
		public static extern bool CSComControlNode3D_OnMouseDown(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600061F RID: 1567
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_OnMouseMove")]
		public static extern IntPtr CSComControlNode3D_OnMouseMove(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000620 RID: 1568
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_OnMouseUp")]
		public static extern void CSComControlNode3D_OnMouseUp(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000621 RID: 1569
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_RefreshSelectable")]
		public static extern void CSComControlNode3D_RefreshSelectable(HandleRef jarg1);

		// Token: 0x06000622 RID: 1570
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SelectShiftKey")]
		public static extern void CSComControlNode3D_SelectShiftKey(HandleRef jarg1, bool jarg2);

		// Token: 0x06000623 RID: 1571
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSWidget")]
		public static extern IntPtr new_CSWidget();

		// Token: 0x06000624 RID: 1572
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSWidget")]
		public static extern void delete_CSWidget(HandleRef jarg1);

		// Token: 0x06000625 RID: 1573
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetCustomSizeEnabled")]
		public static extern bool CSWidget_GetCustomSizeEnabled(HandleRef jarg1);

		// Token: 0x06000626 RID: 1574
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetCustomSizeEnabled")]
		public static extern void CSWidget_SetCustomSizeEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000627 RID: 1575
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetWidgetAutoSize")]
		public static extern IntPtr CSWidget_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x06000628 RID: 1576
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetTouchEnabled")]
		public static extern bool CSWidget_GetTouchEnabled(HandleRef jarg1);

		// Token: 0x06000629 RID: 1577
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetTouchEnabled")]
		public static extern void CSWidget_SetTouchEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x0600062A RID: 1578
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Left")]
		public static extern int CSWidget_GetScale9Left(HandleRef jarg1);

		// Token: 0x0600062B RID: 1579
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Left")]
		public static extern void CSWidget_SetScale9Left(HandleRef jarg1, int jarg2);

		// Token: 0x0600062C RID: 1580
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Right")]
		public static extern int CSWidget_GetScale9Right(HandleRef jarg1);

		// Token: 0x0600062D RID: 1581
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Right")]
		public static extern void CSWidget_SetScale9Right(HandleRef jarg1, int jarg2);

		// Token: 0x0600062E RID: 1582
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Top")]
		public static extern int CSWidget_GetScale9Top(HandleRef jarg1);

		// Token: 0x0600062F RID: 1583
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Top")]
		public static extern void CSWidget_SetScale9Top(HandleRef jarg1, int jarg2);

		// Token: 0x06000630 RID: 1584
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Bottom")]
		public static extern int CSWidget_GetScale9Bottom(HandleRef jarg1);

		// Token: 0x06000631 RID: 1585
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Bottom")]
		public static extern void CSWidget_SetScale9Bottom(HandleRef jarg1, int jarg2);

		// Token: 0x06000632 RID: 1586
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9OriginX")]
		public static extern int CSWidget_GetScale9OriginX(HandleRef jarg1);

		// Token: 0x06000633 RID: 1587
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9OriginY")]
		public static extern int CSWidget_GetScale9OriginY(HandleRef jarg1);

		// Token: 0x06000634 RID: 1588
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Width")]
		public static extern int CSWidget_GetScale9Width(HandleRef jarg1);

		// Token: 0x06000635 RID: 1589
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Height")]
		public static extern int CSWidget_GetScale9Height(HandleRef jarg1);

		// Token: 0x06000636 RID: 1590
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_GetScale9Enabled")]
		public static extern bool CSWidget_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x06000637 RID: 1591
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Enabled")]
		public static extern void CSWidget_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000638 RID: 1592
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SetScale9Rect")]
		public static extern void CSWidget_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x06000639 RID: 1593
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_CloneWidgetCustomProperty")]
		public static extern void CSWidget_CloneWidgetCustomProperty(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600063A RID: 1594
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_ChangeState")]
		public static extern void CSWidget_ChangeState(HandleRef jarg1, bool jarg2);

		// Token: 0x0600063B RID: 1595
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_ResetState")]
		public static extern void CSWidget_ResetState(HandleRef jarg1);

		// Token: 0x0600063C RID: 1596
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSButton")]
		public static extern IntPtr new_CSButton();

		// Token: 0x0600063D RID: 1597
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSButton")]
		public static extern void delete_CSButton(HandleRef jarg1);

		// Token: 0x0600063E RID: 1598
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetWidgetAutoSize")]
		public static extern IntPtr CSButton_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x0600063F RID: 1599
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetCustomSizeEnabled")]
		public static extern void CSButton_SetCustomSizeEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000640 RID: 1600
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetScale9Enabled")]
		public static extern bool CSButton_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x06000641 RID: 1601
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetScale9Enabled")]
		public static extern void CSButton_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000642 RID: 1602
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetScale9Rect")]
		public static extern void CSButton_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x06000643 RID: 1603
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetFlipX")]
		public static extern bool CSButton_GetFlipX(HandleRef jarg1);

		// Token: 0x06000644 RID: 1604
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetFlipX")]
		public static extern void CSButton_SetFlipX(HandleRef jarg1, bool jarg2);

		// Token: 0x06000645 RID: 1605
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetFlipY")]
		public static extern bool CSButton_GetFlipY(HandleRef jarg1);

		// Token: 0x06000646 RID: 1606
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetFlipY")]
		public static extern void CSButton_SetFlipY(HandleRef jarg1, bool jarg2);

		// Token: 0x06000647 RID: 1607
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSButton_GetText(HandleRef jarg1);

		// Token: 0x06000648 RID: 1608
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetText")]
		public static extern void CSButton_SetText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x06000649 RID: 1609
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetFontName")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSButton_GetFontName(HandleRef jarg1);

		// Token: 0x0600064A RID: 1610
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetFontName")]
		public static extern void CSButton_SetFontName(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x0600064B RID: 1611
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetFontSize")]
		public static extern int CSButton_GetFontSize(HandleRef jarg1);

		// Token: 0x0600064C RID: 1612
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetFontSize")]
		public static extern void CSButton_SetFontSize(HandleRef jarg1, int jarg2);

		// Token: 0x0600064D RID: 1613
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetTextColor")]
		public static extern IntPtr CSButton_GetTextColor(HandleRef jarg1);

		// Token: 0x0600064E RID: 1614
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetTextColor")]
		public static extern void CSButton_SetTextColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600064F RID: 1615
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetNormalFilePath")]
		public static extern IntPtr CSButton_GetNormalFilePath(HandleRef jarg1);

		// Token: 0x06000650 RID: 1616
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetNormalFilePath")]
		public static extern void CSButton_SetNormalFilePath(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000651 RID: 1617
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetPressedFilePath")]
		public static extern IntPtr CSButton_GetPressedFilePath(HandleRef jarg1);

		// Token: 0x06000652 RID: 1618
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetPressedFilePath")]
		public static extern void CSButton_SetPressedFilePath(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000653 RID: 1619
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_GetDisabledFilePath")]
		public static extern IntPtr CSButton_GetDisabledFilePath(HandleRef jarg1);

		// Token: 0x06000654 RID: 1620
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SetDisabledFilePath")]
		public static extern void CSButton_SetDisabledFilePath(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000655 RID: 1621
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_EnableShadow__SWIG_0")]
		public static extern void CSButton_EnableShadow__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, int jarg4);

		// Token: 0x06000656 RID: 1622
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_EnableShadow__SWIG_1")]
		public static extern void CSButton_EnableShadow__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x06000657 RID: 1623
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_EnableShadow__SWIG_2")]
		public static extern void CSButton_EnableShadow__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000658 RID: 1624
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_EnableShadow__SWIG_3")]
		public static extern void CSButton_EnableShadow__SWIG_3(HandleRef jarg1);

		// Token: 0x06000659 RID: 1625
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_EnableOutline")]
		public static extern void CSButton_EnableOutline(HandleRef jarg1, HandleRef jarg2, int jarg3);

		// Token: 0x0600065A RID: 1626
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_DisabledEffect")]
		public static extern void CSButton_DisabledEffect(HandleRef jarg1);

		// Token: 0x0600065B RID: 1627
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSCheckBox")]
		public static extern IntPtr new_CSCheckBox();

		// Token: 0x0600065C RID: 1628
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSCheckBox")]
		public static extern void delete_CSCheckBox(HandleRef jarg1);

		// Token: 0x0600065D RID: 1629
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_ResetState")]
		public static extern void CSCheckBox_ResetState(HandleRef jarg1);

		// Token: 0x0600065E RID: 1630
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetChecked")]
		public static extern bool CSCheckBox_GetChecked(HandleRef jarg1);

		// Token: 0x0600065F RID: 1631
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetChecked")]
		public static extern void CSCheckBox_SetChecked(HandleRef jarg1, bool jarg2);

		// Token: 0x06000660 RID: 1632
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetNormalGroundFile")]
		public static extern IntPtr CSCheckBox_GetNormalGroundFile(HandleRef jarg1);

		// Token: 0x06000661 RID: 1633
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetNormalGroudFile")]
		public static extern void CSCheckBox_SetNormalGroudFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000662 RID: 1634
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetPressedGroundFile")]
		public static extern IntPtr CSCheckBox_GetPressedGroundFile(HandleRef jarg1);

		// Token: 0x06000663 RID: 1635
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetPressedGroudFile")]
		public static extern void CSCheckBox_SetPressedGroudFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000664 RID: 1636
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetDisabledGroundFile")]
		public static extern IntPtr CSCheckBox_GetDisabledGroundFile(HandleRef jarg1);

		// Token: 0x06000665 RID: 1637
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetDisabledGroudFile")]
		public static extern void CSCheckBox_SetDisabledGroudFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000666 RID: 1638
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetNormalNodeFile")]
		public static extern IntPtr CSCheckBox_GetNormalNodeFile(HandleRef jarg1);

		// Token: 0x06000667 RID: 1639
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetNormalNodeFile")]
		public static extern void CSCheckBox_SetNormalNodeFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000668 RID: 1640
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_GetDisabledNodeFile")]
		public static extern IntPtr CSCheckBox_GetDisabledNodeFile(HandleRef jarg1);

		// Token: 0x06000669 RID: 1641
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SetDisabledNodeFile")]
		public static extern void CSCheckBox_SetDisabledNodeFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600066A RID: 1642
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSImageView")]
		public static extern IntPtr new_CSImageView();

		// Token: 0x0600066B RID: 1643
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSImageView")]
		public static extern void delete_CSImageView(HandleRef jarg1);

		// Token: 0x0600066C RID: 1644
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_GetFlipX")]
		public static extern bool CSImageView_GetFlipX(HandleRef jarg1);

		// Token: 0x0600066D RID: 1645
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SetFlipX")]
		public static extern void CSImageView_SetFlipX(HandleRef jarg1, bool jarg2);

		// Token: 0x0600066E RID: 1646
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_GetFlipY")]
		public static extern bool CSImageView_GetFlipY(HandleRef jarg1);

		// Token: 0x0600066F RID: 1647
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SetFlipY")]
		public static extern void CSImageView_SetFlipY(HandleRef jarg1, bool jarg2);

		// Token: 0x06000670 RID: 1648
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_GetScale9Enabled")]
		public static extern bool CSImageView_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x06000671 RID: 1649
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SetScale9Enabled")]
		public static extern void CSImageView_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000672 RID: 1650
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SetScale9Rect")]
		public static extern void CSImageView_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x06000673 RID: 1651
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_GetFileData")]
		public static extern IntPtr CSImageView_GetFileData(HandleRef jarg1);

		// Token: 0x06000674 RID: 1652
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SetFileData")]
		public static extern void CSImageView_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000675 RID: 1653
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSText")]
		public static extern IntPtr new_CSText();

		// Token: 0x06000676 RID: 1654
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSText")]
		public static extern void delete_CSText(HandleRef jarg1);

		// Token: 0x06000677 RID: 1655
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetWidgetAutoSize")]
		public static extern IntPtr CSText_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x06000678 RID: 1656
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetCustomSizeEnabled")]
		public static extern void CSText_SetCustomSizeEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x06000679 RID: 1657
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetFlipX")]
		public static extern bool CSText_GetFlipX(HandleRef jarg1);

		// Token: 0x0600067A RID: 1658
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetFlipX")]
		public static extern void CSText_SetFlipX(HandleRef jarg1, bool jarg2);

		// Token: 0x0600067B RID: 1659
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetFlipY")]
		public static extern bool CSText_GetFlipY(HandleRef jarg1);

		// Token: 0x0600067C RID: 1660
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetFlipY")]
		public static extern void CSText_SetFlipY(HandleRef jarg1, bool jarg2);

		// Token: 0x0600067D RID: 1661
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetFontName")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSText_GetFontName(HandleRef jarg1);

		// Token: 0x0600067E RID: 1662
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetFontName")]
		public static extern void CSText_SetFontName(HandleRef jarg1, string jarg2);

		// Token: 0x0600067F RID: 1663
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetFontSize")]
		public static extern int CSText_GetFontSize(HandleRef jarg1);

		// Token: 0x06000680 RID: 1664
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetFontSize")]
		public static extern void CSText_SetFontSize(HandleRef jarg1, int jarg2);

		// Token: 0x06000681 RID: 1665
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetLabelText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSText_GetLabelText(HandleRef jarg1);

		// Token: 0x06000682 RID: 1666
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetLabelText")]
		public static extern void CSText_SetLabelText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x06000683 RID: 1667
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetHorizontalAlignmentType")]
		public static extern int CSText_GetHorizontalAlignmentType(HandleRef jarg1);

		// Token: 0x06000684 RID: 1668
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetHorizontalAlignmentType")]
		public static extern void CSText_SetHorizontalAlignmentType(HandleRef jarg1, int jarg2);

		// Token: 0x06000685 RID: 1669
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetVerticalAlignmentType")]
		public static extern int CSText_GetVerticalAlignmentType(HandleRef jarg1);

		// Token: 0x06000686 RID: 1670
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetVerticalAlignmentType")]
		public static extern void CSText_SetVerticalAlignmentType(HandleRef jarg1, int jarg2);

		// Token: 0x06000687 RID: 1671
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetTouchScaleChangeEanbleState")]
		public static extern bool CSText_GetTouchScaleChangeEanbleState(HandleRef jarg1);

		// Token: 0x06000688 RID: 1672
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetTouchScaleChangeEanbleState")]
		public static extern void CSText_SetTouchScaleChangeEanbleState(HandleRef jarg1, bool jarg2);

		// Token: 0x06000689 RID: 1673
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_EnableShadow__SWIG_0")]
		public static extern void CSText_EnableShadow__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, int jarg4);

		// Token: 0x0600068A RID: 1674
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_EnableShadow__SWIG_1")]
		public static extern void CSText_EnableShadow__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x0600068B RID: 1675
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_EnableShadow__SWIG_2")]
		public static extern void CSText_EnableShadow__SWIG_2(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600068C RID: 1676
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_EnableShadow__SWIG_3")]
		public static extern void CSText_EnableShadow__SWIG_3(HandleRef jarg1);

		// Token: 0x0600068D RID: 1677
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_EnableOutline")]
		public static extern void CSText_EnableOutline(HandleRef jarg1, HandleRef jarg2, int jarg3);

		// Token: 0x0600068E RID: 1678
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_DisableEffect")]
		public static extern void CSText_DisableEffect(HandleRef jarg1);

		// Token: 0x0600068F RID: 1679
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_DisableShadow")]
		public static extern void CSText_DisableShadow(HandleRef jarg1);

		// Token: 0x06000690 RID: 1680
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_DisableOutline")]
		public static extern void CSText_DisableOutline(HandleRef jarg1);

		// Token: 0x06000691 RID: 1681
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SetTextColor")]
		public static extern void CSText_SetTextColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000692 RID: 1682
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_GetTextColor")]
		public static extern IntPtr CSText_GetTextColor(HandleRef jarg1);

		// Token: 0x06000693 RID: 1683
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTextAtlas")]
		public static extern IntPtr new_CSTextAtlas();

		// Token: 0x06000694 RID: 1684
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTextAtlas")]
		public static extern void delete_CSTextAtlas(HandleRef jarg1);

		// Token: 0x06000695 RID: 1685
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SetStartChar")]
		public static extern void CSTextAtlas_SetStartChar(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x06000696 RID: 1686
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_GetStartChar")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextAtlas_GetStartChar(HandleRef jarg1);

		// Token: 0x06000697 RID: 1687
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_GetCharacterWidth")]
		public static extern int CSTextAtlas_GetCharacterWidth(HandleRef jarg1);

		// Token: 0x06000698 RID: 1688
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SetCharacterWidth")]
		public static extern void CSTextAtlas_SetCharacterWidth(HandleRef jarg1, int jarg2);

		// Token: 0x06000699 RID: 1689
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_GetCharacterHeight")]
		public static extern int CSTextAtlas_GetCharacterHeight(HandleRef jarg1);

		// Token: 0x0600069A RID: 1690
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SetCharacterHeight")]
		public static extern void CSTextAtlas_SetCharacterHeight(HandleRef jarg1, int jarg2);

		// Token: 0x0600069B RID: 1691
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_GetText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextAtlas_GetText(HandleRef jarg1);

		// Token: 0x0600069C RID: 1692
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SetText")]
		public static extern void CSTextAtlas_SetText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x0600069D RID: 1693
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_GetAtlasFile")]
		public static extern IntPtr CSTextAtlas_GetAtlasFile(HandleRef jarg1);

		// Token: 0x0600069E RID: 1694
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SetAtlasFile")]
		public static extern bool CSTextAtlas_SetAtlasFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600069F RID: 1695
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTextBMFont")]
		public static extern IntPtr new_CSTextBMFont();

		// Token: 0x060006A0 RID: 1696
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTextBMFont")]
		public static extern void delete_CSTextBMFont(HandleRef jarg1);

		// Token: 0x060006A1 RID: 1697
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextBMFont_GetText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextBMFont_GetText(HandleRef jarg1);

		// Token: 0x060006A2 RID: 1698
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextBMFont_SetText")]
		public static extern void CSTextBMFont_SetText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x060006A3 RID: 1699
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextBMFont_GetFntFile")]
		public static extern IntPtr CSTextBMFont_GetFntFile(HandleRef jarg1);

		// Token: 0x060006A4 RID: 1700
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextBMFont_SetFntFile")]
		public static extern void CSTextBMFont_SetFntFile(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006A5 RID: 1701
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSLoadingBar")]
		public static extern IntPtr new_CSLoadingBar();

		// Token: 0x060006A6 RID: 1702
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSLoadingBar")]
		public static extern void delete_CSLoadingBar(HandleRef jarg1);

		// Token: 0x060006A7 RID: 1703
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_GetProgressPercent")]
		public static extern int CSLoadingBar_GetProgressPercent(HandleRef jarg1);

		// Token: 0x060006A8 RID: 1704
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SetProgressPercent")]
		public static extern void CSLoadingBar_SetProgressPercent(HandleRef jarg1, int jarg2);

		// Token: 0x060006A9 RID: 1705
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_GetProgressType")]
		public static extern int CSLoadingBar_GetProgressType(HandleRef jarg1);

		// Token: 0x060006AA RID: 1706
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SetProgressType")]
		public static extern void CSLoadingBar_SetProgressType(HandleRef jarg1, int jarg2);

		// Token: 0x060006AB RID: 1707
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_GetScale9Enabled")]
		public static extern bool CSLoadingBar_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x060006AC RID: 1708
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SetScale9Enabled")]
		public static extern void CSLoadingBar_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006AD RID: 1709
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SetScale9Rect")]
		public static extern void CSLoadingBar_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x060006AE RID: 1710
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_GetFileData")]
		public static extern IntPtr CSLoadingBar_GetFileData(HandleRef jarg1);

		// Token: 0x060006AF RID: 1711
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SetFileData")]
		public static extern void CSLoadingBar_SetFileData(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006B0 RID: 1712
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSlider")]
		public static extern IntPtr new_CSSlider();

		// Token: 0x060006B1 RID: 1713
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSlider")]
		public static extern void delete_CSSlider(HandleRef jarg1);

		// Token: 0x060006B2 RID: 1714
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetPercent")]
		public static extern int CSSlider_GetPercent(HandleRef jarg1);

		// Token: 0x060006B3 RID: 1715
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetPercent")]
		public static extern void CSSlider_SetPercent(HandleRef jarg1, int jarg2);

		// Token: 0x060006B4 RID: 1716
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetScale9Enabled")]
		public static extern bool CSSlider_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x060006B5 RID: 1717
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetScale9Enabled")]
		public static extern void CSSlider_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006B6 RID: 1718
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetScale9Rect")]
		public static extern void CSSlider_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x060006B7 RID: 1719
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetGroundBarTexture")]
		public static extern IntPtr CSSlider_GetGroundBarTexture(HandleRef jarg1);

		// Token: 0x060006B8 RID: 1720
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetGroundBarTexture")]
		public static extern void CSSlider_SetGroundBarTexture(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006B9 RID: 1721
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetProgressBarTexture")]
		public static extern IntPtr CSSlider_GetProgressBarTexture(HandleRef jarg1);

		// Token: 0x060006BA RID: 1722
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetProgressBarTexture")]
		public static extern void CSSlider_SetProgressBarTexture(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006BB RID: 1723
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetBallNormalTexture")]
		public static extern IntPtr CSSlider_GetBallNormalTexture(HandleRef jarg1);

		// Token: 0x060006BC RID: 1724
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetBallNormalTexture")]
		public static extern void CSSlider_SetBallNormalTexture(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006BD RID: 1725
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetBallPressedTexture")]
		public static extern IntPtr CSSlider_GetBallPressedTexture(HandleRef jarg1);

		// Token: 0x060006BE RID: 1726
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetBallPressedTexture")]
		public static extern void CSSlider_SetBallPressedTexture(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006BF RID: 1727
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetBallDisabledTexture")]
		public static extern IntPtr CSSlider_GetBallDisabledTexture(HandleRef jarg1);

		// Token: 0x060006C0 RID: 1728
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SetBallDisabledTexture")]
		public static extern void CSSlider_SetBallDisabledTexture(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006C1 RID: 1729
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_GetWidgetAutoSize")]
		public static extern IntPtr CSSlider_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x060006C2 RID: 1730
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTextField")]
		public static extern IntPtr new_CSTextField();

		// Token: 0x060006C3 RID: 1731
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTextField")]
		public static extern void delete_CSTextField(HandleRef jarg1);

		// Token: 0x060006C4 RID: 1732
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetCustomSizeEnabled")]
		public static extern void CSTextField_SetCustomSizeEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006C5 RID: 1733
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetFontName")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextField_GetFontName(HandleRef jarg1);

		// Token: 0x060006C6 RID: 1734
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetFontName")]
		public static extern void CSTextField_SetFontName(HandleRef jarg1, string jarg2);

		// Token: 0x060006C7 RID: 1735
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetFontSize")]
		public static extern int CSTextField_GetFontSize(HandleRef jarg1);

		// Token: 0x060006C8 RID: 1736
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetFontSize")]
		public static extern void CSTextField_SetFontSize(HandleRef jarg1, int jarg2);

		// Token: 0x060006C9 RID: 1737
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetLabelText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextField_GetLabelText(HandleRef jarg1);

		// Token: 0x060006CA RID: 1738
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetLabelText")]
		public static extern void CSTextField_SetLabelText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x060006CB RID: 1739
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetPlaceHolderText")]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))]
		public static extern string CSTextField_GetPlaceHolderText(HandleRef jarg1);

		// Token: 0x060006CC RID: 1740
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetPlaceHolderText")]
		public static extern void CSTextField_SetPlaceHolderText(HandleRef jarg1, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(CocoStudio.EngineAdapterWrap.UTF8Marshaler))] string jarg2);

		// Token: 0x060006CD RID: 1741
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetPlaceHolderTextColor")]
		public static extern IntPtr CSTextField_GetPlaceHolderTextColor(HandleRef jarg1);

		// Token: 0x060006CE RID: 1742
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetPlaceHolderTextColor")]
		public static extern void CSTextField_SetPlaceHolderTextColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006CF RID: 1743
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetPassWordEnabled")]
		public static extern bool CSTextField_GetPassWordEnabled(HandleRef jarg1);

		// Token: 0x060006D0 RID: 1744
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetPassWordEnabled")]
		public static extern void CSTextField_SetPassWordEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006D1 RID: 1745
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetPasswordStyleText")]
		public static extern string CSTextField_GetPasswordStyleText(HandleRef jarg1);

		// Token: 0x060006D2 RID: 1746
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetPasswordStyleText")]
		public static extern void CSTextField_SetPasswordStyleText(HandleRef jarg1, string jarg2);

		// Token: 0x060006D3 RID: 1747
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetLengthLimited")]
		public static extern bool CSTextField_GetLengthLimited(HandleRef jarg1);

		// Token: 0x060006D4 RID: 1748
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetLengthLimited")]
		public static extern void CSTextField_SetLengthLimited(HandleRef jarg1, bool jarg2);

		// Token: 0x060006D5 RID: 1749
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetMaxLength")]
		public static extern int CSTextField_GetMaxLength(HandleRef jarg1);

		// Token: 0x060006D6 RID: 1750
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SetMaxLength")]
		public static extern void CSTextField_SetMaxLength(HandleRef jarg1, int jarg2);

		// Token: 0x060006D7 RID: 1751
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_GetWidgetAutoSize")]
		public static extern IntPtr CSTextField_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x060006D8 RID: 1752
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSPanel")]
		public static extern IntPtr new_CSPanel();

		// Token: 0x060006D9 RID: 1753
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSPanel")]
		public static extern void delete_CSPanel(HandleRef jarg1);

		// Token: 0x060006DA RID: 1754
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetWidgetAutoSize")]
		public static extern IntPtr CSPanel_GetWidgetAutoSize(HandleRef jarg1);

		// Token: 0x060006DB RID: 1755
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetScale9Enabled")]
		public static extern bool CSPanel_GetScale9Enabled(HandleRef jarg1);

		// Token: 0x060006DC RID: 1756
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetScale9Enabled")]
		public static extern void CSPanel_SetScale9Enabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006DD RID: 1757
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetScale9Rect")]
		public static extern void CSPanel_SetScale9Rect(HandleRef jarg1, int jarg2, int jarg3, int jarg4, int jarg5);

		// Token: 0x060006DE RID: 1758
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetClipAble")]
		public static extern bool CSPanel_GetClipAble(HandleRef jarg1);

		// Token: 0x060006DF RID: 1759
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetClipAble")]
		public static extern void CSPanel_SetClipAble(HandleRef jarg1, bool jarg2);

		// Token: 0x060006E0 RID: 1760
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundAlpha")]
		public static extern int CSPanel_GetGroundAlpha(HandleRef jarg1);

		// Token: 0x060006E1 RID: 1761
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundAlpha")]
		public static extern void CSPanel_SetGroundAlpha(HandleRef jarg1, int jarg2);

		// Token: 0x060006E2 RID: 1762
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundColorType")]
		public static extern int CSPanel_GetGroundColorType(HandleRef jarg1);

		// Token: 0x060006E3 RID: 1763
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundColorType")]
		public static extern void CSPanel_SetGroundColorType(HandleRef jarg1, int jarg2);

		// Token: 0x060006E4 RID: 1764
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundSingleColor")]
		public static extern IntPtr CSPanel_GetGroundSingleColor(HandleRef jarg1);

		// Token: 0x060006E5 RID: 1765
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundSingleColor")]
		public static extern void CSPanel_SetGroundSingleColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006E6 RID: 1766
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundLineStartColor")]
		public static extern IntPtr CSPanel_GetGroundLineStartColor(HandleRef jarg1);

		// Token: 0x060006E7 RID: 1767
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundLineStartColor")]
		public static extern void CSPanel_SetGroundLineStartColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006E8 RID: 1768
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundLineEndColor")]
		public static extern IntPtr CSPanel_GetGroundLineEndColor(HandleRef jarg1);

		// Token: 0x060006E9 RID: 1769
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundLineEndColor")]
		public static extern void CSPanel_SetGroundLineEndColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006EA RID: 1770
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetGroundColorVector")]
		public static extern IntPtr CSPanel_GetGroundColorVector(HandleRef jarg1);

		// Token: 0x060006EB RID: 1771
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetGroundColorVector")]
		public static extern void CSPanel_SetGroundColorVector(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006EC RID: 1772
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetFilePath")]
		public static extern IntPtr CSPanel_GetFilePath(HandleRef jarg1);

		// Token: 0x060006ED RID: 1773
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetFilePath")]
		public static extern void CSPanel_SetFilePath(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006EE RID: 1774
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_GetContainerLayoutType")]
		public static extern int CSPanel_GetContainerLayoutType(HandleRef jarg1);

		// Token: 0x060006EF RID: 1775
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SetContainerLayoutType")]
		public static extern void CSPanel_SetContainerLayoutType(HandleRef jarg1, int jarg2);

		// Token: 0x060006F0 RID: 1776
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_RemoveBackGroundFile")]
		public static extern void CSPanel_RemoveBackGroundFile(HandleRef jarg1);

		// Token: 0x060006F1 RID: 1777
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScrollView")]
		public static extern IntPtr new_CSScrollView();

		// Token: 0x060006F2 RID: 1778
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSScrollView")]
		public static extern void delete_CSScrollView(HandleRef jarg1);

		// Token: 0x060006F3 RID: 1779
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_GetInnerSize")]
		public static extern IntPtr CSScrollView_GetInnerSize(HandleRef jarg1);

		// Token: 0x060006F4 RID: 1780
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_SetInnerSize")]
		public static extern void CSScrollView_SetInnerSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006F5 RID: 1781
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_GetDirectionType")]
		public static extern int CSScrollView_GetDirectionType(HandleRef jarg1);

		// Token: 0x060006F6 RID: 1782
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_SetDirectionType")]
		public static extern void CSScrollView_SetDirectionType(HandleRef jarg1, int jarg2);

		// Token: 0x060006F7 RID: 1783
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_GetBounceEnabled")]
		public static extern bool CSScrollView_GetBounceEnabled(HandleRef jarg1);

		// Token: 0x060006F8 RID: 1784
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_SetBounceEnabled")]
		public static extern void CSScrollView_SetBounceEnabled(HandleRef jarg1, bool jarg2);

		// Token: 0x060006F9 RID: 1785
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_TransformToSelfInner")]
		public static extern IntPtr CSScrollView_TransformToSelfInner(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006FA RID: 1786
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_GetBoundingRect")]
		public static extern IntPtr CSScrollView_GetBoundingRect(HandleRef jarg1);

		// Token: 0x060006FB RID: 1787
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSPageView")]
		public static extern IntPtr new_CSPageView();

		// Token: 0x060006FC RID: 1788
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSPageView")]
		public static extern void delete_CSPageView(HandleRef jarg1);

		// Token: 0x060006FD RID: 1789
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPageView_AddChild")]
		public static extern void CSPageView_AddChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006FE RID: 1790
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPageView_RemoveChild")]
		public static extern void CSPageView_RemoveChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x060006FF RID: 1791
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPageView_InsertChild")]
		public static extern void CSPageView_InsertChild(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000700 RID: 1792
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPageView_SetSize")]
		public static extern void CSPageView_SetSize(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000701 RID: 1793
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSListView")]
		public static extern IntPtr new_CSListView();

		// Token: 0x06000702 RID: 1794
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSListView")]
		public static extern void delete_CSListView(HandleRef jarg1);

		// Token: 0x06000703 RID: 1795
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_GetItemSpace")]
		public static extern int CSListView_GetItemSpace(HandleRef jarg1);

		// Token: 0x06000704 RID: 1796
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_SetItemSpace")]
		public static extern void CSListView_SetItemSpace(HandleRef jarg1, int jarg2);

		// Token: 0x06000705 RID: 1797
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_GetGravityType")]
		public static extern int CSListView_GetGravityType(HandleRef jarg1);

		// Token: 0x06000706 RID: 1798
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_SetGravityType")]
		public static extern void CSListView_SetGravityType(HandleRef jarg1, int jarg2);

		// Token: 0x06000707 RID: 1799
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_InsertChild")]
		public static extern void CSListView_InsertChild(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000708 RID: 1800
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_AddChild")]
		public static extern void CSListView_AddChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000709 RID: 1801
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_RemoveChild")]
		public static extern void CSListView_RemoveChild(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600070A RID: 1802
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_SetDirectionType")]
		public static extern void CSListView_SetDirectionType(HandleRef jarg1, int jarg2);

		// Token: 0x0600070B RID: 1803
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_RefreshInnerLayout")]
		public static extern void CSListView_RefreshInnerLayout(HandleRef jarg1);

		// Token: 0x0600070C RID: 1804
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTimelineFrame")]
		public static extern IntPtr new_CSTimelineFrame();

		// Token: 0x0600070D RID: 1805
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTimelineFrame")]
		public static extern void delete_CSTimelineFrame(HandleRef jarg1);

		// Token: 0x0600070E RID: 1806
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_GetFrameIndex")]
		public static extern int CSTimelineFrame_GetFrameIndex(HandleRef jarg1);

		// Token: 0x0600070F RID: 1807
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_SetFrameIndex")]
		public static extern void CSTimelineFrame_SetFrameIndex(HandleRef jarg1, int jarg2);

		// Token: 0x06000710 RID: 1808
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_SetTween")]
		public static extern void CSTimelineFrame_SetTween(HandleRef jarg1, bool jarg2);

		// Token: 0x06000711 RID: 1809
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_IsTween")]
		public static extern bool CSTimelineFrame_IsTween(HandleRef jarg1);

		// Token: 0x06000712 RID: 1810
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_SetTweenType")]
		public static extern void CSTimelineFrame_SetTweenType(HandleRef jarg1, int jarg2);

		// Token: 0x06000713 RID: 1811
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_GetTweenType")]
		public static extern int CSTimelineFrame_GetTweenType(HandleRef jarg1);

		// Token: 0x06000714 RID: 1812
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_SetEasingParam")]
		public static extern void CSTimelineFrame_SetEasingParam(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000715 RID: 1813
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_GetEasingParam")]
		public static extern IntPtr CSTimelineFrame_GetEasingParam(HandleRef jarg1);

		// Token: 0x06000716 RID: 1814
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_OnEnter")]
		public static extern void CSTimelineFrame_OnEnter(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000717 RID: 1815
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSExtensionFrame__SWIG_0")]
		public static extern IntPtr new_CSExtensionFrame__SWIG_0(HandleRef jarg1);

		// Token: 0x06000718 RID: 1816
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSExtensionFrame__SWIG_1")]
		public static extern IntPtr new_CSExtensionFrame__SWIG_1();

		// Token: 0x06000719 RID: 1817
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSExtensionFrame")]
		public static extern void delete_CSExtensionFrame(HandleRef jarg1);

		// Token: 0x0600071A RID: 1818
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSExtensionFrame_SetFrameEnterCallBack")]
		public static extern void CSExtensionFrame_SetFrameEnterCallBack(HandleRef jarg1, CSExtensionFrame.FrameEnterCallBack jarg2);

		// Token: 0x0600071B RID: 1819
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSExtensionFrame_SetFrameApplyCallBack")]
		public static extern void CSExtensionFrame_SetFrameApplyCallBack(HandleRef jarg1, CSExtensionFrame.FrameApplyCallBack jarg2);

		// Token: 0x0600071C RID: 1820
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTimeline")]
		public static extern IntPtr new_CSTimeline();

		// Token: 0x0600071D RID: 1821
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTimeline")]
		public static extern void delete_CSTimeline(HandleRef jarg1);

		// Token: 0x0600071E RID: 1822
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_SetActionTag")]
		public static extern void CSTimeline_SetActionTag(HandleRef jarg1, int jarg2);

		// Token: 0x0600071F RID: 1823
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_GetActionTag")]
		public static extern int CSTimeline_GetActionTag(HandleRef jarg1);

		// Token: 0x06000720 RID: 1824
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_GotoFrame")]
		public static extern void CSTimeline_GotoFrame(HandleRef jarg1, int jarg2);

		// Token: 0x06000721 RID: 1825
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_InsertFrame")]
		public static extern void CSTimeline_InsertFrame(HandleRef jarg1, int jarg2, HandleRef jarg3);

		// Token: 0x06000722 RID: 1826
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_RemoveFrame")]
		public static extern void CSTimeline_RemoveFrame(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000723 RID: 1827
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_ClearSearchState")]
		public static extern void CSTimeline_ClearSearchState(HandleRef jarg1);

		// Token: 0x06000724 RID: 1828
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_IsNeedChangeState")]
		public static extern bool CSTimelineAction_IsNeedChangeState(HandleRef jarg1);

		// Token: 0x06000725 RID: 1829
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetOnionSkinPreNum")]
		public static extern void CSTimelineAction_SetOnionSkinPreNum(HandleRef jarg1, int jarg2);

		// Token: 0x06000726 RID: 1830
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetOnionSkinPreNum")]
		public static extern int CSTimelineAction_GetOnionSkinPreNum(HandleRef jarg1);

		// Token: 0x06000727 RID: 1831
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetOnionSkinSuffNum")]
		public static extern void CSTimelineAction_SetOnionSkinSuffNum(HandleRef jarg1, int jarg2);

		// Token: 0x06000728 RID: 1832
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetOnionSkinSuffNum")]
		public static extern int CSTimelineAction_GetOnionSkinSuffNum(HandleRef jarg1);

		// Token: 0x06000729 RID: 1833
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetOnionSkinEnable")]
		public static extern void CSTimelineAction_SetOnionSkinEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x0600072A RID: 1834
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_IsOnionSkinEnable")]
		public static extern bool CSTimelineAction_IsOnionSkinEnable(HandleRef jarg1);

		// Token: 0x0600072B RID: 1835
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_AddOnionSkinKey")]
		public static extern void CSTimelineAction_AddOnionSkinKey(HandleRef jarg1, int jarg2);

		// Token: 0x0600072C RID: 1836
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_RemoveOnionSkinKey")]
		public static extern void CSTimelineAction_RemoveOnionSkinKey(HandleRef jarg1, int jarg2);

		// Token: 0x0600072D RID: 1837
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_IsOnionKeyFrame")]
		public static extern bool CSTimelineAction_IsOnionKeyFrame(HandleRef jarg1, int jarg2);

		// Token: 0x0600072E RID: 1838
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTimelineAction__SWIG_0")]
		public static extern IntPtr new_CSTimelineAction__SWIG_0(HandleRef jarg1);

		// Token: 0x0600072F RID: 1839
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTimelineAction__SWIG_1")]
		public static extern IntPtr new_CSTimelineAction__SWIG_1();

		// Token: 0x06000730 RID: 1840
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTimelineAction")]
		public static extern void delete_CSTimelineAction(HandleRef jarg1);

		// Token: 0x06000731 RID: 1841
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GotoFrame")]
		public static extern void CSTimelineAction_GotoFrame(HandleRef jarg1, int jarg2);

		// Token: 0x06000732 RID: 1842
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_Play")]
		public static extern void CSTimelineAction_Play(HandleRef jarg1, int jarg2, int jarg3, int jarg4, bool jarg5);

		// Token: 0x06000733 RID: 1843
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_Pause")]
		public static extern void CSTimelineAction_Pause(HandleRef jarg1);

		// Token: 0x06000734 RID: 1844
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_Resume")]
		public static extern void CSTimelineAction_Resume(HandleRef jarg1);

		// Token: 0x06000735 RID: 1845
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetTimeSpeed")]
		public static extern void CSTimelineAction_SetTimeSpeed(HandleRef jarg1, float jarg2);

		// Token: 0x06000736 RID: 1846
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetTimeSpeed")]
		public static extern float CSTimelineAction_GetTimeSpeed(HandleRef jarg1);

		// Token: 0x06000737 RID: 1847
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetDuration")]
		public static extern void CSTimelineAction_SetDuration(HandleRef jarg1, int jarg2);

		// Token: 0x06000738 RID: 1848
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetDuration")]
		public static extern int CSTimelineAction_GetDuration(HandleRef jarg1);

		// Token: 0x06000739 RID: 1849
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetEndFrame")]
		public static extern void CSTimelineAction_SetEndFrame(HandleRef jarg1, int jarg2);

		// Token: 0x0600073A RID: 1850
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetEndFrame")]
		public static extern int CSTimelineAction_GetEndFrame(HandleRef jarg1);

		// Token: 0x0600073B RID: 1851
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SetCurrentFrame")]
		public static extern void CSTimelineAction_SetCurrentFrame(HandleRef jarg1, int jarg2);

		// Token: 0x0600073C RID: 1852
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_GetCurrentFrame")]
		public static extern int CSTimelineAction_GetCurrentFrame(HandleRef jarg1);

		// Token: 0x0600073D RID: 1853
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_AddTimeline")]
		public static extern void CSTimelineAction_AddTimeline(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600073E RID: 1854
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_RemoveTimeline")]
		public static extern void CSTimelineAction_RemoveTimeline(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600073F RID: 1855
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_IsPlaying")]
		public static extern bool CSTimelineAction_IsPlaying(HandleRef jarg1);

		// Token: 0x06000740 RID: 1856
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_InitWithRootNode")]
		public static extern void CSTimelineAction_InitWithRootNode(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000741 RID: 1857
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_ActiveAction")]
		public static extern void CSTimelineAction_ActiveAction(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000742 RID: 1858
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSGuidesService")]
		public static extern void delete_CSGuidesService(HandleRef jarg1);

		// Token: 0x06000743 RID: 1859
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_GetInstance")]
		public static extern IntPtr CSGuidesService_GetInstance();

		// Token: 0x06000744 RID: 1860
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_SetVisible")]
		public static extern void CSGuidesService_SetVisible(HandleRef jarg1, bool jarg2);

		// Token: 0x06000745 RID: 1861
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_GetVisible")]
		public static extern bool CSGuidesService_GetVisible(HandleRef jarg1);

		// Token: 0x06000746 RID: 1862
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_Add")]
		public static extern void CSGuidesService_Add(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000747 RID: 1863
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_Remove")]
		public static extern void CSGuidesService_Remove(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000748 RID: 1864
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_Clear")]
		public static extern void CSGuidesService_Clear(HandleRef jarg1);

		// Token: 0x06000749 RID: 1865
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_GetHoldGuides")]
		public static extern IntPtr CSGuidesService_GetHoldGuides(HandleRef jarg1);

		// Token: 0x0600074A RID: 1866
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_SetHoldGuides")]
		public static extern void CSGuidesService_SetHoldGuides(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600074B RID: 1867
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_SetColor")]
		public static extern void CSGuidesService_SetColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600074C RID: 1868
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSGuides")]
		public static extern IntPtr new_CSGuides(int jarg1);

		// Token: 0x0600074D RID: 1869
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSGuides")]
		public static extern void delete_CSGuides(HandleRef jarg1);

		// Token: 0x0600074E RID: 1870
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuides_GetDirection")]
		public static extern int CSGuides_GetDirection(HandleRef jarg1);

		// Token: 0x0600074F RID: 1871
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuides_SetDirection")]
		public static extern void CSGuides_SetDirection(HandleRef jarg1, int jarg2);

		// Token: 0x06000750 RID: 1872
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuides_GetPosition")]
		public static extern float CSGuides_GetPosition(HandleRef jarg1);

		// Token: 0x06000751 RID: 1873
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuides_SetPosition")]
		public static extern void CSGuides_SetPosition(HandleRef jarg1, float jarg2);

		// Token: 0x06000752 RID: 1874
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSBoneNode")]
		public static extern IntPtr new_CSBoneNode();

		// Token: 0x06000753 RID: 1875
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSBoneNode")]
		public static extern void delete_CSBoneNode(HandleRef jarg1);

		// Token: 0x06000754 RID: 1876
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_Display__SWIG_0")]
		public static extern void CSBoneNode_Display__SWIG_0(HandleRef jarg1, HandleRef jarg2, bool jarg3);

		// Token: 0x06000755 RID: 1877
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_Display__SWIG_1")]
		public static extern void CSBoneNode_Display__SWIG_1(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000756 RID: 1878
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SetLength")]
		public static extern void CSBoneNode_SetLength(HandleRef jarg1, float jarg2);

		// Token: 0x06000757 RID: 1879
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_GetLength")]
		public static extern float CSBoneNode_GetLength(HandleRef jarg1);

		// Token: 0x06000758 RID: 1880
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SetDebugDrawEnable")]
		public static extern void CSBoneNode_SetDebugDrawEnable(HandleRef jarg1, bool jarg2);

		// Token: 0x06000759 RID: 1881
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_GetBoneRackShow")]
		public static extern bool CSBoneNode_GetBoneRackShow(HandleRef jarg1);

		// Token: 0x0600075A RID: 1882
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SetBoneRackColor")]
		public static extern void CSBoneNode_SetBoneRackColor(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600075B RID: 1883
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_GetBoneRackColor")]
		public static extern IntPtr CSBoneNode_GetBoneRackColor(HandleRef jarg1);

		// Token: 0x0600075C RID: 1884
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_GetBoxSize")]
		public static extern IntPtr CSBoneNode_GetBoxSize(HandleRef jarg1);

		// Token: 0x0600075D RID: 1885
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SetBlendFunc")]
		public static extern void CSBoneNode_SetBlendFunc(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600075E RID: 1886
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_GetBlendFunc")]
		public static extern IntPtr CSBoneNode_GetBlendFunc(HandleRef jarg1);

		// Token: 0x0600075F RID: 1887
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SetObjectState")]
		public static extern void CSBoneNode_SetObjectState(HandleRef jarg1, int jarg2);

		// Token: 0x06000760 RID: 1888
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_HitTest")]
		public static extern int CSBoneNode_HitTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000761 RID: 1889
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_RectTest")]
		public static extern bool CSBoneNode_RectTest(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000762 RID: 1890
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_ResetBoneScaledWidth")]
		public static extern void CSBoneNode_ResetBoneScaledWidth(HandleRef jarg1);

		// Token: 0x06000763 RID: 1891
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSSkeletonNode")]
		public static extern IntPtr new_CSSkeletonNode();

		// Token: 0x06000764 RID: 1892
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSSkeletonNode")]
		public static extern void delete_CSSkeletonNode(HandleRef jarg1);

		// Token: 0x06000765 RID: 1893
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkeletonNode_ResetAllSubBoneScaledWidth")]
		public static extern void CSSkeletonNode_ResetAllSubBoneScaledWidth(HandleRef jarg1);

		// Token: 0x06000766 RID: 1894
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkeletonNode_SetLength")]
		public static extern void CSSkeletonNode_SetLength(HandleRef jarg1, float jarg2);

		// Token: 0x06000767 RID: 1895
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSControlNodeDrawPen__SWIG_0")]
		public static extern IntPtr new_CSControlNodeDrawPen__SWIG_0();

		// Token: 0x06000768 RID: 1896
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSControlNodeDrawPen")]
		public static extern void delete_CSControlNodeDrawPen(HandleRef jarg1);

		// Token: 0x06000769 RID: 1897
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSControlNodeDrawPen__SWIG_1")]
		public static extern IntPtr new_CSControlNodeDrawPen__SWIG_1(HandleRef jarg1);

		// Token: 0x0600076A RID: 1898
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetDrawNodePen")]
		public static extern void CSControlNodeDrawPen_SetDrawNodePen(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600076B RID: 1899
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_ClearDraw")]
		public static extern void CSControlNodeDrawPen_ClearDraw(HandleRef jarg1);

		// Token: 0x0600076C RID: 1900
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetCenterPoint")]
		public static extern void CSControlNodeDrawPen_SetCenterPoint(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600076D RID: 1901
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetOperateState")]
		public static extern void CSControlNodeDrawPen_SetOperateState(HandleRef jarg1, int jarg2);

		// Token: 0x0600076E RID: 1902
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetOperateState")]
		public static extern int CSControlNodeDrawPen_GetOperateState(HandleRef jarg1);

		// Token: 0x0600076F RID: 1903
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetCollideLineWidth")]
		public static extern void CSControlNodeDrawPen_SetCollideLineWidth(HandleRef jarg1, float jarg2);

		// Token: 0x06000770 RID: 1904
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetCollideLine")]
		public static extern float CSControlNodeDrawPen_GetCollideLine(HandleRef jarg1);

		// Token: 0x06000771 RID: 1905
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetControlXLength")]
		public static extern void CSControlNodeDrawPen_SetControlXLength(HandleRef jarg1, float jarg2);

		// Token: 0x06000772 RID: 1906
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetControlXLength")]
		public static extern float CSControlNodeDrawPen_GetControlXLength(HandleRef jarg1);

		// Token: 0x06000773 RID: 1907
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_SetControlYLength")]
		public static extern void CSControlNodeDrawPen_SetControlYLength(HandleRef jarg1, float jarg2);

		// Token: 0x06000774 RID: 1908
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetControlYLength")]
		public static extern float CSControlNodeDrawPen_GetControlYLength(HandleRef jarg1);

		// Token: 0x06000775 RID: 1909
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_DrawControl")]
		public static extern void CSControlNodeDrawPen_DrawControl(HandleRef jarg1);

		// Token: 0x06000776 RID: 1910
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_PointAtControl")]
		public static extern int CSControlNodeDrawPen_PointAtControl(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000777 RID: 1911
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetSizeRectToWorldTransfrom")]
		public static extern IntPtr CSControlNodeDrawPen_GetSizeRectToWorldTransfrom(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000778 RID: 1912
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_GetNodeRotateToPen")]
		public static extern void CSControlNodeDrawPen_GetNodeRotateToPen(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000779 RID: 1913
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSControlNodeDrawPen_ResetDrawPenScale")]
		public static extern void CSControlNodeDrawPen_ResetDrawPenScale(HandleRef jarg1);

		// Token: 0x0600077A RID: 1914
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneRackDrawPen_DrawBoneRack")]
		public static extern void CSBoneRackDrawPen_DrawBoneRack(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, float jarg4);

		// Token: 0x0600077B RID: 1915
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneRackDrawPen_DrawArrowLine__SWIG_0")]
		public static extern void CSBoneRackDrawPen_DrawArrowLine__SWIG_0(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

		// Token: 0x0600077C RID: 1916
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneRackDrawPen_DrawArrowLine__SWIG_1")]
		public static extern void CSBoneRackDrawPen_DrawArrowLine__SWIG_1(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3, HandleRef jarg4);

		// Token: 0x0600077D RID: 1917
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSBoneRackDrawPen")]
		public static extern IntPtr new_CSBoneRackDrawPen();

		// Token: 0x0600077E RID: 1918
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSBoneRackDrawPen")]
		public static extern void delete_CSBoneRackDrawPen(HandleRef jarg1);

		// Token: 0x0600077F RID: 1919
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSRotationDrawPen")]
		public static extern IntPtr new_CSRotationDrawPen();

		// Token: 0x06000780 RID: 1920
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRotationDrawPen_DrawControl")]
		public static extern void CSRotationDrawPen_DrawControl(HandleRef jarg1);

		// Token: 0x06000781 RID: 1921
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRotationDrawPen_PointAtControl")]
		public static extern int CSRotationDrawPen_PointAtControl(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000782 RID: 1922
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSRotationDrawPen")]
		public static extern void delete_CSRotationDrawPen(HandleRef jarg1);

		// Token: 0x06000783 RID: 1923
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSTranslateNodeDrawPen")]
		public static extern IntPtr new_CSTranslateNodeDrawPen();

		// Token: 0x06000784 RID: 1924
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTranslateNodeDrawPen_SetXYAreaSize")]
		public static extern void CSTranslateNodeDrawPen_SetXYAreaSize(HandleRef jarg1, float jarg2);

		// Token: 0x06000785 RID: 1925
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTranslateNodeDrawPen_GetXYAreaSize")]
		public static extern float CSTranslateNodeDrawPen_GetXYAreaSize(HandleRef jarg1);

		// Token: 0x06000786 RID: 1926
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTranslateNodeDrawPen_DrawControl")]
		public static extern void CSTranslateNodeDrawPen_DrawControl(HandleRef jarg1);

		// Token: 0x06000787 RID: 1927
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTranslateNodeDrawPen_PointAtControl")]
		public static extern int CSTranslateNodeDrawPen_PointAtControl(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x06000788 RID: 1928
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSTranslateNodeDrawPen")]
		public static extern void delete_CSTranslateNodeDrawPen(HandleRef jarg1);

		// Token: 0x06000789 RID: 1929
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScaleNodeDrawPen_DrawControl")]
		public static extern void CSScaleNodeDrawPen_DrawControl(HandleRef jarg1);

		// Token: 0x0600078A RID: 1930
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScaleNodeDrawPen_PointAtControl")]
		public static extern int CSScaleNodeDrawPen_PointAtControl(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600078B RID: 1931
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScaleNodeDrawPen_StretchLine")]
		public static extern void CSScaleNodeDrawPen_StretchLine(HandleRef jarg1, HandleRef jarg2);

		// Token: 0x0600078C RID: 1932
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScaleNodeDrawPen_ResetLine")]
		public static extern void CSScaleNodeDrawPen_ResetLine(HandleRef jarg1);

		// Token: 0x0600078D RID: 1933
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_new_CSScaleNodeDrawPen")]
		public static extern IntPtr new_CSScaleNodeDrawPen();

		// Token: 0x0600078E RID: 1934
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_delete_CSScaleNodeDrawPen")]
		public static extern void delete_CSScaleNodeDrawPen(HandleRef jarg1);

		// Token: 0x0600078F RID: 1935
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSVisualObject_SWIGUpcast")]
		public static extern IntPtr CSVisualObject_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000790 RID: 1936
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCanvas_SWIGUpcast")]
		public static extern IntPtr CSCanvas_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000791 RID: 1937
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScene_SWIGUpcast")]
		public static extern IntPtr CSScene_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000792 RID: 1938
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode_SWIGUpcast")]
		public static extern IntPtr CSNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000793 RID: 1939
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode2D_SWIGUpcast")]
		public static extern IntPtr CSNode2D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000794 RID: 1940
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSNode3D_SWIGUpcast")]
		public static extern IntPtr CSNode3D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000795 RID: 1941
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLayer_SWIGUpcast")]
		public static extern IntPtr CSLayer_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000796 RID: 1942
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkyBox_SWIGUpcast")]
		public static extern IntPtr CSSkyBox_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000797 RID: 1943
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCamera_SWIGUpcast")]
		public static extern IntPtr CSCamera_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000798 RID: 1944
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSceneCamera_SWIGUpcast")]
		public static extern IntPtr CSSceneCamera_SWIGUpcast(IntPtr jarg1);

		// Token: 0x06000799 RID: 1945
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSUserCamera_SWIGUpcast")]
		public static extern IntPtr CSUserCamera_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079A RID: 1946
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDumyNode_SWIGUpcast")]
		public static extern IntPtr CSDumyNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079B RID: 1947
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite3D_SWIGUpcast")]
		public static extern IntPtr CSSprite3D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079C RID: 1948
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticle3D_SWIGUpcast")]
		public static extern IntPtr CSParticle3D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079D RID: 1949
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlice3D_SWIGUpcast")]
		public static extern IntPtr CSSlice3D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079E RID: 1950
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLight_SWIGUpcast")]
		public static extern IntPtr CSLight_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0600079F RID: 1951
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSprite_SWIGUpcast")]
		public static extern IntPtr CSSprite_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A0 RID: 1952
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGameMap_SWIGUpcast")]
		public static extern IntPtr CSGameMap_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A1 RID: 1953
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSParticleSystem_SWIGUpcast")]
		public static extern IntPtr CSParticleSystem_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A2 RID: 1954
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSProjectNode_SWIGUpcast")]
		public static extern IntPtr CSProjectNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A3 RID: 1955
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSimpleAudio_SWIGUpcast")]
		public static extern IntPtr CSSimpleAudio_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A4 RID: 1956
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSDrawNode_SWIGUpcast")]
		public static extern IntPtr CSDrawNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A5 RID: 1957
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode_SWIGUpcast")]
		public static extern IntPtr CSComControlNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A6 RID: 1958
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSComControlNode3D_SWIGUpcast")]
		public static extern IntPtr CSComControlNode3D_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A7 RID: 1959
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSWidget_SWIGUpcast")]
		public static extern IntPtr CSWidget_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A8 RID: 1960
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSButton_SWIGUpcast")]
		public static extern IntPtr CSButton_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007A9 RID: 1961
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSCheckBox_SWIGUpcast")]
		public static extern IntPtr CSCheckBox_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AA RID: 1962
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSImageView_SWIGUpcast")]
		public static extern IntPtr CSImageView_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AB RID: 1963
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSText_SWIGUpcast")]
		public static extern IntPtr CSText_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AC RID: 1964
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextAtlas_SWIGUpcast")]
		public static extern IntPtr CSTextAtlas_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AD RID: 1965
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextBMFont_SWIGUpcast")]
		public static extern IntPtr CSTextBMFont_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AE RID: 1966
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSLoadingBar_SWIGUpcast")]
		public static extern IntPtr CSLoadingBar_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007AF RID: 1967
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSlider_SWIGUpcast")]
		public static extern IntPtr CSSlider_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B0 RID: 1968
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTextField_SWIGUpcast")]
		public static extern IntPtr CSTextField_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B1 RID: 1969
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPanel_SWIGUpcast")]
		public static extern IntPtr CSPanel_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B2 RID: 1970
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScrollView_SWIGUpcast")]
		public static extern IntPtr CSScrollView_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B3 RID: 1971
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSPageView_SWIGUpcast")]
		public static extern IntPtr CSPageView_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B4 RID: 1972
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSListView_SWIGUpcast")]
		public static extern IntPtr CSListView_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B5 RID: 1973
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineFrame_SWIGUpcast")]
		public static extern IntPtr CSTimelineFrame_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B6 RID: 1974
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSExtensionFrame_SWIGUpcast")]
		public static extern IntPtr CSExtensionFrame_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B7 RID: 1975
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimeline_SWIGUpcast")]
		public static extern IntPtr CSTimeline_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B8 RID: 1976
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTimelineAction_SWIGUpcast")]
		public static extern IntPtr CSTimelineAction_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007B9 RID: 1977
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuidesService_SWIGUpcast")]
		public static extern IntPtr CSGuidesService_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BA RID: 1978
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSGuides_SWIGUpcast")]
		public static extern IntPtr CSGuides_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BB RID: 1979
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneNode_SWIGUpcast")]
		public static extern IntPtr CSBoneNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BC RID: 1980
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSSkeletonNode_SWIGUpcast")]
		public static extern IntPtr CSSkeletonNode_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BD RID: 1981
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSBoneRackDrawPen_SWIGUpcast")]
		public static extern IntPtr CSBoneRackDrawPen_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BE RID: 1982
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSRotationDrawPen_SWIGUpcast")]
		public static extern IntPtr CSRotationDrawPen_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007BF RID: 1983
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSTranslateNodeDrawPen_SWIGUpcast")]
		public static extern IntPtr CSTranslateNodeDrawPen_SWIGUpcast(IntPtr jarg1);

		// Token: 0x060007C0 RID: 1984
		[DllImport("CocoStudioEngineAdapter", EntryPoint = "CSharp_CSScaleNodeDrawPen_SWIGUpcast")]
		public static extern IntPtr CSScaleNodeDrawPen_SWIGUpcast(IntPtr jarg1);

		// Token: 0x0400002C RID: 44
		protected static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper swigExceptionHelper = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper();

		// Token: 0x0400002D RID: 45
		protected static CocoStudioEngineAdapterPINVOKE.SWIGStringHelper swigStringHelper = new CocoStudioEngineAdapterPINVOKE.SWIGStringHelper();

		// Token: 0x02000022 RID: 34
		protected class SWIGExceptionHelper
		{
			// Token: 0x060007C2 RID: 1986
			[DllImport("CocoStudioEngineAdapter")]
			public static extern void SWIGRegisterExceptionCallbacks_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemExceptionDelegate);

			// Token: 0x060007C3 RID: 1987
			[DllImport("CocoStudioEngineAdapter", EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_CocoStudioEngineAdapter")]
			public static extern void SWIGRegisterExceptionCallbacksArgument_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate);

			// Token: 0x060007C4 RID: 1988 RVA: 0x000071F5 File Offset: 0x000053F5
			private static void SetPendingApplicationException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new ApplicationException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x00007209 File Offset: 0x00005409
			private static void SetPendingArithmeticException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new ArithmeticException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x0000721D File Offset: 0x0000541D
			private static void SetPendingDivideByZeroException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new DivideByZeroException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007C7 RID: 1991 RVA: 0x00007231 File Offset: 0x00005431
			private static void SetPendingIndexOutOfRangeException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new IndexOutOfRangeException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007C8 RID: 1992 RVA: 0x00007245 File Offset: 0x00005445
			private static void SetPendingInvalidCastException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new InvalidCastException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007C9 RID: 1993 RVA: 0x00007259 File Offset: 0x00005459
			private static void SetPendingInvalidOperationException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new InvalidOperationException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x0000726D File Offset: 0x0000546D
			private static void SetPendingIOException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new IOException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CB RID: 1995 RVA: 0x00007281 File Offset: 0x00005481
			private static void SetPendingNullReferenceException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new NullReferenceException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x00007295 File Offset: 0x00005495
			private static void SetPendingOutOfMemoryException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new OutOfMemoryException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CD RID: 1997 RVA: 0x000072A9 File Offset: 0x000054A9
			private static void SetPendingOverflowException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new OverflowException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CE RID: 1998 RVA: 0x000072BD File Offset: 0x000054BD
			private static void SetPendingSystemException(string message)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new SystemException(message, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007CF RID: 1999 RVA: 0x000072D1 File Offset: 0x000054D1
			private static void SetPendingArgumentException(string message, string paramName)
			{
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new ArgumentException(message, paramName, CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve()));
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x000072E8 File Offset: 0x000054E8
			private static void SetPendingArgumentNullException(string message, string paramName)
			{
				Exception ex = CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new ArgumentNullException(paramName, message));
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x00007324 File Offset: 0x00005524
			private static void SetPendingArgumentOutOfRangeException(string message, string paramName)
			{
				Exception ex = CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
				if (ex != null)
				{
					message = message + " Inner Exception: " + ex.Message;
				}
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Set(new ArgumentOutOfRangeException(paramName, message));
			}

			// Token: 0x060007D2 RID: 2002 RVA: 0x00007360 File Offset: 0x00005560
			static SWIGExceptionHelper()
			{
				CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacks_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.applicationDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.arithmeticDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.divideByZeroDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.indexOutOfRangeDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.invalidCastDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.invalidOperationDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ioDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.nullReferenceDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.outOfMemoryDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.overflowDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.systemDelegate);
				CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacksArgument_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.argumentDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.argumentNullDelegate, CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.argumentOutOfRangeDelegate);
			}

			// Token: 0x0400002E RID: 46
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingApplicationException);

			// Token: 0x0400002F RID: 47
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingArithmeticException);

			// Token: 0x04000030 RID: 48
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingDivideByZeroException);

			// Token: 0x04000031 RID: 49
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingIndexOutOfRangeException);

			// Token: 0x04000032 RID: 50
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingInvalidCastException);

			// Token: 0x04000033 RID: 51
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingInvalidOperationException);

			// Token: 0x04000034 RID: 52
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingIOException);

			// Token: 0x04000035 RID: 53
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingNullReferenceException);

			// Token: 0x04000036 RID: 54
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingOutOfMemoryException);

			// Token: 0x04000037 RID: 55
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingOverflowException);

			// Token: 0x04000038 RID: 56
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate systemDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingSystemException);

			// Token: 0x04000039 RID: 57
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingArgumentException);

			// Token: 0x0400003A RID: 58
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingArgumentNullException);

			// Token: 0x0400003B RID: 59
			private static CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(CocoStudioEngineAdapterPINVOKE.SWIGExceptionHelper.SetPendingArgumentOutOfRangeException);

			// Token: 0x02000023 RID: 35
			// (Invoke) Token: 0x060007D5 RID: 2005
			public delegate void ExceptionDelegate(string message);

			// Token: 0x02000024 RID: 36
			// (Invoke) Token: 0x060007D9 RID: 2009
			public delegate void ExceptionArgumentDelegate(string message, string paramName);
		}

		// Token: 0x02000025 RID: 37
		public class SWIGPendingException
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x060007DC RID: 2012 RVA: 0x000074B8 File Offset: 0x000056B8
			public static bool Pending
			{
				get
				{
					bool result = false;
					if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.numExceptionsPending > 0 && CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException != null)
					{
						result = true;
					}
					return result;
				}
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x000074EC File Offset: 0x000056EC
			public static void Set(Exception e)
			{
				if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException != null)
				{
					throw new ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException.ToString() + ")", e);
				}
				CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException = e;
				lock (typeof(CocoStudioEngineAdapterPINVOKE))
				{
					CocoStudioEngineAdapterPINVOKE.SWIGPendingException.numExceptionsPending++;
				}
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00007574 File Offset: 0x00005774
			public static Exception Retrieve()
			{
				Exception result = null;
				if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.numExceptionsPending > 0)
				{
					if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException != null)
					{
						result = CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException;
						CocoStudioEngineAdapterPINVOKE.SWIGPendingException.pendingException = null;
						lock (typeof(CocoStudioEngineAdapterPINVOKE))
						{
							CocoStudioEngineAdapterPINVOKE.SWIGPendingException.numExceptionsPending--;
						}
					}
				}
				return result;
			}

			// Token: 0x0400003C RID: 60
			[ThreadStatic]
			private static Exception pendingException = null;

			// Token: 0x0400003D RID: 61
			private static int numExceptionsPending = 0;
		}

		// Token: 0x02000026 RID: 38
		protected class SWIGStringHelper
		{
			// Token: 0x060007E1 RID: 2017
			[DllImport("CocoStudioEngineAdapter")]
			public static extern void SWIGRegisterStringCallback_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate);

			// Token: 0x060007E2 RID: 2018 RVA: 0x00007618 File Offset: 0x00005818
			private static string CreateString(string cString)
			{
				return cString;
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x0000762B File Offset: 0x0000582B
			static SWIGStringHelper()
			{
				CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.SWIGRegisterStringCallback_CocoStudioEngineAdapter(CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.stringDelegate);
			}

			// Token: 0x0400003E RID: 62
			private static CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate = new CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.SWIGStringDelegate(CocoStudioEngineAdapterPINVOKE.SWIGStringHelper.CreateString);

			// Token: 0x02000027 RID: 39
			// (Invoke) Token: 0x060007E6 RID: 2022
			public delegate string SWIGStringDelegate(string message);
		}
	}
}
