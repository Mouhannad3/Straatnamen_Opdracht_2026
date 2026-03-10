using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Domein
{
    public class Straat
    {
        public Straat(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }

        public int Id { get; set; }
        public string Naam { get; set; }
    }
}
