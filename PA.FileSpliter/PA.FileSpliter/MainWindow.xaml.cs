using Microsoft.Win32;
using PA.FileSpliter.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PA.FileSpliter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MasterFile> attachedFiles = new List<MasterFile>();
        private string[] files;

        public object TabControlItem { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if(dlg.ShowDialog() ?? false)
            {
                files = dlg.FileNames;
                filenameTextbox.Text = string.Join(",", files);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if(files == null || files.Length == 0)
            {
                MessageBox.Show("No File Selected");
                return;
            }
            foreach(string f in files)
            {
                MasterFile mf = new MasterFile() { SourceFilename = f, SplitBy = SplitType.ByLine, Value = 1 };
                attachedFiles.Add(mf);
                UpdateUI(mf);
            }
            fileControl.SelectedIndex = fileControl.Items.Count - 1;
            files = null;
            filenameTextbox.Text = string.Empty;
        }

        private void UpdateUI(MasterFile mf)
        {
            TabItem tab = new TabItem();
            FileInfo info = new FileInfo(mf.SourceFilename);
            mf.FileExtention = info.Extension;
            tab.Header = info.Name;
            tab.Content = new SpliterControl() { MasterFile = mf, Margin = new Thickness(0) };
            fileControl.Items.Add(tab);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i< attachedFiles.Count; i++)
            {
                List<string> errors = attachedFiles[i].Validate();
                if(errors.Count > 0)
                {
                    fileControl.SelectedIndex = i;
                    Helper.ShowErrors(errors);
                    return;
                }
            }
            try
            {
                foreach (MasterFile file in attachedFiles)
                    file.Start();
                MessageBox.Show("All Done!!!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
