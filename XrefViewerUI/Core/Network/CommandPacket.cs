using Newtonsoft.Json;

namespace XrefViewerUI.Core.Network
{
    public class CommandPacket
    {
        public string TypeName;
        public string MethodName;
        public bool ExactMethodName;

        public CommandPacket(string typeName, string methodName, bool exactName)
        {
            TypeName = typeName;
            MethodName = methodName;
            ExactMethodName = exactName;
        }

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
    }
}
