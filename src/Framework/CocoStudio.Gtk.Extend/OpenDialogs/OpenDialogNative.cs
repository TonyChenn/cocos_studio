using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	public class OpenDialogNative : NativeWindow, IDisposable
	{
		public BaseDialogNative BaseDialogNative { get; private set; }

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

		private void BaseDialogNative_FileNameChanged(BaseDialogNative sender, string filePath)
		{
			if (this.mSourceControl != null)
			{
				this.mSourceControl.OnFileNameChanged(filePath);
			}
		}

		private void BaseDialogNative_FolderNameChanged(BaseDialogNative sender, string folderName)
		{
			if (this.mSourceControl != null)
			{
				this.mSourceControl.OnFolderNameChanged(folderName);
			}
		}

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

		private void PopulateWindowsHandlers()
		{
			NativeMethods.EnumChildWindows(this.mOpenDialogHandle, new NativeMethods.EnumWindowsCallBack(this.OpenFileDialogEnumWindowCallBack), 1);
		}

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

		public string Lable_OpenFloder = "文件夹:";

		public string Lable_SelectTexg = "选择";

		public string Lable_CancelText = "取消";

		private SetWindowPosFlags UFLAGSSIZE = (SetWindowPosFlags)530;

		private SetWindowPosFlags UFLAGSHIDE = (SetWindowPosFlags)659;

		private SetWindowPosFlags UFLAGSZORDER = (SetWindowPosFlags)19;

		private Size mOriginalSize;

		private IntPtr mOpenDialogHandle;

		private IntPtr mListViewPtr;

		private WINDOWINFO mListViewInfo;

		private IntPtr mComboFolders;

		private WINDOWINFO mComboFoldersInfo;

		private IntPtr mGroupButtons;

		private WINDOWINFO mGroupButtonsInfo;

		private IntPtr mComboFileName;

		private WINDOWINFO mComboFileNameInfo;

		private IntPtr mComboExtensions;

		private WINDOWINFO mComboExtensionsInfo;

		private IntPtr mOpenButton;

		private WINDOWINFO mOpenButtonInfo;

		private IntPtr mCancelButton;

		private WINDOWINFO mCancelButtonInfo;

		private IntPtr mHelpButton;

		private WINDOWINFO mHelpButtonInfo;

		private OpenFileDialogEx mSourceControl;

		private IntPtr mToolBarFolders;

		private WINDOWINFO mToolBarFoldersInfo;

		private IntPtr mLabelFileName;

		private WINDOWINFO mLabelFileNameInfo;

		private IntPtr mLabelFileType;

		private WINDOWINFO mLabelFileTypeInfo;

		private IntPtr mChkReadOnly;

		private WINDOWINFO mChkReadOnlyInfo;

		private bool mIsClosing = false;

		private bool mInitializated = false;

		private RECT mOpenDialogWindowRect = default(RECT);

		private RECT mOpenDialogClientRect = default(RECT);

		private IntPtr ParentHandle;

		private IntPtr ptr_SelecteFolder;
	}
}
