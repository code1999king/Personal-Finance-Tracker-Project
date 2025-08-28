using System;
using System.Windows.Forms;
using Presentation.TestForms;
using Presentation.Users;

namespace Presentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLoginRegister());
        }
    }
}
