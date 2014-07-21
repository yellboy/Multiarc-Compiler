using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiArc_Compiler
{
    public partial class LengthsForm : Form
    {
        public LengthsForm()
        {
            InitializeComponent();
            this.Visible = true;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            Form1.Constants.MEM_SIZE = Convert.ToInt32(MemorySizeText.Text);
            Form1.Constants.NUM_OF_REGISTERS = Convert.ToInt32(NumRegText.Text);
            Form1.Constants.MAX_BYTES = Convert.ToInt32(MaxInstructionLengthText.Text);
            Form1.Constants.PC_LENGTH = Convert.ToInt32(PCLengthText.Text);
            Form1.Constants.WORD_LENGTH = Convert.ToInt32(WordLengthText.Text);
            Form1.Constants.ADDR_UNIT_SIZE = Convert.ToInt32(AUSizeText.Text);
            this.Visible = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Form1.Instance.RestoreArchitecture();
            Form1.Instance.Enabled = true;
        }
    }
}
