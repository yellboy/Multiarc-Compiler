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
        public Executor(ArchConstants constants, byte[] binaryCode, LinkedList<int> separators, Int16 entryPoint = 0) 
        {
            this.constants = constants;
            this.binaryCode = binaryCode;
            this.entryPoint = entryPoint;
            this.separators = separators;
        }

        /// <summary>
        /// Method that executes binary code.
        /// </summary>
        public void Execute()
        {
            constants.GetRegister("pc").Val = entryPoint;
            LinkedList<Instruction> instructions = new LinkedList<Instruction>();
            LinkedList<byte> binary = new LinkedList<byte>();
            for (int i = entryPoint; i < binaryCode.Length; i++)
            {
                if (separators.Contains(i) && i != entryPoint)
                {
                    Instruction inst = constants.MatchInstruction(binary.ToArray());
                    instructions.AddLast(inst);
                    binary.Clear();
                }
                binary.AddLast(binaryCode[i]);
            }

        }

    }
}
