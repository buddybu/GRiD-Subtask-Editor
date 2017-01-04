using System.Windows.Forms;

namespace GRiD_Subtask_Editor
{
    partial class WaitDialog
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
            this.waitMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // waitMessage
            // 
            this.waitMessage.AutoSize = true;
            this.waitMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waitMessage.Location = new System.Drawing.Point(12, 9);
            this.waitMessage.Name = "waitMessage";
            this.waitMessage.Size = new System.Drawing.Size(339, 25);
            this.waitMessage.TabIndex = 0;
            this.waitMessage.Text = "Please wait while loading issues...";
            // 
            // WaitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 43);
            this.ControlBox = false;
            this.Controls.Add(this.waitMessage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitDialog";
            this.Text = "WaitDialog";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label waitMessage;

        public Label WaitMessage
        {
            get
            {
                return waitMessage;
            }

            set
            {
                waitMessage = value;
            }
        }
    }
}