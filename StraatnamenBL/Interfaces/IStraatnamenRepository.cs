using StraatnamenBL.Domein;
using StraatnamenBL.Domein.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Interfaces
{
    public interface IStraatnamenRepository
    {
        void UploadProvincieToDB(List<Provincie> data);
        List<GemeenteDTO> GeefAlleGemeenten();
        List<StraatDTO> GeefStraatNamenVanGemeente(int id);
    }
}
