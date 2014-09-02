using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using PerCederberg.Grammatica.Runtime;
using PerCederberg.Grammatica.Runtime.RE;
using System.Reflection;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using System.Threading;
using System.Runtime.InteropServices;

namespace MultiArc_Compiler
{
    public partial class Form1 : Form
    {

        private string arcFileName = null;

        public string ArcFileName
        {
            get
            {
                return arcFileName;
            }
        }

        /// <summary>
        /// Contants that represents whole architecture.
        /// </summary>
        private static ArchConstants constants = new ArchConstants();

        public static ArchConstants Constants
        {
            get
            {
                return constants;
            }
        }

        public static string openedFileName = null;

        public static string openedBinFileName = null;

        private byte[] binary;

        private LinkedList<int> separators;

        public static Form1 Instance = null;

        private LinkedList<int> breakPoints = new LinkedList<int>();

        private bool compiled = false;

        private Form registersForm;

        private Form memoryForm; 

        public Form1()
        {
            InitializeComponent();
            Instance = this;
            CodeBox.AppendText("  ");
            CodeBox.Clear();
            loadToolStripMenuItem.Enabled = false;
            recompileCodeToolStripMenuItem.Enabled = false;
            registersToolStripMenuItem.Enabled = false;
            memoryDumpToolStripMenuItem.Enabled = false;
            executeToolStripMenuItem.Enabled = false;
            executeWithoutDebugToolStripMenuItem.Enabled = false;
            nextStepToolStripMenuItem.Enabled = false;
            LoadArcButton.Enabled = false;
            assembleToolStripMenuItem.Enabled = false;
            DebugButton.Enabled = false;
            fileChanged = false;
            compiled = false;
        }

        private int entryPoint;

        private void AssemblyButton_Click(object sender, EventArgs e)
        {
            Assembler asm = new Assembler(CodeBox.Text, constants, Program.Mem, OutputBox);
            binary = asm.Assemble();
            entryPoint = asm.Origin;
            if (binary != null)
            {
                BinaryCodeBox.Text = "";
                separators = asm.Separators;
                long maxB = binary.Length / Program.Mem.AuSize + entryPoint;
                int count = 0;
                while (maxB != 0)
                {
                    count++;
                    maxB /= 10;
                }
                BinaryCodeBox.Text = String.Format("{0:D" + count + "}", entryPoint) + ":\t";
                for (int i = 0; i < asm.Count; i++)
                {
                    if (separators.Contains(i) && i != 0)
                    {
                        BinaryCodeBox.Text += "\n" + String.Format("{0:D" + count + "}", (i / Program.Mem.AuSize + entryPoint)) + ":\t";
                    }
                    string toAdd = BitConverter.ToString(binary, i, 1);
                    BinaryCodeBox.Text += toAdd; // Convert byte to hex string and write it to binary code box.
                } 
                BinaryCodeBox.Text += "\n" + String.Format("{0:D" + count + "}", (asm.Count / Program.Mem.AuSize + entryPoint)) + ":\t";
                compiled = true;
            }
        }

        private bool fileChanged;

        private void LoadFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            openedFileName = LoadFileDialog.FileName;
            fileChanged = false;
            compiled = false;
            string[] code;
            code = File.ReadAllLines(LoadFileDialog.FileName);
            CodeBox.Text = "";
            breakPoints.Clear();
            for (int i = 0; i < code.Length; i++)
            {
                CodeBox.Text += code[i] + '\n';
            }
            fileChanged = false;
        }

        private void LoadFileBrowseButton_Click(object sender, EventArgs e)
        {
            LoadFileDialog.ShowDialog();
        }

