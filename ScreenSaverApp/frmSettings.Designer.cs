namespace ScreenSaverApp
{
    partial class frmSettings
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTextToDisplay1 = new System.Windows.Forms.TextBox();
            this.txtTextToDisplay2 = new System.Windows.Forms.TextBox();
            this.txtTextToDisplay3 = new System.Windows.Forms.TextBox();
            this.txtTextToDisplay4 = new System.Windows.Forms.TextBox();
            this.txtTextToDisplay5 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(153, 194);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 29);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnOk.Location = new System.Drawing.Point(38, 194);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(102, 29);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(39, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Text to display:";
            // 
            // txtTextToDisplay
            // 
            this.txtTextToDisplay1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTextToDisplay1.Location = new System.Drawing.Point(38, 91);
            this.txtTextToDisplay1.Name = "txtTextToDisplay1";
            this.txtTextToDisplay1.Size = new System.Drawing.Size(217, 23);
            this.txtTextToDisplay1.TabIndex = 6;
            this.txtTextToDisplay1.Text = "People First";
            // 
            // txtTextToDisplay
            // 
            this.txtTextToDisplay2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTextToDisplay2.Location = new System.Drawing.Point(38, 111);
            this.txtTextToDisplay2.Name = "txtTextToDisplay2";
            this.txtTextToDisplay2.Size = new System.Drawing.Size(217, 23);
            this.txtTextToDisplay2.TabIndex = 6;
            this.txtTextToDisplay1.Text = "Stronger Together";
            // 
            // txtTextToDisplay
            // 
            this.txtTextToDisplay3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTextToDisplay3.Location = new System.Drawing.Point(38, 131);
            this.txtTextToDisplay3.Name = "txtTextToDisplay3";
            this.txtTextToDisplay3.Size = new System.Drawing.Size(217, 23);
            this.txtTextToDisplay3.TabIndex = 6;
            this.txtTextToDisplay1.Text = "Do what\'s right, not what\'s easy";
            // 
            // txtTextToDisplay
            // 
            this.txtTextToDisplay4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTextToDisplay4.Location = new System.Drawing.Point(38, 151);
            this.txtTextToDisplay4.Name = "txtTextToDisplay4";
            this.txtTextToDisplay4.Size = new System.Drawing.Size(217, 23);
            this.txtTextToDisplay4.TabIndex = 6;
            this.txtTextToDisplay1.Text = "Be Authentic";
            // 
            // txtTextToDisplay
            // 
            this.txtTextToDisplay5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTextToDisplay5.Location = new System.Drawing.Point(38, 171);
            this.txtTextToDisplay5.Name = "txtTextToDisplay5";
            this.txtTextToDisplay5.Size = new System.Drawing.Size(217, 23);
            this.txtTextToDisplay5.TabIndex = 6;
            this.txtTextToDisplay1.Text = "Always Deliver";
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTextToDisplay1);
            this.Controls.Add(this.txtTextToDisplay2);
            this.Controls.Add(this.txtTextToDisplay3);
            this.Controls.Add(this.txtTextToDisplay4);
            this.Controls.Add(this.txtTextToDisplay5);
            this.Name = "frmSettings";
            this.Text = "Screen Saver Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTextToDisplay1;
        private System.Windows.Forms.TextBox txtTextToDisplay2;
        private System.Windows.Forms.TextBox txtTextToDisplay3;
        private System.Windows.Forms.TextBox txtTextToDisplay4;
        private System.Windows.Forms.TextBox txtTextToDisplay5;
    }
}

