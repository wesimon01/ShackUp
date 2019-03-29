using ShackUp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;

namespace ShackUp.Data.Factory
{
    public class ListingRepositoryFactory
    {
        //Exmaple use of Factory Pattern

        public static IListingsRepository GetRepository()
        {
            string repoType = Settings.GetRepoType();

            switch (repoType.ToUpper())
            {
                case "ADO":
                    return new ListingsRepositoryADO();
                case "DAPPER":
                    return new ListingsRepositoryDapper();

                default:
                    throw new Exception("Could not find valid RepositoryType configuration value");

            }

        }
    }
}
