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
using Atlassian.Jira.Remote;
using SourceGrid;
using System.Xml.Serialization;
using System.IO;

namespace GRiD_Subtask_Editor
{
    [Serializable]
    public partial class AddSubtask : Form
    {

        private List<string> usernameList = new List<string>();
        private SourceGrid.Cells.Editors.ComboBox assigneeEditor = null;
        private List<SubtaskItem> subtaskList = null;
        private int subtasksCreated;

        private Jira jira;
        internal Issue ParentIssue;
        private int numberSubtasks;

        internal List<SubtaskItem> SubtaskList
        {
            get
            {
                return subtaskList;
            }
        }

        public Jira Jira
        {
            get
            {
                return jira;
            }

            set
            {
                jira = value;
            }
        }

        public AddSubtask()
        {
            InitializeComponent();
        }

        private void AddSubtask_Load(object sender, EventArgs e)
        {
//            Properties.Settings.Default.Reload();

            assigneeEditor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
            assigneeEditor.StandardValues = usernameList.ToArray();
            assigneeEditor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.SingleClick | SourceGrid.EditableMode.AnyKey;

            gridNewSubtask.BorderStyle = BorderStyle.FixedSingle;

            gridNewSubtask.ColumnsCount = 5;
            gridNewSubtask.FixedRows = 1;
            gridNewSubtask.AutoStretchColumnsToFitWidth = true;

            gridNewSubtask.Rows.Insert(0);
            gridNewSubtask[0, 0] = new SourceGrid.Cells.ColumnHeader("ADD");
            gridNewSubtask[0, 0].ToolTipText = "Select to add subtask to story.";
            gridNewSubtask[0, 1] = new SourceGrid.Cells.ColumnHeader("SUMMARY");
            gridNewSubtask[0, 2] = new SourceGrid.Cells.ColumnHeader("DESCRIPTION");
            gridNewSubtask[0, 3] = new SourceGrid.Cells.ColumnHeader("ASSIGNEE");
            gridNewSubtask[0, 4] = new SourceGrid.Cells.ColumnHeader("TIME ESTIMATE");

            // check to see if settings has a saved template
            if (Properties.Settings.Default.Template != null)
            {
                List<SubtaskItem> template = new List<SubtaskItem>();
                StringReader xmlTemplate = new StringReader(Properties.Settings.Default.Template);
                XmlSerializer xs = new XmlSerializer(template.GetType());

                template = (List<SubtaskItem>)xs.Deserialize(xmlTemplate);

                if (template != null && template.Count > 0)
                {
                    int i = 1;
                    foreach (SubtaskItem item in template)
                    {
                        gridNewSubtask.Rows.Insert(i);

                        gridNewSubtask[i, 0] = new SourceGrid.Cells.CheckBox(null, item.AddSubtask);
                        gridNewSubtask[i, 1] = new SourceGrid.Cells.Cell(item.Summary, typeof(string));
                        gridNewSubtask[i, 2] = new SourceGrid.Cells.Cell(item.Description, typeof(string));

                        gridNewSubtask[i, 3] = new SourceGrid.Cells.Cell(item.Assignee, assigneeEditor);
                        gridNewSubtask[i, 3].View = SourceGrid.Cells.Views.ComboBox.Default;

                        gridNewSubtask[i, 4] = new SourceGrid.Cells.Cell(item.Estimate, typeof(double));

                        i++;
                    }
                }
            }

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
            gridNewSubtask[index, 3].Value = "john.burnham";
            gridNewSubtask[index, 4] = new SourceGrid.Cells.Cell(0, typeof(double));

            gridNewSubtask.AutoSizeCells();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private int SubtasksAddCount()
        {
            var subtaskCount = 0;

            foreach (SubtaskItem item in subtaskList)
            {

                if (item.AddSubtask.HasValue && item.AddSubtask == true)
                {
                    subtaskCount++;
                }
            }

            return subtaskCount;
        }

        private void SaveSubtaskToTemplate()
        {
            subtaskList = new List<SubtaskItem>();
            SubtaskItem subtaskItem;

            // check to see if new subtasks has any subtasks to save
            if (gridNewSubtask.Rows.Count > 1)
            {
                for (int i = 0; i < gridNewSubtask.Rows.Count - 1; i++)
                {
                    subtaskItem = new SubtaskItem();

                    SourceGrid.Cells.CheckBox chkBox = new SourceGrid.Cells.CheckBox(null, false);
                    chkBox = (SourceGrid.Cells.CheckBox)gridNewSubtask[i + 1, 0];

                    subtaskItem.AddSubtask = chkBox.Checked;
                    subtaskItem.Summary = (string)gridNewSubtask[i + 1, 1].Value;
                    subtaskItem.Description = (string)gridNewSubtask[i + 1, 2].Value;
                    subtaskItem.Assignee = (string)gridNewSubtask[i + 1, 3].Value;
                    subtaskItem.Estimate = (double)gridNewSubtask[i + 1, 4].Value;

                    subtaskList.Add(subtaskItem);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            subtaskCreateWorker.CancelAsync();
            DialogResult = DialogResult.Cancel;
        }

        internal void SetUserCombo(List<string> userList)
        {
            foreach(var user in userList)
            {
                usernameList.Add(user);
            }

        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            SaveSubtaskToTemplate();

            StringWriter sw = new StringWriter();

            XmlSerializer s = new XmlSerializer(subtaskList.GetType());

            s.Serialize(sw, subtaskList);

            Properties.Settings.Default.Template = sw.ToString();

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        internal void AddSubtasksToIssue(Jira jira, Issue parentIssue)
        {
            if (parentIssue != null)
            {

                foreach (SubtaskItem item in subtaskList)
                {

                    if (item.AddSubtask.HasValue && item.AddSubtask == true)
                    {
#if BUD
                        Issue subtask = jira.CreateIssue(parentIssue.Project, parentIssue.Key.ToString());
                        subtask.Type = "5";
                        subtask.Summary = item.Summary;
                        subtask.Description = item.Description;
                        subtask.Assignee = item.Assignee;
                        foreach (ProjectComponent pc in parentIssue.Components)
                            subtask.Components.Add(pc);

                        await subtask.SaveChangesAsync();


                        // set the time estimate
                        var editList = new List<object>();
                        editList.Add(new { edit = new { originalEstimate = item.Estimate + "h"} });
                        var myObject = new { fields = new { timetracking = new { originalEstimate = item.Estimate + "h" } } };
                        var url = "/rest/api/2/issue/" + subtask.Key.ToString();
                        await jira.RestClient.ExecuteRequestAsync(RestSharp.Method.PUT, url, myObject);
#else
                        System.Threading.Thread.Sleep(500);
#endif
                    }
                }
            }
        }

        private void subtaskCreateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            lblStatus.Text = e.ProgressPercentage.ToString() + " %";
        }

        private void subtaskCreateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                lblStatus.Text = "ERROR!";
            }
            else if(e.Cancelled)
            {
                lblStatus.Text = "Cancelled!";
            }
            else
            {
                lblStatus.Text = "Success!";
            }
            btnDone.Enabled = true;
            btnCreateSubtasks.Enabled = false;
        }

        private  void subtaskCreateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            foreach (SubtaskItem item in subtaskList)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (item.AddSubtask.HasValue && item.AddSubtask == true)
                    {
#if BUD
                        Issue subtask = jira.CreateIssue(ParentIssue.Project, ParentIssue.Key.ToString());

                        subtask.Type = "5";
                        subtask.Summary = item.Summary;
                        subtask.Description = item.Description;
                        subtask.Assignee = item.Assignee;
                        foreach (ProjectComponent pc in ParentIssue.Components)
                            subtask.Components.Add(pc);

                        await subtask.SaveChangesAsync();

                        // set the time estimate
                        var editList = new List<object>();
                        editList.Add(new { edit = new { originalEstimate = item.Estimate + "h" } });
                        var myObject = new { fields = new { timetracking = new { originalEstimate = item.Estimate + "h" } } };
                        var url = "/rest/api/2/issue/" + subtask.Key.ToString();
                        await jira.RestClient.ExecuteRequestAsync(RestSharp.Method.PUT, url, myObject);
#else
                        System.Threading.Thread.Sleep(500);
#endif
                        subtasksCreated++;
                        worker.ReportProgress( (int) (((double)subtasksCreated / (double)numberSubtasks) * 100.0) );

                    }
                }
            }

        }

        private void btnCreateSubtasks_Click(object sender, EventArgs e)
        {
            // save list of rows to settings as default
            // save list of rows to local variable to be accessible from main page

            SaveSubtaskToTemplate();
            btnAddRow.Enabled = false;
            btnTemplate.Enabled = false;
            btnDone.Enabled = false;

            subtasksCreated = 0;
            numberSubtasks = SubtasksAddCount();


            if (numberSubtasks > 0)
            {
                subtaskCreateWorker.RunWorkerAsync();
            }


        }
    }
}
