using MelonLoader.TinyJSON;

namespace XrefViewer.Core.Network
{
    public class CommandPacket
    {
        public string TypeName;
        public string MethodName;
        public bool ExactMethodName;

        public CommandPacket()
        {

        }

        public static CommandPacket FromJson(string json) => Decoder.Decode(json).Make<CommandPacket>();
    }
}
