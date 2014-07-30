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
    public partial class MemoryDumpForm : Form
    {
        private Label[] labels;

        private TextBox[] values;

        public MemoryDumpForm()
        {
            InitializeComponent();
            labels = new Label[Program.Mem.Size / 4];
            values = new TextBox[Program.Mem.Size];
            this.Height = 31 * 15;
            //int width = this.constants.NUM_OF_REGISTERS / 10;
            this.Width = 330;
            int xLabel = 12;
            int yLabel = 9;
            int xText = 80;
            int yText = 6;
            for (uint i = 0; i < labels.Length; i++)
            {
                
                labels[i] = new Label();
                labels[i].Size = new Size(70, 13);
                labels[i].Location = new System.Drawing.Point(xLabel, yLabel);
                for (uint j = 0; j < 4; j++)
                {
                    if (j + 4 * i < Program.Mem.Size)
                    {
                        values[j + 4 * i] = new TextBox();
                        values[j + 4 * i].Size = new Size(52, 20);
                        int valueToWrite = 0;
                        byte[] memValue = Program.Mem[j + 4 * i];
                        for (uint k = 0; k < Program.Mem.AuSize; k++)
                        {
                            valueToWrite |= (int)memValue[k] << (int)(k * 8);
                        }
                        values[j + 4 * i].Text = "" + valueToWrite;
                        values[j + 4 * i].Location = new System.Drawing.Point(xText, yText);
                        xText += values[j + 4 * i].Width;
                        this.Controls.Add(values[j + 4 * i]);
                    }
                }
                xText = 80;
                labels[i].Text = "" + i * 4 + "-" + (i * 4 + 3);
                yText += 26;
                yLabel += 26;
                this.Controls.Add(labels[i]);
            }
            /*
            updateButton.Location = new System.Drawing.Point(xText, yText);
            updateButton.Size = new Size(52, 20);
            this.Height += updateButton.Height;
             * */
            this.Visible = true;
            this.AutoScroll = true;
        }
    }
}
