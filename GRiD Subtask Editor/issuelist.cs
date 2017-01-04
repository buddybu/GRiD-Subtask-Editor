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
using Atlassian.Jira.Linq;
namespace GRiD_Subtask_Editor
{
    public partial class issueList : Form
    {
        private Jira jira;
        CellBackColorAlternate viewNormal;
        UserConfig userConfig;
        List<string> userList = new List<string>();

        List<Issue> myIssues = new List<Issue>();
        private bool bForceUserUpdate;
        private Issue parentIssue = null;

        public List<string> UserList
        {
            get
            {
                return userList;
            }
        }

        public issueList()
        {
            InitializeComponent();
        }

        private void issueList_Load(object sender, EventArgs e)
        {
            while (String.IsNullOrWhiteSpace(Properties.Settings.Default.ServerURL))
            {
                ServerConfig sc = new ServerConfig();
                sc.ShowDialog();
            }

            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ServerURL))
            {
                btnConnect.Enabled = true;
            }

            if (Properties.Settings.Default.JiraUsernames != null && 
                Properties.Settings.Default.JiraUsernames.Count > 0)
            {
                userList = Properties.Settings.Default.JiraUsernames;
            }

            //Border
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);

            //Views

            viewNormal = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);
            viewNormal.Border = cellBorder;

            //ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            backHeader.BackColor = Color.Maroon;
            backHeader.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.White;
            viewColumnHeader.Font = new Font("Comic Sans MS", 10, FontStyle.Underline);

            //Create the grids
            issueGrid.BorderStyle = BorderStyle.FixedSingle;
            issueGrid.ColumnsCount = 8;
            issueGrid.FixedRows = 1;
            issueGrid.Rows.Insert(0);
            issueGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            issueGrid.AutoStretchColumnsToFitWidth = true;

            subtaskGrid.BorderStyle = BorderStyle.FixedSingle;
            subtaskGrid.ColumnsCount = 5;
            subtaskGrid.FixedRows = 1;
            subtaskGrid.Rows.Insert(0);
            subtaskGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            subtaskGrid.AutoStretchColumnsToFitWidth = true;

            // initialize header row
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

            issueGrid.AutoSizeCells();

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

            subtaskGrid.AutoSizeCells();

        }

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ServerConfig sc = new ServerConfig();
            sc.ShowDialog();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            userConfig = new UserConfig();
            userConfig.ShowDialog();

            

            if (userConfig.DialogResult == DialogResult.OK)
            {
                var settings = new JiraRestClientSettings()
                {
                    EnableRequestTrace = false
                };

                try
                {
                    jira = Jira.CreateRestClient(Properties.Settings.Default.ServerURL, userConfig.Username, userConfig.Password, settings);

                    if (UserList.Count == 0 || bForceUserUpdate == true)
                    {
                        BuildUserLIst();
                    }

                    jira.MaxIssuesPerRequest = 100;
                    var issues = from issue in jira.Issues.Queryable
                                 where issue.Assignee == "john.burnham" &&
                                 issue.Project == "e-terrapipeline" &&
                                 (issue.Type == "Defect" || issue.Type == "Story" || issue.Type == "Task") &&
                                 issue.Status != "Done"
                                 orderby issue.Created
                                 select issue;


                    int index = 1;
                    foreach (Issue issue in issues)
                    {
                        // add this to my list of issues
                        myIssues.Add(issue);

                        issueGrid.Rows.Insert(index);
                        issueGrid[index, 0] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Key.ToString()) ? issue.Key.ToString() : ""), typeof(string));
                        issueGrid[index, 0].View = viewNormal;
                        issueGrid[index, 0].Editor = null;
                        issueGrid[index, 1] = new SourceGrid.Cells.Cell((!String.IsNullOrEmpty(issue.Type.ToString()) ? issue.Type.ToString() : ""), typeof(string));
                        issueGrid[index, 1].View = viewNormal;
                        issueGrid[index, 1].Editor = null;
                        issueGrid[index, 2] = new SourceGrid.Cells.Cell(issue["Story Points"].ToString());
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
                    }
                    issueGrid.AutoSizeCells();
                }
                catch
                {
                    MessageBox.Show("Unable to communicate with server '" + 
                        Properties.Settings.Default.ServerURL + "'.");
                }
            }
        }

        private void BuildUserLIst()
        {
            for (var c = 'a'; c <= 'z'; c++)
            {
                try
                {
                    var users = jira.Users.SearchUsersAsync(c.ToString(), JiraUserStatus.Active, 1000).Result;

                    foreach (JiraUser user in users)
                    {
                        UserList.Add(user.Username);
                    }
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            Properties.Settings.Default.JiraUsernames = UserList;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void issueGrid_DoubleClick(object sender, EventArgs e)
        {

            if (subtaskGrid.Rows.Count > 1)
            {
                subtaskGrid.Redim(1, 5);
            }


            string Key = issueGrid[issueGrid.Selection.ActivePosition.Row, 0].ToString();
            parentIssue = null;

            foreach (Issue issue in myIssues)
            {
                if (issue.Key == Key)
                {
                    parentIssue = issue;
                    break;
                }
            }


            if (parentIssue != null)
            {

                var subtasks = parentIssue.GetSubTasksAsync().Result;

                foreach (var s in subtasks)
                {
                    Console.WriteLine(s.ToString());
                }

                try
                {
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
        }

        private void btnAddSubtask_Click(object sender, EventArgs e)
        {
            if (parentIssue == null)
            {
                MessageBox.Show("Please select an issue!");
                return;
            }
            else
            {
                // this will display the subtask dialog.
                AddSubtask addSubtask = new AddSubtask();
                addSubtask.Jira = jira;
                addSubtask.ParentIssue = parentIssue;

                addSubtask.SetUserCombo(UserList);
                addSubtask.ShowDialog();
            }
        }

        private void cbUpdateUsers_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUpdateUsers.Checked)
                bForceUserUpdate = true;
            else
                bForceUserUpdate = false;
        }
    }
}
