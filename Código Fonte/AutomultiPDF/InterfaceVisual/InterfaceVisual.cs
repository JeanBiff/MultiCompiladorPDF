using System;
using System.Windows.Forms;

namespace InterfaceVisual
{

    public partial class InterfaceVisual : Form
    {

        public InterfaceVisual()
        {
            InitializeComponent();
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


        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            string diretorioSelecionado = directoryBox.Text;
            MessageBox.Show("Diretório selecionado: " + diretorioSelecionado);
        }

        private void directoryBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
