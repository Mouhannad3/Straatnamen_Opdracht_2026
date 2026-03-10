using StraatnamenBL.Domein;
using StraatnamenBL.Domein.DTO_s;
using StraatnamenBL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Controller
{
    public class StraatNamenBeheerder
    {
        private IFileReader reader;
        private IStraatnamenRepository repo;

        public StraatNamenBeheerder(IFileReader reader, IStraatnamenRepository repo)
        {
            this.reader = reader;
            this.repo = repo;
        }
        //1) unzip zip bestand
        public List<string> UnZip(string padNaarZip)
        {
            var files = reader.UnZip(padNaarZip);

            return files;
        }
        public void MaakDataMap(string zipfile,string unzipfile)
        {
            reader.MaakDataMap(zipfile, unzipfile);
        }
        //2) elk bestand inlezen en verwerken
        public void ImportBestanden(List<string> bestandnamen,string unzippedfile)
        {
            //var unzippedfile = @"C:\Users\lukas\source\Repos\Straatnamen_Opdracht_2026\straatnamenInfo";
            var provincieList = reader.LeesBestanden(bestandnamen, unzippedfile);
            //UploadToDB(provincieList);
        }
        public void UploadToDB(List<Provincie> provincieList)
        {
            repo.UploadProvincieToDB(provincieList);
        }
        public void ExporteerStraatNamen(List<GemeenteDTO> gemeenten, string folder)
        {
            foreach (var gemeente in gemeenten)
            {
                List<StraatDTO> straten = GeefStraatNamenVanGemeente(gemeente.Id);

                List<string> straatnamen = straten.Select(s => s.StraatNaam).ToList();
                string filePath = Path.Combine(folder, $"{gemeente.GemeenteNaam}.txt");

                File.WriteAllLines(filePath, straatnamen);
            }
        }
        public List<GemeenteDTO> GeefAlleGemeenten()
        {
            return repo.GeefAlleGemeenten();
        }
        public List<StraatDTO> GeefStraatNamenVanGemeente(int id)
        {
            return repo.GeefStraatNamenVanGemeente(id);
        }
    }
}
