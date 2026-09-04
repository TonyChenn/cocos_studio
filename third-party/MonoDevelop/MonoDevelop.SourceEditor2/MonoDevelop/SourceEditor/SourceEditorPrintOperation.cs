using System;
using System.Collections.Generic;
using System.Text;
using Cairo;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Core;
using Pango;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor
{
	internal class SourceEditorPrintOperation : PrintOperation
	{
		private TextDocument doc;

		private FilePath filename;

		private SourceEditorPrintSettings settings;

		private int headerLines;

		private int footerLines;

		private int totalPages;

		private int linesPerPage;

		private double lineHeight;

		private double pageWidth;

		private double pageHeight;

		private Pango.Layout layout;

		private ColorScheme style;

		private string headerText;

		private string footerText;

		public SourceEditorPrintOperation(TextDocument doc, FilePath filename)
		{
			this.doc = doc;
			this.filename = filename;
			settings = SourceEditorPrintSettings.Load();
			base.Unit = Unit.Pixel;
		}

		protected override void OnBeginPrint(PrintContext context)
		{
			layout = PangoUtil.CreateLayout(context);
			layout.FontDescription = settings.Font;
			layout.FontDescription.Weight = Weight.Bold;
			layout.SetText(" ");
			layout.GetSize(out var _, out var height);
			lineHeight = (double)height / Pango.Scale.PangoScale;
			layout.FontDescription.Weight = Weight.Normal;
			SetHeaderFormat(settings.HeaderFormat);
			SetFooterFormat(settings.FooterFormat);
			style = Mono.TextEditor.Highlighting.SyntaxModeService.GetColorStyle(settings.ColorScheme);
			pageWidth = context.PageSetup.GetPageWidth(Unit.Pixel);
			pageHeight = context.PageSetup.GetPageHeight(Unit.Pixel);
			double num = pageHeight - ((headerLines > 0) ? settings.HeaderPadding : 0.0) - ((footerLines > 0) ? settings.FooterPadding : 0.0);
			linesPerPage = (int)(num / lineHeight) - (headerLines + footerLines);
			totalPages = (int)Math.Ceiling((double)doc.LineCount / (double)linesPerPage);
			base.NPages = totalPages;
			base.OnBeginPrint(context);
		}

		protected override void OnEndPrint(PrintContext context)
		{
			layout.Dispose();
			layout = null;
			base.OnEndPrint(context);
		}

		protected override void OnDrawPage(PrintContext context, int pageNr)
		{
			using (Cairo.Context context2 = context.CairoContext)
			{
				double xPos = 0.0;
				double yPos = 0.0;
				PrintHeader(context2, context, pageNr, ref xPos, ref yPos);
				int num = pageNr * linesPerPage;
				int num2 = Math.Min(num + linesPerPage - 1, doc.LineCount);
				for (int i = num; i < num2; i++)
				{
					DocumentLine line = doc.GetLine(i + 1);
					if (!settings.UseHighlighting)
					{
						string textAt = doc.GetTextAt(line);
						textAt = textAt.Replace("\t", new string(' ', settings.TabSize));
						layout.SetText(textAt);
						context2.MoveTo(xPos, yPos);
						CairoHelper.ShowLayout(context2, layout);
						yPos += lineHeight;
						continue;
					}
					IEnumerable<Chunk> chunks = doc.SyntaxMode.GetChunks(style, line, line.Offset, line.LengthIncludingDelimiter);
					foreach (Chunk item in chunks)
					{
						ChunkStyle chunkStyle = ((item != null) ? style.GetChunkStyle(item) : null);
						string textAt2 = doc.GetTextAt(item);
						textAt2 = textAt2.Replace("\t", new string(' ', settings.TabSize));
						layout.SetText(textAt2);
						AttrList attrList = ResetAttributes();
						attrList.Insert(new AttrForeground((ushort)(chunkStyle.Foreground.R * 65535.0), (ushort)(chunkStyle.Foreground.G * 65535.0), (ushort)(chunkStyle.Foreground.B * 65535.0)));
						if (chunkStyle.FontWeight != Xwt.Drawing.FontWeight.Normal)
						{
							attrList.Insert(new AttrWeight((Weight)chunkStyle.FontWeight));
						}
						if (chunkStyle.FontStyle != FontStyle.Normal)
						{
							attrList.Insert(new AttrStyle((Pango.Style)chunkStyle.FontStyle));
						}
						if (chunkStyle.Underline)
						{
							attrList.Insert(new AttrUnderline(Underline.Single));
						}
						context2.MoveTo(xPos, yPos);
						CairoHelper.ShowLayout(context2, layout);
						layout.GetSize(out var width, out var _);
						double num3 = (double)width / Pango.Scale.PangoScale;
						xPos += num3;
						if (num3 > pageWidth)
						{
							break;
						}
					}
					xPos = 0.0;
					yPos += lineHeight;
				}
				PrintFooter(context2, context, pageNr, ref xPos, ref yPos);
			}
		}

		private AttrList ResetAttributes()
		{
			if (layout.Attributes != null)
			{
				layout.Attributes.Dispose();
			}
			return layout.Attributes = new AttrList();
		}

		private void PrintHeader(Cairo.Context cr, PrintContext context, int page, ref double xPos, ref double yPos)
		{
			if (headerLines != 0)
			{
				ResetAttributes();
				layout.SetText(Subst(headerText, page));
				layout.GetSize(out var width, out var _);
				double num = (double)width / Pango.Scale.PangoScale;
				cr.MoveTo((pageWidth - num) / 2.0, yPos);
				CairoHelper.ShowLayout(cr, layout);
				yPos += lineHeight * (double)headerLines;
				if (settings.HeaderSeparatorWeight > 0.0)
				{
					cr.LineWidth = settings.HeaderSeparatorWeight;
					cr.MoveTo(pageWidth / 3.0, yPos + settings.HeaderPadding / 2.0);
					cr.LineTo(2.0 * pageWidth / 3.0, yPos + settings.HeaderPadding / 2.0);
					cr.Stroke();
				}
				yPos += settings.HeaderPadding;
			}
		}

		private string Subst(string text, int page)
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			stringBuilder.Replace("%N", (page + 1).ToString());
			stringBuilder.Replace("%Q", totalPages.ToString());
			stringBuilder.Replace("%F", SourceEditorWidget.EllipsizeMiddle(filename, 60));
			return stringBuilder.ToString();
		}

		private void PrintFooter(Cairo.Context cr, PrintContext context, int page, ref double xPos, ref double yPos)
		{
			if (footerLines != 0)
			{
				yPos = pageHeight - lineHeight * (double)footerLines - settings.FooterPadding;
				if (settings.FooterSeparatorWeight > 0.0)
				{
					cr.LineWidth = settings.FooterSeparatorWeight;
					cr.MoveTo(pageWidth / 3.0, yPos + settings.FooterPadding / 2.0);
					cr.LineTo(2.0 * pageWidth / 3.0, yPos + settings.FooterPadding / 2.0);
					cr.Stroke();
				}
				yPos += settings.FooterPadding;
				ResetAttributes();
				layout.SetText(Subst(footerText, page));
				layout.GetSize(out var width, out var _);
				double num = (double)width / Pango.Scale.PangoScale;
				cr.MoveTo((pageWidth - num) / 2.0, yPos);
				CairoHelper.ShowLayout(cr, layout);
			}
		}

		private void SetHeaderFormat(string middle)
		{
			headerText = middle;
			headerLines = ((middle != null && middle.Length != 0) ? middle.Split('\n').Length : 0);
		}

		private void SetFooterFormat(string middle)
		{
			footerText = middle;
			footerLines = ((middle != null && middle.Length != 0) ? middle.Split('\n').Length : 0);
		}

		protected override void OnDone(PrintOperationResult result)
		{
			if (result == PrintOperationResult.Apply)
			{
				settings.Save();
			}
			base.OnDone(result);
		}
	}
}
