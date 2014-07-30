/*
 * File: Executor.cs
 * Author: Bojan Jelaca
 * Date: April 2014
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiArc_Compiler
{
    /// <summary>
    /// Class that provides execution of binary code.
    /// </summary>
    class Executor
    {
        
        /// <summary>
        /// Architecture constants.
        /// </summary>
        private ArchConstants constants;

        /// <summary>
        /// Binary code to be executed.
        /// </summary>
        private byte[] binaryCode;

        /// <summary>
        /// Array that contains points in code that separates instructions.
        /// </summary>
        private LinkedList<int> separators;

        /// <summary>
        /// Entry point of the program.
        /// </summary>
        private int entryPoint;

        /// <summary>
        /// Text box representing output.
        /// </summary>
        private TextBoxBase output;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="constants">
        /// Architecture constants.
        /// <param name="binaryCode">
        /// Binary code to be executed.
        /// </param>
        /// <param name="separators">
        /// Array that has informations about points where instruction separators are.
        /// </param>
        /// <param name="entryPoint">
        /// Entry point of the program.
        /// </param>
        /// <param name="psw">
        /// Program status word before exection.
        /// </param>
        /// <param name="output">
        /// Text box representing output.
        /// </param>
        public Executor(ArchConstants constants, byte[] binaryCode, LinkedList<int> separators, TextBoxBase output,  Int16 entryPoint = 0) 
        {
            this.constants = constants;
            this.binaryCode = binaryCode;
            this.entryPoint = entryPoint;
            this.separators = separators;
            this.output = output;
        }

        /// <summary>
        /// Method that executes binary code.
        /// </summary>
        public void Execute()
        {
            constants.GetRegister("pc").Val = entryPoint;
            LinkedList<Instruction> instructions = new LinkedList<Instruction>();
            LinkedList<byte> binary = new LinkedList<byte>();
            while (true) 
            {
                int pc = constants.GetRegister("pc").Val;
                if (separators.Contains(pc) && pc != entryPoint)
                {
                    Instruction inst = constants.MatchInstruction(binary.ToArray());
                    InstructionRegister ir = new InstructionRegister(binary.ToArray());
                    inst.ReadAddressingModes(binary.ToArray());
                    int[] operands = inst.FetchOperands(ir, constants);
                    int[] result = inst.Execute(ir, constants, operands);
                    inst.StoreResult(ir, constants, result);
                    binary.Clear();
                    pc = constants.GetRegister("pc").Val;
                } 
                if (pc >= binaryCode.Length)
                {
                    break;
                }
                binary.AddLast(binaryCode[pc++]);
                constants.GetRegister("pc").Val = pc;
            }
            output.Text += DateTime.Now.ToString() + " Code executed successfully\n";
        }

    }
}
