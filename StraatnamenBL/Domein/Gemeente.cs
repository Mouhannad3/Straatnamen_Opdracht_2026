using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Domein
{
    public class Gemeente
    {
        public Gemeente(int id)
        {
            Id = id;
            _straten = new List<Straat>();
        }

        public int Id {  get; set; }
        public string Naam { get; set; }
        private List<Straat> _straten { get; set; }

        public IReadOnlyList<Straat> Straten()
        {
            return _straten;
        }
        public void VoegStraatToe(Straat straat)
        {
            _straten.Add(straat);
        }
    }
}
