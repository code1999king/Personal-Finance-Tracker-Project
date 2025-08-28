using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmStartPage : Form
    {
        public frmStartPage()
        {
            InitializeComponent();
        }

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
