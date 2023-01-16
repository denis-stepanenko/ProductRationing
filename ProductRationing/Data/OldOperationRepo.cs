using Dapper;
using ProductRationing.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class OldOperationRepo : RepoBase
    {
        public Operation Get(int id) => conn.Query<Operation, Unit, Group, BigOperation, Operation>(
@"select o.*, u.*, g.*, b.* from PROldOperations o 
join PRUnits u on u.Id = o.UnitId 
join PRGroups g on g.Id = o.GroupId left join ref_top_2 b on b.Id = o.BigOperationId where o.Id = @Id",
(o, u, g, b) => { o.Unit = u; o.Group = g; o.BigOperation = b; return o; },
new { Id = id }).Single();

        public IEnumerable<Operation> GetAll() => conn.Query<Operation, Unit, Group, BigOperation, Operation>(
@"select o.*, u.*, g.*, b.* from PROldOperations o 
join PRUnits u on u.Id = o.UnitId join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 b on b.Id = o.BigOperationId
order by o.Name",
(o, u, g, b) => { o.Unit = u; o.Group = g; o.BigOperation = b; return o; });

        public IEnumerable<Operation> GetByDepartment(int department) => conn.Query<Operation, Unit, Group, BigOperation, Operation>(
@"select o.*, u.*, g.*, b.* from PROldOperations o 
join PRUnits u on u.Id = o.UnitId 
join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 b on b.Id = o.BigOperationId where o.Department = @Department
order by o.Name",
(o, u, g, b) => { o.Unit = u; o.Group = g; o.BigOperation = b; return o; },
new { Department = department });
        
    }
}