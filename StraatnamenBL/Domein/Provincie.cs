using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StraatnamenBL.Domein
{
    public class Provincie
    {
        public Provincie(int id, string naam)
        {
            Id = id;
            Naam = naam;
            _gemeenten = new List<Gemeente>();
        }

        public int Id {  get; set; }
        public string Naam {  get; set; }
        public List<Gemeente> _gemeenten { get; set; }
        public IReadOnlyList<Gemeente> Gemeenten()
        {
            return _gemeenten;
        }
        public void VoegGemeenteToe(Gemeente gemeente)
        {
            _gemeenten.Add(gemeente);
        }
        public bool HeeftGemeente(int id)
        {
            return _gemeenten.Any(x => x.Id == id);
        }
        public Gemeente GeefGemeente(int id)
        {
            return _gemeenten.Select(x => x).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
