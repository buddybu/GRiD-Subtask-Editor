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
    public partial class ServerConfig : Form
    {
        public ServerConfig()
        {
            InitializeComponent();
        }

        /*
         *  Handles dialog load event.
         *  
         *  Retrieves the last used server URL from settings
         */
        private void ServerConfig_Load(object sender, EventArgs e)
        {
            tbServerURL.Text = Properties.Settings.Default.ServerURL;
        }

        /*
         *  Handles OK click.  Checks server URL and will not allow
         *  app to continue if empty.
         */
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbServerURL.Text))
            {
                MessageBox.Show("Please enter the URL for Jira Server!");
                return;
            }

            //  save URL to settings.
            else
            {
                Properties.Settings.Default.ServerURL = tbServerURL.Text;
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
            }
        }

        /*
         *  handles cancel button. Nothing is saved in this case.
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
