using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;

using XrefViewerUI.Core.Network;
using XrefViewerUI.MVVM.Model;

using MessageBox = AdonisUI.Controls.MessageBox;
using MessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using MessageBoxImage = AdonisUI.Controls.MessageBoxImage;

namespace XrefViewerUI.Core
{
    public class DataHandler
    {
        public static ObservableCollection<XrefObject> XrefObjects = new ObservableCollection<XrefObject>();
        public static XrefObject SelectedObject;

        public static void Init()
        {
            NetworkManager.DataReceived += (packet) =>
            {
                List<XrefObject> xrefObjects = packet.XrefData;
                if (xrefObjects != null)
                {
                    foreach (XrefObject obj in xrefObjects)
                        XrefObjects.Add(obj);

                    Console.WriteLine("Added " + xrefObjects.Count + " objects");
                }

                if (packet.ErrorData != null)
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show(packet.ErrorData, "Xref Error", MessageBoxButton.OK, MessageBoxImage.Error));
            };
        }

        public static void Export(string file, XrefObject xrefObject, bool json = true)
        {
            File.WriteAllText(file, json ? xrefObject.ToJson() : xrefObject.ToText());
        }

        public static void Export(string file, List<XrefObject> xrefObjects, bool json = true)
        {
            if (json)
            {
                File.WriteAllText(file, JsonConvert.SerializeObject(xrefObjects, Formatting.Indented));
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                foreach (XrefObject xrefObject in xrefObjects)
                    builder.Append(xrefObject.ToText());

                File.WriteAllText(file, builder.ToString());
            }
        }

        public static void SendScanCommand(string typeName, string methodName, bool exactName)
        {
            CommandPacket packet = new CommandPacket(typeName, methodName, exactName);
            NetworkManager.SendData(packet);
        }
    }
}
