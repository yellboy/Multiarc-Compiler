namespace MultiArc_Compiler
{
    partial class LengthsForm
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
            this.WordLengthLabel = new System.Windows.Forms.Label();
            this.PCLengthLabel = new System.Windows.Forms.Label();
            this.WordLengthText = new System.Windows.Forms.TextBox();
            this.PCLengthText = new System.Windows.Forms.TextBox();
            this.NumRegText = new System.Windows.Forms.TextBox();
            this.NumRegLabel = new System.Windows.Forms.Label();
            this.MemorySizeText = new System.Windows.Forms.TextBox();
            this.MemSizeLabel = new System.Windows.Forms.Label();
            this.AUSizeText = new System.Windows.Forms.TextBox();
            this.AUSizeLabel = new System.Windows.Forms.Label();
            this.MaxInstructionLengthText = new System.Windows.Forms.TextBox();
            this.MaxInstLengthLabel = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WordLengthLabel
            // 
            this.WordLengthLabel.AutoSize = true;
            this.WordLengthLabel.Location = new System.Drawing.Point(12, 47);
            this.WordLengthLabel.Name = "WordLengthLabel";
            this.WordLengthLabel.Size = new System.Drawing.Size(65, 13);
            this.WordLengthLabel.TabIndex = 0;
            this.WordLengthLabel.Text = "Word length";
            // 
            // PCLengthLabel
            // 
            this.PCLengthLabel.AutoSize = true;
            this.PCLengthLabel.Location = new System.Drawing.Point(12, 73);
            this.PCLengthLabel.Name = "PCLengthLabel";
            this.PCLengthLabel.Size = new System.Drawing.Size(53, 13);
            this.PCLengthLabel.TabIndex = 1;
            this.PCLengthLabel.Text = "PC length";
            // 
            // WordLengthText
            // 
            this.WordLengthText.Location = new System.Drawing.Point(149, 44);
            this.WordLengthText.Name = "WordLengthText";
            this.WordLengthText.Size = new System.Drawing.Size(77, 20);
            this.WordLengthText.TabIndex = 2;
            // 
            // PCLengthText
            // 
            this.PCLengthText.Location = new System.Drawing.Point(149, 70);
            this.PCLengthText.Name = "PCLengthText";
            this.PCLengthText.Size = new System.Drawing.Size(77, 20);
            this.PCLengthText.TabIndex = 3;
            // 
            // NumRegText
            // 
            this.NumRegText.Location = new System.Drawing.Point(149, 96);
            this.NumRegText.Name = "NumRegText";
            this.NumRegText.Size = new System.Drawing.Size(77, 20);
            this.NumRegText.TabIndex = 5;
            // 
            // NumRegLabel
            // 
            this.NumRegLabel.AutoSize = true;
            this.NumRegLabel.Location = new System.Drawing.Point(12, 99);
            this.NumRegLabel.Name = "NumRegLabel";
            this.NumRegLabel.Size = new System.Drawing.Size(123, 13);
            this.NumRegLabel.TabIndex = 4;
            this.NumRegLabel.Text = "Number of CPU registers";
            // 
            // MemorySizeText
            // 
            this.MemorySizeText.Location = new System.Drawing.Point(149, 121);
            this.MemorySizeText.Name = "MemorySizeText";
            this.MemorySizeText.Size = new System.Drawing.Size(77, 20);
            this.MemorySizeText.TabIndex = 7;
            // 
            // MemSizeLabel
            // 
            this.MemSizeLabel.AutoSize = true;
            this.MemSizeLabel.Location = new System.Drawing.Point(12, 124);
            this.MemSizeLabel.Name = "MemSizeLabel";
            this.MemSizeLabel.Size = new System.Drawing.Size(65, 13);
            this.MemSizeLabel.TabIndex = 6;
            this.MemSizeLabel.Text = "Memory size";
            // 
            // AUSizeText
            // 
            this.AUSizeText.Location = new System.Drawing.Point(149, 145);
            this.AUSizeText.Name = "AUSizeText";
            this.AUSizeText.Size = new System.Drawing.Size(77, 20);
            this.AUSizeText.TabIndex = 9;
            // 
            // AUSizeLabel
            // 
            this.AUSizeLabel.AutoSize = true;
            this.AUSizeLabel.Location = new System.Drawing.Point(12, 148);
            this.AUSizeLabel.Name = "AUSizeLabel";
            this.AUSizeLabel.Size = new System.Drawing.Size(102, 13);
            this.AUSizeLabel.TabIndex = 8;
            this.AUSizeLabel.Text = "Addressible unit size";
            // 
            // MaxInstructionLengthText
            // 
            this.MaxInstructionLengthText.Location = new System.Drawing.Point(149, 170);
            this.MaxInstructionLengthText.Name = "MaxInstructionLengthText";
            this.MaxInstructionLengthText.Size = new System.Drawing.Size(77, 20);
            this.MaxInstructionLengthText.TabIndex = 11;
            // 
            // MaxInstLengthLabel
            // 
            this.MaxInstLengthLabel.AutoSize = true;
            this.MaxInstLengthLabel.Location = new System.Drawing.Point(12, 173);
            this.MaxInstLengthLabel.Name = "MaxInstLengthLabel";
            this.MaxInstLengthLabel.Size = new System.Drawing.Size(136, 13);
            this.MaxInstLengthLabel.TabIndex = 10;
            this.MaxInstLengthLabel.Text = "Maximum linstruction length";
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(151, 199);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 12;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // LengthsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 231);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.MaxInstructionLengthText);
            this.Controls.Add(this.MaxInstLengthLabel);
            this.Controls.Add(this.AUSizeText);
            this.Controls.Add(this.AUSizeLabel);
            this.Controls.Add(this.MemorySizeText);
            this.Controls.Add(this.MemSizeLabel);
            this.Controls.Add(this.NumRegText);
            this.Controls.Add(this.NumRegLabel);
            this.Controls.Add(this.PCLengthText);
            this.Controls.Add(this.WordLengthText);
            this.Controls.Add(this.PCLengthLabel);
            this.Controls.Add(this.WordLengthLabel);
            this.Name = "LengthsForm";
            this.Text = "CPU constants";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WordLengthLabel;
        private System.Windows.Forms.Label PCLengthLabel;
        private System.Windows.Forms.TextBox WordLengthText;
        private System.Windows.Forms.TextBox PCLengthText;
        private System.Windows.Forms.TextBox NumRegText;
        private System.Windows.Forms.Label NumRegLabel;
        private System.Windows.Forms.TextBox MemorySizeText;
        private System.Windows.Forms.Label MemSizeLabel;
        private System.Windows.Forms.TextBox AUSizeText;
        private System.Windows.Forms.Label AUSizeLabel;
        private System.Windows.Forms.TextBox MaxInstructionLengthText;
        private System.Windows.Forms.Label MaxInstLengthLabel;
        private System.Windows.Forms.Button NextButton;
    }
}