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
    public class BathroomTypesRepositoryFactory
    {
        //Exmaple use of Factory Pattern

        public static IBathroomTypesRepository GetRepository()
        {
            string repoType = Settings.GetRepoType();

            switch (repoType.ToUpper())
            {
                case "ADO":
                    return new BathroomTypesRepositoryADO();
                case "DAPPER":
                    return new BathroomTypesRepositoryDapper();

                default:
                    throw new Exception("Could not find valid RepositoryType configuration value");

            }

        }
    }
}
