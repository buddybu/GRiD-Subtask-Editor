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

        public string Username
        {
            get
            {
                return username;
            }
       }

        public string Password
        {
            get
            {
                return password;
            }
        }

        public UserConfig()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            username = tbUsername.Text;
            password = tbPassword.Text;

            Properties.Settings.Default.ServerUsername = tbUsername.Text;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void UserConfig_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            tbUsername.Text = Properties.Settings.Default.ServerUsername;

            btnOK.Select();
        }
    }
}
