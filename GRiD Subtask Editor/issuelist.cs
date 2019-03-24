using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Atlassian.Jira;
using System.Text.RegularExpressions;

namespace GRiD_Subtask_Editor
{
    public partial class issueList : Form
    {
        private Jira jira;
        UserConfig userConfig;

        private CellBackColorAlternate viewNormal;
        private List<string> userList = new List<string>();
        private List<Issue> myIssues = new List<Issue>();

        private bool bForceUserUpdate;
        private Issue parentIssue = null;

        private IOrderedQueryable<Issue> issuesFromJira = null;

        private BackgroundWorker waitWorker;
        private WaitDialog waitDialog;
        const int ISSUE_LOAD_START = 0;
        const int USER_LOAD = 10;
        const int ISSUE_LOADING = 50;
        const int ISSUE_LOAD_COMPLETE = 100;

        /*
         *  List of usernames extracted from Jira to be used when assigning
         *  subtasks.
         */
        public List<string> UserList
        {
            get { return userList; }
        }

        /*
         *  -------------------------------------------------------------------------------
         *  Initialize the dialog
         *  -------------------------------------------------------------------------------
         */
        public issueList()
        {
            InitializeComponent();
        }

        /*
         *  -------------------------------------------------------------------------------
         *  dialog load event handler
         *  
         *  This routine is called when the dialog is loaded.  It does:
         *      loads data from the user settings
         *      configures the dialog colors
         *      Configures and initializes the issue grid
         *      Configures and initializes the subtask grid
         *  -------------------------------------------------------------------------------
         */
        private void issueList_Load(object sender, EventArgs e)
        {

            //  disable connect button at start
            btnConnect.Enabled = false;


            //  if we have a server URL saved, enable the connect button
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ServerURL))
            {
                btnConnect.Enabled = true;
            }

            //  if we've previously saved the list of usernames from Jira, get them now.
            if (Properties.Settings.Default.JiraUsernames != null && 
                Properties.Settings.Default.JiraUsernames.Count > 0)
            {
                userList = Properties.Settings.Default.JiraUsernames;
            }

