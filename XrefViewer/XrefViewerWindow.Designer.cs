
namespace XrefViewer
{
    partial class XrefViewerWindow
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
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.ConsoleTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // InputTextBox
            // 
            this.InputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputTextBox.Font = new System.Drawing.Font("Consolas", 12F);
            this.InputTextBox.ForeColor = System.Drawing.Color.White;
            this.InputTextBox.Location = new System.Drawing.Point(12, 523);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(960, 26);
            this.InputTextBox.TabIndex = 0;
            this.InputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputTextBox_KeyPress);
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ConsoleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConsoleTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleTextBox.ForeColor = System.Drawing.Color.White;
            this.ConsoleTextBox.HideSelection = false;
            this.ConsoleTextBox.Location = new System.Drawing.Point(12, 12);
            this.ConsoleTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ReadOnly = true;
            this.ConsoleTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.ConsoleTextBox.Size = new System.Drawing.Size(960, 505);
            this.ConsoleTextBox.TabIndex = 0;
            this.ConsoleTextBox.Text = "";
            // 
            // XrefViewerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.ConsoleTextBox);
            this.Controls.Add(this.InputTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "XrefViewerWindow";
            this.Text = "XrefViewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.RichTextBox ConsoleTextBox;
    }
}