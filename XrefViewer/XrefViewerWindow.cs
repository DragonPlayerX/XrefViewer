﻿using System;
using System.Drawing;
using System.Windows.Forms;

using XrefViewer.Core;

namespace XrefViewer
{
    public partial class XrefViewerWindow : Form
    {
        public string ConsoleText => ConsoleTextBox.Text;

        public XrefViewerWindow()
        {
            InitializeComponent();
        }

        public void WriteLine(Color color, string message)
        {
            ConsoleTextBox.SuspendLayout();
            ConsoleTextBox.SelectionStart = ConsoleTextBox.TextLength;
            ConsoleTextBox.SelectionLength = 0;
            ConsoleTextBox.SelectionColor = color;
            ConsoleTextBox.AppendText(message + Environment.NewLine);
            ConsoleTextBox.SelectionColor = ConsoleTextBox.ForeColor;
            ConsoleTextBox.ScrollToCaret();
            ConsoleTextBox.ResumeLayout();
        }

        public void ClearConsole()
        {
            ConsoleTextBox.Clear();
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                string command = InputTextBox.Text;
                InputTextBox.Text = "";
                CommandHandler.ParseAndExecute(command);
            }
        }
    }
}
