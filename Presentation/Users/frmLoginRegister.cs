using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BusinessLogic;
using BusinessLogic.Users;

namespace Presentation.Users
{
    public partial class frmLoginRegister : Form
    {
        private enum AuthMode { Login, Register};
        private AuthMode authMode = AuthMode.Login;
        public frmLoginRegister()
        {
            InitializeComponent();
        } 
        private void SwitchToLoginMode()
        {
            lblFormTitle.Text = "Login to Personal Finance Tracker";
            btnLoginRegister.Text = "Login";
            lblAuthToggleMessage.Text = "Don't have an account yet?";
            llSwitchAuthMode.Text = "register instead";
            lblConfirmPassword.Visible = txtConfirmPassword.Visible = false;
            authMode = AuthMode.Login;
        }
        private void SwitchToRegisterMode()
        {
            lblFormTitle.Text = "Register to Personal Finance Tracker";
            btnLoginRegister.Text = "Register";
            lblAuthToggleMessage.Text = "Already have an account?";
            llSwitchAuthMode.Text = "login instead";
            lblConfirmPassword.Visible = txtConfirmPassword.Visible = true;
            authMode = AuthMode.Register;
        }
        private void PerformLogin()
        {
            
            BllResult<User> loginRes = User.Login(txtUsername.Text, txtPassword.Text);
            if (!loginRes.IsSuccess)
            {
                Util.ShowError(loginRes.Error.ToString());
                return;
            }
            // login succeeded:
            Global.CurrentUser = loginRes.Value;
            this.Hide();
            OpenStartPage();
        }
        private void PerformRegister()
        {
            BllResult<User> registerRes = User.Register(txtUsername.Text, txtPassword.Text);
            if (!registerRes.IsSuccess)
            {
                Util.ShowError(registerRes.Error.ToString());
                return;
            }
            // registration succeeded:
            Global.CurrentUser = registerRes.Value;
            this.Hide();
            OpenStartPage();
        }
        private void OpenStartPage()
        {
            frmStartPage frm = new frmStartPage();
            frm.StartPageClosed += OnStartPageClosed;
            frm.ShowDialog();
        }



        private void OnStartPageClosed()
        {
            if (Global.CurrentUser == null) // user logged out
                this.Show();
            else    // user closed the app
                Application.Exit();
        }
        private void llSwitchAuthMode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch(authMode)
            {
                case AuthMode.Login:
                    SwitchToRegisterMode();
                    break;
                case AuthMode.Register:
                    SwitchToLoginMode();
                    break;
            }
        }

        private void btnLoginRegister_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren(ValidationConstraints.Visible)) return;

            switch(authMode)
            {
                case AuthMode.Login:
                    PerformLogin();
                    break;
                case AuthMode.Register:
                    PerformRegister();
                    break;
            }
        }

        private void txtUsername_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is required");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtUsername, null);
            }
        }

        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Confirmation password does not match password");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void frmLoginRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmLoginRegister_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show("form validating");
        }
    }
}
