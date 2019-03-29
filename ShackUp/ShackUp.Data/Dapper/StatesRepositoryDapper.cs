using Dapper;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Tables;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ShackUp.Data.Dapper
{
    public class StatesRepositoryDapper : IStatesRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public List<State> StateGetAll()
        {
            var states = new List<State>();

            using (var cxn = new SqlConnection(cxnStr))
            {
                states = cxn.Query<State>("StatesSelectAll", commandType: CommandType.StoredProcedure).ToList();
            }
            return states;
        }
    }
}
