using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.Factory
{
    public class StatesRepositoryFactory
    {
        //Exmaple use of Factory Pattern

        public static IStatesRepository GetRepository()
        {
            string repoType = Settings.GetRepoType();

            switch (repoType.ToUpper())
            {
                case "ADO":
                    return new StatesRepositoryADO();
                case "DAPPER":
                    return new StatesRepositoryDapper();

                default:
                    throw new Exception("Could not find valid RepositoryType configuration value");

            }

        }
    }
}
