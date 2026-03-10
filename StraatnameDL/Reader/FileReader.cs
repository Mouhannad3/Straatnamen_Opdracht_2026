using StraatnamenBL.Domein;
using StraatnamenBL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnameDL.Reader
{
    public class FileReader : IFileReader
    {
        public List<string> UnZip(string padNaarZip)
        {
            List<string> bestanden = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(padNaarZip)) 
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    bestanden.Add(entry.FullName);
                }
            }
            return bestanden;
        }
        //extractpad is het pad naar de nieuw aangemaakte folder 
        //vb folder.zip => folder
        public List<Provincie> LeesBestanden(List<string> bestanden,string extractpad)
        {
            //zoek bestand provincieidsVlaanderen.csv
            string bestandProvincieId = bestanden.Find(s => s.ToLower() == "provincieidsvlaanderen.csv");
            if (bestandProvincieId == null) 
            {
                throw new Exception("Bestand niet gevonden");
            }
            string bestandProvincieInfo = bestanden.Find(s => s.ToLower() == "provincieinfo.csv");
            if (bestandProvincieInfo == null)
            {
                throw new Exception("Bestand niet gevonden");
            }
            string bestandGemeentenaam = bestanden.Find(s => s.ToLower() == "gemeentenaam.csv");
            if (bestandGemeentenaam == null)
            {
                throw new Exception("Bestand niet gevonden");
            }
            string bestandStraatnaam = bestanden.Find(s => s.ToLower() == "straatnamen.csv");
            if (bestandStraatnaam == null)
            {
                throw new Exception("Bestand niet gevonden");
            }
            string bestandStraatGemeenteId = bestanden.Find(s => s.ToLower() == "straatnaamid_gemeenteid.csv");
            if (bestandStraatGemeenteId == null)
            {
                throw new Exception("Bestand niet gevonden");
            }

            Dictionary<int, Provincie> provincies = new Dictionary<int, Provincie>();
            Dictionary<int, Gemeente> gemeenten = new Dictionary<int, Gemeente>();
            Dictionary<int,Straat> straten = new Dictionary<int, Straat>();
            List<int> provincieIds = new List<int>();
            using (StreamReader sr = new StreamReader(Path.Combine(extractpad, bestandProvincieId)))
            {
                string[] ids = sr.ReadLine().Trim().Split(",");
                foreach (var id in ids)
                {
                    provincieIds.Add(int.Parse(id));
                }
            }
            using (StreamReader sr = new StreamReader(Path.Combine(extractpad, bestandProvincieInfo)))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] ss = line.Split(";");
                    int gemeenteId = int.Parse(ss[0]);
                    int provincieId = int.Parse(ss[1]);
                    string taalcode = ss[2];
                    string provincieNaam = ss[3];
                    if (provincieIds.Contains(provincieId) && taalcode=="nl")
                    {
                        if (!provincies.ContainsKey(provincieId))
                        {
                            provincies.Add(provincieId, new Provincie(provincieId, provincieNaam));
                        }
                        if (!provincies[provincieId].HeeftGemeente(gemeenteId))
                        {
                            provincies[provincieId].VoegGemeenteToe(new Gemeente(gemeenteId));
                        }
                        gemeenten.Add(gemeenteId,provincies[provincieId].GeefGemeente(gemeenteId));
                       
                    }
                }
            }
            using (StreamReader sr = new StreamReader(Path.Combine(extractpad, bestandGemeentenaam)))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] ss = line.Split(";");
                    int gemeenteId = int.Parse(ss[1]);
                    string taalcode = ss[2];
                    string gemeenteNaam = ss[3];
                    if (taalcode == "nl")
                    {
                        if (gemeenten.ContainsKey(gemeenteId))
                        {
                            gemeenten[gemeenteId].Naam = gemeenteNaam;
                        }
                    }
                }
            }
            using(StreamReader sr = new StreamReader(Path.Combine(extractpad, bestandStraatnaam)))
            {
                string line;
                sr.ReadLine();
                sr.ReadLine();
                while((line = sr.ReadLine()) != null)
                {
                    string[] ss = line.Split(";");
                    int straatnaamId = int.Parse(ss[0]);
                    string straatnaam = ss[1];
                    if (!straten.ContainsKey(straatnaamId))
                    {
                        straten.Add(straatnaamId, new Straat(straatnaamId,straatnaam));
                    }
                }
            }
            using (StreamReader sr = new StreamReader(Path.Combine(extractpad, bestandStraatGemeenteId)))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] ss = line.Split(";");
                    int straatNaamId = int.Parse(ss[0]);
                    int gemeenteId = int.Parse(ss[1]);
                    if (gemeenten.ContainsKey(gemeenteId) && straten.ContainsKey(straatNaamId))
                    {
                        gemeenten[gemeenteId].VoegStraatToe(new Straat(straten[straatNaamId].Id, straten[straatNaamId].Naam));
                    }
                }
            }
            return provincies.Values.ToList();
        }
        public void MaakDataMap(string zipfile,string unzipfile)
        {
            ZipFile.ExtractToDirectory(zipfile, unzipfile/*@"C:\Users\lukas\source\Repos\Straatnamen_Opdracht_2026\straatnamenInfo"*/);
        }

    }
}
