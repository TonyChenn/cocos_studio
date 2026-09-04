using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;
using GLib;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000002 RID: 2
	public class CSCocosHelp : IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002078 File Offset: 0x00000278
		public static void ReloadPlistFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadPlistFileToCache(filePath);
				return false;
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D0 File Offset: 0x000002D0
		public static void ReloadPngFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadPngFileToCache(filePath);
				return false;
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002128 File Offset: 0x00000328
		public static void ReloadFntFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadFntFileToCache(filePath);
				return false;
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002188 File Offset: 0x00000388
		public static void RenamePngFileThreadSafe(string src, string dst)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.RenamePngFileToCache(src, dst);
				return false;
			});
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021E8 File Offset: 0x000003E8
		public static void RemovePngFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.RemovePngFileFromCache(filePath);
				return false;
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002217 File Offset: 0x00000417
		public CSCocosHelp(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002238 File Offset: 0x00000438
		public static HandleRef getCPtr(CSCocosHelp obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002264 File Offset: 0x00000464
		~CSCocosHelp()
		{
			this.Dispose();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022C8 File Offset: 0x000004C8
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						HandleRef handle = new HandleRef(null, this.swigCPtr.Handle);
						if (this.IsContainOpenGLResource())
						{
							GtkInvokeHelp.BeginInvoke(delegate
							{
								this.swigCPtr = handle;
								CocoStudioEngineAdapterPINVOKE.delete_CSCocosHelp(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSCocosHelp(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023C0 File Offset: 0x000005C0
		public CSCocosHelp() : this(CocoStudioEngineAdapterPINVOKE.new_CSCocosHelp(), true)
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023D4 File Offset: 0x000005D4
		public static string ConvertPath(string file)
		{
			string result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertPath(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002400 File Offset: 0x00000600
		public static void ClearResurceCache()
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ClearResurceCache();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000240C File Offset: 0x0000060C
		public static void LoadPListFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_LoadPListFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002434 File Offset: 0x00000634
		public static bool IsPngLoadedFromCache(string filePath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsPngLoadedFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002460 File Offset: 0x00000660
		public static void RenamePngFileToCache(string src, string dst)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RenamePngFileToCache(src, dst);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002488 File Offset: 0x00000688
		public static void ReloadPngFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadPngFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024B0 File Offset: 0x000006B0
		public static void ReloadPlistFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadPlistFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024D8 File Offset: 0x000006D8
		public static void ReloadFntFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadFntFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002500 File Offset: 0x00000700
		public static void UnloadTTFFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_UnloadTTFFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002528 File Offset: 0x00000728
		public static void RemovePngFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RemovePngFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002550 File Offset: 0x00000750
		public static void RemovePlistFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RemovePlistFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002578 File Offset: 0x00000778
		public static CSVectorString GetTmxMapImageArray(string filePath)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetTmxMapImageArray(filePath);
			CSVectorString result = (intPtr == IntPtr.Zero) ? null : new CSVectorString(intPtr, false);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025C0 File Offset: 0x000007C0
		public static CSVectorString GetMeshResourceArray(string filePath)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetMeshResourceArray(filePath);
			CSVectorString result = (intPtr == IntPtr.Zero) ? null : new CSVectorString(intPtr, false);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002608 File Offset: 0x00000808
		public static CSVectorString GetSprite3DTextureArray(string filePath)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetSprite3DTextureArray(filePath);
			CSVectorString result = (intPtr == IntPtr.Zero) ? null : new CSVectorString(intPtr, false);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002650 File Offset: 0x00000850
		public static string ConvertToBinProto(string des, string src, string res)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertToBinProto(des, src, res);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000266C File Offset: 0x0000086C
		public static string ConvertToBinByFlat(string des, string src, string res)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertToBinByFlat(des, src, res);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002688 File Offset: 0x00000888
		public static void RefreshLayoutSystemState(bool state)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RefreshLayoutSystemState(state);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002694 File Offset: 0x00000894
		public static void SetResourcePath(string resourceDir)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_SetResourcePath(resourceDir);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000026BC File Offset: 0x000008BC
		public static void AddSearchPath(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_AddSearchPath(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026E4 File Offset: 0x000008E4
		public static bool CheckOpenGLVersion()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckOpenGLVersion();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002700 File Offset: 0x00000900
		public static bool CheckBMFontResource(string fntPath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckBMFontResource(fntPath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000272C File Offset: 0x0000092C
		public static bool CheckImageFormat(string filePath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckImageFormat(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002758 File Offset: 0x00000958
		public static bool IsEngineInitialized()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsEngineInitialized();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002774 File Offset: 0x00000974
		public static void SetLocalPath(string path)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_SetLocalPath(path);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000279C File Offset: 0x0000099C
		public static IntPtr GetNodeFromScript(ScriptFileData scriptFileData)
		{
			IntPtr result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromScript(CSScriptFileData.getCPtr(new CSScriptFileData(scriptFileData.ScriptFile, (CSScriptFileData.CSScriptType)scriptFileData.FileType)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027E0 File Offset: 0x000009E0
		public static IntPtr GetNodeFromJS(string jsFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromJS(jsFileName);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000027FC File Offset: 0x000009FC
		public static IntPtr GetNodeFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromLua(luaFileName);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002818 File Offset: 0x00000A18
		public static string GetBaseTypeFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetBaseTypeFromLua(luaFileName);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002834 File Offset: 0x00000A34
		public static bool IsUseDefaultRenderFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsUseDefaultRenderFromLua(luaFileName);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000284E File Offset: 0x00000A4E
		public static void StopAllEffects()
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_StopAllEffects();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002858 File Offset: 0x00000A58
		public static bool CheckSprite3DFile(string file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckSprite3DFile(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002884 File Offset: 0x00000A84
		public static bool CheckParticle3DFile(string file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckParticle3DFile(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		private HandleRef swigCPtr;

		// Token: 0x04000002 RID: 2
		protected bool swigCMemOwn;
	}
}
