/*
 * File: Instruction.cs
 * Author: Bojan Jelaca
 * Date: March 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using System.IO;

namespace MultiArc_Compiler
{

    public struct Parameters
    {
        public Int16 acc;
        public Int16 pc;
        public Int16 psw;
    }

    /// <summary>
    /// Class that represents one type of instruction.
    /// </summary>
    public class Instruction
    {

        /// <summary>
        /// String that contains code to be executed when this instruction is executed.
        /// </summary>
        private string[] executionCode;

        /// <summary>
        /// Name of the instruction.
        /// </summary>
        private string name;

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
        /// Instruction size in bytes.
        /// </summary>
        private int size;

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        /// <summary>
        /// Mnemonic of the instruction.
        /// </summary>
        private string mnemonic;

        public string Mnemonic
        {
            get
            {
                return mnemonic;
            }
            set
            {
                mnemonic = value;
            }
        }

        /// <summary>
        /// Operation code for instruction.
        /// </summary>
        private byte opCode;

        public byte OpCode
        {
            get
            {
                return opCode;
            }
            set
            {
                opCode = value;
            }
        }

        /// <summary>
        /// Name of the file with the execution code.
        /// </summary>
        private string fileName;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// Start bit of the opcode.
        /// </summary>
        private int startBit;

        public int StartBit
        {
            get
            {
                return startBit;
            }
            set
            {
                startBit = value;
            }
        }

        /// <summary>
        /// Bit mask for the instruction.
        /// </summary>
        private byte[] mask;

        public byte[] Mask
        {
            get
            {
                return mask;
            }
            set
            {
                mask = value;
            }
        }

        /// <summary>
        /// End bit of the opcode.
        /// </summary>
        private int endBit;

        public int EndBit
        {
            get
            {
                return endBit;
            }
            set
            {
                endBit = value;
            }
        }

        /// <summary>
        ///  List of the arguments.
        /// </summary>
        private LinkedList<Argument> arguments;

        public LinkedList<Argument> Arguments
        {
            get
            {
                return arguments;
            }
        }

        /// <summary>
        /// Adds one argument to the list of arguments.
        /// </summary>
        /// <param name="a">
        /// Argument to be added.
        /// </param>
        public void AddArgument(Argument a)
        {
            arguments.AddLast(a);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Instruction()
        {
            arguments = new LinkedList<Argument>();
        }

        /// <summary>
        /// Returns expression describing this instruction.
        /// </summary>
        /// <returns></returns>
        public string GetExpression()
        {
            string retVal = name + " = " + mnemonic + " ";
            for (int i = 0; i < arguments.Count; i++)
            {
                retVal += arguments.ElementAt(i).GetExpression();
                if (i == arguments.Count - 1)
                {
                    retVal += " ;";
                }
                else
                {
                    retVal += " COMMA ";
                }
            }
            return retVal;
        }

        // TODO Add summary
        public void Execute(byte addrMode, short operand, ref Int16 acc, ref Int16 pc, ref Int16 psw)
        {
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingNotDynamicClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingNotDynamicClass);
            string code = @"

using System;
using System.IO;
using MultiArc_Compiler;

public class DynamicClassEX
{
";
            for (int i = 0; i < executionCode.Length; i++)
            {
                code += executionCode[i] + "\n";
            }
            code += "}";
            var results = provider.CompileAssemblyFromSource(options, new[] { code });
            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    File.AppendAllText("error.txt", this.name + ": " + error + "\n");
                }
            }
            else
            {
                var t = results.CompiledAssembly.GetType("DynamicClassEX");
               // AddressingMode am = this.GetAddressingMode(addrMode);
                Int16 data = 0;
                UInt16 address = 0;
               // am.GetData(operand, ref address, ref data);
                object[] parameters = new object[] {data, address, Program.Mem, acc, pc, psw};
                t.GetMethod("execute" + this.name).Invoke(null, parameters);
                acc = (Int16)parameters[3];
                pc = (Int16)parameters[4];
                psw = (Int16)parameters[5];
            }
        }

    }
}
