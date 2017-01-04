namespace GRiD_Subtask_Editor
{
    partial class AddSubtask
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridNewSubtask = new SourceGrid.Grid();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.subtaskCreateWorker = new System.ComponentModel.BackgroundWorker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCreateSubtasks = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridNewSubtask);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 298);
            this.panel1.TabIndex = 1;
            // 
            // gridNewSubtask
            // 
            this.gridNewSubtask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNewSubtask.EnableSort = true;
            this.gridNewSubtask.Location = new System.Drawing.Point(0, 0);
            this.gridNewSubtask.Name = "gridNewSubtask";
            this.gridNewSubtask.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.gridNewSubtask.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.gridNewSubtask.Size = new System.Drawing.Size(702, 298);
            this.gridNewSubtask.TabIndex = 0;
            this.gridNewSubtask.TabStop = true;
            this.gridNewSubtask.ToolTipText = "";
            // 
            // btnAddRow
            // 
            this.btnAddRow.Location = new System.Drawing.Point(12, 342);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(75, 23);
            this.btnAddRow.TabIndex = 2;
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.UseVisualStyleBackColor = true;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(615, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(534, 342);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 4;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(116, 342);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(105, 23);
            this.btnTemplate.TabIndex = 5;
            this.btnTemplate.Text = "Save As Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(443, 317);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(152, 21);
            this.progressBar1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(378, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Add Status";
            // 
            // subtaskCreateWorker
            // 
            this.subtaskCreateWorker.WorkerReportsProgress = true;
            this.subtaskCreateWorker.WorkerSupportsCancellation = true;
            this.subtaskCreateWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.subtaskCreateWorker_DoWork);
            this.subtaskCreateWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.subtaskCreateWorker_ProgressChanged);
            this.subtaskCreateWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.subtaskCreateWorker_RunWorkerCompleted);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(625, 321);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 8;
            // 
            // btnCreateSubtasks
            // 
            this.btnCreateSubtasks.Location = new System.Drawing.Point(414, 342);
            this.btnCreateSubtasks.Name = "btnCreateSubtasks";
            this.btnCreateSubtasks.Size = new System.Drawing.Size(105, 23);
            this.btnCreateSubtasks.TabIndex = 9;
            this.btnCreateSubtasks.Text = "Create Subtasks";
            this.btnCreateSubtasks.UseVisualStyleBackColor = true;
            this.btnCreateSubtasks.Click += new System.EventHandler(this.btnCreateSubtasks_Click);
            // 
            // AddSubtask
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(702, 377);
            this.Controls.Add(this.btnCreateSubtasks);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddRow);
            this.Controls.Add(this.panel1);
            this.Name = "AddSubtask";
            this.Text = "AddSubtask";
            this.Load += new System.EventHandler(this.AddSubtask_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private SourceGrid.Grid gridNewSubtask;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker subtaskCreateWorker;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCreateSubtasks;
    }
}