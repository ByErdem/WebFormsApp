using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Service.Concrete
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
