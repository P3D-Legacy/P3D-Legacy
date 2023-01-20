#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
#endregion

namespace TrueTypeConverter
{
	/// <summary>
	/// Utility for rendering Windows fonts out into a BMP file
	/// which can then be imported into the XNA Framework using
	/// the Content Pipeline FontTextureProcessor.
	/// </summary>
	public partial class MainForm : Form
	{
		Bitmap globalBitmap;
		Graphics globalGraphics;
		Font font;
		string fontError;
		

		/// <summary>
		/// Constructor.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			globalBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			globalGraphics = Graphics.FromImage(globalBitmap);
			
			foreach (FontFamily font in FontFamily.Families)
				FontName.Items.Add(font.Name);

			FontName.Text = Properties.Settings.Default.FontName;
			OutlineColorSample.BackColor = Properties.Settings.Default.OutlineColor;
			ShadowColorSample.BackColor = Properties.Settings.Default.ShadowColor;

			if (Properties.Settings.Default.TextFiles != null)
			{
				foreach (var fileName in Properties.Settings.Default.TextFiles)
				{
					TextFilesListBox.Items.Add(fileName);
				}
			}
		}


		/// <summary>
		/// When the font selection changes, create a new Font
		/// instance and update the preview text label.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		void SelectionChanged()
		{
			try
			{
				// Parse the size selection.
				float size;

				if (!float.TryParse(FontSize.Text, out size) || (size <= 0))
				{
					fontError = "Invalid font size '" + FontSize.Text + "'";
					return;
				}

				// Parse the font style selection.
				FontStyle style;

				try
				{
					style = (FontStyle)Enum.Parse(typeof(FontStyle), FontStyle.Text);
				}
				catch
				{
					fontError = "Invalid font style '" + FontStyle.Text + "'";
					return;
				}

				// Create the new font.
				Font newFont = new Font(FontName.Text, size, style);

				if (font != null)
					font.Dispose();

				Sample.Font = font = newFont;

				fontError = null;
			}
			catch (Exception exception)
			{
				fontError = exception.Message;
			}
		}


