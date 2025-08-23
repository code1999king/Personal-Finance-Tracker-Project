using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.Users;

namespace Presentation
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            BllResult<User> loginRes = User.Login(txtUsername.Text, txtPassword.Text);
            if (!loginRes.IsSuccess)
                MessageBox.Show($"login Failed : {loginRes.Error.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                User user = loginRes.Value;
                MessageBox.Show($"Login Successful : UserID = {user.UserID}, Username = {user.Username}, RegisteredAt = {user.RegisteredAt}, CurrentBalance = {user.CurrentBalance}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void butRegister_Click(object sender, System.EventArgs e)
        {
            BllResult<User> registerRes = User.Register(txtUsername.Text, txtPassword.Text);
            if (!registerRes.IsSuccess)
                MessageBox.Show($"register Failed : {registerRes.Error.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                User user = registerRes.Value;
                MessageBox.Show($"register Successful : UserID = {user.UserID}, Username = {user.Username}, RegisteredAt = {user.RegisteredAt}, CurrentBalance = {user.CurrentBalance}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
