using Dapper;
using ProductRationing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class ProductOperationRepo : RepoBase
    {
        public ProductOperation Get(int id) => conn.Query<ProductOperation, Operation, Unit, Group, BigOperation, ProductOperation>(
@"select po.*, o.*, u.*, g.*, bo.* from PRProductOperations po 
join PROperations o on o.Id = po.OperationId
join PRUnits u on u.Id = o.UnitId
join PRGroups g on g.Id = o.GroupId 
left join ref_top_2 bo on bo.Id = o.BigOperationId
where po.Id = @Id",
(po, o, u, g, bo) => { o.Unit = u; o.Group = g; o.BigOperation = bo; po.Operation = o; return po; },
new { Id = id }).Single();

        public IEnumerable<ProductOperation> GetAll() => conn.Query<ProductOperation, Operation, Unit, Group, BigOperation, ProductOperation>(
@"select po.*, o.*, u.*, g.*, bo.* from PRProductOperations po 
join PROperations o on o.Id = po.OperationId
join PRUnits u on u.Id = o.UnitId
join PRGroups g on g.Id = o.GroupId
left join ref_top_2 bo on bo.Id = o.BigOperationId",
(po, o, u, g, bo) => { o.Unit = u; o.Group = g; o.BigOperation = bo; po.Operation = o; return po; });

        public IEnumerable<ProductOperation> GetAllByProductCode(string productCode) => conn.Query<ProductOperation, Operation, Unit, Group, BigOperation, ProductOperation>(
@"select po.*, o.*, u.*, g.*, bo.* from PRProductOperations po 
join PROperations o on o.Id = po.OperationId
join PRUnits u on u.Id = o.UnitId
join PRGroups g on g.Id = o.GroupId
left join ref_top_2 bo on bo.Id = o.BigOperationId
where po.ProductCode = @ProductCode
order by Number",
(po, o, u, g, bo) => { o.Unit = u; o.Group = g; o.BigOperation = bo; po.Operation = o; return po; },
new { ProductCode = productCode });

        public IEnumerable<ProductEntryOperation> GetAllForProductEntries(string productCode) => conn.Query<ProductEntryOperation>(
"GetOperationsForProductEntries", 
new { ProductCode = productCode, IsOpen = 1, Tm = DateTime.Now.ToString("HHmmssffff") },
commandType: CommandType.StoredProcedure);

        public IEnumerable<ProductEntryOperation> GetAllforProductEntriesWithoutEmptyProducts(string productCode) => conn.Query<ProductEntryOperation>(
@"declare @tmp table (Num int)

	insert into @tmp
	exec i_CompoundByDec @Decnum=@ProductCode, @IdentOpen=@IsOpen,@Tm=@Tm

	declare @products table (Code varchar(100), Name varchar(300), Type int, Count decimal(18, 3), Path varchar(300), Color varchar(100), FontColor varchar(100), BackColor varchar(100))

	insert into @products
	exec s_CompByDecByDept3__ @Tm=@Tm, @IdentOpen=@IsOpen, @D=0

	select * from
	(
		select 
		p.Code ProductCode, p.Name ProductName, p.Count ProductCount, p.Path ProductRoute, 
		po.Count OperationCount, 
		o.Department OperationDepartment, o.Code OperationCode, o.Name OperationName, o.Labor OperationLabor, o.Description OperationDescription,
		u.Name OperationUnitName, 
		g.Name OperationGroupName,
		bo.Code BigOperationCode,
		bo.Name BigOperationName
		from @products p
		join PRProductOperations po on po.ProductCode = p.Code
		left join PROperations o on o.Id = po.OperationId
		left join PRUnits u on u.Id = o.UnitId
		left join PRGroups g on g.Id = o.GroupId
		left join ref_top_2 bo on bo.Id = o.BigOperationId
	) r
	where 
	r.ProductRoute = cast(r.OperationDepartment as varchar) or
	r.ProductRoute like cast(r.OperationDepartment as varchar) + ' %' or
	r.ProductRoute like '% ' + cast(r.OperationDepartment as varchar) or
	r.ProductRoute like '% ' + cast(r.OperationDepartment as varchar) + ' %' or
	r.OperationDepartment is null", 
new { ProductCode = productCode, IsOpen = 1, Tm = DateTime.Now.ToString("HHmmssffff") });

		public IEnumerable<ProductEntry> GetEmptyProducts(string productCode, int? department) => conn.Query<ProductEntry>(
@"declare @tmp table (Num int)

	insert into @tmp
	exec i_CompoundByDec @Decnum=@ProductCode, @IdentOpen=@IsOpen,@Tm=@Tm

	declare @products table (Code varchar(100), Name varchar(300), Type int, Count decimal(18, 3), Path varchar(300), Color varchar(100), FontColor varchar(100), BackColor varchar(100))

	insert into @products
	exec s_CompByDecByDept3__ @Tm=@Tm, @IdentOpen=@IsOpen, @D=0
	
	select Code, Name, Count, Path Route from @products
	where
    ([Path] = cast(@Department as varchar) or
	[Path] like cast(@Department as varchar) + ' %' or
	[Path] like '% ' + cast(@Department as varchar) or
	[Path] like '% ' + cast(@Department as varchar) + ' %' or
    @Department is null
)    
    and Code not in 
    (
        select ProductCode from PRProductOperations po
        join PROperations o on o.Id = po.OperationId
        where o.Department = @Department or @Department is null
    )",
new { ProductCode = productCode, Department = department, IsOpen = 1, Tm = DateTime.Now.ToString("HHmmssffff") });

		public IEnumerable<ProductEntryOperation> GetAllForProductEntriesWithOnlyProductionDepartments(string productCode)
        {
            return GetAllForProductEntries(productCode).Where(x =>
            x.ProductRoute.Split(' ').Contains("4") || x.ProductRoute.Split(' ').Contains("5") || x.ProductRoute.Split(' ').Contains("6") || x.ProductRoute.Split(' ').Contains("13") ||
            x.ProductRoute.Split(' ').Contains("17") || x.ProductRoute.Split(' ').Contains("80") || x.ProductRoute.Split(' ').Contains("82") || string.IsNullOrEmpty(x.ProductRoute));
        }

        public IEnumerable<ProductOperation> Find(string operationCode) => conn.Query<ProductOperation, Operation, Unit, Group, BigOperation, ProductOperation>(
@"select po.*, o.*, u.*, g.*, bo.* from PRProductOperations po 
join PROperations o on o.Id = po.OperationId
join PRUnits u on u.Id = o.UnitId
join PRGroups g on g.Id = o.GroupId
left join ref_top_2 bo on bo.Id = o.BigOperationId
where o.Code = @Code",
(po, o, u, g, bo) => { o.Unit = u; o.Group = g; o.BigOperation = bo; po.Operation = o; return po; },
new { Code = operationCode });

        public int Add(ProductOperation productOperation) => conn.ExecuteScalar<int>(
@"insert into PRProductOperations
(ProductCode, OperationId, Count, Description, Number)
values
(@ProductCode, @OperationId, @Count, @Description, (select isnull(max(Number), 0) + 1 from PRProductOperations where ProductCode = @ProductCode));
select scope_identity();", productOperation);


        public int Update(ProductOperation productOperation) => conn.Execute(
@"update PRProductOperations
set
ProductCode = @ProductCode,
OperationId = @OperationId,
Count = @Count,
Description = @Description,
MachineTime = @MachineTime,
DifficultyGroup = @DifficultyGroup
where Id = @Id", productOperation);

        public void Swap(ProductOperation item1, ProductOperation item2) => conn.Execute(
@"if exists (select * from PRProductOperations where Id = @Id1 and Number = @Number1)
and exists (select * from PRProductOperations where Id = @Id2 and Number = @Number2)
update PRProductOperations set Number = @Number2 where Id = @Id1
update PRProductOperations set Number = @Number1 where Id = @Id2",
new
{
    Id1 = item1.Id,
    Id2 = item2.Id,
    Number1 = item1.Number,
    Number2 = item2.Number
});
        public void Remove(ProductOperation item) => conn.Execute(
"delete from PRProductOperations where Id = @Id", item);

    }
}