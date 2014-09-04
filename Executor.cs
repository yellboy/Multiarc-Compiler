/*
 * File: Executor.cs
 * Author: Bojan Jelaca
 * Date: April 2014
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

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
        /// True if program is executed in step by step mode.
        /// </summary>
        private volatile bool stepByStepMode = false;
        
        /// <summary>
        /// Semaphores for threads synchronization.
        /// </summary>
        private Semaphore sem = new Semaphore(0, 1), instSem = new Semaphore(0, 1), breakSem = new Semaphore(0, 1);

        /// <summary>
        /// Thread used when in step by step mode.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// True if some program is executing.
        /// </summary>
        private volatile bool executing = false;

        private LinkedList<int> breakPoints;

        /// <summary>
        /// True if some program is currently being executed.
        /// </summary>
        public bool Executing
        {
            get
            {
                bool temp;
                Thread.BeginCriticalRegion();
                temp = executing;
                Thread.EndCriticalRegion();
                return temp;
            }
            set
            {
                Thread.BeginCriticalRegion();
                executing = true;
                Thread.EndCriticalRegion();
            }
        }

        private volatile int next;

        /// <summary>
        /// Next instruction to be executed.
        /// </summary>
        public int Next
        {
            get
            {
                int temp;
                Thread.BeginCriticalRegion();
                temp = next;
                Thread.EndCriticalRegion();
                return temp;
            }
            set
            {
                Thread.BeginCriticalRegion();
                next = value;
                Thread.EndCriticalRegion();
            }
        }

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
        public Executor(ArchConstants constants, byte[] binaryCode, LinkedList<int> separators, LinkedList<int> breakPoints, TextBoxBase output,  int entryPoint = 0) 
        {
            this.constants = constants;
            this.binaryCode = binaryCode;
            this.entryPoint = entryPoint;
            this.separators = separators;
            this.breakPoints = breakPoints;
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
            Thread.BeginCriticalRegion();
            executing = true;
            Thread.EndCriticalRegion();
            Variables vars = new Variables();
            vars.SetVariable("working", true);
            while (true) 
            {
                int pc = constants.GetRegister("pc").Val;
                if (separators.Contains(pc) && pc != entryPoint)
                {
                    Instruction inst = constants.MatchInstruction(binary.ToArray());
                    InstructionRegister ir = new InstructionRegister(binary.ToArray());
                    inst.ReadAddressingModes(binary.ToArray());
                    int[] operands = inst.FetchOperands(ir, constants, vars);
                    int[] result = inst.Execute(ir, constants, vars, operands);
                    inst.StoreResult(ir, constants, vars, result);
                    binary.Clear();
                    pc = constants.GetRegister("pc").Val;
                } 
                if (pc >= binaryCode.Length || (bool)vars.GetVariable("working") == false)
                {
                    Thread.BeginCriticalRegion();
                    executing = false;
                    Thread.EndCriticalRegion();
                    break;
                }
                binary.AddLast(binaryCode[pc++]);
                constants.GetRegister("pc").Val = pc;
            }
            output.Text += DateTime.Now.ToString() + " Code executed successfully.\n";
            output.ScrollToCaret();
        }
        
        /// <summary>
        /// Executes one instruction.
        /// </summary>
        /// <param name="next">
        /// Line of the instruction that shall be executed after this method is executed. Note: This is out parameter.
        /// </param>
        /// <returns>
        /// Bool value signaling whether executed instruction is last in program.
        /// </returns>
        public bool ExecuteNextStep()
        {
            Thread.BeginCriticalRegion();
            stepByStepMode = true;
            Thread.EndCriticalRegion();
            sem.Release(1);
            return !thread.IsAlive; 
        }

        /// <summary>
        /// Executes binary code step by step.
        /// </summary>
        private void executeStepByStep()
        {
            LinkedList<Instruction> instructions = new LinkedList<Instruction>();
            LinkedList<byte> binary = new LinkedList<byte>();
            Thread.BeginCriticalRegion();
            executing = true;
            Thread.EndCriticalRegion();
            try
            {
                Variables vars = new Variables();
                vars.SetVariable("working", true);
                while (true)
                {
                    int pc = constants.GetRegister("pc").Val;
                    if (separators.Contains(pc - entryPoint) && pc != entryPoint)
                    {
                        Thread.BeginCriticalRegion();
                        bool temp = stepByStepMode;
                        Thread.EndCriticalRegion();
                        if (temp == true || breakPoints.Contains(next))
                        {
                            if (breakPoints.Contains(next) && temp == false)
                            {
                                breakSem.Release(1);
                            }
                            sem.WaitOne();
                        }
                        Instruction inst = constants.MatchInstruction(binary.ToArray());
                        InstructionRegister ir = new InstructionRegister(binary.ToArray());
                        inst.ReadAddressingModes(binary.ToArray());
                        int[] operands = inst.FetchOperands(ir, constants, vars);
                        int[] result = inst.Execute(ir, constants, vars, operands);
                        inst.StoreResult(ir, constants, vars, result);
                        binary.Clear();
                        pc = constants.GetRegister("pc").Val;
                        if (separators.Contains(pc - entryPoint))
                        {
                            for (int i = 0; i < separators.Count - 1; i++)
                            {
                                if (separators.ElementAt(i) == pc - entryPoint)
                                {
                                    Thread.BeginCriticalRegion();
                                    next = i;
                                    Thread.EndCriticalRegion();
                                }   
                            }
                        }
                        if (pc - entryPoint >= binaryCode.Length || (bool)vars.GetVariable("working") == false)
                        {

                            Thread.BeginCriticalRegion();
                            executing = false;
                            Thread.EndCriticalRegion();
                            instSem.Release(1);
                            Thread.Yield();
                            break;
                        } 
                        Thread.BeginCriticalRegion();
                        temp = stepByStepMode;
                        Thread.EndCriticalRegion();
                        if (temp == true)
                        {
                            instSem.Release(1);
                            Thread.Yield();
                        }
                    }
                    byte[] readFromMemory = Program.Mem[(uint)pc];
                    pc++;
                    for (int k = 0; k < readFromMemory.Length; k++)
                    {
                        binary.AddLast(readFromMemory[k]);
                    }
                    //binary.AddLast(binaryCode[pc++]);
                    constants.GetRegister("pc").Val = pc;
                }
                breakSem.Release();
                writeToOutput(" Code executed successfully.\n");;
            }
            catch (Exception ex)
            {
                writeToOutput(DateTime.Now + " Execution error: " + ex.Message + "\n");
                File.AppendAllText("error.txt", ex.ToString());
            }
        }

        /// <summary>
        /// Delegate used to write to output.
        /// </summary>
        /// <param name="text">
        /// Text to be written.
        /// </param>
        private delegate void SetTextCallback(string text);

        /// <summary>
        /// Method used to write to output from different thread.
        /// </summary>
        /// <param name="text">
        /// Text to be written.
        /// </param>
        private void writeToOutput(string text)
        {

            if (output.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(writeToOutput);
                Form1.Instance.BeginInvoke(d, new object[] { text });
            }
            else
            {
                output.Text += DateTime.Now.ToString() + text;
                output.ScrollToCaret();
            }
        }

        /// <summary>
        /// Enter step by step mode.
        /// </summary>
        public void EnterStepByStep()
        {
            constants.GetRegister("pc").Val = entryPoint;
            Thread.BeginCriticalRegion();
            next = 0;
            Thread.EndCriticalRegion();
            Thread.BeginCriticalRegion();
            stepByStepMode = true;
            Thread.EndCriticalRegion();
            thread = new Thread(new ThreadStart(executeStepByStep));
            thread.Start();
        }

        /// <summary>
        /// Abort execution, if there is any.
        /// </summary>
        public void Abort()
        {
            Thread.BeginCriticalRegion();
            executing = false;
            Thread.EndCriticalRegion();
            if (thread != null)
            {
                thread.Abort();
            }
        }

        /// <summary>
        /// Continue execution and exit step by step mode.
        /// </summary>
        public void Continue()
        {
            Thread.BeginCriticalRegion();
            stepByStepMode = false;
            Thread.EndCriticalRegion();
            sem.Release(1);
        }

        /// <summary>
        /// Wait until one instruction is executed.
        /// </summary>
        public void WaitForOneInstruction()
        {
            instSem.WaitOne();
        }

        /// <summary>
        /// Debug program.
        /// </summary>
        public void Debug()
        {
            constants.GetRegister("pc").Val = entryPoint;
            Thread.BeginCriticalRegion();
            next = 0;
            Thread.EndCriticalRegion();
            Thread.BeginCriticalRegion();
            stepByStepMode = false;
            Thread.EndCriticalRegion();
            thread = new Thread(new ThreadStart(executeStepByStep));
            thread.Start();
        }

        /// <summary>
        /// Wait untill breakpoint is reached or execution is over.
        /// </summary>
        public void WaitUntilBreakpointOrEnd()
        {
            breakSem.WaitOne();
        }
    }
}
