using System;
using System.Windows.Forms;
using Presentation.Users;

namespace Presentation
{
    public partial class frmStartPage : Form
    {
        public frmStartPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs whenever the StartPage form is closed. Used by <see cref="frmLoginRegister"/> form.
        /// </summary>
        public event Action StartPageClosed;

        private void frmStartPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            // To catch this event by frmLoginRegister form
            StartPageClosed?.Invoke();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.CurrentUser = null; // logging out
            this.Close();
        }
    }
}
