using System;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading.Tasks;

namespace InterfaceVisual
{

    public partial class InterfaceVisual : Form
    {

        public InterfaceVisual()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.TopMost = true;
            textBox2.ReadOnly = true;
            directoryBox.Text = "Digite o diretório, ou selecione manualmente";
            textBox1.Text = "Digite o nome da pasta aqui.";
            textBox2.Text = "";
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = "Selecione um diretório";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string diretorioSelecionado = folderBrowserDialog.SelectedPath;
                directoryBox.Text = diretorioSelecionado;

            }
        }

       
        private async void buttonConfirm_Click(object sender, EventArgs e)
        {
            string selectedDirectory = directoryBox.Text;
            if (string.IsNullOrEmpty(selectedDirectory) || !Directory.Exists(selectedDirectory))
            {
                MessageBox.Show("Inválido, digite novamente.");
            }
            else
            {
                textBox2.Text = "Processando...";
                await Task.Run(() =>
                {
                    string rootFolder = selectedDirectory;
                    string outputFolder = Path.Combine(rootFolder, outputFolderName);
                    Directory.CreateDirectory(outputFolder);

                    ProcessFolders(rootFolder, outputFolder);
                });
                textBox2.Text = "Pronto";
            }
        }


        private void directoryBox_TextChanged(object sender, EventArgs e)
        {

        }

        static string outputFolderName;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            outputFolderName = textBox1.Text;
        }


        static void ProcessFolders(string rootFolder, string outputFolder)
        {
            string[] folders = Directory.GetDirectories(rootFolder);

            foreach (string folder in folders)
            {
                string[] pdfFiles = Directory.GetFiles(folder, "*.pdf");

                if (pdfFiles.Length > 0)
                {
                    string outputFileName = Path.Combine(outputFolder, $"{Path.GetFileName(folder)}.pdf");
                    MergePDFs(pdfFiles, outputFileName);
                }
            }
        }

        static void MergePDFs(string[] filesToMerge, string outputFilePath)
        {

            using (FileStream stream = new FileStream(outputFilePath, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                document.Open();

                foreach (string file in filesToMerge)
                {
                    PdfReader reader = new PdfReader(file);
                    pdf.AddDocument(reader);
                    reader.Close();
                }

                pdf.Close();
                document.Close();
            }
        }



        void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
