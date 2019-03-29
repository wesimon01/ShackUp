using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShackUp.Data.Interfaces;
using ShackUp.Models.Tables;
using System.Data;

namespace ShackUp.Data.ADO
{
    public class StatesRepositoryADO : IStatesRepository
    {
        private static string cxnStr = Settings.GetConnectionString();

        public List<State> StateGetAll()
        {
            var states = new List<State>();

            using (var cxn = new SqlConnection(cxnStr))            
            using (var cmd = new SqlCommand("StatesSelectAll", (SqlConnection)cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cxn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        states.Add(PopulateStateFromDataReader(dr));
                    }
                }
            }           
            return states;
        }

        private State PopulateStateFromDataReader(SqlDataReader dr)
        {
            var state = new State();
            state.StateId = dr["StateId"].ToString();
            state.StateName = dr["StateName"].ToString();

            return state;
        }
    }
}
