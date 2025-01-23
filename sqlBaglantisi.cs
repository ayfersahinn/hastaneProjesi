using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace hastaneProjesi
{
    class sqlBaglantisi
    {
        public SqlConnection baglanti() {

            SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-5ANGUIR\\SQLEXPRESS02;Initial Catalog=HastaneProje;Integrated Security=True;Encrypt=False");
            
            baglan.Open();
            return baglan;
        }
    }
}