        private void LoadFromFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] code = new string[3];
                breakPoints.Clear();
                CodeBox.Text = "";
                for (int i = 0; i < code.Length; i++)
                {
                    CodeBox.Text += code[i] + '\n';
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File not found! ");
            }
        }

        private void SaveFileAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog.ShowDialog();
        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(SaveFileDialog.FileName, CodeBox.Text);
            openedFileName = SaveFileDialog.FileName;
            FileNameLabel.Text = "File: " + SaveFileDialog.FileName;
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            if (openedFileName == null)
            {
                SaveFileDialog.ShowDialog();
            }
            else
            {
                File.WriteAllText(openedFileName, CodeBox.Text);
            }
        }

        private void BinLoadFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            binary = File.ReadAllBytes(BinLoadFileDialog.FileName);
            openedBinFileName = BinLoadFileDialog.FileName;
            BinFileNameLabel.Text = openedBinFileName;
            BinaryCodeBox.Text = "";
            int byteCount = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == (byte)('\n'))
                {
                    BinaryCodeBox.Text += "\n";
                }
                else
                {
                    BinaryCodeBox.Text += BitConverter.ToString(binary, i, 1); // Convert byte to hex string and write it to code box.
                    byteCount++;
                }
            }
        }

        private void BinSaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllBytes(BinSaveFileDialog.FileName, binary);
            openedBinFileName = BinSaveFileDialog.FileName;
            BinFileNameLabel.Text = "File: " + openedBinFileName;
        }

        private void BinSaveFileAsButton_Click(object sender, EventArgs e)
        {
            BinSaveFileDialog.ShowDialog();
        }

        private void BinFileBrowseButton_Click(object sender, EventArgs e)
        {
            BinLoadFileDialog.ShowDialog();
        }

        private void BinSaveFileButton_Click(object sender, EventArgs e)
        {
            if (openedBinFileName == null)
            {
                BinSaveFileDialog.ShowDialog();
            }
            else
            {
                File.WriteAllBytes(openedBinFileName, binary);
            }
        }

        private void BinFileLoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                //binary = File.ReadAllBytes(BinFilePathText.Text);
                //openedBinFileName = BinFilePathText.Text;
                BinFileNameLabel.Text = openedBinFileName;
                BinaryCodeBox.Text = "";
                int byteCount = 0;
                for (int i = 0; i < binary.Length; i++)
                {
                    if (binary[i] == (byte)('\n'))
                    {
                        BinaryCodeBox.Text += "\n";
                    }
                    else
                    {
                        BinaryCodeBox.Text += BitConverter.ToString(binary, i, 1); // Convert byte to hex string and write it to code box.
                        byteCount++;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("File not found! ");
            }
        }

        private ArchConstants savedConstants = null;

        /// <summary>
        /// Saves current architecture constants so that they can be restored later if it is neccessary.
        /// </summary>
        public void SaveArchitecture()
        {
            savedConstants = (ArchConstants)(constants.Clone());
        }

        /// <summary>
        /// Restores old architecture constants.
        /// </summary>
        public void RestoreArchitecture()
        {
            constants = savedConstants;
        }

        private void LoadArcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadArchitectureDialog.ShowDialog();
        }

        private void LoadArchitectureDialog_FileOk(object sender, CancelEventArgs e)
        {
            recompileCodeToolStripMenuItem.Enabled = true;
            int errorCount = 0;
            string fileName;
            if (arcFileName == null || projectOpenning == false)
            {
                fileName = LoadArchitectureDialog.FileName;
            }
            else
            {
                fileName = arcFileName;
            }
            string content = File.ReadAllText(fileName);
            try
            {
                SaveArchitecture();
                constants.RemoveAllInstructions();
                constants.RemoveAllAddressingModes();
                constants.RemoveAllDataTypes();
                constants.RemoveAllMnemonics();
                constants.RemoveAllRegisters();
                constants.ClearTokens();
                XmlReader xmlReader = XmlReader.Create(new StringReader(content));
                XmlDocument doc = new XmlDocument();
                XmlNode head = doc.ReadNode(xmlReader);
                registersForm = null;
                memoryForm = null;
                bool registersSpecified = false;
                foreach (XmlNode node in head.ChildNodes)
                {
                    XmlNodeList list = null;
                    switch (node.Name)
                    {
                        case "name":
                            constants.Name = node.InnerText;
                            break;
                        case "instruction_mnemonics":
                            foreach (XmlNode name in node.ChildNodes)
                                if (name.Name.Equals("name"))
                                    constants.AddMnemonic(name.InnerText);
                            break;
                        case "user_tokens":
                            foreach (XmlNode token in node.ChildNodes)
                            {
                                if (!token.Name.Equals("#whitespace"))
                                {
                                    constants.AddToken(token.Name, token.InnerText);
                                }
                            }
                            break;
                        case "memory":
                            int memoryErrorCount = 0;
                            if (node.HasChildNodes)
                            {
                                list = node.ChildNodes;
                                Program.Mem = new Memory();
                            }
                            foreach (XmlNode n in list)
                            {
                                switch (n.Name)
                                {
                                    case "size":
                                        Program.Mem.Size = Convert.ToInt32(n.InnerText);
                                        break;
                                    case "au":
                                        Program.Mem.AuSize = Convert.ToInt32(n.InnerText);
                                        break;
                                    case "ram_start":
                                        Program.Mem.RamStart = Convert.ToUInt32(n.InnerText);
                                        break;
                                    case "ram_end":
                                        Program.Mem.RamEnd = Convert.ToUInt32(n.InnerText);;
                                        break;
                                    case "rom_start":
                                        Program.Mem.RomStart = Convert.ToUInt32(n.InnerText);
                                        break;
                                    case "rom_end":
                                        Program.Mem.RomEnd = Convert.ToUInt32(n.InnerText);
                                        break;
                                    case "init_file":
                                        Program.Mem.InitFile = dataFolder + n.InnerText;
                                        break;
                                    case "storage_file":
                                        Program.Mem.StorageFile = dataFolder + n.InnerText;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (Program.Mem.Size == -1)
                            {
                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: Memory size must be specified.\n";
                                OutputBox.ScrollToCaret();
                                memoryErrorCount++;
                            }
                            else if (Program.Mem.AuSize == -1)
                            {
                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: Addressible unit size in memory must be specified.\n";
                                OutputBox.ScrollToCaret();
                                memoryErrorCount++;
                            }
                            errorCount += memoryErrorCount;
                            if (memoryErrorCount == 0)
                            {
                                Program.Mem.Initialize();
                            }
                            break;
                        case "data":
                            list = node.ChildNodes;
                            foreach (XmlNode n in list)
                            {
                                if (!n.Name.Equals("#whitespace"))
                                {
                                    constants.AddDataType(n.Name, Convert.ToInt32(n.InnerText));
                                }
                            }
                            break;
                        case "registers":
                            list = node.ChildNodes;
                            registersSpecified = true;
                            int registerErrorCount = 0;
                            foreach (XmlNode n in list)
                            {
                                switch (n.Name)
                                {
                                    case "general_purpose":
                                        {
                                            string prefix = "R";
                                            string group = "";
                                            int size = 0;
                                            int number = 0;
                                            int value = 0;
                                            XmlNodeList children = n.ChildNodes;
                                            foreach (XmlNode child in children)
                                            {
                                                switch (child.Name)
                                                {
                                                    case "number":
                                                        number = Convert.ToInt32(child.InnerText);
                                                        break;
                                                    case "size":
                                                        size = Convert.ToInt32(child.InnerText);
                                                        break;
                                                    case "prefix":
                                                        prefix = child.InnerText;
                                                        break;
                                                    case "value":
                                                        value = Convert.ToInt32(child.InnerText);
                                                        break;
                                                    case "name":
                                                        group = child.InnerText;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            if (group == "")
                                            {
                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: General purpose registers' group name must be specified.\n";
                                                OutputBox.ScrollToCaret();
                                                registerErrorCount++;
                                            }
                                            if (number == 0)
                                            {
                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: General purpose registers' number must be specified.\n";
                                                OutputBox.ScrollToCaret();
                                                registerErrorCount++;
                                            }
                                            if (size == 0)
                                            {
                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: General purpose registers' size must be specified.\n";
                                                OutputBox.ScrollToCaret();
                                                registerErrorCount++;
                                            }
                                            if (registerErrorCount == 0)
                                            {
                                                for (int i = 0; i < number; i++)
                                                {
                                                    Register r = new Register(size, (IRegistersObserver)registersForm);
                                                    r.Size = size;
                                                    r.AddName(prefix + i);
                                                    r.Val = value;
                                                    r.Group = group;
                                                    constants.AddRegister(r);
                                                }
                                            }
                                        }
                                        break;
                                    case "#whitespace":
                                        break;
                                    default:
                                        {
                                            Register r = new Register(0, (IRegistersObserver)registersForm);
                                            r.Size = 0;
                                            r.Val = 0;
                                            r.Group = null;
                                            XmlNodeList children = n.ChildNodes;
                                            foreach (XmlNode child in children)
                                            {
                                                switch (child.Name)
                                                {
                                                    case "size":
                                                        r.Size = Convert.ToInt32(child.InnerText);
                                                        break;
                                                    case "name":
                                                        r.AddName(child.InnerText);
                                                        break;
                                                    case "group":
                                                        r.Group = child.InnerText;
                                                        break;
                                                    case "value":
                                                        r.Val = Convert.ToInt32(child.InnerText);
                                                        break;
                                                    case "part":
                                                        XmlNodeList part = child.ChildNodes;
                                                        Register partReg = new Register(0, (IRegistersObserver)registersForm);
                                                        partReg.Size = 0;
                                                        partReg.BaseReg = r;
                                                        partReg.Group = null;
                                                        partReg.Start = -1;
                                                        partReg.End = -1;
                                                        foreach (XmlNode partParameter in part)
                                                        {
                                                            switch (partParameter.Name)
                                                            {
                                                                case "start":
                                                                    partReg.Start = Convert.ToInt32(partParameter.InnerText);
                                                                    break;
                                                                case "end":
                                                                    partReg.End = Convert.ToInt32(partParameter.InnerText);
                                                                    break;
                                                                case "group":
                                                                    partReg.Group = partParameter.InnerText;
                                                                    break;
                                                                case "name":
                                                                    partReg.AddName(partParameter.InnerText);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                        if (partReg.Start == -1)
                                                        {
                                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + n.Name + "'s part start must be specified.\n";
                                                            OutputBox.ScrollToCaret();
                                                            registerErrorCount++;
                                                        }
                                                        if (partReg.End == -1)
                                                        {
                                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + n.Name + "'s part end must be specified.\n";
                                                            OutputBox.ScrollToCaret();
                                                            registerErrorCount++;
                                                        }
                                                        if (partReg.Names.Count == 0)
                                                        {
                                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + n.Name + "'s part name(s) must be specified.\n";
                                                            OutputBox.ScrollToCaret();
                                                            registerErrorCount++;
                                                        }
                                                        if (registerErrorCount == 0)
                                                        {
                                                            partReg.Size = partReg.End - partReg.Start + 1;
                                                            r.AddPart(partReg);
                                                            constants.AddRegister(partReg);
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            if (r.Size == 0)
                                            {
                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + n.Name + "'s size must be specified.\n";
                                                OutputBox.ScrollToCaret();
                                                registerErrorCount++;
                                            }
                                            if (r.Names.Count == 0)
                                            {
                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + n.Name + "'s name(s) must be specified.\n";
                                                OutputBox.ScrollToCaret();
                                                registerErrorCount++;
                                            }
                                            if (registerErrorCount == 0)
                                            {
                                                constants.AddRegister(r);
                                            }
                                        }
                                        break;
                                }
                            }
                            errorCount += registerErrorCount;
                            break;
                        case "addressing_modes":
                            {
                                int amErrorCount = 0;
                                XmlNodeList modes = node.ChildNodes;
                                foreach (XmlNode mode in modes)
                                {
                                    if (!mode.Name.Equals("#whitespace"))
                                    {
                                        amErrorCount = 0;
                                        AddressingMode am = new AddressingMode();
                                        XmlNodeList children = mode.ChildNodes;
                                        foreach (XmlNode child in children)
                                        {
                                            switch (child.Name)
                                            {
                                                case "name":
                                                    am.Name = child.InnerText;
                                                    break;
                                                case "file":
                                                    am.FileName = dataFolder + child.InnerText;
                                                    break;
                                                case "result":
                                                    am.Result = constants.GetDataType(child.InnerText);
                                                    break;
                                                case "expression_value":
                                                    {
                                                        string exp = "";
                                                        int val = 0;
                                                        foreach (XmlNode valNode in child.ChildNodes)
                                                        {
                                                            switch (valNode.Name)
                                                            {
                                                                case "expression":
                                                                    exp = valNode.InnerText;
                                                                    break;
                                                                case "value":
                                                                    val = Convert.ToInt32(valNode.InnerText);
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                        am.AddValue(exp.ToLower(), val);
                                                    }
                                                    break;
                                                case "operand":
                                                    if (child.HasChildNodes)
                                                    {
                                                        foreach (XmlNode bin in child.ChildNodes)
                                                        {
                                                            switch (bin.InnerText)
                                                            {
                                                                case "user_defined":
                                                                    am.OperandValueDefinedByUser = true;
                                                                    break;
                                                                case "read_from_expression":
                                                                    am.OperandReadFromExpression = true;
                                                                    break;
                                                                case "read_from_values":
                                                                    am.OperandInValues = true;
                                                                    break;
                                                                default:
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case "operand_type":
                                                    am.OperandType = child.InnerText;
                                                    break;
                                                case "expression":
                                                    if (child.HasChildNodes)
                                                    {
                                                        string exp = "";
                                                        LinkedList<string> groups = new LinkedList<string>();
                                                        XmlNodeList expressions = child.ChildNodes;
                                                        foreach (XmlNode expression in expressions)
                                                        {
                                                            switch (expression.Name)
                                                            {
                                                                case "registers_group":
                                                                    am.OperandReadFromExpression = false;
                                                                    am.OperandInValues = true;
                                                                    groups.AddLast(expression.InnerText);
                                                                    exp += expression.InnerText;
                                                                    break;
                                                                case "#whitespace":
                                                                    break;
                                                                default:
                                                                    {
                                                                        string semiExp = expression.InnerText;
                                                                        int index = 0;
                                                                        while (semiExp[index] == '\t' || semiExp[index] == '\n')
                                                                        {
                                                                            index++;
                                                                        }
                                                                        semiExp = semiExp.Substring(index, semiExp.Length - index);
                                                                        index = semiExp.Length - 1;
                                                                        while (semiExp[index] == '\t' || semiExp[index] == '\n')
                                                                        {
                                                                            index--;
                                                                        }
                                                                        semiExp = semiExp.Substring(0, index + 1);
                                                                        exp += semiExp;
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        int lastVal = 0;
                                                        if (groups.Count != 0)
                                                        {
                                                            foreach (string group in groups)
                                                            {
                                                                for (int i = 0; i < constants.NUM_OF_REGISTERS; i++)
                                                                {
                                                                    Register r = constants.GetRegister(i);
                                                                    if (r.Group != null && r.Group.ToLower().Equals(group.ToLower()))
                                                                    {

                                                                        while (am.Values.ContainsValue(lastVal))
                                                                            lastVal++;
                                                                        for (int j = 0; j < r.Names.Count; j++)
                                                                        {
                                                                            string expToAdd = "";
                                                                            int ind = exp.IndexOf(r.Group);
                                                                            expToAdd = exp.Replace(r.Group, r.Names.ElementAt(j));
                                                                            am.AddExpression(expToAdd);
                                                                            string[] expParts = expToAdd.Split('"', ' ');
                                                                            expToAdd = "";
                                                                            for (int c = 0; c < expParts.Length; c++)
                                                                            {
                                                                                expToAdd += expParts[c];
                                                                            }
                                                                            if (!am.Values.ContainsKey(expToAdd.ToLower()))
                                                                            {
                                                                                am.Values.Add(expToAdd.ToLower(), lastVal);
                                                                            }
                                                                        }
                                                                        lastVal++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            am.AddExpression(exp);
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        if (am.Name == null)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s name must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        if (am.FileName == null)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s file must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        if (am.OperandInValues == false && am.OperandReadFromExpression == false && am.OperandValueDefinedByUser == false)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s operand must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        if (am.OperandType == null)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s operand type must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        else if (!am.OperandType.Equals("relative") && !am.OperandType.Equals("absolute"))
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s operand type can only be 'absolute' or 'relative'.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        if (am.Expressions.Count == 0 && am.Values.Count == 0)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + mode.Name + "'s expression must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            amErrorCount++;
                                        }
                                        if (amErrorCount == 0)
                                        {
                                            constants.AddAddressingMode(am);
                                        }
                                        if (amErrorCount == 0)
                                        {
                                            string contents =
@"/* 
 *This is auto-generated text. 
 *Please, edit only method bodies. 
 */

public static void getAddrData_" + am.Name + @"(InstructionRegister ir, Memory memory, ArchConstants constants, Variables variables, int startBit, int endBit, ref int result)
{
	// TODO Write how this addressing mode gets operand here.
	// It this method is not necessary, just leave it like this.
}

public static void setAddrData_" + am.Name + @"(InstructionRegister ir, Memory memory, ArchConstants constants, Variables variables, int startBit, int endBit, int data)
{
	// TODO Write how this addressing mode stores operand here.
	// It this method is not necessary, just leave it like this.
}

public static void getOperand_" + am.Name + @"(string image, int currentLocation, int relativeValue, int absoluteValue, ref int operand)
{
	// TODO Write how this addressing mode gets operand value from instruction assembly code here.
	// If this method is not necessary, just leave it like this.
}";
                                            if (!File.Exists(am.FileName))
                                            {
                                                var file = File.Create(am.FileName);
                                                file.Close();
                                                File.WriteAllText(am.FileName, contents);
                                            }
                                        }
                                        errorCount += amErrorCount;
                                    }
                                }
                            }
                            break;
                        case "instructions":
                            {
                                int instErrorCount = 0;
                                foreach (XmlNode instruction in node.ChildNodes)
                                {
                                    if (!instruction.Name.Equals("#whitespace"))
                                    {
                                        Instruction i = new Instruction();
                                        i.Name = instruction.Name;
                                        i.StartBit = -1;
                                        i.EndBit = -1;
                                        i.Size = -1;
                                        bool opcodeSpecified = false;
                                        foreach (XmlNode child in instruction.ChildNodes)
                                        {
                                            switch (child.Name)
                                            {
                                                case "size":
                                                    i.Size = Convert.ToInt32(child.InnerText);
                                                    break;
                                                case "opcode":
                                                    XmlNodeList opCodeList = child.ChildNodes;
                                                    int size = 0;
                                                    int value = -1;
                                                    opcodeSpecified = true;
                                                    foreach (XmlNode opCodeNode in opCodeList)
                                                    {
                                                        switch (opCodeNode.Name)
                                                        {
                                                            case "start_bit":
                                                                i.StartBit = Convert.ToInt32(opCodeNode.InnerText);
                                                                break;
                                                            case "end_bit":
                                                                i.EndBit = Convert.ToInt32(opCodeNode.InnerText);
                                                                break;
                                                            case "value":
                                                                value = Convert.ToInt32(opCodeNode.InnerText, 2);
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    if (i.StartBit == -1)
                                                    {
                                                        OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s start bit of opcode must be specified.\n";
                                                        OutputBox.ScrollToCaret();
                                                        instErrorCount++;
                                                    }
                                                    if (i.EndBit == -1)
                                                    {
                                                        OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s end bit of opcode must be specified.\n";
                                                        OutputBox.ScrollToCaret();
                                                        instErrorCount++;
                                                    }
                                                    if (value == -1)
                                                    {
                                                        OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s value of opcode must be specified.\n";
                                                        OutputBox.ScrollToCaret();
                                                        instErrorCount++;
                                                    }
                                                    if (instErrorCount == 0)
                                                    {
                                                        size = 1;
                                                        for (int j = i.StartBit; j >= i.EndBit; j--)
                                                        {
                                                            if (j % 8 == 0 && j != i.EndBit)
                                                            {
                                                                size++;
                                                            }
                                                        }
                                                        i.Mask = new byte[size];
                                                        int count = i.StartBit - i.EndBit;
                                                        int byteCount = size - 1;
                                                        for (int j = i.StartBit; j >= i.EndBit; j--)
                                                        {
                                                            int semiValue = (value & (1 << count)) << i.EndBit % 8;
                                                            i.Mask[size - 1 - byteCount] |= (byte)((semiValue & (1 << (i.EndBit % 8 + count))) >> byteCount * 8); // This might be a problem.
                                                            if ((i.EndBit + count) % 8 == 0)
                                                                byteCount--;
                                                            count--;
                                                        }
                                                    }
                                                    break;
                                                case "mnemonic":
                                                    i.Mnemonic = child.InnerText;
                                                    break;
                                                case "file":
                                                    i.FileName = dataFolder + child.InnerText;
                                                    break;
                                                case "arguments":
                                                    {

                                                        XmlNodeList args = child.ChildNodes;
                                                        foreach (XmlNode arg in args)
                                                        {
                                                            if (arg.Name.Equals("arg"))
                                                            {
                                                                Argument a = new Argument();
                                                                bool addressingModeSpecified = false;
                                                                foreach (XmlNode argNode in arg.ChildNodes)
                                                                {
                                                                    switch (argNode.Name)
                                                                    {
                                                                        case "type":
                                                                            a.Type = argNode.InnerText;
                                                                            break;
                                                                        case "addressing_mode":
                                                                            {
                                                                                int codeStart = -1;
                                                                                int codeEnd = -1;
                                                                                int operandStart = -1;
                                                                                int operandEnd = -1;
                                                                                int codeValue = -1;
                                                                                addressingModeSpecified = true;
                                                                                string name = null;
                                                                                XmlNodeList amNodes = argNode.ChildNodes;
                                                                                foreach (XmlNode amNode in amNodes)
                                                                                {
                                                                                    switch (amNode.Name)
                                                                                    {
                                                                                        case "name":
                                                                                            name = amNode.InnerText;
                                                                                            break;
                                                                                        case "opcode":
                                                                                            {
                                                                                                codeStart = -1;
                                                                                                codeEnd = -1;
                                                                                                XmlNodeList opcodeList = amNode.ChildNodes;
                                                                                                foreach (XmlNode opcodeNode in opcodeList)
                                                                                                {
                                                                                                    switch (opcodeNode.Name)
                                                                                                    {
                                                                                                        case "start_bit":
                                                                                                            codeStart = Convert.ToInt32(opcodeNode.InnerText);
                                                                                                            break;
                                                                                                        case "end_bit":
                                                                                                            codeEnd = Convert.ToInt32(opcodeNode.InnerText);
                                                                                                            break;
                                                                                                        case "value":
                                                                                                            codeValue = Convert.ToInt32(opcodeNode.InnerText, 2);
                                                                                                            break;
                                                                                                        default:
                                                                                                            break;
                                                                                                    }
                                                                                                }
                                                                                                if (codeEnd == -1)
                                                                                                {
                                                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument code start bit must be specified, if opcode is specified.\n";
                                                                                                    OutputBox.ScrollToCaret();
                                                                                                    instErrorCount++;
                                                                                                }
                                                                                                if (codeStart == -1)
                                                                                                {
                                                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument code end bit must be specified, if opcode is specified.\n";
                                                                                                    OutputBox.ScrollToCaret();
                                                                                                    instErrorCount++;
                                                                                                }
                                                                                                if (codeValue == -1)
                                                                                                {
                                                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument code value must be specified, if opcode is specified.\n";
                                                                                                    OutputBox.ScrollToCaret();
                                                                                                    instErrorCount++;
                                                                                                }

                                                                                            }
                                                                                            break;
                                                                                        case "operand":
                                                                                            {
                                                                                                XmlNodeList operandList = amNode.ChildNodes;
                                                                                                operandEnd = -1;
                                                                                                operandStart = -1;
                                                                                                foreach (XmlNode operandNode in operandList)
                                                                                                {
                                                                                                    switch (operandNode.Name)
                                                                                                    {
                                                                                                        case "start_bit":
                                                                                                            operandStart = Convert.ToInt32(operandNode.InnerText);
                                                                                                            break;
                                                                                                        case "end_bit":
                                                                                                            operandEnd = Convert.ToInt32(operandNode.InnerText);
                                                                                                            break;
                                                                                                        default:
                                                                                                            break;
                                                                                                    }
                                                                                                }

                                                                                            }
                                                                                            if (operandEnd == -1)
                                                                                            {
                                                                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument operand start bit must be specified, if operand is specified.\n";
                                                                                                OutputBox.ScrollToCaret();
                                                                                                instErrorCount++;
                                                                                            }
                                                                                            if (operandStart == -1)
                                                                                            {
                                                                                                OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument operand end bit must be specified, if operand is specified.\n";
                                                                                                OutputBox.ScrollToCaret();
                                                                                                instErrorCount++;
                                                                                            }
                                                                                            break;
                                                                                        default:
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                if (name == null)
                                                                                {
                                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument addressing mode name must be specified.\n";
                                                                                    OutputBox.ScrollToCaret();
                                                                                    instErrorCount++;
                                                                                }
                                                                                if (constants.GetAddressingMode(name) == null)
                                                                                {
                                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument addressing mode must be previously created.\n";
                                                                                    OutputBox.ScrollToCaret();
                                                                                    instErrorCount++;
                                                                                }
                                                                                if (instErrorCount == 0)
                                                                                {
                                                                                    a.AddAddressingMode(constants.GetAddressingMode(name), codeValue, codeStart, codeEnd, operandStart, operandEnd);
                                                                                }
                                                                            }
                                                                            break;
                                                                        default:
                                                                            break;
                                                                    }
                                                                }
                                                                if (a.Type == null)
                                                                {
                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument's type must be specified.\n";
                                                                    OutputBox.ScrollToCaret();
                                                                    instErrorCount++;
                                                                }
                                                                else if (!a.Type.Equals("src") && !a.Type.Equals("dst"))
                                                                {
                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument's type must be either 'src' or 'dst'.\n";
                                                                    OutputBox.ScrollToCaret();
                                                                    instErrorCount++;
                                                                }
                                                                if (addressingModeSpecified == false)
                                                                {
                                                                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Every " + instruction.Name + "'s argument's addressing mode must be specified.\n";
                                                                    OutputBox.ScrollToCaret();
                                                                    instErrorCount++;
                                                                }
                                                                if (instErrorCount == 0)
                                                                {
                                                                    i.AddArgument(a);
                                                                }
                                                            }

                                                        }
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        if (i.Size == -1)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s size must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            instErrorCount++;
                                        }
                                        if (opcodeSpecified == false)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s opcode must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            instErrorCount++;
                                        }
                                        if (i.Mnemonic == null)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s mnemonic must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            instErrorCount++;
                                        }
                                        if (!constants.Mnemonics.Contains(i.Mnemonic))
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s mnemonic must be listed in mnemonics list.\n";
                                            OutputBox.ScrollToCaret();
                                            instErrorCount++;
                                        }
                                        if (i.FileName == null)
                                        {
                                            OutputBox.Text += DateTime.Now.ToString() + " ERROR: " + instruction.Name + "'s file must be specified.\n";
                                            OutputBox.ScrollToCaret();
                                            instErrorCount++;
                                        }
                                        constants.AddInstruction(i);
                                        if (instErrorCount == 0)
                                        {
                                            string contents =
@"/* 
 *This is auto-generated text. 
 *Please, edit only method bodies. 
 */

public static void execute_" + i.Mnemonic.ToLower() + @"(InstructionRegister ir, Memory memory, ArchConstants constants, Variables variables, int[] operands, ref int[] result)
{	
	// TODO Write how this instruction executes here.
	// If this method is not necessary just leave it like this.
}";
                                            if (!File.Exists(i.FileName))
                                            {
                                                var file = File.Create(i.FileName);
                                                file.Close();
                                                File.WriteAllText(i.FileName, contents);
                                            }
                                        }
                                    }
                                }
                                errorCount += instErrorCount;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (constants.Name == null)
                {
                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Architecture must have a name.\n";
                    OutputBox.ScrollToCaret();
                    errorCount++;
                }
                if (constants.Mnemonics == null || constants.Mnemonics.Count == 0)
                {
                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Mnemonics must be specified.\n";
                    OutputBox.ScrollToCaret();
                    errorCount++;
                }
                if (Program.Mem == null)
                {
                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Memory must be specified.\n";
                    OutputBox.ScrollToCaret();
                    errorCount++;
                }
                if (registersSpecified == false)
                {
                    OutputBox.Text += DateTime.Now.ToString() + " ERROR: Registers must be specified.\n";
                    OutputBox.ScrollToCaret();
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                OutputBox.Text += DateTime.Now.ToString() + " Error in architecture file: " + ex.Message + "\n";
                OutputBox.ScrollToCaret();
                File.AppendAllText("error.txt", ex.ToString());
                projectOpenning = false;
                return;
            }
            if (errorCount == 0)
            {
                prepareGrammarFile();
                foreach (AddressingMode am in constants.AllAddressingModes)
                {
                    appendAMToGrammarFile(am);
                }
                foreach (Instruction inst in constants.InstructionSet)
                {
                    apendInstructionToGrammarFile(inst);
                }
                addAllInstructionsToGrammarFile();
                try
                {
                    string command = "/C java -jar Grammar//grammatica-1.5.jar " + constants.Name + ".grammar --csoutput grammar//cs --csnamespace MultiArc_Compiler --cspublic";
                    Process proc = new Process();
                    proc.StartInfo.FileName = "CMD.exe";
                    proc.StartInfo.Arguments = command;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();
                    string line = "";
                    int count = 30;
                    while (!proc.StandardError.EndOfStream)
                    {
                        string l = proc.StandardError.ReadLine();
                        line += l + "\n";
                        count--;
                        if (count == 0)
                            break;
                    }
                    if (!line.Equals(""))
                    {
                        projectOpenning = false;
                        memoryDumpToolStripMenuItem.Enabled = false;
                        registersToolStripMenuItem.Enabled = false;
                        assembleToolStripMenuItem.Enabled = false;
                        executeToolStripMenuItem.Enabled = false;
                        nextStepToolStripMenuItem.Enabled = false;
                        executeWithoutDebugToolStripMenuItem.Enabled = false;
                        DebugButton.Enabled = false;
                        recompileCodeToolStripMenuItem.Enabled = false;
                        OutputBox.Text += DateTime.Now.ToString() + " Error in grammar: " + line + "\n";
                        OutputBox.ScrollToCaret();
                    }
                    else
                    {
                        string grammar = File.ReadAllText(constants.Name + ".grammar");
                        File.Delete(constants.Name + ".grammar");
                        File.WriteAllText(dataFolder + constants.Name + ".grammar", grammar);
                        bool compileSuccessful = true;
                        foreach (AddressingMode am in constants.AllAddressingModes)
                        {
                            if (am.CompileCode(OutputBox) == false)
                            {
                                compileSuccessful = false;
                            }
                        }
                        foreach (Instruction i in constants.InstructionSet)
                        {
                            if (i.CompileCode(OutputBox) == false)
                            {
                                compileSuccessful = false;
                            }
                        }
                        if (compileSuccessful == true)
                        {
                            if (projectOpenning == false)
                            {
                                if (!File.Exists(dataFolder + fileName))
                                {
                                    var file = File.Create(dataFolder + fileName.Substring(fileName.LastIndexOf('\\')));
                                    file.Close();
                                }
                                File.WriteAllText(dataFolder + fileName.Substring(fileName.LastIndexOf('\\')), content);
                            }
                            arcFileName = dataFolder + fileName;
                            projectOpenning = false;
                            OutputBox.AppendText(DateTime.Now.ToString() + " Architecture loaded successfully. \n");
                            OutputBox.ScrollToCaret();
                            registersForm = new RegistersForm(constants);
                            memoryForm = new MemoryDumpForm();
                            memoryDumpToolStripMenuItem.Enabled = true;
                            registersToolStripMenuItem.Enabled = true;
                            assembleToolStripMenuItem.Enabled = true;
                            executeToolStripMenuItem.Enabled = true;
                            nextStepToolStripMenuItem.Enabled = true;
                            executeWithoutDebugToolStripMenuItem.Enabled = true;
                            DebugButton.Enabled = true;
                            recompileCodeToolStripMenuItem.Enabled = true;
                            compiled = false;
                        }
                        else
                        {
                            projectOpenning = false;
                            memoryDumpToolStripMenuItem.Enabled = false;
                            registersToolStripMenuItem.Enabled = false;
                            assembleToolStripMenuItem.Enabled = false;
                            executeToolStripMenuItem.Enabled = false;
                            nextStepToolStripMenuItem.Enabled = false;
                            executeWithoutDebugToolStripMenuItem.Enabled = false;
                            DebugButton.Enabled = false;
                            recompileCodeToolStripMenuItem.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    projectOpenning = false;
                    memoryDumpToolStripMenuItem.Enabled = false;
                    registersToolStripMenuItem.Enabled = false;
                    assembleToolStripMenuItem.Enabled = false;
                    executeToolStripMenuItem.Enabled = false;
                    nextStepToolStripMenuItem.Enabled = false;
                    executeWithoutDebugToolStripMenuItem.Enabled = false;
                    DebugButton.Enabled = false;
                    recompileCodeToolStripMenuItem.Enabled = false;
                    File.AppendAllText("error.txt", ex.ToString());
                }
            }
            else
            {
                memoryDumpToolStripMenuItem.Enabled = false;
                registersToolStripMenuItem.Enabled = false;
                assembleToolStripMenuItem.Enabled = false;
                executeToolStripMenuItem.Enabled = false;
                nextStepToolStripMenuItem.Enabled = false;
                executeWithoutDebugToolStripMenuItem.Enabled = false;
                DebugButton.Enabled = false;
                recompileCodeToolStripMenuItem.Enabled = false;
                projectOpenning = false;
            }
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (compiled == false)
            {
                this.AssemblyButton_Click(sender, e);
            }
            if (compiled == true)
            {
                if (ex == null || ex.Executing == false)
                {
                    stopDebuggingToolStripMenuItem.Enabled = true;
                    ex = new Executor(constants, binary, separators, breakPoints, OutputBox, entryPoint);
                    assembleToolStripMenuItem.Enabled = false;
                    ex.Debug();
                    ex.WaitUntilBreakpointOrEnd();
                    if (ex.Executing == true)
                    {
                        markInstruction(ex.Next, Color.Yellow);
                    }
                    else
                    {
                        deselectAllLines();
                        assembleToolStripMenuItem.Enabled = true;
                        stopDebuggingToolStripMenuItem.Enabled = false;
                    }
                }
                else
                {
                    markInstruction(ex.Next, Color.Red);
                    ex.Continue();
                    ex.WaitUntilBreakpointOrEnd();
                    if (ex.Executing == true)
                    {
                        markInstruction(ex.Next, Color.Yellow);
                    }
                    else
                    {
                        deselectAllLines();
                        assembleToolStripMenuItem.Enabled = true;
                    }
                }    
            }
        }

        private void markInstruction(int number, Color color)
        {
            string[] lines = CodeBox.Lines;
            int start = 0;
            for (int i = 0; i <= number; i++)
            {
                string temp = "";
                if (lines[i].Contains(";") && (lines[i].StartsWith(" ") || lines[i].StartsWith("\t") || lines[i].StartsWith(";")))
                {
                    int count;
                    for (count = 0; lines[i][count] != ';'; count++);
                    for (int j = count; j < lines[i].Length; j++)
                    {
                        temp += lines[i][j];
                    }
                }
                bool originFound = false;
                string[] words = lines[i].Split(' ', '\t');
                for (int k = 0; k < words.Length; k++)
                {
                    if (words[k].ToLower().Equals("org"))
                    {
                        originFound = true;
                    }
                }
                if (String.IsNullOrWhiteSpace(lines[i]) || String.IsNullOrEmpty(lines[i]) || temp.StartsWith(";") || originFound)
                {
                    number++;
                }
                if (i != number)
                {
                    start += lines[i].Length + 1;
                }
            }
            int length = lines[number].Length;
            CodeBox.Select(start, length);
            bool compiledOld = compiled;
            bool fileChangedOld = fileChanged;
            CodeBox.SelectionBackColor = color;
            compiled = compiledOld;
            fileChanged = fileChangedOld;
        }

        private void memoryDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoryForm.Visible = true;
        }

        private void registersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (registersForm != null)
            {
                registersForm.Visible = true;
            }
        }

        private void prepareGrammarFile()
        {
            try
            {
                string fileName = constants.Name + ".grammar";
                File.Delete(fileName);
                string content = @"/* This is auto-generated text. Do not edit! */
%header%

GRAMMARTYPE = ""LL""
CASESENSITIVE = ""false""

%tokens%

";

                for (int i = 0; i < constants.Tokens.Count; i++)
                {
                    content += constants.Tokens.ElementAt(i).Key + " = " + '"' + constants.Tokens.ElementAt(i).Value + '"' + '\n'; 
                }
                for (int i = 0; i < constants.Mnemonics.Count; i++)
                {
                    string token = constants.GetMnemonic(i);
                    content += token.ToUpper() + " = " + '"' + token + '"' + '\n';
                }
                for (int i = 0; i < constants.NUM_OF_REGISTERS; i++)
                {
                    for (int j = 0; j < constants.GetRegister(i).Names.Count; j++)
                    {
                        string token = constants.GetRegister(i).Names.ElementAt(j);
                        content += token.ToUpper() + " = " + '"' + token + '"' + '\n';
                    }
                }
                content += @"
EQUALS = ""=""
LEFT_PAREN = ""(""
RIGHT_PAREN = "")""
HASH = ""#""
COLON = "":""
COMMA = "",""
ORG = ""org""

SIGN = <<[+-]>>
DEC_NUMBER = <<[0-9]+>>
BIN_NUMBER = <<[01]+[bB]>>
OCT_NUMBER = <<[0-8]+[oO]>>
HEX_NUMBER = <<[0-9a-f]+[hH]>>
IDENTIFIER = <<[a-z][a-z0-9_]*>>

ENTER = <<[\n\r]+>>
SINGLE_LINE_COMMENT = <<;.*>> %ignore%
WHITESPACE = <<[ \t]+>> %ignore%

%productions%

Program = [Separator] [Origin] Lines ;

Separator = ENTER (ENTER)* ;

Origin = ORG DEC_NUMBER Separator ;

Lines = Line (Line)* ;

Line = [IDENTIFIER "":""] Instruction Separator ;

";
                File.AppendAllText(fileName, content);
            }
            catch (Exception ex) 
            {
                File.WriteAllText("error.txt", ex.ToString());
            }
        }

        private void appendAMToGrammarFile(AddressingMode am)
        {
            try
            {
                string toAppend = am.Name.ToUpper() + " = ";
                for (int i = 0; i < am.Expressions.Count; i++)
                {
                    toAppend += am.Expressions.ElementAt(i) + (i == am.Expressions.Count - 1 ? " ;\n\n" : " | ");
                }
                File.AppendAllText(constants.Name + ".grammar", toAppend);
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", ex.ToString());
            }
        }

        private void apendInstructionToGrammarFile(Instruction i)
        {
            try
            {
                File.AppendAllText(constants.Name + ".grammar", i.GetExpression() + "\n\n");
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", ex.ToString());
            }
        }

        private void addAllInstructionsToGrammarFile()
        {
            try
            {
                string toAppend = "Instruction = ( ";
                for (int i = 0; i < constants.InstructionSet.Count; i++)
                {
                    toAppend += constants.InstructionSet.ElementAt(i).Name;
                    if (i == constants.InstructionSet.Count - 1)
                    {
                        toAppend += " ) ;";
                    }
                    else
                    {
                        toAppend += " ) | ( ";
                    }
                }
                File.AppendAllText(constants.Name + ".grammar", toAppend);
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.txt", ex.ToString());
            }
        }

        public void AddToOutput(string text)
        {
            OutputBox.Text += DateTime.Now.ToString() + " " + text + "\n";
            OutputBox.ScrollToCaret();
        }

        private void clearOutputButton_Click(object sender, EventArgs e)
        {
            OutputBox.Text = "";
        }

        Executor ex;

        private void nextStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compiled == false)
            {
                AssemblyButton_Click(sender, e);
            }
            if (compiled == true)
            {
                if (ex == null || ex.Executing == false)
                {
                    ex = new Executor(constants, binary, separators, breakPoints, OutputBox, entryPoint);
                    ex.EnterStepByStep();
                    deselectAllLines();
                    selectLine(ex.Next);
                    assembleToolStripMenuItem.Enabled = false;
                    stopDebuggingToolStripMenuItem.Enabled = true;
                }
                else
                {
                    ex.ExecuteNextStep();
                    ex.WaitForOneInstruction();
                    if (ex.Executing == true)
                    {
                        deselectAllLines();
                        selectLine(ex.Next);
                    }
                    else
                    {
                        deselectAllLines();
                        assembleToolStripMenuItem.Enabled = true;
                        stopDebuggingToolStripMenuItem.Enabled = false;
                    }
                }
            }
        }

        private int start, length;

        private void selectLine(int number)
        {
            string[] lines = CodeBox.Lines;
            start = 0;
            for (int i = 0; i <= number; i++)
            {
                string temp = "";
                if (lines[i].Contains(";") && (lines[i].StartsWith(" ") || lines[i].StartsWith("\t") || lines[i].StartsWith(";")))
                {
                    int count;
                    for (count = 0; lines[i][count] != ';'; count++) ;
                    for (int j = count; j < lines[i].Length; j++)
                    {
                        temp += lines[i][j];
                    }
                }
                bool originFound = false;
                string[] words = lines[i].Split(' ', '\t');
                for (int k = 0; k < words.Length; k++)
                {
                    if (words[k].ToLower().Equals("org"))
                    {
                        originFound = true;
                    }
                }
                if (String.IsNullOrWhiteSpace(lines[i]) || String.IsNullOrEmpty(lines[i]) || temp.StartsWith(";") || originFound)
                {
                    number++;
                }
                if (i != number)
                {
                    start += lines[i].Length + 1;
                }
            }
            length = lines[number].Length;
            CodeBox.Select(start, length);
            bool compiledOld = compiled;
            bool fileChangedOld = fileChanged;
            CodeBox.SelectionBackColor = Color.Yellow;
            compiled = compiledOld;
            fileChanged = fileChangedOld;
        }

        private void deselectAllLines()
        {
            bool compiledOld;
            bool fileChangedOld;
            string[] lines = CodeBox.Lines;
            int start = 0;
            int emptyCount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string temp = "";
                if (lines[i].Contains(";") && (lines[i].StartsWith(" ") || lines[i].StartsWith("\t") || lines[i].StartsWith(";")))
                {
                    int count;
                    for (count = 0; lines[i][count] != ';'; count++);
                    for (int j = count; j < lines[i].Length; j++)
                    {
                        temp += lines[i][j];
                    }
                }
                bool originFound = false;
                string[] words = lines[i].Split(' ', '\t');
                for (int k = 0; k < words.Length; k++)
                {
                    if (words[k].ToLower().Equals("org"))
                    {
                        originFound = true;
                    }
                }
                if (String.IsNullOrWhiteSpace(lines[i]) || String.IsNullOrEmpty(lines[i]) || temp.StartsWith(";") || originFound)
                {
                    emptyCount++;
                }
                else
                {
                    if (breakPoints.Contains(i - emptyCount))
                    {
                        CodeBox.Select(start, lines[i].Length);
                        compiledOld = compiled;
                        fileChangedOld = fileChanged;
                        CodeBox.SelectionBackColor = Color.Red;
                        compiled = compiledOld;
                        fileChanged = fileChangedOld;
                    }
                    else
                    {
                        CodeBox.Select(start, lines[i].Length);
                        compiledOld = compiled;
                        fileChangedOld = fileChanged;
                        CodeBox.SelectionBackColor = Color.White;
                        compiled = compiledOld;
                        fileChanged = fileChangedOld;
                    }
                }
                start += lines[i].Length + 1;
            }
            CodeBox.Select(start, length);
            compiledOld = compiled;
            fileChangedOld = fileChanged;
            CodeBox.SelectionBackColor = Color.White;
            compiled = compiledOld;
            fileChanged = fileChangedOld;
            CodeBox.DeselectAll();
        }

        [DllImport("user32.dll")]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);


        private void lineNumbers_For_RichTextBox1_Click(object sender, EventArgs e)
        {
            int minScroll;
            int maxScroll;
            GetScrollRange(CodeBox.Handle, 1, out minScroll, out maxScroll);
            Point rtfPoint = Point.Empty;
            SendMessage(CodeBox.Handle, 0x400 + 221, 0, ref rtfPoint);
            Point screenPos = new Point(MousePosition.X, MousePosition.Y);
            Point myPos = lineNumbers_For_RichTextBox1.PointToClient(screenPos);
            double height = (double)CodeBox.Height / 19.0;
            int line = (int)(Math.Truncate((myPos.Y + rtfPoint.Y) / height));
            if (line < CodeBox.Lines.Length - 1)
            {
                toggleBreakpoint(line);
            }
        }

        private void toggleBreakpoint(int line)
        {
            string[] lines = CodeBox.Lines; 
            string temp = "";
            if (lines[line].Contains(";") && (lines[line].StartsWith(" ") || lines[line].StartsWith("\t") || lines[line].StartsWith(";")))
            {
                int count;
                for (count = 0; lines[line][count] != ';'; count++) ;
                for (int j = count; j < lines[line].Length; j++)
                {
                    temp += lines[line][j];
                }
            }
            bool originFound = false;
            string[] words = lines[line].Split(' ', '\t');
            for (int k = 0; k < words.Length; k++)
            {
                if (words[k].ToLower().Equals("org"))
                {
                    originFound = true;
                }
            }
            if (!(String.IsNullOrWhiteSpace(lines[line]) || String.IsNullOrEmpty(lines[line]) || temp.StartsWith(";") || originFound))
            {
                int start = 0;
                for (int i = 0; i < line; i++)
                {
                    start += lines[i].Length + 1;
                }
                int length = lines[line].Length;
                CodeBox.Select(start, length);
                for (int i = 0; i < line; i++)
                {
                    string t = "";
                    if (lines[i].Contains(";") && (lines[i].StartsWith(" ") || lines[i].StartsWith("\t") || lines[i].StartsWith(";")))
                    {
                        int count;
                        for (count = 0; lines[i][count] != ';'; count++) ;
                        for (int j = count; j < lines[i].Length; j++)
                        {
                            t += lines[i][j];
                        }
                    } 
                    bool orgFound = false;
                    string[] w = lines[i].Split(' ', '\t');
                    for (int k = 0; k < w.Length; k++)
                    {
                        if (w[k].ToLower().Equals("org"))
                        {
                            orgFound = true;
                        }
                    }
                    if (String.IsNullOrWhiteSpace(lines[i]) || String.IsNullOrEmpty(lines[i]) || lines[i].StartsWith(";") || t.StartsWith(";") || orgFound)
                    {
                        line--;
                    }
                }
                if (breakPoints.Contains(line))
                {
                    bool compiledOld = compiled;
                    bool fileChangedOld = fileChanged;
                    breakPoints.Remove(line);
                    CodeBox.SelectionBackColor = Color.White;
                    CodeBox.DeselectAll();
                    compiled = compiledOld;
                    fileChanged = fileChangedOld;
                }
                else
                {
                    bool compiledOld = compiled;
                    bool fileChangedOld = fileChanged;
                    breakPoints.AddLast(line);
                    CodeBox.SelectionBackColor = Color.Red;
                    CodeBox.DeselectAll();
                    compiled = compiledOld;
                    fileChanged = fileChangedOld;
                }
            }
        }

        private void executeWithoutDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compiled == false)
            {
                AssemblyButton_Click(sender, e);
            }
            if (compiled == true)
            {
                if (ex == null || ex.Executing == false)
                {
                    stopDebuggingToolStripMenuItem.Enabled = true;
                    ex = new Executor(constants, binary, separators, breakPoints, OutputBox, entryPoint);
                    ex.Execute();
                    stopDebuggingToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectDialog.InitialDirectory = Application.StartupPath + "\\Projects";
            NewProjectDialog.ShowDialog();
        }

        private string projectPath = null;

        private string projectName = null;

        private string dataFolder = null;

        private void NewProjectDialog_FileOk(object sender, CancelEventArgs e)
        {
            int lastIndex = NewProjectDialog.FileName.LastIndexOf('\\');
            projectPath = NewProjectDialog.FileName.Substring(0, lastIndex + 1);
            projectName = NewProjectDialog.FileName.Substring(lastIndex + 1);
            dataFolder = projectPath + "Data\\";
            string content = @"Time: " + DateTime.Now.ToString() + @"
Name: " + projectName + @"
";
            File.WriteAllText(NewProjectDialog.FileName, content);
            if (Directory.Exists(dataFolder))
            {
                Directory.Delete(dataFolder, true);
            }
            Directory.CreateDirectory(dataFolder);
            LoadArchitectureDialog.InitialDirectory = dataFolder;
            LoadFileDialog.InitialDirectory = dataFolder;
            loadToolStripMenuItem.Enabled = true;
            LoadArcButton.Enabled = true;
        }

        private void CodeBox_TextChanged(object sender, EventArgs e)
        {
            fileChanged = true;
            compiled = false;
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fileChanged == true)
            {
                new SaveOldFileForm(openedFileName, CodeBox);
            }
            else
            {
                ClearCode();
            }
        }

        public void ClearCode()
        {
            CodeBox.Clear();
            breakPoints.Clear();
            compiled = false;
            fileChanged = false;
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenProjectDialog.InitialDirectory = Application.StartupPath + "\\Projects";
            OpenProjectDialog.ShowDialog();
        }

        private bool projectOpenning = false;

        private void OpenProjectDialog_FileOk(object sender, CancelEventArgs e)
        {
            int lastIndex = OpenProjectDialog.FileName.LastIndexOf('\\');
            projectPath = OpenProjectDialog.FileName.Substring(0, lastIndex + 1);
            projectName = OpenProjectDialog.FileName.Substring(lastIndex + 1);
            dataFolder = projectPath + "Data\\";
            LoadArchitectureDialog.InitialDirectory = dataFolder;
            LoadFileDialog.InitialDirectory = dataFolder;
            loadToolStripMenuItem.Enabled = true;
            LoadArcButton.Enabled = true;
            string[] names = Directory.GetFiles(dataFolder);
            foreach (string n in names)
            {
                if (n.ToLower().EndsWith(".arc"))
                {
                    arcFileName = n;
                    projectOpenning = true;
                    LoadArchitectureDialog_FileOk(sender, e);
                }
            }
        }

        private void recompileCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string contents = File.ReadAllText(dataFolder + constants.Name + ".grammar");
                File.WriteAllText(constants.Name + ".grammar", contents);
                string command = "/C java -jar Grammar//grammatica-1.5.jar " + constants.Name + ".grammar --csoutput grammar//cs --csnamespace MultiArc_Compiler --cspublic";
                Process proc = new Process();
                proc.StartInfo.FileName = "CMD.exe";
                proc.StartInfo.Arguments = command;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                string line = "";
                while (!proc.StandardOutput.EndOfStream)
                    line += proc.StandardOutput.ReadLine() + "\n";
                File.WriteAllText("cmdout.txt", line);
                line = "";
                while (!proc.StandardError.EndOfStream)
                {
                    string l = proc.StandardError.ReadLine();
                    line += l + "\n";
                }
                if (!line.Equals(""))
                {
                    OutputBox.Text += DateTime.Now.ToString() + " Error in grammar: " + line + "\n";
                    OutputBox.ScrollToCaret();
                }
                else
                {
                    string grammar = File.ReadAllText(constants.Name + ".grammar");
                    File.Delete(constants.Name + ".grammar");
                    File.WriteAllText(dataFolder + constants.Name + ".grammar", grammar);
                    bool compileSuccessful = true;
                    foreach (AddressingMode am in constants.AllAddressingModes)
                    {
                        if (am.CompileCode(OutputBox) == false)
                        {
                            compileSuccessful = false;
                        }
                    }
                    foreach (Instruction i in constants.InstructionSet)
                    {
                        if (i.CompileCode(OutputBox) == false)
                        {
                            compileSuccessful = false;
                        }
                    }
                    if (compileSuccessful == true)
                    {
                        OutputBox.AppendText(DateTime.Now.ToString() + " Architecture loaded successfully. \n");
                        OutputBox.ScrollToCaret();
                        memoryDumpToolStripMenuItem.Enabled = true;
                        registersToolStripMenuItem.Enabled = true;
                        assembleToolStripMenuItem.Enabled = true;
                        executeToolStripMenuItem.Enabled = true;
                        nextStepToolStripMenuItem.Enabled = true;
                        executeWithoutDebugToolStripMenuItem.Enabled = true;
                        DebugButton.Enabled = true;
                        compiled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.txt", ex.ToString());
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CodeBox.Size = new Size((int)(Math.Round((this.Size.Width - 16 - 2 * 29) / 1.9974)), (int)(Math.Round((this.Size.Height - 78 - 36 - 25) / 1.4581)));
            BinaryCodeBox.Size = new Size((int)(Math.Round((this.Size.Width - 16 - 2 * 29) / 2.0026)), (int)(Math.Round((this.Size.Height - 78 - 36 - 25) / 1.4581)));
            BinaryCodeBox.Location = new Point(CodeBox.Location.X + CodeBox.Size.Width + 16, CodeBox.Location.Y);
            BinaryCodeLabel.Location = new Point(BinaryCodeBox.Location.X - 3, BinaryCodeLabel.Location.Y);
            OutputBox.Size = new Size((int)(this.Size.Width - 29 - 9), (int)(Math.Round((this.Size.Height - 78 - 36 - 25) / 3.8305)));
            OutputBox.Location = new Point(9, CodeBox.Location.Y + CodeBox.Size.Height + 36);
            outputLabel.Location = new Point(9, OutputBox.Location.Y - 16);
            clearOutputButton.Location = new Point(9 + OutputBox.Size.Width - 45, OutputBox.Location.Y - 22);
        }

        private void stopDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ex.Abort();
            deselectAllLines();
            stopDebuggingToolStripMenuItem.Enabled = false;
            OutputBox.AppendText(DateTime.Now + " Code stopped. \n");
        }
    }
}
