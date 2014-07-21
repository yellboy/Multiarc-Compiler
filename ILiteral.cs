/*
 * File: ILitireal.cs
 * Author: Bojan Jelaca
 * Date: October 2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiArc_Compiler
{
    interface ILiteral
    {
        Type GetType();
        string GetName();
    }
}
