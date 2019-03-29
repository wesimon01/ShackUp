using ShackUp.Data.Interfaces;
using ShackUp.Models.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShackUp.Data.ADO
{
    public class BathroomTypesRepositoryADO : IBathroomTypesRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public List<BathroomType> BathroomTypeGetAll()
        {
            var bathroomTypes = new List<BathroomType>();

            using (var cxn = new SqlConnection(cxnStr))           
            using (var cmd = new SqlCommand("BathroomTypesSelectAll", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        bathroomTypes.Add(PopulateBathTypeFromDataReader(dr));
                    }
                }
            }           
            return bathroomTypes;
        }

        private BathroomType PopulateBathTypeFromDataReader(SqlDataReader dr)
        {
            var bathroomType = new BathroomType();
            bathroomType.BathroomTypeId = (int)dr["BathroomTypeId"];
            bathroomType.BathroomTypeName = dr["BathroomTypeName"].ToString();

            return bathroomType;
        }
    }
}
