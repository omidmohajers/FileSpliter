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

namespace PA.FileSplitter.Controls
{
    /// <summary>
    /// Interaction logic for SplitterControl.xaml
    /// </summary>
    public partial class SplitterControl : UserControl
    {
        private MasterFile file;
        private bool init = false;
        public SplitterControl()
        {
            InitializeComponent();
            init = true;
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
            splitbyComboBox.SelectedIndex = (int)file.SplitBy;
            switch (file.SplitBy)
            {
                case SplitType.ByLine:
                    lineValueTextBox.Text = file.Value;
                    sizeValueTextBox.Text = "0";
                    charValueTextBox.Text = string.Empty;
                    phraseValueTextBox.Text = string.Empty;
                    break;
                case SplitType.BySize:
                    lineValueTextBox.Text = "0";
                    sizeValueTextBox.Text = file.Value;
                    charValueTextBox.Text = string.Empty;
                    phraseValueTextBox.Text = string.Empty;
                    break;
                case SplitType.ByChar:
                    lineValueTextBox.Text = "0";
                    sizeValueTextBox.Text = "0";
                    charValueTextBox.Text = file.Value;
                    phraseValueTextBox.Text = string.Empty;
                    break;
                case SplitType.ByPhrase:
                    lineValueTextBox.Text = "0";
                    sizeValueTextBox.Text = "0";
                    charValueTextBox.Text = string.Empty;
                    phraseValueTextBox.Text = file.Value;
                    break;
            }
            previewButton.IsEnabled = true;
        }

        private void ClearUI()
        {
            outputTextBox.Text = string.Empty;
            splitbyComboBox.SelectedIndex= 0;
            outFileNameTextBox.Text = string.Empty;
            previewListView.Items.Clear();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveToButton_Click(object sender, RoutedEventArgs e)
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
                return;
            }
            previewListView.Items.Clear();
            MasterFile preFile = (MasterFile)file.Clone();
            preFile.Preview();
            foreach(SplittedFile sf in preFile.Files)
            {
                ListViewItem item = new ListViewItem();
                item.Content = sf.FileName;
                previewListView.Items.Add(item);
            }
        }

        private void UpdateData()
        {
            MasterFile.SplitBy = (SplitType)splitbyComboBox.SelectedIndex;
            switch (MasterFile.SplitBy)
            {
                case SplitType.ByLine:
                    MasterFile.Value = lineValueTextBox.Text;
                    break;
                case SplitType.BySize:
                    MasterFile.Value = sizeValueTextBox.Text;
                    break;
                case SplitType.ByChar:
                    MasterFile.Value = charValueTextBox.Text;
                    break;
                case SplitType.ByPhrase:
                    MasterFile.Value = phraseValueTextBox.Text;
                    break;
            }
            MasterFile.OutputFilename = outFileNameTextBox.Text.Trim();
        }

        private void splitbyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!init)
                return;
            lineGroupBox.Visibility = splitbyComboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            sizeGroupBox.Visibility = splitbyComboBox.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            charGroupBox.Visibility = splitbyComboBox.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
            phraseGroupBox.Visibility = splitbyComboBox.SelectedIndex == 3 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
