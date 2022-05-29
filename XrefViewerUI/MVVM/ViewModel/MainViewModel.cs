using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;

using XrefViewerUI.Core;
using XrefViewerUI.MVVM.Model;

namespace XrefViewerUI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public ListCollectionView XrefObjects { get; private set; }

        public XrefObject SelectedObject
        {
            get => DataHandler.SelectedObject;
            set
            {
                if (value != null)
                {
                    UsingXrefObjects = new ListCollectionView(value.IsUsing.Where(x => x.Type != XrefObject.XrefType.Unresolved).ToList());
                    UsedByXrefObjects = new ListCollectionView(value.UsedBy.Where(x => x.Type != XrefObject.XrefType.Unresolved).ToList());
                }

                DataHandler.SelectedObject = value;
                OnPropertyChanged(null);
            }
        }

        public ListCollectionView UsingXrefObjects { get; private set; }
        public ListCollectionView UsedByXrefObjects { get; private set; }

        public string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));

                if (string.IsNullOrEmpty(value))
                    XrefObjects.Filter = null;
                else
                    XrefObjects.Filter = new Predicate<object>(o => (((XrefObject)o).MethodType + ((XrefObject)o).Name).Contains(value));
            }
        }

        public string TypeText { get; set; }
        public string MethodText { get; set; }
        public bool ExactNameState { get; set; }

        public RelayCommand RunScanCommand { get; private set; }
        public RelayCommand ScanMethodCommand { get; private set; }
        public RelayCommand ScanTypeCommand { get; private set; }
        public RelayCommand CopyMethodNameCommand { get; private set; }
        public RelayCommand CopyMethodPointerCommand { get; private set; }
        public RelayCommand ClearListCommand { get; private set; }
        public RelayCommand RemoveEntryCommand { get; private set; }
        public RelayCommand ExportMethodAsJsonCommand { get; private set; }
        public RelayCommand ExportListAsJsonCommand { get; private set; }
        public RelayCommand ExportMethodAsTextCommand { get; private set; }
        public RelayCommand ExportListAsTextCommand { get; private set; }

        public MainViewModel()
        {
            XrefObjects = new ListCollectionView(DataHandler.XrefObjects);

            RunScanCommand = new RelayCommand(o => DataHandler.SendScanCommand(TypeText, MethodText, ExactNameState));
            ScanMethodCommand = new RelayCommand(o =>
            {
                XrefObject x = o as XrefObject;
                DataHandler.SendScanCommand(x.MethodType, x.Name, true);
            }, o => o != null && o is XrefObject);
            ScanTypeCommand = new RelayCommand(o =>
            {
                XrefObject x = o as XrefObject;
                DataHandler.SendScanCommand(x.MethodType, null, false);
            }, o => o != null && o is XrefObject);
            CopyMethodNameCommand = new RelayCommand(o =>
            {
                XrefObject x = o as XrefObject;
                if (x.IsString)
                    Clipboard.SetText(x.Name);
                else
                    Clipboard.SetText(x.MethodType + "#" + x.Name);
            }, o => o != null && o is XrefObject);
            CopyMethodPointerCommand = new RelayCommand(o => Clipboard.SetText((o as XrefObject).Pointer.ToString()), o => o != null && o is XrefObject);
            ClearListCommand = new RelayCommand(o => DataHandler.XrefObjects.Clear(), o => DataHandler.XrefObjects.Count != 0);
            RemoveEntryCommand = new RelayCommand(o => DataHandler.XrefObjects.Remove(o as XrefObject), o => o != null && o is XrefObject);
            ExportMethodAsJsonCommand = new RelayCommand(o =>
            {
                XrefObject x = o as XrefObject;

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Export xref data...";
                fileDialog.Filter = "JSON files|*.json";
                fileDialog.CheckPathExists = true;

                if (fileDialog.ShowDialog() ?? false)
                    DataHandler.Export(fileDialog.FileName, x);
            }, o => o != null && o is XrefObject);
            ExportListAsJsonCommand = new RelayCommand(o =>
            {
                IEnumerable<XrefObject> collection = (o as ListCollectionView).SourceCollection as IEnumerable<XrefObject>;
                List<XrefObject> list = new List<XrefObject>(collection);

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Export xref data...";
                fileDialog.Filter = "JSON files|*.json";
                fileDialog.CheckPathExists = true;

                if (fileDialog.ShowDialog() ?? false)
                    DataHandler.Export(fileDialog.FileName, list);
            }, o => o != null && o is ListCollectionView);
            ExportMethodAsTextCommand = new RelayCommand(o =>
            {
                XrefObject x = o as XrefObject;

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Export xref data...";
                fileDialog.Filter = "Text files|*.txt";
                fileDialog.CheckPathExists = true;

                if (fileDialog.ShowDialog() ?? false)
                    DataHandler.Export(fileDialog.FileName, x, false);
            }, o => o != null && o is XrefObject);
            ExportListAsTextCommand = new RelayCommand(o =>
            {
                IEnumerable<XrefObject> collection = (o as ListCollectionView).SourceCollection as IEnumerable<XrefObject>;
                List<XrefObject> list = new List<XrefObject>(collection);

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Export xref data...";
                fileDialog.Filter = "Text files|*.txt";
                fileDialog.CheckPathExists = true;

                if (fileDialog.ShowDialog() ?? false)
                    DataHandler.Export(fileDialog.FileName, list, false);
            }, o => o != null && o is ListCollectionView);
        }
    }
}