		/// <summary>
		/// Selection changed event handler.
		/// </summary>
		private void FontName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SelectionChanged();
		}


		/// <summary>
		/// Selection changed event handler.
		/// </summary>
		private void FontStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SelectionChanged();
		}


		/// <summary>
		/// Selection changed event handler.
		/// </summary>
		private void FontSize_TextUpdate(object sender, System.EventArgs e)
		{
			SelectionChanged();
		}


		/// <summary>
		/// Selection changed event handler.
		/// </summary>
		private void FontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectionChanged();
		}

		private void ChooseTextFilesButton_Click(object sender, EventArgs e)
		{
			// Choose the files to read text from
			OpenFileDialog fileSelector = new OpenFileDialog();

			fileSelector.InitialDirectory = Properties.Settings.Default.TextFilesDir;
			fileSelector.Title = "Coose Text Files";
			fileSelector.DefaultExt = "*";
			fileSelector.Filter = "All files (*.*)|*.*";
			fileSelector.Multiselect = true;

			if (fileSelector.ShowDialog() == DialogResult.OK)
			{				
				TextFilesListBox.Items.Clear();
				foreach (var file in fileSelector.FileNames)
				{
					TextFilesListBox.Items.Add(file);
					
				}
				Properties.Settings.Default.TextFilesDir = Path.GetDirectoryName(fileSelector.FileNames[0]);
			}
		}

		/// <summary>
		/// Event handler for when the user clicks on the Export button.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private void Export_Click(object sender, EventArgs e)
		{
			try
			{
				// If the current font is invalid, report that to the user.
				if (fontError != null)
					throw new ArgumentException(fontError);
				

				// Choose the output file.
				SaveFileDialog fileSelector = new SaveFileDialog();

				fileSelector.InitialDirectory = Properties.Settings.Default.ExportDir;
				fileSelector.Title = "Export Font";
				fileSelector.DefaultExt = "bmp";
				fileSelector.Filter = "Image files (*.bmp)|*.bmp|All files (*.*)|*.*";

				if (fileSelector.ShowDialog() == DialogResult.OK)
				{

					//Get the path to the game text
					string outputDir = Path.GetDirectoryName(fileSelector.FileName);
					Properties.Settings.Default.ExportDir = outputDir;

					//Just grab every string in every language.
					string allText = "";

					HashSet<char> charSet = new HashSet<char>();
					List<char> charList = new List<char>();

					foreach (string file in TextFilesListBox.Items)
					{
						string absolutePath = Path.GetFullPath(file);
						string readText = File.ReadAllText(file);
						allText += readText;
					}


					// Scan each character of the string.
					foreach (char usedCharacter in allText)
					{
						if (!charSet.Contains(usedCharacter))
						{
							charSet.Add(usedCharacter);
							charList.Add(usedCharacter);
						}
					}
					charList.Sort();

					

					// Build up a list of all the glyphs to be output.
					List<Bitmap> bitmaps = new List<Bitmap>();
					List<int> xPositions = new List<int>();
					List<int> yPositions = new List<int>();

					try
					{
						const int padding = 8;

						int width = padding;
						int height = padding;
						int lineWidth = padding;
						int lineHeight = padding;
						int count = 0;

						// Rasterize each character in turn,
						// and add it to the output list.
						//for (char ch = (char)minChar; ch < maxChar; ch++)
						foreach(char ch in charList)
						{
							Bitmap bitmap = RasterizeCharacter(ch);

							bitmaps.Add(bitmap);

							xPositions.Add(lineWidth);
							yPositions.Add(height);

							lineWidth += bitmap.Width + padding;
							lineHeight = Math.Max(lineHeight, bitmap.Height + padding);

							// Output 16 glyphs per line, then wrap to the next line.
							if (++count == 16)
							{
								width = Math.Max(width, lineWidth);
								height += lineHeight;
								lineWidth = padding;
								lineHeight = padding;
								count = 0;
							}
						}

						using (Bitmap bitmap = new Bitmap(width, height + lineHeight,
														  PixelFormat.Format32bppArgb))
						{
							// Arrage all the glyphs onto a single larger bitmap.
							using (Graphics graphics = Graphics.FromImage(bitmap))
							{
								graphics.Clear(Color.Magenta);
								graphics.CompositingMode = CompositingMode.SourceCopy;

								for (int i = 0; i < bitmaps.Count; i++)
								{
									graphics.DrawImage(bitmaps[i], xPositions[i],
																   yPositions[i]);
								}

								graphics.Flush();
							}

							// Save out the combined bitmap.
							bitmap.Save(fileSelector.FileName, ImageFormat.Bmp);
						}
					}
					finally
					{
						// Clean up temporary objects.
						foreach (Bitmap bitmap in bitmaps)
							bitmap.Dispose();
					}

					//Output the characters we just rendered to a text file
					string charOutput = Path.ChangeExtension(fileSelector.FileName, ".txt");
					TextWriter w = new StreamWriter(charOutput);
					for (int i = 0; i < charList.Count; i++)
					{
						w.Write(charList[i]);
					}
					w.Close();
					w.Dispose();
				}
			}
			catch (Exception exception)
			{
				// Report any errors to the user.
				MessageBox.Show(exception.Message, Text + " Error");
			}
		}


		/// <summary>
		/// Helper for rendering out a single font character
		/// into a System.Drawing bitmap.
		/// </summary>
		private Bitmap RasterizeCharacter(char ch)
		{
			string text = ch.ToString();

			SizeF size = globalGraphics.MeasureString(text, font);

			int width = (int)Math.Ceiling(size.Width);
			int height = (int)Math.Ceiling(size.Height);

			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				if (Antialias.Checked)
				{
					graphics.TextRenderingHint =
						TextRenderingHint.ClearTypeGridFit;
				}
				else
				{
					graphics.TextRenderingHint =
						TextRenderingHint.SingleBitPerPixelGridFit;
				}

				graphics.Clear(Color.Transparent);

				int Alpha = int.Parse(AlphaAmount.Text);
				if (Alpha < 0)
					Alpha = 0;
				if (Alpha > 255)
					Alpha = 255;
				Color customColor = Color.FromArgb(Alpha, ShadowColorSample.BackColor);
				using (Brush brush = new SolidBrush(Color.White))
				using (Brush brushoutline = new SolidBrush(OutlineColorSample.BackColor))
				using (Brush brushshadow = new SolidBrush(customColor))
				using (StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;

					int Shadow = int.Parse(ShadowOffset.Text);
					int Outline = int.Parse(OutlineSize.Text);

					// Draw the shadow first
					// Shadow
					if (Shadow > 0)
					{
						graphics.DrawString(text, font, brushshadow, Shadow, Shadow, format);
					}

					// Next draw the outline
					// outline
					if (Outline > 0)
					{
						for (int i = 1; i <= Outline; ++i)
						{
							graphics.DrawString(text, font, brushoutline, -1 * i, -1 * i, format);
							graphics.DrawString(text, font, brushoutline, 0, -1 * i, format);
							graphics.DrawString(text, font, brushoutline, 1 * i, -1 * i, format);
							graphics.DrawString(text, font, brushoutline, -1 * i, 0, format);
							graphics.DrawString(text, font, brushoutline, 1 * i, 0, format);
							graphics.DrawString(text, font, brushoutline, -1 * i, 1 * i, format);
							graphics.DrawString(text, font, brushoutline, 0, 1 * i, format);
							graphics.DrawString(text, font, brushoutline, 1 * i, 1 * i, format);
						}
					}

					// Finally draw the text
					graphics.DrawString(text, font, brush, 0, 0, format);
				}

				graphics.Flush();
			}

			return CropCharacter(bitmap);
		}


		/// <summary>
		/// Helper for cropping ununsed space from the sides of a bitmap.
		/// </summary>
		private static Bitmap CropCharacter(Bitmap bitmap)
		{
			int cropLeft = 0;
			int cropRight = bitmap.Width - 1;

			// Remove unused space from the left.
			while ((cropLeft < cropRight) && (BitmapIsEmpty(bitmap, cropLeft)))
				cropLeft++;

			//If the entire glyph is blank, output the full blank glyph.
			if (cropLeft == cropRight)
			{
				return bitmap;
			}

			// Remove unused space from the right.
			while ((cropRight > cropLeft) && (BitmapIsEmpty(bitmap, cropRight)))
				cropRight--;

			// Don't crop if that would reduce the glyph down to nothing at all!
			if (cropLeft > cropRight)	//Note:  cropRight is inclusive, so for letter's like I, l, |, etc, cropLeft == cropRight.
				return bitmap;

			// Add some padding back in.
			cropLeft = Math.Max(cropLeft - 1, 0);
			cropRight = Math.Min(cropRight + 1, bitmap.Width - 1);

			int width = cropRight - cropLeft + 1;

			// Crop the glyph.
			Bitmap croppedBitmap = new Bitmap(width, bitmap.Height, bitmap.PixelFormat);

			using (Graphics graphics = Graphics.FromImage(croppedBitmap))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;

				graphics.DrawImage(bitmap, 0, 0,
								   new Rectangle(cropLeft, 0, width, bitmap.Height),
								   GraphicsUnit.Pixel);

				graphics.Flush();
			}

			bitmap.Dispose();

			return croppedBitmap;
		}


		/// <summary>
		/// Helper for testing whether a column of a bitmap is entirely empty.
		/// </summary>
		private static bool BitmapIsEmpty(Bitmap bitmap, int x)
		{
			for (int y = 0; y < bitmap.Height; y++)
			{
				if (bitmap.GetPixel(x, y).A != 0)
					return false;
			}

			return true;
		}


		/// <summary>
		/// Helper for converting strings to integer.
		/// </summary>
		static int ParseHex(string text)
		{
			NumberStyles style;

			if (text.StartsWith("0x"))
			{
				style = NumberStyles.HexNumber;
				text = text.Substring(2);
			}
			else
			{
				style = NumberStyles.Integer;
			}

			int result;

			if (!int.TryParse(text, style, null, out result))
				return -1;

			return result;
		}

		private void OutlineColorSample_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = OutlineColorSample.BackColor;
			colorDialog1.ShowDialog();
			OutlineColorSample.BackColor = colorDialog1.Color;
		}

		private void ShadowColorSample_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = ShadowColorSample.BackColor;
			colorDialog1.ShowDialog();
			ShadowColorSample.BackColor = colorDialog1.Color;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.TextFiles = new System.Collections.Specialized.StringCollection();
			
			foreach (string fileName in TextFilesListBox.Items)
			{
				Properties.Settings.Default.TextFiles.Add(fileName);
			}			


			Properties.Settings.Default.Save();
		}
	}
}
