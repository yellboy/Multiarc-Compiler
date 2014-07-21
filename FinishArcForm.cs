using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MultiArc_Compiler
{
    public partial class FinishArcForm : Form
    {
        public FinishArcForm()
        {
            InitializeComponent();
            this.Visible = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveArchitectureDialog.ShowDialog();
            this.Visible = false;
        }

        private void DiscardButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form1.Instance.RestoreArchitecture();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.Visible = false;
            Form1.Instance.Enabled = true;
        }

        private void SaveArchitectureDialog_FileOk(object sender, CancelEventArgs e)
        {
            Form1.Instance.WriteArchitectureToFile(SaveArchitectureDialog.FileName);
            Form1.Instance.Enabled = true;
        }
    }
}
