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
    public partial class WaitDialog : Form
    {
        public WaitDialog()
        {
            InitializeComponent();
        }

        /*
         *  This property allows dialog message to be changed as needed
         */
        public string ProgressText
        {
            get { return waitMessage.Text; }
            set { waitMessage.Text = value; }
        }
    }
}
