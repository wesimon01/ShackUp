using ShackUp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;

namespace ShackUp.BLL
{
    public class ShackUpServiceFactory
    {
        //Exmaple use of Factory Pattern

        public static IShackUpService GetService()
        {
            string serviceType = Settings.GetServiceType();

            switch (serviceType.ToUpper())
            {
                case "SERVICE":
                    return new ShackUpService();
                
                default:
                    throw new Exception("Could not find valid ServiceType configuration value");

            }

        }
    }
}
