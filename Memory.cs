/*
* File: Memory.cs
* Author: Bojan Jelaca
* Date: September 2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MultiArc_Compiler
{
    /// <sumary>
    /// Simulates RAM memory.
    /// </sumary>
    public class Memory
    {
        /// <summary>
        /// Size of memory in bytes.
        /// </summary>
        private int size
        {
            get;
            set;
        }
        
        /// <summary>
        /// Array that represents memory.
        /// </summary>
        private byte[][] memory;

        /// <summary>
        /// Array that represents the information about taken memory.
        /// </summary>
        private bool[] free;

        /// <summary>
        /// Size of addressible unit in bytes.
        /// </summary>
        private int auSize;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="size">
        /// Desired size of memory in number of addressible units.
        /// </param>
        /// <param name="auSize">
        /// Size of addressible unit in bytes.
        /// </param>
        public Memory(int size, int auSize) 
        {
            this.size = size;
            this.auSize = auSize;
            memory = new byte[size][];
            for (int i = 0; i < size; i++)
                memory[i] = new byte[auSize];
            free = new bool[size];
            for (int i = 0; i < size; i++)
            {
                free[i] = true;
            }
        }

        /// <summary>
        /// Indexers for write to memory and read from memory.
        /// </summary>
        /// <param name="address">
        /// Address to read or to write from.
        /// </param>
        /// <returns></returns>
        public byte[] this[uint address]
        {
            get
            {
                return memory[address];
            }
            set
            {
                free[address] = false;
                memory[address] = value;
            }
        }


        /// <summary>
        /// Gets the address of the first free block in memory.
        /// </summary>
        /// <param name="length">
        /// Length of the required block.
        /// </param>
        /// <returns>
        /// Address of the required block or -1 if there is not enough free space. 
        /// </returns>
        public Int64 firstFree(int length)
        {
            for (int i = 0; i < size; i++)
            {
                bool ok = true;
                for (int j = i; j < i + length; j++)
                {
                    if (free[j] == false)
                        ok = false;
                }
                if (ok == true)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Allocates memory.
        /// </summary>
        /// <param name="address">
        /// Starting address.
        /// </param>
        /// <param name="length">
        /// Length in bytes to allocate.
        /// </param>
        /// <returns>
        /// Bool value that indicates wheter allocation was successfull. 
        /// </returns>
        public bool allocate(Int64 address, int length)
        {
            if (length + address > size)
                return false;
            for (Int64 i = address; i < address + length; i++)
            {
                if (free[i] == false)
                    return false;
            }
            for (Int64 i = address; i < address + length; i++)
            {
                free[i] = false;
            }
            return true;
        }

        public void dump(string path = "memory.mem")
        {
            File.Delete(path);
            for (int i = 0; i < size; i++)
            {
                if (free[i] == false)
                {
                    string text = "" + i + ":\t";
                    for (int j = 0; j < auSize; j++)
                    {
                        text += memory[i][j] + " ";
                    }
                    text += "\n";
                    File.AppendAllText(path, text);
                }

            }
        }
    }
}
