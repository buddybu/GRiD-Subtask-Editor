using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Atlassian.Jira;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace GRiD_Subtask_Editor
{
    [Serializable]
    public partial class AddSubtask : Form
    {
        private string defaultTitle;
        private List<string> usernameList = new List<string>();
        private SourceGrid.Cells.Editors.ComboBox assigneeEditor = null;
        private List<SubtaskItem> subtaskList = null;
        private int subtasksCreated;
        private int numSubtasksToCreate;

        private Jira jira;
        internal Issue ParentIssue;
        private int numberSubtasks;
        private bool bCreateSubtaskDone;

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
            defaultTitle = this.Text;
        }

        /*
         *  Handle form load event
         *      Creates the editor for the grid
         *      Creates the grid
         *      Initializes the header row
         *      If saved template is available, load the template
         */
        private void AddSubtask_Load(object sender, EventArgs e)
        {
            // set the title of the dialog
            this.Text = defaultTitle + " for  " + ParentIssue.Key.ToString();

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
            if (!String.IsNullOrWhiteSpace( Properties.Settings.Default.Template ))
            {
                List<SubtaskItem> template = new List<SubtaskItem>();
                StringReader xmlTemplate = new StringReader(Properties.Settings.Default.Template);
                XmlSerializer xs = new XmlSerializer(template.GetType());

                // template is stored in settings as an XML object, so we must deserialize it into a list.
                template = (List<SubtaskItem>)xs.Deserialize(xmlTemplate);

                // only process the template if it is not empty.
                if (template != null && template.Count > 0)
                {
                    int i = 1;

                    // add each item from the template to the grid.
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

        /*
         *  Method to handle the add row button event
         *  
         *  This will add a new uninitialized row to the new subtask grid.
         */
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            // get the current row count
            int index = gridNewSubtask.Rows.Count;

            // insert a new row at this position
            gridNewSubtask.Rows.Insert(index);

            // set default values
            gridNewSubtask[index, 0] = new SourceGrid.Cells.CheckBox(null, false);
            gridNewSubtask[index, 1] = new SourceGrid.Cells.Cell("", typeof(string));
            gridNewSubtask[index, 2] = new SourceGrid.Cells.Cell("", typeof(string));
            gridNewSubtask[index, 3] = new SourceGrid.Cells.Cell("", assigneeEditor);
            gridNewSubtask[index, 3].View = SourceGrid.Cells.Views.ComboBox.Default;
            gridNewSubtask[index, 3].Value = "";
            gridNewSubtask[index, 4] = new SourceGrid.Cells.Cell(0, typeof(double));

            gridNewSubtask.AutoSizeCells();

            if (btnCreateSubtasks.Enabled == false)
                btnCreateSubtasks.Enabled = true;
        }

        /*
         *  Done button handler
         */
        private void btnDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /*
         *  Cancel button handler
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // cancel subtask creation if in progress
            if (subtaskCreateWorker != null && subtaskCreateWorker.IsBusy)
                subtaskCreateWorker.CancelAsync();
            DialogResult = DialogResult.Cancel;
        }

        /*
         *  This method counts how many subtasks are marked "add" 
         */
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

        /*
         *  This method converts the subtask grid to a subtask list
         */
        private void SaveSubtaskToList()
        {
            subtaskList = new List<SubtaskItem>();
            SubtaskItem subtaskItem;

            //  check to see if new subtasks has any subtasks to save
            if (gridNewSubtask.Rows.Count > 1)
            {
                //  convert each grid row to an item and add it to the list
                for (int i = 1; i < gridNewSubtask.Rows.Count; i++)
                {
                    subtaskItem = new SubtaskItem();

                    SourceGrid.Cells.CheckBox chkBox = new SourceGrid.Cells.CheckBox(null, false);
                    chkBox = (SourceGrid.Cells.CheckBox)gridNewSubtask[i, 0];

                    subtaskItem.AddSubtask = chkBox.Checked;
                    subtaskItem.Summary = (string)gridNewSubtask[i, 1].Value;
                    subtaskItem.Description = (string)gridNewSubtask[i, 2].Value;
                    subtaskItem.Assignee = (string)gridNewSubtask[i, 3].Value;
                    subtaskItem.Estimate = (double)gridNewSubtask[i, 4].Value;

                    subtaskList.Add(subtaskItem);
                }
            }
        }

        /*
         *  Handler for template button
         *      Converts the subtask list to XML and saves it into the settings.
         */
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            // build subtask list
            SaveSubtaskToList();

            // generate XML serializer for subtask list
            StringWriter stringWriter = new StringWriter();
            XmlSerializer subtaskListSerializer = new XmlSerializer(subtaskList.GetType());

            // serialize the subtask list using the string writer
            subtaskListSerializer.Serialize(stringWriter, subtaskList);

            // save the XML string to the settings
            Properties.Settings.Default.Template = stringWriter.ToString();
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        /*
         *  Background worker progress change handler to display progress for subtask creation in Jira
         */
        private void subtaskCreateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            lblStatus.Text = e.ProgressPercentage.ToString() + " %";
        }

        /*
         *  Background worker run completion handler to handle errors, cancellation and success.
         */
        private void subtaskCreateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  if ended in error, show message and set status
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                lblStatus.Text = "ERROR!";
            }

            // if cancelled, set status
            else if(e.Cancelled)
            {
                lblStatus.Text = "Cancelled!";
            }

            // if success, set status
            else
            {
                lblStatus.Text = "Success!";
            }

            // enable buttons which were disabled when subtask creation began.
            btnDone.Enabled = true;
            btnTemplate.Enabled = true;
            btnAddRow.Enabled = true;
        }

        /*
         *  Background worker work handler which creates each subtask in Jira.
         */
        private async void subtaskCreateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (subtasksCreated < numSubtasksToCreate)
            {
                System.Threading.Thread.Sleep(500);
            }
        }

        private async void CreateSubtasks(IProgress<int> progress, int numberSubtasksToCreate)
        {
            subtasksCreated = 0;
            numSubtasksToCreate = numberSubtasksToCreate;

            foreach (SubtaskItem item in subtaskList)
            {
                if (item.AddSubtask.HasValue && item.AddSubtask == true)
                {
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
                    subtasksCreated++;

                    int percent = (int)((subtasksCreated / (double)numberSubtasksToCreate) * 100.0);
                    progress.Report(percent);
                    System.Threading.Thread.Sleep(500);

                }
            }


        }

        /*
         *  Create subtask button handler.  This creates the background worker thread.
         */
        private void btnCreateSubtasks_Click(object sender, EventArgs e)
        {

            var progress = new Progress<int>(percent =>
            {
                if (percent == 100)
                    bCreateSubtaskDone = true;
                progressBar1.Value = percent;
                progressBar1.Update();
                lblStatus.Text = percent.ToString() + "%";
            });

            // save list of rows to settings as default
            // save list of rows to local variable to be accessible from main page

            SaveSubtaskToList();
            btnAddRow.Enabled = false;
            btnTemplate.Enabled = false;
            btnDone.Enabled = false;

            bCreateSubtaskDone = false;

            subtasksCreated = 0;
            numberSubtasks = SubtasksAddCount();


            if (numberSubtasks > 0)
            {
                Task task = new Task(() => CreateSubtasks(progress, numberSubtasks));

                task.Start();
                task.Wait();

                // enable buttons which were disabled when subtask creation began.
                btnDone.Enabled = true;
                btnTemplate.Enabled = true;
                btnAddRow.Enabled = true;

            }


        }

        /*
         *  This mehod takes a list of users and stores it. 
         */
        internal void SetUserCombo(List<string> userList)
        {
            foreach (var user in userList)
            {
                usernameList.Add(user);
            }

        }
    }
}



