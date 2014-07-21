/*
 * File: Literal.cs
 * Author: Bojan Jelaca
 * Date: October 2013
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiArc_Compiler
{
    /// <summary>
    /// Class that represents one literal in assembly code.
    /// </summary>
    /// <typeparam name="T">
    /// Type of value of literal.
    /// </typeparam>
    public class Literal<T> : ILiteral
    {

        /// <summary>
        /// Name of literal.
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
        /// Size in bytes.
        /// </summary>
        private byte size;

        public byte Size
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
        /// Value of literal.
        /// </summary>
        private T val;

        public T Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">
        /// Name of literal.
        /// </param>
        /// <param name="val">
        /// Value of literal.
        /// </param>
        public Literal(string name, T val)
        {
            this.name = name;
            this.val = val;
        }

        /// <summary>
        /// Gives the information about literal type.
        /// </summary>
        /// <returns>
        /// Type of literal.
        /// </returns>
        public Type GetType()
        {
            return val.GetType();
        }

        /// <summary>
        /// Gives the information about literal name.
        /// </summary>
        /// <returns>
        /// String that represents name of literal.
        /// </returns>
        public string GetName()
        {
            return name;
        }

    }
}