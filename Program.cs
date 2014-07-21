using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace MultiArc_Compiler
{
    static class Program
    {

        public static Memory Mem;

        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                //File.("error.txt");
                File.WriteAllText("error.txt", ex.ToString());
            }
        }
    }
}
