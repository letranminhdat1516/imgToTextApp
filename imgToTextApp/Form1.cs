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
using Tesseract;
namespace imgToTextApp
{
    public partial class Form1 : Form
    {
        private string selectedPath; // Đường dẫn của file hoặc folder đã chọn
        private string saveLocation; // Nơi lưu kết quả
        public Form1()
        {
            InitializeComponent();
        }
        // button convert
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(saveLocation))
            {
                MessageBox.Show("Please select a save location first.");
                return;
            }
            // process choosing the folder or file
            // choose the path using tesseractEngine 
            using (var engine = new TesseractEngine(@"C:\Program Files (x86)\tessdata", "eng", EngineMode.Default))
            {
                if (Directory.Exists(selectedPath)) // if folder
                {
                    string newFolderName = "textFolder" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    string newFolderPath = Path.Combine(saveLocation, newFolderName);
                    Directory.CreateDirectory(newFolderPath);

                    foreach (var imageFile in Directory.GetFiles(selectedPath, "*.png").Union(Directory.GetFiles(selectedPath, "*.jpg")))
                    {
                        ProcessImage(imageFile, engine, newFolderPath);
                    }
                }
                else if (File.Exists(selectedPath)) //if file
                {
                    ProcessImage(selectedPath, engine, saveLocation);
                }
            }

            MessageBox.Show("Conversion done!");
        }
        // process img  
        // img --> txt
        private void ProcessImage(string imageFile, TesseractEngine engine, string savePath)
        {
            // load file
            using (var img = Pix.LoadFromFile(imageFile))
            {
                var page = engine.Process(img);
                string textResult = page.GetText();

                string saveFileName = string.IsNullOrEmpty(txtFileName.Text) ? Path.GetFileNameWithoutExtension(imageFile) : txtFileName.Text;
                File.WriteAllText(Path.Combine(savePath, saveFileName + ".txt"), textResult);
            }
        }
        // button select folder
        private void button1_Click_1(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;
                    txtDisplayPath.Text = selectedPath;
                }
            }
        }
        // button save location
        private void button1_Click_2(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    saveLocation = folderDialog.SelectedPath;
                    MessageBox.Show("Location selected!");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        // button select file
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Image Files|*.png;*.jpg";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = fileDialog.FileName;
                    txtDisplayPath.Text = selectedPath;
                }
            }
        }
        // text display input path -- can be change by text
        private void txtDisplayPath_TextChanged(object sender, EventArgs e)
        {

        }
        // text file name  -- can be change by text
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
