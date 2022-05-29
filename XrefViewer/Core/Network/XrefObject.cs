using System.Collections.Generic;

namespace XrefViewer.Core.Network
{
    public class XrefObject
    {
        public string Name;
        public string MethodType;
        public long Pointer;
        public XrefType Type;
        public List<XrefObject> IsUsing = new List<XrefObject>();
        public List<XrefObject> UsedBy = new List<XrefObject>();

        public enum XrefType
        {
            Method,
            String,
            Unresolved
        }
    }
}
