using Dapper;
using ProductRationing.Models;
using System.Collections.Generic;

namespace ProductRationing.DAL.Data
{
    public sealed class ProfessionRepo : RepoBase
    {
        public IEnumerable<Profession> GetAll() => conn.Query<Profession>(
"select * from PRProfessions");
    }
}
