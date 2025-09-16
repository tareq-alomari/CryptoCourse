using System;
using System.Windows.Forms;

namespace CryptoCourse.WinFormsUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Standard Windows Forms setup calls.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This is the most important line:
            // It creates a new instance of our MainForm class and runs it,
            // which displays the window to the user.
            Application.Run(new WinFormsUI.MainForm());
        }
    }
}