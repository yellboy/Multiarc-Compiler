/*
 * File: Register.cs
 * Author: Bojan Jelaca
 * Date: April 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiArc_Compiler
{
    /// <summary>
    /// Class representing one register.
    /// </summary>
    public class Register
    {

        /// <summary>
        /// Size of register in bits.
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
        /// All posible names of the register.
        /// </summary>
        private LinkedList<string> names;

        public LinkedList<string> Names
        {
            get
            {
                return names;
            }
            set
            {
                names = value;
            }
        }

        /// <summary>
        /// Base register for this register.
        /// </summary>
        private Register baseReg;

        public Register BaseReg
        {
            get
            {
                return baseReg;
            }
            set
            {
                baseReg = value;
            }
        }

        /// <summary>
        /// First bit of base register, if this register is part of another register.
        /// </summary>
        private int start;

        public int Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        /// <summary>
        /// Last bit of base register, if this register is part of another register.
        /// </summary>
        private int end;

        public int End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        /// <summary>
        /// Value of the register.
        /// </summary>
        private int val;

        public int Val
        {
            get // Must be checked.
            {
                return val;
            }
            set // Must be checked.
            {
                if (start == 0 && end == this.size - 1)
                    val = value;
                else
                {
                    val = value;
                    if (this.parts.Count > 0)
                    {
                        foreach (Register r in Parts)
                        {
                            int mask = 0;
                            for (int i = 0; i < this.size; i++)
                            {
                                if (i >= r.start && i <= r.end)
                                {
                                    mask |= 1 << i;
                                }
                            } 
                            int subValue = (val & mask) >> r.start;
                            r.val = subValue;
                        }
                    }
                    if (baseReg != null)
                    {
                        baseReg.PartValueChanged(val, this); 
                    }
                }
            }
        }

        /// <summary>
        /// Group of registers where this register belongs.
        /// </summary>
        private string group;

        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
            }
        }

        /// <summary>
        /// Linked list containing all the parts of the register.
        /// </summary>
        private LinkedList<Register> parts;

        public LinkedList<Register> Parts
        {
            get
            {
                return parts;
            }
            set
            {
                parts = value;
            }
        }

        public Register(int size, int val = 0)
        {
            this.val = val;
            this.size = size;
            this.group = null;
            parts = new LinkedList<Register>();
            names = new LinkedList<string>();
        }

        /// <summary>
        /// Adds another name for register into it's names list.
        /// </summary>
        /// <param name="name">
        /// Another name of the register.
        /// </param>
        /// <returns>
        /// True if new name is not already in the list and false otherwise.
        /// </returns>
        public bool AddName(string name)
        {
            foreach (string n in names)
            {
                if (n.Equals(name))
                {
                    return false;
                }
            }
            names.AddLast(name);
            return true;
        }

        /// <summary>
        /// Adds new part into the parts list.
        /// </summary>
        /// <param name="part">
        /// New part to be added.
        /// </param>
        public void AddPart(Register part)
        {
            parts.AddLast(part);
            int mask = 0;
            for (int i = 0; i < this.size; i++)
            {
                if (i >= part.start && i <= part.end)
                {
                    mask |= 1 << i;
                }
            }
            int subValue = (val & mask) >> part.start;
            part.val = subValue;
        }

        public void PartValueChanged(int subValue, Register part)
        {
            int mask = 0;
            for (int i = 0; i < size; i++)
            {
                if (i >= part.start && i <= part.end)
                {
                    mask |= 1 << i;
                }
            }
            int superValue = subValue;
            superValue &= (mask | (val << part.start));
            this.val = superValue;
            foreach (Register r in Parts)
            {
                if (r != part)
                {
                    mask = 0;
                    for (int i = 0; i < this.size; i++)
                    {
                        if (i >= r.start && i <= r.end)
                        {
                            mask |= 1 << i;
                        }
                    }
                    int subVal = (val & mask) >> r.start;
                    r.val = subVal;
                }
            }
        }
    }
}
