using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mono.PkgConfig
{
	// Token: 0x020000B9 RID: 185
	internal class PcFile
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001863E File Offset: 0x0001683E
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00018646 File Offset: 0x00016846
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001864F File Offset: 0x0001684F
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00018657 File Offset: 0x00016857
		public string FilePath
		{
			get
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00018660 File Offset: 0x00016860
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00018668 File Offset: 0x00016868
		public bool HasErrors
		{
			get
			{
				return this.hasErrors;
			}
			set
			{
				this.hasErrors = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00018671 File Offset: 0x00016871
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00018679 File Offset: 0x00016879
		public string Libs
		{
			get
			{
				return this.libs;
			}
			set
			{
				this.libs = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00018682 File Offset: 0x00016882
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0001868A File Offset: 0x0001688A
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00018693 File Offset: 0x00016893
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x0001869B File Offset: 0x0001689B
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x000186A4 File Offset: 0x000168A4
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x000186AC File Offset: 0x000168AC
		public string Requires
		{
			get
			{
				return this.requires;
			}
			set
			{
				this.requires = value;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000186B8 File Offset: 0x000168B8
		public string GetVariable(string varName)
		{
			string result;
			this.variables.TryGetValue(varName, out result);
			return result;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000186D8 File Offset: 0x000168D8
		public void Load(string pcfile)
		{
			this.FilePath = pcfile;
			this.variables.Add("pcfiledir", Path.GetDirectoryName(pcfile));
			using (StreamReader streamReader = new StreamReader(pcfile))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					int num = text.IndexOf(':');
					int num2 = text.IndexOf('=');
					int num3 = Math.Min((num != -1) ? num : int.MaxValue, (num2 != -1) ? num2 : int.MaxValue);
					if (num3 != 2147483647)
					{
						string text2 = text.Substring(0, num3).Trim();
						string value = text.Substring(num3 + 1).Trim();
						value = this.Evaluate(value);
						string a;
						if (num3 == num2)
						{
							this.variables[text2] = value;
						}
						else if ((a = text2) != null)
						{
							if (!(a == "Name"))
							{
								if (!(a == "Description"))
								{
									if (!(a == "Version"))
									{
										if (!(a == "Libs"))
										{
											if (a == "Requires")
											{
												this.Requires = value;
											}
										}
										else
										{
											this.Libs = value;
										}
									}
									else
									{
										this.Version = value;
									}
								}
								else
								{
									this.Description = value;
								}
							}
							else
							{
								this.Name = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00018840 File Offset: 0x00016A40
		private string Evaluate(string value)
		{
			int num = value.IndexOf("${");
			if (num == -1)
			{
				return value;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num2 = 0;
			while (num != -1 && num < value.Length)
			{
				stringBuilder.Append(value.Substring(num2, num - num2));
				if (num == 0 || value[num - 1] != '$')
				{
					num += 2;
					int num3 = value.IndexOf('}', num);
					if (num3 == -1 || num3 == num)
					{
						this.HasErrors = true;
						return value;
					}
					string key = value.Substring(num, num3 - num);
					string value2;
					if (!this.variables.TryGetValue(key, out value2))
					{
						this.HasErrors = true;
						return value;
					}
					stringBuilder.Append(value2);
					num = num3 + 1;
					num2 = num;
				}
				else
				{
					num2 = num++;
				}
				if (num < value.Length)
				{
					num = value.IndexOf("${", num);
				}
			}
			stringBuilder.Append(value.Substring(num2, value.Length - num2));
			return stringBuilder.ToString();
		}

		// Token: 0x04000218 RID: 536
		private Dictionary<string, string> variables = new Dictionary<string, string>();

		// Token: 0x04000219 RID: 537
		private string description;

		// Token: 0x0400021A RID: 538
		private string filePath;

		// Token: 0x0400021B RID: 539
		private bool hasErrors;

		// Token: 0x0400021C RID: 540
		private string libs;

		// Token: 0x0400021D RID: 541
		private string name;

		// Token: 0x0400021E RID: 542
		private string version;

		// Token: 0x0400021F RID: 543
		private string requires;
	}
}
