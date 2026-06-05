using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	// Token: 0x0200002C RID: 44
	public class OpenDialogNative : NativeWindow, IDisposable
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006478 File Offset: 0x00004678
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000648F File Offset: 0x0000468F
		public BaseDialogNative BaseDialogNative { get; private set; }

		// Token: 0x0600013C RID: 316 RVA: 0x00006498 File Offset: 0x00004698
		public OpenDialogNative(IntPtr handle, OpenFileDialogEx sourceControl, IntPtr parentHandle, string lable_OpenFloder, string lable_SelectTexg, string lable_CancelText)
		{
			this.Lable_OpenFloder = lable_OpenFloder;
			this.Lable_SelectTexg = lable_SelectTexg;
			this.Lable_CancelText = lable_CancelText;
			this.ParentHandle = parentHandle;
			this.mOpenDialogHandle = handle;
			this.mSourceControl = sourceControl;
			base.AssignHandle(this.mOpenDialogHandle);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006550 File Offset: 0x00004750
		private void BaseDialogNative_FileNameChanged(BaseDialogNative sender, string filePath)
		{
			if (this.mSourceControl != null)
			{
				this.mSourceControl.OnFileNameChanged(filePath);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000657C File Offset: 0x0000477C
		private void BaseDialogNative_FolderNameChanged(BaseDialogNative sender, string folderName)
		{
			if (this.mSourceControl != null)
			{
				this.mSourceControl.OnFolderNameChanged(folderName);
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000065A8 File Offset: 0x000047A8
		private void BaseDialogNative_FilesSelectedChanged(BaseDialogNative sender, IntPtr handle)
		{
			IntPtr parent = NativeMethods.GetParent(handle);
			IntPtr sysListView32Handle = parent.GetSysListView32Handle();
			if (sysListView32Handle != IntPtr.Zero)
			{
				List<string> slectedItemsText = sysListView32Handle.GetSlectedItemsText(0);
				if (slectedItemsText != null && slectedItemsText.Count > 0)
				{
					NativeMethods.EnableWindow(this.ptr_SelecteFolder, 1);
				}
				else
				{
					NativeMethods.EnableWindow(this.ptr_SelecteFolder, 0);
				}
			}
			if (this.mSourceControl != null)
			{
				this.mSourceControl.OnFilesSelectedHandle(handle);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006630 File Offset: 0x00004830
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00006648 File Offset: 0x00004848
		public bool IsClosing
		{
			get
			{
				return this.mIsClosing;
			}
			set
			{
				this.mIsClosing = value;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006654 File Offset: 0x00004854
		public void Dispose()
		{
			this.ReleaseHandle();
			if (this.BaseDialogNative != null)
			{
				this.BaseDialogNative.FileNameChanged -= this.BaseDialogNative_FileNameChanged;
				this.BaseDialogNative.FolderNameChanged -= this.BaseDialogNative_FolderNameChanged;
				this.BaseDialogNative.FilesSelected -= this.BaseDialogNative_FilesSelectedChanged;
				this.BaseDialogNative.Dispose();
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000066CC File Offset: 0x000048CC
		private void PopulateWindowsHandlers()
		{
			NativeMethods.EnumChildWindows(this.mOpenDialogHandle, new NativeMethods.EnumWindowsCallBack(this.OpenFileDialogEnumWindowCallBack), 1);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000066E8 File Offset: 0x000048E8
		private bool OpenFileDialogEnumWindowCallBack(IntPtr hwnd, int lParam)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, stringBuilder, stringBuilder.Capacity);
			int dlgCtrlID = NativeMethods.GetDlgCtrlID(hwnd);
			WINDOWINFO windowinfo;
			NativeMethods.GetWindowInfo(hwnd, out windowinfo);
			bool result;
			if (stringBuilder.ToString().StartsWith("#32770"))
			{
				this.BaseDialogNative = new BaseDialogNative(hwnd);
				this.BaseDialogNative.FileNameChanged += this.BaseDialogNative_FileNameChanged;
				this.BaseDialogNative.FolderNameChanged += this.BaseDialogNative_FolderNameChanged;
				this.BaseDialogNative.FilesSelected += this.BaseDialogNative_FilesSelectedChanged;
				result = true;
			}
			else
			{
				ControlsID controlsID = (ControlsID)dlgCtrlID;
				if (controlsID <= ControlsID.LabelFileName)
				{
					switch (controlsID)
					{
					case ControlsID.ButtonOpen:
						this.mOpenButton = hwnd;
						this.mOpenButtonInfo = windowinfo;
						break;
					case ControlsID.ButtonCancel:
						this.mCancelButton = hwnd;
						this.mCancelButtonInfo = windowinfo;
						NativeMethods.SetWindowText(hwnd, this.Lable_CancelText);
						break;
					default:
						switch (controlsID)
						{
						case ControlsID.ButtonHelp:
							this.mHelpButton = hwnd;
							this.mHelpButtonInfo = windowinfo;
							this.ptr_SelecteFolder = hwnd;
							NativeMethods.SetWindowText(hwnd, this.Lable_SelectTexg);
							NativeMethods.EnableWindow(hwnd, 0);
							break;
						case (ControlsID)1039:
							break;
						case ControlsID.CheckBoxReadOnly:
							this.mChkReadOnly = hwnd;
							this.mChkReadOnlyInfo = windowinfo;
							NativeMethods.ShowWindow(hwnd, 0);
							break;
						default:
							switch (controlsID)
							{
							case ControlsID.GroupFolder:
								this.mGroupButtons = hwnd;
								this.mGroupButtonsInfo = windowinfo;
								break;
							case ControlsID.LabelFileType:
								this.mLabelFileType = hwnd;
								this.mLabelFileTypeInfo = windowinfo;
								NativeMethods.ShowWindow(hwnd, 0);
								break;
							case ControlsID.LabelFileName:
								this.mLabelFileName = hwnd;
								this.mLabelFileNameInfo = windowinfo;
								NativeMethods.SetWindowText(hwnd, this.Lable_OpenFloder);
								break;
							}
							break;
						}
						break;
					}
				}
				else if (controlsID <= ControlsID.ComboFolder)
				{
					if (controlsID != ControlsID.DefaultView)
					{
						switch (controlsID)
						{
						case ControlsID.ComboFileType:
							this.mComboExtensions = hwnd;
							this.mComboExtensionsInfo = windowinfo;
							NativeMethods.ShowWindow(hwnd, 0);
							break;
						case ControlsID.ComboFolder:
							this.mComboFolders = hwnd;
							this.mComboFoldersInfo = windowinfo;
							break;
						}
					}
					else
					{
						this.mListViewPtr = hwnd;
						NativeMethods.GetWindowInfo(hwnd, out this.mListViewInfo);
						if (this.mSourceControl.DefaultViewMode != FolderViewMode.Default)
						{
							NativeMethods.SendMessage(this.mListViewPtr, 273, (int)this.mSourceControl.DefaultViewMode, 0);
						}
					}
				}
				else if (controlsID != ControlsID.ComboFileName)
				{
					if (controlsID == ControlsID.LeftToolBar)
					{
						this.mToolBarFolders = hwnd;
						this.mToolBarFoldersInfo = windowinfo;
					}
				}
				else if (stringBuilder.ToString().ToLower() == "comboboxex32")
				{
					this.mComboFileName = hwnd;
					this.mComboFileNameInfo = windowinfo;
					NativeMethods.ShowWindow(hwnd, 0);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000069C4 File Offset: 0x00004BC4
		private void InitControls()
		{
			this.mInitializated = true;
			NativeMethods.GetClientRect(this.mOpenDialogHandle, ref this.mOpenDialogClientRect);
			NativeMethods.GetWindowRect(this.mOpenDialogHandle, ref this.mOpenDialogWindowRect);
			this.PopulateWindowsHandlers();
			this.SetCustomPosition(true);
			NativeMethods.SetParent(this.mSourceControl.Handle, this.mOpenDialogHandle);
			NativeMethods.SetWindowPos(this.mSourceControl.Handle, (IntPtr)1L, 0, 0, 0, 0, this.UFLAGSZORDER);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006A48 File Offset: 0x00004C48
		private void SetCustomPosition(bool islocation = false)
		{
			RECT rect = default(RECT);
			NativeMethods.GetWindowRect(this.mComboFileName, ref rect);
			RECT rect2 = default(RECT);
			NativeMethods.GetWindowRect(this.mOpenDialogHandle, ref rect2);
			uint x = rect.left - rect2.left - 8U;
			uint num = rect.top - rect2.top - 30U;
			if (islocation)
			{
				this.mSourceControl.Location = new Point((int)x, (int)num);
				RECT rect3 = default(RECT);
				NativeMethods.GetWindowRect(this.mOpenButton, ref rect3);
				uint x2 = rect3.left - rect2.left - 8U;
				uint y = num;
				NativeMethods.SetWindowPos(this.mHelpButton, (IntPtr)0, (int)x2, (int)y, (int)rect3.Width, (int)rect3.Height, SetWindowPosFlags.SWP_SHOWWINDOW);
				NativeMethods.ShowWindow(this.mOpenButton, 0);
			}
			else
			{
				this.mSourceControl.Width = (int)rect.Width;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006B3C File Offset: 0x00004D3C
		protected override void WndProc(ref Message m)
		{
			bool flag = false;
			int msg = m.Msg;
			if (msg <= 24)
			{
				if (msg == 2)
				{
					if (!this.mIsClosing)
					{
						this.mIsClosing = true;
						NativeMethods.SetWindowPos(this.mOpenDialogHandle, IntPtr.Zero, 0, 0, 0, 0, this.UFLAGSHIDE);
						NativeMethods.GetWindowRect(this.mOpenDialogHandle, ref this.mOpenDialogWindowRect);
						NativeMethods.SetWindowPos(this.mOpenDialogHandle, IntPtr.Zero, (int)this.mOpenDialogWindowRect.left, (int)this.mOpenDialogWindowRect.top, this.mOriginalSize.Width, this.mOriginalSize.Height, this.UFLAGSSIZE);
					}
					goto IL_268;
				}
				if (msg == 5)
				{
					flag = true;
					goto IL_268;
				}
				if (msg != 24)
				{
					goto IL_268;
				}
			}
			else if (msg != 36)
			{
				if (msg == 70)
				{
					if (!this.mIsClosing)
					{
						if (!this.mInitializated)
						{
							float num = 96f;
							using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
							{
								num = graphics.DpiX;
								float dpiY = graphics.DpiY;
							}
							WINDOWPOS windowpos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
							windowpos.cy = (int)(400f * (num / 96f));
							Marshal.StructureToPtr(windowpos, m.LParam, true);
						}
						if (!this.mComboFileName.Equals(IntPtr.Zero))
						{
							this.SetCustomPosition(false);
						}
					}
					goto IL_268;
				}
				if (msg != 642)
				{
					goto IL_268;
				}
				if (m.WParam == (IntPtr)1L && !this.mIsClosing)
				{
					this.mIsClosing = true;
					this.mSourceControl.OnClosingDialog();
					NativeMethods.SetWindowPos(this.mOpenDialogHandle, IntPtr.Zero, 0, 0, 0, 0, this.UFLAGSHIDE);
					NativeMethods.GetWindowRect(this.mOpenDialogHandle, ref this.mOpenDialogWindowRect);
					NativeMethods.SetWindowPos(this.mOpenDialogHandle, IntPtr.Zero, (int)this.mOpenDialogWindowRect.left, (int)this.mOpenDialogWindowRect.top, this.mOriginalSize.Width, this.mOriginalSize.Height, this.UFLAGSSIZE);
				}
				goto IL_268;
			}
			this.mInitializated = true;
			this.InitControls();
			IL_268:
			base.WndProc(ref m);
			if (flag)
			{
				if (!this.mComboFileName.Equals(IntPtr.Zero))
				{
					this.SetCustomPosition(false);
				}
			}
		}

		// Token: 0x0400009F RID: 159
		public string Lable_OpenFloder = "文件夹:";

		// Token: 0x040000A0 RID: 160
		public string Lable_SelectTexg = "选择";

		// Token: 0x040000A1 RID: 161
		public string Lable_CancelText = "取消";

		// Token: 0x040000A2 RID: 162
		private SetWindowPosFlags UFLAGSSIZE = (SetWindowPosFlags)530;

		// Token: 0x040000A3 RID: 163
		private SetWindowPosFlags UFLAGSHIDE = (SetWindowPosFlags)659;

		// Token: 0x040000A4 RID: 164
		private SetWindowPosFlags UFLAGSZORDER = (SetWindowPosFlags)19;

		// Token: 0x040000A5 RID: 165
		private Size mOriginalSize;

		// Token: 0x040000A6 RID: 166
		private IntPtr mOpenDialogHandle;

		// Token: 0x040000A7 RID: 167
		private IntPtr mListViewPtr;

		// Token: 0x040000A8 RID: 168
		private WINDOWINFO mListViewInfo;

		// Token: 0x040000A9 RID: 169
		private IntPtr mComboFolders;

		// Token: 0x040000AA RID: 170
		private WINDOWINFO mComboFoldersInfo;

		// Token: 0x040000AB RID: 171
		private IntPtr mGroupButtons;

		// Token: 0x040000AC RID: 172
		private WINDOWINFO mGroupButtonsInfo;

		// Token: 0x040000AD RID: 173
		private IntPtr mComboFileName;

		// Token: 0x040000AE RID: 174
		private WINDOWINFO mComboFileNameInfo;

		// Token: 0x040000AF RID: 175
		private IntPtr mComboExtensions;

		// Token: 0x040000B0 RID: 176
		private WINDOWINFO mComboExtensionsInfo;

		// Token: 0x040000B1 RID: 177
		private IntPtr mOpenButton;

		// Token: 0x040000B2 RID: 178
		private WINDOWINFO mOpenButtonInfo;

		// Token: 0x040000B3 RID: 179
		private IntPtr mCancelButton;

		// Token: 0x040000B4 RID: 180
		private WINDOWINFO mCancelButtonInfo;

		// Token: 0x040000B5 RID: 181
		private IntPtr mHelpButton;

		// Token: 0x040000B6 RID: 182
		private WINDOWINFO mHelpButtonInfo;

		// Token: 0x040000B7 RID: 183
		private OpenFileDialogEx mSourceControl;

		// Token: 0x040000B8 RID: 184
		private IntPtr mToolBarFolders;

		// Token: 0x040000B9 RID: 185
		private WINDOWINFO mToolBarFoldersInfo;

		// Token: 0x040000BA RID: 186
		private IntPtr mLabelFileName;

		// Token: 0x040000BB RID: 187
		private WINDOWINFO mLabelFileNameInfo;

		// Token: 0x040000BC RID: 188
		private IntPtr mLabelFileType;

		// Token: 0x040000BD RID: 189
		private WINDOWINFO mLabelFileTypeInfo;

		// Token: 0x040000BE RID: 190
		private IntPtr mChkReadOnly;

		// Token: 0x040000BF RID: 191
		private WINDOWINFO mChkReadOnlyInfo;

		// Token: 0x040000C0 RID: 192
		private bool mIsClosing = false;

		// Token: 0x040000C1 RID: 193
		private bool mInitializated = false;

		// Token: 0x040000C2 RID: 194
		private RECT mOpenDialogWindowRect = default(RECT);

		// Token: 0x040000C3 RID: 195
		private RECT mOpenDialogClientRect = default(RECT);

		// Token: 0x040000C4 RID: 196
		private IntPtr ParentHandle;

		// Token: 0x040000C5 RID: 197
		private IntPtr ptr_SelecteFolder;
	}
}
