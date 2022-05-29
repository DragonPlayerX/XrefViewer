using Newtonsoft.Json;
using System.Collections.Generic;

using XrefViewerUI.MVVM.Model;

namespace XrefViewer.Core.Network
{
    public class XrefPacket
    {
        public List<XrefObject> XrefData;
        public string ErrorData;

        public XrefPacket()
        {

        }

        public static XrefPacket FromJson(string json) => JsonConvert.DeserializeObject<XrefPacket>(json);
    }
}
