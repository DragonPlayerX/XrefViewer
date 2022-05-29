using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace XrefViewerUI.MVVM.Model
{
    public class XrefObject
    {
        public string Name { get; set; }
        public string MethodType { get; set; }
        public long Pointer { get; set; }
        public XrefType Type { get; set; }
        public ObservableCollection<XrefObject> IsUsing = new ObservableCollection<XrefObject>();
        public ObservableCollection<XrefObject> UsedBy = new ObservableCollection<XrefObject>();

        [JsonIgnore] public int IsUsingCount => IsUsing.Count;
        [JsonIgnore] public int UsedByCount => UsedBy.Count;
        [JsonIgnore] public int ResolvedIsUsingCount => IsUsing.Where(x => x.Type != XrefType.Unresolved).Count();
        [JsonIgnore] public int ResolvedUsedByCount => UsedBy.Where(x => x.Type != XrefType.Unresolved).Count();
        [JsonIgnore] public bool IsString => Type == XrefType.String;
        [JsonIgnore] public bool IsResolved => Type != XrefType.Unresolved;
        [JsonIgnore] public Visibility TypeVisibility => IsString || !IsResolved ? Visibility.Collapsed : Visibility.Visible;
        [JsonIgnore] public Visibility NameVisibility => IsResolved ? Visibility.Visible : Visibility.Collapsed;
        [JsonIgnore] public Visibility StringVisibility => IsString ? Visibility.Visible : Visibility.Collapsed;

        public enum XrefType
        {
            Method,
            String,
            Unresolved
        }

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public string ToText()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Scan result of " + MethodType + "#" + Name);
            builder.AppendLine("").AppendLine("Method is using:");

            foreach (XrefObject xrefObject in IsUsing)
            {
                if (xrefObject.IsResolved)
                {
                    if (xrefObject.IsString)
                        builder.AppendLine("String: " + xrefObject.Name + " | Pointer: " + xrefObject.Pointer);
                    else
                        builder.AppendLine(xrefObject.MethodType + "#" + xrefObject.Name + " | Pointer: " + xrefObject.Pointer);
                }
            }

            builder.AppendLine("").AppendLine(ResolvedIsUsingCount + "/" + IsUsingCount + " resolved");
            builder.AppendLine("").AppendLine("").AppendLine("Method is used by:");

            foreach (XrefObject xrefObject in UsedBy)
            {
                if (xrefObject.IsResolved)
                {
                    if (xrefObject.IsString)
                        builder.AppendLine("String: " + xrefObject.Name + " | Pointer: " + xrefObject.Pointer);
                    else
                        builder.AppendLine(xrefObject.MethodType + "#" + xrefObject.Name + " | Pointer: " + xrefObject.Pointer);
                }
            }

            builder.AppendLine("").AppendLine(ResolvedUsedByCount + "/" + UsedByCount + " resolved");
            builder.AppendLine("").AppendLine("");

            return builder.ToString();
        }
    }
}
