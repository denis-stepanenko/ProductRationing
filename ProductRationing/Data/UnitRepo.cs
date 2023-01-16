using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class UnitRepo : RepoBase
    {
        public Unit Get(int id) => conn.Query<Unit>(
"select * from PRUnits where Id = @Id", new { Id = id }).Single();
         

        public IEnumerable<Unit> GetAll() => conn.Query<Unit>(
"select * from PRUnits");

        public int Add(Unit unit) => conn.ExecuteScalar<int>(
@"insert into PRUnits
(Name)
values
(@Name);
select scope_identity();", unit);

        public void Update(Unit unit) => conn.Execute(
@"update PRUnits
set
Name = @Name
where Id = @Id", unit);

        public void Remove(Unit item) => conn.Execute(
"delete from PRUnits where Id = @Id", item);
    }
}
