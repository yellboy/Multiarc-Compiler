namespace MultiArc_Compiler
{
    partial class FinishArcForm
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
            this.MessageLabel1 = new System.Windows.Forms.Label();
            this.MessageLabel2 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SaveArchitectureDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // MessageLabel1
            // 
            this.MessageLabel1.AutoSize = true;
            this.MessageLabel1.Location = new System.Drawing.Point(13, 13);
            this.MessageLabel1.Name = "MessageLabel1";
            this.MessageLabel1.Size = new System.Drawing.Size(198, 13);
            this.MessageLabel1.TabIndex = 0;
            this.MessageLabel1.Text = "Your architecture is succesfully created. ";
            // 
            // MessageLabel2
            // 
            this.MessageLabel2.AutoSize = true;
            this.MessageLabel2.Location = new System.Drawing.Point(12, 26);
            this.MessageLabel2.Name = "MessageLabel2";
            this.MessageLabel2.Size = new System.Drawing.Size(234, 13);
            this.MessageLabel2.TabIndex = 1;
            this.MessageLabel2.Text = "Do you want to save it or you want to discard it?";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(64, 52);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(68, 25);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(148, 54);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 3;
            this.DiscardButton.Text = "Discard";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // SaveArchitectureDialog
            // 
            this.SaveArchitectureDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveArchitectureDialog_FileOk);
            // 
            // FinishArcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 83);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.MessageLabel2);
            this.Controls.Add(this.MessageLabel1);
            this.Name = "FinishArcForm";
            this.Text = "FinishArcForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MessageLabel1;
        private System.Windows.Forms.Label MessageLabel2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.SaveFileDialog SaveArchitectureDialog;
    }
}