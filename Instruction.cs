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

    /// <summary>
    /// Class that represents one type of instruction.
    /// </summary>
    public class Instruction
    {

        /// <summary>
        /// String that contains code to be executed when this instruction is executed.
        /// </summary>
        private string executionCode;

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
                if (fileName != null)
                {
                    executionCode = File.ReadAllText(fileName);
                }
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

        /// <summary>
        /// Execution of instruction. 
        /// </summary>
        public int[] Execute(InstructionRegister ir, ArchConstants constants, int[] operands)
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
            code += executionCode;
            code += "}";
            var results = provider.CompileAssemblyFromSource(options, new[] { code });
            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    File.AppendAllText("error.txt", this.name + ": " + error + "\n");
                }
                return null;
            }
            else
            {
                int[] result = null;
                var t = results.CompiledAssembly.GetType("DynamicClassEX");
                object[] parameters = new object[] { ir, Program.Mem, constants, operands, result };
                t.GetMethod("execute_" + this.name).Invoke(null, parameters);
                result = (int[])(parameters[4]);
                return result;
            }
        }

        /// <summary>
        /// Reads addressing modes from binary code.
        /// </summary>
        /// <param name="binary">
        /// Binary code.
        /// </param>
        public void ReadAddressingModes(byte[] binary)
        {
            foreach (Argument arg in arguments)
            {
                if (arg.AddressingModes.Count > 1)
                {
                    foreach (AddressingMode addrMode in arg.AddressingModes)
                    {
                        int codeStarts = arg.CodeStarts[addrMode.Name];
                        int codeEnds = arg.CodeEnds[addrMode.Name];
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
                            int semiValue = (binary[binary.Length - 1 - codeEnds / 8 - byteCount] & (1 << (codeCount + codeEnds % 8)));
                            codeValue |= (byte)semiValue;
                            //codeValue |= (byte)((semiValue & (1 << (codeEnds % 8 + codeCount))) >> byteCount * 8); // This might be a problem.
                            if ((codeEnds + codeCount) % 8 == 0)
                                byteCount--;
                            codeCount--;
                        }
                        codeValue >>= codeEnds % 8;
                        if (arg.CodeValues[addrMode.Name] == codeValue)
                        {
                            arg.SelectedAddressingMode = addrMode;
                            break;
                        }
                    }
                }
                else
                {
                    arg.SelectedAddressingMode = arg.AddressingModes.ElementAt(0);
                }

            }
        }

        /// <summary>
        /// Fetches operands for all arguments that are declared as src.
        /// </summary>
        /// <param name="ir">
        /// Binary code of the instruction represented with InstructionRegister object.
        /// </param>
        /// <param name="constants">
        /// Architecture constants of the curent architecture.
        /// </param>
        /// <returns>
        /// Array containing fetched operands.
        /// </returns>
        public int[] FetchOperands(InstructionRegister ir, ArchConstants constants)
        {
            LinkedList<int> operands = new LinkedList<int>();
            foreach (Argument arg in arguments)
            {
                if (arg.Type.ToLower().Equals("src"))
                {
                    int result = arg.SelectedAddressingMode.GetData(ir, constants, arg.OperandStarts[arg.SelectedAddressingMode.Name], arg.OperandEnds[arg.SelectedAddressingMode.Name]);
                    operands.AddLast(result);
                }
            }
            return operands.ToArray();
        }

        /// <summary>
        /// Stores result of the execution.
        /// </summary>
        /// <param name="ir">
        /// Binary code of the instruction represented with InstructionRegister object.
        /// </param>
        /// <param name="constants">
        /// Architecture constants.
        /// </param>
        /// <param name="dataToStore">
        /// Result of the instruction execution.
        /// </param>
        public void storeResult(InstructionRegister ir, ArchConstants constants, int[] dataToStore)
        {
            int argCount = 0;
            foreach (Argument arg in arguments)
            {
                if (arg.Type.ToLower().Equals("dst"))
                {
                    arg.SelectedAddressingMode.SetData(ir, constants, arg.OperandStarts[arg.SelectedAddressingMode.Name], arg.OperandEnds[arg.SelectedAddressingMode.Name], dataToStore[argCount++]);
                }
            }
        }
    }
}
