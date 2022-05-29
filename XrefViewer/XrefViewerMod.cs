using System;
using System.Diagnostics;
using System.Collections.Generic;
using MelonLoader;

using XrefViewer;
using XrefViewer.Core;
using XrefViewer.Core.Network;
using XrefViewer.Resources;

[assembly: MelonInfo(typeof(XrefViewerMod), "XrefViewer", "2.0.0", "DragonPlayer", "https://github.com/DragonPlayerX/XrefViewer")]
[assembly: MelonGame]

namespace XrefViewer
{
    public class XrefViewerMod : MelonMod
    {
        public static readonly string Version = "2.0.0";

        public static XrefViewerMod Instance { get; private set; }
        public static MelonLogger.Instance Logger => Instance.LoggerInstance;

        public override void OnApplicationStart()
        {
            Instance = this;
            Logger.Msg("Initializing XrefViewer " + Version + "...");

            ResourceHandler.ExtractIfNecessary("XrefViewerUI.exe", "Executables/XrefViewer");
            ResourceHandler.ExtractIfNecessary("AdonisUI.dll", "Executables/XrefViewer");
            ResourceHandler.ExtractIfNecessary("AdonisUI.ClassicTheme.dll", "Executables/XrefViewer");
            ResourceHandler.ExtractIfNecessary("Newtonsoft.Json.dll", "Executables/XrefViewer");

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "/Executables/XrefViewer/XrefViewerUI.exe";
            Process process = Process.Start(processStartInfo);

            NetworkManager.Init();
            NetworkManager.DataReceived += (connection, command) =>
            {
                try
                {
                    Logger.Msg("Received command [Type=" + command.TypeName + "/Method=" + command.MethodName + "/ExactMethodName=" + command.ExactMethodName + "]");
                    List<XrefObject> xrefResult = new List<XrefObject>();
                    string error = XrefCore.Scan(command.TypeName, command.MethodName, command.ExactMethodName, ref xrefResult);
                    XrefPacket xrefPacket = new XrefPacket(xrefResult, error);
                    NetworkManager.SendData(connection, xrefPacket);
                    Logger.Msg("Responded with " + xrefResult.Count + " xref results.");
                }
                catch (Exception e)
                {
                    Logger.Error("Error while executing remote command", e);
                }
            };

            Logger.Msg("Running version " + Version + " of XrefViewer.");
        }
    }
}
