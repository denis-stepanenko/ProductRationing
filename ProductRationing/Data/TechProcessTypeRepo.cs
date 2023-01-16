using Dapper;
using ProductRationing.Models;
using System.Collections.Generic;

namespace ProductRationing.DAL.Data
{
    public sealed class TechProcessTypeRepo : RepoBase
    {
        public IEnumerable<TechProcessType> GetAll() => conn.Query<TechProcessType>(
"select * from PRTechProcessTypes");
    }
}
