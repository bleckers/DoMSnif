namespace KS0108
{
    partial class FormLCD
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLCD));
            this.pictureBoxLCD = new System.Windows.Forms.PictureBox();
            this.contextMenuStripImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackWhiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueWhiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkGreenLightGreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackBlueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackGreyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.openFileDialogCSV = new System.Windows.Forms.OpenFileDialog();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBoxInvert = new System.Windows.Forms.CheckBox();
            this.checkBoxSlow = new System.Windows.Forms.CheckBox();
            this.checkBoxDebug = new System.Windows.Forms.CheckBox();
            this.toolStripStatusLabelSplitter = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLCD)).BeginInit();
            this.contextMenuStripImage.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLCD
            // 
            this.pictureBoxLCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxLCD.ContextMenuStrip = this.contextMenuStripImage;
            this.pictureBoxLCD.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxLCD.Name = "pictureBoxLCD";
            this.pictureBoxLCD.Size = new System.Drawing.Size(256, 128);
            this.pictureBoxLCD.TabIndex = 0;
            this.pictureBoxLCD.TabStop = false;
            // 
            // contextMenuStripImage
            // 
            this.contextMenuStripImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colourToolStripMenuItem});
            this.contextMenuStripImage.Name = "contextMenuStripImage";
            this.contextMenuStripImage.Size = new System.Drawing.Size(104, 26);
            // 
            // colourToolStripMenuItem
            // 
            this.colourToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackWhiteToolStripMenuItem,
            this.blueWhiteToolStripMenuItem,
            this.darkGreenLightGreenToolStripMenuItem,
            this.blackBlueToolStripMenuItem,
            this.blackGreyToolStripMenuItem});
            this.colourToolStripMenuItem.Name = "colourToolStripMenuItem";
            this.colourToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.colourToolStripMenuItem.Text = "Color";
            // 
            // blackWhiteToolStripMenuItem
            // 
            this.blackWhiteToolStripMenuItem.Name = "blackWhiteToolStripMenuItem";
            this.blackWhiteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.blackWhiteToolStripMenuItem.Text = "Black/White";
            this.blackWhiteToolStripMenuItem.Click += new System.EventHandler(this.blackWhiteToolStripMenuItem_Click);
            // 
            // blueWhiteToolStripMenuItem
            // 
            this.blueWhiteToolStripMenuItem.Name = "blueWhiteToolStripMenuItem";
            this.blueWhiteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.blueWhiteToolStripMenuItem.Text = "Blue/White";
            this.blueWhiteToolStripMenuItem.Click += new System.EventHandler(this.blueWhiteToolStripMenuItem_Click);
            // 
            // darkGreenLightGreenToolStripMenuItem
            // 
            this.darkGreenLightGreenToolStripMenuItem.Name = "darkGreenLightGreenToolStripMenuItem";
            this.darkGreenLightGreenToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.darkGreenLightGreenToolStripMenuItem.Text = "DarkGreen/LightGreen";
            this.darkGreenLightGreenToolStripMenuItem.Click += new System.EventHandler(this.darkGreenLightGreenToolStripMenuItem_Click);
            // 
            // blackBlueToolStripMenuItem
            // 
            this.blackBlueToolStripMenuItem.Name = "blackBlueToolStripMenuItem";
            this.blackBlueToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.blackBlueToolStripMenuItem.Text = "Black/Blue";
            this.blackBlueToolStripMenuItem.Click += new System.EventHandler(this.blackBlueToolStripMenuItem_Click);
            // 
            // blackGreyToolStripMenuItem
            // 
            this.blackGreyToolStripMenuItem.Name = "blackGreyToolStripMenuItem";
            this.blackGreyToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.blackGreyToolStripMenuItem.Text = "Black/Grey";
            this.blackGreyToolStripMenuItem.Click += new System.EventHandler(this.blackGreyToolStripMenuItem_Click);
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.Location = new System.Drawing.Point(13, 147);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.ReadOnly = true;
            this.textBoxFilename.Size = new System.Drawing.Size(172, 20);
            this.textBoxFilename.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(191, 146);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(191, 175);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 3;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // openFileDialogCSV
            // 
            this.openFileDialogCSV.FileName = "file.csv";
            this.openFileDialogCSV.Filter = "CSV Files|*.csv";
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabelCPS,
            this.toolStripStatusLabelSplitter,
            this.toolStripStatusLabel});
            this.statusStripMain.Location = new System.Drawing.Point(0, 203);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(280, 22);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 4;
            this.statusStripMain.Text = "statusStripMain";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Maximum = 10;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel.Text = "   ";
            // 
            // toolStripStatusLabelCPS
            // 
            this.toolStripStatusLabelCPS.Name = "toolStripStatusLabelCPS";
            this.toolStripStatusLabelCPS.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabelCPS.Text = "   ";
            // 
            // checkBoxInvert
            // 
            this.checkBoxInvert.AutoSize = true;
            this.checkBoxInvert.Location = new System.Drawing.Point(13, 180);
            this.checkBoxInvert.Name = "checkBoxInvert";
            this.checkBoxInvert.Size = new System.Drawing.Size(53, 17);
            this.checkBoxInvert.TabIndex = 5;
            this.checkBoxInvert.Text = "Invert";
            this.checkBoxInvert.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlow
            // 
            this.checkBoxSlow.AutoSize = true;
            this.checkBoxSlow.Location = new System.Drawing.Point(72, 180);
            this.checkBoxSlow.Name = "checkBoxSlow";
            this.checkBoxSlow.Size = new System.Drawing.Size(49, 17);
            this.checkBoxSlow.TabIndex = 6;
            this.checkBoxSlow.Text = "Slow";
            this.checkBoxSlow.UseVisualStyleBackColor = true;
            // 
            // checkBoxDebug
            // 
            this.checkBoxDebug.AutoSize = true;
            this.checkBoxDebug.Location = new System.Drawing.Point(127, 180);
            this.checkBoxDebug.Name = "checkBoxDebug";
            this.checkBoxDebug.Size = new System.Drawing.Size(58, 17);
            this.checkBoxDebug.TabIndex = 7;
            this.checkBoxDebug.Text = "Debug";
            this.checkBoxDebug.UseVisualStyleBackColor = true;
            // 
            // toolStripStatusLabelSplitter
            // 
            this.toolStripStatusLabelSplitter.Name = "toolStripStatusLabelSplitter";
            this.toolStripStatusLabelSplitter.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabelSplitter.Text = "|";
            // 
            // FormLCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 225);
            this.Controls.Add(this.checkBoxDebug);
            this.Controls.Add(this.checkBoxSlow);
            this.Controls.Add(this.checkBoxInvert);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxFilename);
            this.Controls.Add(this.pictureBoxLCD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormLCD";
            this.Text = "KS0108";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLCD_FormClosing);
            this.Load += new System.EventHandler(this.FormLCD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLCD)).EndInit();
            this.contextMenuStripImage.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLCD;
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.OpenFileDialog openFileDialogCSV;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.CheckBox checkBoxInvert;
        private System.Windows.Forms.CheckBox checkBoxSlow;
        private System.Windows.Forms.CheckBox checkBoxDebug;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripImage;
        private System.Windows.Forms.ToolStripMenuItem colourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackWhiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueWhiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkGreenLightGreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackBlueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackGreyToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCPS;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSplitter;
    }
}

