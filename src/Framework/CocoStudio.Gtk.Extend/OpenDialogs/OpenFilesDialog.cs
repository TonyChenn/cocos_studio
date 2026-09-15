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
	public class OpenFilesDialog : OpenFileDialogEx
	{
		public override string Lable_SelectTexg
		{
			get
			{
				return "选择";
			}
		}

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

		private void OpenDialog_FileOk(object sender, CancelEventArgs e)
		{
			this.OnClosingDialog();
			this.OnSelectedNameChanged();
		}

		private void OpenFilesDialog_FolderNameChanged(OpenFileDialogEx sender, string filePath)
		{
			this.cando = true;
			this.fileConvert.Clear();
		}

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

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
		}

		public string FolderParent = string.Empty;

		public List<string> FilesPath = new List<string>();

		private List<string> fileConvert = new List<string>();

		private bool cando = false;

		private IContainer components = null;
	}
}
