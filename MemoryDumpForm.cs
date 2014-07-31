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

        private uint beginAddress;

        private uint endAddress;

        public MemoryDumpForm()
        {
            InitializeComponent();
            beginAddress = 0;
            endAddress = Program.Mem.Size >= 100 ? 100 : (uint)Program.Mem.Size;
            labels = new Label[endAddress / 10];
            values = new TextBox[endAddress];
            readNewValues();
            this.Visible = true;
            prevButton.Enabled = false;
        }

        private void readNewValues()
        {
            int xLabel = 12;
            int yLabel = 9;
            int xText = 80;
            int yText = 6;
            for (uint i = 0; i < labels.Length; i++)
            {
                this.Controls.Remove(labels[i]);
                if (beginAddress + 10 * i < Program.Mem.Size)
                {
                    labels[i] = new Label();
                    labels[i].Size = new Size(70, 13);
                    labels[i].Location = new System.Drawing.Point(xLabel, yLabel);
                    for (uint j = 0; j < 10; j++)
                    {
                        this.Controls.Remove(values[j + 10 * i]);
                        if (beginAddress + j + 10 * i < Program.Mem.Size)
                        {
                            values[j + 10 * i] = new TextBox();
                            values[j + 10 * i].Size = new Size(52, 20);
                            int valueToWrite = 0;
                            byte[] memValue = Program.Mem[beginAddress + j + 10 * i];
                            for (uint k = 0; k < Program.Mem.AuSize; k++)
                            {
                                valueToWrite |= (int)memValue[k] << (int)(k * 8);
                            }
                            values[j + 10 * i].Text = "" + valueToWrite;
                            values[j + 10 * i].Location = new System.Drawing.Point(xText, yText);
                            xText += values[j + 10 * i].Width;
                            this.Controls.Add(values[j + 10 * i]);
                        }
                    }
                    xText = 80;
                    labels[i].Text = "" + (beginAddress + i * 10) + "-" + (beginAddress + i * 10 + 9 >= Program.Mem.Size ? (int)(Program.Mem.Size - 1) : (int)(beginAddress + i * 10 + 9));
                    yText += 26;
                    yLabel += 26;
                    this.Controls.Add(labels[i]);
                }
                else
                {
                    for (uint j = 0; j < 10; j++)
                    {
                        this.Controls.Remove(values[j + 10 * i]);
                    }
                }
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            prevButton.Enabled = true;
            beginAddress += 100;
            endAddress += 100;
            if (endAddress >= Program.Mem.Size)
            {
                nextButton.Enabled = false;
            }
            readNewValues();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            nextButton.Enabled = true;
            beginAddress -= 100;
            endAddress -= 100;
            if (beginAddress <= 0)
            {
                prevButton.Enabled = false;
            }
            readNewValues();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            for (uint i = 0; i < labels.Length; i++)
            {
                if (beginAddress + 10 * i < Program.Mem.Size)
                {
                    for (uint j = 0; j < 10; j++)
                    {
                        if (beginAddress + j + 10 * i < Program.Mem.Size)
                        {
                            int valueToWrite = Convert.ToInt32(values[j + 10 * i].Text);
                            byte[] memValue = new byte[Program.Mem.AuSize];
                            for (int k = 0; k < Program.Mem.AuSize; k++)
                            {
                                memValue[k] = (byte)(valueToWrite << 8 * k);
                            }
                            Program.Mem[beginAddress + j + 10 * i] = memValue;
                        }
                    }
                }
            }
        }
    }
}
