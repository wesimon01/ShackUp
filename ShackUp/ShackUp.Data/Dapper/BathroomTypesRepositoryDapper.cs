using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ShackUp.Data.Dapper
{
    public class BathroomTypesRepositoryDapper : IBathroomTypesRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public List<BathroomType> BathroomTypeGetAll()
        {
            var bathroomTypes = new List<BathroomType>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                bathroomTypes = cxn.Query<BathroomType>("BathroomTypesSelectAll", commandType: CommandType.StoredProcedure).ToList();
            } 
            return bathroomTypes;
        }
    }
}
