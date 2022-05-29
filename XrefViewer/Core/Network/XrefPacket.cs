using MelonLoader.TinyJSON;
using System.Collections.Generic;

namespace XrefViewer.Core.Network
{
    public class XrefPacket
    {
        public List<XrefObject> XrefData;
        public string ErrorData;

        public XrefPacket()
        {

        }

        public XrefPacket(List<XrefObject> xrefData, string errorData)
        {
            XrefData = xrefData;
            ErrorData = errorData;
        }

        public string ToJson() => Encoder.Encode(this, EncodeOptions.NoTypeHints);
    }
}
