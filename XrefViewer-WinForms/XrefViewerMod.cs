using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MelonLoader;

using XrefViewer;
using XrefViewer.Core;

[assembly: MelonInfo(typeof(XrefViewerMod), "XrefViewer", "1.1.0", "DragonPlayer", "https://github.com/DragonPlayerX/XrefViewer")]
[assembly: MelonGame]

namespace XrefViewer
{
    public class XrefViewerMod : MelonMod
    {
        public static readonly string Version = "1.1.0";

        public static XrefViewerMod Instance { get; private set; }

        public XrefViewerWindow ViewerWindow { get; private set; }

        public override void OnApplicationStart()
        {
            Instance = this;
            MelonLogger.Msg("Initializing XrefViewer " + Version + "...");
            CommandHandler.RegisterCommand("help", new Action<CommandHandler.ArgumentData[]>(args =>
            {
                ViewerWindow.WriteLine(Color.White, "Use command 'xref <args>' to scan a method or a whole type.");
                ViewerWindow.WriteLine(Color.White, "Exact usage: xref [-t typename] [-m methodname] [-s] [-c] [-l]");
                ViewerWindow.WriteLine(Color.White, "    -t  Defines the type.");
                ViewerWindow.WriteLine(Color.White, "    -m  Defines the method from the type.");
                ViewerWindow.WriteLine(Color.White, "    -s  Print strings of the method or of all methods from the given type.");
                ViewerWindow.WriteLine(Color.White, "    -c  Method name will be used as part of name instead of exact name.");
                ViewerWindow.WriteLine(Color.White, "    -l  Allow large scan results.");
                ViewerWindow.WriteLine(Color.White, "");
                ViewerWindow.WriteLine(Color.White, "Use command 'clear' to clear the console.");
                ViewerWindow.WriteLine(Color.White, "Exact usage: clear");
                ViewerWindow.WriteLine(Color.White, "");
                ViewerWindow.WriteLine(Color.White, "Use command 'dump <args>' to write the current console output to a file.");
                ViewerWindow.WriteLine(Color.White, "Exact usage: dump [-f filepath]");
                ViewerWindow.WriteLine(Color.White, "    -f  Defines the destination file.");
            }));

            CommandHandler.RegisterCommand("clear", new Action<CommandHandler.ArgumentData[]>(args => ViewerWindow.ClearConsole()));

            CommandHandler.RegisterCommand("xref", new Action<CommandHandler.ArgumentData[]>(args =>
            {
                string typeName = null;
                string methodName = null;
                bool printStrings = false;
                bool exactName = true;
                bool largeScans = false;

                foreach (CommandHandler.ArgumentData arg in args)
                {
                    switch (arg.Identifier.ToLower())
                    {
                        case "-t":
                            typeName = arg.Content;
                            break;
                        case "-m":
                            methodName = arg.Content;
                            break;
                        case "-s":
                            printStrings = true;
                            break;
                        case "-c":
                            exactName = false;
                            break;
                        case "-l":
                            largeScans = true;
                            break;
                    }
                }

                XrefCore.Scan(typeName, methodName, printStrings, exactName, largeScans);
            }));

            CommandHandler.RegisterCommand("dump", new Action<CommandHandler.ArgumentData[]>(args =>
            {
                string filePath = null;

                foreach (CommandHandler.ArgumentData arg in args)
                {
                    switch (arg.Identifier.ToLower())
                    {
                        case "-f":
                            filePath = arg.Content;
                            break;
                    }
                }

                if (filePath == null)
                    filePath = "xref-dump_" + DateTime.Now.ToString("dd.MM.yyyy_HH_mm_ss.fff") + ".txt";

                File.WriteAllText(filePath, ViewerWindow.ConsoleText);
                ViewerWindow.WriteLine(Color.LimeGreen, "Successfully written console content to " + filePath);
            }));

            Application.EnableVisualStyles();

            Thread thread = new Thread(() =>
            {
                ViewerWindow = new XrefViewerWindow();
                CommandHandler.ParseAndExecute("help");
                Application.ThreadException += new ThreadExceptionEventHandler((sender, e) =>
                {
                    DialogResult result = MessageBox.Show("Restart the WinForms Application Thread?", "Fatal WinForms Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (result == DialogResult.Yes)
                    {
                        ViewerWindow.Dispose();
                        Application.ExitThread();
                        ViewerWindow = new XrefViewerWindow();
                        CommandHandler.ParseAndExecute("help");
                        Application.Run(ViewerWindow);
                    }
                    else
                    {
                        Application.Exit();
                    }
                });
                Application.Run(ViewerWindow);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            MelonLogger.Msg("Running version " + Version + " of XrefViewer.");
        }
    }
}