            //  Border
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);

            //  Views -- at some point make this configurable
            viewNormal = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);
            viewNormal.Border = cellBorder;

            //  ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            backHeader.BackColor = Color.Maroon;
            backHeader.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.White;
            viewColumnHeader.Font = new Font("Comic Sans MS", 10, FontStyle.Underline);

            //  Create the issue grid
            issueGrid.BorderStyle = BorderStyle.FixedSingle;
            issueGrid.ColumnsCount = 8;
            issueGrid.FixedRows = 1;
            issueGrid.Rows.Insert(0);
            issueGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            issueGrid.AutoStretchColumnsToFitWidth = true;

            // create the subtask grid
            subtaskGrid.BorderStyle = BorderStyle.FixedSingle;
            subtaskGrid.ColumnsCount = 5;
            subtaskGrid.FixedRows = 1;
            subtaskGrid.Rows.Insert(0);
            subtaskGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            subtaskGrid.AutoStretchColumnsToFitWidth = true;

            // initialize issue grid header row
            SourceGrid.Cells.ColumnHeader columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("KEY");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 0] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("TYPE");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 1] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("POINTS");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 2] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("SUMMARY");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 3] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("PRIORITY");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 4] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("STATUS");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 5] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("ASSIGNEE");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 6] = columnHeader;

            columnHeader = new SourceGrid.Cells.ColumnHeader("CREATED");
            columnHeader.View = viewColumnHeader;
            issueGrid[0, 7] = columnHeader;

            // autosize the cells to fit contents and show horizontal scrollbar
            issueGrid.AutoSizeCells();
            issueGrid.HScrollBar.Visible = true;

            // create the subtask grid header row
            columnHeader = new SourceGrid.Cells.ColumnHeader("KEY");
            columnHeader.View = viewColumnHeader;
            subtaskGrid[0, 0] = columnHeader;
            columnHeader = new SourceGrid.Cells.ColumnHeader("SUMMARY");
            columnHeader.View = viewColumnHeader;
            subtaskGrid[0, 1] = columnHeader;
            columnHeader = new SourceGrid.Cells.ColumnHeader("STATUS");
            columnHeader.View = viewColumnHeader;
            subtaskGrid[0, 2] = columnHeader;
            columnHeader = new SourceGrid.Cells.ColumnHeader("ASSIGNEE");
            columnHeader.View = viewColumnHeader;
            subtaskGrid[0, 3] = columnHeader;
            columnHeader = new SourceGrid.Cells.ColumnHeader("CREATED");
            columnHeader.View = viewColumnHeader;
            subtaskGrid[0, 4] = columnHeader;

            // autosize the cells to fit contents
            subtaskGrid.AutoSizeCells();
            subtaskGrid.HScrollBar.Visible = true;

        }

        /*
         * This routine sets up the row view to alternate between two colors.
         */
        private class CellBackColorAlternate : SourceGrid.Cells.Views.Cell
        {
            public CellBackColorAlternate(Color firstColor, Color secondColor)
            {
                FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
                SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
            }

            private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
            public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
            {
                get { return mFirstBackground; }
                set { mFirstBackground = value; }
            }

            private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
            public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
            {
                get { return mSecondBackground; }
                set { mSecondBackground = value; }
            }

            protected override void PrepareView(SourceGrid.CellContext context)
            {
                base.PrepareView(context);

                if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                    Background = FirstBackground;
                else
                    Background = SecondBackground;
            }
        }

        /*
         *  This displays the configuration dialog.  Presently, this only configures the server name. 
         *  
         *  TO-DO: Expand to include 
         *      Color configuration
         *      Connection debugging enable/disable
         */
        private void pbConfigure_Click(object sender, EventArgs e)
        {
            ServerConfig sc = new ServerConfig();
            sc.ShowDialog();
            //  if we have a server URL saved, enable the connect button
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ServerURL))
            {
                btnConnect.Enabled = true;
            }

        }

        /*
         *  This routine handles the Connect button click event.  It shows the user configuration dialog to 
         *  enter your username and password for the Jira server.
         *  
         *  If username/password successfully entered, then attempt to connect to Jira, download user list if needed or
         *  requested, download specified user's issues, and then display the issues in the issue grid.  
         */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // create and display username dialog
            userConfig = new UserConfig();
            userConfig.ShowDialog();

            // if dialog accepted attempt to connect to Jira and then download the specified users issues.
            if (userConfig.DialogResult == DialogResult.OK)
            {

                // Rest client settings for debugging
                var settings = new JiraRestClientSettings()
                {
                    // set this to true if you desire to enable debugging.  
                    EnableRequestTrace = false
                };

                try
                {
                    //  attempt to connect to Jira via REST interface
                    jira = Jira.CreateRestClient(Properties.Settings.Default.ServerURL, userConfig.Username, userConfig.Password, settings);

                    // create a dialog to let user know something is going on
                    waitDialog = new WaitDialog();
                    
                    // create a background worker thread to load the data from the Jira server
                    waitWorker = new BackgroundWorker();
                    waitWorker.WorkerReportsProgress = true;

                    // setup background worker methods
                    waitWorker.DoWork += new DoWorkEventHandler(waitWorker_DoWork);
                    waitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(waitWorker_RunWorkerCompleted);
                    waitWorker.ProgressChanged += new ProgressChangedEventHandler(waitWorker_ProgressChanged);

                    // start the background worker
                    waitWorker.RunWorkerAsync();

                    //  show the wait dialog.
                    waitDialog.ShowDialog();

                    //  if we didn't receive any issues from Jira, then don't display anything to the user
                    if (issuesFromJira != null)
                    {
                        int index = 1;

                        // loop through each issue from Jira
                        foreach (Issue issue in issuesFromJira)
                        {
                            // add this to my list of issues
                            myIssues.Add(issue);

                            // add the issue to the issue grid
                            issueGrid.Rows.Insert(index);
                            issueGrid[index, 0] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Key.ToString()) ? issue.Key.ToString() : ""), typeof(string));
                            issueGrid[index, 0].View = viewNormal;
                            issueGrid[index, 0].Editor = null;
                            issueGrid[index, 1] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Type.ToString()) ? issue.Type.ToString() : ""), typeof(string));
                            issueGrid[index, 1].View = viewNormal;
                            issueGrid[index, 1].Editor = null;
                            string storyPointsString = "";
                            if (issue["Story Points"] != null)
                            {
                                storyPointsString = issue["Story Points"].ToString();
                            }
                            issueGrid[index, 2] = new SourceGrid.Cells.Cell(storyPointsString);
                            issueGrid[index, 2].View = viewNormal;
                            issueGrid[index, 2].Editor = null;
                            issueGrid[index, 3] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Summary) ? issue.Summary.ToString() : ""), typeof(string));
                            issueGrid[index, 3].View = viewNormal;
                            issueGrid[index, 3].Editor = null;
                            issueGrid[index, 4] = new SourceGrid.Cells.Cell(issue.Priority);
                            issueGrid[index, 4].View = viewNormal;
                            issueGrid[index, 4].Editor = null;
                            issueGrid[index, 5] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Status.ToString()) ? issue.Status.ToString() : ""), typeof(string));
                            issueGrid[index, 5].View = viewNormal;
                            issueGrid[index, 5].Editor = null;
                            issueGrid[index, 6] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Assignee) ? issue.Assignee.ToString() : ""), typeof(string));
                            issueGrid[index, 6].View = viewNormal;
                            issueGrid[index, 6].Editor = null;
                            issueGrid[index, 7] = new SourceGrid.Cells.Cell(issue.Created, typeof(DateTime));
                            issueGrid[index, 7].View = viewNormal;
                            issueGrid[index, 7].Editor = null;

                            index++;

                            // resize the cells to fit the contents
                            issueGrid.AutoSizeCells();
                            issueGrid.Update();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // something went wrong communicating with the server, show the error
                    MessageBox.Show("Unable to communicate with server '" + 
                        Properties.Settings.Default.ServerURL + "'./nException: '"+ex.Message+"'");
                }
            }
        }

        /*
         * This routine loads the subtasks from Jira when the corresponding issue is clicked.  
         */
        private void issueGrid_Click(object sender, EventArgs e)
        {
            //  check to see if the selected row is valid
            if (issueGrid.Selection.ActivePosition.Row >= 0 &&
                issueGrid.Selection.ActivePosition.Row < issueGrid.Rows.Count)
            {

                // get the issue Key from the selected row
                string Key = issueGrid[issueGrid.Selection.ActivePosition.Row, 0].ToString();
                parentIssue = null;

                // find the issue which matches the key and assign it as the parent
                foreach (Issue issue in myIssues)
                {
                    if (issue.Key == Key)
                    {
                        parentIssue = issue;
                        break;
                    }
                }


                // if we have a valid parent, get the subtasks.
                if (parentIssue != null)
                {
                    tbETPLItem.Text = parentIssue.Key.ToString();
                    tbDescription.Text = parentIssue.Summary.ToString();
                    ShowSubTasks();
                }
            }
        }

        private void ShowSubTasks()
        {
            //  if there are subtasks listed from another task, clear them
            if (subtaskGrid.Rows.Count > 1)
            {
                subtaskGrid.Redim(1, 5);
            }

            var subtasks = parentIssue.GetSubTasksAsync().Result;
            try
            {
                // if we received subtasks, place them in the subtask grid
                if (subtasks.Count() > 0)
                {
                    int index = 1;
                    foreach (Issue subtask in subtasks)
                    {
                        subtaskGrid.Rows.Insert(index);
                        subtaskGrid[index, 0] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(subtask.Key.ToString()) ? subtask.Key.ToString() : ""), typeof(string));
                        subtaskGrid[index, 0].View = viewNormal;
                        subtaskGrid[index, 0].Editor = null;
                        subtaskGrid[index, 1] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(subtask.Summary) ? subtask.Summary.ToString() : ""), typeof(string));
                        subtaskGrid[index, 1].View = viewNormal;
                        subtaskGrid[index, 1].Editor = null;
                        subtaskGrid[index, 2] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(subtask.Status.ToString()) ? subtask.Status.ToString() : ""), typeof(string));
                        subtaskGrid[index, 2].View = viewNormal;
                        subtaskGrid[index, 2].Editor = null;
                        subtaskGrid[index, 3] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(subtask.Assignee) ? subtask.Assignee.ToString() : ""), typeof(string));
                        subtaskGrid[index, 3].View = viewNormal;
                        subtaskGrid[index, 3].Editor = null;
                        subtaskGrid[index, 4] = new SourceGrid.Cells.Cell(subtask.Created, typeof(DateTime));
                        subtaskGrid[index, 4].View = viewNormal;
                        subtaskGrid[index, 4].Editor = null;

                        index++;
                    }
                    subtaskGrid.AutoSizeCells();
                }
            }
            catch
            {
                MessageBox.Show("No subtasks!");
            }

        }

        /*
         *  This routine handles the Add Subtask button click
         */
        private void btnAddSubtask_Click(object sender, EventArgs e)
        {
            // check to see if a parent was selected
            if (parentIssue == null)
            {
                MessageBox.Show("Please select an issue!");
                return;
            }
            else
            {
                // create the subtask dialog.
                AddSubtask addSubtask = new AddSubtask();

                // initialize the necessary data
                addSubtask.Jira = jira;
                addSubtask.ParentIssue = parentIssue;
                addSubtask.SetUserCombo(UserList);

                // display the subtask dialog
                addSubtask.ShowDialog();
            }
        }

        /*
         *  This method handles the checkbox for updating users.
         */
        private void cbUpdateUsers_CheckedChanged(object sender, EventArgs e)
        {
            //  if the box is checked, force an update on the users when we connect next time
            if (cbUpdateUsers.Checked)
                bForceUserUpdate = true;
            else
                bForceUserUpdate = false;
        }

        /*  
         *  -----------------------------------------------------
         *  Background worker routines
         *  -----------------------------------------------------
         */

        /*
         * This routine will report progress to the dialog during the loading of users and issues.
         */
        private void waitWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case USER_LOAD:
                    waitDialog.ProgressText = "Please wait, loading users...";
                    break;

                case ISSUE_LOADING:
                    waitDialog.ProgressText = "Please wait, loading issues...";
                    break;

                case ISSUE_LOAD_COMPLETE:
                    waitDialog.ProgressText = "Loading complete.";
                    break;

                case ISSUE_LOAD_START:
                default:
                    waitDialog.ProgressText = "Please wait...";
                    break;
            }
        }

        /*
         *  This routine is called when the dowork method completes
         */
        private void waitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            waitDialog.Close();
        }

        /*
         *  This routine is where the work is done by the background worker.  
         *      * updates the status
         *      * loads the users from Jira if requested or needed
         *      * loads the issues from Jira
         */
        private void waitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            waitWorker.ReportProgress(ISSUE_LOAD_START);

            //  if the user list was not stored in the settings
            //  or if an update is requested, load the users from
            //  Jira
            if (UserList.Count == 0 || bForceUserUpdate == true)
            {
                waitWorker.ReportProgress(USER_LOAD);
                BuildUserLIst();
            }

            waitWorker.ReportProgress(ISSUE_LOADING);

            // Allow a maximum issues to be located.
            // TODO:  Make this a configurable item
            jira.MaxIssuesPerRequest = 100;

            //  load the issues for the specified user from Jira.
            //  TODO:   Make project configurable
            //  TODO:   Make issue type selection configurable
            issuesFromJira = from issue in jira.Issues.Queryable
                             where (issue.Assignee == userConfig.Username) &&
                             (issue.Project == Properties.Settings.Default.ProjectName) &&
                             (issue.Type == "Defect" || issue.Type == "Story" || issue.Type == "Task") &&
                             (issue.Status != "Done" && issue.Status != "Complete")
                             orderby issue.Created
                             select issue;

            // slight delay before exiting.
            waitWorker.ReportProgress(ISSUE_LOAD_COMPLETE);
            System.Threading.Thread.Sleep(500);
        }

        /*
         *  -----------------------------------------------------
         *  Local helper methods
         *  -----------------------------------------------------
         */

        /*
         *  Gather all users in Jira from a-z.  This will get a list of users.  Add all active users 
         *  to the userlist.
         */
        private void BuildUserLIst()
        {
            // starting with 'a', get all usernames starting with a, then b, then c, and so on.
            for (var c = 'a'; c <= 'z'; c++)
            {
                try
                {
                    var users = jira.Users.SearchUsersAsync(c.ToString(), JiraUserStatus.Active, 1000).Result;

                    foreach (JiraUser user in users)
                    {
                        // only add the user to the userlist if the user is marked active.
                        if (user.IsActive)
                            UserList.Add(user.Username);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            // save the active user list to settings.
            Properties.Settings.Default.JiraUsernames = UserList;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void btnUpdateTaskList_Click(object sender, EventArgs e)
        {
            bool isBadIssueId = true;

            //  if we have a server URL saved, enable the connect button
            if (!String.IsNullOrWhiteSpace(tbETPLItem.Text))
            {
                // see if the string is all numbers, if so conver
                if (Regex.IsMatch(tbETPLItem.Text.ToString(), @"^\d+$"))
                {
                    // convert entry to ETPL-XXXX
                    tbETPLItem.Text = Properties.Settings.Default.ProjectPrefix + "_" + tbETPLItem.Text;
                }

                parentIssue = getIssue(tbETPLItem.Text);
                if (parentIssue != null)
                {
                    tbDescription.Text = parentIssue.Summary.ToString();
                    ShowSubTasks();
                    isBadIssueId = false;
                }
            }

            if (isBadIssueId)
            {
                MessageBox.Show("Please enter a valid issue id!");
            }
        }

        private void tbETPLItem_TextChanged(object sender, EventArgs e)
        {
            btnUpdateTaskList.Enabled = true;
        }

        private Issue getIssue(string key)
        {
            IOrderedQueryable<Issue> issues = null;
            Issue foundIssue = null;

            // Allow a maximum issues to be located.
            // TODO:  Make this a configurable item
            jira.MaxIssuesPerRequest = 100;

            try
            {
                //  load the issues for the specified user from Jira.
                //  TODO:   Make project configurable
                //  TODO:   Make issue type selection configurable
                issues = from issue in jira.Issues.Queryable
                         where issue.Key == key
                         orderby issue.Created
                         select issue;

                if (issues != null && issues.Count() > 0)
                {
                    foundIssue = issues.ElementAt(0);
                }
            }
            catch
            {
                foundIssue = null;
            }

            return foundIssue;
        }


    }
}
