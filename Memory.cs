﻿/*
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
    /// Simulates memory.
    /// </sumary>
    public class Memory
    {
        private int size = -1;

        /// <summary>
        /// Size of memory in bytes.
        /// </summary>
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

        private uint romStart;

        /// <summary>
        /// Begining address of rom memory.
        /// </summary>
        public uint RomStart
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

        private uint romEnd;

        /// <summary>
        /// Ending address of rom memory.
        /// </summary>
        public uint RomEnd
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

        private uint ramStart;

        /// <summary>
        /// Starting address of ram memory.
        /// </summary>
        public uint RamStart
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

        private uint ramEnd;

        /// <summary>
        /// Ending address of ram memory.
        /// </summary>
        public uint RamEnd
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
        private int auSize = -1;

        public int AuSize
        {
            get
            {
                return auSize;
            }
            set
            {
                auSize = value;
            }
        }

        private string initFile;

        /// <summary>
        /// Path to the initializatoin file.
        /// </summary>
        public string InitFile
        {
            get
            {
                return initFile;
            }
            set
            {
                initFile = value;
            }
        }

        private string storageFile;
        
        /// <summary>
        /// Path to the file storing values for memory.
        /// </summary>
        public string StorageFile
        {
            get
            {
                return storageFile;
            }
            set
            {
                storageFile = value;
            }
        }

        private IMemoryObserver observer;

        /// <summary>
        /// Observer following all changes.
        /// </summary>
        public IMemoryObserver Observer
        {
            get
            {
                return observer;
            }
            set
            {
                observer = value;
            }
        }

        /// <summary>
        /// Indexers for write to memory and read from memory.
        /// </summary>
        /// <param name="address">
        /// Address to read or to write from.
        /// </param>
        /// <returns>
        /// Value from wanted address.
        /// </returns>
        public byte[] this[uint address]
        {
            get
            {
                FileStream fs = new FileStream(storageFile, FileMode.Open);
                fs.Seek(address - fs.Position, SeekOrigin.Current);
                byte[] ret = new byte[auSize];
                for (int i = 0; i < auSize; i++)
                {
                    ret[i] = (byte)(fs.ReadByte());
                }
                fs.Close();
                return ret;
                
            }
            set
            {
                FileStream fs = new FileStream(storageFile, FileMode.Open);
                fs.Seek(address - fs.Position, SeekOrigin.Current);
                byte[] ret = new byte[auSize];
                for (int i = 0; i < auSize; i++)
                {
                    fs.WriteByte(value[i]);
                }
                free[address] = false;
                fs.Close();
                if (observer != null)
                {
                    SignalLocationChange(address, value);
                }
            }
        }

        /// <summary>
        /// Delegate used for notifying observer that some location was changed.
        /// </summary>
        /// <param name="address">
        /// Address of the changed location.
        /// </param>
        /// <param name="newValue">
        /// New value of the changed value.
        /// </param>
        private delegate void LocationChangeNotifier(uint address, byte[] newValue);

        /// <summary>
        /// Notifies observer that some location was changed.
        /// </summary>
        /// <param name="address">
        /// Address of the changed location.
        /// </param>
        /// <param name="newValue">
        /// New value of the changed location.
        /// </param>
        private void SignalLocationChange(uint address, byte[] newValue)
        {
            try
            {
                if (((MemoryDumpForm)observer).InvokeRequired == true)
                {
                    LocationChangeNotifier d = new LocationChangeNotifier(SignalLocationChange);
                    ((MemoryDumpForm)observer).BeginInvoke(d, new object[] { address, newValue });
                }
                else
                {
                    observer.LocationChanged(address, newValue);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", ex.ToString());
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
        public int FirstFree(int length)
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

        public int FirstFreeInRAM(int length)
        {
            for (uint i = ramStart; i < size; i++)
            {
                bool ok = true;
                for (uint j = i; j < i + length; j++)
                {
                    if (free[j] == false)
                        ok = false;
                }
                if (ok == true)
                    return (int)i;
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
        public bool Allocate(Int64 address, int length)
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

        /// <summary>
        /// Memory dump.
        /// </summary>
        public void dump()
        {
            
        }

        /// <summary>
        /// Initializes memory from init file.
        /// </summary>
        public void Initialize()
        {
            free = new bool[size];
            for (int i = 0; i < size; i++)
            {
                free[i] = true;
            }
            string[] lines = null;
            if (initFile != null && File.Exists(initFile))
            {
                lines = File.ReadAllLines(initFile);
            }
            if (storageFile == null)
            {
                storageFile = "memory.mem";
            }
            Dictionary<uint, byte[]> map = new Dictionary<uint, byte[]>();
            for (int i = 0; lines != null && i < lines.Length; i++)
            {
                string[] words = lines[i].Split(' ', '\t');
                uint address = 0;
                if (words[0].StartsWith("0x") || words[0].StartsWith("0X"))
                {
                    address = Convert.ToUInt32(words[0], 16);
                }
                else if (words[0].StartsWith("0b") || words[0].StartsWith("0B"))
                {
                    address = Convert.ToUInt32(words[0], 2);
                }
                else if (words[0].StartsWith("0o") || words[0].StartsWith("0O"))
                {
                    address = Convert.ToUInt32(words[0], 8);
                }
                else
                {
                    address = Convert.ToUInt32(words[0], 10);
                }
                byte[] val = new byte[auSize];
                for (int j = 1; j <= auSize; j++)
                {
                    if (words[j].StartsWith("0x") || words[j].StartsWith("0X"))
                    {
                        val[j - 1] = Convert.ToByte(words[j].Substring(2, words[j].Length - 2), 16);
                    }
                    else if (words[j].StartsWith("0b") || words[j].StartsWith("0B"))
                    {
                        val[j - 1] = Convert.ToByte(words[j].Substring(2, words[j].Length - 2), 2);
                    }
                    else if (words[j].StartsWith("0o") || words[j].StartsWith("0O"))
                    {
                        val[j - 1] = Convert.ToByte(words[j].Substring(2, words[j].Length - 2), 8);
                    }
                    else
                    {
                        val[j - 1] = Convert.ToByte(words[j], 10);
                    }
                }
                map.Add(address, val);
            }
            File.Delete(storageFile);
            for (uint i = 0; i < size; i++)
            {
                if (map.ContainsKey(i))
                {
                    for (int k = 0; k < auSize; k++)
                    {
                        File.AppendAllText(storageFile, "" + (char)(map[i][k]));
                        free[k + i] = false;
                    }
                }
                else
                {
                    for (int k = 0; k < auSize; k++)
                    {
                        File.AppendAllText(storageFile, "\0");
                    }
                }
            }
        }
    }
}
