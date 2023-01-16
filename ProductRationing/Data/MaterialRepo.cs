using Dapper;
using ProductRationing.Models;
using System.Collections.Generic;

namespace ProductRationing.DAL.Data
{
    public sealed class MaterialRepo : RepoBase
    {
        public IEnumerable<Material> GetAll() => conn.Query<Material>(
@"select MaterialId Id, Code, Name, Size, Type, Price, UnitId 
from tMaterial");
    }
}