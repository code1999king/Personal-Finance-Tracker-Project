using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    internal class Util
    {
        /// <summary>
        /// Displays an error message with "Error" caption, OK button, Error icon.
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
