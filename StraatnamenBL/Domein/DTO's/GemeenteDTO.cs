using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Domein.DTO_s
{
    public class GemeenteDTO
    {
        public GemeenteDTO(int id, string gemeenteNaam)
        {
            Id = id;
            GemeenteNaam = gemeenteNaam;
        }
        public int Id { get; set; }
        public string GemeenteNaam { get; set; }
       
    }
}
