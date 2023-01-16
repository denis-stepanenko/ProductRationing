using Dapper;
using ProductRationing.DAL.Models;
using ProductRationing.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class OperationRepo : RepoBase
    {
        public Operation Get(int id) => conn.Query<Operation, Unit, Group, BigOperation, Profession, TechProcessType, Operation>(
@"select o.*, u.*, g.*, b.*, p.*, pt.* from PROperations o 
join PRUnits u on u.Id = o.UnitId join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 b on b.Id = o.BigOperationId
left join PRProfessions p on p.Id = o.ProfessionId
left join PRTechProcessTypes pt on pt.Id = o.TechProcessTypeId
where o.Id = @Id",
(o, u, g, b, p, pt) => { o.Unit = u; o.Group = g; o.BigOperation = b; o.Profession = p; o.TechProcessType = pt; return o; },
new { Id = id }).Single();

        public IEnumerable<Operation> GetAll() => conn.Query<Operation, Unit, Group, BigOperation, Profession, TechProcessType, Operation>(
@"select o.*, u.*, g.*, b.*, p.*, pt.* from PROperations o 
join PRUnits u on u.Id = o.UnitId join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 b on b.Id = o.BigOperationId
left join PRProfessions p on p.Id = o.ProfessionId
left join PRTechProcessTypes pt on pt.Id = o.TechProcessTypeId
order by o.Name",
(o, u, g, b, p, pt) => { o.Unit = u; o.Group = g; o.BigOperation = b; o.Profession = p; o.TechProcessType = pt; return o; });

        public IEnumerable<Operation> GetByDepartment(int department) => conn.Query<Operation, Unit, Group, BigOperation, Profession, TechProcessType, Operation>(
@"select o.*, u.*, g.*, b.*, p.*, pt.* from PROperations o 
join PRUnits u on u.Id = o.UnitId join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 b on b.Id = o.BigOperationId
left join PRProfessions p on p.Id = o.ProfessionId
left join PRTechProcessTypes pt on pt.Id = o.TechProcessTypeId
where o.Department = @Department
order by o.Name",
(o, u, g, b, p, pt) => { o.Unit = u; o.Group = g; o.BigOperation = b; o.Profession = p; o.TechProcessType = pt; return o; },
new { Department = department });


        public int Add(Operation operation) => conn.ExecuteScalar<int>(
@"declare @number int = (select isnull(max(convert(int, Code)), 0)+1 from PROperations);

insert into PROperations
(Code, Department, Name, Labor, Description, UnitId, GroupId, BigOperationId, 
Rank, ProfessionId, TechProcessTypeId, CodifierCode, CodifierName, CodifierGroupCode, CodifierGroupName, Description2, TechProOperationName, MaterialName)
values
(replicate('0', 6 - len(@number)) + cast(@number as varchar), 
@Department, @Name, @Labor, @Description, @UnitId, @GroupId, @BigOperationId,
@Rank, @ProfessionId, @TechProcessTypeId, @CodifierCode, @CodifierName, @CodifierGroupCode, @CodifierGroupName, @Description2, @TechProOperationName, @MaterialName);
select scope_identity();", operation);

        public bool IsOperationAlreadyUsedInSomeProducts(Operation item) => conn.ExecuteScalar<bool>(
@"select case when count(*) > 0 then 1 else 0 end from PRProductOperations where OperationId = @Id", item);

        public IEnumerable<string> GetProductsInWhichOperationIsUsed(Operation item) => conn.Query<string>(
"select distinct ProductCode from PRProductOperations where OperationId = @Id", item);
         
        public void Update(Operation operation) => conn.Execute(
@"update PROperations
set
Department = @Department,
Code = @Code,
Name = @Name,
Labor = @Labor,
Description = @Description,
UnitId = @UnitId,
GroupId = @GroupId,
BigOperationId = @BigOperationId,
Rank = @Rank,
ProfessionId = @ProfessionId,
TechProcessTypeId = @TechProcessTypeId,
CodifierCode = @CodifierCode,
CodifierName = @CodifierName,
CodifierGroupCode = @CodifierGroupCode,
CodifierGroupName = @CodifierGroupName,
Description2 = @Description2,
TechProOperationName = @TechProOperationName,
MaterialName = @MaterialName
where Id = @Id", operation);

        public void Remove(Operation item) => conn.Execute(
"delete from PROperations where Id = @Id", item);
    }
}