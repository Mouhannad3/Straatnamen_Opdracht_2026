using StraatnameDL;
using StraatnameDL.Reader;
using StraatnameDL.Repos;
using StraatnamenBL.Controller;
using StraatnamenBL.Interfaces;
using System.IO.Compression;

namespace StraatnamenUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string connectionstring = @"Data Source= Asus-TUF-A15\SQLEXPRESS; Initial Catalog = ProjectStraat; Integrated Security=True;TrustServerCertificate=True";
            //IFileReader reader = new FileReader();
            //var repo = new StraatnamenRepository(connectionstring);
            //StraatNamenBeheerder beheerder = new StraatNamenBeheerder(reader, repo);
            //var bestanden = beheerder.UnZip(@"C:\Users\lukas\source\Repos\Straatnamen_Opdracht_2026\straatnamenInfo.zip");
            ////var extractpad = @"C:\Users\lukas\source\Repos\Straatnamen_Opdracht_2026\straatnameninfo";
            ////foreach (var item in bestanden)
            ////{
            ////    Console.WriteLine(item);
            ////}
            //beheerder.ImportBestanden(bestanden);

        }
    }
}
