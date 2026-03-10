using StraatnamenBL.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Interfaces
{
    public interface IFileReader
    {
        List<string> UnZip(string padNaarZip);
        List<Provincie> LeesBestanden(List<string> entries, string pad);
        void MaakDataMap(string zipfile, string unzipfile);
    }
}
