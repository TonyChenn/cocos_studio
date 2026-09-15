using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;
using GLib;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSCocosHelp : IDisposable
	{
		public static void ReloadPlistFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadPlistFileToCache(filePath);
				return false;
			});
		}

		public static void ReloadPngFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadPngFileToCache(filePath);
				return false;
			});
		}

		public static void ReloadFntFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.ReloadFntFileToCache(filePath);
				return false;
			});
		}

		public static void RenamePngFileThreadSafe(string src, string dst)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.RenamePngFileToCache(src, dst);
				return false;
			});
		}

		public static void RemovePngFileThreadSafe(string filePath)
		{
			Timeout.Add(0U, delegate
			{
				CSCocosHelp.RemovePngFileFromCache(filePath);
				return false;
			});
		}

		public CSCocosHelp(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSCocosHelp obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSCocosHelp()
		{
			this.Dispose();
		}

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

		public CSCocosHelp() : this(CocoStudioEngineAdapterPINVOKE.new_CSCocosHelp(), true)
		{
		}

		public static string ConvertPath(string file)
		{
			string result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertPath(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static void ClearResurceCache()
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ClearResurceCache();
		}

		public static void LoadPListFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_LoadPListFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static bool IsPngLoadedFromCache(string filePath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsPngLoadedFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static void RenamePngFileToCache(string src, string dst)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RenamePngFileToCache(src, dst);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void ReloadPngFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadPngFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void ReloadPlistFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadPlistFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void ReloadFntFileToCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ReloadFntFileToCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void UnloadTTFFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_UnloadTTFFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void RemovePngFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RemovePngFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void RemovePlistFileFromCache(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RemovePlistFileFromCache(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

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

		public static string ConvertToBinProto(string des, string src, string res)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertToBinProto(des, src, res);
		}

		public static string ConvertToBinByFlat(string des, string src, string res)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_ConvertToBinByFlat(des, src, res);
		}

		public static void RefreshLayoutSystemState(bool state)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_RefreshLayoutSystemState(state);
		}

		public static void SetResourcePath(string resourceDir)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_SetResourcePath(resourceDir);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void AddSearchPath(string filePath)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_AddSearchPath(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static bool CheckOpenGLVersion()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckOpenGLVersion();
		}

		public static bool CheckBMFontResource(string fntPath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckBMFontResource(fntPath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool CheckImageFormat(string filePath)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckImageFormat(filePath);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool IsEngineInitialized()
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsEngineInitialized();
		}

		public static void SetLocalPath(string path)
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_SetLocalPath(path);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static IntPtr GetNodeFromScript(ScriptFileData scriptFileData)
		{
			IntPtr result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromScript(CSScriptFileData.getCPtr(new CSScriptFileData(scriptFileData.ScriptFile, (CSScriptFileData.CSScriptType)scriptFileData.FileType)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static IntPtr GetNodeFromJS(string jsFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromJS(jsFileName);
		}

		public static IntPtr GetNodeFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetNodeFromLua(luaFileName);
		}

		public static string GetBaseTypeFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_GetBaseTypeFromLua(luaFileName);
		}

		public static bool IsUseDefaultRenderFromLua(string luaFileName)
		{
			return CocoStudioEngineAdapterPINVOKE.CSCocosHelp_IsUseDefaultRenderFromLua(luaFileName);
		}

		public static void StopAllEffects()
		{
			CocoStudioEngineAdapterPINVOKE.CSCocosHelp_StopAllEffects();
		}

		public static bool CheckSprite3DFile(string file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckSprite3DFile(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool CheckParticle3DFile(string file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSCocosHelp_CheckParticle3DFile(file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
