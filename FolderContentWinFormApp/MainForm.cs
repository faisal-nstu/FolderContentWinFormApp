using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FolderContentWinFormApp
{
    public partial class MainForm : Form
    {
        private DirectoryInfo _SelectedDirectory;
        private FileInfo[] _files;
        private StringBuilder clipData;
        public MainForm()
        {
            InitializeComponent();
            saveButton.Enabled = false;
            //saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileOk += SaveFileDialogOnFileOk;
        }

        private void SaveFileDialogOnFileOk(object sender, CancelEventArgs cancelEventArgs)
        {
            string saveAsName = saveFileDialog.FileName;
            string fileNames = "";
            foreach (var fileInfo in _files)
            {
                fileNames += fileInfo.Name;
                fileNames += "\n";
            }

            WriteNamesToFile(saveAsName,_files);
            saveButton.Enabled = false;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _SelectedDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                saveButton.Enabled = true;
            }

            _files = _SelectedDirectory.GetFiles("*.*", SearchOption.AllDirectories);
            // copy to clipboard
            clipData = new StringBuilder();
            //foreach (var file in _files)
            //{
            //    clipData.AppendLine(file.Name);
            //}
            foreach (var line in GetFileNamesFromPath(_files))
            {
                clipData.AppendLine(line);
            }
            Clipboard.SetText(clipData.ToString());
            MessageBox.Show("File names copied in Clipboard\nClick \"Save as Text File\" to save as text file");
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        public static void WriteNamesToFile(string saveAsName, FileInfo[] fileInfos)
        {
            try
            {
                //var fNam = GetFileNamesFromPath(fileInfos);
                File.WriteAllText(saveAsName, Clipboard.GetText());
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
            }
        }

        private static List<string> GetFileNamesFromPath(FileInfo[] fileInfos)
        {
            List<string> fNam = new List<string>();
            string currentDir = fileInfos[0].DirectoryName;
            fNam.Add(fileInfos[0].Directory.Name);
            //, FileInfo[] fileInfos
            //File.WriteAllText(saveAsName, names);
            foreach (var fileInfo in fileInfos)
            {
                if (currentDir == fileInfo.DirectoryName)
                {
                    fNam.Add("\t" + fileInfo.Name);
                }
                else
                {
                    currentDir = fileInfo.DirectoryName;
                    fNam.Add(fileInfo.Directory.Name);
                }
            }
            return fNam;
        }
    }
}
