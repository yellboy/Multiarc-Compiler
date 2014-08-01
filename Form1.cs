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

        public Form1()
        {
            InitializeComponent();
            Instance = this;
        }

        private void AssemblyButton_Click(object sender, EventArgs e)
        {
            Assembler asm = new Assembler(CodeBox.Text, constants, Program.Mem, OutputBox);
            binary = asm.Assemble();
            if (binary != null)
            {
                BinaryCodeBox.Text = "";
                separators = asm.Separators;
                ByteCountBox.Text = "00:\n";
                for (int i = 0; i < asm.Count; i++)
                {
                    if (separators.Contains(i) && i != 0)
                    {
                        ByteCountBox.Text += i.ToString().PadLeft(2, '0') + ":\n";
                        BinaryCodeBox.Text += "\n";
                    }
                    BinaryCodeBox.Text += BitConverter.ToString(binary, i, 1); // Convert byte to hex string and write it to binary code box.
                }
            }
        }

        private void LoadFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            openedFileName = LoadFileDialog.FileName;
            string[] code;
            code = File.ReadAllLines(LoadFileDialog.FileName);
            LoadFilePathText.Text = LoadFileDialog.FileName;
            FileNameLabel.Text = "File: " + LoadFileDialog.FileName;
            CodeBox.Text = "";
            for (int i = 0; i < code.Length; i++)
            {
                CodeBox.Text += code[i] + '\n';
            }
        }

        private void LoadFileBrowseButton_Click(object sender, EventArgs e)
        {
            LoadFileDialog.ShowDialog();
        }

        private void LoadFromFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] code;
                code = File.ReadAllLines(LoadFilePathText.Text);
                openedFileName = LoadFilePathText.Text;
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
            ByteCountBox.Text = "00:\n";
            BinaryCodeBox.Text = "";
            int byteCount = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == (byte)('\n'))
                {
                    BinaryCodeBox.Text += "\n";
                    ByteCountBox.Text += byteCount.ToString().PadLeft(2, '0') + ":\n";
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
                binary = File.ReadAllBytes(BinFilePathText.Text);
                openedBinFileName = BinFilePathText.Text;
                BinFileNameLabel.Text = openedBinFileName;
                ByteCountBox.Text = "00:\n";
                BinaryCodeBox.Text = "";
                int byteCount = 0;
                for (int i = 0; i < binary.Length; i++)
                {
                    if (binary[i] == (byte)('\n'))
                    {
                        BinaryCodeBox.Text += "\n";
                        ByteCountBox.Text += byteCount.ToString().PadLeft(2, '0') + ":\n";
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

        private void createNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            new LengthsForm();
            SaveArchitecture();
        }

        // THIS METHOD MUST BE REWRITEN
        public void WriteArchitectureToFile(string fileName)
        {
            //Program.Mem = new Memory(constants.MEM_SIZE, constants.ADDR_UNIT_SIZE);
            StringBuilder content = new StringBuilder();
            content.Append(constants.WORD_LENGTH);
            content.Append("\n");
            content.Append(constants.PC_LENGTH);
            content.Append("\n");
            content.Append(constants.NUM_OF_REGISTERS);
            content.Append("\n");
            content.Append(constants.MEM_SIZE);
            content.Append("\n");
            content.Append(constants.ADDR_UNIT_SIZE);
            content.Append("\n");
            content.Append(constants.MAX_BYTES);
            content.Append("\n");
            content.Append("AddrModes:\n");
            for (int i = 0; i < constants.AllAddressingModes.Count; i++)
            {
                AddressingMode am = constants.AllAddressingModes.ElementAt(i);
                //content.Append(am.Name + " " + am.Size + " " + am.Code + "\n");
            }
            content.Append("InstructionSet:\n");
            for (int i = 0; i < constants.InstructionSet.Count; i++)
            {
                Instruction inst = constants.InstructionSet.ElementAt(i);
                content.Append(inst.Name + " " + inst.OpCode + "\n");
                content.Append("AM: ");
              //  for (int j = 0; j < inst.AddressingModes.Count; j++)
               // {
                //    content.Append(inst.AddressingModes.ElementAt(j).Name + " ");
               // }
                content.Append("\n");
            }
            content.Append("END");
            File.WriteAllText(fileName, content.ToString());
        }

        private void LoadArcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadArchitectureDialog.ShowDialog();
        }

        private void LoadArchitectureDialog_FileOk(object sender, CancelEventArgs e)
        {
            int errorCount = 0;
            try
            {
                string fileName = LoadArchitectureDialog.FileName;
                string content = File.ReadAllText(fileName);
                SaveArchitecture();
                constants.RemoveAllInstructions();
                constants.RemoveAllAddressingModes();
                constants.RemoveAllDataTypes();
                constants.RemoveAllMnemonics();
                constants.RemoveAllRegisters();
                XmlReader xmlReader = XmlReader.Create(new StringReader(content));
                XmlDocument doc = new XmlDocument();
                XmlNode head = doc.ReadNode(xmlReader);
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
                            prepareGrammarFile();
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
                                        Program.Mem.InitFile = n.InnerText;
                                        break;
                                    case "storage_file":
                                        Program.Mem.StorageFile = n.InnerText;
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
                                                    default:
                                                        break;
                                                }
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
                                                    Register r = new Register(size);
                                                    r.Size = size;
                                                    r.Val = value;
                                                    r.Group = "general_purpose";
                                                    r.AddName(prefix + i);
                                                    constants.AddRegister(r);
                                                }
                                            }
                                        }
                                        break;
                                    case "#whitespace":
                                        break;
                                    default:
                                        {
                                            Register r = new Register(0);
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
                                                        Register partReg = new Register(0);
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
                                                    am.FileName = child.InnerText;
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
                                                        string group = "";
                                                        XmlNodeList expressions = child.ChildNodes;
                                                        foreach (XmlNode expression in expressions)
                                                        {
                                                            switch (expression.Name)
                                                            {
                                                                case "registers_group":
                                                                    am.OperandReadFromExpression = false;
                                                                    am.OperandInValues = true;
                                                                    group = expression.InnerText;
                                                                    exp += group;
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
                                                        if (!group.Equals(""))
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
                                                                        int ind = exp.IndexOf(group);
                                                                        expToAdd = exp.Replace(group, r.Names.ElementAt(j));
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
                                            appendAMToGrammarFile(am);
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
                                                    i.FileName = child.InnerText;
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
                                        apendInstructionToGrammarFile(i);
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
                if (errorCount == 0)
                {
                    OutputBox.Text += DateTime.Now.ToString() + " Architecture loaded successfully.\n";
                    OutputBox.ScrollToCaret();
                }
                else
                {
                    OutputBox.Text += DateTime.Now.ToString() + " Architecture could not be loadded. " + errorCount + " error(s) existed.\n";
                    OutputBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                OutputBox.Text += DateTime.Now.ToString() + " Error in architecture file: " + ex.Message + "\n";
                OutputBox.ScrollToCaret();
                File.AppendAllText("error.txt", ex.ToString());
                return;
            }
            if (errorCount == 0)
            {
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
                    File.WriteAllText("cmderr.txt", line);
                }
                catch (Exception ex)
                {
                    File.AppendAllText("error.txt", ex.ToString());
                }
            }
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Executor ex = new Executor(constants, binary, separators, OutputBox);
            ex.Execute();
        }

        private void memoryDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MemoryDumpForm();
        }

        private void registersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RegistersForm(constants);
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

SIGN = <<[+-]>>
DEC_NUMBER = <<[0-9]+>>
BIN_NUMBER = <<[01]+[bB]>>
OCT_NUMBER = <<[0-8]+[oO]>>
HEX_NUMBER = <<[0-9a-f]+[hH]>>
IDENTIFIER = <<[a-z][a-z0-9_]*>>

ENTER = <<[\n\r]+>>
SINGLE_LINE_COMMENT = <<;.*>> %ignore%
WHITESPACE = <<[ \t]+>> %ignore%
ORG = ""org""

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
                string toAppend = "Instruction = ";
                for (int i = 0; i < constants.InstructionSet.Count; i++)
                {
                    toAppend += constants.InstructionSet.ElementAt(i).Name;
                    if (i == constants.InstructionSet.Count - 1)
                    {
                        toAppend += " ;";
                    }
                    else
                    {
                        toAppend += " | ";
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

    }
}
