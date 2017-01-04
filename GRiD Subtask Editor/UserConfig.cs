using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRiD_Subtask_Editor
{
    public partial class UserConfig : Form
    {
        private String username;
        private String password;

        //  username to connect to Jira
        public string Username
        {
            get { return username; }
       }

        //  Password to connect to Jira
        public string Password
        {
            get { return password; }
        }

        public UserConfig()
        {
            InitializeComponent();
        }

        //  handler for Cancel button.
        //  nothing is saved to settings.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        //  Handler for OK button.
        //  if username is empty, then not allowed.  Otherwise
        //  saves username to settings and sets status to OK
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbUsername.Text))
            {
                MessageBox.Show("Please provide a username.");
                return;
            }
            else
            {
                username = tbUsername.Text;
                password = tbPassword.Text;

                Properties.Settings.Default.ServerUsername = tbUsername.Text;
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
            }
        }

        //  Dialog load handler.  
        //  Upon loading, retrieves the settings and sets the last used username
        private void UserConfig_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            tbUsername.Text = Properties.Settings.Default.ServerUsername;

            // set input position to password
            tbPassword.Select();
        }
    }
}
