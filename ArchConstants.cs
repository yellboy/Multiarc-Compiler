/*
 * File: ArchConstants.cs
 * Author: Bojan Jelaca
 * Date: October 2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiArc_Compiler
{

    /// <summary>
    /// Contains constants for generating binary code.
    /// </summary>
    public class ArchConstants: ICloneable
    {

        /// <summary>
        /// Name of the architecture.
        /// </summary>
        private string name = null;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// List of strings that represents all the mnemonics for the instructions in architecture.
        /// </summary>
        private LinkedList<string> mnemonics;

        public LinkedList<string> Mnemonics
        {
            get
            {
                return mnemonics;
            }
        }

        /// <summary>
        /// Adds new mnemonic to mnemonics list.
        /// </summary>
        /// <param name="name">
        /// Name of the mnemonic to add.
        /// </param>
        /// <returns>
        /// True if name is not already in the list and false otherwise.
        /// </returns>
        public bool AddMnemonic(string name)
        {
            foreach (string mnemonic in mnemonics)
            {
                if (mnemonic.Equals(name))
                {
                    return false;
                }
            }
            mnemonics.AddLast(name);
            return true;
        }

        /// <summary>
        /// Removes all mnemonics from the mnemonics list.
        /// </summary>
        public void RemoveAllMnemonics()
        {
            mnemonics.Clear();
        }

        /// <summary>
        /// Gets one mnemonic from the mnemonics list.
        /// </summary>
        /// <param name="index">
        /// Index of the wanted mnemonic.
        /// </param>
        /// <returns>
        /// Wanted mnemonic if index is OK and null otherwise.
        /// </returns>
        public string GetMnemonic(int index)
        {
            if (mnemonics.Count > index && index >= 0)
                return mnemonics.ElementAt(index);
            return null;
        }

        /// <summary>
        /// Length of PC in bytes.
        /// </summary>
        private int pcLength = 2;

        public int PC_LENGTH
        {
            get
            {
                return pcLength;
            }
            set
            {
                pcLength = value;
            }
        }

        /// <summary>
        /// Length of word in bytes.
        /// </summary>
        private int wordLength = 2;

        public int WORD_LENGTH
        {
            get
            {
                return wordLength;
            }
            set
            {
                wordLength = value;
            }
        }

        /// <summary>
        /// Number of cpu registers.
        /// </summary>
        private int numOfRegisters = 0;

        public int NUM_OF_REGISTERS
        {
            get
            {
                return numOfRegisters;
            }
            set
            {
                numOfRegisters = value;
            }
        }

        /// <summary>
        /// Number of bytes in memory.
        /// </summary>
        private int memorySize = 4096;

        public int MEM_SIZE
        {
            get
            {
                return memorySize;
            }
            set
            {
                memorySize = value;
            }
        }

        /// <summary>
        /// Starting address of ROM memory.
        /// </summary>
        private int romStart = 0;

        public int ROM_START
        {
            get
            {
                return romStart;
            }
            set
            {
                romStart = value;
            }
        }

        /// <summary>
        /// Ending address of ROM memory. 
        /// </summary>
        private int romEnd = 2047;

        public int ROM_END
        {
            get
            {
                return romEnd;
            }
            set
            {
                romEnd = value;
            }
        }

        /// <summary>
        /// Starting address of RAM memory.
        /// </summary>
        private int ramStart = 2048;

        public int RAM_START
        {
            get
            {
                return ramStart;
            }
            set
            {
                ramStart = value;
            }
        }

        private int ramEnd = 4096;

        public int RAM_END
        {
            get
            {
                return ramEnd;
            }
            set
            {
                ramEnd = value;
            }
        }

        /// <summary>
        /// Size of addresible unit in bytes.
        /// </summary>
        private int addressibleUnitSize = 2;

        public int ADDR_UNIT_SIZE
        {
            get
            {
                return addressibleUnitSize;
            }
            set
            {
                addressibleUnitSize = value;
            }
        }

        /// <summary>
        /// Max length of instruction in bytes.
        /// </summary>
        private int maxBytes = 4;

        public int MAX_BYTES
        {
            get
            {
                return maxBytes;
            }
            set
            {
                maxBytes = value;
            }
        }

        /// <summary>
        /// Instruction set for given architecture.
        /// </summary>
        private LinkedList<Instruction> instructionSet;

        public LinkedList<Instruction> InstructionSet
        {
            get
            {
                return instructionSet;
            }
        }

        /// <summary>
        /// Adds new instruction to instruction set, if it is not already in the set.
        /// </summary>
        /// <param name="inst">
        /// Instruction to be added.
        /// </param>
        /// <returns>
        /// Returns true if new instruction is added and false if it already was in the set.
        /// </returns>
        public bool AddInstruction(Instruction inst)
        {
            foreach (Instruction i in instructionSet)
            {
                if (i.Name.ToLower().Equals(inst.Name.ToLower()))
                    return false;
            }
            instructionSet.AddLast(inst);
            return true;
        }

        /// <summary>
        /// Gets the instruction from instruction set.
        /// </summary>
        /// <param name="name">
        /// Name of instruction.
        /// </param>
        /// <returns>
        /// Wanted instruction if it is in instruction set or null otherwise.
        /// </returns>
        public Instruction GetInstruction (string name) 
        {
            foreach (Instruction i in instructionSet)
            {
                if (name.ToLower().Equals(i.Name.ToLower()))
                    return i;
            }
            return null;
        }

        /// <summary>
        /// Gets the instruction from instruction set.
        /// </summary>
        /// <param name="opCode">
        /// Operation code of instruction.
        /// </param>
        /// <returns></returns>
        public Instruction GetInstruction(byte opCode)
        {
            foreach (Instruction i in instructionSet)
            {
                if (i.OpCode == opCode)
                    return i;
            }
            return null;
        }
        
        /// <summary>
        /// Removes instruction from instruction set.
        /// </summary>
        /// <param name="name">
        /// Name of the instruction to be removed.
        /// </param>
        /// <returns>
        /// Removed instruction if it was in instruction set or null otherwise.
        /// </returns>
        public Instruction RemoveInstruction(string name)
        {
            foreach (Instruction i in instructionSet)
            {
                if (name.ToLower().Equals(i.Name.ToLower()))
                {
                    instructionSet.Remove(i);
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// List of all addressing supported addressing modes.
        /// </summary>
        private LinkedList<AddressingMode> allAddressingModes;

        public LinkedList<AddressingMode> AllAddressingModes
        {
            get
            {
                return allAddressingModes;
            }
            set // Can not be set.
            {
            }
        }

        /// <summary>
        /// Removes all addressing modes.
        /// </summary>
        public void RemoveAllAddressingModes()
        {
            allAddressingModes.Clear();
        }

        /// <summary>
        /// Adds new addressing mode to list of all addressing modes.
        /// </summary>
        /// <param name="am">
        /// Addressing mode to be added.
        /// </param>
        /// <returns>
        /// True if addressing mode is added or false if it is already in list.
        /// </returns>
        public bool AddAddressingMode(AddressingMode am)
        {
            if (!allAddressingModes.Contains(am))
            {
                allAddressingModes.AddLast(am);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the addressing mode by name.
        /// </summary>
        /// <param name="name">
        /// Name of wanted addressing mode.
        /// </param>
        /// <returns>
        /// Wanted addressing mode or null if it does not exist.
        /// </returns>
        public AddressingMode GetAddressingMode(string name)
        {
            if (name == null)
                return null;
            foreach (AddressingMode am in allAddressingModes)
            {
                if (am.Name.ToLower().Equals(name.ToLower()))
                    return am;
            }
            return null;
        }

        /// <summary>
        /// Removes all instructions from instruction set.
        /// </summary>
        public void RemoveAllInstructions()
        {
            int count = instructionSet.Count;
            for (int i = 0; i < count; i++)
                instructionSet.RemoveLast();
        }

        private LinkedList<Data> dataTypes;

        public LinkedList<Data> DataTypes
        {
            get
            {
                return dataTypes;
            }
            set
            {
                dataTypes = value;
            }
        }

        /// <summary>
        /// Adds new data type to data types list.
        /// </summary>
        /// <param name="name">
        /// Name of new data type.
        /// </param>
        /// <param name="size">
        /// Size of new data type in bits.
        /// </param>
        /// <returns>
        /// True if new data type is not already in the list, and false otherwise.
        /// </returns>
        public bool AddDataType(string name, int size)
        {
            foreach (Data d in dataTypes)
            {
                if (d.Name.ToLower().Equals(name.ToLower()))
                {
                    return false;
                }
            }
            dataTypes.AddLast(new Data(size, name));
            return true;
        }

        /// <summary>
        /// Gets data type from list of all data types.
        /// </summary>
        /// <param name="name">
        /// Name of the wanted data type.
        /// </param>
        /// <returns>
        /// Wanted data type if it exists in the list or null otherwise.
        /// </returns>
        public Data GetDataType(string name) 
        {
            foreach (Data d in dataTypes)
            {
                if (d.Name.ToLower().Equals(name.ToLower()))
                {
                    return d;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes all data types from data types list.
        /// </summary>
        public void RemoveAllDataTypes()
        {
            int count = DataTypes.Count;
            for (int i = 0; i < count; i++)
                dataTypes.RemoveLast();
        }

        /// <summary>
        /// Linked list of all available registers.
        /// </summary>
        private LinkedList<Register> registers;

        /// <summary>
        /// Gets register from list of all registers.
        /// </summary>
        /// <param name="name">
        /// Name of the wanted register.
        /// </param>
        /// <returns>
        /// Wanted register if it exists or null otherwise.
        /// </returns>
        public Register GetRegister(string name)
        {
            foreach (Register r in registers)
            {
                foreach (string n in r.Names)
                {
                    if (n.ToLower().Equals(name.ToLower()))
                        return r;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets register from list of all registers.
        /// </summary>
        /// <param name="i">
        /// Index of the wanted register.
        /// </param>
        /// <returns>
        /// Wanted register if it exists or null otherwise.
        /// </returns>
        public Register GetRegister(int i)
        {
            if (i >= 0 && i < registers.Count)
                return registers.ElementAt(i);
            else
                return null;
        }

        /// <summary>
        /// Adds new register into the list of registers.
        /// </summary>
        /// <param name="r">
        /// Register to be added.
        /// </param>
        public void AddRegister(Register r)
        {
            registers.AddLast(r);
            this.numOfRegisters++;
        }

        /// <summary>
        /// Removes all registers from list of registers.
        /// </summary>
        public void RemoveAllRegisters()
        {
            int count = registers.Count;
            for (int i = 0; i < count; i++)
            {
                registers.RemoveLast();
            }
            this.numOfRegisters = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArchConstants()
        {
            allAddressingModes = new LinkedList<AddressingMode>();
            instructionSet = new LinkedList<Instruction>();
            dataTypes = new LinkedList<Data>();
            registers = new LinkedList<Register>();
            mnemonics = new LinkedList<string>();
        }

        public object Clone()
        {
            ArchConstants clone = new ArchConstants();
            clone.instructionSet = new LinkedList<Instruction>(instructionSet);
            clone.allAddressingModes = new LinkedList<AddressingMode>(allAddressingModes);
            clone.dataTypes = new LinkedList<Data>(dataTypes);
            return clone;
        }

        /// <summary>
        /// Constants for all operations.
        /// </summary>
        private byte ld = 0x10;

        public byte LD
        {
            get
            {
                return ld;
            }
            set
            {
                ld = value;
            }
        }

        private byte st = 0x20;

        public byte ST
        {
            get
            {
                return st;
            }
            set
            {
                st = value;
            }
        }

        private byte jmp = 0x30;

        public byte JMP
        {
            get
            {
                return jmp;
            }
            set
            {
                jmp = value;
            }
        }

        private byte add = 0x40;

        public byte ADD
        {
            get
            {
                return add;
            }
            set
            {
                add = value;
            }
        }

        private byte sub = 0x50;

        public byte SUB
        {
            get
            {
                return sub;
            }
            set
            {
                sub = value;
            }
        }

        private byte mul = 0x60;

        public byte MUL
        {
            get
            {
                return mul;
            }
            set
            {
                mul = value;
            }
        }

        private byte div = 0x70;

        public byte DIV
        {
            get
            {
                return div;
            }
            set
            {
                div = value;
            }
        }

        private byte jgr = 0x80;

        public byte JGR
        {
            get
            {
                return jgr;
            }
            set
            {
                jgr = value;
            }
        }

        private byte jeq = 0x90;

        public byte JEQ
        {
            get
            {
                return jeq;
            }
            set
            {
                jeq = value;
            }
        }

        private byte jls = 0xa0;

        public byte JLS
        {
            get
            {
                return jls;
            }
            set
            {
                jls = value;
            }
        }

        private byte jge = 0xb0;

        public byte JGE
        {
            get
            {
                return jge;
            }
            set
            {
                jge = value;
            }
        }

        private byte jle = 0xc0;

        public byte JLE
        {
            get
            {
                return jle;
            }
            set
            {
                jle = value;
            }
        }

        private byte push = 0xd0;

        public byte PUSH
        {
            get
            {
                return push;
            }
            set
            {
                push = value;
            }
        }

        private byte pop = 0xe0;

        public byte POP
        {
            get
            {
                return pop;
            }
            set
            {
                pop = value;
            }
        }

        private byte halt = 0xf0;

        public byte HALT
        {
            get
            {
                return halt;
            }
            set
            {
                halt = value;
            }
        }

        /// <summary>
        /// Constants for address modes.
        /// </summary>
        private byte imm = 0x0f;

        public byte IMM
        {
            get
            {
                return imm;
            }
            set
            {
                imm = value;
            }
        }

        private byte regInd = 0x04;

        public byte REGIND
        {
            get
            {
                return regInd;
            }
            set
            {
                regInd = value;
            }
        }

        private byte memInd = 0x0a;

        public byte MEMIND
        {
            get
            {
                return memInd;
            }
            set
            {
                memInd = value;
            }
        }

        private byte memDir = 0x09;

        public byte MEMDIR
        {
            get
            {
                return memDir;
            }
            set
            {
                memDir = value;
            }
        }

        /// <summary>
        /// Matches one instruction from instruction set with its binary code.
        /// </summary>
        /// <param name="binary">
        /// Binary code of the instruction.
        /// </param>
        /// <returns>
        /// Matching instruction.
        /// </returns>
        public Instruction MatchInstruction(byte[] binary) // NOTE: This method must be tested more.
        {
            foreach (Instruction i in instructionSet)
            {
                if (i.Size != binary.Length)
                {
                    continue;
                }
                /*
                int size = 1;
                for (int j = i.StartBit; j >= i.EndBit; j--)
                {
                    if (j % 8 == 0 && j != i.EndBit)
                    {
                        size++;
                    }
                }
                int startBite = binary.Length - i.StartBit / 8 - 1;
                int endBite = binary.Length - i.EndBit / 8 - 1;
                int count = 0;
                bool matched = true;
                for (int j = startBite; j <= endBite; j++)
                {
                    if (!(((int)(i.Mask[count]) & (int)(binary[j])) == (int)(i.Mask[count])))
                    {
                        matched = false;
                    }
                    count++;
                }
                if (matched == true)
                {
                    return i;
                }*/
                int codeStarts = i.StartBit;
                int codeEnds = i.EndBit;
                int codeValue = 0;
                int codeSize = 1;
                for (int k = codeStarts; k >= codeEnds; k--)
                {
                    if (k % 8 == 0 && k != codeEnds)
                    {
                        codeSize++;
                    }
                }
                int codeCount = codeStarts - codeEnds;
                int byteCount = codeSize - 1;
                for (int k = codeStarts; k >= codeEnds; k--)
                {
                    int semiValue = (binary[binary.Length - 1 - codeEnds / 8 - byteCount] & (1 << ((codeCount + codeEnds) % 8)));
                    codeValue |= semiValue << byteCount * 8;
                    //codeValue |= (byte)((semiValue & (1 << (codeEnds % 8 + codeCount))) >> byteCount * 8); // This might be a problem.
                    if ((codeEnds + codeCount) % 8 == 0)
                        byteCount--;
                    codeCount--;
                }
                int maskValue = 0;
                if (i.Size != i.Mask.Length)
                {
                    codeStarts -= i.Mask.Length * 8;
                    codeEnds -= i.Mask.Length * 8;
                }
                byteCount = codeSize - 1;
                codeCount = codeStarts - codeEnds;
                for (int k = codeStarts; k >= codeEnds; k--)
                {
                    int semiValue = (i.Mask[i.Mask.Length - 1 - codeEnds / 8 - byteCount] & (1 << ((codeCount + codeEnds) % 8)));
                    maskValue |= semiValue << byteCount * 8;
                    //codeValue |= (byte)((semiValue & (1 << (codeEnds % 8 + codeCount))) >> byteCount * 8); // This might be a problem.
                    if ((codeEnds + codeCount) % 8 == 0)
                        byteCount--;
                    codeCount--;
                }
                if (maskValue == codeValue)
                {
                    return i;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Calculates size of instruction in bytes. THIS METHOD IS OBSCURE.
        /// </summary>
        /// <param name="opCode">
        /// Byte whose higher 4 bits represents operation code.
        /// </param>
        /// <param name="addrMode">
        /// Byte whose lower 4 bits represents addressing mode.
        /// </param>
        /// <returns>
        /// Size in bytes for given instruction.
        /// </returns>
        public ushort InstructionSize(byte opCode, byte addrMode)
        {
                        return (ushort)maxBytes;
        }
    }
}
