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
    public partial class RegistersForm : Form
    {

        private Label[] registersNames;

        private TextBox[] registersValues;

        private ArchConstants constants;

        public RegistersForm(ArchConstants constants)
        {
            InitializeComponent();
            this.constants = constants;
            registersNames = new Label[this.constants.NUM_OF_REGISTERS];
            registersValues = new TextBox[this.constants.NUM_OF_REGISTERS];
            this.Height = 31 * (this.constants.NUM_OF_REGISTERS >= 10 ? 10 : this.constants.NUM_OF_REGISTERS);
            int width = this.constants.NUM_OF_REGISTERS / 10;
            this.Width = 122 * (this.constants.NUM_OF_REGISTERS % 10 == 0 ? width : width + 1);
            int xLabel = 12;
            int yLabel = 9;
            int xText = 53;
            int yText = 6;
            for (int i = 0; i < this.constants.NUM_OF_REGISTERS; i++)
            {
                if (i != 0 && i % 10 == 0)
                {
                    xText += 118;
                    xLabel += 111;
                    yLabel = 9;
                    yText = 6;
                }
                registersNames[i] = new Label();
                registersNames[i].Size = new Size(35, 13);
                registersNames[i].Location = new System.Drawing.Point(xLabel, yLabel);
                registersValues[i] = new TextBox();
                registersValues[i].Size = new Size(52, 20);
                registersValues[i].Location = new System.Drawing.Point(xText, yText);
                registersNames[i].Text = this.constants.GetRegister(i).Names.ElementAt(0);
                registersValues[i].Text = this.constants.GetRegister(i).Val.ToString();
                yText += 26;
                yLabel += 26;
                this.Controls.Add(registersNames[i]);
                this.Controls.Add(registersValues[i]);
            }
            updateButton.Location = new System.Drawing.Point(xText, yText);
            updateButton.Size = new Size(52, 20);
            this.Height += updateButton.Height;
            this.Visible = true;
        }
    }
}
