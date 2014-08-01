/*
 * File: AddressingMode.cs
 * Author: Bojan Jelaca
 * Date: March 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace MultiArc_Compiler
{

    /// <summary>
    /// Class that represents one type of addressing mode.
    /// </summary>
    public class AddressingMode
    {

        /// <summary>
        /// Name of the addressing mode. Per example, memdir, memind, regdir, imm...
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
        /// Path to the file with execution code for this addressing mode.
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
        /// Binary code of this addressing mode.
        /// </summary>
        private byte code;

        public byte Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        /// <summary>
        /// Code to be executed when GetData method is called.
        /// </summary>
        private string executionCode;

        /// <summary>
        /// List of regular expressions representing this addressing mode in assembly file.
        /// </summary>
        private LinkedList<string> expressions = new LinkedList<string>();

        public LinkedList<string> Expressions
        {
            get
            {
                return expressions;
            }
        }

        /// <summary>
        /// Result that this addressing mode returns.
        /// </summary>
        private Data result;

        public Data Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }


        /// <summary>
        /// Adds new expression to expressions list.
        /// </summary>
        /// <param name="expression">
        /// Expression to be added.
        /// </param>
        /// <returns>
        /// True if expression is not already in the list and false otherwise.
        /// </returns>
        public bool AddExpression(string expression)
        {
            foreach (string e in expressions)
            {
                if (e.Equals(expression))
                {
                    return false;
                }
            }
            expressions.AddLast(expression);
            return true;
        }
        
        /// <summary>
        /// Dictionary containing pairs of expressions and their joined values. 
        /// </summary>
        private Dictionary<string, int> values;

        public Dictionary<string, int> Values
        {
            get
            {
                return values;
            }
        }

        /// <summary>
        /// True if this addressing mode can read operand value from expression.
        /// </summary>
        private bool operandReadFromExpression = false;

        public bool OperandReadFromExpression
        {
            get
            {
                return operandReadFromExpression;
            }
            set
            {
                operandReadFromExpression = value;
            }
        }

        private bool operandInValues = false;

        /// <summary>
        /// True if this addressing mode has set of binary code for operand for every expression.
        /// </summary>
        public bool OperandInValues
        {
            get
            {
                return operandInValues;
            }
            set
            {
                operandInValues = value;
            }
        }

        private bool operandValueDefinedByUser = false;

        /// <summary>
        /// True if this the way to get binary value for operand of this addressing mode is defined by user.
        /// </summary>
        public bool OperandValueDefinedByUser
        {
            get
            {
                return operandValueDefinedByUser;
            }
            set
            {
                operandValueDefinedByUser = value;
            }
        }

        private string operandType;

        /// <summary>
        /// Relative or absolute.
        /// </summary>
        public string OperandType
        {
            get
            {
                return operandType;
            }
            set
            {
                operandType = value;
            }
        }

        /// <summary>
        /// Adds new pair of expression and its joined value.
        /// </summary>
        /// <param name="expression">
        /// Expression to be added.
        /// </param>
        /// <param name="value">
        /// Joined value.
        /// </param>
        public void AddValue(string expression, int value)
        {
            if (values.ContainsKey(expression))
            {
                values[expression] = value;
            }
            else
            {
                values.Add(expression, value);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">
        /// Name of the addressing mode.
        /// </param>
        /// <param name="code">
        /// Binary code for the addressing mode.
        /// </param>
        /// <param name="result">
        /// Result this addressing mode returns.
        /// </param>
        /// <param name="fileName">
        /// Name of the file where the execution code for this addressing mode is.
        /// </param>
        public AddressingMode(string name = null, byte code = 0, Data result = null, string fileName = null)
        {
            this.name = name;
            this.code = code;
            this.fileName = fileName;
            this.result = result;
            if (fileName == null)
            {
                executionCode = null;
            }
            else
            {
                executionCode = File.ReadAllText(fileName);
            }
            values = new Dictionary<string, int>();
        }


        /// <summary>
        /// Method that compares this addressing mode with another.
        /// </summary>
        /// <param name="am">
        /// Another addressing mode to be compared with.
        /// </param>
        /// Bool value that tells whether two addressing modes are equal or not.
        /// <returns></returns>
        public bool Equals(AddressingMode am)
        {
            return this.name.Equals(am.name);
        }

        /// <summary>
        /// Gets address and data for this addressing mode.
        /// </summary>
        /// <param name="ir">
        /// Binary code of the instruction represented with InstructionRegister object.
        /// </param>
        /// <param name="constants">
        /// Constants representing current architecture.
        /// </param>
        /// <param name="startBit">
        /// Start bit of the operand in instruction code.
        /// </param>
        /// <param name="endBit">
        /// End bit of the operand in instruction code.
        /// </param>
        /// <returns>
        /// Wanted data.
        /// </returns>
        public int GetData(InstructionRegister ir, ArchConstants constants, int startBit, int endBit)
        {
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingNotDynamicClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingNotDynamicClass);
            string code = @"

using System;
using System.IO;
using MultiArc_Compiler;

public class DynamicClassAM
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
                return 0;
            }
            else
            {
                var t = results.CompiledAssembly.GetType("DynamicClassAM");
                int result = 0;
                object[] parameters = new object[] { ir, Program.Mem, constants, startBit, endBit, result};
                t.GetMethod("getAddrData_" + this.name).Invoke(null, parameters);
                result = (int)(parameters[5]);
                return result;
            }
        }

        public void SetData(InstructionRegister ir, ArchConstants constants, int startBit, int endBit, int data)
        {
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingNotDynamicClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingNotDynamicClass);
            string code = @"

using System;
using System.IO;
using MultiArc_Compiler;

public class DynamicClassAM
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
            }
            else
            {
                var t = results.CompiledAssembly.GetType("DynamicClassAM");
                object[] parameters = new object[] { ir, Program.Mem, constants, startBit, endBit, data };
                t.GetMethod("setAddrData_" + this.name).Invoke(null, parameters);
            }
        }

        /// <summary>
        /// Calculates binary value for operand.
        /// </summary>
        /// <param name="image">
        /// String representing instruction in assembly language.
        /// </param>
        /// <param name="currentLocation">
        /// Current value of location counter (value of pc during execution).
        /// </param>
        /// <returns>
        /// Binary value for operand.
        /// </returns>
        public int GetOperandValue(string image, int currentLocation, int relativeValue, int absoluteValue)
        {
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingNotDynamicClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingNotDynamicClass);
            string code = @"

using System;
using System.IO;
using MultiArc_Compiler;

public class DynamicClassAM
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
                return 0;
            }
            else
            {
                var t = results.CompiledAssembly.GetType("DynamicClassAM");
                int operand = 0;
                object[] parameters = new object[] { image, currentLocation, relativeValue, absoluteValue, operand };
                t.GetMethod("getOperand_" + this.name).Invoke(null, parameters);
                operand = (int)parameters[4];
                return operand;
            }
        }
    }
}
