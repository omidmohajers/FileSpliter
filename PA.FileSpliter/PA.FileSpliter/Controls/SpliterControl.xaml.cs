using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PA.FileSpliter.Controls
{
    /// <summary>
    /// Interaction logic for SpliterControl.xaml
    /// </summary>
    public partial class SpliterControl : UserControl
    {
        private MasterFile file;

        public SpliterControl()
        {
            InitializeComponent();
        }

        public MasterFile MasterFile
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (file == null)
            {
                ClearUI();
                return;
            }
            switch (file.SplitBy)
            {
                case SplitType.ByLine:
                    byLineCheckbox.IsChecked = true;
                    lineValueTextBox.Text = file.Value.ToString("N0");
                    sizeValueTextBox.Text = "0";
                    break;
                case SplitType.bySize:
                    bySizeCheckBox.IsChecked = true;
                    sizeValueTextBox.Text = file.Value.ToString("N0");
                    lineValueTextBox.Text = "0";
                    break;
            }
            previewButton.IsEnabled = true;
        }

        private void ClearUI()
        {
            outputTextBox.Text = string.Empty;
            byLineCheckbox.IsChecked = true;
            outFileNameTextBox.Text = string.Empty;
            previewListView.Items.Clear();
            previewButton.IsEnabled = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void saveToButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browser = new System.Windows.Forms.FolderBrowserDialog();
            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputTextBox.Text = MasterFile.OutputPath = browser.SelectedPath;
            }
        }

        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
            List<string> errors = MasterFile.Validate();
            if (errors.Count > 0)
            {
                Helper.ShowErrors(errors);
            }
            previewListView.Items.Clear();
            MasterFile preFile = (MasterFile)file.Clone();
            preFile.Preview();
            foreach(SplitedFile sf in preFile.Files)
            {
                ListViewItem item = new ListViewItem();
                item.Content = sf.FileName;
                previewListView.Items.Add(item);
            }
        }

        private void UpdateData()
        {
            MasterFile.SplitBy = byLineCheckbox.IsChecked.Value ?  SplitType.ByLine  : SplitType.bySize;
            MasterFile.Value = MasterFile.SplitBy == SplitType.ByLine ? int.Parse(lineValueTextBox.Text) : int.Parse(sizeValueTextBox.Text);
            MasterFile.OutputFilename = outFileNameTextBox.Text.Trim();
        }
    }
}
