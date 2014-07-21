﻿/*
 * File: Argument.cs
 * Author: Bojan Jelaca
 * Date: May 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiArc_Compiler
{
    /// <summary>
    /// Class representing one argument of the instruction.
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// Type of the argument.
        /// </summary>
        private string type;

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// List of all addressing modes possible for this argument.
        /// </summary>
        private LinkedList<AddressingMode> addressingModes;

        public LinkedList<AddressingMode> AddressingModes
        {
            get
            {
                return addressingModes;
            }
        }

        /// <summary>
        /// Dictionary contains pairs of addressing mode name and opcode for it.
        /// </summary>
        private Dictionary<string, int> codeValues;

        public Dictionary<string, int> CodeValues
        {
            get
            {
                return codeValues;
            }
        }

        /// <summary>
        /// Adds new addressing mode to the list of the addressing modes.
        /// </summary>
        /// <param name="am">
        /// Addressing mode to be added.
        /// </param>
        /// <param name="code">
        /// Code paired with addressing mode.
        /// </param>
        /// <param name="codeStarts">
        /// Start bit of the opcode in the instruction.
        /// </param>
        /// <param name="codeEnds">
        /// End bit of the opcode in the instruction.
        /// </param>
        /// <param name="operandStarts">
        /// Start bit of the operand in the instruction.
        /// </param>
        /// <param name="operandEnds">
        /// End bit of the operand in the instruction.
        /// </param>
        public void AddAddressingMode(AddressingMode am, int code, int codeStarts, int codeEnds, int operandStarts, int operandEnds)
        {
            addressingModes.AddLast(am);
            codeValues.Add(am.Name, code);
            this.codeStarts.Add(am.Name, codeStarts);
            this.codeEnds.Add(am.Name, codeEnds);
            this.operandStarts.Add(am.Name, operandStarts);
            this.operandEnds.Add(am.Name, operandEnds);
        }

        /// <summary>
        /// Dictionary containing pairs of addressing mode names and start bits of the opcode for that addressing mode.
        /// </summary>
        private Dictionary<string, int> codeStarts;

        public Dictionary<string, int> CodeStarts
        {
            get
            {
                return codeStarts;
            }
        }

        /// <summary>
        /// Dictionary containing pairs of addressing mode names and end bits of the opcode for that addressing mode.
        /// </summary>
        private Dictionary<string, int> codeEnds;

        public Dictionary<string, int> CodeEnds
        {
            get
            {
                return codeEnds;
            }
        }   

        /// <summary>
        /// Dictionary containing pairs of addressing mode names and start bits of the operand for that addressing mode.
        /// </summary>
        private Dictionary<string, int> operandStarts;

        public Dictionary<string, int> OperandStarts
        {
            get
            {
                return operandStarts;
            }
        }

        /// <summary>
        /// Dictionary containing pairs of addressing mode names and end bits of the operand for that addressing mode.
        /// </summary>
        private Dictionary<string, int> operandEnds;

        public Dictionary<string, int> OperandEnds
        {
            get
            {
                return operandEnds;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Argument()
        {
            addressingModes = new LinkedList<AddressingMode>();
            codeValues = new Dictionary<string, int>();
            codeStarts = new Dictionary<string, int>();
            codeEnds = new Dictionary<string, int>();
            operandStarts = new Dictionary<string, int>();
            operandEnds = new Dictionary<string, int>();
        }

        /// <summary>
        /// Gets expression describing this argument.
        /// </summary>
        /// <returns>
        /// Expression for the argument.
        /// </returns>
        public string GetExpression()
        {
            string retVal = "( ";
            for (int i = 0; i < addressingModes.Count; i++)
            {
                retVal += addressingModes.ElementAt(i).Name.ToUpper();
                if (i == addressingModes.Count - 1)
                {
                    retVal += " )";
                }
                else
                {
                    retVal += " | ";
                }
            }
            return retVal;
        }
    }
}
