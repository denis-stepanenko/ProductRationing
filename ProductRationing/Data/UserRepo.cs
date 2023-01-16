using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class UserRepo : RepoBase
    {
        public User Get(int id) => conn.Query<User, Role, User>(
@"select *.u, *.r from PRUsers u  
join PRRoles r on r.Id = u.RoleId
where Id = @Id",
(u, r) => { u.Role = r; return u; },
new { Id = id }).Single();

        public IEnumerable<User> GetAll() => conn.Query<User, Role, User>(
@"select *.u, *.r from PRUsers u  
join PRRoles r on r.Id = u.RoleId",
(u, r) => { u.Role = r; return u; });

        public int Add(User user) => conn.ExecuteScalar<int>(
@"insert into PRUsers
(Name)
values
(@Name);
select scope_identity();", user);

        public void Update(User user) => conn.Execute(
@"update PRUsers
set
Name = @Name
where Id = @Id", user);

        public void Remove(User item) => conn.Execute(
"delete from PRUsers where Id = @Id", item);
    }
}