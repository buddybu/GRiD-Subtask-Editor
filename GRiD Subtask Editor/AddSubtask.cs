using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atlassian.Jira;

namespace GRiD_Subtask_Editor
{
    public partial class AddSubtask : Form
    {

        private List<string> usernameList = new List<string>();
        private SourceGrid.Cells.Editors.ComboBox assigneeEditor = null;

        public AddSubtask()
        {
            InitializeComponent();
        }

        private void AddSubtask_Load(object sender, EventArgs e)
        {
            assigneeEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            assigneeEditor.StandardValues = usernameList.ToArray();
            assigneeEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

            gridNewSubtask.BorderStyle = BorderStyle.FixedSingle;

            gridNewSubtask.ColumnsCount = 5;
            gridNewSubtask.FixedRows = 1;
            gridNewSubtask.Rows.Insert(0);
            gridNewSubtask.AutoStretchColumnsToFitWidth = true;

            gridNewSubtask[0, 0] = new SourceGrid.Cells.ColumnHeader("ADD");
            gridNewSubtask[0, 1] = new SourceGrid.Cells.ColumnHeader("SUMMARY");
            gridNewSubtask[0, 2] = new SourceGrid.Cells.ColumnHeader("DESCRIPTION");
            gridNewSubtask[0, 3] = new SourceGrid.Cells.ColumnHeader("ASSIGNEE");
            gridNewSubtask[0, 4] = new SourceGrid.Cells.ColumnHeader("TIME ESTIMATE");

            gridNewSubtask.AutoSizeCells();
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            int index = gridNewSubtask.Rows.Count;

            gridNewSubtask.Rows.Insert(index);

            gridNewSubtask[index, 0] = new SourceGrid.Cells.CheckBox(null, true);
            gridNewSubtask[index, 1] = new SourceGrid.Cells.Cell("", typeof(string));
            gridNewSubtask[index, 2] = new SourceGrid.Cells.Cell("", typeof(string));
            gridNewSubtask[index, 3] = new SourceGrid.Cells.Cell("", assigneeEditor);
            gridNewSubtask[index, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
            gridNewSubtask[index, 4] = new SourceGrid.Cells.Cell(0, typeof(double));

            gridNewSubtask.AutoSizeCells();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        internal void SetUserCombo(List<string> userList)
        {
            foreach(var user in userList)
            {
                usernameList.Add(user);
            }

        }
    }
}
