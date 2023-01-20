//-----------------------------------------------------------------------------
// MainForm.Designer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace TrueTypeConverter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.Antialias = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.Sample = new System.Windows.Forms.Label();
			this.Export = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.TextFilesListBox = new System.Windows.Forms.ListBox();
			this.ChooseTextFilesButton = new System.Windows.Forms.Button();
			this.AlphaAmount = new System.Windows.Forms.TextBox();
			this.ShadowColorSample = new System.Windows.Forms.PictureBox();
			this.ShadowOffset = new System.Windows.Forms.TextBox();
			this.OutlineColorSample = new System.Windows.Forms.PictureBox();
			this.OutlineSize = new System.Windows.Forms.TextBox();
			this.FontSize = new System.Windows.Forms.ComboBox();
			this.FontStyle = new System.Windows.Forms.ComboBox();
			this.FontName = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ShadowColorSample)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OutlineColorSample)).BeginInit();
			this.SuspendLayout();
			// 
			// Antialias
			// 
			this.Antialias.AutoSize = true;
			this.Antialias.Checked = global::TrueTypeConverter.Properties.Settings.Default.Antialias;
			this.Antialias.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TrueTypeConverter.Properties.Settings.Default, "Antialias", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.Antialias.Location = new System.Drawing.Point(284, 212);
			this.Antialias.Name = "Antialias";
			this.Antialias.Size = new System.Drawing.Size(77, 17);
			this.Antialias.TabIndex = 10;
			this.Antialias.Text = "&Antialiased";
			this.Antialias.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(215, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Font s&tyle:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(310, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(30, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "&Size:";
			// 
			// Sample
			// 
			this.Sample.AutoEllipsis = true;
			this.Sample.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Sample.Location = new System.Drawing.Point(12, 375);
			this.Sample.Name = "Sample";
			this.Sample.Size = new System.Drawing.Size(457, 162);
			this.Sample.TabIndex = 12;
			this.Sample.Text = "The quick brown fox jumped over the LAZY camel";
			this.Sample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Export
			// 
			this.Export.Location = new System.Drawing.Point(394, 210);
			this.Export.Name = "Export";
			this.Export.Size = new System.Drawing.Size(75, 23);
			this.Export.TabIndex = 11;
			this.Export.Text = "&Export";
			this.Export.UseVisualStyleBackColor = true;
			this.Export.Click += new System.EventHandler(this.Export_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 14);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(31, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "&Font:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(30, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Size:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(38, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Offset:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.OutlineColorSample);
			this.groupBox1.Controls.Add(this.OutlineSize);
			this.groupBox1.Location = new System.Drawing.Point(367, 30);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(102, 68);
			this.groupBox1.TabIndex = 19;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Outline:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.AlphaAmount);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.ShadowColorSample);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.ShadowOffset);
			this.groupBox2.Location = new System.Drawing.Point(368, 117);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(101, 89);
			this.groupBox2.TabIndex = 20;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Shadow:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 70);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(37, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "Alpha:";
			// 
			// TextFilesListBox
			// 
			this.TextFilesListBox.AllowDrop = true;
			this.TextFilesListBox.FormattingEnabled = true;
			this.TextFilesListBox.Location = new System.Drawing.Point(15, 239);
			this.TextFilesListBox.Margin = new System.Windows.Forms.Padding(2);
			this.TextFilesListBox.Name = "TextFilesListBox";
			this.TextFilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.TextFilesListBox.Size = new System.Drawing.Size(454, 121);
			this.TextFilesListBox.TabIndex = 36;
			this.TextFilesListBox.TabStop = false;
			// 
			// ChooseTextFilesButton
			// 
			this.ChooseTextFilesButton.Location = new System.Drawing.Point(15, 212);
			this.ChooseTextFilesButton.Name = "ChooseTextFilesButton";
			this.ChooseTextFilesButton.Size = new System.Drawing.Size(110, 23);
			this.ChooseTextFilesButton.TabIndex = 37;
			this.ChooseTextFilesButton.Text = "Choose Text Files";
			this.ChooseTextFilesButton.UseVisualStyleBackColor = true;
			this.ChooseTextFilesButton.Click += new System.EventHandler(this.ChooseTextFilesButton_Click);
			// 
			// AlphaAmount
			// 
			this.AlphaAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "AlphaAmount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.AlphaAmount.Location = new System.Drawing.Point(47, 67);
			this.AlphaAmount.Name = "AlphaAmount";
			this.AlphaAmount.Size = new System.Drawing.Size(43, 20);
			this.AlphaAmount.TabIndex = 21;
			this.AlphaAmount.Text = global::TrueTypeConverter.Properties.Settings.Default.AlphaAmount;
			// 
			// ShadowColorSample
			// 
			this.ShadowColorSample.BackColor = global::TrueTypeConverter.Properties.Settings.Default.ShadowColor;
			this.ShadowColorSample.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ShadowColorSample.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::TrueTypeConverter.Properties.Settings.Default, "ShadowColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ShadowColorSample.Location = new System.Drawing.Point(9, 39);
			this.ShadowColorSample.Name = "ShadowColorSample";
			this.ShadowColorSample.Size = new System.Drawing.Size(81, 21);
			this.ShadowColorSample.TabIndex = 19;
			this.ShadowColorSample.TabStop = false;
			this.ShadowColorSample.Click += new System.EventHandler(this.ShadowColorSample_Click);
			// 
			// ShadowOffset
			// 
			this.ShadowOffset.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "ShadowOffset", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.ShadowOffset.Location = new System.Drawing.Point(50, 13);
			this.ShadowOffset.Name = "ShadowOffset";
			this.ShadowOffset.Size = new System.Drawing.Size(36, 20);
			this.ShadowOffset.TabIndex = 16;
			this.ShadowOffset.Text = global::TrueTypeConverter.Properties.Settings.Default.ShadowOffset;
			// 
			// OutlineColorSample
			// 
			this.OutlineColorSample.BackColor = global::TrueTypeConverter.Properties.Settings.Default.OutlineColor;
			this.OutlineColorSample.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.OutlineColorSample.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::TrueTypeConverter.Properties.Settings.Default, "OutlineColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.OutlineColorSample.Location = new System.Drawing.Point(9, 40);
			this.OutlineColorSample.Name = "OutlineColorSample";
			this.OutlineColorSample.Size = new System.Drawing.Size(81, 21);
			this.OutlineColorSample.TabIndex = 18;
			this.OutlineColorSample.TabStop = false;
			this.OutlineColorSample.Click += new System.EventHandler(this.OutlineColorSample_Click);
			// 
			// OutlineSize
			// 
			this.OutlineSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "OutlineSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.OutlineSize.Location = new System.Drawing.Point(42, 13);
			this.OutlineSize.Name = "OutlineSize";
			this.OutlineSize.Size = new System.Drawing.Size(48, 20);
			this.OutlineSize.TabIndex = 13;
			this.OutlineSize.Text = global::TrueTypeConverter.Properties.Settings.Default.OutlineSize;
			// 
			// FontSize
			// 
			this.FontSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "FontSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.FontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
			this.FontSize.FormattingEnabled = true;
			this.FontSize.Items.AddRange(new object[] {
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "23",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
			this.FontSize.Location = new System.Drawing.Point(312, 30);
			this.FontSize.Name = "FontSize";
			this.FontSize.Size = new System.Drawing.Size(49, 176);
			this.FontSize.TabIndex = 5;
			this.FontSize.Text = global::TrueTypeConverter.Properties.Settings.Default.FontSize;
			this.FontSize.SelectedIndexChanged += new System.EventHandler(this.FontSize_SelectedIndexChanged);
			this.FontSize.TextUpdate += new System.EventHandler(this.FontSize_TextUpdate);
			// 
			// FontStyle
			// 
			this.FontStyle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.FontStyle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.FontStyle.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "FontStyle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.FontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
			this.FontStyle.FormattingEnabled = true;
			this.FontStyle.Items.AddRange(new object[] {
            "Regular",
            "Italic",
            "Bold",
            "Bold, Italic"});
			this.FontStyle.Location = new System.Drawing.Point(218, 30);
			this.FontStyle.Name = "FontStyle";
			this.FontStyle.Size = new System.Drawing.Size(80, 176);
			this.FontStyle.TabIndex = 3;
			this.FontStyle.Text = global::TrueTypeConverter.Properties.Settings.Default.FontStyle;
			this.FontStyle.SelectedIndexChanged += new System.EventHandler(this.FontStyle_SelectedIndexChanged);
			// 
			// FontName
			// 
			this.FontName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.FontName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.FontName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TrueTypeConverter.Properties.Settings.Default, "FontName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.FontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
			this.FontName.FormattingEnabled = true;
			this.FontName.Location = new System.Drawing.Point(15, 30);
			this.FontName.Name = "FontName";
			this.FontName.Size = new System.Drawing.Size(189, 176);
			this.FontName.TabIndex = 1;
			this.FontName.Text = global::TrueTypeConverter.Properties.Settings.Default.FontName;
			this.FontName.SelectedIndexChanged += new System.EventHandler(this.FontName_SelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(490, 546);
			this.Controls.Add(this.ChooseTextFilesButton);
			this.Controls.Add(this.TextFilesListBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.Export);
			this.Controls.Add(this.Sample);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.Antialias);
			this.Controls.Add(this.FontSize);
			this.Controls.Add(this.FontStyle);
			this.Controls.Add(this.FontName);
			this.Name = "MainForm";
			this.Text = "ttf2bmp";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ShadowColorSample)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OutlineColorSample)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox FontName;
        private System.Windows.Forms.ComboBox FontStyle;
        private System.Windows.Forms.ComboBox FontSize;
        private System.Windows.Forms.CheckBox Antialias;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Sample;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox OutlineSize;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox ShadowOffset;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.PictureBox OutlineColorSample;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.PictureBox ShadowColorSample;
		private System.Windows.Forms.TextBox AlphaAmount;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ListBox TextFilesListBox;
		private System.Windows.Forms.Button ChooseTextFilesButton;
	}
}

