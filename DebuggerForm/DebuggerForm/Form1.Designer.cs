namespace DebuggerForm
{
    partial class Form1
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
            this.attachToProcess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // attachToProcess
            // 
            this.attachToProcess.Location = new System.Drawing.Point(26, 43);
            this.attachToProcess.Name = "attachToProcess";
            this.attachToProcess.Size = new System.Drawing.Size(103, 37);
            this.attachToProcess.TabIndex = 0;
            this.attachToProcess.Text = "Attach to process";
            this.attachToProcess.UseVisualStyleBackColor = true;
            this.attachToProcess.Click += new System.EventHandler(this.attachToProcess_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 144);
            this.Controls.Add(this.attachToProcess);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button attachToProcess;
    }
}

