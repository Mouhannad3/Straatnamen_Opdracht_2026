using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Domein.DTO_s
{
    public class StraatDTO
    {
        public StraatDTO(string straatNaam)
        {
            StraatNaam = straatNaam;
        }

        public int Id { get; set; }
        public string StraatNaam { get; set; }
        
    }
}
