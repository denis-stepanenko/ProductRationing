using System.Data;
using System.Data.SqlClient;

namespace ProductRationing.DAL.Data
{
    public class RepoBase
    {
        protected IDbConnection conn;

        public RepoBase()
        {
            conn = new SqlConnection("");
            conn.Open();
        }
    }
}