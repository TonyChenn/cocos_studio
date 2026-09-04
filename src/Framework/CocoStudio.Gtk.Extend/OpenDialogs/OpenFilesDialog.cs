using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	// Token: 0x02000030 RID: 48
	public class OpenFilesDialog : OpenFileDialogEx
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600016F RID: 367 RVA: 0x000074B4 File Offset: 0x000056B4
		public override string Lable_SelectTexg
		{
			get
			{
				return "选择";
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000074CC File Offset: 0x000056CC
		public OpenFilesDialog()
		{
			this.InitializeComponent();
			base.OpenDialog.Multiselect = true;
			base.OpenDialog.Filter = "All Files|*.*";
			base.FilesSelected += this.OpenFilesDialog_FilesSelected;
			base.FileNameChanged += this.OpenFilesDialog_FileNameChanged;
			base.FolderNameChanged += this.OpenFilesDialog_FolderNameChanged;
			base.OpenDialog.FileOk += this.OpenDialog_FileOk;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007587 File Offset: 0x00005787
		private void OpenDialog_FileOk(object sender, CancelEventArgs e)
		{
			this.OnClosingDialog();
			this.OnSelectedNameChanged();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007598 File Offset: 0x00005798
		private void OpenFilesDialog_FolderNameChanged(OpenFileDialogEx sender, string filePath)
		{
			this.cando = true;
			this.fileConvert.Clear();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000075F4 File Offset: 0x000057F4
		private void OpenFilesDialog_FileNameChanged(OpenFileDialogEx sender, string filePath)
		{
			this.cando = true;
			this.fileConvert.Clear();
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				if (filePath.IsFileOrDirectory())
				{
					this.FolderParent = string.Empty;
					this.fileConvert.Add(filePath);
				}
				else
				{
					int num = filePath.LastIndexOf("\\");
					if (num != -1 && num <= filePath.Length)
					{
						string text = new string(filePath.Skip(num).ToArray<char>());
						string[] array = text.Split(new string[]
						{
							"\\",
							"/",
							"\""
						}, StringSplitOptions.RemoveEmptyEntries);
						if (array != null && array.Count<string>() > 0)
						{
							string value = array.FirstOrDefault((string i) => !string.IsNullOrWhiteSpace(i));
							int num2 = filePath.IndexOf(value);
							this.FolderParent = ((num2 != -1) ? new string(filePath.Take(num2 - 1).ToArray<char>()) : string.Empty);
							array.ToList<string>().ForEach(delegate(string i)
							{
								if (!string.IsNullOrWhiteSpace(i))
								{
									this.fileConvert.Add(i);
								}
							});
						}
					}
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000777C File Offset: 0x0000597C
		private void OpenFilesDialog_FilesSelected(OpenFileDialogEx sender, IntPtr handle)
		{
			if (this.cando)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				NativeMethods.SendMessage(NativeMethods.GetParent(handle), 1126, 256, stringBuilder);
				if (!string.IsNullOrWhiteSpace(stringBuilder.ToString()))
				{
					this.FolderParent = stringBuilder.ToString();
				}
				this.FilesPath.Clear();
				List<string> list = new List<string>();
				this.textBox1.Text = string.Empty;
				IntPtr parent = NativeMethods.GetParent(handle);
				IntPtr sysListView32Handle = parent.GetSysListView32Handle();
				if (sysListView32Handle != IntPtr.Zero)
				{
					List<string> list2 = sysListView32Handle.GetSlectedItemsText(0);
					if (list2 != null && list2.Count > 0)
					{
						if (list2.Count > 1)
						{
							list2 = (from i in list2
							orderby i
							select i).ToList<string>();
							if (this.fileConvert.Count > 0)
							{
								this.fileConvert = (from i in this.fileConvert
								orderby i
								select i).ToList<string>();
							}
							int num = 0;
							int count = list2.Count;
							for (int j = 0; j < count; j++)
							{
								string text = list2[j].Split(new string[]
								{
									"\0"
								}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>();
								string path = Path.Combine(this.FolderParent, text);
								if (!Directory.Exists(path))
								{
									string folderParent = this.FolderParent;
									string text2 = string.Empty;
									if (!string.IsNullOrWhiteSpace(this.FolderParent))
									{
										string text3 = Path.Combine(this.FolderParent, text);
										try
										{
											FileInfo fileInfo = new FileInfo(text3);
											if (fileInfo != null && string.IsNullOrWhiteSpace(fileInfo.Extension))
											{
												text3 += ".lnk";
											}
											if (fileInfo != null && !fileInfo.Exists)
											{
												if (this.fileConvert.Count > num)
												{
													if (!Directory.Exists(this.fileConvert[num]))
													{
														text3 = this.fileConvert[num];
													}
												}
											}
											text2 = this.CheckFile(text, text3, ref folderParent);
										}
										catch
										{
										}
									}
									else if (this.fileConvert.Count > num)
									{
										text2 = this.CheckFile(text, this.fileConvert[num], ref folderParent);
									}
									if (!string.IsNullOrWhiteSpace(text2))
									{
										list.Add("\"" + text2 + "\"");
										num++;
									}
								}
								else
								{
									string item = Path.Combine(this.FolderParent, text);
									if (!this.FilesPath.Contains(item))
									{
										this.FilesPath.Add(item);
										list.Add("\"" + text + "\"");
									}
								}
							}
							string text4 = string.Join(" ", list.ToArray());
							if (list.Count == 1)
							{
								text4 = text4.Replace("\"", "");
							}
							this.textBox1.Text = text4;
						}
						else if (this.fileConvert.Count == 1)
						{
							this.textBox1.Text = this.CheckFile(list2[0], this.fileConvert[0], ref this.FolderParent);
						}
					}
				}
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007B88 File Offset: 0x00005D88
		private string CheckFile(string selettext, string converttext, ref string floderparent)
		{
			string text = selettext.Split(new string[]
			{
				"\0"
			}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>();
			bool flag = false;
			string text2 = converttext;
			FileInfo fileInfo = new FileInfo(text2);
			if (fileInfo.Extension.ToLower() == ".lnk")
			{
				if (!fileInfo.Exists)
				{
					string allUsersDesktopFolderPath = WindowHelper.GetAllUsersDesktopFolderPath();
					string fileName = Path.Combine(allUsersDesktopFolderPath, text + ".lnk");
					fileInfo = new FileInfo(fileName);
				}
				if (fileInfo.Exists)
				{
					flag = true;
					text2 = fileInfo.FullName.GetLnkRealtivePath();
				}
			}
			if (string.IsNullOrWhiteSpace(floderparent))
			{
				if (text2.IsFileOrDirectory())
				{
					DirectoryInfo directoryInfo = new FileInfo(text2).Directory;
					if (directoryInfo == null)
					{
						directoryInfo = new DirectoryInfo(text2);
					}
					if (directoryInfo != null && directoryInfo.Exists)
					{
						floderparent = directoryInfo.FullName;
						this.FilesPath.Add(Path.Combine(floderparent, text2));
						return text;
					}
				}
			}
			else
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				if (floderparent.Equals(folderPath))
				{
					if (flag)
					{
						FileInfo fileInfo2 = new FileInfo(text2);
						if (fileInfo2 != null && fileInfo2.Exists)
						{
							this.FilesPath.Add(text2);
							return fileInfo2.Name;
						}
					}
					else if (text2.IsFileOrDirectory())
					{
						this.FilesPath.Add(Path.Combine(floderparent, text2));
						return text;
					}
				}
				else if (text2.IsFileOrDirectory())
				{
					this.FilesPath.Add(Path.Combine(floderparent, text2));
					return text;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007DAC File Offset: 0x00005FAC
		public override void OnSelectedNameChanged()
		{
			if (this.FilesPath.Count > 0)
			{
				List<string> filesPath = new List<string>();
				this.FilesPath.ForEach(delegate(string i)
				{
					filesPath.Add(i.Replace("\"", ""));
				});
				this.FilesPath = filesPath;
			}
			base.OnSelectedNameChanged();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007E10 File Offset: 0x00006010
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007E48 File Offset: 0x00006048
		private void InitializeComponent()
		{
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
		}

		// Token: 0x040000D5 RID: 213
		public string FolderParent = string.Empty;

		// Token: 0x040000D6 RID: 214
		public List<string> FilesPath = new List<string>();

		// Token: 0x040000D7 RID: 215
		private List<string> fileConvert = new List<string>();

		// Token: 0x040000D8 RID: 216
		private bool cando = false;

		// Token: 0x040000D9 RID: 217
		private IContainer components = null;
	}
}
