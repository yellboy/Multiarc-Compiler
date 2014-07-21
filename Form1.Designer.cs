namespace MultiArc_Compiler
{
    partial class Form1
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
            this.CodeBox = new System.Windows.Forms.RichTextBox();
            this.BinaryCodeBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AssemblyButton = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.LoadFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.LoadFilePathText = new System.Windows.Forms.TextBox();
            this.LoadFileBrowseButton = new System.Windows.Forms.Button();
            this.ChooseFileLabel = new System.Windows.Forms.Label();
            this.LoadFromFileButton = new System.Windows.Forms.Button();
            this.SaveFileButton = new System.Windows.Forms.Button();
            this.SaveFileAsButton = new System.Windows.Forms.Button();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.BinLoadFileLabel = new System.Windows.Forms.Label();
            this.BinFilePathText = new System.Windows.Forms.TextBox();
            this.BinFileBrowseButton = new System.Windows.Forms.Button();
            this.BinSaveFileAsButton = new System.Windows.Forms.Button();
            this.BinSaveFileButton = new System.Windows.Forms.Button();
            this.BinFileLoadButton = new System.Windows.Forms.Button();
            this.BinSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.BinLoadFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.BinFileNameLabel = new System.Windows.Forms.Label();
            this.ByteCountBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.architectureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadArcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadArchitectureDialog = new System.Windows.Forms.OpenFileDialog();
            this.registersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CodeBox
            // 
            this.CodeBox.AcceptsTab = true;
            this.CodeBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CodeBox.Location = new System.Drawing.Point(12, 45);
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(413, 310);
            this.CodeBox.TabIndex = 1;
            this.CodeBox.TabStop = false;
            this.CodeBox.Text = "";
            // 
            // BinaryCodeBox
            // 
            this.BinaryCodeBox.AcceptsTab = true;
            this.BinaryCodeBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BinaryCodeBox.Location = new System.Drawing.Point(476, 45);
            this.BinaryCodeBox.Name = "BinaryCodeBox";
            this.BinaryCodeBox.ReadOnly = true;
            this.BinaryCodeBox.Size = new System.Drawing.Size(358, 310);
            this.BinaryCodeBox.TabIndex = 2;
            this.BinaryCodeBox.TabStop = false;
            this.BinaryCodeBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(435, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Binary Code";
            // 
            // AssemblyButton
            // 
            this.AssemblyButton.Location = new System.Drawing.Point(12, 439);
            this.AssemblyButton.Name = "AssemblyButton";
            this.AssemblyButton.Size = new System.Drawing.Size(67, 45);
            this.AssemblyButton.TabIndex = 5;
            this.AssemblyButton.Text = "Assembly";
            this.AssemblyButton.UseVisualStyleBackColor = true;
            this.AssemblyButton.Click += new System.EventHandler(this.AssemblyButton_Click);
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.Image = global::MultiArc_Compiler.Properties.Resources.play;
            this.ExecuteButton.Location = new System.Drawing.Point(85, 439);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(47, 45);
            this.ExecuteButton.TabIndex = 6;
            this.ExecuteButton.UseVisualStyleBackColor = true;
            this.ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // LoadFileDialog
            // 
            this.LoadFileDialog.DefaultExt = "as";
            this.LoadFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.LoadFileDialog_FileOk);
            // 
            // LoadFilePathText
            // 
            this.LoadFilePathText.Location = new System.Drawing.Point(12, 374);
            this.LoadFilePathText.Name = "LoadFilePathText";
            this.LoadFilePathText.Size = new System.Drawing.Size(289, 20);
            this.LoadFilePathText.TabIndex = 8;
            this.LoadFilePathText.Text = "Full path to file...";
            // 
            // LoadFileBrowseButton
            // 
            this.LoadFileBrowseButton.Location = new System.Drawing.Point(307, 374);
            this.LoadFileBrowseButton.Name = "LoadFileBrowseButton";
            this.LoadFileBrowseButton.Size = new System.Drawing.Size(56, 20);
            this.LoadFileBrowseButton.TabIndex = 9;
            this.LoadFileBrowseButton.Text = "Browse";
            this.LoadFileBrowseButton.UseVisualStyleBackColor = true;
            this.LoadFileBrowseButton.Click += new System.EventHandler(this.LoadFileBrowseButton_Click);
            // 
            // ChooseFileLabel
            // 
            this.ChooseFileLabel.AutoSize = true;
            this.ChooseFileLabel.Location = new System.Drawing.Point(12, 358);
            this.ChooseFileLabel.Name = "ChooseFileLabel";
            this.ChooseFileLabel.Size = new System.Drawing.Size(70, 13);
            this.ChooseFileLabel.TabIndex = 10;
            this.ChooseFileLabel.Text = "Load from file";
            // 
            // LoadFromFileButton
            // 
            this.LoadFromFileButton.Location = new System.Drawing.Point(12, 400);
            this.LoadFromFileButton.Name = "LoadFromFileButton";
            this.LoadFromFileButton.Size = new System.Drawing.Size(67, 23);
            this.LoadFromFileButton.TabIndex = 11;
            this.LoadFromFileButton.Text = "Load";
            this.LoadFromFileButton.UseVisualStyleBackColor = true;
            this.LoadFromFileButton.Click += new System.EventHandler(this.LoadFromFileButton_Click);
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.Location = new System.Drawing.Point(85, 400);
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.Size = new System.Drawing.Size(71, 23);
            this.SaveFileButton.TabIndex = 12;
            this.SaveFileButton.Text = "Save";
            this.SaveFileButton.UseVisualStyleBackColor = true;
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // SaveFileAsButton
            // 
            this.SaveFileAsButton.Location = new System.Drawing.Point(162, 400);
            this.SaveFileAsButton.Name = "SaveFileAsButton";
            this.SaveFileAsButton.Size = new System.Drawing.Size(68, 23);
            this.SaveFileAsButton.TabIndex = 13;
            this.SaveFileAsButton.Text = "Save As";
            this.SaveFileAsButton.UseVisualStyleBackColor = true;
            this.SaveFileAsButton.Click += new System.EventHandler(this.SaveFileAsButton_Click);
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.DefaultExt = "as";
            this.SaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog_FileOk);
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(54, 6);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(0, 13);
            this.FileNameLabel.TabIndex = 14;
            // 
            // BinLoadFileLabel
            // 
            this.BinLoadFileLabel.AutoSize = true;
            this.BinLoadFileLabel.Location = new System.Drawing.Point(435, 358);
            this.BinLoadFileLabel.Name = "BinLoadFileLabel";
            this.BinLoadFileLabel.Size = new System.Drawing.Size(70, 13);
            this.BinLoadFileLabel.TabIndex = 15;
            this.BinLoadFileLabel.Text = "Load from file";
            // 
            // BinFilePathText
            // 
            this.BinFilePathText.Location = new System.Drawing.Point(431, 374);
            this.BinFilePathText.Name = "BinFilePathText";
            this.BinFilePathText.Size = new System.Drawing.Size(289, 20);
            this.BinFilePathText.TabIndex = 16;
            this.BinFilePathText.Text = "Full path to file...";
            // 
            // BinFileBrowseButton
            // 
            this.BinFileBrowseButton.Location = new System.Drawing.Point(726, 373);
            this.BinFileBrowseButton.Name = "BinFileBrowseButton";
            this.BinFileBrowseButton.Size = new System.Drawing.Size(56, 20);
            this.BinFileBrowseButton.TabIndex = 17;
            this.BinFileBrowseButton.Text = "Browse";
            this.BinFileBrowseButton.UseVisualStyleBackColor = true;
            this.BinFileBrowseButton.Click += new System.EventHandler(this.BinFileBrowseButton_Click);
            // 
            // BinSaveFileAsButton
            // 
            this.BinSaveFileAsButton.Location = new System.Drawing.Point(582, 400);
            this.BinSaveFileAsButton.Name = "BinSaveFileAsButton";
            this.BinSaveFileAsButton.Size = new System.Drawing.Size(68, 23);
            this.BinSaveFileAsButton.TabIndex = 20;
            this.BinSaveFileAsButton.Text = "Save As";
            this.BinSaveFileAsButton.UseVisualStyleBackColor = true;
            this.BinSaveFileAsButton.Click += new System.EventHandler(this.BinSaveFileAsButton_Click);
            // 
            // BinSaveFileButton
            // 
            this.BinSaveFileButton.Location = new System.Drawing.Point(505, 400);
            this.BinSaveFileButton.Name = "BinSaveFileButton";
            this.BinSaveFileButton.Size = new System.Drawing.Size(71, 23);
            this.BinSaveFileButton.TabIndex = 19;
            this.BinSaveFileButton.Text = "Save";
            this.BinSaveFileButton.UseVisualStyleBackColor = true;
            this.BinSaveFileButton.Click += new System.EventHandler(this.BinSaveFileButton_Click);
            // 
            // BinFileLoadButton
            // 
            this.BinFileLoadButton.Location = new System.Drawing.Point(431, 400);
            this.BinFileLoadButton.Name = "BinFileLoadButton";
            this.BinFileLoadButton.Size = new System.Drawing.Size(67, 23);
            this.BinFileLoadButton.TabIndex = 18;
            this.BinFileLoadButton.Text = "Load";
            this.BinFileLoadButton.UseVisualStyleBackColor = true;
            this.BinFileLoadButton.Click += new System.EventHandler(this.BinFileLoadButton_Click);
            // 
            // BinSaveFileDialog
            // 
            this.BinSaveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.BinSaveFileDialog_FileOk);
            // 
            // BinLoadFileDialog
            // 
            this.BinLoadFileDialog.FileName = "openFileDialog1";
            this.BinLoadFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.BinLoadFileDialog_FileOk);
            // 
            // BinFileNameLabel
            // 
            this.BinFileNameLabel.AutoSize = true;
            this.BinFileNameLabel.Location = new System.Drawing.Point(505, 6);
            this.BinFileNameLabel.Name = "BinFileNameLabel";
            this.BinFileNameLabel.Size = new System.Drawing.Size(0, 13);
            this.BinFileNameLabel.TabIndex = 21;
            // 
            // ByteCountBox
            // 
            this.ByteCountBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ByteCountBox.Location = new System.Drawing.Point(438, 45);
            this.ByteCountBox.Name = "ByteCountBox";
            this.ByteCountBox.ReadOnly = true;
            this.ByteCountBox.Size = new System.Drawing.Size(32, 310);
            this.ByteCountBox.TabIndex = 22;
            this.ByteCountBox.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(846, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.architectureToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // architectureToolStripMenuItem
            // 
            this.architectureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadArcToolStripMenuItem,
            this.createNewToolStripMenuItem1});
            this.architectureToolStripMenuItem.Name = "architectureToolStripMenuItem";
            this.architectureToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.architectureToolStripMenuItem.Text = "Architecture";
            // 
            // LoadArcToolStripMenuItem
            // 
            this.LoadArcToolStripMenuItem.Name = "LoadArcToolStripMenuItem";
            this.LoadArcToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.LoadArcToolStripMenuItem.Text = "Load from file";
            this.LoadArcToolStripMenuItem.Click += new System.EventHandler(this.LoadArcToolStripMenuItem_Click);
            // 
            // createNewToolStripMenuItem1
            // 
            this.createNewToolStripMenuItem1.Name = "createNewToolStripMenuItem1";
            this.createNewToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.createNewToolStripMenuItem1.Text = "Create new";
            this.createNewToolStripMenuItem1.Click += new System.EventHandler(this.createNewToolStripMenuItem1_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memoryDumpToolStripMenuItem,
            this.registersToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // memoryDumpToolStripMenuItem
            // 
            this.memoryDumpToolStripMenuItem.Name = "memoryDumpToolStripMenuItem";
            this.memoryDumpToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.memoryDumpToolStripMenuItem.Text = "Memory dump";
            this.memoryDumpToolStripMenuItem.Click += new System.EventHandler(this.memoryDumpToolStripMenuItem_Click);
            // 
            // LoadArchitectureDialog
            // 
            this.LoadArchitectureDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.LoadArchitectureDialog_FileOk);
            // 
            // registersToolStripMenuItem
            // 
            this.registersToolStripMenuItem.Name = "registersToolStripMenuItem";
            this.registersToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.registersToolStripMenuItem.Text = "Registers";
            this.registersToolStripMenuItem.Click += new System.EventHandler(this.registersToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 496);
            this.Controls.Add(this.ByteCountBox);
            this.Controls.Add(this.BinFileNameLabel);
            this.Controls.Add(this.BinSaveFileAsButton);
            this.Controls.Add(this.BinSaveFileButton);
            this.Controls.Add(this.BinFileLoadButton);
            this.Controls.Add(this.BinFileBrowseButton);
            this.Controls.Add(this.BinFilePathText);
            this.Controls.Add(this.BinLoadFileLabel);
            this.Controls.Add(this.FileNameLabel);
            this.Controls.Add(this.SaveFileAsButton);
            this.Controls.Add(this.SaveFileButton);
            this.Controls.Add(this.LoadFromFileButton);
            this.Controls.Add(this.ChooseFileLabel);
            this.Controls.Add(this.LoadFileBrowseButton);
            this.Controls.Add(this.LoadFilePathText);
            this.Controls.Add(this.ExecuteButton);
            this.Controls.Add(this.AssemblyButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BinaryCodeBox);
            this.Controls.Add(this.CodeBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MultiArc Compiler";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CodeBox;
        private System.Windows.Forms.RichTextBox BinaryCodeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AssemblyButton;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.OpenFileDialog LoadFileDialog;
        private System.Windows.Forms.TextBox LoadFilePathText;
        private System.Windows.Forms.Button LoadFileBrowseButton;
        private System.Windows.Forms.Label ChooseFileLabel;
        private System.Windows.Forms.Button LoadFromFileButton;
        private System.Windows.Forms.Button SaveFileButton;
        private System.Windows.Forms.Button SaveFileAsButton;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.Label BinLoadFileLabel;
        private System.Windows.Forms.TextBox BinFilePathText;
        private System.Windows.Forms.Button BinFileBrowseButton;
        private System.Windows.Forms.Button BinSaveFileAsButton;
        private System.Windows.Forms.Button BinSaveFileButton;
        private System.Windows.Forms.Button BinFileLoadButton;
        private System.Windows.Forms.SaveFileDialog BinSaveFileDialog;
        private System.Windows.Forms.OpenFileDialog BinLoadFileDialog;
        private System.Windows.Forms.Label BinFileNameLabel;
        private System.Windows.Forms.RichTextBox ByteCountBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem architectureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadArcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog LoadArchitectureDialog;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryDumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registersToolStripMenuItem;

    }
}

