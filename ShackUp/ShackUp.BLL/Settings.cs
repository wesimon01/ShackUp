using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ShackUp.BLL
{
    public class Settings
    {
        private static string _serviceType;

        public static string GetServiceType()
        {
            if (string.IsNullOrEmpty(_serviceType))
                _serviceType = ConfigurationManager.AppSettings["ServiceType"].ToString();

            return _serviceType;
        }
    }
}
