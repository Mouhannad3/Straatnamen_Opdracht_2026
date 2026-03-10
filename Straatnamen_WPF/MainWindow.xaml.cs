using Microsoft.Win32;
using StraatnameDL.Reader;
using StraatnameDL.Repos;
using Straatnamen_Utlis;
using StraatnamenBL.Controller;
using StraatnamenBL.Domein.DTO_s;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Straatnamen_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog fileDialog = new OpenFileDialog();
        private OpenFolderDialog folderDialog = new OpenFolderDialog();
        private StraatNamenBeheerder Beheerder;
        private List<string> bestanden = new();
        string connectionstring = @"Data Source= Asus-TUF-A15\SQLEXPRESS; Initial Catalog = ProjectStraat; Integrated Security=True;TrustServerCertificate=True";
        public MainWindow()
        {
            InitializeComponent();
            this.Beheerder = new StraatNamenBeheerder(new FileReader(), new StraatnamenRepository(connectionstring));
            fileDialog.DefaultExt = ".zip";
            fileDialog.Filter = "Zip files (.zip)|*.zip";
            fileDialog.InitialDirectory = @"C:\Users\lukas\source\Repos\Straatnamen_Opdracht_2026";
        }
        private void Refresh()
        {
            lstboxGemeenten.ItemsSource = Beheerder.GeefAlleGemeenten();
        }

        private void Btn_OpenSourceFile_Click(object sender, RoutedEventArgs e)
        {
            bool? result = fileDialog.ShowDialog();
            if (result == true && !string.IsNullOrWhiteSpace(fileDialog.FileName))
            {
                TextBoxZipFile.Text = fileDialog.FileName;
                bestanden = Beheerder.UnZip(fileDialog.FileName);
            }
        }
        private void Btn_ImportAndUploadToDB_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxUnzippedFolder.Text))
            {
                Beheerder.MaakDataMap(fileDialog.FileName, TextBoxUnzippedFolder.Text);
                Beheerder.ImportBestanden(bestanden, TextBoxUnzippedFolder.Text);
                Refresh();
                MessageBox.Show("Gelukt");
            }
            else
            {
                MessageBox.Show("Unzipped textbox moet ingevuld zijn");
                return;
            }
        }
        private void Btn_OpenDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            bool? result = folderDialog.ShowDialog();
            if (result == true && !string.IsNullOrWhiteSpace(folderDialog.FolderName))
            {
                TextBoxDestinationFolder.Text = folderDialog.FolderName;
            }
        }
        private void Btn_Execute_Click(object sender, RoutedEventArgs e)
        {
            if (lstboxGemeenten.SelectedItems.Count > 0 )
            {
                var geselecteerdeGemeenten = lstboxGemeenten.SelectedItems.Cast<GemeenteDTO>().ToList();
                Beheerder.ExporteerStraatNamen(geselecteerdeGemeenten,TextBoxDestinationFolder.Text);
               
                MessageBox.Show("Gelukt");
            }
        }

        
    }
}