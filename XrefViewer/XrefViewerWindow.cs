using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using XrefViewer.Core;

namespace XrefViewer
{
    public partial class XrefViewerWindow : Form
    {
        public static readonly int HistorySize = 256;

        public string ConsoleText => ConsoleTextBox.Text;

        private List<string> commandHistory = new List<string>() { "" };
        private int historyIndex;

        public XrefViewerWindow()
        {
            InitializeComponent();
        }

        public void WriteLine(Color color, string message) => Write(color, message + Environment.NewLine);

        public void Write(Color color, string message)
        {
            ConsoleTextBox.SuspendLayout();
            ConsoleTextBox.SelectionStart = ConsoleTextBox.TextLength;
            ConsoleTextBox.SelectionLength = 0;
            ConsoleTextBox.SelectionColor = color;
            ConsoleTextBox.AppendText(message);
            ConsoleTextBox.SelectionColor = ConsoleTextBox.ForeColor;
            ConsoleTextBox.ScrollToCaret();
            ConsoleTextBox.ResumeLayout();
        }

        public void ClearConsole() => ConsoleTextBox.Clear();

        private void ConsoleTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (ConsoleTextBox.SelectionColor == Color.Aqua || ConsoleTextBox.SelectionColor == Color.DarkGray)
            {
                ConsoleTextBox.SelectionLength = 0;

                while (ConsoleTextBox.SelectionColor == Color.Aqua || ConsoleTextBox.SelectionColor == Color.DarkGray)
                    ConsoleTextBox.SelectionStart--;

                int startIndex = ConsoleTextBox.SelectionStart;
                ConsoleTextBox.SelectionStart++;

                while (ConsoleTextBox.SelectionColor == Color.Aqua || ConsoleTextBox.SelectionColor == Color.DarkGray)
                    ConsoleTextBox.SelectionStart++;

                ConsoleTextBox.SelectionStart--;
                int endIndex = ConsoleTextBox.SelectionStart;

                ConsoleTextBox.SelectionStart = startIndex;
                ConsoleTextBox.SelectionLength = endIndex - startIndex;

                string[] data = ConsoleTextBox.Text.Substring(startIndex, endIndex - startIndex).Split('#');
                InputTextBox.Text = "xref -t " + data[0] + " -m " + data[1];
            }
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                historyIndex = 0;
                string command = InputTextBox.Text;
                InputTextBox.Text = "";

                commandHistory.Insert(1, command);
                if (commandHistory.Count > HistorySize + 1)
                    commandHistory.RemoveAt(commandHistory.Count - 1);

                CommandHandler.ParseAndExecute(command);
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (historyIndex < commandHistory.Count - 1)
                    historyIndex++;

                InputTextBox.Text = commandHistory[historyIndex];
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                InputTextBox.SelectionLength = 0;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (historyIndex > 0)
                    historyIndex--;

                InputTextBox.Text = commandHistory[historyIndex];
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                InputTextBox.SelectionLength = 0;
            }
        }
    }
}
