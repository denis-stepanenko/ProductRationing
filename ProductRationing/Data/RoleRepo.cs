using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class RoleRepo : RepoBase
    {
        public Role Get(int id) => conn.Query<Role>(
"select * from PRRoles where Id = @Id", 
new { Id = id }).Single();

        public IEnumerable<Role> GetAll() => conn.Query<Role>(
"select * from PRRoles");

        public int Add(Role role) => conn.ExecuteScalar<int>(
@"insert into PRRoles
(Name)
values
(@Name);
select scope_identity();", role);
         
        public void Update(Role role) => conn.Execute(
@"update PRRoles
set
Name = @Name
where Id = @Id", role);

        public void Remove(Role item) => conn.Execute(
"delete from PRRoles where Id = @Id", item);
    }
}
