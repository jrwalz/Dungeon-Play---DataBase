/*
 * Student: Jerry Ray Walz Jr
 * Class: CIS262AD
 * Section: 29682
 * Assignment: Final Project, Level II Class
 * Submission Date: 5/9/2022
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Play
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Character trans = new Character();
            trans.firstMethod();

            InitializeOnce lockedGui = new InitializeOnce();
            lockedGui.LockedGui = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }

    public class InitializeOnce
    {
        private static bool lockedGui;

        public bool LockedGui { get { return lockedGui; } set {lockedGui = value; } }

        public InitializeOnce() {
      
        }

        public InitializeOnce(bool start)
        {
            LockedGui = start;
        }
    }
}
