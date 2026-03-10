using Microsoft.Data.SqlClient;
using StraatnamenBL.Domein;
using StraatnamenBL.Domein.DTO_s;
using StraatnamenBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StraatnameDL.Repos
{
    public class StraatnamenRepository : IStraatnamenRepository
    {
        private string connectionstring;

        public StraatnamenRepository(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }
        public void UploadProvincieToDB(List<Provincie> data)
        {
            string queryProvincie = "insert into Provincie(Id,ProvincieNaam) values (@Id,@ProvincieNaam)";
            string queryGemeente = "insert into Gemeente(Id,GemeenteNaam,ProvincieId) values (@Id,@GemeenteNaam,@ProvincieId)";
            string queryStraat = "insert into Straat(Id,StraatNaam,GemeenteId) values (@Id,@StraatNaam,@GemeenteId)";
            using(SqlConnection connection = new SqlConnection(connectionstring))
            using(SqlCommand cmdProvincie = connection.CreateCommand())
            using (SqlCommand cmdGemeente = connection.CreateCommand())
            using (SqlCommand cmdStraat = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                cmdProvincie.Transaction = transaction;
                cmdGemeente.Transaction = transaction;
                cmdStraat.Transaction = transaction;
                try
                {
                    cmdProvincie.CommandText = queryProvincie;
                    cmdProvincie.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmdProvincie.Parameters.Add("@ProvincieNaam", System.Data.SqlDbType.NVarChar);

                    cmdGemeente.CommandText = queryGemeente;
                    cmdGemeente.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmdGemeente.Parameters.Add("@GemeenteNaam", System.Data.SqlDbType.NVarChar);
                    cmdGemeente.Parameters.Add("@ProvincieId", System.Data.SqlDbType.Int);

                    cmdStraat.CommandText = queryStraat;
                    cmdStraat.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmdStraat.Parameters.Add("@StraatNaam", System.Data.SqlDbType.NVarChar);
                    cmdStraat.Parameters.Add("@GemeenteId", System.Data.SqlDbType.Int);

                    foreach (Provincie p in data)
                    {
                        cmdProvincie.Parameters["@Id"].Value = p.Id;
                        cmdProvincie.Parameters["@ProvincieNaam"].Value = p.Naam;
                        cmdProvincie.ExecuteNonQuery();
                        foreach (Gemeente g in p.Gemeenten())
                        {
                            cmdGemeente.Parameters["@Id"].Value = g.Id;
                            cmdGemeente.Parameters["@GemeenteNaam"].Value = g.Naam;
                            cmdGemeente.Parameters["@ProvincieId"].Value = p.Id;
                            cmdGemeente.ExecuteNonQuery();
                            foreach (Straat s in g.Straten())
                            {
                                cmdStraat.Parameters["@Id"].Value = s.Id;
                                cmdStraat.Parameters["@StraatNaam"].Value = s.Naam;
                                cmdStraat.Parameters["@GemeenteId"].Value = g.Id;
                                cmdStraat.ExecuteNonQuery();
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch(Exception ex) 
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<GemeenteDTO> GeefAlleGemeenten()
        {
            List<GemeenteDTO> gemeenten = new List<GemeenteDTO>();
            string query = "select Id,GemeenteNaam from Gemeente";
            using(SqlConnection connection = new SqlConnection(connectionstring))
            using(SqlCommand cmd  = connection.CreateCommand()) 
            {
                connection.Open();
                cmd.CommandText = query;
                using (SqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        GemeenteDTO gemeente = new GemeenteDTO((int)reader["Id"], (string)reader["GemeenteNaam"]);
                        gemeenten.Add(gemeente);
                    }
                }
                return gemeenten;
            }
        }
        public List<StraatDTO> GeefStraatNamenVanGemeente(int id)
        {
            List<StraatDTO> straten = new List<StraatDTO>();
            string query = "select s.StraatNaam from Straat s where s.GemeenteId = @id order by s.StraatNaam";
            using(SqlConnection connection=new SqlConnection(connectionstring))
            using(SqlCommand cmd = connection.CreateCommand())
            {
                connection.Open();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        StraatDTO straat = new StraatDTO((string)reader["StraatNaam"]);
                        straten.Add(straat);
                    }
                }
                return straten;
            }
        }
        //public List<GemeenteDTO> GeefAlleGemeenten()
        //{
        //    Dictionary<int,GemeenteDTO> gemeenten = new Dictionary<int,GemeenteDTO>();
        //    string query = "select g.Id,g.GemeenteNaam,s.StraatNaam from Gemeente g join Straat s on s.GemeenteId = g.Id";
        //    using(SqlConnection connection = new SqlConnection(connectionstring))
        //    using(SqlCommand cmd = connection.CreateCommand())
        //    {
        //        connection.Open();
        //        cmd.CommandText = query;
        //        using (SqlDataReader reader = cmd.ExecuteReader()) 
        //        {
        //            while (reader.Read())
        //            {
        //                int id = (int)reader["Id"];
        //                string gemeentenaam = (string)reader["GemeenteNaam"];
        //                string straatNaam = (string)reader["StraatNaam"];
        //                if (!gemeenten.ContainsKey(id))
        //                {
        //                    gemeenten[id] = new GemeenteDTO(id, gemeentenaam);
        //                }
        //                gemeenten[id].StraatNamen.Add(straatNaam);
        //            }
        //        }
        //    }
        //    return gemeenten.Values.ToList();
        //}
    }
}
