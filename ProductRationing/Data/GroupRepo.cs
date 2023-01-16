using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class GroupRepo : RepoBase
    {
        public Group Get(int id) => conn.Query<Group>(
"select * from PRGroups where Id = @Id",
new { Id = id }).Single();

        public IEnumerable<Group> GetAll() => conn.Query<Group>(
"select * from PRGroups");

        public int Add(Group group) => conn.ExecuteScalar<int>(
@"insert into PRGroups
(Name)
values
(@Name);
select scope_identity();", group);

        public void Update(Group group) => conn.Execute(
@"update PRGroups
set
Name = @Name
where Id = @Id", group);

        public void Remove(Group item) => conn.Execute(
"delete from PRGroups where Id = @Id", item);
    }
}
