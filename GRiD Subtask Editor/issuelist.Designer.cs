namespace GRiD_Subtask_Editor
{
    partial class issueList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.issueGrid = new SourceGrid.Grid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.subtaskGrid = new SourceGrid.Grid();
            this.Subtasks = new System.Windows.Forms.Label();
            this.btnAddSubtask = new System.Windows.Forms.Button();
            this.cbUpdateUsers = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::GRiD_Subtask_Editor.Properties.Resources.serverConfig;
            this.pictureBox1.Location = new System.Drawing.Point(764, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 22);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(683, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 22);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.issueGrid);
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 243);
            this.panel1.TabIndex = 5;
            // 
            // issueGrid
            // 
            this.issueGrid.AcceptsInputChar = false;
            this.issueGrid.AutoSize = true;
            this.issueGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.issueGrid.AutoStretchColumnsToFitWidth = true;
            this.issueGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.issueGrid.EnableSort = true;
            this.issueGrid.Location = new System.Drawing.Point(0, 0);
            this.issueGrid.Name = "issueGrid";
            this.issueGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.issueGrid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.issueGrid.Size = new System.Drawing.Size(787, 243);
            this.issueGrid.TabIndex = 3;
            this.issueGrid.TabStop = true;
            this.issueGrid.ToolTipText = "";
            this.issueGrid.Click += new System.EventHandler(this.issueGrid_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.subtaskGrid);
            this.panel2.Location = new System.Drawing.Point(0, 291);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(787, 243);
            this.panel2.TabIndex = 6;
            // 
            // subtaskGrid
            // 
            this.subtaskGrid.AutoSize = true;
            this.subtaskGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.subtaskGrid.AutoStretchColumnsToFitWidth = true;
            this.subtaskGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtaskGrid.EnableSort = true;
            this.subtaskGrid.Location = new System.Drawing.Point(0, 0);
            this.subtaskGrid.Name = "subtaskGrid";
            this.subtaskGrid.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.subtaskGrid.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.subtaskGrid.Size = new System.Drawing.Size(787, 243);
            this.subtaskGrid.TabIndex = 3;
            this.subtaskGrid.TabStop = true;
            this.subtaskGrid.ToolTipText = "";
            // 
            // Subtasks
            // 
            this.Subtasks.AutoSize = true;
            this.Subtasks.Location = new System.Drawing.Point(23, 274);
            this.Subtasks.Name = "Subtasks";
            this.Subtasks.Size = new System.Drawing.Size(51, 13);
            this.Subtasks.TabIndex = 7;
            this.Subtasks.Text = "Subtasks";
            // 
            // btnAddSubtask
            // 
            this.btnAddSubtask.Location = new System.Drawing.Point(146, 269);
            this.btnAddSubtask.Name = "btnAddSubtask";
            this.btnAddSubtask.Size = new System.Drawing.Size(83, 23);
            this.btnAddSubtask.TabIndex = 8;
            this.btnAddSubtask.Text = "Add Subtask";
            this.btnAddSubtask.UseVisualStyleBackColor = true;
            this.btnAddSubtask.Click += new System.EventHandler(this.btnAddSubtask_Click);
            // 
            // cbUpdateUsers
            // 
            this.cbUpdateUsers.AutoSize = true;
            this.cbUpdateUsers.Location = new System.Drawing.Point(560, 5);
            this.cbUpdateUsers.Name = "cbUpdateUsers";
            this.cbUpdateUsers.Size = new System.Drawing.Size(117, 17);
            this.cbUpdateUsers.TabIndex = 9;
            this.cbUpdateUsers.Text = "Update JIRA Users";
            this.cbUpdateUsers.UseVisualStyleBackColor = true;
            this.cbUpdateUsers.CheckedChanged += new System.EventHandler(this.cbUpdateUsers_CheckedChanged);
            // 
            // issueList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(788, 534);
            this.Controls.Add(this.cbUpdateUsers);
            this.Controls.Add(this.btnAddSubtask);
            this.Controls.Add(this.Subtasks);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "issueList";
            this.Text = "JIRA Issues";
            this.Load += new System.EventHandler(this.issueList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panel1;
        private SourceGrid.Grid issueGrid;
        private System.Windows.Forms.Panel panel2;
        private SourceGrid.Grid subtaskGrid;
        private System.Windows.Forms.Label Subtasks;
        private System.Windows.Forms.Button btnAddSubtask;
        private System.Windows.Forms.CheckBox cbUpdateUsers;
    }
}

