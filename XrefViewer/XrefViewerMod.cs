using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MelonLoader;

using XrefViewer;
using XrefViewer.Core;

[assembly: MelonInfo(typeof(XrefViewerMod), "XrefViewer", "1.0.0", "DragonPlayer", "https://github.com/DragonPlayerX/XrefViewer")]
[assembly: MelonGame]

namespace XrefViewer
{
    public class XrefViewerMod : MelonMod
    {
        public static readonly string Version = "1.0.0";

        public static XrefViewerMod Instance { get; private set; }

        public XrefViewerWindow ViewerWindow { get; private set; }

        public override void OnApplicationStart()
        {
            Instance = this;
            MelonLogger.Msg("Initializing XrefViewer " + Version + "...");

            CommandHandler.RegisterCommand("help", new Action<CommandHandler.ArgumentData[]>(args =>
            {
                ViewerWindow.WriteLine(Color.White, "Use command 'xref <args>' to scan a method or a whole type.");
                ViewerWindow.WriteLine(Color.White, "Exact usage: xref [-t typename] [-m methodname] [-s]");
                ViewerWindow.WriteLine(Color.White, "    -t  Defines the type.");
                ViewerWindow.WriteLine(Color.White, "    -m  Defines the method from the type.");
                ViewerWindow.WriteLine(Color.White, "    -s  Print strings of the method or of all methods from the given type.");
                ViewerWindow.WriteLine(Color.White, "");
                ViewerWindow.WriteLine(Color.White, "Use command 'clear' to clear the console.");
                ViewerWindow.WriteLine(Color.White, "Exact usage: clear");
            }));

            CommandHandler.RegisterCommand("clear", new Action<CommandHandler.ArgumentData[]>(args => ViewerWindow.ClearConsole()));

            CommandHandler.RegisterCommand("xref", new Action<CommandHandler.ArgumentData[]>(args =>
            {
                string typeName = null;
                string methodName = null;
                bool printStrings = false;

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
                    }
                }

                XrefCore.Scan(typeName, methodName, printStrings);
            }));

            Application.EnableVisualStyles();

            Thread thread = new Thread(() =>
            {
                ViewerWindow = new XrefViewerWindow();
                ViewerWindow.Show();
                CommandHandler.ParseAndExecute("help");
                Application.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            MelonLogger.Msg("Running version " + Version + " of XrefViewer.");
        }
    }
}
