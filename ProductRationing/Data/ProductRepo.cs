using Dapper;
using ProductRationing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProductRationing.DAL.Data
{
    public sealed class ProductRepo : RepoBase
    {
        public IEnumerable<Product> FindProducts(string code, string name) => conn.Query<Product>(
@"select top 500 * from
(
    select Decnum Code, Name from ref_dse
    union				  
    select DecNum Code, Name from ref_purchase
) p
where p.Code like '%' + @Code + '%' and p.Name like '%' + @Name + '%'",
new { Code = code, Name = name });

        public IEnumerable<Product> GetProducts() => conn.Query<Product>(
@"select Decnum Code, Name from ref_dse
union				  
select DecNum Code, Name from ref_purchase");

        public IEnumerable<ProductEntry> GetProductEntries(string productCode)
        {
            string tm = DateTime.Now.ToString("HHmmssffff");

            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute("i_CompoundByDec", new
                    {
                        Decnum = productCode,
                        IdentOpen = 1,
                        Tm = tm
                    }, commandType: CommandType.StoredProcedure, transaction: tran);

                    var result = conn.Query("s_CompByDecByDept3__",
                        new { Tm = tm, IdentOpen = 1, D = 0 }, commandType: CommandType.StoredProcedure, transaction: tran)
                        .Select(x => new ProductEntry
                        {
                            Code = x.DecNumWhat,
                            Name = x.Name,
                            Count = (int)x.Count,
                            Route = x.path
                        });

                    tran.Commit();
                    return result;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}