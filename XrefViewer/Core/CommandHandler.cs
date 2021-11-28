using System;
using System.Collections.Generic;
using System.Drawing;

namespace XrefViewer.Core
{
    public static class CommandHandler
    {
        private static Dictionary<string, Action<ArgumentData[]>> commands = new Dictionary<string, Action<ArgumentData[]>>();

        public static void RegisterCommand(string name, Action<ArgumentData[]> handler) => commands.Add(name, handler);

        public static void ParseAndExecute(string input)
        {
            try
            {
                XrefViewerMod.Instance.ViewerWindow.WriteLine(Color.White, ">> " + input);
                string[] parts = input.Split(' ');

                string commandName = parts[0];

                List<ArgumentData> arguments = new List<ArgumentData>();

                for (int i = 1; i < parts.Length; i++)
                {
                    string s1 = parts[i];

                    if (s1.StartsWith("-"))
                    {
                        if (parts.Length > i + 1)
                        {
                            string s2 = parts[i + 1];
                            if (s2.StartsWith("-"))
                            {
                                arguments.Add(new ArgumentData() { Identifier = s1, Content = null });
                            }
                            else
                            {
                                arguments.Add(new ArgumentData() { Identifier = s1, Content = parts[i + 1] });
                                i++;
                            }
                        }
                        else
                        {
                            arguments.Add(new ArgumentData() { Identifier = s1, Content = null });
                        }
                    }
                }

                if (commands.TryGetValue(commandName, out Action<ArgumentData[]> handler))
                    handler.Invoke(arguments.ToArray());
                else
                    XrefViewerMod.Instance.ViewerWindow.WriteLine(Color.Yellow, "Unknown command");

            }
            catch (Exception e)
            {
                XrefViewerMod.Instance.ViewerWindow.WriteLine(Color.Red, "An error occurred: " + e.ToString());
            }
        }

        public struct ArgumentData
        {
            public string Identifier;
            public string Content;
        }
    }
}
