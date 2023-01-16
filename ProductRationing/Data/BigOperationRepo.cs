using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class BigOperationRepo : RepoBase
    {
        public BigOperation Get(int id) => conn.Query<BigOperation>(
"select Id, Code, Name from ref_top_2 where forLastVerRef = 1 and Id = @Id", new { Id = id }).Single();

        public IEnumerable<BigOperation> GetAll() => conn.Query<BigOperation>(
"select Id, Code, Name from ref_top_2 where forLastVerRef = 1");
    }
}